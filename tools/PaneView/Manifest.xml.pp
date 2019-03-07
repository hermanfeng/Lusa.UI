<?xml version="1.0" encoding="utf-8"?>
<Bundle xmlns="urn:uiosp-bundle-manifest-2.0" Name="$AssemblyName$" SymbolicName="$AssemblyName$" Version="1.0.0.0" InitializedState="Active">
  <Runtime>
  </Runtime>
  <Extension Point="WorkBench.PaneViewPoint">
    <instance class="$rootnamespace$.$AssemblyName$PanViewProvider" />
  </Extension>
</Bundle>