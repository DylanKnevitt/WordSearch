using System;
using System.Collections.Generic;
using Wordsearch.Repository.Interfaces;

namespace Wordsearch.Repository
{
    /// <summary>
    /// A Concrete implementation of the ISearchRepository, this implements a file based data store
    /// </summary>
    public class TextFileSearchRepository : ISearchRepository
    {
        private readonly IStreamReaderFactory _streamReader;
        public TextFileSearchRepository(IStreamReaderFactory streamReader)
        {
            _streamReader = streamReader ?? throw new NullReferenceException("Stream reader factory cannot be null");
        }

        /// <summary>
        /// Gets the words from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetWords()
        {
            var result = new List<string>();
            using (var reader = _streamReader.GetReader())
            {
                while (reader.Peek() >= 0)
                {
                    result.Add(reader.ReadLine());
                }
                return result;

            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
