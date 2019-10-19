module Houses =

    type House = { Address : string; Price : decimal }
    type PriceBand = Cheap | Medium | Expensive

    /// Make an array of 'count' random houses.
    let getHouses count =
        let random = System.Random(Seed = 1)
        Array.init count (fun i ->
            { Address = sprintf "%i Stochastic Street" (i+1)
              Price = random.Next(50_000, 500_000) |> decimal })

    let random = System.Random(Seed = 1)

    /// Try to get the distance to the nearest school.
    /// (Results are simulated)
    let trySchoolDistance (house : House) =
        // Because we simulate results, the house
        // parameter isn’t actually used.
        let dist = random.Next(10) |> double
        if dist < 8. then
            Some dist
        else
            None

    // Return a price band based on price.
    let priceBand (price : decimal) =
        if price < 100_000m then
            Cheap
        else if price < 200_000m then
            Medium
        else
            Expensive

module Exercise04_01 =

    open Houses

    let housePrices =
        getHouses 20
        |> Array.map (fun h ->
            sprintf "Address %s - Price: %f" h.Address h.Price)

module Exercise04_02 =

    open Houses

    let houseAverage =
        getHouses 20
        |> Array.averageBy (fun h -> h.Price)

module Exercise04_03 =

    open Houses

    let fancyHouses =
        getHouses 20
        |> Array.filter (fun h -> h.Price > 250_000m)

module Exercise04_04 =

    open Houses

    let housesNearSchools =
        getHouses 20
        |> Array.choose (fun h ->
            match h |> trySchoolDistance with
            | Some d -> Some (h, d)
            | None -> None)

module Exercise04_05 =

    open Houses

    getHouses 20
    |> Array.filter (fun h -> h.Price > 100_000m)
    |> Array.iter (fun h ->
        printfn "Address: %s Price: %f" h.Address h.Price)

module Exercise04_06 =

    open Houses

    getHouses 20
    |> Array.filter (fun h -> h.Price > 100_000m)
    |> Array.sortByDescending (fun h -> h.Price)
    |> Array.iter (fun h ->
        printfn "Address: %s Price: %f" h.Address h.Price)

module Exercise04_07 =

    open Houses

    let houseAverage =
        getHouses 20
        |> Array.filter (fun h -> h.Price > 200_000m)
        |> Array.averageBy (fun h -> h.Price)

module Exercise04_08 =

    open Houses

    let exercise =
        getHouses 20
        |> Array.filter (fun h -> h.Price < 100_000m)
        |> Array.pick (fun h ->
            match trySchoolDistance h with
            | Some d -> Some (h, d)
            | None -> None)

module Exercise04_09 =

    open Houses

    let exercise =
        getHouses 20
        |> Array.groupBy (fun h -> priceBand h.Price)
        |> Array.map (fun (g, a) -> (g, a |> Array.sortBy (fun h -> h.Price)))