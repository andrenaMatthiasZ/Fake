module Game

type StepCount = int
type Position = { x: int; y: int}
type Direction = Down | Up | Left | Right
type SnakeSegment = {position: Position}
type SnakeHead = {headPosition: Position; direction: Direction}
type Snake = {head : SnakeHead; body : SnakeSegment list}
type Size = {width: int; height: int}
type Reason = EscapePressed | CollisionWithWall | CollisionWithBody
type State =
    | Running
    | Abborted of Reason

type GameState = {size: Size; steps: StepCount; snake: Snake}

type FinishedGame = {state : GameState;reason: Reason}
type Game =
    | Running of GameState
    | Finished of FinishedGame

let initialState = 
    let startPosition = { x=5; y=3}
    let startHead = {headPosition = startPosition; direction = Right}
    let firstSegment = {position = {x=4;y=3}}
    let secondSegment = {position = {x=4;y=2}}
    let startSnake = {head= startHead ;body = firstSegment::secondSegment::[]}
    let size = {width= 8; height = 7}
    {size = size; steps = 1; snake = startSnake} 
    
let initialGame = Running(initialState)


let checkIfPositionInWall gameState position =
    let {size = size} = gameState
    let {width = width; height= height} = size
    let {x=x;y=y} = position
    x=1 || y=1 || x = width || y = height

let hasSamePosition position segment =
                    let {position = segmentPosition}= segment
                    position = segmentPosition

let checkIfPositionIsInBody gameState position =
    let {snake = {body=body}} = gameState
    let positionExists = position |> hasSamePosition |> List.exists
    body |> positionExists

