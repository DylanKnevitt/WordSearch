using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Wordsearch.Repository.Interfaces;
using Xunit;

namespace Wordsearch.Repository.Tests
{
    /// <summary>
    /// A base class that will run tests against any implementation specified, to ensure that tests that are core to the interface working correctly
    /// are run against all implementations.
    /// </summary>
    public abstract class BaseSearchRepositoryTest
    {
        [Fact]
        public void SearchRepositoryTest_Construct_ShouldNotError()
        {
            var searchRepository = CreateSearchRepository();

            Assert.NotNull(searchRepository);
            Assert.IsAssignableFrom<ISearchRepository>(searchRepository);
        }

        public static IEnumerable<object[]> Data()
        {
           var testcase = new List<List<string>>
            {
                new List<string> {"", "a", "bb", "ccc"},
                new List<string> {"123", "444", "222", "000"},
                new List<string> {"-=-=-=", "<><", "/|||", "[][][]"},
                new List<string> {"~`", "!@#$%^&**()", "*.,", "¿"}
            };
            yield return new object[] {testcase};
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void SearchRepositoryTest_GetWords_Returns_IEnumerableString(List<List<string>> testsList)
        {
            var repository = CreateSearchRepository();
            var result = repository.GetWords();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<string>>(result);
        }

        protected abstract ISearchRepository CreateSearchRepository();
    }

    /// <summary>
    /// Text file implementation of the search repository
    /// </summary>
    public class TextFileSearchRepositoryTest : BaseSearchRepositoryTest
    {
        protected override ISearchRepository CreateSearchRepository()
        {
            var streamReader = new Mock<IStreamReaderFactory>();
            streamReader.Setup(x => x.GetReader()).Returns(new StreamReader(new MemoryStream()));
            var repository = new TextFileSearchRepository(streamReader.Object);
            return repository;
        }

        protected ISearchRepository CreateInvalidSearchRepository()
        {
            var repository = new TextFileSearchRepository(null);
            return repository;
        }

        [Fact]
        public void TestTextFileSearchRepository_Construct_WithoutStreamReader_ThrowsError()
        {
            Assert.Throws<NullReferenceException>(() => CreateInvalidSearchRepository());
        }

    }
}
