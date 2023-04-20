using FluentAssertions;

namespace ScooterRental.Tests;

public class ScooterRentTests
{
    private ScooterRent _scooterRent;
    private const string Id = "1";
    private const decimal PricePerMinute = 1;
    private Scooter _scooter = new Scooter(Id, PricePerMinute);
        
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateScooter_CreateValidScooterRent_ScooterBillShouldBeZeroAndEndDateNullAndIdAsProvided()
    {
        _scooterRent = new ScooterRent(_scooter);
        _scooterRent.Bill.Should().Be(0);
        _scooterRent.RentEnd.Should().BeNull();
        _scooterRent.Scooter.Id.Should().Be(Id);
    }
    
    [Test]
    public void CalculateBill_RentTo5Min_BillFor5MinGenerated()
    {
        _scooterRent = new ScooterRent(_scooter);
        var endRent = DateTime.Now.AddMinutes(5);
        _scooterRent.CalculateBill(endRent);
        _scooterRent.Bill.Should().Be(_scooter.PricePerMinute * 5);
        _scooterRent.RentEnd.Should().Be(endRent);
    }
    
    [Test]
    public void CalculateBill_EndDateLessThanStartDate_ThrowsEndRentDateIncorrectException()
    {
        _scooterRent = new ScooterRent(_scooter);
        var endRent = new DateTime(2000, 1,1);
        Action act = () => _scooterRent.CalculateBill(endRent);
        act.Should().Throw<EndRentDateIncorrectException>();
    }
    
    [Test]
    public void CalculateBill_RentTo5DaysIfPricePerDayMoreThan20EUR_BillFor5DaysGenerated()
    {
        var highPrice = 20;
        _scooterRent = new ScooterRent(new Scooter(Id, highPrice));
        var endRent = DateTime.Now.AddDays(5);
        _scooterRent.CalculateBill(endRent);
        _scooterRent.Bill.Should().Be(highPrice * 5 + highPrice);
        _scooterRent.RentEnd.Should().Be(endRent);
    }
    
    [Test]
    public void CalculateRentedScooterBill_RentToPricePerDayMoreThan20EUR_BillForEachDayLeftInYearGenerated()
    {
        var highPrice = 20;
        _scooterRent = new ScooterRent(new Scooter(Id, highPrice));
        var endRent = new DateTime(DateTime.Now.Year + 1, 1, 1);
        _scooterRent.CalculateRentedScooterBill(endRent).Should().Be(highPrice * endRent.Subtract(DateTime.Now).Days + highPrice);
        _scooterRent.Bill.Should().Be(0);
        _scooterRent.RentEnd.Should().Be(null);
    }
    
    [Test]
    public void CalculateRentedScooterBill_EndDateLessThanStartDate_ThrowsEndRentDateIncorrectException()
    {
        _scooterRent = new ScooterRent(_scooter);
        var endRent = new DateTime(2000, 1,1);
        Action act = () => _scooterRent.CalculateRentedScooterBill(endRent);
        act.Should().Throw<EndRentDateIncorrectException>();
    }
}