module AstroCalcs

open System
open System.Security.Cryptography.X509Certificates

let Radians = Math.PI / 180.
let Degrees = 180. / Math.PI

let JulianDateTime (date : DateTime) = 
    let hour = double date.Hour
    let minute = double date.Minute
    let second = double date.Second

    let a = (date.Month - 14) / 12
    let year = date.Year + 4800 + a

    // http://aa.usno.navy.mil/faq/docs/JD_Formula.php
    let jdn = date.Day - 32075 + 1461 * year / 4 + 367 * (date.Month - 2 - a * 12) / 12 - 3 * ((date.Year + 4900 + a) / 100) / 4
    let jd = double jdn + ((hour - 12.) / 24.) + (minute / 1440.) + (second / 86400.)
    jd

let JulianDate2000 (date : DateTime) = (JulianDateTime date) - 2451545.

let JulianCentury2000 (date : DateTime) = (JulianDate2000 date) / 36525.

let L (date : DateTime) =
    let jc = JulianCentury2000 date
    (280.46646 + jc * (36000.76983 + jc * 0.0003032)) % 360.

let G (date : DateTime) =
    let jc = JulianCentury2000 date
    357.52911 + jc * (35999.05029 - 0.0001537 * jc)

let e (date : DateTime) =
    let jc = JulianCentury2000 date
    0.016708634 - jc * (0.000042037 + 0.0000001267 * jc)

let SunEquationOfCentre (date : DateTime) =
    let jc = JulianCentury2000 date
    let g = (G date) * Radians
    sin(g) * (1.914602 - jc * (0.004817 + 0.000014 * jc)) 
        + sin(2. * g) * (0.019993 - 0.000101 * jc)
        + sin(3. * g) * 0.000289

let SunTrueLongitude (date : DateTime) = (L date) + (SunEquationOfCentre date)

let sunTrueAnomoly (date : DateTime) = (G date) + (SunEquationOfCentre date)

let R (date : DateTime) =
    let ecc = (e date)
    (1.000001018 * (1. - ecc * ecc)) / (1. + ecc * cos(Radians * (sunTrueAnomoly date)))

let lambda (date : DateTime) =
    let jc = JulianCentury2000 date
    (SunTrueLongitude date) - 0.00569 - 0.00478 * sin(Radians * (125.04 - 1934.136 * jc))

let MeanObliquityCorrection (date : DateTime) =
    let jc = JulianCentury2000 date
    23. + (26. + ((21.448 - jc * (46.815 + jc * (0.00059 - jc * 0.001813)))) / 60.0) / 60.0

let epsilon (date : DateTime) =
    let jc = JulianCentury2000 date
    (MeanObliquityCorrection date) + 0.00256 * cos(125.04 - 1934.136 * jc * Radians)

let RightAscension (date : DateTime) =
    let l = (lambda date) * Radians
    let e = (epsilon date) * Radians
    (atan2 (cos(e) * sin(l)) (cos(l))) * Degrees

let Declination (date : DateTime) = 
    let e = (epsilon date) * Radians
    let l = (lambda date) * Radians
    (asin(sin(e) * sin(l))) * Degrees

let E (date : DateTime) =
    let e2 = (epsilon date) / 2. * Radians
    let e = (e date)
    let y = tan(e2) * tan(e2)
    let l = (L date) * Radians
    let g = (G date) * Radians

    4. * (
        y * sin(2. * l)
        - 2. * e * sin(g)
        + 4. * e * y * sin(g) * cos(2. * l)
        - 0.5 * y * y * sin(4. * l)
        - 1.25 * e * e * sin(2. * g)) * Degrees

let SolarNoon longitude E tz =
    (720. - 4. * longitude - E + tz * 60.0) / 1440.0

let HourAngleSunrise (date : DateTime) latitude =
    let l = latitude * Radians
    let d = (Declination date) * Radians
    (acos(cos(90.833 * Radians) / (cos(l) * cos(d)) - tan(l) * tan(d))) * Degrees

let SunRise (date : DateTime) latitude longitude tz =
    let ha = HourAngleSunrise date latitude
    let noon = SolarNoon longitude (E date) tz
    (noon - ha * 4. / 1440.0) * 24.0

let SunSet (date : DateTime) latitude longitude tz =
    let ha = HourAngleSunrise date latitude
    let noon = SolarNoon longitude (E date) tz
    (noon + ha * 4. / 1440.0) * 24.0

let DayLength (date : DateTime) latitude =
    8. * HourAngleSunrise date latitude

let TrueSolarTime (date : DateTime) longitude tz =
    let e = E date
    let ut = double date.Hour / 24.
    (ut * 1440. + e + 4. * longitude - 60. * tz) % 1440.

let HourAngle (date : DateTime) longitude tz =
    let time = (TrueSolarTime date longitude tz) / 4.
    match time < 0. with
    | true -> time + 180.
    | false -> time - 180.

let SolarZenithAngle (date : DateTime) latitude longitude tz =
    let delta = (Declination date) * Radians
    let ha = (HourAngle date longitude tz) * Radians
    let lat = latitude * Radians
    (acos(sin(lat) * sin(delta) + cos(lat) * cos(delta) * cos(ha))) * Degrees

let SolarElevationAngle (date : DateTime) latitude longitude tz =
    90. - (SolarZenithAngle date latitude longitude tz)

let AtmosphericRefraction (date : DateTime) latitude longitude tz = 
    let elevAngle = SolarElevationAngle date latitude longitude tz
    let angle = elevAngle * Radians
    let ref = 
        match elevAngle with
        | x when x > 85. -> 0.
        | x when x > 5. -> 58.1 / tan(angle) - 0.07 / (tan(angle) ** 3.) + 0.000086 / (tan(angle) ** 5.0)
        | x when x > -0.575 -> 1735. + elevAngle * (-518.2 + elevAngle * (103.4 + elevAngle * (-12.79 + elevAngle * 0.711)))
        | _ -> -20.772 / tan(angle)
    ref / 3600.

let SolarElevationCorrected (date : DateTime) latitude longitude tz = 
    (SolarElevationAngle date latitude longitude tz) + (AtmosphericRefraction date latitude longitude tz)

let SolarAzimuthAngle (date : DateTime) latitude longitude tz =
    let ha = HourAngle date longitude tz
    let zenith = (SolarZenithAngle date latitude longitude tz) * Radians
    let delta = (Declination date) * Radians
    let lat = latitude * Radians
    let az =
        if ha > 0. then
            (acos(((sin(lat) * cos(zenith)) - sin(delta)) / (cos(lat) * sin(zenith))) + 180.) * Degrees
        else
            540. - (acos(((sin(lat) * cos(zenith)) - sin(delta)) / (cos(lat) * sin(zenith)))) * Degrees
    az % 360.
