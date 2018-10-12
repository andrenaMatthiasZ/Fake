module Steps

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
