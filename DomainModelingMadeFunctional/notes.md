# Domain Modeling Made Functional

## Chapter 1

* Use "Event Storming" to brainstorm with everyone involved in the project,
  and come up with the "ubiquitous language," most importantly
  the business events and workflows

* An event can trigger a commands, which initiates a workflow,
  which causes domain events to be triggered, which can in turn
  generate more commands

* Partition the problem into smaller "subdomains" (e.g., order-taking,
  shipping, billing)

* The domain model or context (solution space) will never be as rich
  as the real world domain (problem space), and it will not
  have a one-to-one relationship with it; e.g., multiple domains
  in the problem space may be modeled as a single bounded context

* Design bounded contexts such that they can evolve independently
  from each other and make the business workflow smoother,
  even if the design is not 100% pure

* Trying to implement all bounded contexts at once often leads to failure;
  focus on those that add the most value, and expand from there

* Don't expose things in the design that don't represent something
  in the domain expert's mental model; e.g., don't have an "OrderFactory"
  since a domain expert likely won't know what that is

* It's a good idea to keep a shared document that lists the terms
  and definitions in the ubiquitous language

## Chapter 2

* Instead of having one long interview with domain experts, who are busy,
  it's better to have many small interviews, each focused on a single workflow

* You can also discuss the non-functional requirements
  (e.g., number of customers, customer expertise, latency)

* Don't think about the database schema yet, and don't think about
  class hiearchies either (e.g., the abstract class OrderBase,
  which doesn't exist in the domain)

* Use simple text to capture the domain, documenting inputs and outputs
  and pseudocode for business logic in workflows,
  and use "AND" or "OR" for data structures,
  depending on whether they're both required or either one

* In a business, certain processes are more important than others
  (i.e., they make money), so this must be documented in the design

* Capture the domain from the domain expert's point of view,
  even if the design appears too "strict", as it can be changed later

* Capture quantity constraints (minimum and maximum), which avoids errors

* Define the basic data types as close to the domain as possible,
  including their different states (e.g., UnvalidatedOrder and ValidatedOrder)

* Flesh out the steps and substeps of the workflow in pseudocode,
  including inputs and outputs (such as further events),
  in such detail that a domain expert can check its validity

## Chapter 3

* Using the "C4" architecture approach, a software architucture
  is made up of containers (e.g., website, database),
  which are made up of components, which are made up of classes
  (or modules in functional programming)

* The goal of a good architucture is to define the boundaries
  between containers, components, and modules

* How events are transmitted from one bounded context to another
  depends on the architecture; it could be via asynchronous communication,
  internal queue, or function calls

* The data transmitted are DTOs that must be serialized or deserialized,
  and validated so that the bounded context contains a valid domain

* The ways in which contexts understand each other (like a contract) are:
  shared kernel (contexts share some domain design), customer/supplier or
  consumer-driven (downstream context defines the contract),
  and conformist (upstream context defines the contract)

* When communicating with a third-party component, use an anti-corruption layer,
  which translates the external language to the bounded context language

* The input to a workflow is always the data associated with a command,
  and the output is always a set of events to communicate to other contexts

* Instead of a layered architecture, use an onion architecture,
  where dependencies point inward (the domain is at the center
  and I/O is on the edges)

## Chapter 4

* If Person is defined as

      type Person = { First:string; Last:string }

  a Person value (aPerson) can be deconstructed with

      let { First=first; Last=last } = aPerson

  which in equivalent to

      let first = aPerson.First
      let last  = aPerson.Last

* Use the "rec" keyword in a module to allow types defined first
  to reference types defined later (less strict order of type definition),
  or use the "and" keyword instead of "type" for the later types

## Chapter 5

* There may be types that appear similar (e.g., ShippingAddress and
  BillingAddress), but you need to ask the domain expert if they are different,
  because they may need to evolve independently

* You can use "type Undefined = exn" to temporarily define domain types
  as Undefined until you have a better idea of the actual type

* For inputs (or outputs) one could use a tuple to contain several values,
  but it's often better to create a named record type

* Instead of a function signature like this:

      type ValidateOrder =
          UnvalidatedOrder -> Async<Result<ValidatedOrder,ValidationError list>>

  define an alias for the return type:

      type ValidationResponse<'a> = Async<Result<'a, ValidationError list>>

  so that the new function signature is

      type ValidatioOrder =
          UnvalidatedOrder -> ValidationResponse<ValidatedOrder>

* When modeling an entity that is a choice (e.g., UpaidInvoice and PaidInvoice),
  put the ID inside the choice:

      type UpaidInvoice = { InvoiceId : InvoiceId; ... }
      type PaidInvoice  = { InvoiceId : InvoiceId; ... }

      type Invoice =
        | Unpaid of UnpaidInvoice
        | Paid   of PaidInvoice

* Also for entities, disallow structural equality using the attributes
  ``[<NoEquality; NoComparison>]``, so entities will have to be compared
  by using their IDs explicitly

## Chapter 6

* To keep the domain clean, we should model both integrity
  (e.g., value within proper range, order has at least one order line)
  and consistency (e.g., total bill is equal to sum of orders)

* For simple values, make the type constructor private, and instead
  write a "create" function inside a module with the same name as the type:

      type UnitQuantity = private UnitQuantity of int

      module UnitQuantity =
          let create qty =
            ...

  and write a "value" function to extract the value when pattern matching:

      let value (UnitQuantity qty) = qty

* Numeric values can be annotated with "units of measure"

* Enforce lists to have at least one element by creating a NonEmptyList type:

      type NonEmptyList<'a> =
        { First: 'a
          Rest:  'a list }

* When modeling business rules, instead of

      type CustomerEmail =
        | Unverified of EmailAddress
        | Verified   of EmailAddress

  use a new type VerifiedEmailAddress that can only be created
  by the e-mail verification service:

      type CustomerEmail =
        | Unverified of EmailAddress
        | Verified   of VerifiedEmailAddress

  so that it's impossible for anyone to create a Verified customer e-mail

* Use aggregates to guarantee consistency, making use that if they are saved
  to a database, all its contents are saved in the same transaction

* When ensuring consistency between bounded contexts, it may be easier
  to rely on "eventual consistency," i.e., the system will be consistent
  some time in the future, not immediately (because that can take time)

* For aggregates within a bounded context, eventual consistency can be used,
  but there are cases where multiple aggregates must be involved in a single
  transaction (e.g., money being transferred)

* In some cases, the transaction can be refactored to be an entity itself;
  and, in general, aggregates can be created freely for just one use case

## Chapter 7

* The input to a workflow should always be a domain object,
  typically a command (e.g., PlaceOrder)

* A command can have common fields, so instead of defining them
  in every command, use a separate type:

      type Command<'data> =
        { Data: 'data
          Timestamp: DateTime
          UserId: string }

  so now a workflow-specific command can be defined as

      type PlaceOrder = Command<UnvalidatedOrder>

* When an object (e.g., Order) can be in different states,
  this may be modeled by creating a type for each state,
  then combining them into a choice type

* When designing the domain model, it helps to think in terms of state machines

* Implement each state with a type, storing any data necessary inside it,
  and combine all the states into a single choice type

* A function can take the choice type and decide how to move one state
  to the next, depending on what the function does

* For functions exposed as an API (i.e., to the public), you should hide the
  dependency to services; but for functions used internally, be explicit

## Chapter 8

* One way to make sure function signatures don't lie (e.g., throw exception)
  is to restrict the input/output with a specialized type, e.g.,

      type NonZeroInteger = private NonZeroInteger of int
      // and define a smart constructor

  so a ``twelveDividedBy`` function could not have signature
  ``NonZeroInteger -> int`` instead of throwing an exception
  if the input (an ``int``) is 0

* Another way is to extend the output, such as making it an ``option``
  for output cases that don't make sense

* To build a complete application, you compose low-level functions
  to create services, then compose services to create workflows,
  and finally compose workflows in parallel to create the application

## Chapter 9

* To help with the implementation, you can define a type for the function
  signature, and define the function in terms of that new type,
  using a lambda to assign the function; this helps with type errors
  while implementing the function

* Use partial application to build "baked-in" function, i.e., functions
  with some parameters applied, so that the baked-in function only takes
  a single paramater, making it easier to use in deeper functions
