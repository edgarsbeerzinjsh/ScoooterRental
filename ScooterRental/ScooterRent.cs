namespace ScooterRental;

public class ScooterRent : IScooterRent
{
    private const int MaxOneDayBill = 20;
    private DateTime _rentStart;

    public DateTime? RentEnd { get; private set; }

    public Scooter Scooter { get; }

    public decimal Bill { get; private set; }

    public ScooterRent(Scooter scooter)
    {
        Scooter = scooter;
        _rentStart = DateTime.Now;
        RentEnd = null;
        Bill = 0;
    }

    public void CalculateBill(DateTime rentEndingDateTime)
    {
        if (_rentStart > rentEndingDateTime)
        {
            throw new EndRentDateIncorrectException();
        }
        
        var currentDay = _rentStart;
        var isLastDay = false;
        do
        {
            var nextDay = currentDay.AddDays(1);
            var nextDayStart = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day);
            isLastDay = (currentDay.Year == rentEndingDateTime.Year &&
                         currentDay.DayOfYear == rentEndingDateTime.DayOfYear);
            if (isLastDay)
            {
                nextDayStart = rentEndingDateTime;
            }
            
            Bill += TimeSpanBill(currentDay, nextDayStart);
            currentDay = nextDayStart;
        } while (!isLastDay);

        RentEnd = rentEndingDateTime;
    }

    public decimal CalculateRentedScooterBill(DateTime timeToEnd)
    {
        CalculateBill(timeToEnd);
        var billForNow = Bill;
        RentEnd = null;
        Bill = 0;
        return billForNow;
    }

    private decimal TimeSpanBill(DateTime startDateTime, DateTime endDateTime)
    {
        var dayRent = endDateTime.Subtract(startDateTime);
        var dayBill = Scooter.PricePerMinute * (decimal)dayRent.TotalMinutes;
        return dayBill > MaxOneDayBill ? MaxOneDayBill : Math.Round(dayBill, 3);
    }
}