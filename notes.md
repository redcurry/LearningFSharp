# Real-World Functional Programming

## Chapter 3

* An expression ending in semicolon is evaluated before the next expression
  (a semicolon is not needed in "lightweight" syntax, which is the default)

* Access the first and second elements of a tuple with "fst" and "snd"

* Recursive functions must be specified with the keyword "rec"

## Chapter 4

* When calling a function that returns a value that's not needed,
  wrap the call with "ignore"

* When a function has a unit type as parameter (e.g., randomBrush()),
  the function is evaluated each time it is called,
  which means it can return a different result each call
  (i.e., it has side effects)

* One can write "let add(a, b) = a + b" or "let add a b = a + b".
  The first form is a tuple parameter, which is consistent
  with interacting with .NET methods. The second form makes sense
  when working only with F#.

* When doing pattern matching, [x, y] is the same as [(x, y)]

## Chapter 5

* When calling a C# function with an "out" parameter,
  you can call it from F# as if the function return a tuple, e.g.,
  let (success, parsed) = Int32.TryParse("41")

* An (int, string) tuple is represented by the "int * string" type.

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

* Optional values (like Maybe in Haskell) can be made using
  Same(x) and None (just like Just x and Nothing).

* These two definitions are equivalent:

      let square1 a = a * a            // regular function
      let square2 = fun a -> a * a     // lambda function

* Lamba functions can have more than one parameter:

      let add = fun a b -> a + b

* F# supports partial function application:

      let add a b = a + b
      List.map (add 10) [1 .. 10]    // (add 10) returns a function
