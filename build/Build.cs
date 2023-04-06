using DarkCalVer;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using System.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.PushNuget);

    private const string CoreProjectName = "DarkCalVer";
    private const string TestProjectName = "DarkCalVer.Tests";

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    readonly Configuration Configuration = Configuration.Release;

    [Parameter] readonly string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Parameter] readonly string NugetApiKey;

    readonly CalVer Version = CalVer.Create(new CalVerOptions
    {
        Accuracy = Accuracy.Hours,
        PreventLeadingZeros = true,
    });

    Target Info => _ => _
        .Executes(() =>
        {
            Serilog.Log.Logger.Information("Auto-generated version: {VersionString}", Version.VersionString);
        });

    Target Clean => _ => _
        .DependsOn(Info)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
        });

    Target RestoreCore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution.GetProject(CoreProjectName)));
        });

    Target CompileCore => _ => _
        .DependsOn(RestoreCore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution.GetProject(CoreProjectName))
                .SetVersion(Version.VersionString)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target CompileTests => _ => _
        .DependsOn(CompileCore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution.GetProject(TestProjectName))
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target RunTests => _ => _
        .DependsOn(CompileTests)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject(TestProjectName))
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });


    Target PackCore => _ => _
        .DependsOn(RunTests)
        .Executes(() =>
        {
            DotNetPack(s => s
              .SetProject(Solution.GetProject(CoreProjectName))
              .SetVersion(Version.VersionString)
              .SetConfiguration(Configuration)
              .EnableNoBuild()
              .EnableNoRestore()
              .SetOutputDirectory(ArtifactsDirectory));
        });

    Target PushNuget => _ => _
        .DependsOn(PackCore)
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory, "*.nupkg")
               .Where(x => !string.IsNullOrEmpty(x) && !x.EndsWith("symbols.nupkg"))
               .ForEach(x =>
               {
                   DotNetNuGetPush(s => s
                       .SetTargetPath(x)
                       .SetSource(NugetApiUrl)
                       .SetApiKey(NugetApiKey)
                   );
               });
        });
}