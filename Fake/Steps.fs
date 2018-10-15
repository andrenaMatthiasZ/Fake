﻿module Steps

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open System.Threading
open ReadKey
open Game
open Drawing  

type SomeOrNoneDirection =
    | Some of Direction
    | None



let computeNewPosition position direction = 
    let {x=x;y=y} = position
    match direction with
    | Up _ -> {x = x; y = y-1}
    | Right _ -> {x = x+1; y = y}
    | Down _-> {x = x; y = y+1}
    | Left _ -> {x = x-1; y = y}

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
            
type ReasonForInvalidPosition = 
    | CollisionWithWall
type PositionValidity =
    | Valid
    | Invalid of ReasonForInvalidPosition

let computeHeadValidity state = 
                let {snake={head={headPosition=position}}} = state
                let positionIsInWall = checkIfPositionInWall state position
                if positionIsInWall then 
                    Invalid(CollisionWithWall)
                else 
                    Valid

let computeNewHead head =
    let {direction = direction; headPosition = position} = head
    let newPosition = computeNewPosition position direction
    {headPosition = newPosition; direction = direction}

let moveSnake game  =      
    match game with
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=steps; snake=lastSnake} = state
            let {head = head; body=tail} = lastSnake
            let newHead = head |> computeNewHead 
            let newSnake= {head=newHead; body=tail}
            let newState = {size=size;steps = steps ;snake=newSnake}

            match computeHeadValidity newState with
                | Valid _ ->   
                    Running newState
                | Invalid _ -> 
                    Finished {state = state; reason = Reason.CollisionWithWall}
                    
          
           

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
