module Lesson19.FileRepository

open Lesson19.Domain
open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path
let private findAccountFolder owner =    
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name
let private buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

let serialize transaction =
    sprintf "%O,%s,%M,%b" transaction.Timestamp transaction.Operation transaction.Amount transaction.Accepted

let deserialize (transactionStr:string) =
    let tokens = transactionStr.Split ','
    { Timestamp = DateTime.Parse tokens.[0]
      Operation = tokens.[1]
      Amount = Decimal.Parse tokens.[2]
      Accepted = Boolean.Parse tokens.[3] }

let findTransactionsOnDisk owner =
    let folder = findAccountFolder owner
    if folder = "" then (Guid.Empty, Seq.empty)
    else
        let transactions =
            Path.Combine ("accounts", folder)
            |> Directory.GetFiles
            |> Array.map (fun f -> File.ReadAllText f)
            |> Array.map (fun t -> deserialize t)
            |> Seq.ofArray
        let tokens = folder.Split '_'
        (Guid.Parse tokens.[1], transactions)

/// Logs to the file system
let writeTransaction accountId owner transaction =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (DateTime.UtcNow.ToFileTimeUtc())
    File.WriteAllText(filePath, serialize transaction)

