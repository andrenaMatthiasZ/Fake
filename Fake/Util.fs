module Util

open System

let removeLastElement list =
    list |> List.rev |> List.tail |> List.rev

let getRandomElement list = 
    let random = new Random()
    let randomIndex = list |> List.length |> random.Next
    list |> List.item randomIndex