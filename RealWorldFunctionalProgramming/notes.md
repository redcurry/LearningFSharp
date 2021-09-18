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
