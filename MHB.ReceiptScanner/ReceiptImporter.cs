// -----------------------------------------------------------------------
// <copyright file="ReceiptImporter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace MHB.ReceiptScanner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Tesseract;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReceiptImporter
    {
        public class Product
        {
            public string Name { get; set; }

            public double Quantity { get; set; }

            public double PricePerUnit { get; set; }

            public decimal Price { get; set; }
        }

        public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public ReceiptImporter()
        {
        }

        private async Task<string> GetGoogleAutoSuggest(string searchWord)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(searchWord))
            {
                using (HttpClient client = new HttpClient())
                {
                    result = await client.GetStringAsync($"https://www.google.bg/search?q={searchWord}");

                    Match corrected = Regex.Match(result, @"<b><i>(.*?)</i></b>", RegexOptions.None);

                    if (corrected.Groups.Count >= 1)
                    {
                        result = corrected.Groups[1].Value;
                    }
                    else
                    {
                        result = corrected.Value;
                    }
                }
            }

            return result;
        }

        private string AskGoogle(string productFullName)
        {
            string result = string.Empty;

            try
            {
                string[] nameSegments = productFullName.Split(' ');

                foreach (string segment in nameSegments)
                {
                    Task<string> googleAutoCompleteText = this.GetGoogleAutoSuggest(segment);

                    // Add what google suggests
                    if (!string.IsNullOrEmpty(googleAutoCompleteText.Result))
                    {
                        result += $" {googleAutoCompleteText.Result}";
                    } // .. else concat original text
                    else
                    {
                        result += $" {segment}";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"###### ReceiptImporter: {ex.Message}");
            }

            return result.Trim();
        }

        public Product[] Scan(string fileName)
        {
            string[] text = new string[] { };

            using (var engine = new TesseractEngine(Path.Combine(this.AssemblyDirectory, "tessdata"), "bul", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(fileName))
                {
                    using (var page = engine.Process(img, PageSegMode.SingleBlock))
                    {
                        string result = page.GetText();

                        ProofingTools proofingTools = new ProofingTools();

                        string spellCheckedResult = proofingTools.Proof(result);

                        text = spellCheckedResult.Split('\r', '\n');
                    }
                }
            }

            List<Product> products = new List<Product>();

            foreach (string item in text)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                Product product = new Product();

                Match matchPrice = Regex.Match(item.Replace(',', '.'), @"\s\d{1,}\.\d*");

                if (matchPrice.Success)
                {
                    decimal price = 0;

                    decimal.TryParse(matchPrice.Value, out price);

                    product.Price = price;

                    MatchCollection matchesName = Regex.Matches(item, @"[^0-9\W]{2,}");

                    if (matchesName.Count > 0)
                    {
                        StringBuilder sbMatchedProductNames = new StringBuilder();

                        foreach (Match match in matchesName)
                        {
                            sbMatchedProductNames.AppendFormat("{0} ", match.Value);
                        }

                        product.Name = sbMatchedProductNames.ToString().Trim();

                        string googleResult = this.AskGoogle(product.Name);

                        if (!string.IsNullOrEmpty(googleResult))
                        {
                            product.Name = googleResult;
                        }

                        products.Add(product);
                    }
                }
            }

            return products.ToArray();
        }
    }
}