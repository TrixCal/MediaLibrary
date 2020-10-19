using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog.Web;

namespace MediaLibrary
{
    public class MovieFile
    {
        //property
        public string filePath { get; set; }
        public List<Movie> Movies { get; set; }
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

        //constructor
        public MovieFile(string movieFilePath)
        {
            filePath = movieFilePath;
            Movies = new List<Movie>();

            try
            {
                StreamReader sr = new StreamReader(filePath);
                while(!sr.EndOfStream)
                {
                    //Create new Movie instance
                    Movie movie = new Movie();
                    string line = sr.ReadLine();
                    //Check if comma is in movie title
                    int idx = line.IndexOf('"');
                    if(idx == -1) //No Quote
                    {
                        string[] movieDetails = line.Split(',');
                        movie.mediaId = UInt64.Parse(movieDetails[0]);
                        movie.title = movieDetails[1];
                        movie.genres = movieDetails[2].Split('|').ToList();
                        movie.director = movieDetails[3];
                        movie.runningTime = TimeSpan.Parse(movieDetails[4]);
                    }
                    else //Quote, comma in movie title
                    {
                        //extract mediaID
                        movie.mediaId = UInt64.Parse(line.Substring(0, idx - 1));
                        //remove mediaID and first quote
                        line = line.Substring(idx + 1);
                        //find next quote
                        idx = line.IndexOf('"');
                        //extract title
                        movie.title = line.Substring(0, idx);
                        //remove title and last comma from string
                        line = line.Substring(idx + 2);
                        //Rest of details split by ','
                        string[] movieDetails = line.Split(',');
                        movie.genres = movieDetails[0].Split('|').ToList();
                        movie.director = movieDetails[1];
                        movie.runningTime = TimeSpan.Parse(movieDetails[2]);
                    }
                    Movies.Add(movie);
                }
                sr.Close();
                logger.Info($"Movies in file {Movies.Count}");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public bool isUniqueTitle(string title)
        {
            if(Movies.ConvertAll(m => m.title.ToLower()).Contains(title.ToLower()))
            {
                logger.Info($"Duplicate movie title {title}");
                return false;
            }
            return true;
        }

        public void AddMovie(Movie movie)
        {
            try
            {
                movie.mediaId = Movies.Max(m => m.mediaId) + 1;
                //Adds new movie to file
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{movie.mediaId},{movie.title},{string.Join("|", movie.genres)},{movie.director},{movie.runningTime}");
                sw.Close();
                //Adds movie to list
                Movies.Add(movie);
                
                logger.Info($"Movie id {movie.mediaId} added");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}