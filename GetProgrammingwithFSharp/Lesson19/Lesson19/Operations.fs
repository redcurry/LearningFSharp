module Lesson19.Operations

open System
open Lesson19.Domain

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

/// Deposits an amount into an account
let deposit amount account =
    { account with Balance = account.Balance + amount }

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount account =
    let audit = audit account.AccountId account.Owner.Name
    let updatedAccount = operation amount account
    
    let accountIsUnchanged = (updatedAccount = account)

    audit { Timestamp = DateTime.UtcNow
            Operation = operationName
            Amount = amount
            Accepted = (not accountIsUnchanged) }

    updatedAccount

let processCommand account transaction =
    let (command, amount) = (transaction.Operation, transaction.Amount)
    if command = "deposit" then deposit amount account
    elif command = "withdrawal" then withdraw amount account
    else account

let loadAccount owner accountId transactions =
    let openingAccount = { Owner = owner; Balance = 0M; AccountId = accountId } 
    transactions
    |> List.filter (fun t -> t.Accepted)
    |> List.sortBy (fun t -> t.Timestamp)
    |> List.fold processCommand openingAccount

