using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NLog.Web;

namespace MediaLibrary
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program Started");

            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            logger.Info(scrubbedFile);
            MovieFile movieFile = new MovieFile(scrubbedFile);

            string choice;
            do{
                //menu
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("3) Search for Movie");
                Console.WriteLine("Enter to quit");
                choice = Console.ReadLine();
                if(choice == "1"){
                    try{
                        Movie movie = new Movie();
                        List<string> movieGenres = new List<string>();
                        movie.mediaId = 0;
                        //title prompt
                        Console.Write("Enter movie title: ");
                        movie.title = Console.ReadLine();
                        if(movieFile.isUniqueTitle(movie.title)){
                            //genres prompt
                            Console.Write("Enter genre (type \"done\" to quit): ");
                            string input = Console.ReadLine();
                            while(input != "done"){
                                movieGenres.Add(input);
                                input = Console.ReadLine();
                            }
                            if(movieGenres.Count > 0){
                                movie.genres = movieGenres;
                            }else{
                                movie.genres.Add("unassigned");
                            }
                            //director prompt
                            Console.Write("Enter movie director: ");
                            movie.director = Console.ReadLine();
                            //runtime prompt
                            Console.Write("Enter running time (h:m:s): ");
                            movie.runningTime = TimeSpan.Parse(Console.ReadLine());
                            //Add movie to file
                            movieFile.AddMovie(movie);
                        }
                    }
                    catch(Exception ex){
                        logger.Error(ex.Message);
                    }
                }
                else if(choice == "2"){
                    //print out all movies
                    movieFile.DisplayMovies();
                }
                else if(choice == "3"){
                    //Search for movie
                    Console.Write("Movie title: ");
                    string title = Console.ReadLine();

                    var movies = movieFile.Movies.Where(m => m.title.Contains(title));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"There were {movies.Count()} movies by the title \"{title}\".\n");
                    foreach(Movie m in movies){
                        Console.WriteLine(m.Display());
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }while(choice == "1" | choice == "2" | choice == "3");

            logger.Info("Program Ended");
        }
    }
}
