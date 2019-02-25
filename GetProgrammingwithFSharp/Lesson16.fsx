open System.IO

@"C:\Temp"
|> Directory.GetDirectories
|> Array.map (fun d -> (d, Directory.GetFiles d))
|> Array.map (fun (d, files) -> (d, files |> Array.sumBy (fun f -> (FileInfo f).Length)))
|> Array.sortByDescending (fun (_, size) -> size)
