namespace btcnoti

open Result
open Http
open FSharp.Data

module CoinMarketCap =

    type private JsonType = JsonProvider<"""
[
    {
        "id": "ethereum", 
        "name": "Ethereum", 
        "symbol": "ETH", 
        "rank": "2", 
        "price_usd": "379.191", 
        "price_btc": "0.151798", 
        "24h_volume_usd": "972166000.0", 
        "market_cap_usd": "35110928896.0", 
        "available_supply": "92594310.0", 
        "total_supply": "92594310.0", 
        "percent_change_1h": "0.76", 
        "percent_change_24h": "1.54", 
        "percent_change_7d": "11.18", 
        "last_updated": "1497803365"
    }
]""">

    let getPriceETH proxyOp =
        "http://api.coinmarketcap.com/v1/ticker/ethereum/"
        |> download proxyOp
        |> map JsonType.Parse
        |> map (fun data -> data.[0].PriceUsd |> float)
