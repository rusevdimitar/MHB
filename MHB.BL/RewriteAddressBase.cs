using System;
using System.Xml.Serialization;

namespace MHB.BL
{
    [Serializable]
    public class RewriteAddressBase
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("RequestedAddress")]
        public string RequestedAddress { get; set; }

        [XmlElement("ActualLocation")]
        public string ActualLocation { get; set; }

        public RewriteAddressBase() { }
    }
}