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
        match delivery with
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
(return `None` when there's no return value, `Some` when there is).

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

## Chapter 8: Classes

F# classes can inherit from C# classes and implement C# interfaces.

When to use F# classes:

- internal and external reperesentations of data need to differ
- need to hold on to or mutate state over time
- need to interact with an object-oriented codebase like C#

A class declaration and the main constructor are one and the same:

    type ConsolePrompt(message : string) =
        ...

Above, the value `message` is available throughout the rest of the class.

Instantiate a class with our without the `new` keyword:

    let prompt = ConsolePrompt("Hello")
    let prompt2 = new ConsolePrompt("Hello")

Class members are declared inside the class like:

    member this.MemberName(parameter : type) =
        ...

Above, `this` is the convertion but can be any nonkeyword.

Members can be called recursively without the use of `rec`.

The body of the constructor can appear directly under the type declaration
(before any members are declared).

Only two kinds of operations can be done in the constructor:
`do` (imperative actions) and `let` (binding actions).

Any `let` bindings in the constructor are available throughout the class.

You can expose any value via a property definition:

    member this.Message =
        message

You can define an auto-implemented property with a default value:

    // No need for "this"
    member val BeepOnError = true
        with get, set

Additional constructors are defined with `new` and must call
the main constructor:

    // Class definition and main constructor
    type ConsolePrompt(message : string, beepOnError : bool) =
        ...

        // Additional constructor
        new (message : string) =
            ConsolePrompt(message, true)

You can define a property with getter and setter:

    type ConsolePrompt =

        let mutable foreground = ConsoleColor.White
        let mutable background = ConsoleColor.Black

    member this.ColorScheme
        with get() =
            foreground, background      // Tuple
        and set(fg, bg) =               // Tuple
            if fg = bg then
                raise <| ArgumentException(
                            "Foreground, background can't be the same")

            foreground <- fg
            background <- bg

When using mutable state, always be aware of the thread-safety implications.

Classes can be generic:

    type ConsolePrompt<'T>(message : string) =
        ...

You can name the parameters in a constructor when called:

    let prompt = ConsolePrompt(message = "Name", ...)

You can also initialize properties in the constructor parameters:

    // BeepOnError is a mutable property
    let prompt = ConsolePrompt("Name", BeepOnError = false)

You can have read-only indexed properties:

    member this.Item i =
        ...

For a mutable index property:

    member this.Item =
        with get(i) =
            ...
        and set i value =
            ...

It's possible to have a multidimensional indexed property.

### Interfaces and Abstract Classes

Define an interface as follows:

    type IMediaPlayer =
        abstract member Open : string -> unit
        abstract member Play : unit -> unit
        ...

Implement an interface as follows:

    type DummyPlayer() =

        interface IMediaPlayer with

            member this.Open(mediaId : string) =
                ...

            member this.Play() =
                ...

To access any interface members, you must cast the concrete class
to the interface, using the `:>` operator:

    let player = new DummyPlayer() :> IMediaPlayer
    player.Open("Dreamer")
    player.Play()

You can implement an interface (or inherit a class) on the fly,
without naming it, by using an object expression:

    // Interface
    type ILogger =
        abstract member Info : string -> unit
        abstract member Error : string -> unit

    let logger = {
        new ILogger with
            member this.Info(msg) = printfn "%s" msg
            member this.Error(msg) = printfn "%s" msg }

An abstract class can be defined as the following example:

    [<AbstractClass>]
    type AbstractClass() =
        abstract member SaySomething : string -> string

It must have the `[<AbstractClass>]` attribute because not all members
have default implementations.

To implement:

    type ConcreteClass(name : string) =
        inherit AbstractClass()
        override this.SaySomething(whatToSay) =
            ...

To specify a default implementation:

    type ParentClass() =
        abstract member SaySomething : string -> string
        default this.SaySomething(whatToSay) =
            ...

    // Inherit default implementation
    type ConcreteClass1() =
        inherit ParentClass()

    // Override default implementation
    type ConcreteClass2() =
        inherit ParentClass()
        override this.SaySomething(whatToSay) =
            ...

### Equality and Comparison

Classes use reference equality (not structural equality).

To properly implement structural equality in classes, you need to:

- implement interface `IEquatable<>`
- override `Equals()`
- override `GetHashCode()`
- override `op_Equality` (static member)

Creating a hash code is simple: tuple together the items
that represent equality and apply the built-in `hash` function.

You can also implement the generic and non-generic versions
if `IComparable` when necessary (e.g., using a set).

## Ch. 9: Programming with Functions

Use type hints when the compiler gets confused,
but one should be able to remove them when everything compiles.

You can hide state inside a function:

    let randomByte =
        let r = System.Random()
        fun () ->
            r.Next(0, 255) |> byte

Compose functions using `>>`.

## Ch. 10: Asyrchronous and Parallel Programming

Tip: Write the program as synchronous, and then convert the relevant parts
to asynchronous.

To learn more about async, read "F# Async Guide" by Leo Gorodinski
on medium.com.

### Pattern Matching with `function`

The `function` keyword may be used to create a pattern matching function:

    let isOk = function
        | OK _ -> true
        | Failed _ -> false

    // Which is the same as
    let isOk outcome =
        match outcome with
        | OK _ -> true
        | Failed _ -> false

### Private functions

Functions may be private to a module with the keywoard `private`:

    let private myFunc = ...

### Convert Synchronous Function to Asynchronous

To turn a synchronous function into an asynchronous function:

    1. Place the body in `async {}`
    2. Use the `Async` version of the function to use
       (hopefully provided by the third-party library)
    3. Use `let!` or `do!` (or `match!`) to call the async function above,
       and may use `Async.AwaitTask` to convert from C# task
    4. Use `return` to return the result

For example, if the original is

    let getLinks uri =
        let html = HtmlDocument.Load(uri)
        let links = html.Descendants ["a"]
        links

The async version should be

    let getLinks uri =
        async {
            let! html = HtmlDocument.AsyncLoad(uri)
            let links = html.Descendants ["a"]
            return links
        }

### The `do!` Keyword and `Async.AwaitTask`

The `do!` is used when the function doesn't return anything,
but should be called for its side-effects (most likely it's a C# function).
For example (which also shows using `Async.AwaitTask`),

    do!
        client.DownloadFileTaskAsync(fileUri, filePath)
        |> Async.AwaitTask

### Run Async Computations in Parallel

If you have several computations to run in parallel, pipe to `Async.Parallel`.
In the following example, `tryDownload` is an async computation:

    let! downloadResults =
        links
        |> Seq.map (tryDownload localPath)
        |> Async.Parallel

### Run Async Computation Synchronously

At the top level (e.g., `main`), pipe an async computation
to `Async.RunSynchronously` to run it and wait for its results
(that async computation should run whatever it needs in parallel).

### Thread-Safe Functions with `lock`

Use a lock object and `lock` to make a function thread-safe.
For example, if the original is

    let report color message =
        Console.ForegroundColor <- color
        printfn "%s" message
        Console.ResetColor()

The thread-safe version is

    let report =
        let lockObj = obj()
        fun color message ->
            lock lockObj (fun _ ->
                Console.ForegroundColor <- color
                printfn "%s" message
                Console.ResetColor())

### Batching

Run a set of async computations in parallel before running the next set:

    links
    |> Seq.map (tryDownload path)
    |> Seq.chunkBySize batchSize
    |> Seq.collect (fun batch ->
        batch
        |> Async.Parallel
        |> Async.RunSynchronously)

But may be inefficient because the next set won't start until
all runs from the previous set finish. Instead, use throttling.

### Throttling

Run many async computations with only a limited set running at once
(requires package FSharpx.Async):

    links
    |> Seq.map (tryDownload path)
    |> Async.ParallelWithThrottle 5

This example keeps the network busy without having too many downloads
fighting for limited bandwith.

### C# Tasks

To use a C# `Task`, convert it to an F# `Async` with `Async.AwaitTask`
in order to use it with `let!` or `do!`.

To expose async functions in an API, convert them to C# `Tasks`
using `Async.StartAsTask`.
