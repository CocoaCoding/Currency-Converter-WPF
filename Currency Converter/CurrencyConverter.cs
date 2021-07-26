using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace Currency_Converter
{
    public class CurrencyConverter
    {
        private string ecbUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
        protected Dictionary<string, double> currencyRatesDic = new Dictionary<string, double>();

        /// <summary>
        /// As List of all currency rates.
        /// Will be bound to the ComboBox
        /// </summary>
        public List<string> CurrencyRateKeys { get; private set; }


        public CurrencyConverter()
        {
            string responseString = this.LoadCurrencyRates();
            if (!string.IsNullOrEmpty(responseString))
            {
                this.ParseCurrencyRates(responseString);
                this.CurrencyRateKeys = this.currencyRatesDic.Keys.ToList();
            }
        }

        /// <summary>
        /// Load currency rates from European Central Bank
        /// </summary>
        /// <returns></returns>
        private string LoadCurrencyRates()
        {
            string responseString = string.Empty;

            WebRequest request = WebRequest.Create(this.ecbUrl);
            WebResponse webResponse = request.GetResponse();

            using (Stream dataStream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseString = reader.ReadToEnd();
            }
            webResponse.Close();
            return responseString;
        }

        /// <summary>
        /// Parse Xml Nodes and attributes from document
        /// </summary>
        /// <param name="responseString"></param>
        private void ParseCurrencyRates(string responseString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseString);

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
            xmlNamespaceManager.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");
            xmlNamespaceManager.AddNamespace("default", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");

            XmlNode rootNode = doc.SelectSingleNode("gesmes:Envelope", xmlNamespaceManager);
            XmlNodeList cubeNodeList = rootNode.SelectNodes("default:Cube/default:Cube/default:Cube", xmlNamespaceManager);

            foreach (XmlNode cubeNode in cubeNodeList)
            {
                string currencyKey = string.Empty;
                string rateString = string.Empty;

                XmlAttribute attribute = cubeNode.Attributes["currency"];
                if (attribute != null)
                {
                    currencyKey = attribute.InnerText;
                }

                attribute = cubeNode.Attributes["rate"];
                if (attribute != null)
                {
                    rateString = attribute.InnerText;
                }

                this.AddToCurrencyRatesDictionary(currencyKey, rateString);
            }
        }

        /// <summary>
        /// Add to dictionary of currency rates if all values are vaild
        /// </summary>
        /// <param name="currencyKey"></param>
        /// <param name="rateString"></param>
        private void AddToCurrencyRatesDictionary(string currencyKey, string rateString)
        {
            if (!string.IsNullOrEmpty(currencyKey))
            {
                double rateValue;
                if (double.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out rateValue))
                {
                    this.currencyRatesDic.Add(currencyKey, rateValue);
                }
            }
        }

        /// <summary>
        /// Convert Euro value to selected Currency
        /// </summary>
        /// <param name="currencyIdentifierKey">Currency Key e.g "USD"</param>
        /// <param name="currencyValueEuro">The Amount in Euros</param>
        /// <returns></returns>
        public double Convert(string currencyIdentifierKey, double currencyValueEuro)
        {
            try
            {
                double rate = this.currencyRatesDic[currencyIdentifierKey];
                return currencyValueEuro * rate;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }
    }
}
