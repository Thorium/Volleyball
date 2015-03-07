namespace MainScene
open UnityEngine

type PlayerKeys =
| Player1 = 0
| Player2 = 1
//| Computer

type PlayerControl() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let mutable side = Unchecked.defaultof<PlayerKeys>

    member i.MoveLeft () = i.GetComponent<Rigidbody>().AddForce(-1500.f, 0.f, 0.f)
    member i.MoveRight () = i.GetComponent<Rigidbody>().AddForce(1500.f, 0.f, 0.f)
    member i.Jump y = 
        if y > float32 Screen.height / 2.f && i.transform.position.y < 0.9f then
            i.GetComponent<Rigidbody>().AddForce(0.f, 10000.f, 0.f)

    member i.Move half current = 
        if current < half then i.MoveLeft()
        elif current > half then i.MoveRight()
        else ()

    member i.Update () = 
        let touches = Input.touches

        match side with
        | PlayerKeys.Player2 ->  
            touches |> Seq.iter(fun t -> 
                match t.position.x > (float32 Screen.width) / 2.f with
                | true ->
                    i.Jump t.position.y
                    i.Move (float32 Screen.width * 3.f / 4.f) t.position.x
                | false -> ())

            if Input.GetKey KeyCode.A then i.MoveLeft()
            if Input.GetKey KeyCode.D then i.MoveRight()
            if Input.GetKey KeyCode.W then i.Jump (float32 Screen.height)

        | PlayerKeys.Player1 ->  
            touches |> Seq.iter(fun t -> 
                match t.position.x < (float32 Screen.width) / 2.f with
                | true ->
                    i.Jump t.position.y
                    i.Move (float32 Screen.width / 4.f) t.position.x
                | false -> ())

            if Input.GetKey KeyCode.LeftArrow then i.MoveLeft()
            if Input.GetKey KeyCode.RightArrow then i.MoveRight()
            if Input.GetKey KeyCode.UpArrow then i.Jump (float32 Screen.height)
        | _ -> ()

        if Input.GetKey (KeyCode.Escape) then Application.Quit()