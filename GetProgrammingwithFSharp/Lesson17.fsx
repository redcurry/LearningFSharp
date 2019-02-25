open System.IO

// Map

let inventory =
    [ ("Apples", 0.33); ("Oranges", 0.23); ("Bananas", 0.45) ]
    |> Map.ofList

let apples = inventory.["Apples"]
let pineapples = inventory.["Pineapples"]

let newInventory =
    inventory
    |> Map.add "Pineapples" 0.87
    |> Map.remove "Apples"

// Map exercise

System.IO.Directory.EnumerateDirectories @"C:\"
|> Seq.map System.IO.DirectoryInfo
|> Seq.map (fun d -> (d.Name, d.CreationTimeUtc))
|> Map.ofSeq
|> Map.map (fun name time -> System.DateTime.UtcNow - time)

// Final exercise
let winTypes =
    Directory.GetFiles(@"C:\Windows")
    |> Array.map (fun f -> Path.GetExtension f)
    |> Set.ofArray

let sysTypes =
    Directory.GetFiles(@"C:\Windows\system32")
    |> Array.map (fun f -> Path.GetExtension f)
    |> Set.ofArray

Set.intersect winTypes sysTypes
|> Array.ofSeq
