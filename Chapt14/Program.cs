using System.Text.Json;
using LaYumba.Functional;
using static System.Console;

// Example1.
var lazayGrandma = () =>
{
    WriteLine("getting grandma..");
    return "grandma";
};

var turnBlue = (string s) =>
{
    WriteLine("turning blue..");
    return $"blue {s}";
};

var lazyGrandmaBlue = lazayGrandma.Map(turnBlue);
lazyGrandmaBlue();

// Example2.

Try<Uri> CreateUri(string uri) => () => new Uri(uri);

WriteLine(CreateUri("http://github.com").Run().ToString());
WriteLine(CreateUri("rubbish").Run().ToString());
    
// Example3.
// unsafe
Uri ExtractUri(string json)
{
    var website = JsonSerializer.Deserialize<Website>(json);

    return new Uri(website.Uri);
}

// safe
Try<T> Parse<T>(string s) => () => JsonSerializer.Deserialize<T>(s);
Try<Uri> ExtractUri2(string json) => Parse<Website>(json).Bind(website => CreateUri(website.Uri));

WriteLine(ExtractUri2(@"{
    ""Name"":""Github"",
    ""Uri"":""http://github.com""
}").Run().ToString());

WriteLine(ExtractUri2(@"{}").Run().ToString());

// Example4.


record Website(string Name, string Uri);

public static class Ext
{
    public static Exceptional<T> Run<T>(this Try<T> f)
    {
        try
        {
            return f();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}