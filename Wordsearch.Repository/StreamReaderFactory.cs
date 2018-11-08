using System.IO;
using Wordsearch.Repository.Interfaces;

namespace Wordsearch.Repository
{
    /// <summary>
    /// Concrete implementation of IStreamReaderFactory, used to abstract away stream reader for testability
    /// </summary>
    public class StreamReaderFactory : IStreamReaderFactory
    {

        public StreamReaderFactory(string filePath)
        {
            Path = filePath;
        }

        public string Path { get; set; }
        public StreamReader GetReader()
        {
            var streamReader = new StreamReader(Path);
            return streamReader;
        }
    }
}
