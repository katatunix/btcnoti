namespace btcnoti

open Result
open System.Threading
open NghiaBui.Common.Rop

module Main =

    [<EntryPoint>]
    let main args =
        UI.printUsage ()

        let proxyOp = Proxy.load "proxy.json"
        UI.printProxyInfo proxyOp

        let interval = UI.parseInterval args
        UI.printInterval interval

        while true do
            rop {
                let! btc = BlockChain.getPriceBTC proxyOp
                let! eth = CoinMarketCap.getPriceETH proxyOp
                UI.logPrice btc eth
                UI.noti btc eth }
            |> UI.logError
            Thread.Sleep (interval * 1000)
        
        0
