﻿module Feeding

open Game
open Movement

let fillStomachIfFoodInFrontOfSnakeHead game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body};foodOption=foodOption; points=points} = state
            match foodOption with
                | FoodOption.None -> game
                | FoodOption.Some {foodPosition=foodPosition} ->
                    let nextPosition = computeNewPosition headPosition direction
                    if nextPosition = foodPosition then
                        Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Full}; points = points;foodOption=foodOption}
                    else 
                        game



let removeFoodIfEaten game = 
    match game with
        | Finished _-> game
        | Running state ->
            let {size=size; steps = steps; snake = snake; foodOption = foodOption; points=points} = state
            match foodOption with 
                | FoodOption.Some food -> 
                    let {head = {headPosition=headPosition}} = snake
                    if checkIfIsFood foodOption headPosition then   
                        Running {size=size; snake=snake;steps=steps; foodOption = FoodOption.None; points=points}
                    else
                        Running state
                | FoodOption.None -> Running state

let addFoodIfMissing game =
    match game with 
        | Finished _ -> game
        | Running state ->
            let {foodOption = foodOption} = state
            match foodOption with 
                | FoodOption.Some _ -> Running state
                | FoodOption.None -> 
                    let addFood state = 
                        let {size=size; snake = snake; points = points;steps=steps} = state;
                        {size=size; steps=steps;snake = snake; points = points; foodOption = Some {foodPosition = {x=3;y=3}}}
                    state |> addFood |> Running
let emptyStomach game = 
    match game with 
        | Finished _ -> game 
        | Running state ->
            let {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;};foodOption=foodOption; points=points} = state
            Running {size=size;steps=steps;snake={head={headPosition = headPosition;direction=direction};body=body;stomach=Stomach.Empty};points=points;foodOption=foodOption}
