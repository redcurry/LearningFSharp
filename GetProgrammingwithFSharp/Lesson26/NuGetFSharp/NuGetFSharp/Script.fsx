#I @"..\packages"

#r @"Humanizer.Core\lib\netstandard1.0\Humanizer.dll"

open Humanizer

"ScriptsAreAGreatWayToExplorePackages".Humanize()

#r @"Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"

#load "Library1.fs"

Library1.getPerson()
