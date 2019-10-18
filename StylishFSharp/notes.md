# Stylish F#

## Chapter 3: Missing Data

Generic discriminated union:

    type Shape<'T> =
    | Square of height:'T
    | Rectangle of height:'T * width:'T
    | Circle of radius:'T

### The `Option` Type

Using pattern matching on an option type:

    let address =
        match delivery
        | Some s -> s
        | None   -> billing

Using the `Option` module, this is the same as

    let address = Option.defaultValue billing delivery

so that if `delivery` is `None`, it evaluates to `billing`.
This is also the same as

    let address = delivery |> Options.defaultValue billing

`Option.iter` performs the given function (first argument)
if the given value (second argument) is `Some`;
otherwise, it doesn't do anything.

`Option.map` performs the given function if the given value is `Some`,
and returns the result as a `Some`.
If the value is `None`, it returns `None`.

`Option.bind` is similar to `Option.map`,
except that the given function must return an `Option`.

Combine `Option.map` and `Option.bind` to apply functions
to a value, where each function may (use `Option.bind`)
or may not (use `Option.map`) return an `Option`.

### Model Design

Here is a simple model:

    type BillingDetails = {
        name : string
        billing : string
        delivery : string option }

But if we know more about the delivery address,
we can model `BillingDetails` better like this:

    type Delivery =
    | AsBilling
    | Physical of string
    | Download

    type BillingDetails = {
        name : string
        billing : string
        delivery : Delivery }

Now we explicitly describe why the delivery address
may not be there (it's a download).

Here's an example using it:

    let tryDeliveryLabel (billingDetails : BillingDetails) =
        match billingDetails.delivery with
        | AsBilling ->
            billingDetails.billing |> Some
        | Physical address ->
            address |> Some
        | Download -> None
        |> Option.map (fun address ->
            sprintf "%s\n%s" billing.name address)

### Dealing with NULL

`Option.ofObj` takes a reference type and returns `Some`
if it's not null or `None` if it's null.
Use this at the boundaries of the application.

Use `Option.toObj` if you need to convert an option type
into a reference type that may be null.

### Recommendations

Start modeling using Discriminated Unions,
but if it becomes too complicated, try option types.

Don't expose option types or DUs in APIs
that may be consuming in C#.

### Miscellaneous

`sprintf` returns a string, whereas `printf` prints it out.
