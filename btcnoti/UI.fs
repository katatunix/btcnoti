namespace btcnoti

open System
open System.Net
open NghiaBui.Common

open Noti
open Ethermine

module UI =

    let printUsage () =
        printfn "=================================================================================="
        printfn "Notification for Bitcoin/Ethereum and related stuff"
        printfn "(c) 2017 Nghia Bui :: katatunix@gmail.com"
        printfn "Usage:"
        printfn "    macOS   : mono btcnoti.exe [intervalSec] [ethermineId]"
        printfn "    Windows : btcnoti.exe [intervalSec] [ethermineId]"
        printfn "intervalSec: interval of notification in seconds, default (and minimum) is 30"
        printfn "ethermineId: ID of Ethermine.org miner account, this is optional"
        printfn "For example:"
        printfn "    btcnoti.exe"
        printfn "    btcnoti.exe 60"
        printfn "    btcnoti.exe 60 58801ebec6685d0d5461a30999fa5df91549a59e"
        printfn "Proxy config (if used): rename proxy_.json to proxy.json and edit it"
        printfn "=================================================================================="

    type ArgData = { Interval : int; EthermineId : string option }

    let parseArgData args =
        let MIN = 30
        match args with
        | [| Int x |] ->
            { Interval = max x MIN; EthermineId = None }
        | [| Int x; id |] ->
            { Interval = max x MIN; EthermineId = Some id }
        | _ ->
            { Interval = MIN; EthermineId = None }

    let printArgData data =
        printfn "Interval: %ds" data.Interval
        match data.EthermineId with
        | Some id -> printfn "Ethermine ID: %s" id
        | None -> ()

    let printProxyInfo (proxyOp : WebProxy option) =
        match proxyOp with
        | Some proxy -> printfn "Use proxy: %s" (proxy.Address.ToString ())
        | None -> printfn "No proxy"

    let private makeMsgText priceBTC priceETH (mInfo : MiningInfo option) =
        let msg = sprintf "BTC: %.2f USD | ETH: %.2f USD" priceBTC priceETH
        match mInfo with
        | None ->
            msg
        | Some info ->
            sprintf "%s | EffHR: %s; EthPerDay: %.5f; Unpaid: %.5f ETH"
                msg info.EffectiveHashRate info.EthPerDay info.Unpaid

    let noti priceBTC priceETH (mInfo : MiningInfo option) =
        let title = "BTCNOTI UPDATED"
        show title (makeMsgText priceBTC priceETH mInfo)

    let logInfo priceBTC priceETH (mInfo : MiningInfo option) =
        printfn "[%O] %s" DateTime.Now (makeMsgText priceBTC priceETH mInfo)

    let logError res =
        match res with
        | Error msg -> printfn "%s" msg
        | Ok _ -> ()
