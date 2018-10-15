module ReadKey

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open Game


type KnownKeys = 
    | Esc 
    | Arrow of Direction

type KeyPressed =
    | KnownKey of KnownKeys
    | None

let getKeyPressed (keyInfoProvider: unit->ConsoleKey) =
    let key = keyInfoProvider()
    match key with
    | ConsoleKey.Escape -> KnownKey(Esc)
    | ConsoleKey.DownArrow -> KnownKey(Arrow(Down))
    | ConsoleKey.UpArrow -> KnownKey(Arrow(Up))
    | ConsoleKey.LeftArrow -> KnownKey(Arrow(Left))
    | ConsoleKey.RightArrow -> KnownKey(Arrow(Right))
    | _ -> None

let clearInput =
   let action = fun ()-> while (Console.KeyAvailable) do
                            Console.ReadKey(true) |> ignore
   action

let keyInfoProvider = 
   fun () -> 
       if Console.KeyAvailable then
           let action = fun ()-> Console.ReadKey(true)
           let consoleKeyInfo = action()
           consoleKeyInfo.Key
       else
           ConsoleKey.A
