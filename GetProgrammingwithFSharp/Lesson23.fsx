open System.Numerics

type CustomerId = CustomerId of string

type ContactDetails =
| Address of string
| Telephone of string
| Email of string

type Customer =
    { CustomerId : CustomerId
      PrimaryContactDetails : ContactDetails
      SecondaryContactDetails : ContactDetails option }

let createCustomer customerId primaryContactDetails secondaryContactDetails =
    { CustomerId = customerId
      PrimaryContactDetails = primaryContactDetails
      SecondaryContactDetails = secondaryContactDetails }

let customer = createCustomer (CustomerId "C-123") (Email "nicki@myemail.com") None

// Final exercise

type TpsStructureId = TpsStructureId of string

type Priority =
| One
| Two
| Three

type MetricName = MetricName of string

type Comparison =
| LessThan
| GreaterThan

type Dose = Dose of double

type Plan =
    { Name : string }

type PlanSum =
    { Name : string }

type Goal =
    { Plan : Choice<Plan, PlanSum> option
      TpsStructureId : TpsStructureId
      Priority : Priority
      MetricName : MetricName
      Comparison : Comparison
      Limit : Dose }

let goal =
    { Plan = Some (Choice1Of2 ({ Name = "HN" }))
      TpsStructureId = TpsStructureId "PTV"
      Priority = One
      MetricName = MetricName "Mean[Gy]"
      Comparison = LessThan
      Limit = Dose 65.0 }