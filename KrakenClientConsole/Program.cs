namespace KrakenClientConsole
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Providers;

    public class Program
    {


        public static void Main(string[] args)
        {
            Execute();
            Console.ReadKey();
        }

        static void Execute()
        {
            //XXBTZUSD,XXRPZUSD,XETHZUSD
            List<string> pairs = new List<string>();
            pairs.Add("XXBTZUSD");
            pairs.Add("XXRPZUSD");
            pairs.Add("XETHZUSD");
            pairs.Add("BCHXBT");
            pairs.Add("BCHUSD");
            pairs.Add("BCHEUR");
            pairs.Add("XLTCZUSD");
            pairs.Add("XXMRZUSD");
            pairs.Add("XZECZUSD");
            pairs.Add("XETCZUSD");
            pairs.Add("XREPZUSD");

            KrakenProvider provider = new KrakenProvider(pairs);
            //provider.DisplayAssets();
            //provider.DisplayQuotes();
            provider.Start();
            provider.Stop();
        }
    }
}
