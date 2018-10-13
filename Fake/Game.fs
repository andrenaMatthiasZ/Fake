module Game



type StepCount = int
type Position = { x: int; y: int}
type Snake = Position list
type Game = { steps: StepCount; snake: Snake }

