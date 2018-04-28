// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Neo4JSample.Converters.Parameters
{
    public abstract class BaseDictionaryConverter<TSourceType> : IConverter<TSourceType, Dictionary<string, object>>
    {
        public Dictionary<string, object> Convert(TSourceType source)
        {
            if (source == null)
            {
                return null;
            }

            var target = new Dictionary<string, object>();

            InternalConvert(source, target);

            return target;
        }

        protected abstract void InternalConvert(TSourceType source, Dictionary<string, object> target);

        public Type SourceType => typeof(TSourceType);

        public Type TargetType => typeof(Dictionary<string, object>);
    }
}
