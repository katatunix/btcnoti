namespace btcnoti

open Result
open Http
open FSharp.Data

module CoinDesk =

    type private JsonType = JsonProvider<""" {"time":{"updated":"May 30, 2017 07:47:00 UTC","updatedISO":"2017-05-30T07:47:00+00:00","updateduk":"May 30, 2017 at 08:47 BST"},"disclaimer":"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org","chartName":"Bitcoin","bpi":{"USD":{"code":"USD","symbol":"&#36;","rate":"2,298.1783","description":"United States Dollar","rate_float":2298.1783},"GBP":{"code":"GBP","symbol":"&pound;","rate":"1,791.4277","description":"British Pound Sterling","rate_float":1791.4277},"EUR":{"code":"EUR","symbol":"&euro;","rate":"2,064.6719","description":"Euro","rate_float":2064.6719}}} """>

    let getPriceBTC proxyOp =
        "http://api.coindesk.com/v1/bpi/currentprice.json"
        |> download proxyOp
        |> map JsonType.Parse
        |> map (fun data -> data.Bpi.Usd.RateFloat |> float)
