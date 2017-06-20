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

        let data = UI.parseArgData args
        UI.printArgData data

        while true do
            rop {
                let! btc = BlockChain.getPriceBTC proxyOp
                let! eth = CoinMarketCap.getPriceETH proxyOp

                match data.EthermineId with
                | None ->
                    UI.logPrice btc eth None
                    UI.noti btc eth None
                | Some id ->
                    let! unpaid = Ethermine.getUnpaid proxyOp id
                    UI.logPrice btc eth (Some unpaid)
                    UI.noti btc eth (Some unpaid) }

            |> UI.logError

            Thread.Sleep (data.Interval * 1000)
        
        0
