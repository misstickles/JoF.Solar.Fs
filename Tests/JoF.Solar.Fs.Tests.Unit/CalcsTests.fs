namespace JoF.Solar.Fs.Tests.Unit

module CalcsTests =
    open System
    open Xunit

    let AssertDelta exp act delta =
        let low = exp - delta
        let high = exp + delta
        Assert.InRange(act, low, high)

    let myDate = DateTime.Parse "18 February 2017, 16:00:00"
    let lat = 51.
    let lon = 0.
    let tz = 0.

    [<Fact>]
    let ``JulianDateTime myBirthday isCorrect``() =
        AssertDelta 2457803.166666667 (AstroCalcs.JulianDateTime myDate) 0.00000001

    [<Fact>]
    let ``JulianCentury2000 myBirthday isCorrect``() =
        AssertDelta 0.17133927 (AstroCalcs.JulianCentury2000 myDate) 0.00000001

    [<Fact>]
    let ``MeanLongitudeOfSun myBirthday isCorrect``() =
        AssertDelta 328.8119 (AstroCalcs.L myDate) 0.0001

    [<Fact>]
    let ``MeanAnomolyOfSun myBirthday isCorrect``() =
        AssertDelta 6525.58 (AstroCalcs.G myDate) 0.0001

    [<Fact>]
    let ``OrbitalEccentricityOfEarth myBirthday isCorrect``() =
        AssertDelta 0.016701 (AstroCalcs.e myDate) 0.000001

    [<Fact>]
    let ``DistanceToSun myBirthday isCorrect``() =
        AssertDelta 0.988456 (AstroCalcs.R myDate) 0.000001

    [<Fact>]
    let ``SunApparentLongitude myBirthday isCorrect``() =
        AssertDelta 330.1912 (AstroCalcs.lambda myDate) 0.0001

    [<Fact>]
    let ``ObliquityCorrection myBirthday isCorrect``() =
        AssertDelta 23.43477 (AstroCalcs.epsilon myDate) 0.005

    [<Fact>]
    let ``RightAscensionSun myBirthday isCorrect``() =
        AssertDelta -27.7288 (AstroCalcs.RightAscension myDate) 0.001

    [<Fact>]
    let ``DeclinationSun myBirthday isCorrect``() =
        AssertDelta -11.4026 (AstroCalcs.Declination myDate) 0.01

    [<Fact>]
    let ``EquationOfTime myBirthday isCorrect``() =
        AssertDelta -13.8903 (AstroCalcs.E myDate) 0.01
    
    [<Fact>]
    let ``SolarNoon myBirthday isCorrect``() =
        AssertDelta 0.51 (AstroCalcs.SolarNoon lon (AstroCalcs.E myDate) tz) 0.001

    [<Fact>]
    let ``SolarNoon myBirthday_BST isCorrect``() =
        AssertDelta 0.551 (AstroCalcs.SolarNoon lon (AstroCalcs.E myDate) (tz + 1.0)) 0.001

    [<Fact>]
    let ``HourAngleSunrise myBirthday isCorrect``() =
        AssertDelta 76.96823 (AstroCalcs.HourAngleSunrise myDate lat) 0.003

    [<Fact>]
    let ``SunRise myBirthday isCorrect``() =
        AssertDelta 7.11028984 (AstroCalcs.SunRise myDate lat lon tz) 0.01

    [<Fact>]
    let ``SunSet myBirthday isCorrect``() =
        AssertDelta 17.362720104 (AstroCalcs.SunSet myDate lat lon tz) 0.0002

    [<Fact>]
    let ``TrueSolarTime myBirthday isCorrect``() =
        AssertDelta 946.1097 (AstroCalcs.TrueSolarTime myDate lon tz) 0.004

    [<Fact>]
    let ``HourAngle myBirthday isCorrect``() =
        AssertDelta 56.52743 (AstroCalcs.HourAngle myDate lon tz) 0.001

    [<Fact>]
    let ``SolarZenithAngle myBirthday isCorrect``() =
        AssertDelta 79.24559 (AstroCalcs.SolarZenithAngle myDate lat lon tz) 0.002

    [<Fact>]
    let ``SolarElevationAngle myBirthday isCorrect``() =
        AssertDelta 10.75441 (AstroCalcs.SolarElevationAngle myDate lat lon tz) 0.002

    [<Fact>]
    let ``AtmosphericRefraction myBirthday isCorrect``() =
        AssertDelta 0.082229 (AstroCalcs.AtmosphericRefraction myDate lat lon tz) 0.0001

    [<Fact>]
    let ``SolarElevationCorrected myBirthday isCorrect``() =
        AssertDelta 10.83664 (AstroCalcs.SolarElevationCorrected myDate lat lon tz) 0.002

    [<Fact>]
    let ``SolarAzimuthAngle myBirthday isCorrect``() =
        AssertDelta 236.3362 (AstroCalcs.SolarAzimuthAngle myDate lat lon tz) 0.00001
