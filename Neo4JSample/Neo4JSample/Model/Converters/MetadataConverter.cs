// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Neo4JSample.Converters.Parameters;

namespace Neo4JSample.Model.Converters
{
    public class MetadataConverter : BaseDictionaryConverter<Metadata>
    {
        private readonly BaseDictionaryConverter<Movie> movieConverter = new MovieConverter();
        private readonly BaseDictionaryConverter<Person> personConverter = new PersonConverter();
        private readonly BaseDictionaryConverter<Genre> genreConverter = new GenreConverter();

        protected override void InternalConvert(Metadata source, Dictionary<string, object> target)
        {
            target["movie"] = ConvertMovie(source.Movie);
            target["cast"] = ConvertCast(source.Cast);
            target["director"] = ConvertDirector(source.Director);
            target["genres"] = ConvertGenres(source.Genres);
        }

        private Dictionary<string, object> ConvertMovie(Movie movie)
        {
            return movieConverter.Convert(movie);
        }

        private Dictionary<string, object> ConvertDirector(Person director)
        {
            return personConverter.Convert(director);
        }

        private IList<Dictionary<string, object>> ConvertCast(IList<Person> persons)
        {
            return persons
                .Select(x => personConverter.Convert(x))
                .ToList();
        }

        private IList<Dictionary<string, object>> ConvertGenres(IList<Genre> genres)
        {
            return genres
                .Select(x => genreConverter.Convert(x))
                .ToList();
        }
    }
}
