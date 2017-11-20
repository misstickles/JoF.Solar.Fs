namespace JoF.Solar.Fs.WebApp

open WebSharper

module Server =
    open System
    open WebSharper.Charting

    [<Remote>]
    let DoSomething input =
        let R (s: string) = System.String(Array.rev(s.ToCharArray()))
        async {
            return R input
        }
    
    let dateRange days = 
        let startDate = DateTime.Now
        let endDate = DateTime.Now.AddDays days
        Seq.unfold (fun d -> if d < endDate then Some(d, d.AddDays 1.) else None) startDate

    [<Remote>]
    let SunRiseSet = 
        let rises = [ for d in dateRange 365. -> string d, (AstroCalcs.SunRise d 51. 0. 1.) ]
        let sets = [ for d in dateRange 365. -> string d, (AstroCalcs.SunSet d 51. 0. 1.) ]
        [
            Chart.Line(rises)
            Chart.Line(sets)
        ]
        |> Chart.Combine
        //|> Chart.WithXAxis(Title = "Date", LabelStyle = LabelStyle(Angle = -45))
        //|> Chart.WithYAxis(Title = "Time of Day")
