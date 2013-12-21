module Fake.XamarinHelper

open System
open System.IO

open Fake.FileUtils
open Fake.ProcessHelper

exception Exited of int

// Convenience function for running external commands
let sh command args =
    let result =
        ExecProcess (fun info ->
            info.FileName <- command
            info.Arguments <- args
        ) TimeSpan.MaxValue

    if result <> 0 then raise (Exited result)

let RestoreComponents (solution: string) =
    mkdir "tools/xpkg"
    if not <| File.Exists "tools/xpkg/xamarin-component.exe" then
        sh "curl" "-o xpkg.zip https://components.xamarin.com/submit/xpkg"

        // unzip annoyingly returns 1 on success, so we have to catch that
        try
            sh "unzip" "xpkg.zip -d tools/xpkg"
        with Exited 1 -> ()

        rm "xpkg.zip"
    sh "tools/xpkg/xamarin-component.exe" (sprintf "restore %s" solution)

let BuildiOSPackage (project: string) =
    project
    |> sprintf "build %s -c:'Release|iPhone'"
    |> sh "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"

let BuildAndroidPackage (project: string) =
    project
    |> sprintf "%s /property:Configuration=Release /target:SignAndroidPackage"
    |> sh "xbuild"

let TestFlight api_token team_token note build =
    [
      "file", sprintf "@%s" build
      "api_token", api_token
      "team_token", team_token
      "notes", defaultArg note ""
    ]
    |> Seq.map (fun (k, v) -> sprintf "-F %s='%s'" k v)
    |> String.concat " "
    |> sprintf "http://testflightapp.com/api/builds.json %s"
    |> sh "curl"
