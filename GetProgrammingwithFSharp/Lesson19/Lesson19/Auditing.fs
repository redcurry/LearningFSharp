module Lesson19.Auditing

open Lesson19.Operations
open Lesson19.Domain

/// Logs to the console
let printTransaction _ accountId transaction = printfn "Account %O: %s" accountId (FileRepository.serialize transaction)

// Logs to both console and file system
let composedLogger = 
    let loggers =
        [ FileRepository.writeTransaction
          printTransaction ]
    fun accountId owner message ->
        loggers
        |> List.iter(fun logger -> logger accountId owner message)

