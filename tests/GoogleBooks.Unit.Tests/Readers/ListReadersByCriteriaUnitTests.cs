using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.Models;
using GoogleBooks.Application.Common.Validators;
using GoogleBooks.Application.Readers;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Application.Readers.UseCases;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Contracts.Readers.Requests;
using GoogleBooks.Contracts.Readers.Responses;
using GoogleBooks.Domain.Exceptions;
using Moq;

namespace GoogleBooks.Unit.Tests.Readers
{
    [Collection("Unit tests collection")]
    public class ListReadersByCriteriaUnitTests(TestFactory testFactory)
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

            var mockedReaders = new EntitiesByCriteria<ReaderFull>
            {
                Items = [new ReaderFull {
                    Address = address,
                    Birthdate = birthDate,
                    City = city,
                    Email = email,
                    Id = id,
                    Name = name,
                    LastName = lastName,
                    ZipCode = zipCode
                }],
                TotalItems = 1
            };

            var expectedResult = new EntitiesByCriteriaDto<ReaderFullDto>
            {
                Items = [new ReaderFullDto {
                    Address = address,
                    Birthdate = birthDate,
                    City = city,
                    Email = email,
                    Id = id,
                    Name = name,
                    LastName = lastName,
                    ZipCode = zipCode
                }],
                TotalItems = 1
            };

            var page = 1;
            var pageSize = 10;
            var readerServiceRequest = new ReadersSearchCriteria(page, pageSize, null, null, null, null, null, birthDate);

            _mockedReaderService
                .Setup(_ => _.ListByCriteriaAsync(
                    It.Is<ReadersSearchCriteria>(_ =>
                        _.Page == page
                        && _.PageSize == pageSize
                        && _.BirthDate == birthDate),
                    TestContext.Current.CancellationToken))
                .ReturnsAsync(mockedReaders);

            var request = new ReadersSearchCriteriaDto { Page = page, PageSize = pageSize, BirthDate = birthDate };

            var sut = new ListReadersByCriteria(
                testFactory.GetRequiredService<IGoogleBooksValidator<ReadersSearchCriteriaDto>>(),
                _mockedReaderService.Object,
                testFactory.GetRequiredService<IGoogleBooksMapper>());

            // act
            var actualResult = await sut.DoAsync(request, TestContext.Current.CancellationToken);

            // assert
            Assert.Equivalent(expectedResult, actualResult);

            _mockedReaderService.Verify(_ =>
                _.ListByCriteriaAsync(
                    It.Is<ReadersSearchCriteria>(_ =>
                        _.Page == page
                        && _.PageSize == pageSize
                        && _.BirthDate == birthDate),
                    TestContext.Current.CancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException()
        {
            // arrange
            var email = "bad-email-format";

            var expectedException = new BadRequestException(new Dictionary<string, string[]> {
                { "Email", ["Value \"bad-email-format\" is not a valid email address"] }
            });

            var page = 1;
            var pageSize = 10;
            var readerServiceRequest = new ReadersSearchCriteria(page, pageSize, null, null, email, null, null, null);

            var request = new ReadersSearchCriteriaDto { Page = page, PageSize = pageSize, Email = email };

            var sut = new ListReadersByCriteria(
                testFactory.GetRequiredService<IGoogleBooksValidator<ReadersSearchCriteriaDto>>(),
                _mockedReaderService.Object,
                testFactory.GetRequiredService<IGoogleBooksMapper>());

            // act
            var actualException = await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DoAsync(request, TestContext.Current.CancellationToken));

            // assert
            Assert.Equal(expectedException.Message, actualException.Message);

            _mockedReaderService.Verify(_ =>
                _.ListByCriteriaAsync(
                    It.IsAny<ReadersSearchCriteria>(),
                    TestContext.Current.CancellationToken),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowException_When_CallingReaderService()
        {
            // arrange
            var birthDate = new DateOnly(1985, 10, 10);

            var expectedException = new Exception("This is an unhandled exception on the ReaderService");

            var page = 1;
            var pageSize = 10;
            var readerServiceRequest = new ReadersSearchCriteria(page, pageSize, null, null, null, null, null, birthDate);

            _mockedReaderService
                .Setup(_ => _.ListByCriteriaAsync(
                    It.Is<ReadersSearchCriteria>(_ =>
                        _.Page == page
                        && _.PageSize == pageSize
                        && _.BirthDate == birthDate),
                    TestContext.Current.CancellationToken))
                .ThrowsAsync(expectedException);

            var request = new ReadersSearchCriteriaDto { Page = page, PageSize = pageSize, BirthDate = birthDate };

            var sut = new ListReadersByCriteria(
               testFactory.GetRequiredService<IGoogleBooksValidator<ReadersSearchCriteriaDto>>(),
               _mockedReaderService.Object,
               testFactory.GetRequiredService<IGoogleBooksMapper>());

            // act
            var actualException = await Assert.ThrowsAsync<Exception>(async () => await sut.DoAsync(request, TestContext.Current.CancellationToken));

            // assert
            Assert.Equivalent(expectedException, actualException);

            _mockedReaderService.Verify(_ =>
                _.ListByCriteriaAsync(
                    It.Is<ReadersSearchCriteria>(_ =>
                        _.Page == page
                        && _.PageSize == pageSize
                        && _.BirthDate == birthDate),
                    TestContext.Current.CancellationToken),
                Times.Once);
        }
    }
}
