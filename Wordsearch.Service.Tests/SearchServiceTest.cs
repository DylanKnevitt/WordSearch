using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Moq;
using Wordsearch.Repository.Interfaces;
using Wordsearch.Service.Interfaces;
using Xunit;

namespace Wordsearch.Service.Tests
{
    /// <summary>
    /// A base class that will run tests against any implementation specified, to ensure that tests that are core to the interface working correctly
    /// are run against all implementations.
    /// </summary>
    public abstract class BaseSearchServiceTest
    {
        [Fact]
        public void SearchServiceTest_Construct_ShouldNotError()
        {
            var searchRepository = new Mock<ISearchRepository>();
            var searchService = CreateSearchService(searchRepository.Object);

            Assert.NotNull(searchService);
            Assert.IsAssignableFrom<ISearchService>(searchService);
        }


        //Testing basic input and output, testing the actual logic of recursive function would take quite a while
        [Fact]
        public void SearchServiceTest_SearchGridForWords_ShouldReturn_Keys()
        {
           var  data = new char[,]{
                {'d','u','k','e','a'},
                {'a','c','p','x','x'},
                {'y','x','d','x','o'},
                {'x','x','x','e','o'},
                {'x','x','p','a','z'}
            };

            var expectedResult = new List<string> {
                "duke","duck","dup","dupe","day","auk","ya","yack","yaup","yaud","uke","up","cay","cad","cuke",
                "cup","cud","kea","kex","kep","puke","puck","pud","pea","dex","pa","pax","ped","azo","ape","apex",
                "aped","axe","axed","ooze","oozed","ox","oxo","zap","zax","zoea","zoo","zoa","zed"
            };
            var searchRepository = new Mock<ISearchRepository>();
            var searchService = CreateSearchService(searchRepository.Object);
            Assembly asm = this.GetType().Assembly;
            using (var stream = asm.GetManifestResourceStream("Wordsearch.Service.Tests.words.txt"))
            {
                var reader = new StreamReader(stream);
                var list = new List<string>();
                while (reader.Peek() >= 0)
                {
                    list.Add(reader.ReadLine());
                }
                var actual = searchService.SearchGridForWords(data,list);

                Assert.Equal(expectedResult.Count,actual.Keys.Count);

                foreach (var word in expectedResult)
                {
                    Assert.Contains(word,actual.Keys);
                }
            }

            

        }

        protected abstract ISearchService CreateSearchService(ISearchRepository searchRepository);
    }

    /// <summary>
    /// Standard implementation of the search service
    /// </summary>
    public class SearchServiceTest : BaseSearchServiceTest
    { 
        protected override ISearchService CreateSearchService(ISearchRepository searchRepository)
        {
            var service = new SearchService(searchRepository);
            return service;
        }
    }
}
