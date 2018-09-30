module ReadKey

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics

type KeyDirection = Down | Up | Left | Right

type Direction =
    | Some of KeyDirection
    | None

type Key = 
    | Esc 
    | Arrow of KeyDirection

type KeyPressed =
    | KnownKey of Key
    | Other

let getKeyPressed (keyInfo:ConsoleKeyInfo) =
    let key = keyInfo.Key
    match key with
    | ConsoleKey.Escape -> KnownKey(Esc)
    | ConsoleKey.DownArrow -> KnownKey(Arrow(Down))
    | ConsoleKey.UpArrow -> KnownKey(Arrow(Up))
    | ConsoleKey.LeftArrow -> KnownKey(Arrow(Left))
    | ConsoleKey.RightArrow -> KnownKey(Arrow(Right))
    | _ -> Other