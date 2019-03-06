using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lusa.AddinEngine.Extension;
using UIShell.OSGi;
using UIShell.OSGi.Configuration.BundleManifest;
using UIShell.OSGi.Core.Service;

namespace Lusa.AddinEngine
{
    public class AddinEngineHost
    {
        static BundleRuntime bruntime;

        public static BundleRuntime Runtime
        {
            get { return AddinEngineHost.bruntime; }
            set { AddinEngineHost.bruntime = value; }
        }

        const string PluginPath = "Plugins";
        static List<string> assistantpluginpath;
        static AddinEngineStartUpSetting setting;
        public static void InitializeBundleRuntime(AddinEngineStartUpSetting setting)
        {
            AddinEngineHost.setting = setting;
            bruntime = new BundleRuntime(CombinePluginPaths());
            var serviceContainer = bruntime.Framework.ServiceContainer;
            bruntime.Framework.ReflectSetProperty("ServiceContainer", new WrapServiceContainer(serviceContainer));
        }

        private class WrapServiceContainer : IServiceManager
        {
            private readonly IServiceManager orignal;

            public WrapServiceContainer(IServiceManager orignal)
            {
                this.orignal = orignal;
            }
            public void AddService(IBundle owner, Type serviceType, params object[] serviceInstances)
            {
                if (serviceType == typeof(IBundleInstallerService))
                {
                    var orignal = serviceInstances[0] as IBundleInstallerService;
                    this.orignal.AddService(owner, serviceType, new WrapBundleInstallerService(orignal));
                }
                else
                {
                    this.orignal.AddService(owner, serviceType, serviceInstances);
                }
            }

            public void AddService<T>(IBundle owner, params object[] serviceInstances)
            {
                this.orignal.AddService(owner, serviceInstances);
            }

            public void AddService(IBundle owner, object serviceInstance, params Type[] serviceTypes)
            {
                this.orignal.AddService(owner, serviceInstance, serviceTypes);
            }

            public T GetFirstOrDefaultService<T>()
            {
                return this.orignal.GetFirstOrDefaultService<T>();
            }

            public object GetFirstOrDefaultService(Type serviceType)
            {
                return this.orignal.GetFirstOrDefaultService(serviceType);
            }

            public object GetFirstOrDefaultService(string serviceTypeName)
            {
                return this.orignal.GetFirstOrDefaultService(serviceTypeName);
            }

            public List<object> GetService(string serviceTypeName)
            {
                return this.orignal.GetService(serviceTypeName);
            }

            public List<T> GetService<T>()
            {
                return this.orignal.GetService<T>();
            }

            public List<object> GetService(Type serviceType)
            {
                return this.orignal.GetService(serviceType);
            }

            public Dictionary<Type, List<object>> GetServices()
            {
                return this.orignal.GetServices();
            }

            public void RemoveService(IBundle owner, Type serviceType, object serviceInstance)
            {
                this.orignal.RemoveService(owner, serviceType, serviceInstance);
            }

            public void RemoveService<T>(IBundle owner, object serviceInstance)
            {
                this.orignal.RemoveService<T>(owner, serviceInstance);
            }

            public void RemoveService(IBundle owner, object serviceInstance)
            {
                this.orignal.RemoveService(owner, serviceInstance);
            }
        }

        private class WrapBundleInstallerService : IBundleInstallerService
        {
            private readonly IBundleInstallerService orignal;

            public WrapBundleInstallerService(IBundleInstallerService orignal)
            {
                this.orignal = orignal;
            }
            public string GlobalPersistentFile => orignal.GlobalPersistentFile;

            public IDictionary<string, BundleData> BundleDatas => orignal.BundleDatas;

            public string UpdateFolder => orignal.UpdateFolder;

            public BundleData CreateBundleData(string bundleDir, Stream stream)
            {
                return orignal.CreateBundleData(bundleDir, stream);
            }

            public BundleData CreateBundleData(string bundleDir)
            {
                return orignal.CreateBundleData(bundleDir);
            }

            public BundleData FindBundleContainPath(string path)
            {
                return orignal.FindBundleContainPath(path);
            }

            public BundleData GetBundleDataByName(string symbolicName)
            {
                return orignal.GetBundleDataByName(symbolicName);
            }

            public string GetBundlePath(string symbolicName)
            {
                return orignal.GetBundlePath(symbolicName);
            }

            public bool InstallBundles()
            {
                var result = orignal.InstallBundles();
                var boundDatas = orignal.BundleDatas;
                foreach(var data in boundDatas.Values)
                {
                    var binPath = Path.Combine(data.Path, "bin");
                    if (Directory.Exists(binPath))
                    {
                        var allAssems = Directory.EnumerateFiles(binPath, "*.dll").Select(p => p.Substring(data.Path.Length + 1)).ToList();
                        var asses = data.Runtime.Assemblies.ToDictionary(a => a.Path);
                        foreach (var assePath in allAssems)
                        {
                            if (!asses.ContainsKey(assePath))
                            {
                                try
                                {
                                    var name = System.Reflection.AssemblyName.GetAssemblyName(Path.Combine(data.Path, assePath));

                                    data.Runtime.AddAssembly(new AssemblyData()
                                    {
                                        Path = assePath,
                                    });
                                }
                                catch(Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
                return result;
            }
        }

        public static void AddPluginPath(string path)
        {
            if (assistantpluginpath == null)
            {
                assistantpluginpath = new List<string>();
            }
            assistantpluginpath.Add(path);
        }

        private static string[] CombinePluginPaths()
        {
            var paths = new List<string>();
            paths.Add(PluginPath);
            if (assistantpluginpath != null)
            {
                paths.AddRange(assistantpluginpath);
            }
            return paths.ToArray();
        }

        public static void StartRuntime()
        {
            if (bruntime != null)
            {
                bruntime.Start();
            }
        }
        
    }

    public class AddinEngineStartUpSetting
    {
        
    }


}
