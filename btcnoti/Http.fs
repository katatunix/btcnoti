namespace btcnoti

open System
open System.Net

module Http =

    let download (proxyOp : IWebProxy option) link =
        use wc = new WebClient ()
        match proxyOp with
        | Some proxy -> wc.Proxy <- proxy
        | None -> ()
        try
            wc.DownloadString (Uri link) |> Ok
        with ex -> Error ex.Message
