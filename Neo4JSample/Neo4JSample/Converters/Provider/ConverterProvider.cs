using System;
using System.Collections.Generic;
using System.Reflection;
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4JSample.Model.Converters;

namespace Neo4JSample.Converters.Provider
{
    public class ConverterProvider : IConverterProvider
    {
        private readonly IDictionary<Tuple<Type, Type>, IConverter> converters;

        public ConverterProvider()
        {
            converters = new Dictionary<Tuple<Type, Type>, IConverter>();

            Add(new MovieConverter());
            Add(new GenreConverter());
            Add(new PersonConverter());
            Add(new MetadataConverter());
        }

        public ConverterProvider Add<TSourceType, TTargetType>(IConverter<TSourceType, TTargetType> converter)
        {
            var sourceType = converter.SourceType;
            var targetType = converter.TargetType;

            var key = Tuple.Create(sourceType, targetType);

            if (converters.ContainsKey(key))
            {
                throw new Exception($"Existing registration for Converter with Source Type '{sourceType}' and Target Type '{targetType}'");
            }

            converters[key] = converter;

            return this;
        }

        public IConverter<TSourceType, TTargetType> Resolve<TSourceType, TTargetType>()
        {
            var sourceType = typeof(TSourceType);
            var targetType = typeof(TTargetType);

            var key = Tuple.Create(sourceType, targetType);
            
            IConverter converter = null;
            if (!converters.TryGetValue(key, out converter))
            {
                throw new Exception($"No TypeConverter registered for Source Type '{sourceType}' and TargetType '{targetType}'");
            }

            return converter as IConverter<TSourceType, TTargetType>;
        }
    }
}
