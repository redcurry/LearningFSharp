#r @"..\packages\FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"..\packages\XPlot.GoogleCharts\lib\net45\XPlot.GoogleCharts.dll"
#r @"..\packages\Google.DataTable.Net.Wrapper\lib\Google.DataTable.Net.Wrapper.dll"

open FSharp.Data
open XPlot.GoogleCharts

type Football = CsvProvider< @"..\data\FootballResults.csv">
let data = Football.GetSample().Rows |> Seq.toArray

data
|> Seq.filter (fun row -> row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
|> Seq.countBy (fun row -> row.``Home Team``)
|> Seq.sortByDescending snd
|> Seq.take 10
|> Chart.Column
|> Chart.Show