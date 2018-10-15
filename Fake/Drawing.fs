module Drawing

open Game
open System
open System.Drawing

type Symbol =
    | Wall
    | Empty
    | SnakeHead 


type Field = Symbol*Position

let createGameBoard gameState =
        let {snake = snake; size = size} = gameState
        let {width = width; height= height} = size
        let xPositions =  [ 1..width ]
        let yPositions = [ 1..height]
        [
        for y in yPositions ->
            [ 
                for x in xPositions ->

                let symbol = 
                    let position = {x=x; y=y}
                    let positionIsWall = position |> checkIfPositionInWall gameState
                    if positionIsWall then
                        Wall
                    else 
                    let {head=head} = snake
                    let {position = headPosition} = head;
                    if headPosition = position then
                        SnakeHead
                    else
                        Empty

                ({x=x;y=y},symbol)
            ]
        ]

let drawBoard gameBoard =
    for line in gameBoard do
        for field in line do
            let character = 
                let (_,symbol) = field
                match symbol with
                | Wall _ -> '#'
                | Empty _ -> ' '
                | SnakeHead _ -> 'o'
            Console.Write character
        Console.WriteLine "" 

let drawHeader gameState =
    let {steps = steps} = gameState
    Console.WriteLine "" 
    Console.WriteLine "" 
    Console.WriteLine ("Number of steps: {0}",steps)
    Console.WriteLine "" 

let drawGame gameState = 
    gameState |> drawHeader
    gameState |> createGameBoard |> drawBoard
    

    


   
    