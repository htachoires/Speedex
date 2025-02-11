using Speedex.Domain.Orders;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

public class RecipientBuilder
{
    private string _firstName = "fooFirstName";
    private string _lastName = "fooLastName";
    private string _email = "fooEmail";
    private string _phoneNumber = "fooPhoneNumber";
    private string _address = "fooAddress";
    private string? _additionalAddress = "fooAdditionalAddress";
    private string _city = "fooCity";
    private string _country = "fooCountry";

    public static RecipientBuilder ARecipient => new();

    public Recipient Build()
    {
        return new Recipient
        {
            FirstName = _firstName,
            LastName = _lastName,
            Email = _email,
            PhoneNumber = _phoneNumber,
            Address = _address,
            AdditionalAddress = _additionalAddress,
            City = _city,
            Country = _country
        };
    }

    public RecipientBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }
}