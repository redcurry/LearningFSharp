namespace DreamTheater.Api

module Songs =

    open FSharp.Data

    type Song =
        { Title : string
          Album : string
          Year : int }

    type private DreamBandProvider = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_songs_recorded_by_Dream_Theater">

    let GetSongs () : Song seq =
        let dreamBandData = DreamBandProvider.GetSample().Tables.List
        dreamBandData.Rows
        |> Seq.map (fun row ->
            { Title = row.Title
              Album = row.Album
              Year = row.Year })

