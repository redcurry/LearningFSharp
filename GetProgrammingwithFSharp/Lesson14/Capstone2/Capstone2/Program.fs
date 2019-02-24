open System
open Domain
open Operations
open Auditing

[<EntryPoint>]
let main argv =
    printfn "Enter your first and last name:"
    let name = Console.ReadLine()
    let tokens = name.Split()
    let customer =
        { FirstName = tokens.[0]
          LastName = tokens.[1] }
    printfn "Enter your opening balance:"
    let balanceStr = Console.ReadLine()
    let balance = Decimal.Parse balanceStr
    let mutable account =
        { Id = Guid.NewGuid()
          Owner = customer
          Balance = balance }
    let depositWithConsoleLogger = deposit |> auditAs "deposit" consoleLogger
    let withdrawWithConsoleLogger = withdraw |> auditAs "withdraw" consoleLogger
    while true do
        let action = Console.ReadLine()
        if action = "exit" then Environment.Exit 0
        let tokens = action.Split()
        let accountAction = tokens.[0]
        let amount = Decimal.Parse tokens.[1]
        account <-
            if accountAction = "deposit" then account |> depositWithConsoleLogger amount
            elif accountAction = "withdraw" then account |> withdrawWithConsoleLogger amount
            else account
    0
