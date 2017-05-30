namespace btcnoti

open System
open System.IO
open System.Net
open System.Diagnostics

open FSharp.Data

module Main =

    type ProxyJsonType = JsonProvider<""" { "host":"abc.com", "port":80, "username":"nghia", "password":"hehe" } """>

    let loadProxyInfo file =
        try
            file
            |> File.ReadAllText
            |> ProxyJsonType.Parse
            |> Some
        with ex -> printfn "Note: %s" ex.Message; None

    let makeWebProxy (info : ProxyJsonType.Root) =
        let proxy = WebProxy (info.Host, info.Port)
        proxy.Credentials <- NetworkCredential (info.Username, info.Password)
        proxy

    let download proxyOp link =
        use wc = new WebClient ()
        match proxyOp with
        | Some proxy -> wc.Proxy <- proxy
        | None -> ()
        wc.DownloadString (Uri link)

    let priceJsonLink = "http://api.coindesk.com/v1/bpi/currentprice.json"
    type PriceJsonType = JsonProvider<""" {"time":{"updated":"May 30, 2017 07:47:00 UTC","updatedISO":"2017-05-30T07:47:00+00:00","updateduk":"May 30, 2017 at 08:47 BST"},"disclaimer":"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org","chartName":"Bitcoin","bpi":{"USD":{"code":"USD","symbol":"&#36;","rate":"2,298.1783","description":"United States Dollar","rate_float":2298.1783},"GBP":{"code":"GBP","symbol":"&pound;","rate":"1,791.4277","description":"British Pound Sterling","rate_float":1791.4277},"EUR":{"code":"EUR","symbol":"&euro;","rate":"2,064.6719","description":"Euro","rate_float":2064.6719}}} """>

    let parse json =
        let data = PriceJsonType.Parse json
        data.Bpi.Usd.RateFloat |> float

    let makeNotiCommand_MacOS title msg =
        "osascript",
        sprintf """ -e " display notification \"%s\" with title \"%s\" " """ msg title

    let makeNotiCommand_Win title msg =
        "notifu.exe",
        sprintf "/p \"%s\" /m \"%s\"" title msg

    let isMacOS = Environment.OSVersion.Platform = PlatformID.Unix

    let makeNotiCommand =
        if isMacOS then makeNotiCommand_MacOS else makeNotiCommand_Win

    let noti rate =
        let title = "Dau ma gia Bitcoin ne"
        let msg = sprintf "%.2f USD nha con" rate
        makeNotiCommand title msg
        |> Process.Start |> ignore

    let (|Int|_|) str =
        match System.Int32.TryParse str with
        | true, x -> Some x
        | _ -> None

    let getInterval argv =
        match argv with
        | [| Int x |] when x >= 60 -> printfn "Use interval: %ds" x; x
        | _ -> printfn "Use default interval: 10s"; 10
        |> (( * ) 1000)

    let getProxyOp () =
        let proxy =
            "proxy.json"
            |> loadProxyInfo
            |> Option.map makeWebProxy
        match proxy with
        | Some _ -> printfn "Use proxy"
        | None -> printfn "Not use proxy"
        proxy

    let loop (interval : int) proxy =
        printfn "Ngon lanh canh dao"
        while true do
            priceJsonLink
            |> download proxy
            |> parse
            |> noti
            System.Threading.Thread.Sleep interval

    [<EntryPoint>]
    let main argv =
        (getInterval argv, getProxyOp ()) ||> loop
        0
