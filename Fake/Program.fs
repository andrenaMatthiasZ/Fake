// Learn more about F# at http://fsharp.org


open ReadKey
open Steps
open Game

 
    
    
[<EntryPoint>] 
let main argv =
    printfn "Press escape key to close."
    let startSnake =  {x=1;y=1}::[] 
    let game =  { steps = 1; snake = startSnake} 
    let {steps=allSteps} = doNextStep keyInfoProvider game
    0
    
