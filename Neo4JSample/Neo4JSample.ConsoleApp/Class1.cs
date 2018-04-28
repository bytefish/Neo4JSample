using Neo4JSample.ConsoleApp.Services;
using System;
using System.Threading.Tasks;

namespace Neo4JSample.ConsoleApp
{
    public class ConsoleApplication
    {

        public static void Main(string[] args)
        {
            var service = new MovieDataService();

            RunAsync(service).GetAwaiter().GetResult();
        }

        public static async Task RunAsync(IMovieDataService service)
        {
            using (var client = new Neo4JClient("bolt://localhost:7687/db/actors", "neo4j", "test_pwd"))
            {
                // Create Indices for faster Lookups:
                await client.CreateIndices();

                // Create Base Data:
                await client.CreateMovies(service.Movies);
                await client.CreatePersons(service.Persons);
                await client.CreateGenres(service.Genres);

                // Create Relationships:
                await client.CreateRelationships(service.Metadatas);
            }
        }
    }
}
