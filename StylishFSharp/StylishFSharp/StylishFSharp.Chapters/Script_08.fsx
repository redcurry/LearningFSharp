module Chapter_08 =

    type ConsolePrompt(message : string) =

        member val BeepOnError = true
            with get, set

module Exercise_08_01 =

    type Grayscale(r : byte, g : byte, b : byte) =
        member this.Level =
            (int r + int g + int b) / 3 |> byte

module Exercise_08_02 =

    type Grayscale(r : byte, g : byte, b : byte) =

        // Alternate constructor
        new (c : System.Drawing.Color) =
            Grayscale(c.R, c.G, c.B)

        member this.Level =
            (int r + int g + int b) / 3 |> byte

module Exercise_08_03 =

    type Grayscale(r : byte, g : byte, b : byte) =
        new (c : System.Drawing.Color) =
            Grayscale(c.R, c.G, c.B)

        member this.Level =
            (int r + int g + int b) / 3 |> byte

        override this.ToString() =
            sprintf "Grayscale(%i)" this.Level

module Exercise_08_04 =

    open System

    type Grayscale(r : byte, g : byte, b : byte) =

        let eq (that : Grayscale) =
            r = that.R && g = that.G && b = that.B

        new (c : System.Drawing.Color) =
            Grayscale(c.R, c.G, c.B)

        member this.R = r
        member this.G = g
        member this.B = b

        member this.Level =
            (int r + int g + int b) / 3 |> byte

        override this.ToString() =
            sprintf "Grayscale(%i)" this.Level

        override this.Equals(thatObj : obj) =
            match thatObj with
            | :? Grayscale as that -> eq that
            | _                    -> false

        override this.GetHashCode() =
            hash (r, g, b)

        interface IEquatable<Grayscale> with
            member this.Equals(that : Grayscale) =
                eq that
