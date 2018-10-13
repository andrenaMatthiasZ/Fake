module Drawing

open Game
open System
open System.Drawing

type Symbol =
    | Wall
    | Empty

let outherLine = Wall::Wall::Wall::Wall::Wall::Wall::[]
let innerLine = Wall::Empty::Empty::Empty::Empty::Wall::[]

let drawGame = fun ()->
 
    let gameField = [outherLine;innerLine;innerLine;innerLine;outherLine]

    for line in gameField do
        for symbol in line do
            let character = 
                match symbol with
                | Wall _ -> '#'
                | Empty _ -> ' '
            Console.Write(character)
        Console.WriteLine("")

    

    


   
    