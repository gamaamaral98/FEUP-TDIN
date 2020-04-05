using System;

public class Remote : MarshalByRefObject
{
    public Remote()
    {
        Console.WriteLine("Service Created");
    }

    public string Hello()
    {
        Console.WriteLine("SERVER - Hello called");
        return "CLIENT - Hello .NET client!";
    }
}
