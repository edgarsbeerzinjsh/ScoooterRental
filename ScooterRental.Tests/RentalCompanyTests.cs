using FluentAssertions;
using Microsoft.VisualBasic;
using Moq;
using Moq.AutoMock;

namespace ScooterRental.Tests
{
    public class RentalCompanyTests
    {
        private DateTime defaultDate = new DateTime(1, 1, 1);
        private IRentalCompany _company;
        private List<IScooterRent> _rentList;
        private const string CompanyName = "test";
        private AutoMocker _mocker;

        [SetUp]
        public void Setup()
        {
            _rentList = new List<IScooterRent>();
            _mocker = new AutoMocker();
            _company = new RentalCompany(CompanyName, _mocker.GetMock<IScooterService>().Object, _rentList);
        }

        [Test]
        public void CreateRentalCompany_TestAsNameProvided_NameShouldBeTest()
        {
            _company.Name.Should().Be(CompanyName);
        }

        [Test]
        public void StartRent_ValidIdProvided_ScooterIsRented()
        {
            var scooter = new Scooter("1", 1m) {IsRented = false};
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Returns(scooter);

            _company.StartRent("1");

            scooter.IsRented.Should().BeTrue();
            _rentList.Any(x => x.Scooter == scooter).Should().BeTrue();
        }
        
        [Test]
        public void StartRent_ValidIdProvidedButScooterIsRented_ThrowScooterIsInRentException()
        {
            var scooter = new Scooter("1", 1m) {IsRented = true};
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Returns(scooter);

            Action act = () => _company.StartRent("1");
            act.Should().Throw<ScooterIsInRentException>();
        }
        
        [Test]
        public void StartRent_InvalidScooterId_ThrowsScooterNotFoundException()
        {
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Throws<ScooterNotFoundException>();

            Action act = () => _company.StartRent("1");
            act.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void StartRent_IdNullOrEmptyProvided_ThrowsScooterIdNotProvidedException(string value)
        {
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById(value)).Throws<ScooterIdNotProvidedException>();
            
            Action act = () => _company.StartRent(value);
            act.Should().Throw<ScooterIdNotProvidedException>();
        }
        
        [Test]
        public void EndRent_ValidIdProvided_ScooterIsRentedFalse()
        {
            var scooter = new Scooter("1", 1m) {IsRented = true};
            _rentList.Add(new ScooterRent(scooter));
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Returns(scooter);
            
            _company.EndRent("1");

            scooter.IsRented.Should().BeFalse();
        }
        
        [Test]
        public void EndRent_ValidIdProvided_ScooterRentProvidesBill()
        {
            var scooter = new Scooter("1", 1m) {IsRented = true};
            _rentList.Add(new ScooterRent(scooter));
            _rentList[0].RentEnd.Should().Be(null);
            
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Returns(scooter);
            
            var bill = _company.EndRent("1");
            _rentList[0].Bill.Should().Be(bill);
        }
        
        [Test]
        public void EndRent_IdScooterIsAlreadyInRentProvided_ThrowScooterIsNotRentedNowException()
        {
            var scooter = new Scooter("1", 1m) {IsRented = false};
            _rentList.Add(new ScooterRent(scooter));
            
            Action act = () => _company.EndRent("1");
            act.Should().Throw<ScooterIsNotRentedNowException>();
        }
        
        [Test]
        public void EndRent_InvalidScooterId_ThrowsScooterNotFoundException()
        {
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Throws<ScooterNotFoundException>();

            Action act = () => _company.EndRent("1");
            act.Should().Throw<ScooterNotFoundException>();
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void EndRent_IdNullOrEmptyProvided_ThrowsScooterIdNotProvidedException(string value)
        {
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById(value)).Throws<ScooterIdNotProvidedException>();
            
            Action act = () => _company.EndRent(value);
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        private void FillRentedScooterDataList()
        {
            var year1 = new DateTime(1, 1, 1);
            var year2 = new DateTime(2, 1, 1);
            _mocker = new AutoMocker();
            var year1Finished = _mocker.GetMock<IScooterRent>();
            year1Finished.Setup(x => x.RentEnd).Returns(year1);
            year1Finished.Setup(x => x.Bill).Returns(11M);
            year1Finished.Setup(x => x.Scooter).Returns(new Scooter ("1", 1M) {IsRented = false});
            _rentList.Add(year1Finished.Object);

            _mocker = new AutoMocker();
            var year1Rented = _mocker.GetMock<IScooterRent>();
            year1Rented.Setup(x => x.RentEnd).Returns(year1);
            year1Rented.Setup(x => x.CalculateRentedScooterBill(It.IsAny<DateTime>())).Returns(11.1M);
            year1Rented.Setup(x => x.Scooter).Returns(new Scooter ("1", 1M) {IsRented = true});
            _rentList.Add(year1Rented.Object);
            
            _mocker = new AutoMocker();
            var year2Finished = _mocker.GetMock<IScooterRent>();
            year2Finished.Setup(x => x.RentEnd).Returns(year2);
            year2Finished.Setup(x => x.Bill).Returns(22M);
            year2Finished.Setup(x => x.Scooter).Returns(new Scooter ("1", 1M) {IsRented = false});
            _rentList.Add(year2Finished.Object);
            
            _mocker = new AutoMocker();
            var year2Rented = _mocker.GetMock<IScooterRent>();
            year2Rented.Setup(x => x.RentEnd).Returns(year2);
            year2Rented.Setup(x => x.CalculateRentedScooterBill(It.IsAny<DateTime>())).Returns(22.2M);
            year2Rented.Setup(x => x.Scooter).Returns(new Scooter ("1", 1M) {IsRented = true});
            _rentList.Add(year2Rented.Object);
        }

        [Test]
        [TestCase(1, false, 11)]
        [TestCase(1, true, 22.1)]
        [TestCase(null, false, 33)]
        [TestCase(null, true, 66.3)]
        public void CalculateIncome_YearsGiven_CorrectBillExpected(int? year, bool includeNotComplited, decimal bill)
        {
            FillRentedScooterDataList();
            var billYear = _company.CalculateIncome(year, includeNotComplited);

            billYear.Should().Be(bill);
        }
    }
}
