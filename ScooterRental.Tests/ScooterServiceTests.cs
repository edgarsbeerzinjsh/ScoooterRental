using FluentAssertions;

namespace ScooterRental.Tests
{
    public class ScooterServiceTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _scooters;

        [SetUp]
        public void Setup()
        {
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
        }

        [Test]
        public void AddScooter_AddValidScooter_ScooterAdded()
        {
            _scooterService.AddScooter("1", 1m);
            _scooters.Count.Should().Be(1);
        }
        
        [Test]
        public void AddScooter_AddNotUniqueIdScooter_ThrowsNotUniqueScooterIdException()
        {
            _scooterService.AddScooter("1", 1m);
            Action act = () => _scooterService.AddScooter("1", 1m);
            act.Should().Throw<NotUniqueScooterIdException>();
        }

        [Test]
        public void AddScooter_AddScooterWithoutId_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => _scooterService.AddScooter("", 1m);
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void AddScooter_AddScooterWithNonPositivePricePerMinute_ThrowsInvalidPriceException()
        {
            Action act = () => _scooterService.AddScooter("1", -1m);
            act.Should().Throw<InvalidPriceException>();
        }

        [Test]
        public void AddScooter_AddScooterWithNullId_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => _scooterService.AddScooter(null, 1m);
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void RemoveScooter_ValidScooterId_ScooterRemoved()
        {
            _scooters.Add(new Scooter("1", 1m));
            _scooters.Add(new Scooter("2", 1m));
            _scooterService.RemoveScooter("1");
            _scooters.Any(x => x.Id == "1").Should().BeFalse();
        }

        [Test]
        public void RemoveScooter_ScooterInRentProvided_ThrowsScooterIsInRentException()
        {
            _scooters.Add(new Scooter("1", 1m) {IsRented = true});
            var scooter = _scooterService.GetScooterById("1");
            Action act = () => _scooterService.RemoveScooter("1");
            act.Should().Throw<ScooterIsInRentException>();
        }
        
        [Test]
        public void RemoveScooter_IdNullProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => _scooterService.RemoveScooter(null);
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void RemoveScooter_IdEmptyProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => _scooterService.RemoveScooter("");
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void RemoveScooter_InvalidScooterId_ThrowsScooterNotFoundException()
        {
            Action act = () => _scooterService.RemoveScooter("1");
            act.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void GetScooters_ReturnsAllScooters()
        {
            _scooters.Add(new Scooter("1", 1m));
            _scooterService.GetScooters().Count.Should().Be(1);
        }

        [Test]
        public void GetScooterById_ValidScooterId_ScooterReturned()
        {
            _scooters.Add(new Scooter("1", 1m));
            var scooter = _scooterService.GetScooterById("1");
            scooter.Id.Should().Be("1");
        }

        [Test]
        public void GetScooterById_IdNullProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => _scooterService.GetScooterById(null);
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void GetScooterById_IdEmptyProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => _scooterService.GetScooterById("");
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void GetScooterById_InvalidScooterId_ThrowsScooterNotFoundException()
        {
            Action act = () => _scooterService.GetScooterById("1");
            act.Should().Throw<ScooterNotFoundException>();
        }
    }
}