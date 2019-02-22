open System

// 10.1.2 exercise

type Car =
    { Manufacturer : string
      EngineSize : string
      DoorCount: int }

let car =
    { Manufacturer = "Toyota"
      EngineSize = "5 L"
      DoorCount = 4 }

// 10.2.3 exercise

type Address =
    { Street : string
      City : string
      State : string }

let myAddress =
    { Street = "123 Main St."
      City = "New York"
      State = "NY" }

let herAddress =
    { Street = "123 Main St."
      City = "New York"
      State = "NY" }

myAddress = herAddress
myAddress.Equals(herAddress)
System.Object.ReferenceEquals(myAddress, herAddress)

type Customer =
    { Name : string
      Age : int }

let customer =
    { Name = "John Smith"
      Age = 35 }

let randomAge (customer:Customer) =
    let newCustomer =
        { customer with
            Age = (new System.Random()).Next(18, 45) }
    printfn "old age: %d, new age: %d" customer.Age newCustomer.Age
    newCustomer

randomAge customer
