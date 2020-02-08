module Exercise07_01 =

    open System

    type Record =
        { X : float32
          Y : float32
          Z : float32
          DateTime : DateTime }

    [<Struct>]
    type RecordStruct =
        { X' : float32
          Y' : float32
          Z' : float32
          DateTime' : DateTime }

    let before = DateTime.Now
    List.init 10_000_000 (fun i ->
        { X = 10.0f
          Y = 20.0f
          Z = 30.0f
          DateTime = DateTime.MinValue })
    let elapsed = DateTime.Now - before

    let before' = DateTime.Now
    List.init 10_000_000 (fun i ->
        { X' = 10.0f
          Y' = 20.0f
          Z' = 30.0f
          DateTime' = DateTime.MinValue })
    let elapsed' = DateTime.Now - before

module Exercise07_04 =

    open System

    [<Struct>]
    type Position =
        { X : float32
          Y : float32
          Z : float32
          Time : DateTime }

    // Position is last to make it pipeline-friendly
    let translate dx dy dz (p : Position) =
        { p with X = p.X + dx; Y = p.Y + dy; Z = p.Z + dz }
