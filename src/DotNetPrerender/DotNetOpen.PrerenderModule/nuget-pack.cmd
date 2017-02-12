rem dotnet pack doesn't let me provide my own nuspec, https://github.com/dotnet/cli/issues/2170
rem we will still use nuget pack command until we get the latest .NET Core CLI
nuget pack DotNetOpen.PrerenderModule.csproj