echo %1
UpdatePackageVersion.exe %1

nuget pack Lusa.UI.PaneView.nuspec
nuget pack Lusa.UI.Startup.nuspec
nuget pack Lusa.UI.Host.nuspec

nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.Host.%1.nupkg
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.PaneView.%1.nupkg
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.Startup.%1.nupkg
pause