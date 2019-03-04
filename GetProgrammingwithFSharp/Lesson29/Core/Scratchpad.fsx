#I @"..\packages"
#r @"Newtonsoft.Json.12.0.1\lib\net45\Newtonsoft.Json.dll"

#load "Domain.fs"
#load "Operations.fs"

open Capstone5.Operations
open Capstone5.Domain
open System

let t = { Timestamp = DateTime.Now; Operation = "withdrawal"; Amount = 100.0m }

let serialized = t |> Newtonsoft.Json.JsonConvert.SerializeObject
let deserialized = serialized |> Newtonsoft.Json.JsonConvert.DeserializeObject<Transaction>
