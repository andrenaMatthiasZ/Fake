open ReadKey
open Steps
open Game
open System.Threading
open System
    
[<EntryPoint>] 
let main argv =
    Console.WriteLine("Press escape key to close.")

    let {state = state} = doNextStep keyInfoProvider initialGameState
    match state with
        | Abborted reason -> Console.WriteLine("Escape pressed")
        | Running _ -> Console.WriteLine("This should never happen.")
    Console.WriteLine("Closing game ...")
    Thread.Sleep(1000)
    0
    
