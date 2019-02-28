module Capstone4.Operations

open System
open Capstone4.Domain

let classifyAccount account =
    if account.Balance >= 0m then (InCredit (CreditAccount account))
    else Overdrawn account

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount (CreditAccount account) =
    { account with Balance = account.Balance - amount }
    |> classifyAccount

let withdrawSafe amount ratedAccount =
    match ratedAccount with
    | InCredit account -> account |> withdraw amount
    | Overdrawn _ ->
        printfn "Your account is overdrawn - withdrawal rejected!"
        ratedAccount

/// Deposits an amount into an account
let deposit amount account =
    let account =
        match account with
        | InCredit (CreditAccount account) -> account
        | Overdrawn account -> account
    { account with Balance = account.Balance + amount }
    |> classifyAccount

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount (account:RatedAccount) =
    let updatedAccount = operation amount account
    
    let accountIsUnchanged = (updatedAccount = account)

    let transaction =
        let transaction = { Operation = operationName; Amount = amount; Timestamp = DateTime.UtcNow; Accepted = true }
        if accountIsUnchanged then { transaction with Accepted = false }
        else transaction

    match updatedAccount with
    | InCredit (CreditAccount account) -> audit account.AccountId account.Owner.Name transaction
    | Overdrawn account -> audit account.AccountId account.Owner.Name transaction

    updatedAccount

/// Creates an account from a historical set of transactions
let loadAccount (owner, accountId, transactions) =
    let openingAccount = { AccountId = accountId; Balance = 0M; Owner = { Name = owner } }

    transactions
    |> Seq.sortBy(fun txn -> txn.Timestamp)
    |> Seq.fold(fun account txn ->
        if txn.Operation = "withdraw" then account |> withdrawSafe txn.Amount
        else account |> deposit txn.Amount) (openingAccount |> classifyAccount)
