// Exercise 11-2

open System

type Message =
    { FileName : string
      Content : float[] }

type Reading =
    { TimeStamp : DateTimeOffset
      Data : float[] }

let example =
    [|
        { FileName = "2019-02-23T02:00:00-05:00"
          Content = [|1.0; 2.0; 3.0; 4.0|] }
        { FileName = "2019-02-23T02:00:10-05:00"
          Content = [|5.0; 6.0; 7.0; 8.0|] }
        { FileName = "error"
          Content = [||] }
        { FileName = "2019-02-23T02:00:20-05:00"
          Content = [|1.0; 2.0; 3.0; Double.NaN|] }
    |]

let log s = printfn "Logging: %s" s

type MessageError =
    | InvalidFileName of fileName:string
    | DataContainsNaN of fileName:string * index:int

let getReading message =
    match DateTimeOffset.TryParse(message.FileName) with
    | true, dt ->
        let reading = { TimeStamp = dt; Data = message.Content }
        Ok (message.FileName, reading)
    | false, _ ->
        Error (InvalidFileName(message.FileName))

let validateData(fileName, reading) =
    let nanIndex =
        reading.Data
        |> Array.tryFindIndex (Double.IsNaN)
    match nanIndex with
    | Some i ->
        Error (DataContainsNaN(fileName, i))
    | None ->
        Ok reading

let logError (e : MessageError) =
    match e with
    | InvalidFileName fn -> log (sprintf "Invalid file name: %s" fn)
    | DataContainsNaN (fn, i) -> log (sprintf "NaN found for %s at %i" fn i)

open Result

let processMessage =
    getReading
    >> bind validateData
    >> mapError logError

let processData data =
    data
    |> Array.map processMessage
    |> Array.choose (fun result ->
        match result with
        | Ok reading -> reading |> Some
        | Error _ -> None)

let demo() =
    example
    |> processData
    |> Array.iter (printfn "%A")
