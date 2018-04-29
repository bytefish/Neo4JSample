# Neo4JSample #

This is an example for working with the official Neo4J Client in C#. It is written for the blog post:

* [https://bytefish.de/blog/neo4j_dotnet/](https://bytefish.de/blog/neo4j_dotnet/)

## Screenshot ##

<a href="https://github.com/bytefish/Neo4JSample/raw/master/Neo4JSample/Screenshots/neo4j_browser.jpg">
    <img src="https://github.com/bytefish/Neo4JSample/raw/master/Neo4JSample/Screenshots/neo4j_browser.jpg" width="50%">
</a>

## Article ##

Document Databases, Map Reduce, Hadoop... NoSQL. Name a hype technology from the 2000s and chance is good 
I was a **huge** fanboy. Then I started working and realized: one of the few constants in our industry are 
Relational Database Management Systems. So by now I am a **huge** RDBMS fanboy (of course) and advertise 
to use SQL for everything. 

My rule of thumb for every new project is: 

> When in doubt use PostgreSQL. 

## Wait... A Silver Bullet? ##

Well. Sometimes no matter how hard you try, no matter how often you iterate over your relational model, your 
queries are hell. A product of sweat, tears and 23 Joins stuck inside a chain of [Common Table Expressions], 
that only a query planner could make sense of. It's hard to admit, but sometimes SQL doesn't fit.

That's the reason why I have taught myself how to work with [Elasticsearch in .NET](https://bytefish.de/blog/elasticsearch_net/) [and Java](https://bytefish.de/blog/elasticsearch_java/), 
how to do [Stream Data Processing with Apache Flink] and [Complex Event Processing] for detecting complex patterns in 
temporal data.

I still advertise using a Relational Database as your Primary Datastore, but I also advertise to complement 
your relational data model with the best tools and data representations for the job at hand.

[Common Table Expressions]: https://en.wikipedia.org/wiki/Hierarchical_and_recursive_queries_in_SQL#Common_table_expression
[Stream Data Processing with Apache Flink]: https://bytefish.de/blog/stream_data_processing_flink/
[Complex Event Processing]: https://bytefish.de/blog/apache_flink_series_5/
[SQL Server Column Store]: https://github.com/bytefish/WeatherDataColumnStore

## Enter Graph Databases ##

[Microsoft has a perfect introduction to Graph Databases]: https://docs.microsoft.com/en-us/sql/relational-databases/graphs/sql-graph-overview?view=sql-server-2017

In real life you often have to deal complex many-to-many relationships in your data, think of big Social Networks. These networks have 
ever-evolving relationships in the data, that will be hard to add in a relational database. Also analyzing these relationships in a 
relational database and making sense of it becomes a very tedious job.

[Microsoft has a perfect introduction to Graph Databases] and their use cases :

> ### What is a graph database ###
>
> A graph database is a collection of nodes (or vertices) and edges (or relationships). A node represents an entity (for example, 
> a person or an organization) and an edge represents a relationship between the two nodes that it connects (for example, likes or 
> friends). Both nodes and edges may have properties associated with them. Here are some features that make a graph database unique:
>
> * Edges or relationships are first class entities in a Graph Database and can have attributes or properties associated with them.
> * A single edge can flexibly connect multiple nodes in a Graph Database.
> * You can express pattern matching and multi-hop navigation queries easily.
> * You can express transitive closure and polymorphic queries easily.
>
> ### When to use a graph database ###
>
> There is nothing a graph database can achieve, which cannot be achieved using a relational database. However, 
> a graph database can make it easier to express certain kind of queries. Also, with specific optimizations, certain 
> queries may perform better. Your decision to choose one over the other can be based on following factors:
>
> * Your application has hierarchical data. The HierarchyID datatype can be used to implement hierarchies, but it has some limitations. For example, it does not allow you to store multiple parents for a node.
> * Your application has complex many-to-many relationships; as application evolves, new relationships are added.
> * You need to analyze interconnected data and relationships.

### Should I care? Do Graph Databases matter? ###

Graph database can be a very, very useful tool to simplify your life, when SQL queries become your painful enemy. But is it useful to 
learn about them? Do they matter? I think so, because parts of the industry are currently betting big on Graph data processing, like:

... Microsoft:

* [CosmosDB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction)
* [Graph Engine](https://www.graphengine.io/)
* [SQL Server 2017 Graph Database](https://docs.microsoft.com/en-us/sql/relational-databases/graphs/sql-graph-overview?view=sql-server-2017)

... or SAP:

* [SAP HANA Graph Engine](https://blogs.sap.com/2016/08/01/what-s-new-in-sap-hana-sps12-sap-hana-graph-engine/)

... or Oracle:

* [Oracle Spatial and Graph](http://www.oracle.com/technetwork/database/options/spatialandgraph/overview/index.html)

So your database might already include Graph database capabilities!

### On using Neo4j for the Article ###

[Cypher Query Language]: https://en.wikipedia.org/wiki/Cypher_Query_Language
[Neo4j]: https://neo4j.com
[DB-Engines Ranking of Graph DBMS]: https://db-engines.com/en/ranking/graph+dbms

So what Graph database should I use? The SQL Server 2017 Graph Database would be interesting, but it is at an early stage of development, and it comes with 
limited [Cypher Query Language] support. I don't have a SAP HANA License and no Oracle License at hand. 

So I decided to base this Graph database example on [Neo4j], which is the most popular Open Source Graph Database (according to the [DB-Engines Ranking of Graph DBMS]):

> [Neo4j] is a graph database management system developed by Neo4j, Inc. Described by its developers as an ACID-compliant transactional database 
> with native graph storage and processing, Neo4j is the most popular graph database according to DB-Engines ranking.

## The Project: The Movie Database ##

The canonical Neo4j introduction is the [Movie Database](https://neo4j.com/developer/movie-database/):

> The movie database is a traditional dataset for graph databases, similiar to IMDB it contains movies and actors, 
> directors, producers etc. It could be extended with Genres, Publishers, Ratings and more.

There are sample projects for Movie database applications in languages like Java, .NET, JavaScript, Python, Ruby, Elixir and even Haskell:

* [https://github.com/neo4j-examples](https://github.com/neo4j-examples/)

### The Plan ###

All of the Movie Database sample projects deal with querying the dataset. As a developer I ask myself instantly: How is the data actually imported? 
How do you Bulk Insert data efficiently? It turns out, that the data import for the Movie database applications is done with CSV files and uses the 
Neo4j ``LOAD CSV`` statement: 

* [https://neo4j.com/developer/example-project/#_data_setup](https://neo4j.com/developer/example-project/#_data_setup)

In this article I will you show how to bulk insert data with the Neo4j .NET driver without using CSV files.

### Source Code ###

The Source Code for this project can be found at:

* [https://github.com/bytefish/Neo4JSample/](https://github.com/bytefish/Neo4JSample/)

## Database Schema ##

The Graph data model is straightforward:

```
(:Movie {id, title})
(:Person {id, name​})
(:Genre {name})

(:Person)-[:ACTED_IN|:DIRECTED]→(:Movie)
(:Movie)-[:GENRE])→(:Genre)
```

Using the Neo4j Browser the Graph can be nicely visualized as a Graph and exported as SVG:

<div style="display:flex; align-items:center; justify-content:center;">
    <img style="width: 50%;" src="https://github.com/bytefish/Neo4JSample/raw/master/Neo4JSample/Screenshots/graph.png">
</div>

[Neo4j Browser]: https://neo4j.com/developer/guide-neo4j-browser/

## .NET Implementation ##

### Installing the .NET Client ###

The [Neo4jDotNetDriver](https://neo4j.com/developer/dotnet/) is the officially supported .NET driver for Neo4j. It can be easily installed with NuGet:

```
PM> Install-Package Neo4j.Driver
```

### Domain Model ###

Next we are translating the Graph Model into a Domain model. There is an additional entity *MovieInformation*, which is used to model the Relationships 
between Movies, Director, Genres and Cast.

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Neo4JSample.Model
{
    public class Genre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    
    public class Movie
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
    
    public class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    
    public class MovieInformation
    {
        [JsonProperty("movie")]
        public Movie Movie { get; set; }

        [JsonProperty("director")]
        public Person Director { get; set; }

        [JsonProperty("genres")]
        public IList<Genre> Genres { get; set; }

        [JsonProperty("cast")]
        public IList<Person> Cast { get; set; }
    }
}
```

### Connection Settings ###

Next we are abstracting the Connection Settings away. You might feel like this is an unneccessary abstraction, but imagine you don't want to 
hardcode the Credentials anymore and want to load them with a Configuration file. If you inject the ``IConnectionSettings`` into a class, 
then all you have to do is implement the ``IConnectionSettings``.

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4j.Driver.V1;

namespace Neo4JSample.Settings
{
    public interface IConnectionSettings
    {
        string Uri { get; }

        IAuthToken AuthToken { get; }
    }
    
    public class ConnectionSettings : IConnectionSettings
    {
        public string Uri { get; private set; }

        public IAuthToken AuthToken { get; private set; }

        public ConnectionSettings(string uri, IAuthToken authToken)
        {
            Uri = uri;
            AuthToken = authToken;
        }

        public static ConnectionSettings CreateBasicAuth(string uri, string username, string password)
        {
            return new ConnectionSettings(uri, AuthTokens.Basic(username, password));
        }
    }
}
```

### Parameter Serialization ###

[JSON.NET]: https://www.newtonsoft.com/json
[Neo4j .NET driver source code]: https://github.com/neo4j/neo4j-dotnet-driver/blob/1.6/Neo4j.Driver/Neo4j.Driver/V1/Extensions/ValueExtensions.cs

There is a catch with the official Neo4j .NET Driver: It doesn't support user-defined types. In the [Neo4j .NET driver source code] this is 
explicitly mentioned ([here](https://github.com/neo4j/neo4j-dotnet-driver/blob/c58e56f88a90cf579296e6e7c306f721b7e6c2de/Neo4j.Driver/Neo4j.Driver/V1/Extensions/ValueExtensions.cs#L55)): 

```csharp
/// <summary>
/// A helper method to explicitly cast the value streamed back via Bolt to a local type.
/// </summary>
/// <typeparam name="T">
/// Supports for the following types (or nullable version of the following types if applies):
/// ...
///
/// Undefined support for other types that are not listed above.
/// No support for user-defined types, e.g. Person, Movie.
/// </typeparam>
```

The Neo4j .NET driver expects you to pass your data as Dictionaries. So to overcome this issue I have used [JSON.NET] and serialize my domain model to JSON first and 
then deserialize it into a ``IDictionary<string, object>`` again. The nested user-defined objects also need to be deserialized as a ``IDictionary<string, object>``.

To do this we need to define a Custom JSON Converter first:

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo4JSample.Serializer.Converters
{
    /// <summary>
    /// This Converter is only a slightly modified converter from the JSON Extension library. 
    /// 
    /// All Credit goes to Oskar Gewalli (https://github.com/wallymathieu) and the Makrill Project (https://github.com/NewtonsoftJsonExt/makrill).
    /// </summary>
    public class CustomDictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ExpectObject(reader);
        }

        private static object ExpectDictionaryOrArrayOrPrimitive(JsonReader reader)
        {
            reader.Read();
            var startToken = reader.TokenType;
            switch (startToken)
            {
                case JsonToken.String:
                case JsonToken.Integer:
                case JsonToken.Boolean:
                case JsonToken.Bytes:
                case JsonToken.Date:
                case JsonToken.Float:
                case JsonToken.Null:
                    return reader.Value;
                case JsonToken.StartObject:
                    return ExpectObject(reader);
                case JsonToken.StartArray:
                    return ExpectArray(reader);
            }
            throw new JsonSerializationException($"Unrecognized token: {reader.TokenType}");
        }

        private static object ExpectObject(JsonReader reader)
        {
            var dic = new Dictionary<string, object>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;
                    case JsonToken.PropertyName:
                        dic.Add(reader.Value.ToString(), ExpectDictionaryOrArrayOrPrimitive(reader));
                        break;
                    case JsonToken.EndObject:
                        return dic;
                    default:
                        throw new JsonSerializationException($"Unrecognized token: {reader.TokenType}");
                }
            }
            throw new JsonSerializationException("Missing End Token");
        }

        private static object ExpectArray(JsonReader reader)
        {
            var array = new List<Object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Null:
                        array.Add(reader.Value);
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.StartObject:
                        array.Add(ExpectObject(reader));
                        break;
                    case JsonToken.StartArray:
                        array.Add(ExpectArray(reader));
                        break;
                    case JsonToken.EndArray:
                        return array.ToArray();
                    default:
                        throw new JsonSerializationException($"Unrecognized token: {reader.TokenType}");
                }
            }
            throw new JsonSerializationException("Missing End Token");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }
    }
}
```

And finally we can write a ``ParameterSerializer``, which turns given list of user-defined types into a list of Dictionaries (with nested Dictionaries):

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Neo4JSample.Serializer.Converters;
using Newtonsoft.Json;

namespace Neo4JSample.Serializer
{
    public static class ParameterSerializer
    {
        public static IList<Dictionary<string, object>> ToDictionary<TSourceType>(IList<TSourceType> source)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(source, settings);

            return JsonConvert.DeserializeObject<IList<Dictionary<string, object>>>(json, new CustomDictionaryConverter());
        }
    }
}
```


### The Neo4j Client ###

Now we can implement the ``Neo4JClient`` to insert the Movie database. I am doing a lot of ``MERGE`` operations in the Cypher queries. So I am also adding indexes on 
the properties to provide faster lookups while inserting the data. I want the Client to be asynchronous, that's why I am exlusively using the Asynchronous API of 
the Neo4j .NET Driver. 

You don't see any transaction handling in here, it's because the ``RunAsync`` automatically commits a transaction, if the Query was successful. For Bulk Inserts I 
have used the ``UNWIND`` operator for all the inserts, and the ``ParameterSerializer`` is used to prepare the user-defined types for the Neo4j .NET Driver.

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4j.Driver.V1;
using Neo4JSample.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Neo4JSample.Serializer;
using Neo4JSample.Settings;

namespace Neo4JSample
{
    public class Neo4JClient : IDisposable
    {
        private readonly IDriver driver;
        
        public Neo4JClient(IConnectionSettings settings)
        {
            this.driver = GraphDatabase.Driver(settings.Uri, settings.AuthToken);
        }

        public async Task CreateIndices()
        {
            string[] queries = {
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
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "persons", ParameterSerializer.ToDictionary(persons) } });
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
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "genres", ParameterSerializer.ToDictionary(genres) } });
            }
        }

        public async Task CreateMovies(IList<Movie> movies)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {movies} AS movie")
                .AppendLine("MERGE (m:Movie {id: movie.id})")
                .AppendLine("SET m = movie")
                .ToString();

            using (var session = driver.Session())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "movies", ParameterSerializer.ToDictionary(movies) } });
            }
        }

        public async Task CreateRelationships(IList<MovieInformation> metadatas)
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
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "metadatas", ParameterSerializer.ToDictionary(metadatas) } });
            }
        }
        
        public void Dispose()
        {
            driver?.Dispose();
        }
    }
}
```

### Providing the Movie Database data ###

What's still left is the data. I decided to go with Mocked data first, but due to the ``IMovieDataService`` you can easily switch the 
implementation to a real datasource. Don't pin me on correctness of the movie information:

```csharp
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
```

### Using the Movie Database Client ###

It looks like so much preliminary work just to insert some data. But with all the infrastructure code using our ``Neo4JClient`` and connecting the 
parts becomes really easy. You probably need to adjust the username and password to run the example against your Neo4j database:

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4JSample.ConsoleApp.Services;
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
```

## Querying the Data ##

### Neo4j Browser ###

The Neo4j Browser is an excellent tool to query and visualize your data. To open the Neo4j Browser go to:

```
http://localhost:7474/browser/
```

It will show you the data we have just inserted:

<a href="https://github.com/bytefish/Neo4JSample/raw/master/Neo4JSample/Screenshots/neo4j_browser_movie_database.jpg">
    <img src="https://github.com/bytefish/Neo4JSample/raw/master/Neo4JSample/Screenshots/neo4j_browser_movie_database.jpg" width="50%">
</a>


So let's go ahead and query the graph!

#### Who acted in a Movie? ####

```
MATCH (m:Movie {title: 'Kill Bill'})<-[:ACTED_IN]-(a:Person)
RETURN a.name
```

<pre>
╒═════════════╕
│"a.name"     │
╞═════════════╡
│"Uma Thurman"│
└─────────────┘
</pre>

#### Which Action movies are available? ####

```
MATCH (g:Genre {name: 'Action'})<-[:GENRE]-(m:Movie)
RETURN m.title
```

<pre>
╒═════════════╕
│"m.title"    │
╞═════════════╡
│"Running Man"│
├─────────────┤
│"Kill Bill"  │
└─────────────┘
</pre>

## Conclusion ##

Working with the official Neo4j .NET driver turned out to be more complicated, than I initially expected. But it was easy to overcome these issues 
with just a little infrastructure code, and it's now easier to use for upcoming projects. I think, that the Neo4j Browser is a great way to analyze 
and visualize your data and their relationships. I love the instant feedback!

The dataset in this article was admittedly tiny. In the next article I will investigate how Neo4j performs at scale.
