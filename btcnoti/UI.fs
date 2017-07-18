namespace btcnoti

open System
open System.Net
open NghiaBui.Common

open Ethermine

module UI =

    let printUsage () =
        printfn "=================================================================================="
        printfn "Notification for Bitcoin/Ethereum and related stuff"
        printfn "(c) 2017 Nghia Bui :: katatunix@gmail.com"
        printfn "Usage:"
        printfn "    macOS   : mono btcnoti.exe [intervalSec] [ethermineId_1] [ethermineId_2] ..."
        printfn "    Windows : btcnoti.exe [intervalSec] [ethermineId_1] [ethermineId_2] ..."
        printfn "intervalSec: interval of notification in seconds, default (and minimum) is 30"
        printfn "ethermineId: ID of Ethermine.org miner account, this is optional"
        printfn "For example:"
        printfn "    btcnoti.exe"
        printfn "    btcnoti.exe 60"
        printfn "    btcnoti.exe 60 58801ebec6685d0d5461a30999fa5df91549a59e"
        printfn "Proxy config (if used): rename proxy_.json to proxy.json and edit it"
        printfn "=================================================================================="

    type ArgData = { Interval : int; EthermineIds : string [] }

    let parseArgData args =
        let MIN_INTERVAL = 30
        match args with
        | Array (Int i, ids) ->
            {   Interval = max i MIN_INTERVAL
                EthermineIds = ids }
        | _ ->
            {   Interval = MIN_INTERVAL
                EthermineIds = Array.empty }

    let printArgData data =
        printfn "Interval: %ds" data.Interval
        if data.EthermineIds.Length > 0 then
            printfn "Ethermine IDs: %s" (data.EthermineIds |> String.concat " ")

    let printProxyInfo (proxyOp : WebProxy option) =
        match proxyOp with
        | Some proxy -> printfn "Use proxy: %s" (proxy.Address.ToString ())
        | None -> printfn "No proxy"

    let private makePriceText btcPrice ethPrice =
        sprintf "BTC: %.2f USD | ETH: %.2f USD" btcPrice ethPrice

    let private makeLogText btcPrice ethPrice (miningInfos : MiningInfo []) =
        let SEP = "\n    "
        let priceMsg = makePriceText btcPrice ethPrice
        let miningMsg =
            miningInfos
            |> Array.map (fun info -> sprintf "Id: %s | EffHR: %s | EthPerDay: %.5f | Unpaid: %.5f ETH"
                                        info.Id info.EffectiveHashRate info.EthPerDay info.Unpaid)
            |> String.concat SEP
        if miningMsg.Length = 0 then
            priceMsg
        else
            priceMsg + SEP + miningMsg

    let noti btcPrice ethPrice =
        let title = "BTCNOTI UPDATED"
        Noti.show title (makePriceText btcPrice ethPrice)

    let log btcPrice ethPrice miningInfo =
        printfn "[%O] %s" DateTime.Now (makeLogText btcPrice ethPrice miningInfo)

    let logError res =
        match res with
        | Error msg -> printfn "%s" msg
        | Ok _ -> ()
