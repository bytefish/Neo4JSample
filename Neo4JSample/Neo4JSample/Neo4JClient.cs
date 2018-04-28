using Neo4j.Driver.V1;
using Neo4JSample.Model;
using Neo4JSample.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JSample
{
    public class Neo4JClient : IDisposable
    {
        private readonly IDriver _driver;

        public Neo4JClient(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
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

            using (var session = _driver.Session())
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

            using (var session = _driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "persons", persons.Select(x => SerializationUtils.AsDictionary(x)) } });
            }
        }

        public async Task CreateGenres(IList<Genre> genres)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {genres} AS genre")
                .AppendLine("MERGE (g:Genre {name: genre.name})")
                .AppendLine("SET g = genre")
                .ToString();

            using (var session = _driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "genres", genres.Select(x => SerializationUtils.AsDictionary(x)) } });
            }
        }

        public async Task CreateMovies(IList<Movie> movies)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {movies} AS movie")
                .AppendLine("MERGE (m:Movie {id: movie.id})")
                .AppendLine("SET m = movie")
                .ToString();

            using (var session = _driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "movies", movies.Select(x => SerializationUtils.AsDictionary(x)) } });
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

            var parameters = metadatas.Select(x => SerializationUtils.AsDictionary(x)).ToArray();

            using (var session = _driver.Session())
            {
                var res = await session.RunAsync(cypher, new Dictionary<string, object>() { { "metadatas", parameters } });
            }
        }



        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
