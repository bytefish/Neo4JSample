// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4j.Driver.V1;
using Neo4JSample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4JSample.Converters.Provider;
using Neo4JSample.Settings;

namespace Neo4JSample
{
    public class Neo4JClient : IDisposable
    {
        private readonly IDriver driver;
        private readonly IConverterProvider converters;

        public Neo4JClient(IConnectionSettings settings)
            : this(settings, new ConverterProvider()) { }

        public Neo4JClient(IConnectionSettings settings, IConverterProvider converters)
        {
            this.driver = GraphDatabase.Driver(settings.Uri, settings.AuthToken);
            this.converters = converters;
        }

        public async Task CreateIndices()
        {
            string[] queries = new[] 
            {
                "CREATE INDEX ON :Movie(title)",
                "CREATE INDEX ON :Movie(id)",
                "CREATE INDEX ON :Person(id)",
                "CREATE INDEX ON :Person(name)",
                "CREATE INDEX ON :Genre(name)"
            };

            using (var session = driver.Session())
            {
                foreach(var query in queries)
                {
                    await session.RunAsync(query);
                }
            }
        }

        public async Task CreatePersons(IList<Person> persons)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {persons} AS person")
                .AppendLine("MERGE (p:Person {name: person.name})")
                .AppendLine("SET p = person")
                .ToString();

            using (var session = driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "persons", ConvertList(persons) } });
            }
        }

        public async Task CreateGenres(IList<Genre> genres)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {genres} AS genre")
                .AppendLine("MERGE (g:Genre {name: genre.name})")
                .AppendLine("SET g = genre")
                .ToString();

            using (var session = driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "genres", ConvertList(genres) } });
            }
        }

        public async Task CreateMovies(IList<Movie> movies)
        {
            string cypher = new StringBuilder()
                .AppendLine("WITH {json} AS data")
                .AppendLine("UNWIND data AS movie")
                .AppendLine("MERGE (m:Movie {id: movie.id})")
                .AppendLine("SET m = movie")
                .ToString();

            using (var session = driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "json", ConvertList(movies) } });
            }
        }

        public async Task CreateRelationships(IList<Metadata> metadatas)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {metadatas} AS metadata")
                // Find the Movie:
                 .AppendLine("MATCH (m:Movie { title: metadata.movie.title })")
                 // Create Cast Relationships:
                 .AppendLine("UNWIND metadata.cast AS actor")   
                 .AppendLine("MATCH (a:Person { name: actor.name })")
                 .AppendLine("MERGE (a)-[r:ACTED_IN]->(m)")
                  // Create Director Relationship:
                 .AppendLine("WITH metadata, m")
                 .AppendLine("MATCH (d:Person { name: metadata.director.name })")
                 .AppendLine("MERGE (d)-[r:DIRECTED]->(m)")
                // Add Genres:
                .AppendLine("WITH metadata, m")
                .AppendLine("UNWIND metadata.genres AS genre")
                .AppendLine("MATCH (g:Genre { name: genre.name})")
                .AppendLine("MERGE (m)-[r:GENRE]->(g)")
                .ToString();


            using (var session = driver.Session())
            {
                var res = await session.RunAsync(cypher, new Dictionary<string, object>() { { "metadatas", ConvertList(metadatas) } });
            }
        }

        private IList<Dictionary<string, object>> ConvertList<TSourceType>(IList<TSourceType> source)
        {
            var converter = converters.Resolve<TSourceType, Dictionary<string, object>>();

            return source
                .Select(x => converter.Convert(x))
                .ToList();
        }


        private Dictionary<string, object> Convert<TSourceType>(TSourceType source)
        {
            var converter = converters.Resolve<TSourceType, Dictionary<string, object>>();

            return converter.Convert(source);
        }
        
        public void Dispose()
        {
            driver?.Dispose();
        }
    }
}
