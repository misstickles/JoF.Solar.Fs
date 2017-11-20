// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open FSharp.Charting
open FSharp.Charting.ChartTypes

let AsTimeString (x : double) =
    let fraction n =
        let y = n - floor n
        match y with
        | z when z < 0. -> y + 1.
        | _ -> y
    
    let h = floor x
    let m = floor (60. * fraction x)
    let s = 60. * (60. * fraction x - m)

    String.Format("{00:0#}h", h) + String.Format("{00:0#}m", m) + String.Format("{00:0.000#}s", s);

let dateRange days = 
    let startDate = DateTime.Now
    let endDate = DateTime.Now.AddDays days
    Seq.unfold (fun d -> if d < endDate then Some(d, d.AddDays 1.) else None) startDate

let SunRiseSet = 
    let rises = [ for d in dateRange 365. -> d, (AstroCalcs.SunRise d 51. 0. 1.) ]
    let sets = [ for d in dateRange 365. -> d, (AstroCalcs.SunSet d 51. 0. 1.) ]
//    let es = [ for d in dateRange 365. -> d, AstroCalcs.E d ]

    [
        Chart.Line(rises, Name="Rises")
        Chart.Line(sets, Name="Sets")
//        Chart.Line(es, Name="Equation of Time")
//        Chart.Line(length, Name="Day Length")
    ]
    |> Chart.Combine
    |> Chart.WithXAxis(Title = "Date", LabelStyle = LabelStyle(Angle = -45))
    |> Chart.WithYAxis(Title = "Time of Day")

let DayLength =
    let length = [ for d in dateRange 365. -> d, (AstroCalcs.DayLength d 51.) ]
    Chart.Line(length, Name="Day Length")

[<EntryPoint>]
let main argv = 
    SunRiseSet |> Chart.Show
    DayLength |> Chart.Show
    0

