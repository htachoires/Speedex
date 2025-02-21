using System.Collections.Concurrent;
using Speedex.Domain.Products;

namespace Speedex.Data.Generators;

public class ProductsGenerator : IDataGenerator<ProductId, Product>
{
    public Dictionary<ProductId, Product> Data { get; private set; } = new();
    private readonly Random _random = new();

    #region Fake Data

    private readonly List<Tuple<string, string, string, string, string>> _productsInfo =
    [
        new(
            "Wireless Bluetooth Headphones",
            "High-quality sound with noise-canceling feature for a better listening experience.",
            "Electronics",
            "Audio",
            "Headphones"),
        new(
            "Stainless Steel Water Bottle",
            "Durable and keeps your drinks cool for hours, perfect for daily use.",
            "Home & Kitchen",
            "Drinkware",
            "Bottles"),
        new(
            "Running Shoes",
            "Lightweight, comfortable, and designed for high performance during your runs.",
            "Sportswear",
            "Footwear",
            "Shoes"),
        new(
            "Smartphone 128GB",
            "A powerful smartphone with a stunning 6.5-inch display and 128GB of storage.",
            "Electronics",
            "Mobile Phones",
            "Smartphones"),
        new(
            "Ergonomic Office Chair",
            "Designed for maximum comfort and support during long hours of work.",
            "Furniture",
            "Office Furniture",
            "Chairs"),
        new(
            "Electric Pressure Cooker",
            "Multi-functional pressure cooker, ideal for cooking meals quickly.",
            "Kitchen Appliances",
            "Cookware",
            "Pressure Cookers"),
        new(
            "4K Ultra HD TV",
            "Enjoy an immersive experience with a 55-inch 4K UHD TV and crisp visuals.",
            "Electronics",
            "Home Entertainment",
            "Televisions"),
        new(
            "Wireless Smart Speaker",
            "Compact and portable smart speaker with voice control and premium sound quality.",
            "Electronics",
            "Audio",
            "Speakers"),
        new(
            "LED Desk Lamp",
            "Adjustable LED lamp with multiple brightness settings, perfect for any workspace.",
            "Furniture",
            "Lighting",
            "Lamps"),
        new(
            "Laptop Bag",
            "Stylish, durable, and spacious laptop bag with multiple compartments for accessories.",
            "Accessories",
            "Bags",
            "Laptop Bags"),
        new(
            "Electric Toothbrush",
            "A powerful electric toothbrush for a deep and effective cleaning.",
            "Health & Personal Care",
            "Personal Care",
            "Toothbrushes"),
        new(
            "Smartwatch",
            "Track your health and fitness with this modern smartwatch with heart rate monitor.",
            "Electronics",
            "Wearables",
            "Smartwatches"),
        new(
            "Bluetooth Car Kit",
            "Hands-free Bluetooth car kit for safe driving and easy calls.",
            "Electronics",
            "Car Accessories",
            "Bluetooth Devices"),
        new(
            "Yoga Mat",
            "High-quality yoga mat with extra cushioning for comfort during exercise.",
            "Sports & Outdoors",
            "Fitness Equipment",
            "Yoga"),
        new(
            "Portable Charger",
            "Compact and powerful portable charger for all your devices.",
            "Electronics",
            "Chargers",
            "Power Banks"),
        new(
            "Air Purifier",
            "Improve your indoor air quality with this advanced air purifier.",
            "Home Appliances",
            "Air Quality",
            "Purifiers"),
        new(
            "Shoe Rack",
            "Space-saving shoe rack with multiple shelves to organize your footwear.",
            "Furniture",
            "Storage",
            "Shoe Racks"),
        new(
            "Coffee Maker",
            "Enjoy fresh coffee every morning with this easy-to-use coffee maker.",
            "Home Appliances",
            "Kitchen Appliances",
            "Coffee Machines"),
        new(
            "Blender",
            "Blend smoothies, soups, and more with this powerful blender.",
            "Home Appliances",
            "Kitchen Appliances",
            "Blenders"),
        new(
            "Backpack",
            "Durable and spacious backpack for work, travel, and outdoor adventures.",
            "Accessories",
            "Bags",
            "Backpacks"),
        new(
            "Gaming Mouse",
            "Precision gaming mouse with customizable buttons and RGB lighting.",
            "Electronics",
            "Gaming",
            "Mice"),
        new(
            "Fitness Tracker",
            "Track your steps, calories, and sleep with this advanced fitness tracker.",
            "Electronics",
            "Wearables",
            "Fitness Trackers"),
        new(
            "Non-stick Frying Pan",
            "Cook your meals without the mess, using this non-stick frying pan.",
            "Home & Kitchen",
            "Cookware",
            "Pans"),
        new(
            "Smart Home Security Camera",
            "Keep your home safe with this smart security camera that offers live streaming and motion detection.",
            "Electronics",
            "Smart Home",
            "Cameras"),
        new(
            "Electric Grill",
            "Grill your favorite foods indoors with this compact and easy-to-use electric grill.",
            "Home Appliances",
            "Kitchen Appliances",
            "Grills"),
        new(
            "Instant Pot",
            "A multi-functional cooker that does the job of a pressure cooker, slow cooker, and more.",
            "Kitchen Appliances",
            "Cookware",
            "Instant Pots"),
        new(
            "Mini Refrigerator",
            "Compact and energy-efficient mini refrigerator, ideal for small spaces.",
            "Home Appliances",
            "Refrigeration",
            "Mini Fridges"),
        new("Pet Bed", "Soft and comfortable bed for your pets to sleep in.", "Pet Supplies", "Pet Furniture", "Beds"),
        new(
            "LED Light Bulbs",
            "Energy-saving LED light bulbs that last longer and provide bright light.",
            "Home Improvement",
            "Lighting",
            "Light Bulbs"),
        new(
            "Digital Camera",
            "Capture high-quality photos and videos with this compact digital camera.",
            "Electronics",
            "Cameras",
            "Digital Cameras"),
        new("Pillow", "Supportive pillow for a comfortable night's sleep.", "Home & Kitchen", "Bedding", "Pillows"),
        new(
            "Scented Candles",
            "Fill your home with relaxing aromas using these beautifully-scented candles.",
            "Home & Kitchen",
            "Decor",
            "Candles"),
        new(
            "Table Lamp",
            "Elegant table lamp that adds a touch of style and brightness to any room.",
            "Furniture",
            "Lighting",
            "Table Lamps"),
        new(
            "Massage Gun",
            "Powerful massage gun that helps relieve muscle tension and soreness.",
            "Health & Personal Care",
            "Massage",
            "Massage Guns"),
        new(
            "Portable Speaker",
            "Take your music anywhere with this lightweight and portable speaker.",
            "Electronics",
            "Audio",
            "Speakers"),
        new(
            "Electric Heater",
            "Efficient electric heater for heating small rooms during the colder months.",
            "Home Appliances",
            "Heating",
            "Heaters"),
        new(
            "Bluetooth Headset",
            "Comfortable and wireless Bluetooth headset for hands-free calls and music.",
            "Electronics",
            "Audio",
            "Headsets"),
        new(
            "Camping Tent",
            "Durable camping tent that accommodates up to four people.",
            "Sports & Outdoors",
            "Camping & Hiking",
            "Tents"),
        new(
            "Fishing Rod",
            "Lightweight fishing rod perfect for anglers of all levels.",
            "Sports & Outdoors",
            "Fishing",
            "Rods"),
        new(
            "Cooking Utensil Set",
            "Complete cooking utensil set including spatulas, ladles, and more.",
            "Home & Kitchen",
            "Cookware",
            "Utensils"),
        new(
            "Smart Thermostat",
            "Control your home's temperature remotely with this smart thermostat.",
            "Electronics",
            "Smart Home",
            "Thermostats"),
        new(
            "Food Processor",
            "Chop, slice, and dice with ease using this versatile food processor.",
            "Home Appliances",
            "Kitchen Appliances",
            "Food Processors"),
        new(
            "Coffee Grinder",
            "Grind your coffee beans fresh every time with this easy-to-use coffee grinder.",
            "Home Appliances",
            "Kitchen Appliances",
            "Grinders"),
        new(
            "Blender Bottle",
            "Portable bottle with a built-in shaker for protein shakes and smoothies.",
            "Sports & Outdoors",
            "Fitness",
            "Bottles"),
        new(
            "Wall Clock",
            "Modern and stylish wall clock to enhance the look of any room.",
            "Furniture",
            "Decor",
            "Clocks"),
        new(
            "Smart Door Lock",
            "Control access to your home with this advanced smart door lock.",
            "Electronics",
            "Smart Home",
            "Locks"),
        new(
            "Electric Toothbrush",
            "Rechargeable electric toothbrush with multiple brushing modes.",
            "Health & Personal Care",
            "Oral Care",
            "Toothbrushes"),
        new(
            "Cordless Vacuum Cleaner",
            "Powerful cordless vacuum cleaner for quick and easy cleaning.",
            "Home Appliances",
            "Cleaning",
            "Vacuum Cleaners"),
        new(
            "Waterproof Bluetooth Speaker",
            "Enjoy music outdoors without worrying about water damage.",
            "Electronics",
            "Audio",
            "Speakers"),
        new(
            "Electric Kettle",
            "Boil water quickly with this electric kettle that shuts off automatically.",
            "Home Appliances",
            "Kitchen Appliances",
            "Kettles"),
        new(
            "Double Bed Frame",
            "Stylish and sturdy double bed frame to enhance your bedroom.",
            "Furniture",
            "Bedroom Furniture",
            "Bed Frames"),
        new(
            "Cordless Drill",
            "Compact and powerful cordless drill for all your home projects.",
            "Tools",
            "Power Tools",
            "Drills"),
        new(
            "Smart Light Bulb",
            "Control the color and brightness of your lights remotely with this smart bulb.",
            "Electronics",
            "Smart Home",
            "Lighting"),
        new(
            "Electric Rice Cooker",
            "Perfectly cooked rice every time with this electric rice cooker.",
            "Home Appliances",
            "Kitchen Appliances",
            "Cookers"),
        new(
            "Bicycle",
            "High-quality bicycle with durable tires and a comfortable seat.",
            "Sports & Outdoors",
            "Cycling",
            "Bikes"),
        new(
            "Ice Cream Maker",
            "Make homemade ice cream in minutes with this easy-to-use ice cream maker.",
            "Home Appliances",
            "Kitchen Appliances",
            "Ice Cream Makers"),
        new(
            "Standing Desk",
            "Adjustable standing desk to help you work comfortably and improve posture.",
            "Furniture",
            "Office Furniture",
            "Desks"),
        new(
            "Portable Power Bank",
            "Charge your devices on the go with this portable power bank.",
            "Electronics",
            "Chargers",
            "Power Banks"),
        new(
            "Portable Air Conditioner",
            "Compact and portable air conditioner that cools small rooms efficiently.",
            "Home Appliances",
            "Cooling",
            "Air Conditioners"),
        new(
            "Smart Home Hub",
            "Control all your smart devices with this central home hub.",
            "Electronics",
            "Smart Home",
            "Hubs"),
        new(
            "Compact Refrigerator",
            "Small and efficient refrigerator for tight spaces like offices or dorm rooms.",
            "Home Appliances",
            "Refrigeration",
            "Fridges"),
        new(
            "Adjustable Dumbbells",
            "Space-saving adjustable dumbbells for home workouts.",
            "Sports & Outdoors",
            "Fitness Equipment",
            "Weights"),
        new(
            "Tabletop Water Fountain",
            "Bring tranquility to your space with this peaceful tabletop water fountain.",
            "Home & Kitchen",
            "Decor",
            "Fountains"),
        new(
            "Cordless Hair Clippers",
            "High-performance cordless hair clippers with adjustable blades for precise cuts.",
            "Health & Personal Care",
            "Hair Care",
            "Clippers"),
        new(
            "Inflatable Pool",
            "Easy-to-setup inflatable pool for fun summer days.",
            "Sports & Outdoors",
            "Outdoor Recreation",
            "Pools"),
        new(
            "Wireless Gaming Headset",
            "Wireless headset designed for immersive gaming experiences with superior sound quality.",
            "Electronics",
            "Gaming",
            "Headsets"),
        new(
            "Travel Backpack",
            "Spacious and durable backpack for your travel essentials.",
            "Accessories",
            "Bags",
            "Backpacks"),
        new(
            "Motion Sensor Light",
            "Automatically lights up when movement is detected, perfect for hallways or stairs.",
            "Home & Kitchen",
            "Lighting",
            "Lights"),
        new(
            "Portable Fridge",
            "Keep your beverages cool on the go with this portable fridge.",
            "Home Appliances",
            "Refrigeration",
            "Portable Fridges"),
        new(
            "Smart Security System",
            "A smart security system that provides real-time alerts and remote monitoring.",
            "Electronics",
            "Smart Home",
            "Security Systems"),
        new(
            "Outdoor Fire Pit",
            "Create a cozy atmosphere in your backyard with this outdoor fire pit.",
            "Furniture",
            "Outdoor Furniture",
            "Fire Pits"),
        new(
            "Coffee Table",
            "A modern coffee table that adds style and function to your living room.",
            "Furniture",
            "Living Room Furniture",
            "Tables"),
        new(
            "Electric Grill Pan",
            "Compact electric grill pan for indoor grilling without the smoke.",
            "Home Appliances",
            "Kitchen Appliances",
            "Grills"),
        new(
            "Electric Bicycle",
            "Eco-friendly and efficient electric bicycle for daily commutes or weekend rides.",
            "Sports & Outdoors",
            "Cycling",
            "Bikes"),
        new(
            "Wireless Charger",
            "Charge your devices wirelessly with this fast and convenient charger.",
            "Electronics",
            "Chargers",
            "Wireless Chargers"),
        new(
            "Smart Mirror",
            "A mirror with built-in technology for daily news, weather updates, and more.",
            "Electronics",
            "Smart Home",
            "Mirrors"),
        new(
            "Digital Kitchen Scale",
            "Precise digital scale for accurate ingredient measurements in your kitchen.",
            "Home & Kitchen",
            "Kitchen Tools",
            "Scales"),
        new(
            "Heavy Duty Extension Cord",
            "Durable extension cord for powering multiple devices with ease.",
            "Tools",
            "Electrical",
            "Extension Cords"),
        new(
            "Adjustable Standing Desk",
            "Height-adjustable desk for comfortable working whether sitting or standing.",
            "Furniture",
            "Office Furniture",
            "Desks"),
        new(
            "Wireless Earbuds",
            "True wireless earbuds with great sound quality and long-lasting battery life.",
            "Electronics",
            "Audio",
            "Earbuds"),
        new(
            "Water Filter Pitcher",
            "Filter and purify your water easily with this water filter pitcher.",
            "Home & Kitchen",
            "Kitchen Tools",
            "Water Filters"),
        new(
            "Electric Wine Opener",
            "Easily open wine bottles with this electric wine opener.",
            "Home & Kitchen",
            "Kitchen Tools",
            "Wine Openers"),
        new(
            "Automatic Pet Feeder",
            "Keep your pets fed on time with this automatic pet feeder.",
            "Pet Supplies",
            "Pet Food",
            "Feeders"),
        new(
            "Foldable Drone",
            "A compact and foldable drone with HD camera for aerial photography.",
            "Electronics",
            "Drones",
            "Camera Drones"),
        new(
            "Candle Warmer",
            "Enjoy the aroma of candles without lighting them with this candle warmer.",
            "Home & Kitchen",
            "Decor",
            "Candles"),
        new(
            "Solar Garden Lights",
            "Energy-efficient solar-powered lights to brighten up your garden.",
            "Home & Kitchen",
            "Outdoor Decor",
            "Garden Lights"),
        new(
            "Smart Doorbell",
            "See who’s at your door and communicate with them through the app.",
            "Electronics",
            "Smart Home",
            "Doorbells"),
        new(
            "Electric Pressure Washer",
            "Powerful electric pressure washer for outdoor cleaning tasks.",
            "Tools",
            "Cleaning Tools",
            "Pressure Washers"),
        new(
            "Cordless Handheld Vacuum",
            "Lightweight and portable handheld vacuum for quick cleaning.",
            "Home Appliances",
            "Cleaning",
            "Vacuum Cleaners"),
        new(
            "Portable Hammock",
            "Relax and enjoy the outdoors with this easy-to-setup portable hammock.",
            "Sports & Outdoors",
            "Camping & Hiking",
            "Hammocks"),
        new(
            "Yoga Block Set",
            "Enhance your yoga practice with this set of supportive yoga blocks.",
            "Sports & Outdoors",
            "Fitness",
            "Yoga Blocks"),
        new(
            "Car Seat Organizer",
            "Keep your car neat and tidy with this car seat organizer.",
            "Automotive",
            "Car Accessories",
            "Organizers"),
        new(
            "Smart Fridge",
            "A fridge with built-in smart features, allowing for inventory tracking and remote control.",
            "Home Appliances",
            "Refrigeration",
            "Fridges"),
        new(
            "Electric Scooters",
            "Eco-friendly and fast electric scooters for urban commuting.",
            "Sports & Outdoors",
            "Cycling",
            "Scooters"),
        new(
            "Electric Cooktop",
            "Efficient and easy-to-use electric cooktop for precise temperature control.",
            "Home Appliances",
            "Kitchen Appliances",
            "Cooktops"),
        new(
            "Portable Water Bottle",
            "Take your water with you on the go with this portable and reusable water bottle.",
            "Home & Kitchen",
            "Drinkware",
            "Water Bottles"),
        new(
            "Outdoor String Lights",
            "Create a cozy atmosphere outside with these decorative string lights.",
            "Home & Kitchen",
            "Outdoor Decor",
            "Lights"),
        new(
            "Bamboo Cutting Board",
            "Eco-friendly bamboo cutting board that is durable and easy to clean.",
            "Home & Kitchen",
            "Kitchen Tools",
            "Cutting Boards"),
        new(
            "Cordless Electric Screwdriver",
            "Effortlessly screw and unscrew with this cordless electric screwdriver.",
            "Tools",
            "Hand Tools",
            "Screwdrivers"),
        new(
            "Smart Thermostat",
            "Control your home's heating and cooling from your phone with this smart thermostat.",
            "Electronics",
            "Smart Home",
            "Thermostats"),
        new(
            "Memory Foam Mattress",
            "Comfortable and supportive memory foam mattress for a good night’s sleep.",
            "Furniture",
            "Bedroom Furniture",
            "Mattresses"),
        new(
            "Robot Vacuum",
            "Let the robot do the cleaning for you with this efficient robotic vacuum.",
            "Home Appliances",
            "Cleaning",
            "Vacuum Cleaners"),
        new(
            "Cordless Electric Kettle",
            "Boil water with ease and without the hassle of cords using this electric kettle.",
            "Home Appliances",
            "Kitchen Appliances",
            "Kettles"),
        new(
            "Electric Food Dehydrator",
            "Preserve food for longer with this efficient electric food dehydrator.",
            "Home Appliances",
            "Kitchen Appliances",
            "Dehydrators"),
        new(
            "Smart Garden System",
            "Grow fresh herbs and vegetables indoors with this smart garden system.",
            "Home & Kitchen",
            "Gardening",
            "Indoor Gardens"),
        new(
            "Wireless Bluetooth Keyboard",
            "Type comfortably with this slim and portable wireless Bluetooth keyboard.",
            "Electronics",
            "Computer Accessories",
            "Keyboards"),
        new(
            "Wall-Mounted Shelf",
            "Space-saving wall-mounted shelf for storing books, plants, and other decor.",
            "Furniture",
            "Storage",
            "Shelves"),
        new(
            "Cordless Leaf Blower",
            "Keep your yard clean and tidy with this cordless leaf blower.",
            "Tools",
            "Outdoor Tools",
            "Blowers"),
        new(
            "Bike Lock",
            "Secure your bike with this durable and reliable bike lock.",
            "Sports & Outdoors",
            "Cycling",
            "Locks"),
        new(
            "Smart Wristband",
            "Track your fitness and health with this advanced smart wristband.",
            "Electronics",
            "Wearables",
            "Wristbands"),
        new(
            "Electric Pressure Cooker",
            "Quickly cook meals with this versatile and easy-to-use electric pressure cooker.",
            "Home Appliances",
            "Cookware",
            "Pressure Cookers"),
        new(
            "Adjustable Treadmill",
            "Stay active at home with this adjustable and foldable treadmill.",
            "Sports & Outdoors",
            "Fitness",
            "Treadmills"),
        new(
            "Home Theater System",
            "Immerse yourself in high-quality sound with this complete home theater system.",
            "Electronics",
            "Home Entertainment",
            "Sound Systems"),
        new(
            "Smart Lock Box",
            "Securely store your keys or other small items with this smart lock box.",
            "Electronics",
            "Smart Home",
            "Storage"),
        new(
            "Exercise Ball",
            "Improve your core strength and balance with this durable exercise ball.",
            "Sports & Outdoors",
            "Fitness",
            "Exercise Equipment"),
        new(
            "Pet Tracking Collar",
            "Keep track of your pet’s location with this GPS-enabled tracking collar.",
            "Pet Supplies",
            "Pet Accessories",
            "Collars"),
        new(
            "UV Sterilizer",
            "Sterilize and disinfect everyday items with this UV sterilizer.",
            "Health & Personal Care",
            "Personal Care",
            "Sterilizers"),
        new(
            "Thermal Coffee Mug",
            "Keep your coffee hot for hours with this insulated thermal coffee mug.",
            "Home & Kitchen",
            "Drinkware",
            "Mugs"),
        new(
            "Adjustable Gaming Chair",
            "Comfortable and adjustable gaming chair for long gaming sessions.",
            "Furniture",
            "Office Furniture",
            "Chairs"),
        new(
            "Compact Air Purifier",
            "Improve your indoor air quality with this compact air purifier.",
            "Health & Personal Care",
            "Air Quality",
            "Purifiers")
    ];

    #endregion Fake Data

    public void GenerateData(int nbElements)
    {
        var concurrentData = new ConcurrentDictionary<ProductId, Product>();

        Enumerable
            .Range(0, nbElements)
            .AsParallel()
            .ForAll(
                x =>
                {
                    var product = GenerateProduct(x);
                    concurrentData.TryAdd(product.ProductId, product);
                });

        Data = concurrentData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private Product GenerateProduct(int index)
    {
        var dimensions = Enumerable
            .Range(1, 3)
            .Select(_ => _random.Next(10, 100) / 100d)
            .Order()
            .ToArray();

        var productInfo = _productsInfo[_random.Next(_productsInfo.Count - 1)];

        return new Product
        {
            ProductId = new ProductId($"PR_{index}_{GenerateHexadecimal(10)}"),
            Name = productInfo.Item1,
            Description = productInfo.Item2,
            Category = productInfo.Item3,
            SecondLevelCategory = productInfo.Item4,
            ThirdLevelCategory = productInfo.Item5,

            Price = new Price
            {
                Amount = (decimal)Math.Round(_random.Next(1_000) + _random.NextDouble(), 2),
                Currency = Currency.EUR
            },
            Dimensions = new Dimensions
            {
                X = dimensions[0],
                Y = dimensions[1],
                Z = dimensions[2],
                Unit = DimensionUnit.M
            },
            Weight = new Weight
            {
                Value = Math.Round(_random.Next(10, 3_000) / 1_000f, 3),
                Unit = WeightUnit.Kg
            },
            CreationDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
    }
}