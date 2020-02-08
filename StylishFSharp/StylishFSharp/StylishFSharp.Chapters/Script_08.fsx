module Chapter_08 =

    type ConsolePrompt(message : string) =

        member val BeepOnError = true
            with get, set
