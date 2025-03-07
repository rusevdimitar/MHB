using System.Collections.Generic;
using System.Xml.Serialization;

namespace MHB.BL
{
    [XmlRoot("Addresses")]
    public class AddressesBase
    {
        public AddressesBase() { }

        [XmlElement("RewriteAddress")]
        public List<RewriteAddress> RewriteAddresses { get; set; }
    }
}