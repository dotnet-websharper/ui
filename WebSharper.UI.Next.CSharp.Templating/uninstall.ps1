param($installPath, $toolsPath, $package, $project)

if($project.Type -eq "C#")
{
	try { $project.Object.AnalyzerReferences.Remove((Join-Path $toolsPath "WebSharper.UI.Next.Templating.Common.dll")) } catch { }
	try { $project.Object.AnalyzerReferences.Remove((Join-Path $toolsPath WebSharper.UI.Next.CSharp.Templating.dll")) } catch { }
}
