open System

let getCreditLimit customer =
    match customer with
    | ("medium", 1) ->  500
    | ("good",   years) when years < 2 ->  750
    | ("good",   2) -> 1000
    | ("good",   _) -> 2000
    | _             ->  250

getCreditLimit ("good", 2)

type Customer =
    { Name : string
      Balance : int }

// Using if/else
let handleCustomers customers =
    if List.length customers = 0 then failwith "There are no customers."
    elif List.length customers = 1 then printfn "%s" customers.[0].Name
    elif List.length customers = 2 then
        let totalBalance =
            customers
            |> List.sumBy (fun c -> c.Balance)
        printfn "%d" totalBalance
    else printfn "%d" (List.length customers)

// Using pattern matching
let handleCustomers2 customers =
    match customers with
    | []              -> failwith "There are no customers."
    | [customer]      -> printfn "%s" customer.Name
    | [first; second] -> printfn "%d" (first.Balance + second.Balance)
    | customers       -> printfn "%d" customers.Length

[ { Name = "Carlos"; Balance = 100 } ]
|> handleCustomers2

// Last exercise
let randomNumbers =
    seq {
        let random = new Random()
        while true do
            yield random.Next (1, 100) }

let testList list =
    match list with
    | []                         -> "List is empty."
    | list when list.Length = 10 -> "List has 10 items."
    | 5 :: _                     -> "First item is 5."
    | _                          -> "Normal list."

let n = (new Random()).Next (1, 20)
randomNumbers
|> Seq.take n
|> List.ofSeq
|> testList
