using System;
using System.Collections.Generic;

namespace MediaLibrary{
    public abstract class Media{
        public int mediaId { get; set; }
        public string title { get; set; }
        public List<string> genres { get; set; }
        
        public Media(){
            genres = new List<string>();
        }

        public virtual string Display(){
            return $"Id: {mediaId}\nTitle: {title}\nGenres: {string.Join(", ", genres)}\n";
        }
    }
    public class Movie : Media{
        public string director { get; set; }
        public TimeSpan runningTime { get; set; }

        public override string Display()
        {
            return $"Id: {mediaId}\nTitle: {title}\nDirector: {director}\nRun Time: {runningTime}\nGenres: {string.Join(", ", genres)}\n";
        }
    }
    public class Album : Media
    {
        public string artist { get; set; }
        public string recordLabel { get; set; }

        public override string Display()
        {
            return $"Id: {mediaId}\nTitle: {title}\nArtist: {artist}\nLabel: {recordLabel}\nGenres: {string.Join(", ", genres)}\n";
        }
    }
}