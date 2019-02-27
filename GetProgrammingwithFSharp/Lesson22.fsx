open System

type Customer =
    { Name : string
      SafetyScore : int option
      YearPassed : int }

let customers = [ { Name = "Fred Smith"; SafetyScore = Some 550; YearPassed = 1980 }
                  { Name = "Jane Dunn";  SafetyScore = None;     YearPassed = 1980 } ]

let calculateannualPremiumUsd customer =
    match customer.SafetyScore with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None ->
        printfn "No score supplied! Using temporary premium."
        300

customers
|> List.map calculateannualPremiumUsd

// 22.4.2 exercise

let tryLoadCustomer id =
    if (2 <= id && id <= 7) then Some (sprintf "Customer %d" id)
    else None

let customerIds = [0 .. 10]

customerIds
|> List.choose tryLoadCustomer

// Last exercise

type Rule = string -> bool * string

let threeWordRule (text:string) =
    printfn "Running 3-word rule"
    (text.Split ' ').Length = 3,
    "Must be three words"

let lengthRule (text:string) =
    printfn "Running length rule"
    text.Length <= 30,
    "Max length is 30 characters"

let capsRule (text:string) =
    printfn "Running caps rule"
    text
    |> Seq.filter Char.IsLetter
    |> Seq.forall Char.IsUpper,
    "All letters must be caps"

let numberRule (text:string) =
    printfn "Running number rule"
    text
    |> Seq.forall (fun c -> not (Char.IsDigit c)),
    "All charactors must not be numbers"

let rules : Rule list =
    [  threeWordRule
       lengthRule
       capsRule
       numberRule ]

let liftRule (rule:Rule) (text:string) =
    let (passed, error) = rule text
    (passed, if passed then None else Some error)

type NewRule = string -> bool * string option

let newRules =
    rules
    |> List.map liftRule

let buildValidator (rules : NewRule list) =
    rules
    |> List.reduce (fun firstRule secondRule ->
        fun word ->
            let (passed, error) = firstRule word
            if passed then
                let (passed, error) = secondRule word
                if passed then (true, None) else (false, error)
            else (false, error))

let validate = buildValidator newRules
let word = "HELLO FrOM F#"

validate "HELLO A WORLD"
