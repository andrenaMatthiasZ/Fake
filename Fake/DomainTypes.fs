﻿module Game

type Steps = StepCount of int

type Position = { x: int; y: int}
type Direction = Down | Up | Left | Right
type SnakeSegment = {position: Position}
type SnakeHead = {headPosition: Position; direction: Direction}
type Stomach = Full | Empty
type Snake = {head : SnakeHead; body : SnakeSegment list; stomach: Stomach}

type Size = {width: int; height: int}

type Food = {foodPosition: Position}
type FoodOption = Option<Food>

type InvalidPosition =
    | CollisionWithWall 
    | CollisionWithBody

type Reason = 
    | EscapePressed 
    | InvalidPosition of InvalidPosition


type Points = Points of int

type GameState = 
    {size: Size; steps: Steps; snake: Snake; foodOption: FoodOption; points: Points}

type FinishedGame = 
    {state : GameState;reason: Reason}
type Game =
    | Running of GameState
    | Finished of FinishedGame

let initialState = 
    let startPosition = { x=5; y=3}
    let firstSegment = {position = {x=4;y=3}}
    let secondSegment = {position = {x=4;y=2}}
    let startHead = {headPosition = startPosition; direction = Right}
    let startSnake = {head= startHead ;body = firstSegment::secondSegment::[]; stomach = Empty}
    let size = {width= 8; height = 7}
    {size = size; steps = StepCount 1; snake = startSnake; foodOption = Some {foodPosition = {x=6;y=5} }; points = Points 0} 
    
let initialGame = Running(initialState)

