module Feeding

open Game
open Movement
open GameUtil
open Util

let fillStomachIfFoodInFrontOfSnakeHead game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body};foodOption=foodOption; points=points} = state
            match foodOption with
                | FoodOption.None -> game
                | FoodOption.Some {foodPosition=foodPosition} ->
                    let nextPosition = computeNextPosition headPosition direction
                    if nextPosition = foodPosition then
                        Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Full}; points = points;foodOption=foodOption}
                    else 
                        game

let removeFoodIfEaten game = 
    match game with
        | Finished _-> game
        | Running state ->
            let {size=size; steps = steps; snake = snake; foodOption = foodOption; points=points} = state
            match foodOption with 
                | FoodOption.Some _ -> 
                    let {head = {headPosition=headPosition}} = snake
                    if checkIfIsFood foodOption headPosition then   
                        Running {size=size; snake=snake;steps=steps; foodOption = FoodOption.None; points=points}
                    else
                        Running state
                | FoodOption.None -> Running state



let isInSnake snake position = 
       let {head={headPosition=headPosition};body=body} = snake
       let snakePositions = {position=headPosition}::body |> List.map toPosition
       snakePositions |> List.contains position

let removeSnakePositions snake positions =
    let filter = isInSnake snake >> not
    positions |> List.filter filter

let removeWallPositions size positions=
    let filter  = checkIfPositionInWall size >> not
    positions |> List.filter filter



let getRandomFoodPosition state = 
    let {size=size;snake = snake} = state
    let positions = createListOfAllPositions size
    positions |> removeSnakePositions snake |> removeWallPositions size |> getRandomElement

let addFood state = 
    let {size=size; snake = snake; points = points;steps=steps} = state;
    let foodPosition = getRandomFoodPosition state
    {size=size; steps=steps;snake = snake; points = points; foodOption = Some {foodPosition = foodPosition}}
   
let getFoodAdding foodOption =
    match foodOption with 
        | FoodOption.Some _ -> fun(state)->state
        | FoodOption.None -> addFood 

let addFoodIfMissing game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {foodOption = foodOption} = state
            state |> getFoodAdding foodOption |> Running

let emptyStomach game = 
    match game with 
        | Finished _ -> game 
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;};foodOption=foodOption; points=points} = state
            Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Stomach.Empty};points=points;foodOption=foodOption}
