#load "Domain.fs"
#load "Operations.fs"

open Lesson19.Domain
open Lesson19.Operations
open System

let openingAccount =
    { Owner = { Name = "Carlos" }; Balance = 0m; AccountId = Guid.Empty }

let isValidCommand command =
    set [ 'd'; 'w'; 'x' ] |> Set.contains command

let isStopCommand command =
    command = 'x'

let getAmount command =
    if command = 'd' then (command, 50m)
    elif command = 'w' then (command, 25m)
    else (command, 0m)

let processCommand account commandAndAmount =
    let (command, amount) = commandAndAmount
    if command = 'd' then deposit amount account
    elif command = 'w' then withdraw amount account
    else account

let processCommandT account transaction =
    let (command, amount) = (transaction.Operation, transaction.Amount)
    if command = "d" then deposit amount account
    elif command = "w" then withdraw amount account
    else account

let loadAccount owner accountId transactions =
    let openingAccount = { Owner = owner; Balance = 0M; AccountId = accountId } 
    transactions
    |> List.filter (fun t -> t.Accepted)
    |> List.sortBy (fun t -> t.Timestamp)
    |> List.fold processCommandT openingAccount

let transactions = [
    { Timestamp = DateTime.UtcNow; Operation = "d"; Amount = 100m; Accepted = true }
    { Timestamp = DateTime.UtcNow; Operation = "w"; Amount = 50m; Accepted = true }
    { Timestamp = DateTime.UtcNow; Operation = "m"; Amount = 50m; Accepted = false }
    { Timestamp = DateTime.UtcNow; Operation = "d"; Amount = 10m; Accepted = true } ]

loadAccount ({ Name = "Carlos" }) Guid.Empty transactions

let serialize transaction =
    sprintf "%O,%s,%M,%b" transaction.Timestamp transaction.Operation transaction.Amount transaction.Accepted

let deserialize (transactionStr:string) =
    let tokens = transactionStr.Split ','
    { Timestamp = DateTime.Parse tokens.[0]
      Operation = tokens.[1]
      Amount = Decimal.Parse tokens.[2]
      Accepted = Boolean.Parse tokens.[3] }

transactions
|> List.map serialize
|> List.map deserialize

let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount
