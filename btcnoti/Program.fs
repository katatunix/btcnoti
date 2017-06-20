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
                let unpaid = data.EthermineId |> Option.bind (Ethermine.getUnpaid proxyOp)

                UI.logPrice btc eth unpaid
                UI.noti btc eth unpaid }

            |> UI.logError

            Thread.Sleep (data.Interval * 1000)
        
        0
