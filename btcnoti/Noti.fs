namespace btcnoti

open System
open System.Diagnostics
open System.Windows.Forms
open System.Drawing

module Noti =

    let showWin title msg =
        use item = new NotifyIcon ()
        item.Visible <- true
        item.Icon <- SystemIcons.Information
        item.ShowBalloonTip (3000, title, msg, ToolTipIcon.Info)

    let showMacOS title msg =
        (   "osascript",
            sprintf """ -e " display notification \"%s\" with title \"%s\" " """ msg title )
        |> Process.Start |> ignore

    let private isMacOS = Environment.OSVersion.Platform = PlatformID.Unix

    let show =
        if isMacOS then showMacOS else showWin
        