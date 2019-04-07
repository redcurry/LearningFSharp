open System.Threading
open System

// 36.2

printfn "Loading data!"
Thread.Sleep(5000)
printfn "Loaded data!"
printfn "My name is Carlos."
async {
    printfn "Loading data!"
    Thread.Sleep(5000)
    printfn "Loaded data!" }
|> Async.Start
printfn "My name is Carlos."

let asyncHello : Async<string> = async { return "Hello" }
// let length = asyncHello.Length
let text = asyncHello |> Async.RunSynchronously
let lengthTwo = text.Length

// 36.2.2

let printThread text = printfn "THREAD %d: %s" Thread.CurrentThread.ManagedThreadId text

let doWork() =
    printThread "Starting long running work!"
    Thread.Sleep 5000
    "HELLO"

let asyncLength : Async<int> =
    printThread "Creating async block"
    let asyncBlock =
        async {
            printThread "In block!"
            let text = doWork()
            return (text + " WORLD").Length }
    printThread "Created async block"
    asyncBlock

let length = asyncLength |> Async.RunSynchronously

// 36.3

let getTextAsync = async { return "HELLO" }

let printHelloWorld =
    async {
        let! text = getTextAsync
        return printfn "%s WORLD" text }

printHelloWorld |> Async.Start

// 36.4

let random = Random()
let pickANumberAsync =
    async {
        printfn "Thread %d" Thread.CurrentThread.ManagedThreadId
        Thread.Sleep 100
        return random.Next(10) }
let createFiftyNumbers =
    let workflows = [ for i in 1..50 -> pickANumberAsync ]

    async {
        let! numbers = workflows |> Async.Parallel
        printfn "Total is %d" (numbers |> Array.sum) }

createFiftyNumbers |> Async.Start

// Exercise

let downloadData url =
    let webClient = new System.Net.WebClient()
    async {
        printfn "Downloading data on thread %d" Thread.CurrentThread.ManagedThreadId
        let! data = webClient.AsyncDownloadData (Uri url)
        return data.Length }

let urls = [ "http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com" ]

let work =
    urls
    |> List.map downloadData
    |> Async.Parallel
    |> Async.RunSynchronously

Array.sum work

// 36.5 Exercise

let downloadData' url =
    let webClient = new System.Net.WebClient()
    async {
        printfn "Downloading data on thread %d" Thread.CurrentThread.ManagedThreadId
        let! data = webClient.DownloadDataTaskAsync (Uri url) |> Async.AwaitTask
        return data.Length }

let urls' = [ "http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com" ]

let work' =
    urls'
    |> List.map downloadData
    |> Async.Parallel
    |> Async.StartAsTask

Array.sum work

// Final exercise

let myUrls = [
    "http://www.carlosjanderson.com"
    "http://www.microsoft.com"
    "http://www.trello.com"
    "http://www.facebook.com"
    "http://www.stackoverflow.com"
    "http://www.amazon.com"
    "http://www.github.com"
    "http://www.apple.com"
    "http://www.youtube.com"
    "http://www.nasa.gov" ]

// synchronous

let downloadWebData url =
    let webClient = new System.Net.WebClient()
    let data = webClient.DownloadData (Uri url)
    data.Length

let syncTest() =
    myUrls
    |> List.map downloadWebData

let benchmark f =
    let t1 = DateTime.Now
    let _ = f()
    let t2 = DateTime.Now
    printfn "%f ms" (t2 - t1).TotalMilliseconds

benchmark syncTest

// multithreaded

let downloadWebDataAsync url =
    async {
        return downloadWebData url }

let threadTest() =
    myUrls
    |> List.map downloadWebDataAsync
    |> Async.Parallel
    |> Async.RunSynchronously

benchmark threadTest

// asynchronous

let downloadWebDataAsync' url =
    async {
        let webClient = new System.Net.WebClient()
        let! data = webClient.AsyncDownloadData (Uri url)
        return data.Length
    }

let asyncTest() =
    myUrls
    |> List.map downloadWebDataAsync'
    |> Async.Parallel
    |> Async.RunSynchronously

benchmark asyncTest

// handle exceptions

let testExceptionAsync url =
    async {
        let webClient = new System.Net.WebClient()
        let! data = webClient.AsyncDownloadData (Uri url)
        return data.Length }

testExceptionAsync "http://www.microsoft.com" |> Async.Catch |> Async.RunSynchronously
testExceptionAsync "http://www.thisdomainshouldnotexist.com" |> Async.Catch |> Async.RunSynchronously