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

## Chapter 4: Collection Functions

The `Seq` module works on the IEnumerable type.

Most collection functions return a new collection,
except `iter`, which does the desired operation but returns `unit`.

Function `choose` applies the given function,
and returns those that were `Some` (but doesn't return them in a `Some`).
Function `pick` is similar but returns the first value that is `Some`.

Function `sub` slices an array, similar to `array.[3..5]`.

Function `truncate` is the same as Haskell's `init`.

Function `init` creates a collection using the provided function.

Function `partition` splits a collection into two depending
on the given function, which returns `true` or `false`.

Partial collection functions are those that may cause an exception
(e.g., List.head on an empty list).
Think carefully whether the given input could cause an exception.

There are many collection functions that have `try` equivalents
(return `None` when there's no return value, `Some` when there is.

If a partial function doesn't have a `try` equivalent,
you can write your own.

Other modules where collection functions are found:
`Array2D`, `Array3D`, `Array4D`, `Map`, and `Set`.

Instead of `|> Array.map (fun x -> doSomething x)`, just use
`|> Array.map doSomething`.

### Miscellaneous

An underscore in a large number (`200\_000`), helps as a visual aid.

## Chapter 5

In F#, there's no `break` keyword to break out of a loop.

`ResizeArray` in F# is equivalent to `System.Collections.Generic.List`.

Use `Seq.cast<'a>` to convert a non-generic `IEnumerable`
to the generic `IEnumerable<T>`.

Use `seq { }` to return a sequence with what's inside.
Use `yield` to return the next item in the sequence.
Use `yield!` to return the elements of another sequence, one by one,
which may use a recursive call to the function.

Be aware of `Seq.isEmpty`.

Use the *acc-elem* phrase to remember the order
in which the acculumator and element go in a `fold` function,
e.g., `Seq.fold (fun acc elem -> acc * elem) 1`.

## Chapter 6

You can group several cases in a `match` expression to perform the same code.

If you follow a matched case with `as x`, the variable `x`
will have the actual value that was matched (such as in a group of cases).

You can use `when` in a match expression, e.g.,

    match number with
    | 1             -> "One"
    | 2             -> "Two"
    | x when x < 12 -> "Less than a dozen"
    | x when x = 12 -> "A dozen"
    | _             -> "More than a dozen"

You can pattern match a record in the function argument
(and don't need to specify all fields, just the ones you want), e.g.,

    let formatMenuItem ({ Title = title; Artist = artist }) =
        sprintf "%s - %s" title artist

Recommended to include labels for discriminated unions (DUs)
with a payload during declaration, construction, and decomposition, e.g.,

    // Declaration
    type MeterReading =
    | Standard of int
    | Economy7 of Day:int * Night:int

    // Construction
    let reading = Economy7(Day=3244, Night=98218)

    // Decomposition
    match reading with
    | Economy7(Day=day; Night=night) -> ...

In decomposition, the separator is a semicolon rather than a comma
because you can omit parameters you don't need.

You can use single case DUs to model simple types, e.g.,

    type Complex = Complex of Real:float * Imaginary:float

DUs can also be pattern matched in a function declaration,
but don't overdo it, as non-single case DUs can be even more confusing.

You can do pattern matching directly in a `let` binding,
so that `real` and `imaginary` will have the corresponding values:

    let c = Complex(Real=0.2, Imaginary=3.4)
    let (Complex(real, imaginary)) = c

    // Or using the labels (but you must use a semicolon)
    let (Complex(Real=real; Imaginary=imaginary)) = c

A DU can be made to behave like C# `enum` flags,
using the `[<Flags>]` attribute, but this is used in rare cases.

You can define and use a single case active pattern as follows:

    let (|Currency|) (x : float) =
        Math.Round(x, 2)

    match 100./3. with
    | Currency 33.33 -> true
    | _              -> false

Above, the value `100./3.` will be passed to `Currency`,
and the result will be compared to `33.33` to test the match.

In a multi-case active pattern, it would return one
of the cases (up to 7) defined by the pattern, e.g.,:

    let (|Mitsubishi|Samsung|Other|) (s : string) =
        let m = Regex.Match(s, @"([A-Z]{3})(\-?)(.*)")
        if m.Success then
            match m.Groups.[1].Value with
            | "MWT" -> Mitsubishi
            | "SWT" -> Samsung
            | _     -> Other
        else
            Other

In a partial active pattern, you can match by a case
or by no cases:

    let (|Mitsubishi|_|) (s : string) =
        let m = Regex.Match(s, @"([A-Z]{3})(\-?)(.*)")
        if m.Success then
            match m.Groups.[1].Value with
            | "MWT" -> Some s
            | _     -> None
        else
            None

    turbines
    |> Seq.iter (fun t ->
        match t with
        | Mitsubishi m -> printfn "%s is a Mitsubishi turbine" m
        | _ as s       -> printfn "%s is not a Mitsubishi turbine" s)

Notice that even though the active pattern "function" above
may return `Some`, this is matched with `Mitsubishi`.

Active patterns may have parameters, passed before the primary input.

Patterns can be compined with `&` in a pattern match.

When working with classes, you can cast an object of one type
to another, using `:>`.

You can pattern match on the type of an object, using `:?`, e.g.,

    match person with
    | :? Child as child -> // ...
    | _ as person       -> // ...

You can pattern match on `null` directly:

    match s with
    | null -> "(NONE)"
    | _    -> s.ToUpper()

## Chapter 7: Record Types

To create a copy of a record with a field replaced, use `with`:

    let myRecord =
        { String = "Hello"
          Int = 99 }

    let myRecord2 =
        { myRecord with String = "Hi" }

Use the `[<CLIMutable>]` attribute on a record when you need
the record to have a default constructor and getters/setters
(used by external code, such as deserialization).

In F# (and C#), classes have referential equality
(equal if they represent the same object in memory).

Record types have structural equality, except when any of its fields
has referential equality.

F# has the attributes `ReferenceEquality`, `NoEquality`,
and `NoComparison` to change the equality behavior of records.

Declaring a record as a `struct` with the `[<Struct>]` attribute
may improve performance (in some cases).

Records can be generic:

    type LatLon<'T> =
        { Latitude : 'T
          Longitude : 'T }

    // Automatic type inference to LatLon<float>
    let waterloo =
        { Latitude = 51.3021
          Longitude = -0.1132 }

    // Manual type specification
    let waterloo : LatLon<float> =
        { Latitude = 51.3021
          Longitude = -0.1132 }

Records may be recursive, but be careful to create circular
hierarchies (it is possible using `let rec` and `and`).

Records may have instance and static methods
(defined using `with` keyword).

A common method to override in a record is `ToString()`,
which is called by `printfn` using the `%O` format specifier.

### Miscelaneous

Use `#time "on"` and `#time "off"` to do performance checks.
