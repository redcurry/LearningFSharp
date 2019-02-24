#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open Domain
open Operations
open Auditing

open System

let carlos =
    { FirstName = "Carlos"
      LastName = "Anderson" }

let checking =
    { Id = Guid.NewGuid()
      Balance = 0m
      Owner = carlos }

checking |> deposit 100m |> withdraw 25m |> withdraw 100m

let depositWithConsoleLogger = auditAs "deposit" consoleLogger deposit
let withdrawWithConsoleLogger = auditAs "withdraw" consoleLogger withdraw

checking
|> depositWithConsoleLogger 100m
|> withdrawWithConsoleLogger 25m
