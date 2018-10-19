module Input

open System
open Game
open Waiting


type Key = 
    | Esc 
    | Arrow of Direction

type OptionalKey =
    | KnownKey of Key
    | None

let getKeyPressed (keyInfoProvider: unit->ConsoleKey) =
    let key = keyInfoProvider()
    match key with
    | ConsoleKey.Escape -> Esc |> KnownKey
    | ConsoleKey.DownArrow -> Down |> Arrow |> KnownKey 
    | ConsoleKey.UpArrow -> Up |> Arrow |> KnownKey
    | ConsoleKey.LeftArrow -> Left |> Arrow |> KnownKey
    | ConsoleKey.RightArrow -> Right |> Arrow |> KnownKey
    | _ -> None

let clearInput() = while (Console.KeyAvailable) do
                            Console.ReadKey(true) |> ignore
   

let clearInputWithDelay = fun()->
    waitBeforeClearingInput()
    clearInput()
    waitAfterClearingInput()
 
let defaultUnknown = ConsoleKey.A

let keyInfoProvider = 
   fun () -> 
       if Console.KeyAvailable then
           let action = fun ()-> Console.ReadKey(true)
           let consoleKeyInfo = action()
           consoleKeyInfo.Key
       else
            defaultUnknown
           
