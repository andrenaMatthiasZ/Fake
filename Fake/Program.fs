open ReadKey
open Steps
open Game
open System.Threading
open System
    
[<EntryPoint>] 
let main argv =
    Console.WriteLine("Press escape key to close.")

    let {steps=allSteps} = doNextStep keyInfoProvider initialGameState

    Console.WriteLine("Closing game ...")
    Thread.Sleep(1000)
    0
    
