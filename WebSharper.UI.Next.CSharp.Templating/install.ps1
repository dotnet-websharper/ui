param($installPath, $toolsPath, $package, $project)

if($project.Type -eq "C#")
{
	$project.Object.AnalyzerReferences.Add((Join-Path $toolsPath "FSharp.Core.dll"))
	$project.Object.AnalyzerReferences.Add((Join-Path $toolsPath "WebSharper.UI.Next.Templating.Common.dll"))
	$project.Object.AnalyzerReferences.Add((Join-Path $toolsPath "WebSharper.UI.Next.CSharp.Templating.dll"))
}
