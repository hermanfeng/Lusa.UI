echo %1
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.Host.%1.nupkg
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.PaneView.%1.nupkg
nuget push -src http://hermanlinux.seismic-dev.com -ApiKey 123 Lusa.UI.Startup.%1.nupkg
pause