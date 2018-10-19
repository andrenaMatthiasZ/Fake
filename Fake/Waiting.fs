module Waiting

open System.Threading

let waitForClosing() = Thread.Sleep(1000)
let waitBeforeClearingInput() = Thread.Sleep(800)
let waitAfterClearingInput() = Thread.Sleep(200)