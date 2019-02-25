type FootballResult =
    { HomeTeam : string
      AwayTeam : string
      HomeGoals : int
      AwayGoals : int }

let create (ht, hg) (at, ag) =
    { HomeTeam = ht; AwayTeam = at; HomeGoals = hg; AwayGoals = ag }

let results =
    [ create ("Messiville", 1) ("Ronaldo City", 2)
      create ("Messiville", 1) ("Bale Town", 3)
      create ("Bale Town", 3) ("Ronaldo City", 1)
      create ("Bale Town", 2) ("Messiville", 1)
      create ("Ronaldo City", 4) ("Messiville", 2)
      create ("Ronaldo City", 1) ("Bale Town", 2) ]

// My initial attempt
results
|> List.map (fun result -> (result.AwayTeam, result.AwayGoals > result.HomeGoals))
|> List.filter (fun win -> snd win)
|> List.map (fun x -> fst x)
|> List.countBy (fun x -> x)
|> List.sortByDescending (fun x -> snd x)
|> List.map (fun x -> sprintf "%s: %d wins" (fst x) (snd x))

// Book's solution
let isAwayWin result = result.AwayGoals > result.HomeGoals

results
|> List.filter isAwayWin
|> List.countBy (fun result -> result.AwayTeam)
|> List.sortByDescending (fun (_, awayWins) -> awayWins)

// Lists
let numbers = [ 1; 2; 3; 4; 5; 6 ]
let numbersQuick = [ 1..6 ]
let head :: tail = numbers
let moreNumbers = 0 :: numbers
let evenMoreNumbers = moreNumbers @ [ 7..9 ]
