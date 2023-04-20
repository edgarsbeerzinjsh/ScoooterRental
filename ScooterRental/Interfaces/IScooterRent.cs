namespace ScooterRental;

public interface IScooterRent
{
    DateTime? RentEnd { get; }
    Scooter Scooter { get; }
    decimal Bill { get; }

    void CalculateBill(DateTime rentEndingDateTime);
    decimal CalculateRentedScooterBill(DateTime timeToEnd);
}