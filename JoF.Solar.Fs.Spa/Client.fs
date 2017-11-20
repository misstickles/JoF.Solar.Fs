namespace JoF.Solar.Fs.Spa

open WebSharper

[<JavaScript>]
module Client =
    open System
    open WebSharper.Charting
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client
    open JoF.Solar.Fs.Library

    let dateRange days = 
        let startDate = DateTime.Now
        let endDate = DateTime.Now.AddDays days
        Seq.unfold (fun d -> if d < endDate then Some(d, d.AddDays 1.) else None) startDate

    //let rises = [ for d in dateRange 365. -> string d, (AstroCalcs.SunRise d 51. 0. 1.) ]
    //let sets = [ for d in dateRange 365. -> string d, (AstroCalcs.SunSet d 51. 0. 1.) ]
    let rises = [ for d in dateRange 365. -> string d, float d.Date.Day ]
    let sets = [ for d in dateRange 365. -> string d, float d.Month ]

    let chart = 
        [
            Chart.Line(rises)
            Chart.Line(sets)
        ]
        |> Chart.Combine
        |> fun e ->
            let c = ChartJs.CommonChartConfig()
            Renderers.ChartJs.Render(e, Size=Size(500, 500), Config = c)

    [<SPAEntryPoint>]
    let Main () =
        div [
            chart
        ]
        |> Doc.RunById "main"
