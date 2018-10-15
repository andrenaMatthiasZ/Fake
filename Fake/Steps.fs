module Steps

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open System.Threading
open ReadKey
open Game
open Drawing  
open Movement

type SomeOrNoneDirection =
    | Some of Direction
    | None




let changeHeadDirectionTo direction game =
    match game with
        | Finished _ -> game 
        | Running state ->
            let  {size = size; steps=steps;snake=snake} =state
            let {head=head;body=tail} = snake
            let {headPosition = position; direction = _} = head;
            let newhead = {headPosition=position; direction=direction}
            let newSnake = {head=newhead;body=tail}
            Running {size=size;steps=steps;snake=newSnake}


let consumeKeyPressed keyPressed game =
    match game with 
        | Finished _ -> game
        | Running state -> 
            match keyPressed with
            | KnownKey knownKey -> 
                match knownKey with
                | Esc _ ->
                    let game = {state= state; reason=EscapePressed}
                    Finished game
                | Arrow direction-> 
                    let changeHeadDirection = changeHeadDirectionTo direction
                    Running(state) |> changeHeadDirection
            | OptionalKey.None _ ->
                Running state  

     
           

let increaseStepCount game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=lastStep; snake=snake} = state
            Running {size = size; steps = lastStep+1;snake= snake}


let rec doNextStep keyPressedProvider game = 
    match game with 
        | Finished finishedGame -> finishedGame
        | Running state ->
            drawGame state
            clearInputWithDelay()
            let changeDirection = keyPressedProvider |> getKeyPressed |> consumeKeyPressed
            Running(state) |> changeDirection |> moveSnake |> increaseStepCount |> doNextStep keyPressedProvider
