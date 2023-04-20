namespace ScooterRental;

public class ScooterIsInRentException : Exception
{
    public ScooterIsInRentException() : base("Scooter is in rent. You can not remove or rent scooter which is in use.")
    {
        
    }
}