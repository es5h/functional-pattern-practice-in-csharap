using LaYumba.Functional;
using static System.Console;
using static LaYumba.Functional.F;
using Rates = System.Collections.Immutable.ImmutableDictionary<string,decimal>;
    
static class Program
{
    public static void Main(string[] args)
        => MainRec("Enter a currency pair like 'EURUSD', or 'q' to quit", Rates.Empty);    
    
    static void MainRec(string Message, Rates cache)
    {
        var input = ReadLine().ToUpper();
        if (input.Equals("Q")) return;
    
        var (rate, newState) = GetRate(input, cache);
        WriteLine(rate);
        MainRec(newState);
    }

// string -> Rates -> (decimal, Rates)
    static Try<(decimal Rate, Rates NewState)> GetRate(Func<string, Try<decimal>> getRate, string ccyPair, Rates cache)
    {
        if (cache.ContainsKey(ccyPair))
        {
            return Try(() => (cache[ccyPair], cache));
        }
        else
        {
            return getRate(ccyPair).Select(rate => (rate, cache.Add(ccyPair, rate)));
        }
    }
}

static class RatesApi
{
    public static decimal GetRate(string ccyPair)
    {
        //
        return default;
    }
}