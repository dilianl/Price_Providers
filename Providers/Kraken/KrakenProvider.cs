
using System;
using System.Collections.Generic;
using System.Threading;

namespace Providers
{
    public class KrakenProvider : IQuotesProvider
    {
        List<string> pairs;
        bool started;
        Data data;

        public KrakenProvider(List<string> pairs)
        {
            this.started = true;
            this.pairs = pairs;
            this.data = new Data();
        }

        public FXQuote[] GetLatestQuotes()
        {
            return data.GetLatestQuotes(pairs);
        }

        public ProviderEnum GetProvider()
        {
            return ProviderEnum.Kraken;
        }

        public void Start()
        {
            while (started)
            {
                DisplayQuotes();
            }
        }
        
        public void Stop()
        {
            started = false;
        }

        public void DisplayQuotes()
        {
            double eth = 0.62;  //0.62 //0.1357
            double btc = 0;     //0.0107;  //
            double xrp = 164.285;
            double totalUSD = 0;
            Console.Clear();
            Console.WriteLine("----------------------------------------------------------------------");
            FXQuote[] quotes = GetLatestQuotes();
            foreach (FXQuote quote in quotes)
            {
                if (quote.Symbol == "XETHZUSD" || quote.Symbol == "XXRPZUSD" || quote.Symbol == "XXBTZUSD")
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Time: {0} {1}", quote.ReceivedTS.ToLocalTime().ToString("HH:mm:ss"), quote.ToString());
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Time: {0} {1}", quote.ReceivedTS.ToLocalTime().ToString("HH:mm:ss"), quote.ToString());
                }
                    
                if (quote.Symbol == "XETHZUSD")
                    totalUSD += quote.Ask * eth;

                if (quote.Symbol == "XXRPZUSD")
                    totalUSD += quote.Ask * xrp;

                if (quote.Symbol == "XXBTZUSD")
                    totalUSD += quote.Ask * btc;
            }

            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Total: {0}", totalUSD.ToString("N2")));
            Console.ResetColor();
            Thread.Sleep(20000);
        }

        public void DisplayAssets()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------------------------------------");
            foreach (Currency c in data.Assets.Values)
            {
                Console.WriteLine(c.ToString());
            }
            Console.WriteLine("----------------------------------------------------------------------");
            Thread.Sleep(5000);
        }
    }
}
