using System.Collections.Concurrent;
using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Data.Generators;

public class OrdersGenerator(IDataGenerator<ProductId, Product> productGenerator) : IDataGenerator<OrderId, Order>
{
    public Dictionary<OrderId, Order> Data { get; private set; }

    private readonly Random _random = new();

    #region Fake Data

    private readonly List<string> _firstNames =
        ["Emma", "Liam", "Olivia", "Noah", "Sophia", "James", "Isabella", "Elijah", "Ava", "Lucas", "Margaux", "Thomas", "Ambre", "Ana"];

    private readonly List<string> _lastNames =
        ["Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Martinez", "Hernandez"];

    private readonly List<Tuple<string, List<string>, string>> _locations =
    [
        new("France", ["Paris", "Lyon", "Marseille"], "33"),
        new("USA", ["New York", "Los Angeles", "Chicago"], "1"),
        new("Japan", ["Tokyo", "Osaka", "Kyoto"], "81"),
        new("Australia", ["Sydney", "Melbourne", "Brisbane"], "61"),
        new("Germany", ["Berlin", "Munich", "Hamburg"], "49"),
        new("Canada", ["Toronto", "Vancouver", "Montreal"], "1"),
        new("Italy", ["Rome", "Milan", "Venice"], "39"),
        new("South Africa", ["Cape Town", "Johannesburg", "Durban"], "27"),
        new("Thailand", ["Bangkok", "Chiang Mai", "Phuket"], "66"),
        new("UAE", ["Dubai", "Abu Dhabi", "Sharjah"], "971"),
        new("Spain", ["Madrid", "Tenerife", "Barcelona"], "34"),
    ];

    private readonly List<string> _streetTypes =
        ["Street", "Avenue", "Boulevard", "Road", "Lane", "Drive", "Way", "Place"];

    private readonly List<string> _streetParts =
        ["Main", "Park", "Oak", "Pine", "Elm", "Maple", "Sunset", "Hill", "River", "Lake"];

    #endregion Fake Data

    public void GenerateData(int nbElements)
    {
        var concurrentData = new ConcurrentDictionary<OrderId, Order>();

        Enumerable
            .Range(0, nbElements)
            .AsParallel()
            .ForAll(
                x =>
                {
                    var order = GenerateOrder(x);
                    concurrentData.TryAdd(order.OrderId, order);
                });

        Data = concurrentData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private Order GenerateOrder(int index)
    {
        var nbProducts = _random.Next(1, 10);
        var firstName = _firstNames[_random.Next(0, _firstNames.Count - 1)];
        var lastName = _lastNames[_random.Next(0, _lastNames.Count - 1)];
        var location = _locations[_random.Next(0, _locations.Count - 1)];
        var email = $"{firstName}{lastName}@speedex.com";

        var phoneNumber = $"+{location.Item3} ({_random.Next(100, 1000)}) {_random.Next(1000000, 10000000).ToString()}";

        var buildingNumber = _random.Next(1, 500);
        var streetType = _streetTypes[_random.Next(_streetTypes.Count - 1)];
        var streetParts = _streetParts[_random.Next(_streetParts.Count - 1)];

        var address = $"{buildingNumber} {streetType} {streetParts}";

        var products = Enumerable
            .Range(0, nbProducts * 2)
            .Select(x => _random.Next(0, productGenerator.Data.Count))
            .Distinct()
            .Take(nbProducts)
            .Select(x => productGenerator.Data.ElementAt(x).Value)
            .ToList();

        var orderProducts = products.Select(
            x => new OrderProduct
            {
                ProductId = x.ProductId,
                Quantity = _random.Next(1, 5),
            }).ToList();

        var totalAmount = new Price
        {
            Amount = 0,
            Currency = Currency.EUR
        };

        foreach (var commandProduct in orderProducts)
        {
            var product = products.Single(x => x.ProductId == commandProduct.ProductId);
            totalAmount = new Price
            {
                Amount = totalAmount.Amount + (product.Price.ToEUR().Amount * commandProduct.Quantity),
                Currency = Currency.EUR
            };
        }

        return new Order
        {
            OrderId = new OrderId($"OR_{index}_{GenerateHexadecimal(10)}"),
            Products = orderProducts,
            DeliveryType = (DeliveryType)_random.Next(0, 2),
            Recipient = new Recipient
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                AdditionalAddress = null,
                City = location.Item2[_random.Next(0, location.Item2.Count - 1)],
                Country = location.Item1,
            },
            CreationDate = DateTime.Today,
            UpdateDate = DateTime.Today,
            TotalAmount = totalAmount
        };
    }

    public int GetPriority()
    {
        return 20;
    }
}