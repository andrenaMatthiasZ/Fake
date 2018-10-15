﻿module Steps

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open System.Threading
open ReadKey
open Game
open Drawing  

type SomeOrNoneDirection =
    | Some of Direction
    | None



 let computeNewPosition position direction = 
    let {x=x;y=y} = position
    match direction with
    | Up _ -> {x=x;y=y+1}
    | Right _ -> {x=x+1;y=y}
    | Down _-> {x=x;y=y-1}
    | Left _ -> {x=x-1;y=y}

let rec doNextStep keyPressedProvider game = 
    drawGame game
    (clearInput())
    Thread.Sleep(1000)
    let {size= size;steps=lastStep; snake=lastSnake} = game
    let nextStep = lastStep + 1
    Console.WriteLine("next step: {0}",nextStep)

    let keyPressed = getKeyPressed keyPressedProvider
    match keyPressed with
    | KnownKey knownKey -> 
        match knownKey with
        | Esc _ ->
            Console.WriteLine("Esc")
            {size=size;steps = nextStep;snake=lastSnake}

        | Arrow _-> 
            Console.WriteLine("Arrow")
            doNextStep keyPressedProvider {size=size;steps = nextStep;snake=lastSnake}
    | KeyPressed.None _ ->
        Console.WriteLine("Other or none")
        let {head = head; tail=tail} = lastSnake
        let {direction = direction; position = position} = head
      
        let newPosition = computeNewPosition position direction
        Console.WriteLine(newPosition)
        let newHead = {position = newPosition;direction=direction}
        let newSnake = {head=newHead;tail=tail}
        doNextStep keyPressedProvider {size=size;steps = nextStep;snake=newSnake}
