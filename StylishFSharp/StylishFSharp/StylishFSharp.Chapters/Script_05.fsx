module Exercise_05_01 =

    let clip c (s : seq<'a>) =
        s
        |> Seq.map (fun x -> min x c)

    open System

    let extremes_mutable (s : seq<float>) =
        let mutable min = Double.MaxValue
        let mutable max = Double.MinValue
        for item in s do
            if item < min then
                min <- item
            if item > max then
                max <- item
        (min, max)

    let extremes (s : seq<float>) =
        (Seq.min s, Seq.max s)

    let then1 = DateTime.Now
    let e = extremes_mutable (seq { 1. .. 100000000. })
    let time1 = DateTime.Now - then1

    let then2 = DateTime.Now
    let e2 = extremes (seq { 1. .. 100000000. })
    let time2 = DateTime.Now - then2
