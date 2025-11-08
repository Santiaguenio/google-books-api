using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.Models;
using GoogleBooks.Contracts.Books.Requests;
using GoogleBooks.Contracts.Books.Responses;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Domain.Exceptions;
using Moq;

namespace GoogleBooks.Unit.Tests.Books;

[Collection("Unit tests collection")]
public class ListBooksByKeyWordsUnitTests(TestFactory testFactory)
{
    private readonly Mock<IBookService> _mockedBookService = new();

    [Fact]
    public async Task Should()
    {
        // arrange
        var mockedBooks = new EntitiesByCriteria<BookFull>
        {
            Items = [new BookFull {
                Id = "bookId",
                SaleInfo = new SaleInfo { BuyLink = "buyLink", Country = "AR", IsEbook = true, Saleability = "saleability"},
                SelfLink = "selfLink",
                VolumeInfo = new VolumeInfo { Authors = ["Jhon Doe"], Language = "ES" }
            }],
            TotalItems = 1
        };

        var expectedResult = new EntitiesByCriteriaDto<BookFullDto>
        {
            Items = [new BookFullDto {
                Id = "bookId",
                SaleInfo = new SaleInfoDto { BuyLink = "buyLink", Country = "AR", IsEbook = true, Saleability = "saleability"},
                SelfLink = "selfLink",
                VolumeInfo = new VolumeInfoDto { Authors = ["Jhon Doe"], Language = "ES" }
            }],
            TotalItems = 1
        };

        var page = 1;
        var pageSize = 10;
        var keyWords = "any keywords";
        var readerServiceRequest = new BooksSearchCriteria(page, pageSize, keyWords);

        _mockedBookService
            .Setup(_ => _.ListByKeyWordsAsync(
                It.Is<BooksSearchCriteria>(_ =>
                    _.Page == page
                    && _.PageSize == pageSize
                    && _.KeyWords == keyWords),
                TestContext.Current.CancellationToken))
            .ReturnsAsync(mockedBooks);

        var request = new BooksSearchCriteriaDto { Page = page, PageSize = pageSize, KeyWords = keyWords };

        var sut = new ListBooksByKeyWords(
            _mockedBookService.Object,
            testFactory.GetRequiredService<IGoogleBooksMapper>());

        // act
        var actualResult = await sut.DoAsync(request, TestContext.Current.CancellationToken);

        // assert
        Assert.Equivalent(expectedResult, actualResult);

        _mockedBookService.Verify(_ =>
            _.ListByKeyWordsAsync(
                It.Is<BooksSearchCriteria>(_ =>
                    _.Page == page
                    && _.PageSize == pageSize
                    && _.KeyWords == keyWords),
                TestContext.Current.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Should_ThrowException_When_CallingBookService()
    {
        // arrange
        var keyWords = "these are key words";
        var page = 1;
        var pageSize = 10;

        var pageParams = new BooksSearchCriteriaDto { KeyWords = keyWords, Page = page, PageSize = pageSize };
        var listByKeyWordsParams = new BooksSearchCriteria(1, 10, keyWords);

        var expectedException = new Exception("This is an unhandled exception on the BookService");

        _mockedBookService
            .Setup(_ => _.ListByKeyWordsAsync(
                It.Is<BooksSearchCriteria>(_ =>
                    _.KeyWords == keyWords
                    && _.Page == page
                    && _.PageSize == pageSize),
                TestContext.Current.CancellationToken))
            .ThrowsAsync(expectedException);

        var sut = new ListBooksByKeyWords(_mockedBookService.Object, testFactory.GetRequiredService<IGoogleBooksMapper>());

        // act
        var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(pageParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException, actualException);

        _mockedBookService.Verify(_ =>
            _.ListByKeyWordsAsync(
                It.Is<BooksSearchCriteria>(_ =>
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
        var expectedException = new BadRequestException(new Dictionary<string, string[]> { { nameof(BooksSearchCriteriaDto.KeyWords), [$"{nameof(BooksSearchCriteriaDto.KeyWords)} is mandatory"] } });

        var sut = new ListBooksByKeyWords(_mockedBookService.Object, testFactory.GetRequiredService<IGoogleBooksMapper>());
        var requestParams = new BooksSearchCriteriaDto
        {
            KeyWords = keyWords!,
            Page = 1,
            PageSize = 10
        };

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(requestParams, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);

        _mockedBookService.Verify(_ =>
            _.ListByKeyWordsAsync(
                It.IsAny<BooksSearchCriteria>(),
                TestContext.Current.CancellationToken),
            Times.Never);
    }
}
