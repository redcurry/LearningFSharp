open System
open System.IO

// 11.2 exercise

let writeToFile (date:DateTime) filename text =
    let path = sprintf "%s-%s.txt" (date.ToString "yyMMdd") filename
    File.WriteAllText(path, text)

let writeToToday = writeToFile DateTime.Now.Date
let writeToTomorrow = writeToFile (DateTime.Now.Date.AddDays 1.0)
let writeToTodayHelloWorld = writeToToday "hello-world"

printfn "%s" (Directory.GetCurrentDirectory())
writeToToday "first-file" "The quick brown fox jumped over the lazy dog"

// 11.2.1 exercise

let drive distance petrol =
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

let startPetrol = 100.0

startPetrol |> drive "far" |> drive "medium" |> drive "short"