open ReadKey
open Steps
open Game
open System.Threading
open System
    
[<EntryPoint>] 
let main argv =
    Console.WriteLine("Press escape key to close.")

    let {reason=reason} = doNextStep keyInfoProvider initialGame
    Console.WriteLine("Closing game ...")
    Thread.Sleep(1000)
    0
    
