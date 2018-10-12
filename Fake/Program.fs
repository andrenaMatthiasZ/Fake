// Learn more about F# at http://fsharp.org


open ReadKey
open Steps

 
    
    
[<EntryPoint>] 
let main argv =
    printfn "Press escape key to close."
    let game =  { steps = 1 } 
    let {steps=allSteps} = doNextStep keyInfoProvider game
    0
    
