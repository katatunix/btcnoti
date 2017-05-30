namespace btcnoti

open System
open System.Diagnostics

module Noti =

    let private makeCommandMacOS title msg =
        "osascript",
        sprintf """ -e " display notification \"%s\" with title \"%s\" " """ msg title

    let private makeCommandWin title msg =
        "notifu.exe",
        sprintf "/p \"%s\" /m \"%s\"" title msg

    let private isMacOS = Environment.OSVersion.Platform = PlatformID.Unix

    let makeCommand =
        if isMacOS then makeCommandMacOS else makeCommandWin

    let show title msg =
        makeCommand title msg |> Process.Start |> ignore
