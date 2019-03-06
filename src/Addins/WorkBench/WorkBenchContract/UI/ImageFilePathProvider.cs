using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace WorkBenchContract
{
    public class ImageFilePathProvider
    {
        public static Uri GetAssemblyImageLocalUri(Assembly assembly, string fileName)
        {
            return new Uri(GetAssemblyImageLocalPath(assembly, fileName));
        }

        public static string GetAssemblyImageLocalPath(Assembly assembly, string fileName)
        {
            fileName = fileName.TrimStart(@" \/".ToCharArray()).Replace(@"\", "/");

            return GetAssemblyLocalPath(assembly,fileName);            
        }

        public static string GetAssemblyLocalPath(Assembly assembly, string rootpath, bool includeAppPack = true)
        {
            //string path = "pack://application:,,,/" + StorageProvider.GetStorageAssemblyName() + ";component/Storage";
            string path = includeAppPack ? "pack://application:,,,/" : "/";
            path += assembly.GetName().Name + ";component" + rootpath;
            return path;
        }

        public static BitmapImage CreateBitmapImage(string imagePath)
        {
            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(imagePath, UriKind.Absolute);
            bmpImage.EndInit();
            return bmpImage;
        }
    }

}