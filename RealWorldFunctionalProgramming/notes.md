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
