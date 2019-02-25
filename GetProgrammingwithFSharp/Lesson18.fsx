open System

let sum inputs =
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + input
    accumulator

let length inputs =
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + 1
    accumulator

let max inputs =
    let mutable accumulator = Seq.head inputs
    for input in inputs do
        accumulator <- if input > accumulator then input else accumulator
    accumulator

sum [-10..10]
length [-10..10]
max [-10..10]

let length2 inputs =
    Seq.fold (fun state _ -> state + 1) 0 inputs

let max2 inputs =
    Seq.fold (fun state x -> if x > state then x else state) (Seq.head inputs) inputs

length2 [-10..10]
max2 [-3; 4; -2; 8; 3; 9; -20; 15; 12]

let sum3 inputs =
    (0, inputs) ||> Seq.fold (fun state input -> state + input)

sum3 [-3; 4; -2; 8; 3; 9; -20; 15; 12]

// Rules exercise

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

let buildValidator (rules : Rule list) =
    rules
    |> List.reduce (fun firstRule secondRule ->
        fun word ->
            let (passed, error) = firstRule word
            if passed then
                let (passed, error) = secondRule word
                if passed then (true, "") else (false, error)
            else (false, error))

let validate = buildValidator rules
let word = "HELLO FrOM F#"

validate "HELLO 1 WORLD"
