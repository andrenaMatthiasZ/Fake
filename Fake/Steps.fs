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
            let  {size = size; steps=steps;snake=snake; foodOption = foodOption} =state
            let {head=head;body=body; stomach = stomach} = snake
            let {headPosition = position} = head;
            let newhead = {headPosition=position; direction=direction}
            let newSnake = {head=newhead;body=body;stomach=stomach}
            Running {size=size;steps=steps;snake=newSnake; foodOption = foodOption}


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
                    let  {snake = {head={direction = oldDirection}}} = state
                    if directionsAreInverse direction oldDirection then
                        Running state
                    else
                        let changeHeadDirection = changeHeadDirectionTo direction
                        Running state |> changeHeadDirection
            | OptionalKey.None _ ->
                Running state  

     
let removeFood game = 
    match game with
        | Finished _-> game
        | Running state ->
            let {size=size; steps = steps; snake = snake; foodOption = foodOption} = state
            match foodOption with 
                | FoodOption.Some food -> 
                    let {head = {headPosition=headPosition}} = snake
                    if checkIfIsFood foodOption headPosition then   
                        Running {size=size; snake=snake;steps=steps; foodOption = FoodOption.None}
                    else
                        Running state
                | FoodOption.None -> Running state


let increaseStepCount game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=lastStep; snake=snake; foodOption = foodOption} = state
            let (StepCount lastStepCount) = lastStep 
            let nextStep = lastStepCount + 1 |> StepCount
            Running {size = size; steps = nextStep;snake= snake; foodOption = foodOption}

let fillStomach game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=stomach};foodOption=foodOption} = state
            match foodOption with
                | FoodOption.None -> game
                | FoodOption.Some {foodPosition=foodPosition} ->
                    let nextPosition = computeNewPosition headPosition direction
                    if nextPosition = foodPosition then
                        Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Full};foodOption=foodOption}
                    else 
                        game

let emptyStomach game = 
    match game with 
        | Finished _ -> game 
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;};foodOption=foodOption} = state
            Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Stomach.Empty};foodOption=foodOption}

let rec doNextStep keyPressedProvider game = 
    match game with 
        | Finished finishedGame -> finishedGame
        | Running state ->
            drawGame state
            clearInputWithDelay()
            let changeDirection = keyPressedProvider |> getKeyPressed |> consumeKeyPressed
            Running(state) 
            |> changeDirection 
            |> fillStomach
            |> moveAndGrowSnake 
            |> emptyStomach
            |> removeFood 
            |> increaseStepCount 
            |> doNextStep keyPressedProvider
