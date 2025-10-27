using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.Models;
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
        var keyWords = "these are key words";
        var page = 1;
        var pageSize = 10;

        var pageParams = new PageParamsDto { KeyWords = keyWords, Page = page, PageSize = pageSize };
        var listByKeyWordsParams = new ListByKeyWordsParams(1, 10, keyWords);

        var expectedException = new NullReferenceException("Object reference not set to an instance of an object.");

        var sut = new ListBooksByKeyWords(null!);

        // act
        var actualException = await Assert.ThrowsAsync<NullReferenceException>(async () => await sut.DoAsync(pageParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);
        _mockedBookService.Verify(_ => _.ListByKeyWordsAsync(
                It.Is<ListByKeyWordsParams>(_ =>
                    _.KeyWords == keyWords
                    && _.Page == page
                    && _.PageSize == pageSize)
                , TestContext.Current.CancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_CallingBookService()
    {
        // arrange

        var keyWords = "these are key words";
        var page = 1;
        var pageSize = 10;

        var pageParams = new PageParamsDto { KeyWords = keyWords, Page = page, PageSize = pageSize };
        var listByKeyWordsParams = new ListByKeyWordsParams(1, 10, keyWords);

        var expectedException = new Exception("This is an unhandled exception on the BookService");

        _mockedBookService
            .Setup(_ => _.ListByKeyWordsAsync(
                It.Is<ListByKeyWordsParams>(_ =>
                    _.KeyWords == keyWords
                    && _.Page == page
                    && _.PageSize == pageSize),
                TestContext.Current.CancellationToken))
            .ThrowsAsync(expectedException);

        var sut = new ListBooksByKeyWords(_mockedBookService.Object);

        // act
        var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(pageParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);
        _mockedBookService.Verify(_ => _.ListByKeyWordsAsync(
                It.Is<ListByKeyWordsParams>(_ =>
                    _.KeyWords == keyWords
                    && _.Page == page
                    && _.PageSize == pageSize)
                , TestContext.Current.CancellationToken),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Should_ThrowBadRequestException_When_KeyWordsFieldIsNullOrEmpty(string? keyWords)
    {
        // arrange
        var expectedException = new BadRequestException(new Dictionary<string, string[]> { { nameof(PageParamsDto.KeyWords), [$"{nameof(PageParamsDto.KeyWords)} is mandatory"] } });

        var sut = new ListBooksByKeyWords(_mockedBookService.Object);
        var requestParams = new PageParamsDto
        {
            KeyWords = keyWords!,
            Page = 1,
            PageSize = 10
        };

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(requestParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);
        _mockedBookService.Verify(_ => _.ListByKeyWordsAsync(It.IsAny<ListByKeyWordsParams>(), TestContext.Current.CancellationToken), Times.Never);
    }
}
