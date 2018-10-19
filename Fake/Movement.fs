module Movement

open Game
open GameUtil
open Util

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
            

let computeNextPosition position direction = 
    let {x=x;y=y} = position
    match direction with
    | Up _ -> {x = x; y = y-1}
    | Right _ -> {x = x+1; y = y}
    | Down _-> {x = x; y = y+1}
    | Left _ -> {x = x-1; y = y}

let computeNewHead head =
    let {direction = direction; headPosition = position} = head
    let newPosition = computeNextPosition position direction
    {headPosition = newPosition; direction = direction}



let computeNewBody snake =
    let {head=head; body = body;stomach=stomach} = snake
    let {headPosition = headPosition} = head
    let fullSnake = {position=headPosition}::body
    match stomach with 
        | Stomach.Empty ->    fullSnake |> removeLastElement
        | Full -> fullSnake        
 
let takeNewIfValid oldState newState =
    match computeHeadValidity newState with
        | Valid _ ->   
            Running newState
        | Invalid invalidReason ->
            let reason =
                match invalidReason with 
                | CollisionWithWall _ -> InvalidPosition InvalidPosition.CollisionWithWall
                | CollisionWithBody _ -> InvalidPosition InvalidPosition.CollisionWithBody
            Finished {state = oldState; reason = reason}

let moveSnakeAndGrowIfStomachFull game  =      
    match game with
        | Finished _ -> game
        | Running oldState ->
            let {size= size;steps=steps; snake=lastSnake;foodOption = foodOption; points = points} = oldState
            let {head = head; stomach = stomach} = lastSnake
            let newHead = head |> computeNewHead 
            let newBody = lastSnake |> computeNewBody
            let newSnake= {head=newHead; body=newBody; stomach = stomach}
            let newState = {size=size;steps = steps ; snake=newSnake; foodOption = foodOption;points = points}
            newState |> (takeNewIfValid oldState)

                     
                    
     