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
            let  {size = size; steps=steps;snake=snake; foodOption = foodOption; points = points} =state
            let {head=head;body=body; stomach = stomach} = snake
            let {headPosition = position} = head;
            let newhead = {headPosition=position; direction=direction}
            let newSnake = {head=newhead;body=body;stomach=stomach}
            Running {size=size;steps=steps;snake=newSnake; foodOption = foodOption; points=points}


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
            let {size=size; steps = steps; snake = snake; foodOption = foodOption; points=points} = state
            match foodOption with 
                | FoodOption.Some food -> 
                    let {head = {headPosition=headPosition}} = snake
                    if checkIfIsFood foodOption headPosition then   
                        Running {size=size; snake=snake;steps=steps; foodOption = FoodOption.None; points=points}
                    else
                        Running state
                | FoodOption.None -> Running state


let increaseStepCount game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=lastStep; snake=snake; foodOption = foodOption;points=points} = state
            let (StepCount lastStepCount) = lastStep 
            let nextStep = lastStepCount + 1 |> StepCount
            Running {size = size; steps = nextStep;snake= snake; foodOption = foodOption; points=points}

let fillStomach game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body};foodOption=foodOption; points=points} = state
            match foodOption with
                | FoodOption.None -> game
                | FoodOption.Some {foodPosition=foodPosition} ->
                    let nextPosition = computeNewPosition headPosition direction
                    if nextPosition = foodPosition then
                        Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Full}; points = points;foodOption=foodOption}
                    else 
                        game
                        
let increasePoints game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size=size;steps=steps;snake={head=head;body=body;stomach=stomach};foodOption=foodOption; points=points} = state
            match stomach with
                | Stomach.Empty -> game
                | Full ->
                        let (Points pointsValue) = points
                        Running {size=size;steps=steps;snake={head=head;body=body;stomach=stomach}; points = Points (pointsValue + 100);foodOption=foodOption}

let emptyStomach game = 
    match game with 
        | Finished _ -> game 
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;};foodOption=foodOption; points=points} = state
            Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Stomach.Empty};points=points;foodOption=foodOption}

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
            |> increasePoints
            |> emptyStomach
            |> removeFood 
            |> increaseStepCount 
            |> doNextStep keyPressedProvider
