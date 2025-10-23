using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Domain.Exceptions;
using Moq;

namespace GoogleBooks.Unit.Tests.Books;

public class ListBooksByKeyWordsUnitTests
{
    private readonly Mock<IBookService> _mockedBookService = new();

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
