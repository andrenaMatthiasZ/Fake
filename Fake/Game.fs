module Game

type StepCount = int
type Position = { x: int; y: int}
type Snake = {head : Position; tail : Position list}
type Size = {width: int; height: int}
type Game = {size: Size; steps: StepCount; snake: Snake }

