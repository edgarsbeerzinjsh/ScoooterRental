namespace ScooterRental;

public class EndRentDateIncorrectException : Exception
{
    public EndRentDateIncorrectException() : base("Scooter rents ending date and time can not be earlier than its start rent time.")
    {
        
    }
}