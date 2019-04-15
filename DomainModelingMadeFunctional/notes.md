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
