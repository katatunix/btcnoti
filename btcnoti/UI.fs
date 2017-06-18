namespace btcnoti

open System
open NghiaBui.Common
open Noti

module UI =

    let printUsage () =
        printfn "======================================================================"
        printfn "Bitcoin price notification (c) 2017 Nghia Bui :: katatunix@gmail.com"
        printfn "Usage:"
        printfn "    macOS   : mono btcnoti.exe intervalSec [ethermineId]"
        printfn "    Windows : btcnoti.exe intervalSec [ethermineId]"
        printfn "For example:"
        printfn "    btcnoti.exe 30"
        printfn "    -> the interval of notification is 30 seconds"
        printfn "Proxy config (if used): rename proxy_.json to proxy.json and edit it"
        printfn "======================================================================"

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
        | Some id -> printfn "Ethermine Id: %s" id
        | None -> ()

    let printProxyInfo proxyOp =
        match proxyOp with
        | Some _ -> printfn "Use proxy"
        | None -> printfn "No proxy"

    let noti priceBTC priceETH ethermineUnpaid =
        let title = "PRICE UPDATED"
        let msg = sprintf "1 BTC = %.2f USD | 1 ETH = %.2f USD" priceBTC priceETH
        let msg = match ethermineUnpaid with
                    | None -> msg
                    | Some unpaid -> sprintf "%s | Ethermine Unpaid = %.5f ETH" msg unpaid
        show title msg

    let logPrice priceBTC priceETH ethermineUnpaid =
        printf "[Price] [%O] 1 BTC = %.2f USD | 1 ETH = %.2f USD" DateTime.Now priceBTC priceETH
        match ethermineUnpaid with
        | None -> printfn ""
        | Some unpaid -> printfn " | Ethermine Unpaid = %.5f ETH" unpaid

    let logError res =
        match res with Error msg -> printfn "[Error] %s" msg | Ok _ -> ()
