module Steps

open System
open System.Windows
open System.Windows.Input
open System.Diagnostics
open System.Threading
open ReadKey
open Game
open Drawing  

type Direction =
    | Some of KeyDirection
    | None





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
        doNextStep keyPressedProvider {size=size;steps = nextStep;snake=lastSnake}
