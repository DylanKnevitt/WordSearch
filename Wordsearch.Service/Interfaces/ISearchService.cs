using System;
using System.Collections.Generic;

namespace Wordsearch.Service.Interfaces
{
    /// <summary>
    /// An interface for abstracting away logic
    /// </summary>
    public interface ISearchService
    {
        IEnumerable<string> GetWords();
        Dictionary<string,List<Tuple<int, int>>> SearchGridForWords(char[,] charGrid, IEnumerable<string> words);
        char[,] GetCharGrid(int height, int width);
    }
}
