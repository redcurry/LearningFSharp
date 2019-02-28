#r @"CSharpProject\bin\Debug\CSharpProject.dll"

open CSharpProject
let simon = Person "Simon"
simon.PrintName()

open System.Collections.Generic

type PersonComparer() =
    interface IComparer<Person> with
        member this.Compare(x, y) = x.Name.CompareTo(y.Name)

let pComparer = PersonComparer() :> IComparer<Person>
pComparer.Compare(simon, Person "Fred")

// Object expression

let personComparer =
    { new IComparer<Person> with
          member this.Compare(x, y) = x.Name.CompareTo(y.Name) }
personComparer.Compare(simon, Person "Fred")