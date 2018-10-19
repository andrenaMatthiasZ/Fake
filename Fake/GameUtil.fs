module GameUtil

open Game


let checkIfPositionInWall size position =
    let {width = width; height= height} = size
    let {x=x;y=y} = position
    x=1 || y=1 || x = width || y = height

let hasSamePosition position segment =
                    let {position = segmentPosition}= segment
                    position = segmentPosition

let checkIfPositionIsInBody gameState position =
    let {snake = {body=body}} = gameState
    let positionExists = position |> hasSamePosition |> List.exists
    body |> positionExists

let checkIfIsFood foodOption position = 
    match foodOption with 
        | Some {foodPosition=foodPosition} -> foodPosition = position
        | None -> false