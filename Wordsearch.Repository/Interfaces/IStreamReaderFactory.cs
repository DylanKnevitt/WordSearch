using System.IO;

namespace Wordsearch.Repository.Interfaces
{
    /// <summary>
    /// An interface for abstracting out a concrete stream reader
    /// </summary>
    public interface IStreamReaderFactory
    {
        string Path { get; set; }
        StreamReader GetReader();
    }
}
