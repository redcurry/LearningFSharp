module NuGet

open FSharp.Data

type Package = HtmlProvider< @"..\..\..\data\sample-package.html">

// Original function
//let getDownloadsForPackage packageName =
//    let package = Package.Load(sprintf "http://www.nuget.org/packages/%s" packageName)
//    package.Tables.``Version History``.Rows
//    |> Seq.sumBy (fun p -> p.Downloads)

// Refactored functions

let loadPackage =
    sprintf "http://www.nuget.org/packages/%s" >> Package.Load

let getVersionsForPackage (package:Package) =
    package.Tables.``Version History``.Rows

let loadPackageVersions = loadPackage >> getVersionsForPackage

let getDownloadsForPackage =
    loadPackageVersions >> Seq.sumBy (fun p -> p.Downloads)

let getDetailsForVersion version =
    loadPackageVersions >> Seq.tryFind (fun p -> p.Version = version)

open System

type PackageVersion =
    | CurrentVersion
    | Prerelease
    | Old

type VersionDetails =
    { Version : Version
      Downloads : decimal
      PackageVersion : PackageVersion
      LastUpdated : DateTime }

type NuGetPackage =
    { PackageName : string
      Versions : VersionDetails list }

let parse (versionStr:string) =
    if versionStr.EndsWith "(this version)" then
        (Version.Parse (versionStr.Split ' ').[0], CurrentVersion)
    else
        let versionPart = versionStr.Split ' ' |> Seq.last
        let versionTokens = versionPart.Split '-'
        if versionTokens.Length = 1 then
            (Version.Parse versionTokens.[0], Old)
        else
            (Version.Parse versionTokens.[0], Prerelease)

let enrich (rows:Package.VersionHistory.Row []) =
    { PackageName = ((rows.[0].Version).Split ' ').[0]
      Versions = rows
      |> Array.map (fun r ->
        { Version = fst (parse r.Version)
          Downloads = r.Downloads
          PackageVersion = snd (parse r.Version)
          LastUpdated = r.``Last updated`` })
      |> Array.toList }

let loadPackageVersions2 = loadPackage >> getVersionsForPackage >> enrich >> (fun p -> p.Versions)
let getDetailsForVersion2 version = loadPackageVersions2 >> Seq.find (fun p -> p.Version = version)
let getDetailsForCurrentVersion = loadPackageVersions2 >> Seq.find (fun p -> p.PackageVersion = CurrentVersion)

let details = "Newtonsoft.Json" |> getDetailsForVersion2 (Version.Parse "9.0.1")