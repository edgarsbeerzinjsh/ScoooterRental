namespace ScooterRental
{
    public class ScooterNotFoundException : Exception
    {
        public ScooterNotFoundException() : base("Scooter with provided ID not found.")
        {

        }
    }
}
