using GoogleBooks.Application.Common.Services;
using GoogleBooks.Application.Common.Validators;
using GoogleBooks.Application.Readers;
using GoogleBooks.Application.Readers.UseCases;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Domain.Exceptions;
using GoogleBooks.Domain.Readers.Entities;
using Moq;

namespace GoogleBooks.Unit.Tests.Readers;

[Collection("Unit tests collection")]
public class CreateReaderUnitTests(TestFactory testFactory)
{
    private readonly Mock<IGoogleBooksValidator<ReaderCreationDto>> _mockedValidatorService = new();
    private readonly Mock<IDateTimeProvider> _mockedDateTimeProvider = new();
    private readonly Mock<IReaderService> _mockedReaderService = new();

    [Fact]
    public async Task Should()
    {
        // arrange
        var address = "666 Evergreen Terrace";
        var birthDate = new DateOnly(1985, 10, 10);
        var city = "Springfield";
        var email = "johndoe@gmail.com";
        var name = "John";
        var lastName = "Doe";
        var zipCode = "65619";

        var readerCreation = new ReaderCreationDto
        {
            Address = address,
            Birthdate = birthDate,
            City = city,
            Email = email,
            Name = name,
            LastName = lastName,
            ZipCode = zipCode
        };

        _mockedValidatorService
            .Setup(_ => _.ValidateAsync(readerCreation, TestContext.Current.CancellationToken))
            .ReturnsAsync(new GoogleBooksValidationResult());

        var expectedDatetime = new DateTime(2025, 10, 26, 22, 00, 00);
        _mockedDateTimeProvider
            .Setup(_ => _.UtcNow())
            .Returns(expectedDatetime);

        var expectedParsedReader = new Reader
        {
            Address = address,
            Birthdate = birthDate,
            City = city,
            CreationDate = expectedDatetime,
            Email = email,
            Name = name,
            LastName = lastName,
            LastUpdate = expectedDatetime,
            ZipCode = zipCode
        };

        var expectedResult = 1;

        _mockedReaderService
            .Setup(_ => _.AddAsync(
                It.Is<Reader>(_ =>
                    _.Address == address &&
                    _.Birthdate == birthDate &&
                    _.City == city &&
                    _.CreationDate == expectedDatetime &&
                    _.Email == email &&
                    _.Name == name &&
                    _.LastName == lastName &&
                    _.ZipCode == zipCode &&
                    _.LastUpdate == expectedDatetime
                ),
                TestContext.Current.CancellationToken))
            .ReturnsAsync(new Reader
            {
                Id = expectedResult,
                Address = address,
                Birthdate = birthDate,
                City = city,
                CreationDate = expectedDatetime,
                Email = email,
                Name = name,
                LastName = lastName,
                LastUpdate = expectedDatetime,
                ZipCode = zipCode
            });

        var sut = new CreateReader(
            _mockedValidatorService.Object,
            _mockedDateTimeProvider.Object,
            _mockedReaderService.Object);

        // act
        var actualResult = await sut.DoAsync(readerCreation, TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);

        _mockedValidatorService.Verify(_ =>
            _.ValidateAsync(
                readerCreation,
                TestContext.Current.CancellationToken),
            Times.Once);

        _mockedDateTimeProvider.Verify(_ => _.UtcNow(), Times.Once);

        _mockedReaderService.Verify(_ =>
            _.AddAsync(It.Is<Reader>(_ =>
                _.Address == address &&
                _.Birthdate == birthDate &&
                _.City == city &&
                _.CreationDate == expectedDatetime &&
                _.Email == email &&
                _.Name == name &&
                _.LastName == lastName &&
                _.ZipCode == zipCode &&
                _.LastUpdate == expectedDatetime
            ),
            TestContext.Current.CancellationToken), Times.Once);
    }

    [Theory]
    [MemberData(nameof(GetEntryDataAndExpectedResult))]
    public async Task Should_ThrowBadRequestException(
        ReaderCreationDto readerToFailValidation,
        BadRequestException expectedException)
    {
        // arrange
        var sut = new CreateReader(
            testFactory.GetRequiredService<IGoogleBooksValidator<ReaderCreationDto>>(),
            _mockedDateTimeProvider.Object,
            _mockedReaderService.Object);

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(readerToFailValidation, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);

        _mockedDateTimeProvider.Verify(_ => _.UtcNow(), Times.Never);
        _mockedReaderService.Verify(_ =>
            _.AddAsync(
                It.IsAny<Reader>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_CallingReaderService()
    {
        // arrange
        var expectedException = new Exception("This is an unhandled exception on the ReaderService");

        _mockedValidatorService
            .Setup(_ =>
                _.ValidateAsync(
                    It.IsAny<ReaderCreationDto>(),
                    TestContext.Current.CancellationToken))
            .ReturnsAsync(new GoogleBooksValidationResult());

        _mockedReaderService
            .Setup(_ => _.AddAsync(It.IsAny<Reader>(), TestContext.Current.CancellationToken))
            .ThrowsAsync(new Exception("This is an unhandled exception on the ReaderService"));

        var sut = new CreateReader(
            _mockedValidatorService.Object,
            _mockedDateTimeProvider.Object,
            _mockedReaderService.Object);

        var expectedDatetime = new DateTime(2025, 10, 26, 22, 00, 00);
        _mockedDateTimeProvider
            .Setup(_ => _.UtcNow())
            .Returns(expectedDatetime);

        var address = "666 Evergreen Terrace";
        var birthDate = new DateOnly(1985, 10, 10);
        var city = "Springfield";
        var email = "johndoe@gmail.com";
        var name = "John";
        var lastName = "Doe";
        var zipCode = "65619";
        var readerCreation = new ReaderCreationDto
        {
            Address = address,
            Birthdate = birthDate,
            City = city,
            Email = email,
            Name = name,
            LastName = lastName,
            ZipCode = zipCode
        };

        // act
        var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(readerCreation, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);

        _mockedReaderService.Verify(_ =>
            _.AddAsync(It.Is<Reader>(_ =>
                _.Address == address &&
                _.Birthdate == birthDate &&
                _.City == city &&
                _.CreationDate == expectedDatetime &&
                _.Email == email &&
                _.Name == name &&
                _.LastName == lastName &&
                _.ZipCode == zipCode &&
                _.LastUpdate == expectedDatetime
            ),
            TestContext.Current.CancellationToken), Times.Once);
    }

    public static IEnumerable<object[]> GetEntryDataAndExpectedResult()
    {
        yield return new object[] {
            new ReaderCreationDto
            {
                Address = "666 Evergreen Terrace",
                Birthdate = new DateOnly(1989, 10, 10),
                City = null,
                Email = "  ",
                LastName = null!,
                Name = string.Empty,
                ZipCode = null
            },

            new BadRequestException(new Dictionary<string, string[]> {
                { "Email", ["Email is mandatory", "Value \u0022  \u0022 is not a valid email address"] },
                { "Name", ["Name is mandatory"] },
                { "LastName", ["Last name is mandatory"] }
            })
        };
    }
}
