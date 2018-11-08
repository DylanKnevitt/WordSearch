using System;
using System.Collections.Generic;

namespace Wordsearch.Repository.Interfaces
{
    /// <summary>
    /// An interface for abstracting out data access
    /// </summary>
    public interface ISearchRepository : IDisposable
    {
        IEnumerable<string> GetWords();
    }
}
