using HtmxDotnet.Models;

public class MultiTabFormViewModel
{
    public HtmxConfig? HtmxConfig { get; set; }
    // Personal Info Tab
    public PersonalInfoViewModel PersonalInfo { get; set; } = new PersonalInfoViewModel();

    // Address Info Tab
    public AddressInfoViewModel AddressInfo { get; set; } = new AddressInfoViewModel();
    public FavoritesInfoViewModel FavoritesInfo { get; set; } = new FavoritesInfoViewModel();
    // Confirmation Tab
    public bool IsConfirmed { get; set; }
}

// Personal Info ViewModel
public class PersonalInfoViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

// Address Info ViewModel
public class AddressInfoViewModel
{
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
}

public class FavoritesInfoViewModel
{
    public string? FavoriteColor { get; set; }
    public string? FavoriteFood { get; set; }
}
