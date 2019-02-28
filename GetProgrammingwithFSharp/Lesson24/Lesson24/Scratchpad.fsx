#load "Domain.fs"
#load "Operations.fs"

open Capstone4.Operations
open Capstone4.Domain
open System

type Command =
| Withdraw
| Deposit
| Exit

let tryParseCommand char =
    match char with
    | 'w' -> Some Withdraw
    | 'd' -> Some Deposit
    | 'x' -> Some Exit
    | _   -> None

tryParseCommand 'w'
tryParseCommand 'd'
tryParseCommand 'x'
tryParseCommand 'a'