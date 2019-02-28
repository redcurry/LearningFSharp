module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdrawSafe
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

let loadAccountOption = Option.map loadAccount
let tryLoadAccountFromDisk = FileRepository.tryFindTransactionsOnDisk >> loadAccountOption

[<AutoOpen>]
module CommandParsing =
    let isStopCommand = (=) Exit
    let tryParseCommand char =
        match char with
        | 'w' -> Some (AccountCommand Withdraw)
        | 'd' -> Some (AccountCommand Deposit)
        | 'x' -> Some Exit
        | _   -> None
    let tryGetBankOperation cmd =
        match cmd with
        | AccountCommand op -> Some op
        | Exit              -> None

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }

    let tryGetAmount command =
        Console.WriteLine()
        Console.Write "Enter Amount: "
        let amount = Console.ReadLine() |> Decimal.TryParse
        match amount with
        | true, amount -> Some (command, amount)
        | false, _     -> None

[<EntryPoint>]
let main _ =
    let openingAccount =
        Console.Write "Please enter your name: "
        let owner = Console.ReadLine()

        match (tryLoadAccountFromDisk owner) with
        | Some account -> account
        | None ->
            { Balance = 0m
              AccountId = Guid.NewGuid()
              Owner = { Name = owner } } |> classifyAccount
    
    let balance =
        match openingAccount with
        | InCredit (CreditAccount account) -> account.Balance
        | Overdrawn account -> account.Balance

    printfn "Current balance is £%M" balance

    let processCommand account (command, amount) =
        printfn ""
        let account =
            match command with
            | Withdraw -> account |> withdrawWithAudit amount
            | Deposit  -> account |> depositWithAudit amount

        let balance =
            match account with
            | InCredit (CreditAccount a) -> a.Balance
            | Overdrawn a -> a.Balance

        printfn "Current balance is £%M" balance
        account

    let closingAccount =
        commands
        |> Seq.choose tryParseCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.choose tryGetBankOperation
        |> Seq.choose tryGetAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0