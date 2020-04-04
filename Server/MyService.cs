using System;

public class MyService : MarshalByRefObject
{
    public MyService()
    {
        Console.WriteLine("Constructor called");
    }

    public string Hello()
    {
        Console.WriteLine("SERVER - Hello called");
        return "Hello .NET client!";
    }
}
