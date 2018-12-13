
namespace Providers
{
    public class Currency
    {
        public string Symbol { get; set; }
        public string Name { get; set; }

        public int Decimals { get; set; }
        public int Display_decimals { get; set; }

        public override string ToString()
        {
            return string.Format(" {0} {1} Decimals:{2} Display_decimals:{3}", Symbol, Name, Decimals, Display_decimals);
        }

    }
}
