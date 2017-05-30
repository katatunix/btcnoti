namespace btcnoti

open System
open Noti

module UI =

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
        let title = "Dau ma gia Bitcoin ne"
        let msg = sprintf "%.2f USD nha con" price
        show title msg

    let logPrice price =
        printfn "[Price] %.2f USD at %O" price DateTime.Now
        price

    let logError res =
        match res with Error msg -> printfn "[Error] %s" msg | Ok _ -> ()
