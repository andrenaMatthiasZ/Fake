module Game

type StepCount = int
type Position = { x: int; y: int}
type Direction = Down | Up | Left | Right
type SnakeSegment = {position: Position; direction: Direction}
type Snake = {head : SnakeSegment; tail : SnakeSegment list}
type Size = {width: int; height: int}
type Reason = EscapePressed
type State =
    | Running
    | Abborted of Reason

type GameState = {size: Size; steps: StepCount; snake: Snake}

type FinishedGame = {state : GameState;reason: Reason}
type Game =
    | Running of GameState
    | Finished of FinishedGame

let initialState = 
    let startPosition = { x=3; y=3}
    let startHead = {position = startPosition; direction = Right}
    let startSnake = {head= startHead ;tail=[]}
    let size = {width= 8; height = 7}
    {size = size; steps = 1; snake = startSnake} 
    
let initialGame = Running(initialState)


let checkIfPositionInWall gameState position =
    let {size = size} = gameState
    let {width = width; height= height} = size
    let {x=x;y=y} = position
    x=1 || y=1 || x = width || y = height