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

    let private makePriceText btcPrice ethPrice =
        sprintf "BTC: %.2f USD | ETH: %.2f USD" btcPrice ethPrice

    let private makeMsgText btcPrice ethPrice (miningInfo : MiningInfo option) =
        let msg = makePriceText btcPrice ethPrice
        match miningInfo with
        | None ->
            msg
        | Some info ->
            sprintf "%s | EffHR: %s; EthPerDay: %.5f; Unpaid: %.5f ETH"
                msg info.EffectiveHashRate info.EthPerDay info.Unpaid

    let noti btcPrice ethPrice =
        let title = "BTCNOTI UPDATED"
        show title (makePriceText btcPrice ethPrice)

    let log btcPrice ethPrice miningInfo =
        printfn "[%O] %s" DateTime.Now (makeMsgText btcPrice ethPrice miningInfo)

    let logError res =
        match res with
        | Error msg -> printfn "%s" msg
        | Ok _ -> ()
