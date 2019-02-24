module Domain

open System

type Customer =
    { FirstName : string
      LastName : string }

type Account =
    { Id : Guid
      Balance : decimal
      Owner : Customer }
