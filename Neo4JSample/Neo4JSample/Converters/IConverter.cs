// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Neo4JSample.Converters
{
    public interface IConverter
    {
        Type SourceType { get; }

        Type TargetType { get; }
    }

    public interface IConverter<in TSourceType, out TTargetType> : IConverter
    {
        TTargetType Convert(TSourceType source);
    }
}
