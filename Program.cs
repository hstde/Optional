using System;

namespace Optional;

public static class Program
{
    public static int Main(string[] args)
    {
        var opt = Optional.None<int>();
        Console.WriteLine(opt.Any());
        Console.WriteLine(opt.Where(e => e > 12).Any());
        Console.WriteLine(opt.Select(e => e + 12).ValueOr(-1));
        Console.WriteLine(opt.Or(1337).ExpectValue());
        Console.WriteLine(opt.Intersect(13.Some()));

        opt.OnAny(() => Console.WriteLine("opt has a value"))
            .OnNone(() => Console.WriteLine("opt has no value"));

        opt.Where(e => e > 12)
            .OnAny(() => Console.WriteLine("opt was larger than 12"))
            .OnNone(() => Console.WriteLine("opt was not larger than 12"));
        
        opt.Select(e => e + 12)
            .OnAny(e => Console.WriteLine($"opt + 12 = {e}"))
            .Or(-1)
            .OnAny(e => Console.WriteLine($"The final value is {e}"));
        
        opt
            .Intersect(13.Some())
            .OnAny(v => Console.WriteLine($"{v.Item1} : {v.Item2}"))
            .Select(v => v.Item1 + v.Item2)
            .OnAny(v => Console.WriteLine($"The sum was {v}"));



        var germanUser = new User { Address = new Address { Country = "Germany" }};
        var indianUser = new User { Address = new Address { Country = "India" }};
        var nullUser = new User();
        UserTest(null);
        UserTest(nullUser);
        UserTest(germanUser);
        UserTest(indianUser);
        return 0;
    }

    public static void UserTest(User user)
    {
        Optional.Some(user)
            .Select(u => u.Address)
            .Where(a => a.Country.Equals("India", StringComparison.OrdinalIgnoreCase))
            .OnAny(() => Console.WriteLine("User is in India"))
            .OnNone(() => Console.WriteLine("User is not in India"));

        Optional.Some(user)
            .Select(u => u.GetMaybeAddress())
            .Where(a => a.Country.Equals("India", StringComparison.OrdinalIgnoreCase))
            .OnAny(() => Console.WriteLine("User is in India"))
            .OnNone(() => Console.WriteLine("User is not in India"));
    }
}

public class User
{
    public Address Address { get; set; }

    public Optional<Address> GetMaybeAddress()
    {
        return Optional.Some(Address);
    }
}

public class Address
{
    public string Country { get; set; }
}