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

let createListOfAllPositions size =
    let {width=width;height=height} =  size
    let listList = 
        [for x in [1..width] ->
            [for y in [1..height] ->
                {x=x;y=y}
            ]
        ]
    listList |> List.concat 

let toPosition snakeSegment = 
    let {position=position} = snakeSegment
    position


type ReasonForInvalidPosition = 
    | CollisionWithWall
    | CollisionWithBody
type PositionValidity =
    | Valid
    | Invalid of ReasonForInvalidPosition

let computeHeadValidity state = 
                let {snake=snake;size=size} = state
                let {head={headPosition=headPosition}} = snake
                let isInWall = checkIfPositionInWall size
                let isInBody = checkIfPositionIsInBody state
                if isInWall headPosition then 
                    Invalid CollisionWithWall
                else if isInBody headPosition then
                    Invalid CollisionWithBody
                else
                    Valid