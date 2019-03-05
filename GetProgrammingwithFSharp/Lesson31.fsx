#r @"packages\FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

type TvListing = JsonProvider<"http://www.bbc.co.uk/programmes/genres/comedy/schedules/upcoming.json">
let tvListing = TvListing.GetSample()
let title = tvListing.Broadcasts.[0].Programme.DisplayTitles.Title

#r @"packages\Google.DataTable.Net.Wrapper\lib\Google.DataTable.Net.Wrapper.dll"
#r @"packages\XPlot.GoogleCharts\lib\net45\XPlot.GoogleCharts.dll"

open XPlot

type Films = HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">

let deNiro = Films.GetSample()
deNiro.Tables.Film.Rows
|> Array.countBy (fun row -> string row.Year)
|> GoogleCharts.Chart.SteppedArea
|> GoogleCharts.Chart.Show

type NunitHtml = HtmlProvider< @"data\sample-package.html">

let nunit = NunitHtml.Load "https://www.nuget.org/packages/nunit"
let entity = NunitHtml.Load "https://www.nuget.org/packages/entityframework"
let json = NunitHtml.Load "https://www.nuget.org/packages/newtonsoft.json"

// Doesn't work because schema has changed since publication of the book
[ entity; nunit; json ]
|> Seq.collect (fun x -> x.Tables.``Version History``.Rows)
|> Seq.sortByDescending (fun x -> x.Downloads)
|> Seq.take 10
|> Seq.map (fun x -> (x.Version, x.Downloads))
|> GoogleCharts.Chart.Column
|> GoogleCharts.Chart.Show

type DreamBand = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_songs_recorded_by_Dream_Theater">
let dreamBand = DreamBand.GetSample()
dreamBand.Tables.List.Rows
|> Seq.countBy (fun x -> x.Year)
|> Seq.sortByDescending (fun (year, _) -> year)
|> GoogleCharts.Chart.Line
|> GoogleCharts.Chart.Show