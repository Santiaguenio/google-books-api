using GoogleBooks.Application.Readers;
using GoogleBooks.Application.Readers.UseCases;
using GoogleBooks.Contracts.Responses.Readers;
using GoogleBooks.Domain.Exceptions;
using GoogleBooks.Domain.Readers.Entities;
using Moq;

namespace GoogleBooks.Unit.Tests.Readers;

[Collection("Unit tests collection")]
public class GetReaderByIdUnitTests()
{
    private readonly Mock<IReaderService> _mockedReaderService = new();

    [Fact]
    public async Task Should()
    {
        // arrange
        var address = "666 Evergreen Terrace";
        var birthDate = new DateOnly(1985, 10, 10);
        var city = "Springfield";
        var email = "johndoe@gmail.com";
        var id = 1;
        var name = "John";
        var lastName = "Doe";
        var zipCode = "65619";

        var mockedReader = new Reader
        {
            Address = address,
            Birthdate = birthDate,
            City = city,
            Email = email,
            Id = id,
            Name = name,
            LastName = lastName,
            ZipCode = zipCode,
        };

        var expectedResult = new ReaderDto
        {
            Address = address,
            Birthdate = birthDate,
            City = city,
            Email = email,
            Id = id,
            Name = name,
            LastName = lastName,
            ZipCode = zipCode,
        };

        _mockedReaderService
            .Setup(_ => _.GetByIdAsync(id, TestContext.Current.CancellationToken))
            .ReturnsAsync(mockedReader);

        var sut = new GetReaderById(_mockedReaderService.Object);

        // act
        var actualResult = await sut.DoAsync(id, TestContext.Current.CancellationToken);

        // assert
        Assert.Equivalent(expectedResult, actualResult);

        _mockedReaderService.Verify(_ =>
            _.GetByIdAsync(
                id,
                TestContext.Current.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Should_ThrowEntityNotFoundException()
    {
        // arrange
        var id = 1;
        var expectedResult = new EntityNotFoundException($"The Reader with Id: {id} was not found");

        var sut = new GetReaderById(_mockedReaderService.Object);

        // act
        var actualResult = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await sut.DoAsync(id, TestContext.Current.CancellationToken));

        // assert
        Assert.Equal(expectedResult.Message, actualResult.Message);

        _mockedReaderService.Verify(_ =>
            _.GetByIdAsync(
                id,
                TestContext.Current.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Should_ThrowException_When_CallingReaderService()
    {
        // arrange
        var id = 1;

        var expectedException = new Exception("This is an unhandled exception on the ReaderService");

        _mockedReaderService
            .Setup(_ => _.GetByIdAsync(id, TestContext.Current.CancellationToken))
            .ThrowsAsync(expectedException);

        var sut = new GetReaderById(_mockedReaderService.Object);

        // act
        var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(id, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);

        _mockedReaderService.Verify(_ =>
            _.GetByIdAsync(
                id,
                TestContext.Current.CancellationToken),
            Times.Once);
    }
}