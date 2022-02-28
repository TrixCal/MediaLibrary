using System;
using System.Collections.Generic;

namespace MediaLibrary{
    public class Media{
        public int mediaId { get; set; }
        public string title { get; set; }
        public List<string> genres { get; set; }
        
        public Media(){
            genres = new List<string>();
        }

        public string Display(){
            return $"Id: {mediaId}\nTitle: {title}\nGenres: {string.Join(", ", genres)}\n";
        }
    }
}