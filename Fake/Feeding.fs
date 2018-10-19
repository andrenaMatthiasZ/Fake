module Feeding

open Game
open Movement
open System
open System.Drawing

let fillStomachIfFoodInFrontOfSnakeHead game =
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



let removeFoodIfEaten game = 
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

let createListOfAllPositions size =
    let {width=width;height=height} =  size
    let listList = 
        [for x in [1..width] ->
            [for y in [1..height] ->
                {x=x;y=y}
            ]
        ]
    listList |> List.concat 

let toPosition snakeSegment = 
    let {position=position} = snakeSegment
    position

let isInSnake snake position = 
       let {head={headPosition=headPosition};body=body} = snake
       let snakePositions = {position=headPosition}::body |> List.map toPosition
       snakePositions |> List.contains position

let removeSnakePositions snake positions =
    let filter = (isInSnake snake) >> not
    positions |> List.filter filter

let removeWall size positions=
    let filter  = checkIfPositionInWall size >> not
    positions |> List.filter filter

let getRandomElement list = 
    let random = new Random()
    let length = list |> List.length
    let randomIndex = random.Next(1,length+1)
    list |> List.item randomIndex

let getRandomFoodPosition state = 
    let {size=size;snake = snake} = state
    let positions = createListOfAllPositions size
    let possiblePositionsForFood = positions |> removeSnakePositions snake |> removeWall size 
    possiblePositionsForFood |> getRandomElement
   

let addFoodIfMissing game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {foodOption = foodOption} = state
            match foodOption with 
                | FoodOption.Some _ -> Running state
                | FoodOption.None -> 
                    let addFood state = 
                        let {size=size; snake = snake; points = points;steps=steps} = state;
                        let foodPosition = getRandomFoodPosition state
                        {size=size; steps=steps;snake = snake; points = points; foodOption = Some {foodPosition = foodPosition}}
                    state |> addFood |> Running

let emptyStomach game = 
    match game with 
        | Finished _ -> game 
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;};foodOption=foodOption; points=points} = state
            Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Stomach.Empty};points=points;foodOption=foodOption}
