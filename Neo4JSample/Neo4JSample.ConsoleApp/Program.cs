using Neo4JSample.ConsoleApp.Services;
using System;
using System.Threading.Tasks;
using Neo4JSample.Settings;

namespace Neo4JSample.ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var service = new MovieDataService();

            RunAsync(service).GetAwaiter().GetResult();
        }

        public static async Task RunAsync(IMovieDataService service)
        {
            var settings = ConnectionSettings.CreateBasicAuth("bolt://localhost:7687/db/actors", "neo4j", "test_pwd");

            using (var client = new Neo4JClient(settings))
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
