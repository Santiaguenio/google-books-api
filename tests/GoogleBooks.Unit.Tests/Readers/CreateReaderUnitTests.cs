using FluentValidation;
using GoogleBooks.Application.Common.Services;
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
    private readonly Mock<IDateTimeProvider> _mockedDateTimeProvider = new();
    private readonly Mock<IReaderService> _mockedReaderService = new();

    [Theory]
    [MemberData(nameof(GetEntryDataAndExpectedResult))]
    public async Task Should_ThrowBadRequestException(
        ReaderCreationDto readerToFailValidation,
        BadRequestException expectedException)
    {
        // arrange
        var sut = new CreateReader(
            testFactory.GetRequiredService<IValidator<ReaderCreationDto>>(),
            _mockedDateTimeProvider.Object,
            _mockedReaderService.Object);

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(readerToFailValidation, TestContext.Current.CancellationToken));
        
        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);

        _mockedDateTimeProvider.Verify(_ => _.UtcNow(), Times.Never);
        _mockedReaderService.Verify(_ => _.AddAsync(It.IsAny<Reader>(), It.IsAny<CancellationToken>()), Times.Never);
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
