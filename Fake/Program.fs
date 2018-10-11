// Learn more about F# at http://fsharp.org

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open System.Threading
open ReadKey
  
type Direction =
    | Some of KeyDirection
    | None

 
type StepCount = int
type Game = { steps: StepCount }

let clearInput =
   let action = fun ()-> while (Console.KeyAvailable) do
                            Console.ReadKey(true) |> ignore
   action

let rec doNextStep keyPressedProvider game = 
    (clearInput())
    Thread.Sleep(1000)
    let {steps=lastStep} = game
    let nextStep = lastStep + 1
    Console.WriteLine("next step: {0}",nextStep)

    let keyPressed = getKeyPressed keyPressedProvider
    match keyPressed with
    | KnownKey knownKey -> 
        match knownKey with
        | Esc _ ->
            Console.WriteLine("Esc")
            {steps = nextStep}

        | Arrow _-> 
            Console.WriteLine("Arrow")
            doNextStep keyPressedProvider {steps = nextStep}
    | KeyPressed.None _ ->
        Console.WriteLine("Other or none")
        doNextStep keyPressedProvider {steps = nextStep}
    
    
[<EntryPoint>] 
let main argv =
    printfn "Press escape key to close."
    let keyInfoProvider = 
        fun () -> 
            if Console.KeyAvailable then
                let action = fun ()-> Console.ReadKey(true)
                let consoleKeyInfo = action()
                consoleKeyInfo.Key
            else
                ConsoleKey.A
    let game =  { steps = 1 } 
    let {steps=allSteps} = doNextStep keyInfoProvider game
    0
    
