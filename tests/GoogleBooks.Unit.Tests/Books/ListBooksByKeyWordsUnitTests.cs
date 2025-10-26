using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Domain.Exceptions;
using Moq;

namespace GoogleBooks.Unit.Tests.Books;

public class ListBooksByKeyWordsUnitTests
{
    private readonly Mock<IBookService> _mockedBookService = new();

    [Fact]
    public async Task Should_ThrowException_When_BookServiceIsNotCorrectlyInjected()
    {
        // arrange
        var expectedException = new NullReferenceException("Object reference not set to an instance of an object.");

        var requestParams = new PageParams
        {
            KeyWords = "these are key words",
            Page = 1,
            PageSize = 10
        };

        var sut = new ListBooksByKeyWords(null!);

        // act
        var actualException = await Assert.ThrowsAsync<NullReferenceException>(async () => await sut.DoAsync(requestParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);
        _mockedBookService.Verify(_ => _.ListByKeyWordsAsync(requestParams, TestContext.Current.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_CallingBookService()
    {
        // arrange
        var expectedException = new Exception("This is an unhandled exception on the BookService");

        var requestParams = new PageParams
        {
            KeyWords = "these are key words",
            Page = 1,
            PageSize = 10
        };

        _mockedBookService
            .Setup(_ => _.ListByKeyWordsAsync(requestParams, TestContext.Current.CancellationToken))
            .ThrowsAsync(new Exception("This is an unhandled exception on the BookService"));

        var sut = new ListBooksByKeyWords(_mockedBookService.Object);

        // act
        var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(requestParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);
        _mockedBookService.Verify(_ => _.ListByKeyWordsAsync(requestParams, TestContext.Current.CancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Should_ThrowBadRequestException_When_KeyWordsFieldIsNullOrEmpty(string? keyWords)
    {
        // arrange
        var expectedException = new BadRequestException(new Dictionary<string, string[]> { { nameof(PageParams.KeyWords), [$"{nameof(PageParams.KeyWords)} is mandatory"] } });

        var sut = new ListBooksByKeyWords(_mockedBookService.Object);
        var requestParams = new PageParams
        {
            KeyWords = keyWords!,
            Page = 1,
            PageSize = 10
        };

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(requestParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);
        _mockedBookService.Verify(_ => _.ListByKeyWordsAsync(It.IsAny<PageParams>(), TestContext.Current.CancellationToken), Times.Never);
    }
}
