using dcavaletto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MvcMovie.Models
{
    public class MovieInitializer : DropCreateDatabaseIfModelChanges<MovieDBContext>
    {
        protected override void Seed(MovieDBContext context)
        {
            var movies = new List<Movie> {  
  
                 new Movie { Title = "When Harry Met Sally",   
                             Date=DateTime.Parse("1989-1-11"),   
                             Genre="Romantic Comedy",  
                             Rating="R",  
                             Director="Steve-o"},  

                     new Movie { Title = "Ghostbusters ",   
                             Date=DateTime.Parse("1984-3-13"),   
                             Genre="Comedy",  
                              Rating="R",  
                             Director="That Guy"},   
  
                 new Movie { Title = "Ghostbusters 2",   
                             Date=DateTime.Parse("1986-2-23"),   
                             Genre="Comedy",  
                             Rating="R",  
                             Director="That Guy"},   

               new Movie { Title = "Rio Bravo",   
                             Date=DateTime.Parse("1959-4-15"),   
                             Genre="Western",  
                             Rating="R",  
                             Director="John Wayne"},   
             };

            movies.ForEach(d => context.Movies.Add(d));
        }
    }
}