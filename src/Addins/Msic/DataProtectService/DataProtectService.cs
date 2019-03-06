using System.IO;
using System.Security.Cryptography;
using System.Text;
using UIShell.OSGi;
using AddinEngine;
using CommonExtension;
using System;

namespace CommonLibrary
{
    public interface IDataProtectService
    {
        string ProtectData(string input, string key = "");
        string UnprotectData(string input, string key = "");
    }
}
