using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace MediaLibrary
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            logger.Info(scrubbedFile);

            MovieFile movieFile = new MovieFile(scrubbedFile);

            string choice = "";
            do
            {
                //Choices
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display Movies");
                Console.WriteLine("3) Find Movie");
                Console.WriteLine("Enter to quit");
                //input
                choice = Console.ReadLine();
                logger.Info($"User choice: {choice}");

                if(choice == "1")
                {
                    Movie movie = new Movie();
                    //request and input title
                    Console.WriteLine("Enter movie title: ");
                    movie.title = Console.ReadLine();
                    //verify title
                    if(movieFile.isUniqueTitle(movie.title))
                    {
                        //input genres
                        string input;
                        do
                        {
                            //request and input genre
                            Console.WriteLine("Enter genre (or 'done' to quit)");
                            input = Console.ReadLine();
                            if(input != "done" && input.Length > 0)
                            {
                                movie.genres.Add(input);
                            }
                        } while(input != "done");
                        if(movie.genres.Count == 0)
                        {
                            movie.genres.Add("(no genres listed)");
                        }
                        //request and input director
                        Console.WriteLine("Enter director: ");
                        movie.director = Console.ReadLine();
                        //request and input running time
                        Console.WriteLine("Enter running time (hh:mm:ss)");
                        movie.runningTime = TimeSpan.Parse(Console.ReadLine());
                        //add movie
                        movieFile.AddMovie(movie);
                    }
                }
                else if(choice == "2")
                {
                    foreach(Movie m in movieFile.Movies)
                    {
                        Console.WriteLine(m.Display());
                    }
                }
                else if(choice == "3")
                {
                    //request search of movie title
                    Console.WriteLine("Enter name of movie to search");
                    string title = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    var SearchMovies = movieFile.Movies.Where(m => m.title.Contains(title));
                    Console.WriteLine($"{SearchMovies.Count()} movies found with \"{title}\" in them");
                    foreach(Movie m in SearchMovies)
                    {
                        Console.WriteLine($" - {m.title}");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while(choice == "1" || choice == "2" || choice == "3");

            logger.Info("Program ended");
        }
    }
}
