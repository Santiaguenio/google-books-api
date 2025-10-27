using AutoMapper;
using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Domain.Exceptions;
using Moq;

namespace GoogleBooks.Unit.Tests.Books;

public class GetBookByIdUnitTests()
{
    private readonly Mock<IBookService> _mockedBookService = new();
    private readonly Mock<IMapper> _mockedMapper = new();

    [Fact]
    public async Task Should_ThrowException_When_BookServiceIsNotCorrectlyInjected()
    {
        // arrange
        var expectedException = new NullReferenceException("Object reference not set to an instance of an object.");

        var sut = new GetBookById(null!, null!);

        // act
        var actualException = await Assert.ThrowsAsync<NullReferenceException>(async () => await sut.DoAsync("this is an id", TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);
        _mockedBookService.Verify(_ => _.GetByIdAsync(It.IsAny<string>(), TestContext.Current.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_CallingBookService()
    {
        // arrange
        var id = "this is an id";
        var expectedException = new Exception("This is an unhandled exception on the BookService");

        _mockedBookService
            .Setup(_ => _.GetByIdAsync(id, TestContext.Current.CancellationToken))
            .ThrowsAsync(new Exception("This is an unhandled exception on the BookService"));

        var sut = new GetBookById(_mockedBookService.Object, _mockedMapper.Object);

        // act
        var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(id, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Message, actualException.Message);
        _mockedBookService.Verify(_ => _.GetByIdAsync(id, TestContext.Current.CancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Should_ThrowBadRequestException_When_IdIsNullOrEmpty(string? id)
    {
        // arrange
        var expectedException = new BadRequestException(new Dictionary<string, string[]> { { "Id", ["Id is mandatory"] } });

        var sut = new GetBookById(_mockedBookService.Object, _mockedMapper.Object);

        // act
        var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(id!, TestContext.Current.CancellationToken));

        // assert
        Assert.Equivalent(expectedException.Errors, actualException.Errors);
        _mockedBookService.Verify(_ => _.GetByIdAsync(It.IsAny<string>(), TestContext.Current.CancellationToken), Times.Never);
    }
}