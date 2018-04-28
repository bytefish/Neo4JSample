// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Neo4JSample.Model
{
    public class Metadata
    {
        public Movie Movie { get; set; }

        public Person Director { get; set; }

        public IList<Genre> Genres { get; set; }

        public IList<Person> Cast { get; set; }
    }
}