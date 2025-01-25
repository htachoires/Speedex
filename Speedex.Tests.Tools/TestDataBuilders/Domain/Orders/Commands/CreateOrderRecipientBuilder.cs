using Speedex.Domain.Orders.UseCases.CreateOrder;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Commands;

public class CreateOrderRecipientBuilder
{
    private string _firstName = "fooFirstName";
    private string _lastName = "fooLastName";
    private string _email = "fooEmail";
    private string _phone = "fooPhone";
    private string _address = "fooAddress";
    private string _additionalAddress = "fooAdditionalAddress";
    private string _city = "fooCity";
    private string _country = "fooCountry";

    public static CreateOrderRecipientBuilder ACreateOrderRecipient => new();

    
    private CreateOrderRecipientBuilder()
    {
    }

    public CreateOrderRecipientBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public CreateOrderRecipientBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public CreateOrderRecipientBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public CreateOrderRecipientBuilder WithPhone(string phone)
    {
        _phone = phone;
        return this;
    }

    public CreateOrderRecipientBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public CreateOrderRecipientBuilder WithAdditionalAddress(string additionalAddress)
    {
        _additionalAddress = additionalAddress;
        return this;
    }

    public CreateOrderRecipientBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public CreateOrderRecipientBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public CreateOrderCommand.CreateOrderRecipient Build()
    {
        return new CreateOrderCommand.CreateOrderRecipient
        {
            FirstName = _firstName,
            LastName = _lastName,
            Email = _email,
            Phone = _phone,
            Address = _address,
            AdditionalAddress = _additionalAddress,
            City = _city,
            Country = _country
        };
    }
}