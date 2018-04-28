// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Neo4JSample.Converters.Parameters;

namespace Neo4JSample.Model.Converters
{
    public class MovieConverter : BaseDictionaryConverter<Movie>
    {
        protected override void InternalConvert(Movie source, Dictionary<string, object> target)
        {
            target["id"] = source.Id;
            target["title"] = source.Title;
        }
    }
}
