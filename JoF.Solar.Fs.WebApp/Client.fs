namespace JoF.Solar.Fs.WebApp

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Html

[<JavaScript>]
module Client =
    open WebSharper.Charting
    open WebSharper.ChartJs
    open WebSharper.UI.Next.CSharp.Client.Html
    open WebSharper.UI.Next.CSharp.Client.Html.attr
    open WebSharper.UI.Next.Client.Attr

    [<Inline "$0.getContext(\"2d\")">]
    let getContext (canvas : Dom.Node) = X<CanvasRenderingContext2D>

    let RenderChart =
        Div [
            H3 [ Text "Sun Rise and Set Times"]
            Canvas [
                Attr.Width "600"
                Attr.Height "600"
            ]
        ]
        |>! OnAfterRender(fun canvas ->
            async {
                let context = getContext canvas.Body
                let! riseSet = Server.SunRiseSet

                let (labels, data) = 
                    riseSet
                    |> Array.map(fun rs -> rs.D
            let data = 
                LineChartData(
                    Labels = labels,
                    DataSets = [| LineChartDataSet(Data = dataset) |]
                )
            Chart.Defaults.ShowTooltips <- false
            let options =
                LineChartConfiguration(
                    BezierCurve = false,
                    DatasetFill = false
                )
            chart := Some <| Chart(canvas.GetContext "2d").Line(data, options)
        )
        Div [
            Button [ Text "Pish" ]
            |>! OnClick (fun _ _ ->
                Push <| Math.Random()
            )
        ]

        let riseSet = Server.SunRiseSet
        let c = ChartJs.CommonChartConfig()
        Renderers.ChartJs.Render(riseSet, Size=Size(500, 500), Config = c)

    let Main () =
        div [
            divAttr [attr.``class`` "jumbotron"] [RenderChart]
        ]
