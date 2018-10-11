module ReadKey

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics

type KeyDirection = Down | Up | Left | Right

type KnownKeys = 
    | Esc 
    | Arrow of KeyDirection

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