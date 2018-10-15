module Drawing

open Game
open System
open System.Drawing

type Symbol =
    | Wall
    | Empty
    | SnakeHead 


type Field = Symbol*Position

let drawGame game = 
    let {snake = snake; size = size; steps = steps} = game
    let {width = width; height= height} = size
    let xPositions =  [ 1..width ]
    let yPositions = [ 1..height]
    let gameField = [
        for y in yPositions ->
            [ 
                for x in xPositions ->

                let symbol = 
                    let position = {x=x;y=y}
                    let positionIsWall = checkIfPositionInWall game position
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
      
    Console.WriteLine()
    Console.WriteLine()
    Console.WriteLine("Number of steps: {0}",steps)
    Console.WriteLine()

    for line in gameField do
        for field in line do
            let character = 
                let (_,symbol) = field
                match symbol with
                | Wall _ -> '#'
                | Empty _ -> ' '
                | SnakeHead _ -> 'o'
            Console.Write(character)
        Console.WriteLine("")

    

    


   
    