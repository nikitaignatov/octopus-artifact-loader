#r "FSharp.Data.dll"
open FSharp.Data

let args index = fsi.CommandLineArgs.[index]

type release        = JsonProvider<"samples/release.json">
type releases       = JsonProvider<"samples/releases.json">
type artifacts      = JsonProvider<"samples/artifacts.json">

let version         = args 1 // "2.6.0-release"
let key             = args 2 // "API-SOMEKEYTOOCTOPUS"
let server          = args 3 // "https://deployment.octopus.com"
let artifactName    = args 4 // "test_result.xml"
let output          = args 5 // "reports\integration-test-result-2.6.0-release.xml"

let q (a:string)    = if a.Contains("?") then "&" else "?"
let toMap map data  = data |> Array.map map |> Map.ofArray
let withKey path    = sprintf "%s%sapiKey=%s" path (q path) key
let url path        = sprintf "%s%s" server (withKey path)

let releasesData    = url "/api/releases" |> releases.Load
let lookup          = releasesData.Items |> toMap (fun r-> r.Version.String, r.Id) 
let get version     = lookup.[Some(version)]
let releaseData     = url ("/api/releases/" + get version) |> release.Load
let articatsData    = url releaseData.Links.Artifacts |> artifacts.Load
let arts            = articatsData.Items |> toMap (fun r -> r.Filename, r.Links.Content)
let file            = url arts.[artifactName] |> Http.RequestString

(output, file) |> System.IO.File.WriteAllText

printf "Artifact: %s is stored in location: %s\nGLHF!" artifactName output