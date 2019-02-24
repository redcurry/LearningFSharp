module Auditing

open System.IO
open Domain

let fileLogger (account:Account) message =
    let path = Path.Combine(@"C:\Temp", sprintf "%s.txt" (account.Id.ToString()))
    File.AppendAllText(path, message)

let consoleLogger (account:Account) message =
    printfn "Account %s: %s" (account.Id.ToString()) message

