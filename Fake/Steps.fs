module Steps

open Input
open Game
open Output  
open Movement
open Feeding

type SomeOrNoneDirection =
    | Some of Direction
    | None



let consumeKeyPressed keyPressed game =
    match game with 
        | Finished _ -> game
        | Running state -> 
            match keyPressed with
            | KnownKey knownKey -> 
                match knownKey with
                | Esc _ ->
                    let game = {state= state; reason=EscapePressed}
                    Finished game
                | Arrow direction-> 
                    let  {snake = {head={direction = oldDirection}}} = state
                    let changeDirection = changeHeadDirectionIfNotInversseToOldDirection oldDirection direction
                    Running state |> changeDirection
            | OptionalKey.None _ ->
                Running state  




let increaseStepCount game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size= size;steps=lastStep; snake=snake; foodOption = foodOption;points=points} = state
            let (StepCount lastStepCount) = lastStep 
            let nextStep = lastStepCount + 1 |> StepCount
            Running {state with steps = nextStep}

                        
let increasePointsIfStomachFull game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {snake={stomach=stomach}; points=points} = state
            match stomach with
                | Stomach.Empty -> game
                | Full ->
                        let (Points pointsValue) = points
                        Running {state with  points = Points (pointsValue + 100)}


let rec doNextStep keyPressedProvider game = 
    match game with 
        | Finished finishedGame -> finishedGame
        | Running state ->
            drawGame state
            clearInputWithDelay()
            let changeDirectionAccordingToKeyPressed = keyPressedProvider |> getKeyPressed |> consumeKeyPressed
            let stepsToDo = 
                changeDirectionAccordingToKeyPressed 
                >> fillStomachIfFoodInFrontOfSnakeHead
                >> moveSnakeAndGrowIfStomachFull 
                >> increasePointsIfStomachFull
                >> emptyStomach
                >> removeFoodIfEaten 
                >> addFoodIfMissing
                >> increaseStepCount 
                
            state |> Running |> stepsToDo |> doNextStep keyPressedProvider
            
