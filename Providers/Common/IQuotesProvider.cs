using System;
using System.Collections.Generic;
using System.Text;

namespace Providers
{
    /// <summary>
    /// The Quotes Provider interface
    /// </summary>
    public interface IQuotesProvider
    {
        ProviderEnum GetProvider();
        void Start();
        void Stop();
        FXQuote[] GetLatestQuotes();
    }

}
