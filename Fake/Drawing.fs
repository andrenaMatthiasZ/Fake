module Drawing

open Game
open System
open System.Drawing

type Symbol =
    | Wall
    | Empty
    | SnakeHead 
    | SnakeBody
    | Food


type Field = Symbol*Position

let createSymbol gameState position = 
    let symbol = 
        let {snake=snake; foodOption=foodOption; size=size} = gameState
        let {head={headPosition = headPosition;direction = _}; body=body} = snake
        
        let isFood = checkIfIsFood foodOption

        let isHead position= 
            headPosition = position;

        let isBody position = 
                let isCurrentPosition = hasSamePosition position
                body  |>  List.exists isCurrentPosition

        let isWall position = 
            position |> checkIfPositionInWall size

        if isWall position then
            Wall
        else if isFood position then
            Food
        else if isHead position then
            SnakeHead
        else if isBody position then
            SnakeBody
        else
            Empty
            
    (position,symbol)

let createGameBoard gameState =
        let { size = size} = gameState
        let {width = width; height= height} = size
        let xPositions =  [ 1..width ]
        let yPositions = [ 1..height]
        [
        for y in yPositions ->
            [ 
                for x in xPositions ->
                    let position = {x=x; y=y}
                    position |> createSymbol gameState 
                               
            ]
        ]

let drawBoard gameBoard =
    for line in gameBoard do
        for field in line do
            let character = 
                let (_,symbol) = field
                match symbol with
                | Wall -> '#'
                | Empty -> ' '
                | SnakeHead -> 'o'
                | SnakeBody -> 'x'
                | Food -> 'ö'
            Console.Write character
        Console.WriteLine "" 

let drawHeader gameState =
    let {steps = steps; points= (Points points)} = gameState
    Console.WriteLine "" 
    Console.WriteLine "" 
    Console.WriteLine ("Number of steps: {0}",steps)
    Console.WriteLine ("Points: {0}",points)
    Console.WriteLine "" 

let drawGame gameState = 
    gameState |> drawHeader
    gameState |> createGameBoard |> drawBoard
    

    


   
    