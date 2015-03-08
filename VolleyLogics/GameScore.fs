namespace MainScene

open UnityEngine

type PlayerTurn =
| Initial
| PlayerLeft
| PlayerRight

type GameScore() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let mutable playerLeftScore = Unchecked.defaultof<GUIText>
    [<SerializeField>]
    let mutable playerRightScore = Unchecked.defaultof<GUIText>

    let mutable startTime=0.f
    let mutable playerTurn = PlayerTurn.Initial
    let mutable plPoints = 0
    let mutable prPoints = 0

    member i.nextBall() =

        let nextStarter =
            match playerTurn, i.transform.position.x > 0.f with
            | Initial, true  -> PlayerTurn.PlayerLeft
            | Initial, false -> PlayerTurn.PlayerRight
            | PlayerRight, true  -> PlayerTurn.PlayerLeft
            | PlayerLeft, false -> PlayerTurn.PlayerRight
            | PlayerLeft, true  ->
                //MonoBehaviour.print "p1 scored!"
                plPoints<-plPoints+1
                PlayerTurn.PlayerLeft
            | PlayerRight, false ->
                //MonoBehaviour.print "p2 scored!"
                prPoints<-prPoints+1
                PlayerTurn.PlayerRight

        let x = 
            match nextStarter with 
            | PlayerLeft -> -3.f 
            | PlayerRight -> 3.f
            | Initial -> failwith "Won't happen"

        playerTurn <- nextStarter
        i.transform.position <- Vector3 (x, 4.f, 0.f)

        playerLeftScore.text <- plPoints.ToString()
        playerRightScore.text <- prPoints.ToString()
        let r = i.GetComponent<Rigidbody>()
        r.isKinematic <- true
        r.Sleep ()
        r.isKinematic <- false
        r.useGravity <- false
        startTime <- Time.time + 3.0f

    member i.Start () =
        let x = match Random.value > 0.5f with | true -> -3.f | false -> 3.f
        i.transform.position <- Vector3 (x, 4.f, 0.f)

    member i.Update () =
        let r = i.GetComponent<Rigidbody>()
        if not(r.useGravity) && Time.time > startTime then r.useGravity <- true
        if i.transform.position.y <= 0.4f then i.nextBall()