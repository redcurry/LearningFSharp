/// Provides access to the banking API.
module Capstone5.Api

open Capstone5.Domain
open Capstone5.Operations
open System
open Capstone5

/// Loads an account from disk. If no account exists, an empty one is automatically created.
let LoadAccount (customer:Customer) : RatedAccount =
    let accountDetails = FileRepository.tryFindTransactionsOnDisk customer.Name
    match accountDetails with
    | None -> InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                       Balance = 0M
                                       Owner = customer })
    | Some account -> Operations.loadAccount account

/// Deposits funds into an account.
let Deposit (amount:decimal) (customer:Customer) : RatedAccount =
    let account = LoadAccount customer
    let accountId = account.GetField (fun a -> a.AccountId)
    let owner = account.GetField(fun a -> a.Owner)
    auditAs "deposit" Auditing.composedLogger deposit amount account accountId owner

/// Withdraws funds from an account that is in credit.
let Withdraw (amount:decimal) (customer:Customer) : RatedAccount =
    let ratedAccount = LoadAccount customer
    let accountId = ratedAccount.GetField (fun a -> a.AccountId)
    let owner = ratedAccount.GetField(fun a -> a.Owner)
    match ratedAccount with
    | InCredit creditAccount -> auditAs "withdraw" Auditing.composedLogger withdraw amount creditAccount accountId owner
    | Overdrawn _ -> ratedAccount
                                 
/// Loads the transaction history for an owner. If no transactions exist, returns an empty sequence.
let LoadTransactionHistory(customer:Customer) : Transaction seq =
    let accountDetails = FileRepository.tryFindTransactionsOnDisk customer.Name
    match accountDetails with
    | None -> Seq.empty
    | Some (_, _, transactions) -> transactions

