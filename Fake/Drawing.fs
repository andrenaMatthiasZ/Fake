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
    let {snake = snake} = game
    let xPositions =  [ 1..width ]
    let yPositions = [ 1..height]
    let gameField = [
        for y in yPositions ->
            [ 
                for x in xPositions ->

                let symbol = 
                    let position = {x=x;y=y}
                    if x=1 || y=1 || x = width || y = height then
                        Wall
                    else 
                    let {head=head} = snake
                    if head=position then
                            SnakeHead
                    else
                        Empty

                ({x=x;y=y},symbol)
            ]
        ]
        

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

    

    


   
    