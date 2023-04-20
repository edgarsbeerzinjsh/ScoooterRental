namespace ScooterRental;

public class ScooterIsNotRentedNowException : Exception
{
    public ScooterIsNotRentedNowException() : base ("Scooter with this Id is not in rent.")
    {
        
    }
}