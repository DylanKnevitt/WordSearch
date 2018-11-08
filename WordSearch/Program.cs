using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Wordsearch.Repository;
using Wordsearch.Repository.Interfaces;
using Wordsearch.Service;
using Wordsearch.Service.Interfaces;

namespace WordSearch.Console
{
    internal static class Program
    {
        //Simple Injector container for IOC as a form of DI
        static readonly Container Container;
        private static readonly string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}words.txt";


        //Setting up IOC container
        static Program()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            Container.Register<IStreamReaderFactory>(() => new StreamReaderFactory(FilePath),Lifestyle.Scoped);

            Container.Register<ISearchRepository, TextFileSearchRepository>(Lifestyle.Scoped);
            Container.Register<ISearchService,SearchService>(Lifestyle.Scoped);

            Container.Verify();
        }

        /// <summary>
        /// Removed args as it is unused
        /// </summary>
        static void Main()
        {
            //Using a scoped lifestyle as this thread will only run once
            using (ThreadScopedLifestyle.BeginScope(Container))
            {
                // Using Service locator pattern since this is a console app
                var searchService = Container.GetInstance<ISearchService>();

                //Get list of words
                var words = searchService.GetWords();

                //Get the height and width of the grid
                var height = PromptForValue("height");
                var width = PromptForValue("width");

                //Get the randomly generated grid
                var charGrid = searchService.GetCharGrid(height, width);

                //Using the character grid and list of words retrieved, perform word search and return words found as well as their location
                 var result = searchService.SearchGridForWords(charGrid,words);

                //For every word found, print out the grid, and where it was found on the grid
                foreach (var keyValuePair in result)
                {
                    System.Console.WriteLine(keyValuePair.Key);
                    DisplayCharGrid(charGrid,keyValuePair.Value);
                }
                

                System.Console.ReadLine();
            }
        }

        /// <summary>
        /// Logic to display the results
        /// </summary>
        /// <param name="charGrid"></param>
        /// <param name="gridPosition"></param>
        private static void DisplayCharGrid(char[,] charGrid,List<Tuple<int,int>> gridPosition)
        {
            for (var i = 0; i < charGrid.GetLength(0); i++)
            {
                
                for (var j = 0; j < charGrid.GetLength(1); j++)
                {
                    
                    if (gridPosition.Any(x=> x.Item1 == i && x.Item2 == j))
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.Write($"{charGrid[i, j]}  ".ToUpper());
                    }
                    else
                    {
                        System.Console.BackgroundColor = ConsoleColor.DarkBlue;
                        System.Console.ForegroundColor = ConsoleColor.White;
                        System.Console.Write($"{charGrid[i, j]}  ");
                    }
                }
                System.Console.BackgroundColor = ConsoleColor.Black;
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
        }

        //Reusable prompt
        public static int PromptForValue(string name)
        {
            var result = -1;

            while (result <= 0)
            {
                System.Console.WriteLine($"Please specify a {name}:");

                var heightFromConsole = System.Console.ReadLine();
                if (!Int32.TryParse(heightFromConsole, out result))
                {
                    System.Console.WriteLine($"{heightFromConsole} is an invalid input. Please try again.");
                }
            }

            return result;
        }



    }

}


