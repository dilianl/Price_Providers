namespace Providers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class KrakenClient : IDisposable
    {
        /// <summary>
        /// Costants
        /// </summary>
        const string AssetsAction = "Assets";
        const string TickerAction = "Ticker";

        string url;
        public KrakenClient()
        {
            this.url = ConfigurationManager.AppSettings["KrakenAddress"];
        }
        
        #region API Calls

        private async Task<string> CallApiAsync(string action, string param = null)
        {
            string address = string.Empty;

            if (string.IsNullOrEmpty(param))
            {
                address = string.Format("{0}/public/{1}", url, action);
            }
            else
            {
                address = string.Format("{0}/public/{1}?{2}", url, action, param);
            }


            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(address);
                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary>
        /// Get a list of active assets
        /// </summary>
        /// <returns>The Assets in Json</returns>
        public async Task<string> GetAllAssetsAsync()
        {
            return await CallApiAsync(AssetsAction);
        }

       
        /// <summary>
        /// Get tick for pairs
        /// </summary>
        /// <param name="pairs">List with pairs- XXBTZUSD,XXRPZUSD,XETHZUSD </param>
        /// <returns>The Tick in Json</returns>
        public async Task<string> GetTickAsync(List<string> pairs)
        {
            if (pairs == null || pairs.Count() == 0)
            {
                return null;
            }

            StringBuilder pair = new StringBuilder("pair=");
            foreach (var item in pairs)
            {
                pair.Append(item + ",");
            }
            pair.Length--; 


            return await CallApiAsync(TickerAction, pair.ToString());
        }
        #endregion...

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

