module Steps

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
            let {head=head;tail=tail} = snake
            let {position = position} = head;
            let newhead = {position=position; direction=direction}
            let newSnake = {head=newhead;tail=tail}
            Running {size=size;steps=steps;snake=newSnake}

let moveSnake game  =      
    match game with
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=steps; snake=lastSnake} = state
            let {head = head; tail=tail} = lastSnake
            let {direction = direction; position = position} = head
            let newPosition = computeNewPosition position direction
            let newHead = {position = newPosition; direction = direction}
            let newSnake = {head=newHead; tail=tail}
            Running {size=size;steps = steps ;snake=newSnake}
           

let rec doNextStep keyPressedProvider game = 
    match game with 
        | Finished finishedGame -> finishedGame
        | Running state ->
            drawGame state
            Thread.Sleep(900)
            (clearInput())
            Thread.Sleep(100)
            let {size= size;steps=lastStep; snake=lastSnake} = state
            let nextStep = lastStep + 1
            Console.WriteLine("next step: {0}",nextStep)

            let keyPressed = getKeyPressed keyPressedProvider
            match keyPressed with
            | KnownKey knownKey -> 
                match knownKey with
                | Esc _ ->
                    let state = {size=size;steps = nextStep;snake=lastSnake}
                    let game = {state= state; reason=EscapePressed}
                    doNextStep keyPressedProvider (Finished(game))

                | Arrow direction-> 
                    Console.WriteLine(direction)
                    let changeHeadDirection = changeHeadDirectionTo direction
                    Running(state) |> changeHeadDirection |> moveSnake |> doNextStep keyPressedProvider
            | OptionalKey.None _ ->
                Console.WriteLine("Other or none")
                Running(state) |> moveSnake |> doNextStep keyPressedProvider
