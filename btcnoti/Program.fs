﻿namespace btcnoti

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
                let! btcPrice = BlockChainInfo.getBtcPrice proxyOp
                let! ethPrice = CoinMarketCap.getEthPrice proxyOp
                let ethermineInfos =
                    data.EthermineIds
                    |> Array.choose (Ethermine.getEthermineInfo proxyOp)

                UI.log btcPrice ethPrice ethermineInfos
                UI.noti btcPrice ethPrice }

            |> UI.logError

            Thread.Sleep (data.Interval * 1000)
        
        0
