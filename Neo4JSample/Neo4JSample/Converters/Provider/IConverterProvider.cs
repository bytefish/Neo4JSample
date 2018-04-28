// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Neo4JSample.Converters.Provider
{
    public interface IConverterProvider
    {
        ConverterProvider Add<TSourceType, TTargetType>(IConverter<TSourceType, TTargetType> converter);

        IConverter<TSourceType, TTargetType> Resolve<TSourceType, TTargetType>();
    }
}
