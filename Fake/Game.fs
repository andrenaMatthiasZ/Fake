module Game

type StepCount = int
type Position = { x: int; y: int}
type Direction = Down | Up | Left | Right
type SnakeSegment = {position: Position; direction: Direction}
type Snake = {head : SnakeSegment; tail : SnakeSegment list}
type Size = {width: int; height: int}
type gameState = {size: Size; steps: StepCount; snake: Snake }

