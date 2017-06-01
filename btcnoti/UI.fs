namespace btcnoti

open System
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

    let private (|Int|_|) str =
        match System.Int32.TryParse str with
        | true, x -> Some x
        | _ -> None

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

    let noti price =
        let title = "BITCOIN PRICE UPDATED"
        let msg = sprintf "1 BTC = %.2f USD" price
        show title msg

    let logPrice price =
        printfn "[Price] %.2f USD at %O" price DateTime.Now
        price

    let logError res =
        match res with Error msg -> printfn "[Error] %s" msg | Ok _ -> ()
