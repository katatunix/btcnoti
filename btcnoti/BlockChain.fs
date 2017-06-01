namespace btcnoti

open Result
open Http
open FSharp.Data

module BlockChain =
    type private JsonType = JsonProvider<"""
{
  "USD" : {"15m" : 2383.63, "last" : 2383.63, "buy" : 2378.58, "sell" : 2383.64,  "symbol" : "$"},
  "ISK" : {"15m" : 235860.19, "last" : 235860.19, "buy" : 235360.49, "sell" : 235861.18,  "symbol" : "kr"},
  "HKD" : {"15m" : 18573.41, "last" : 18573.41, "buy" : 18534.06, "sell" : 18573.49,  "symbol" : "$"},
  "TWD" : {"15m" : 71740.04, "last" : 71740.04, "buy" : 71588.05, "sell" : 71740.34,  "symbol" : "NT$"},
  "CHF" : {"15m" : 2306.82, "last" : 2306.82, "buy" : 2301.93, "sell" : 2306.83,  "symbol" : "CHF"},
  "EUR" : {"15m" : 2120.02, "last" : 2120.02, "buy" : 2115.53, "sell" : 2120.03,  "symbol" : "€"},
  "DKK" : {"15m" : 15768.74, "last" : 15768.74, "buy" : 15735.33, "sell" : 15768.81,  "symbol" : "kr"},
  "CLP" : {"15m" : 1604659.72, "last" : 1604659.72, "buy" : 1601260.06, "sell" : 1604666.45,  "symbol" : "$"},
  "CAD" : {"15m" : 3217.85, "last" : 3217.85, "buy" : 3211.04, "sell" : 3217.87,  "symbol" : "$"},
  "INR" : {"15m" : 153783.86, "last" : 153783.86, "buy" : 153458.05, "sell" : 153784.51,  "symbol" : "₹"},
  "CNY" : {"15m" : 16227.87, "last" : 16227.87, "buy" : 16193.49, "sell" : 16227.94,  "symbol" : "¥"},
  "THB" : {"15m" : 81150.68, "last" : 81150.68, "buy" : 80978.76, "sell" : 81151.02,  "symbol" : "฿"},
  "AUD" : {"15m" : 3208.25, "last" : 3208.25, "buy" : 3201.46, "sell" : 3208.27,  "symbol" : "$"},
  "SGD" : {"15m" : 3297.18, "last" : 3297.18, "buy" : 3290.19, "sell" : 3297.19,  "symbol" : "$"},
  "KRW" : {"15m" : 2668724.07, "last" : 2668724.07, "buy" : 2663070.06, "sell" : 2668735.26,  "symbol" : "₩"},
  "JPY" : {"15m" : 264038.48, "last" : 264038.48, "buy" : 263479.08, "sell" : 264039.59,  "symbol" : "¥"},
  "PLN" : {"15m" : 8859.89, "last" : 8859.89, "buy" : 8841.12, "sell" : 8859.93,  "symbol" : "zł"},
  "GBP" : {"15m" : 1849.28, "last" : 1849.28, "buy" : 1845.36, "sell" : 1849.29,  "symbol" : "£"},
  "SEK" : {"15m" : 20706.9, "last" : 20706.9, "buy" : 20663.03, "sell" : 20706.98,  "symbol" : "kr"},
  "NZD" : {"15m" : 3364.47, "last" : 3364.47, "buy" : 3357.34, "sell" : 3364.48,  "symbol" : "$"},
  "BRL" : {"15m" : 7691.01, "last" : 7691.01, "buy" : 7674.71, "sell" : 7691.04,  "symbol" : "R$"},
  "RUB" : {"15m" : 135260.28, "last" : 135260.28, "buy" : 134973.71, "sell" : 135260.84,  "symbol" : "RUB"}
}  """>

    
    let getPrice proxyOp =
        "https://blockchain.info/ticker"
        |> download proxyOp
        |> map JsonType.Parse
        |> map (fun data -> data.Usd.``15m`` |> float)

    