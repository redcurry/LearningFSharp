module Chapter_06 =

    let oneList = [ "One" ]

    match oneList with
    | [] -> "Empty"
    | [x] -> "Something"
    | h::t -> sprintf "One %s and %i more things" h (t |> List.length)

module Exercise06_01 =

    open System

    type MeterValue =
    | Standard of int
    | Economy7 of Day:int * Night:int

    type MeterReading =
        { ReadingDate : DateTime
          MeterValue : MeterValue }
    
    // Method 1
    let formatReading (reading : MeterReading) =
        match reading with
        | { ReadingDate=readingDate
            MeterValue=Standard reading } ->
            sprintf "Your reading on: %s was %07i"
                (readingDate.ToShortDateString()) reading
        | { ReadingDate=readingDate
            MeterValue=Economy7(Day=day; Night=night) } ->
            sprintf "Your readings on: %s: Day: %07i Night: %07i"
                (readingDate.ToShortDateString()) day night

    // Method 2
    let formatReading2 (reading : MeterReading) =
        match reading.MeterValue with
        | Standard r ->
            sprintf "Your reading on: %s was %07i" (reading.ReadingDate.ToShortDateString()) r
        | Economy7(Day=day; Night=night) ->
            sprintf "Your readings on: %s: Day: %07i Night: %07i" (reading.ReadingDate.ToShortDateString()) day night

    // Method 3
    let formatReading3 { ReadingDate=readingDate; MeterValue=meterValue } =
        let dateString = readingDate.ToShortDateString()
        match meterValue with
        | Standard reading ->
            sprintf "Your reading on: %s was %07i" dateString reading
        | Economy7(Day=day; Night=night) ->
            sprintf "Your readings on: %s: Day: %07i Night: %07i" dateString day night

module Exercise06_02 =

    type FruitBatch =
        { Name : string
          Count : int }

    let fruits =
        [ { Name="Apples"; Count=3 }
          { Name="Oranges"; Count=4 }
          { Name="Bananas"; Count=2 } ]

    for { Name=name; Count=count } in fruits do
        printfn "There are %i %s" count name

    fruits
    |> List.iter (fun { Name=name; Count=count } ->
        printfn "There are %i %s" count name)

module Exercise06_03 =

    open System
    open System.Text.RegularExpressions

    let zipCodes = [
        "90210"
        "94043"
        "94043-0138"
        "10013"
        "90210-3124"
        "1OO13" ]

    let (|USZipCode|_|) s =
        let m = Regex.Match(s, @"^(\d{5})$")
        if m.Success then
            USZipCode s |> Some
        else
            None

    let (|USZipPlus4Code|_|) s =
        let m = Regex.Match(s, @"^(\d{5})\-(\d{4})$")
        if m.Success then
            USZipPlus4Code (m.Groups.[1].Value, m.Groups.[2].Value) |> Some
        else
            None

    zipCodes
    |> List.iter (fun z ->
        match z with
        | USZipCode c ->
            printfn "A normal zip code: %s" c
        | USZipPlus4Code(code, suffix) ->
            printfn "A Zip+4 code: prefix %s, suffix %s" code suffix
        | _ as n ->
            printfn "Not a zip code: %s" n)
