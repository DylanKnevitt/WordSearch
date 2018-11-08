using System;
using System.Collections.Generic;
using System.Linq;
using Wordsearch.Repository.Interfaces;
using Wordsearch.Service.Interfaces;

namespace Wordsearch.Service
{
    public class SearchService : ISearchService
    {
        //Declaring the alphabet as a constant for easy random string generation
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly ISearchRepository _searchRepository;

        /// <summary>
        /// A service for all logical operations for the word search
        /// </summary>
        /// <param name="searchRepository"></param>
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the list of possible words from underlying repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetWords()
        {
            var result = _searchRepository.GetWords();
            return result;
        }

        /// <summary>
        /// A method to get a grid of characters for the word search
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public char[,] GetCharGrid(int height, int width)
        {
            var random = new Random();
            var result = new char[width, height];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var letter = Alphabet[random.Next(0, 26)];
                    result[i, j] = letter;
                }
            }

            return result;
        }

        /// <summary>
        /// Searches the character 2d array for words
        /// </summary>
        /// <param name="charGrid"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public Dictionary<string,List<Tuple<int,int>>> SearchGridForWords(char[,] charGrid, IEnumerable<string> words)
        {
            var dict = new Dictionary<string, List<Tuple<int,int>>>();
            var width = charGrid.GetLength(0);
            var height = charGrid.GetLength(1);
            var covered = new bool[height, width];
            var wordsList = words.ToList();

            for (var i = 0; i < width; i++)
            {
                for (var a = 0; a < height; a++)
                {
                    //Kicks off recursive search function
                    SearchGrid(charGrid, i, a, width, height, "", covered,wordsList,dict, new List<Tuple<int, int>>());
                }
            }

            //Returns word as well as a list of tuples with the position from the 
            return dict;
        }


        private void SearchGrid(char[,] charGrid,int i,int j,int width,int height,string build,
      bool[,] covered,List<string> words, IDictionary<string,List<Tuple<int, int>>> found,ICollection<Tuple<int, int>> gridPosition)
        {

            //Return if array bounds are exceeded
            if (i >= width || i < 0 || j >= height || j < 0)
            {
                return;
            }
            
            //If the point has already been searched, dont search again
            if (covered[j, i])
            {
                return;
            }
            // Get current letter from array
            var pass = build + charGrid[j, i];

            if (words.Contains(pass))
            {
                if (!found.ContainsKey(pass))
                {
                    var foundGrid = new List<Tuple<int, int>>(gridPosition) {new Tuple<int, int>(j, i)};
                    found.Add(pass, foundGrid);
                }
            }

            //If the word doesnt match the first couple of characters in the dictionary, pass it
            var wordsWithLength = words.Where(x => x.Length >= pass.Length).Select(x => x.Substring(0, pass.Length));
            if (!wordsWithLength.Contains(pass))
            {
                return;
            }
            else
            {
                gridPosition.Add(new Tuple<int, int>(j,i));
            }

            // Wanted to use array.clone but it wasnt working nicely with a 2d array.
            var cov = new bool[height, width];
            for (var i2 = 0; i2 < width; i2++)
            {
                for (var j2 = 0; j2 < height; j2++)
                {
                    cov[j2, i2] = covered[j2, i2];
                }
            }

            cov[j, i] = true;

            // Omni directional search
            SearchGrid(charGrid, i + 1, j, width, height, pass, cov,words,found,new List<Tuple<int,int>>(gridPosition));
            SearchGrid(charGrid, i, j + 1, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));
            SearchGrid(charGrid, i + 1, j + 1, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));
            SearchGrid(charGrid, i - 1, j, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));
            SearchGrid(charGrid, i, j - 1, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));
            SearchGrid(charGrid, i - 1, j - 1, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));
            SearchGrid(charGrid, i - 1, j + 1, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));
            SearchGrid(charGrid, i + 1, j - 1, width, height, pass, cov,words,found, new List<Tuple<int, int>>(gridPosition));

        }
    }
}
