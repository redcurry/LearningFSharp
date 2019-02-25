module Lesson19.Program

open System
open Lesson19.Domain
open Lesson19.Operations

let isValidCommand command =
    set [ 'd'; 'w'; 'x' ] |> Set.contains command

let isStopCommand command =
    command = 'x'

let getAmountConsole command =
    let amount =
        Console.Write "Please enter the amount: "
        Decimal.Parse (Console.ReadLine())
    if command = 'd' then (command, amount)
    elif command = 'w' then (command, amount)
    else (command, 0m)

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

let processCommand account commandAndAmount =
    let (command, amount) = commandAndAmount
    if command = 'd' then depositWithAudit amount account
    elif command = 'w' then withdrawWithAudit amount account
    else account

[<EntryPoint>]
let main _ =
    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let (accountId, transactions) = FileRepository.findTransactionsOnDisk name
    let openingAccount = loadAccount { Name = name } accountId (List.ofSeq transactions)

    let closingAccount =
        let commands = seq {
            while true do
                Console.Write "(d)eposit, (w)ithdraw, e(x)it: "
                yield Console.ReadKey().KeyChar }

        commands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.map getAmountConsole
        |> Seq.fold processCommand openingAccount

    Console.Clear()
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0