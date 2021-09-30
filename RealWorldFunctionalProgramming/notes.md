# Real-World Functional Programming

## Chapter 3

* To be explicit about scope, use the `in` keyword:

      let number = 42 in printfn $"{number}"

* Function definitions can be nested, and the nested function
  has access to the values of the parent function.

* To declare and use a mutable value:

      let mutable n = 10  // declaration
      n <- 11             // re-assignment

* Access the first and second elements of a tuple with "fst" and "snd"

* Recursive functions must be specified with the keyword "rec"

## Chapter 4

* When calling a method on a value in F# (e.g., `String.Split`),
  the type of the value (e.g., `data` below) must be specified:

      let convert (data : string) = data.Split(',')

* Common numeric types in F#: `int`, `uint32`, `int16`, `uint16`,
  `int64`, `uint64`, `float`, `float32`, `sbyte`, `byte`,
  `decimal`, and `bigint`.
  They usually have conversion functions with the same name.

* F# shortcut for the equivalent C# `TryParse`:

      let (success, number) = Int32.TryParse(str)

* In F# you can use a special syntax to create and initialize
  the properties of an object (similar to the C# initializer syntax):

      let form = new Form(Width = 620, Height = 450)

  If the constructor has required parameters, you can specify those first.

* Ignoring the returned value of a function (i.e., called like a statement)
  produces a warning (except when the value is `unit`).
  To stop the warning, wrap the function call with `ignore`,
  which returns `unit`.

* To define a function without parameters but with side-effects,
  it must have `unit` as its only parameter; otherwise,
  it will be treated as a value (and therefore evaluated only once).

* One can write "let add(a, b) = a + b" or "let add a b = a + b".
  The first form is a tuple parameter, which is consistent
  with interacting with .NET methods. The second form makes sense
  when working only with F#.

* In a `match` expression involving statements with side-effects,
  a branch can return `()` (`unit`) to indicate nothing should be done.

* When doing pattern matching, [x, y] is the same as [(x, y)]

## Chapter 5

* When calling a C# function with an `out` parameter,
  you can call it from F# as if the function return a tuple:

      let (success, parsed) = Int32.TryParse("41")

* The `*` notation to specify tuple types is deliberate
  because the type it creates (e.g., `int * string`) represents
  the domain of all possible combinations of its constituent types

* Create a "discriminated union" like this:

      type Schedule =
          | Never
          | Once of DateTime
          | Repeatedly of DateTime * DateTime

  then use it for pattern matching:

      match schedule with
      | Never -> ...
      | Once(eventDate) -> ...
      | Repeatedly(startDate, interval) -> ...

* You can hide a value by defining another value with the same name:

      let a = 5
      let a = a + 2    // hides a above

* In functional programming, adding new functions to an existing type
  is easier than in OOP, and all the code for that functionality
  is in that function. In OOP, adding new types is easier (via inheritance),
  and all the code for a single type is in one place.

* Optional values (like Maybe in Haskell) can be made using
  `Same(x)` and `None` (just like `Just x` and `Nothing` in Haskell).

* When defining generic types, the type parameter must start
  with an apostrophe:

      type Option<'T> =
      | Some of 'T
      | None

  and the definition is equivalent to putting the type paramater in front:

      type 'T Option =
      | Some of 'T
      | None

* In F# you can create a generic value (which doesn't exist in C#)
  by partially specifying a type:

      let n = None   // n has type 'a option, where `a is a type parameter

  and it can be used:

      n = None          // returns true
      n = Some 123      // returns false
      n = Some "Hello"  // returns false

* These two definitions are equivalent:

      let square1 a = a * a            // regular function
      let square2 = fun a -> a * a     // lambda function

* Lamba functions can have more than one parameter:

      let add = fun a b -> a + b

  and the C# equivalent:

      Func<int, int, int> add = (a, b) => a + b;

* In F#, a function can return another function,
  and in C# a method can return a delegate:

      let adder n = fun a -> a + n                        // F#
      Func<int, int> Adder(int n) { return a => a + n; }  // C#

* F# supports partial function application:

      let add a b = a + b
      List.map (add 10) [1 .. 10]    // (add 10) returns a function

## Chapter 6

* You can create generic functions in F# and generic methods in C#:

      // F#, type is 'a -> ('a -> bool) -> ('a -> string) -> unit
      let condPrint value test format =
          if (test value) then printfn "%s" (format value)

      // C#
      void CondPrint<T>(T value, Func<T, bool> test, Func<T, string> format)
      {
          if (test(value)) Console.WriteLine(format(value));
      }

* Example of defining a custom (infix) operator in F#:

      let (+>) a b = a + "\n>> " + b

  The operator must be enclosed in parenthesis, and can be any of the symbols
  `+/-*<>&|=$%.?@^~!`.
  A prefix operator must start with `~` or `!`.

* Can use pipelining operator (reverse, then take the head):

      [1 .. 5] |> List.rev |> List.head

* Functions to change first or second member of a tuple:

      let mapFirst f (a, b) = (f(a), b)
      let mapSecond f (a, b) = (a, f(b))

  which can then be used like:

      ("Prague", 1000) |> mapSecond ((+) 2000)  // uses partial application

  In C#, this can be written and used as extension methods:

      public static Tuple<B, C> MapFirst<A, B, C>
          (this Tuple<A, C> t, Func<A, B> f)
      {
          return Tuple.Create(f(t.Item1), t.Item2);
      }

      Tuple.Create("Prague", 1000).MapSecond(n => n + 2000);

* When creating your own types, such as a discriminated union,
  it could be helpful to write your own map function for it.

* `Option.map` takes a function and an `option` and returns
  an `option` with the function applied to the containing value.

* `Option.bind` is similar to `Option.map`, but the given function
  must return an `option` rather than a plan value.

* Using function composition, the following:

      places |> List.map (fun (_, p) -> status p)

  can be written as

      places |> List.map (snd >> status)

* Type inference is processed in order in F#. For example,

      Option.map (fun dt -> dt.Year) (Some DateTime.Now)

  fails because it doesn't know the type of `dt` when encountered. But

      Some DateTime.Now |> Option.map (fun dt -> dt.Year)

  works because when `dt` is encountered later
  it knows which type it should be.

* Recursive discriminated unions are a common way to represent data.

* The following:

      let names =
          places |> List.filter (fun (_, pop) -> 1000000 < pop)
                 |> List.map fst

  can be simplified to

      let names =
          places |> List.filter (snd >> ((<) 1000000))
                 |> List.map fst

  but it's a bit harder to read, so always consider readability.

* Using C# queries and F# sequence expressions, the above
  could also be written as:

      // C#
      var names = from p in places
                  where 1000000 < p.Population
                  select p.Name

     // F#
     var names =
         seq { for (name, pop) in places do
                   if (1000000 < pop) then yield name }

* You can use a tuple as in initial value in a `fold` to store
  a temporary value, and then discard it in the end:

      places
      |> List.fold (fun (b, str) (name, _) ->
             let n = if b then name.PadRight(20) else name + "\n"
             (not b, str + n)
         ) (true, "")          // true is the temp value
      |> snd                   // snd discards it

* The same code as above can be written in C# using an anonymous type:

      places.Aggregate(new { StartOfLine = true, Result = "" },
      (r, pl) => {
          var n = b.StartOfLine ? pl.Name.PadRight(20) : (pl.Name + "\n");
          return new { StartOfLine = !r.StartOfLine, Result = r.Result + n };
      }).Result;

* `List.collect` applies a function (which returns a list) to each element
  and concatenates all the lists. It is similar to `SelectMany` in LINQ.

## Chapter 7

* In functional programming, it's common to represent the same data
  with multiple data structures, and write transformations between them.

* Instead of `let`, use the `use` keyword to automatically dispose
  of the declared variable (before the function returns).

* Write functions using data structures that make the code easy to write,
  where the data may need to be transformed from some other data structure.

* Write functions such that their parameters are the minimum needed,
  both in number and in type.

* Such "minimal" functions may need to be passed to higher-order functions
  that create and clean-up the necessary context for those functions.
  This is called the "Hole in the Middle" pattern.

* A discriminated union can recursively reference itself:

      type DocumentPart =
        | SplitPart  of Orientation * list<DocumentPart>
        | TitledPart of TextContent * DocumentPart
        | TextPart   of TextContent
        | ImagePart  of string

* You can handle multiple values at once with one match:

      match a, b with
      | true, false  -> c
      | false, true  -> d
      | true, true   -> e
      | false, false -> f

* When creating a data structure, it may be helpful to write
  a map-like function for it (possibly recursive)
  that applies any given function to each element.
  We need to consider the kind of traversal that's needed.

* Similarly, it may be helpful to write a fold-like
  and filter-like functions for the new data structure.

* In a `match` expression, you can use an "or-pattern" where
  you can specify multiple cases separated by `|`,
  but it can only be used when both patterns bind a value
  to the same identifier with the same type:

      match part with
      | TextPart tx | TitledPart (tx, _) -> a
      | _                                -> b

* The functional equivalent of the composite design pattern (OOP) is

      type AbstractComponent
      | CompositeComponent of AbstractComponent list
      | ConcreteComponent of (...)

* The functional equivalent of the decorator pattern is

      type AbstractComponent
      | DecoratedComponent of AbstractComponent * (...)  // added state
      | ConcreteComponent of (...)

## Chapter 8

* In F#, you can store application behaviors as a list of functions.

* Point-free style: not having to assign a name to a value when
  calling a higher-order function. For example:

      [1..10] |> List.map ((+) 100)
      places |> List.map (snd >> statusByPopulation)
      tests |> List.filter ((|>) client) // same as (fun f -> client |> f)

  But, as the last example shows, it may be harder to read, so use carefully.

* The strategy pattern in OOP (where algorithms within a larger class
  can be swapped easily) can be implemented functionally by using functions.

* The command pattern in OOP can also be implemented functionally
  by using functions (the command is the function).

* In F#, you can create another kind of mutable value using a reference cell.
  It is a small object that contains a mutable value, which can be assigned
  (using `:=`) and accessed (using `!'). For example,

      let st = ref 10
      st := 11
      printfn "%d" (!st)

* Using a closure, you can capture a `ref` value and have the ability
  to change it in one function and use it in another
  while sharing the same value:

      let createIncomeTest() =
          let minimalIncome = ref 30000
          (fun newMinimal -> minimalIncome := newMinimal),    // tuple
          (fun client -> client.Income < !minimalIncome)

      let (setMinimalIncome, testIncome) = createIncomeTest()

      testClient(john)
      setMinimalIncome(45000)   // changes the ref value
      testClient(john)          // uses the changed value

* Related functions may be grouped in a tuple (as above) or in a record:

      type ClientTest =
        { Check  : Client -> bool
          Report : Client -> unit }

  This is especially beneficial when creating a collection of function groups
  because they can be called in the same way.

* A decision tree is a flowchart of questions and answers, where some answers
  lead to additional questions and other answers lead to a final decision.

* Mutually recursive types need to be defined using `and`:

      type QueryInfo =
        { Title    : string
          Check    : Check -> bool
          Positive : Decision
          Negative : Decision }

      and Decision =
        | Result of string
        | Query  of QueryInfo

* When defining a value using `let`, you can also use `and` to define
  values based on others defined later. The `rec` keyword is also necessary:

      let rec form = createForm "Main form" [ btn ]
      and btn = createButton "Close" (fun () -> form.Close())

* You can mix OOP with functional concepts by having a class
  contain a property that is a `Func`, so its behavior changes
  based on the assigned lambda, and not having to create derived types.

## Chapter 8

* Example of adding a method to a record in F# (but can also add to
  any data type, including discriminated unions):

      type Rect =
        { Left   : float32
          Top    : float32
          Width  : float32
          Height : float32 }

        member x.Deflate(wspace, hspace) =
          { Left   = x.Left + wspace
            Top    = x.Top + hspace
            Width  = x.Width - (2.0f * wspace)
            Height = x.Height - (2.0f * hspace) }

    where `x` refers to the current instance. It may be named
    something else, like `this` or `self`.

* When intending to expose methods to C#, use tuples as parameters.

* Use `///` to write a summary of the method to be used by IntelliSense.

* You can add members after the original type has been defined using `with`:

      type Schedule with
          member x.GetNextOccurrence() = ...
          member x.OccursNextWeek = ...         // property

  The original type can even be in another assembly.

* One benefit of defining methods after the original type is that
  there can be utility functions between the type and the methods
  that are used by the methods.

* You can declare a type that behaves like an interface in F#
  (and looks like an interface from C#):

      type ClientTest =
          abstract Check : Client -> bool
          abstract Report : Client -> unit

* You can implement this interface on the fly using object expressions:

      let testCriminal =
        { new ClientTest with
              member x.Check(cl) = cl.CriminalRecord = true
              member x.Report(cl) =
                  printfn "'%s' has a criminal record!" cl.Name }

* An example of using object expressions is when working with the
  `Dictionary` class of .NET. You can create a value that implements
  `IEqualityComparer<T>` and pass that to the `Dictionary` constructor.

* You don't need to specify the `T` type of .NET generics and instead
  use `\_`. The F# compiler will figure out the type automatically.

* Normally, the `use` keyword disposes the value when the function
  exists. But one can dispose of a value earlier by using `use`
  instead of a `let`:

      let text =
          use reader = new StreamReader("C:\\test.txt")
          reader.ReadToEnd()
      Console.Write(text)        // reader has been disposed

* Another use of object expressions is when implementing `IDisposable`
  to use with the `use` keyword:

      let changeColor(clr) =
          let orig = Console.ForegroundColor
          Console.ForegroundColor <- clr
          { new IDisposable with
                member x.Dispose() =
                    Console.ForegroundColor <- orig }

  When `changeColor` is called in a `use` expression, the console's color
  will be changed immediatelly, and the disposable part will be called
  when the containing function exists, restoring the console's color.

* You can define a class with a constructor in F#:

      type ClientInfo(name, income, years) =
          let loanCoefficient = income / 5000 * years  // constructor

          member x.Name = name                         // properties
          member x.Income = income

          member x.Report() =      // method (can use loanCoefficient)
              printfn "Loan coefficient: %d" loanCoefficient

* In the functional style, the class should be immutable,
  where changing a property returns a new object with the property changed:

      type Client(name, income) =
          member x.WithIncome(v) =
              new Client(name, v)

  In the imperative style, you can define a mutable field
  and a getter and setter for a property:

      type Client(name, income) =
          let mutable income = income    // hides original income

          member x.Income
              with get()  = income
              and  set(v) = income <- v

* To implement an interface:

      type CoefficientTest(minValue, ...) =

          let coeff(client) = ...
          let report(client) = ...

          interface ClientTest with
              member x.Check(client) = coeff(client) < minValue
              member x.Report(client) = report(client)

* In F#, the implementation methods must be used with the interface type,
  not with the derived class, so we will need to upcast:

      let test = new CoefficientTest(...)
      let clTest = (test :> ClientTest)     // upcast
      clTest.Report(...)

  In order to have class methods (not interface methods) that implement
  the interface, create normal members in addition to the interface methods.

* The downcast operator is `:?>`, and the equivalent of `is` (C#) is `:?`,
  such as `obj :? String`.

* When accessed from C#, a record in F# appear as a class, its fields
  will appear as properties, and its members will appear as methods.
  A constructor that takes all the initial field values will also be
  automatically generated.

* To make an F# code file accessible from C#, include a `namespace` at the top.

* To expose values and functions to C#, wrap the code in a `module`,
  which is seen as a static class in C#.

* To expose a higher-order function that takes another function,
  use `Func` as a parameter:

      let WithIncome (f:Func<_, _>) client =
        { client with Income = f.Invoke(client.Income) }

## Chapter 10

* In project properties (Build), the "Generate tail calls" option
  uses the `tailcall` IL instruction to optimize tail call recursion.
  It is off in the Debug configuration by default.

* The following recursive function to sum a list of numbers is inefficient:

      let rec sum lst =
          match lst with
          | []    -> 0
          | x::xs -> x + sum xs

  This is because the `x` value needs to be stored in order to perform
  the sum after each recursive call returns.

* It's better to write functions using tail call recursion,
  where no computation happens after each recursive call.
  This way, the compiler can avoid creating a stack for each call.
  The above example can be re-written to use tail call recursion:

      let sum lst =
          let rec sumUtil lst n =
              match lst with
              | []    -> n
              | x::xs -> sumUtil xs (x + n)
          sumUtil lst 0

* To cache results in a function, so that it doesn't recalculate
  a computation with the same input, use the memoization technique:

      let add' a b = a + b             // normal function

      let add =
          let cache = new Dictionary<_, _>()
          fun a b ->
              match cache.TryGetValue((a, b)) with
              | true, result -> result
              | _            ->
                  let result = add' a b
                  cache.Add((a, b), result)
                  result

* Memoization can be generalized to apply to any function:

      let memoize f =
          let cache = new Dictionary<_, _>()
          fun x ->
              match cache.TryGetValue(x) with
              | true, result -> result
              | _            ->
                  let result = f x
                  cache.Add(x, result)
                  result

  But be careful when using with recursive functions because
  the recursive call needs to use the memoized function:

      let rec factorial = memoize (fun x ->
          if (x <= 0) then 1 else x * factorial (x - 1))

* Another example of using tail recursion:

      let filter f list =
          let rec filter' f list acc =
              match list with
              | []   -> List.rev acc
              | x:xs -> let acc = if (f x) then x::acc else acc
                        filter' f xs acc
          filter' f list []

* Use the `#time` directive in F# Interactive to measure timing.

* Prepending an element to a list is O(1) whereas appending is O(N).

* To use arrays in a functional style, instead of mutating the array
  directly, write a function that returns a new array.
  Internally, the function will need to mutate the new array,
  but this is not visible to the outside, so it is OK.

* Most LINQ operations in C# don't return arrays, but the `System.Array`
  class has static methods that work with arrays functionally.
  For example, the map operation is under the name `ConvertAll`.

* Hide the `Select` method in LINQ with one that processes arrays:

      public static R[] Select<T, R>(this T[] array, Func<T, R> map)
      {
          var result = new R[arr.Length];
          for (int i = 0; i < array.Length; i++)
              result[i] = map(array[i]);
          return result;
      }

* Consider a tree data structure:

      type IntTree =
        | Leaf of int
        | Node of IntTree * IntTree

  and a recursive function to sum all elements:

      let rec sumTree tree =
          match tree with
          | Leaf n           -> n
          | Node left, right -> sumTree left + sumTree right

  The above implementation is inefficient (produces stack overflow)
  on imbalanced trees because it does work after a recursive call.

  It can be rewritten using the "continuation" technique,
  which uses tail recursion and is more efficient:

      let rec sumTree tree cont =
          match tree with
          | Leaf n           -> cont n
          | Node left, right ->
              sumTree left (fun leftSum ->
                  sumTree right (fun rightSum ->
                      cont(leftSum + rightSum)))

      // Use the identity function to get the final sum
      sumTree myTree id

## Chapter 11

* If a higher-order function takes a function that returns a
  type annotated with `#`, such as `#seq<int>`, it means that
  you can pass that higher-order function a function that
  returns a derived type (a `list<int>` in the example).

* One refactoring is to create a function with the differing code
  to a higher-order function that has the boiler-plate code.

* Put unit tests in their own module to keep them separete
  from the main program.

* My note: For unit tests to run properly in Visual Studio using NUnit,
  the project must target .NET Core or Framework (not .NET Standard).
  Also, install packages NUnit, NUnit3TestAdapter, and
  Microsoft.NET.Test.Sdk. Example test:

       module UtilTests =

           [<Test>]
           let applyn_ten() =
               let result = applyn 10 ((+) 1) 0
               Assert.That(result, Is.EqualTo([1..10]))

* F# automatically generates comparison and equality methods
  for immutable types, like records, tuples, and discriminated unions,
  but not for classes.

* Evaluations in F# are eager: arguments of a function are evaluated
  before executing the function.

* In F#, you can create a lazy operation with the `lazy` keyword,
  and force the computation (and extract its value) by accessing the
  `Value` property:

      let n = lazy (foo 10)
      n.Value

  The type returned by `lazy` is `Lazy<'a>`.

* There's an implentation of a lazy list in the FSharp.PowerPack.dll
  library as `LazyList<'a>` (but it seems it's in the FSharpx library).

* An additional advantage of using lazy evaluation, is that the computation
  is cached, so calling the `Value` on the lazy value a second time
  does not compute it again; it returns the cached value.

## Chapter 12

* Example of `Seq.unfold` to create a sequence of numbers as strings:

      let nums = Seq.unfold (fun num ->
          if (num <= 10) then Some(string(num), num + 1) else None) 0

  `None` ends the sequence. `Some` contains a tuple where the first value
  is what to put next in the sequence, and the second value is what to send
  to the function for the next element of the sequence.

* The F# type `seq<int>` is equivalent to `IEnumerable<int>`.

* In C#, you can generate `IEnumerable` using iterators:

      static IEnumerable<string> Factorials()
      {
          int factorial = 1;
          for (int num = 0; factorial < 1000000; num++)
          {
              factorial *= num;
              yield return $"{num}! = {factorial}";
          }
      }

  To end the sequence, use `yield break`.

* Example of a sequence expression in F#:

      let nums =
          seq { let n = 10
                yield n + 1
                yield n + 2 }

  Sequences are evaluated lazily. Use `yield!` to yield another sequence,
  element by element.

* The F# version of the factorial example above:

      let factorials =
          let rec factorialsUtil num factorial =
              seq { if factorial < 1000000 then
                        yield $"{num}! = {factorial}"
                        let num = num + 1
                        yield! factorialsUtil num (factorial * num) }
          factorialsUtil 0 1
