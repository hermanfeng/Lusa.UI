using System;
using System.Xml.Serialization;
using AddinEngine;
using CommonExtension;

namespace WorkBenchContract
{
    public interface IPanViewSerializer
    {
        string Serializer();
        void DeSerializer(string data);
    }
}