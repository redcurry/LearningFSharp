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