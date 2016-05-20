# Octopus artifact loader

Ocotpus makes it possible to collect files on the server during deployment, they will be stored as artifacts.
In the octopus documentation you can find more details about how to collect artifacts http://docs.octopusdeploy.com/display/OD/Artifacts 

Then after release process is complete, artifacts can be downloaded via api.
This script is using F# JsonType provider to load the artifacts from the api based on the current release version.

    version         = args 1 // "2.6.0-release"
    key             = args 2 // "API-YOURKEY"
    server          = args 3 // "https://server"
    artifactName    = args 4 // "dump.xml"
    output          = args 5 // "dump\file-2.6.0.xml"

From command line execute with fsi.exe

    fsi loader.fsx "2.6.0-release" "API-YOURKEY" "https://your.server/" "dump.xml" "dump\integration-test.xml"
