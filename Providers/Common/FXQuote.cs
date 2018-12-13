using System;

namespace Providers
{
    /// <summary>
    /// The FX Quote
    /// </summary>
    public class FXQuote
    {
        /// <summary>
        /// The Ask price
        /// </summary>
        public double Ask { get; set; }

        /// <summary>
        /// The Bid price
        /// </summary>
        public double Bid { get; set; }

        /// <summary>
        /// The Provider
        /// </summary>
        public ProviderEnum? Provider { get; set; }

        /// <summary>
        /// TimeStamp in ServerTime (UTC) when the quote has been recorded in our system
        /// </summary>
        public DateTime ReceivedTS { get; set; }

        /// <summary>
        /// The Symbol
        /// </summary>
        public string Symbol { get; internal set; }

        /// <summary>
        /// The Name
        /// </summary>
        public string Name { get; internal set; }

        public override string ToString()
        {
            return string.Format("FXQuote: --- {0}:{1} Bid:{2} Ask:{3} --{4}", Name, Symbol, Bid, Ask, Provider);
        }
    }

}
