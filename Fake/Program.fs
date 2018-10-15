﻿// Learn more about F# at http://fsharp.org


open ReadKey
open Steps
open Game

 
    
    
[<EntryPoint>] 
let main argv =
    printfn "Press escape key to close."
    let startSnake = {head= {x=3;y=3};tail=[]}
    let size = {width= 8; height = 7}
    let game =  {size = size; steps = 1; snake = startSnake} 
    let {steps=allSteps} = doNextStep keyInfoProvider game
    0
    
