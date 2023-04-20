namespace ScooterRental;

public class NotUniqueScooterIdException : Exception
{
    public NotUniqueScooterIdException() : base("Scooter with this Id already exists. Id must be unique.")
    {
        
    }
}