set local=%~dp0
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.Host.1.0.0.nupkg
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.PaneView.1.0.0.nupkg
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.Startup.1.0.0.nupkg
pause