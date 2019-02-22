// 9.2 Exercise
let parse (person:string) =
    let tokens = person.Split()
    let playername, game, score = tokens.[0], tokens.[1], tokens.[2]
    let scoreInt = System.Int32.Parse(score)
    playername, game, scoreInt