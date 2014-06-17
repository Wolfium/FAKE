// include Fake lib
#r @"tools\FAKE\tools\FakeLib.dll"
#r @"tools\FAKE\tools\Fake.Deploy.exe"
#r @"tools\FAKE\tools\Fake.Deploy.Lib.dll"

open Fake

// Directories
let deployDir = @".\Publish\"
let serverUrl = "http://localhost:8085/fake/"

let traceActiveReleases() =
    tracefn "Active Releases:"
    DeploymentHelper.getAllReleases "."
      |> Seq.iter (tracefn "%A")

// Targets
Target "Deploy" (fun _ ->
    !! (deployDir + "*.nupkg") 
        |> Seq.head
        |> FakeDeployAgentHelper.DeployPackage serverUrl

    traceActiveReleases()
)

Target "Rollback" (fun _ ->
    FakeDeployAgentHelper.RollbackPackage serverUrl "Fake_Website" "HEAD~1"

    traceActiveReleases()
)

// start build
RunParameterTargetOrDefault "target" "Deploy"