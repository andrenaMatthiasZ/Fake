// Learn more about F# at http://fsharp.org

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open ReadKey

type KeyDirection = Down | Up | Left | Right
  
let nextStep = 
    Console.WriteLine("next step")

[<EntryPoint>]
let main argv =
    
    printfn "Press escape key to close."

    let mutable escapeNotPressed = true
    while escapeNotPressed do
        if Console.KeyAvailable then
            let keyInfo = Console.ReadKey(true)
            let keyPressed = getKeyPressed keyInfo
            match keyPressed with 
                | KnownKey key-> 
                    match key with 
                    | Esc -> escapeNotPressed <- false
                    | Arrow(_) -> nextStep
                | Other -> nextStep
                

        else 
            nextStep
    
    0 // return an integer exit code
