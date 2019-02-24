open System
open System.IO

type Customer = { Age : int }
let where filter customers =
    seq {
        for customer in customers do
            if filter customer then
                yield customer }
let customers = [ { Age = 21 }; { Age = 35 }; { Age = 36 }]
let isOver35 customer = customer.Age > 35
customers |> where isOver35
customers |> where (fun customer -> customer.Age > 35)

let printCustomerAge writer customer =
    if customer.Age < 13 then writer "Child"
    elif customer.Age < 20 then writer "Teenager"
    else writer "Adult"

printCustomerAge Console.WriteLine { Age = 24 }

let writeToFile text = File.WriteAllText(@"C:\Temp\output.txt", text)
let printToFile = printCustomerAge writeToFile
printToFile { Age = 21 }

let fileContents = File.ReadAllText(@"C:\Temp\output.txt")