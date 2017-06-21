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
                let ethermineInfo = data.EthermineId
                                    |> Option.bind (Ethermine.getEthermineInfo proxyOp)

                UI.logInfo  btc eth ethermineInfo
                UI.noti     btc eth ethermineInfo }

            |> UI.logError

            Thread.Sleep (data.Interval * 1000)
        
        0
