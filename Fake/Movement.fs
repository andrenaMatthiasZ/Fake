module Movement

open Game

let directionsAreInverse firstDirection secondDirection =
    match firstDirection with
        | Up -> secondDirection = Down
        | Down -> secondDirection = Up
        | Left -> secondDirection = Right
        | Right -> secondDirection = Left

let changeHeadDirectionTo direction game =
    match game with
        | Finished _ -> game 
        | Running state ->
            let  {size = size; steps=steps;snake=snake; foodOption = foodOption; points = points} =state
            let {head=head;body=body; stomach = stomach} = snake
            let {headPosition = position} = head;
            let newhead = {headPosition=position; direction=direction}
            let newSnake = {head=newhead;body=body;stomach=stomach}
            Running {size=size;steps=steps;snake=newSnake; foodOption = foodOption; points=points}

let changeHeadDirectionIfNotInversseToOldDirection oldDirection newDirection= 
                        if directionsAreInverse newDirection oldDirection then
                            fun(game) -> game
                        else
                            changeHeadDirectionTo newDirection
            
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
    let {head=head; body = body;stomach=stomach} = snake
    let {headPosition = headPosition} = head
    let fullSnake = {position=headPosition}::body
    match stomach with 
        | Empty ->    fullSnake |> removeLastElement
        | Full -> fullSnake        

let moveSnakeAndGrowIfStomachFull game  =      
    match game with
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=steps; snake=lastSnake;foodOption = foodOption; points = points} = state
            let {head = head; stomach = stomach} = lastSnake
            let newHead = head |> computeNewHead 
            let newBody = lastSnake |> computeNewBody
            let newSnake= {head=newHead; body=newBody; stomach = stomach}
            let newState = {size=size;steps = steps ; snake=newSnake; foodOption = foodOption;points = points}

            match computeHeadValidity newState with
                | Valid _ ->   
                    Running newState
                | Invalid invalidReason -> 
                    let reason =
                        match invalidReason with 
                        | CollisionWithWall _ -> Reason.CollisionWithWall
                        | CollisionWithBody _ -> Reason.CollisionWithBody
                    Finished {state = state; reason = reason}
                     
                    
     