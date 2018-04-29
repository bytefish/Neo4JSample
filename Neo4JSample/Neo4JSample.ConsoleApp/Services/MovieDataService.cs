// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4JSample.Model;
using System.Collections.Generic;

namespace Neo4JSample.ConsoleApp.Services
{
    public interface IMovieDataService
    {
        IList<Genre> Genres { get; }

        IList<Person> Persons { get; }

        IList<Movie> Movies { get; }

        IList<MovieInformation> Metadatas { get; }
    }

    public class MovieDataService : IMovieDataService
    {
        private static Movie Movie0 = new Movie
        {
            Id = "1",
            Title = "Kill Bill"
        };

        private static Movie Movie1 = new Movie
        {
            Id = "2",
            Title = "Running Man"
        };

        private static Person Actor0 = new Person
        {
            Id = "1",
            Name = "Uma Thurman"
        };

        private static Person Actor1 = new Person
        {
            Id = "2",
            Name = "Arnold Schwarzenegger"
        };

        private static Person Director0 = new Person
        {
            Id = "3",
            Name = "Quentin Tarantino"
        };

        private static Person Director1 = new Person
        {
            Id = "3",
            Name = "Sergio Leone"
        };

        private static Genre Genre0 = new Genre
        {
            Name = "Romantic"
        };

        private static Genre Genre1 = new Genre
        {
            Name = "Action"
        };
        
        private static MovieInformation Metadata0 = new MovieInformation
        {
            Cast = new[] { Actor0 },
            Director = Director0,
            Genres = new[] { Genre0, Genre1 },
            Movie = Movie0
        };

        private static MovieInformation Metadata1 = new MovieInformation
        {
            Cast = new[] { Actor1 },
            Director = Director1,
            Genres = new[] { Genre1 },
            Movie = Movie1
        };

        public IList<Genre> Genres
        {
            get
            {
                return new[] { Genre0, Genre1 };
            }
        }

        public IList<Person> Persons
        {
            get
            {
                return new[] { Actor0, Actor1, Director0, Director1 };
            }
        }
        
        public IList<Movie> Movies
        {
            get
            {
                return new[] { Movie0, Movie1 };
            }
        }

        public IList<MovieInformation> Metadatas
        {
            get
            {
                return new[] { Metadata0, Metadata1 };
            }
        }
    }
}
