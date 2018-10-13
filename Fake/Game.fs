module Game

let width = 6
let height = 5

type StepCount = int
type Position = { x: int; y: int}
type Snake = {head : Position; tail : Position list} 
type Game = { steps: StepCount; snake: Snake }

