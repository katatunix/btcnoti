namespace btcnoti

open System.IO
open System.Net
open FSharp.Data

module Proxy =

    type JsonType = JsonProvider<""" { "host":"abc.com", "port":80, "username":"nghia", "password":"hehe" } """>

    let private loadInfo file =
        try
            file
            |> File.ReadAllText
            |> JsonType.Parse
            |> Some
        with ex -> None

    let private createObject (info : JsonType.Root) =
        let proxy = WebProxy (info.Host, info.Port)
        proxy.Credentials <- NetworkCredential (info.Username, info.Password)
        proxy :> IWebProxy

    let load file =
        file |> loadInfo |> Option.map createObject
