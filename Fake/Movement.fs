module Movement

open Game

            
type ReasonForInvalidPosition = 
    | CollisionWithWall
    | CollisionWithBody
type PositionValidity =
    | Valid
    | Invalid of ReasonForInvalidPosition

let computeNewPosition position direction = 
    let {x=x;y=y} = position
    match direction with
    | Up _ -> {x = x; y = y-1}
    | Right _ -> {x = x+1; y = y}
    | Down _-> {x = x; y = y+1}
    | Left _ -> {x = x-1; y = y}
    

let computeHeadValidity state = 
                let {snake={head={headPosition=headPosition}}} = state
                let isInWall = checkIfPositionInWall state
                let isInBody = checkIfPositionIsInBody state
                if isInWall headPosition then 
                    Invalid CollisionWithWall
                else if isInBody headPosition then
                    Invalid CollisionWithBody
                else
                    Valid


let computeNewHead head =
    let {direction = direction; headPosition = position} = head
    let newPosition = computeNewPosition position direction
    {headPosition = newPosition; direction = direction}

let removeLastElement list =
    list |> List.rev |> List.tail |> List.rev

let computeNewBody snake =
    let {head=head; body = body} = snake
    let {headPosition = headPosition} = head
    let fullSnake = {position=headPosition}::body
    fullSnake |> removeLastElement
        

let moveSnake game  =      
    match game with
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=steps; snake=lastSnake} = state
            let {head = head; body=tail} = lastSnake
            let newHead = head |> computeNewHead 
            let newBody = lastSnake |> computeNewBody
            let newSnake= {head=newHead; body=newBody}
            let newState = {size=size;steps = steps ;snake=newSnake}

            match computeHeadValidity newState with
                | Valid _ ->   
                    Running newState
                | Invalid invalidReason -> 
                    let reason =
                        match invalidReason with 
                        | CollisionWithWall _ -> Reason.CollisionWithWall
                        | CollisionWithBody _ -> Reason.CollisionWithBody
                    Finished {state = state; reason = reason}
                     
                    
     