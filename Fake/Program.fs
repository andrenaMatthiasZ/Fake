open Input
open Steps
open Game
open Waiting
open Output
    
[<EntryPoint>] 
let main _ =
    writeDescription()
    let {reason=reason} = doNextStep keyInfoProvider initialGame

    write reason
    writeClosingMessage()

    waitForClosing()
    0
    
