namespace btcnoti

open Result
open System.Threading

module Main =

    [<EntryPoint>]
    let main args =
        let proxyOp = Proxy.load "config.json"
        UI.printProxyInfo proxyOp

        let interval = UI.parseInterval args
        UI.printInterval interval

        while true do
            proxyOp
            |> CoinDesk.getPrice
            |> map UI.logPrice
            |> map UI.noti
            |> UI.logError
            Thread.Sleep (interval * 1000)
        
        0
