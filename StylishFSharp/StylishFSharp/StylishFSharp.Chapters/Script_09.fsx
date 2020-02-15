module Exercise_09_01 =

    let multiply a b = a * b

    let applyAndPrint f a b =
        let r = f a b
        printfn "%i" r

    applyAndPrint multiply 2 3

    // Subtract without naming the function
    applyAndPrint (fun a b -> a - b) 2 3
    applyAndPrint (-) 2 3

module Exercise_09_02 =

    let rangeCounter start stop =
        let mutable current = start
        fun () ->
            let this = current
            current <-
                if current = stop then
                    start
                else
                    current + 1
            this

    let c = rangeCounter 3 6
    for _ in 1..10 do
        printfn "%i" (c())

module Exercise_09_03 =

    let featureScale a b xMin xMax x =
        a + ((x - xMin) * (b - a)) / (xMax- xMin)

    let scale (data : seq<float>) =
        let minX = data |> Seq.min
        let maxX = data |> Seq.max
        let zeroOneScale = featureScale 0. 1. minX maxX
        data
        |> Seq.map zeroOneScale

    [100.; 150.; 200.]
    |> scale

module Exercise_09_04 =

    let pipeline =
        [ fun x -> x * 2.
          fun x -> x * x
          fun x -> x - 99.9 ]

    let applyAll (p : (float -> float) list) =
        p |> List.reduce (>>)

    // Or more compact
    let applyAll2 =
        List.reduce (>>)

    100. |> applyAll pipeline
    100. |> applyAll2 pipeline
