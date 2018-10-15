﻿module ReadKey

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open Game
open System.Threading


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

let clearInput =
   let action = fun ()-> while (Console.KeyAvailable) do
                            Console.ReadKey(true) |> ignore
   action

let clearInputWithDelay = fun()->
    Thread.Sleep(900)
    (clearInput())
    Thread.Sleep(100)
 

let keyInfoProvider = 
   fun () -> 
       if Console.KeyAvailable then
           let action = fun ()-> Console.ReadKey(true)
           let consoleKeyInfo = action()
           consoleKeyInfo.Key
       else
           ConsoleKey.A
