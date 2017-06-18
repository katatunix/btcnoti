namespace btcnoti

open System
open NghiaBui.Common
open Noti

module UI =

    let printUsage () =
        printfn "======================================================================"
        printfn "Bitcoin price notification (c) 2017 Nghia Bui :: katatunix@gmail.com"
        printfn "Usage:"
        printfn "    macOS   : mono btcnoti.exe intervalSec"
        printfn "    Windows : btcnoti.exe intervalSec"
        printfn "For example:"
        printfn "    btcnoti.exe 10"
        printfn "    -> the interval of notification is 10 seconds"
        printfn "Proxy config (if used): rename proxy_.json to proxy.json and edit it"
        printfn "======================================================================"

    let parseInterval args =
        match args with
        | [| Int x |] when x >= 10 -> x
        | _ -> 10

    let printInterval i =
        printfn "Interval: %ds" i

    let printProxyInfo proxyOp =
        match proxyOp with
        | Some _ -> printfn "Use proxy"
        | None -> printfn "No proxy"

    let noti priceBTC priceETH =
        let title = "PRICE UPDATED"
        let msg = sprintf "1 BTC = %.2f USD\n1 ETH = %.2f USD" priceBTC priceETH
        show title msg

    let logPrice priceBTC priceETH =
        printfn "[Price] [%O] 1 BTC = %.2f USD | 1 ETH = %.2f USD" DateTime.Now priceBTC priceETH

    let logError res =
        match res with Error msg -> printfn "[Error] %s" msg | Ok _ -> ()
