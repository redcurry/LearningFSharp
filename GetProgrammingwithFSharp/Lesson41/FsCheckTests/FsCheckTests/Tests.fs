module Tests

open System
open Xunit
open FsCheck.Xunit
open FsCheck

let sumsNumbers numbers =
    if List.contains 5 numbers then -1
    else numbers |> List.fold (+) 0

[<Property(Verbose = true)>]
let ``Correctly adds numbers`` numbers =
    let actual = sumsNumbers numbers
    actual = List.sum numbers

let flipCase (text:string) =
    text.ToCharArray()
    |> Array.map (fun c ->
        if Char.IsUpper c then Char.ToLower c
        else Char.ToUpper c)
    |> String

// custom generator
type LettersOnlyGen() =
    static member Letters() =
        Arb.Default.Char() |> Arb.filter Char.IsLetter

[<Property(Verbose = true, Arbitrary = [| typeof<LettersOnlyGen> |])>]
let ``Always has same number of letters`` (NonEmptyString input) =
    input <> null ==> lazy
        let output = input |> flipCase
        input.Length = output.Length
