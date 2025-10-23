using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Domain.Exceptions;
using Moq;

namespace GoogleBooks.Unit.Tests.Books;

[Collection("Unit tests collection")]
public class GetBookByIdUnitTests()
{
    private readonly Mock<IBookService> _mockedBookService = new();

    [Fact]
    public async Task Should_ThrowBadRequestException_When_IdIsNullOrEmpty()
    {
        // arrange
        var expectedException = new BadRequestException(new Dictionary<string, string[]> { { "Id", ["Id is mandatory"] } });

        var sut = new GetBookById(_mockedBookService.Object);

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(string.Empty, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);
        _mockedBookService.Verify(_ => _.GetByIdAsync(It.IsAny<string>(), TestContext.Current.CancellationToken), Times.Never);
    }
}
