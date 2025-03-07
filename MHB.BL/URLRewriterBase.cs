using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MHB.BL
{
    public class URLRewriterBase
    {
        private string _addressesSourcePath = string.Empty;

        public string AddressesSourcePath
        {
            get
            {
                return this._addressesSourcePath;
            }
            set
            {
                this._addressesSourcePath = value;
            }
        }

        public URLRewriterBase()
        {
        }

        public URLRewriterBase(string path)
        {
            this._addressesSourcePath = path;
        }

        #region [ GetDeserializedAddresses ]

        public List<RewriteAddress> GetDeserializedAddresses()
        {
            try
            {
                string xml = File.OpenText(string.Format("{0}URLRewrite.xml", AppDomain.CurrentDomain.BaseDirectory)).ReadToEnd();

                Addresses adr = Deserialize<Addresses>(xml);

                return adr.RewriteAddresses;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetDeserializedAddresses", string.Empty, 0, string.Empty);
                return new List<RewriteAddress>();
            }
        }

        #endregion [ GetDeserializedAddresses ]

        #region [ Deserialize<T> ]

        public static T Deserialize<T>(string xml)
        {
            T result;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }

        #endregion [ Deserialize<T> ]

        #region [ GetAddresses ]

        public List<RewriteAddress> GetAddresses()
        {
            List<RewriteAddress> addresses = new List<RewriteAddress>();

            try
            {
                XmlDocument doc = new XmlDocument();

                if (string.IsNullOrEmpty(_addressesSourcePath))
                    doc.Load(string.Format("{0}URLRewrite.xml", AppDomain.CurrentDomain.BaseDirectory));
                else
                    doc.Load(_addressesSourcePath);

                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    addresses.Add(new RewriteAddress { Key = node["Key"].InnerXml, RequestedAddress = node["RequestedAddress"].InnerXml, ActualLocation = node["ActualLocation"].InnerXml });
                }

                return addresses;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetAddresses", string.Empty, 0, string.Empty);
                return new List<RewriteAddress>();
            }
        }

        #endregion [ GetAddresses ]

        #region [ GetLink ]

        public static string GetKey(string path)
        {
            try
            {
                string key = string.Empty;

                key = (from RewriteAddress a in new URLRewriter().GetAddresses() where a.ActualLocation == path select a.RequestedAddress).FirstOrDefault();

                return key;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetKey", string.Empty, 0, string.Empty);
                return string.Empty;
            }
        }

        public static string GetLink(string key)
        {
            try
            {
                string link = string.Empty;

                link = (from RewriteAddress a in new URLRewriter().GetAddresses() where a.Key == key select a.RequestedAddress).FirstOrDefault();

                return link;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetLink", string.Empty, 0, string.Empty);
                return string.Empty;
            }
        }

        public static string GetLink(string key, params KeyValuePair<string, string>[] args)
        {
            try
            {
                string link = string.Empty;

                link = (from RewriteAddress a in new URLRewriter().GetAddresses() where a.Key == key select a.RequestedAddress).FirstOrDefault();

                if (args != null)
                {
                    StringBuilder linkWithParams = new StringBuilder(link);

                    linkWithParams.Append("?");
                  
                    foreach (KeyValuePair<string, string> kv in args)
                    {                        
                        linkWithParams.Append(string.Format("{0}={1}", kv.Key, kv.Value));
                        linkWithParams.Append("&");
                    }

                    linkWithParams.Length -= 1;

                    link = linkWithParams.ToString();
                }

                return link;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetLink", string.Empty, 0, string.Empty);
                return string.Empty;
            }
        }

        #endregion [ GetLink ]
    }
}