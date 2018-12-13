using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Providers
{
    /// <summary>
    ///Process all data from kraken client
    /// </summary>
    public class Data
    {
        Hashtable assets;

        /// <summary>
        /// All Assets 
        /// </summary>
        public Hashtable Assets
        {
            get
            {
                if (assets == null)
                {
                    assets = GetAssets();
                    return assets;
                }
                else
                    return assets;

            }
        }


        /// <summary>
        /// Get Latest Quotes
        /// </summary>
        /// <param name="pairs">List with pairs</param>
        /// <returns>The Array with FXQuote objects</returns>
        public FXQuote[] GetLatestQuotes(List<string> pairs)
        {
            using (KrakenClient client = new KrakenClient())
            {
                Task<string> tick = Task.Run<string>(async () => await client.GetTickAsync(pairs));
                dynamic ticksData = JsonConvert.DeserializeObject<dynamic>(tick.Result);

                return (ProcessTicker(ticksData)).ToArray();
            }
        }

        /// <summary>
        /// Get All Assets
        /// </summary>
        /// <returns>Dynamic ojbect with assets</returns>
        Hashtable GetAssets()
        {
            using (KrakenClient client = new KrakenClient())
            {
                Task<string> assets = Task.Run<string>(async () => await client.GetAllAssetsAsync());
                dynamic data = JsonConvert.DeserializeObject<dynamic>(assets.Result);
                return ProcessAssets(data);
            }
        }

        /// <summary>
        /// Process Ticker
        /// </summary>
        /// <param name="data">Dynamic ticker data</param>
        /// <returns>List from FXQuote<FXQuote></returns>
        List<FXQuote> ProcessTicker(dynamic data)
        {
            List<FXQuote> result = new List<FXQuote>();

            try
            {
                if (data.errors == null && data.result != null)
                {
                    foreach (string symbol in GetProperties(data.result))
                    {
                        dynamic propertyValue = data.result[symbol];
                        FXQuote quote = new FXQuote();
                        Currency firstCurrency = GetCurrency(Position.First, symbol);
                        Currency secondCurrency = GetCurrency(Position.Second, symbol);
                        quote.Symbol = symbol;
                        quote.Name = firstCurrency.Name + "/" + secondCurrency.Name;
                        quote.Ask = Math.Round((double)propertyValue.a[0], firstCurrency.Display_decimals);
                        quote.Bid = Math.Round((double)propertyValue.b[0], firstCurrency.Display_decimals);
                        quote.Provider = ProviderEnum.Kraken;
                        quote.ReceivedTS = DateTime.UtcNow;
                        result.Add(quote);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        /// <summary>
        /// Process Assets
        /// </summary>
        /// <param name="data">Dynamic data</param>
        /// <returns>Assets from Hashtable</returns>
        Hashtable ProcessAssets(dynamic data)
        {
            Hashtable result = new Hashtable();
            try
            {
                if (data.errors == null && data.result != null)
                {
                    foreach (string propertyName in GetProperties(data.result))
                    {
                        dynamic propertyValue = data.result[propertyName];
                        Currency currency = new Currency();
                        currency.Symbol = propertyName;
                        currency.Name = propertyValue.altname;
                        currency.Decimals = propertyValue.decimals;
                        currency.Display_decimals = propertyValue.display_decimals;
                        result.Add(propertyName, currency);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        

        #region Helpers
        List<string> GetProperties(dynamic data)
        {
            List<string> result = new List<string>();

            JObject jObject = data;
            Dictionary<string, object> values = jObject.ToObject<Dictionary<string, object>>();
            
            foreach (string key in values.Keys)
            {
                result.Add(key);
            }

            return result;
        }

        Currency GetCurrency(Position position, string symbol)
        {
            string val = string.Empty;
            if(symbol.Length == 6)
            {
                if (position == Position.Second)
                    val = symbol.Substring(((int)position - 1), 3);
                else
                    val = symbol.Substring((int)position, 3);

                // It necessary to research for all currencies
                if (val == "XBT") val = "XXBT";
                if (val == "USD") val = "ZUSD";
                if (val == "EUR") val = "ZEUR";
            }
            else
            {
                val = symbol.Substring((int)position, 4);
            }
           
            return Assets.ContainsKey(val) ? (Currency)Assets[val] : null;
        }

        #endregion

    }
}
