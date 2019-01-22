#region FreeBSD

// Copyright (c) 2014, The Tribe
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.HBase.Stargate.Client.Models
{
  /// <summary>
  ///   Provides general extensions for Stargate Client components.
  /// </summary>
  public static class Extensions
  {
    private static readonly DateTime _epoch = new DateTime(1970, 1, 1);

    /// <summary>
    ///   Converts the text to a nullable 32-bit integer value.
    /// </summary>
    /// <param name="text">The text.</param>
    public static int? ToNullableInt32(this string text)
    {
      if (string.IsNullOrWhiteSpace(text))
      {
        return null;
      }

      int value;
      return int.TryParse(text, out value) ? value : (int?) null;
    }

    /// <summary>
    ///   Converts the text to a nullable 64-bit integer value.
    /// </summary>
    /// <param name="text">The text.</param>
    public static long? ToNullableInt64(this string text)
    {
      if (string.IsNullOrWhiteSpace(text))
      {
        return null;
      }

      long value;
      return long.TryParse(text, out value) ? value : (long?) null;
    }

    /// <summary>
    ///   Determines whether the descriptor can describe a table.
    /// </summary>
    /// <param name="descriptor">The descriptor.</param>
    public static bool CanDescribeTable(this HBaseDescriptor descriptor)
    {
      return descriptor != null && !string.IsNullOrEmpty(descriptor.Table);
    }

    /// <summary>
    ///   Determines whether the descriptor can describe a row.
    /// </summary>
    /// <param name="descriptor">The descriptor.</param>
    public static bool CanDescribeRow(this HBaseDescriptor descriptor)
    {
      return descriptor.CanDescribeTable() && !string.IsNullOrEmpty(descriptor.Row);
    }

    /// <summary>
    ///   Determines whether the identifier can describe a cell.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    public static bool CanDescribeCell(this Identifier identifier)
    {
      return identifier.CanDescribeRow() && identifier.CellDescriptor != null && !string.IsNullOrEmpty(identifier.CellDescriptor.Column);
    }

    /// <summary>
    ///   Converts the identifier into a cell query.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    public static CellQuery ToQuery(this Identifier identifier)
    {
      return new CellQuery
      {
        Table = identifier.Table,
        Row = identifier.Row,
        Cells = new[] {identifier.CellDescriptor},
        BeginTimestamp = identifier.Timestamp.HasValue ? identifier.Timestamp : null,
        EndTimestamp = identifier.Timestamp.HasValue ? identifier.Timestamp + 1 : null,
        MaxVersions = 1
      };
    }

    /// <summary>
    ///   Returns a value indicating whether or not the two identifiers match.
    ///   If a property has not been set on the other identifier, it will not be included
    ///   in the evaluation.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <param name="other">The other identifier.</param>
    public static bool Matches(this Identifier identifier, Identifier other)
    {
      if (!string.IsNullOrEmpty(other.Table) && other.Table != identifier.Table)
      {
        return false;
      }

      if (!string.IsNullOrEmpty(other.Row) && other.Row != identifier.Row)
      {
        return false;
      }

      HBaseCellDescriptor otherCell = other.CellDescriptor;
      HBaseCellDescriptor currentCell = identifier.CellDescriptor;

      if (otherCell != null && currentCell != null)
      {
        if (!string.IsNullOrEmpty(otherCell.Column) && otherCell.Column != currentCell.Column)
        {
          return false;
        }

        if (!string.IsNullOrEmpty(otherCell.Qualifier) && otherCell.Qualifier != currentCell.Qualifier)
        {
          return false;
        }
      }

      return !other.Timestamp.HasValue || other.Timestamp == identifier.Timestamp;
    }

    /// <summary>
    ///   Gets the first value with an identifier matching the one specified.
    /// </summary>
    /// <param name="cellSet">The cell set.</param>
    /// <param name="identifier">The identifier.</param>
    public static string GetValue(this IEnumerable<Cell> cellSet, Identifier identifier)
    {
      return cellSet.Where(cell => cell.Identifier.Matches(identifier))
        .Select(cell => cell.Value)
        .FirstOrDefault();
    }

    /// <summary>
    ///   Gets the first value with the specified identifier values.
    /// </summary>
    /// <param name="cellSet">The cell set.</param>
    /// <param name="table">The table.</param>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    /// <param name="qualifier">The qualifier.</param>
    /// <param name="timestamp">The timestamp.</param>
    public static string GetValue(this IEnumerable<Cell> cellSet, string table = null, string row = null, string column = null,
      string qualifier = null,
      long? timestamp = null)
    {
      return cellSet.GetValue(new Identifier
      {
        Table = table,
        Row = row,
        CellDescriptor = new HBaseCellDescriptor
        {
          Column = column,
          Qualifier = qualifier
        },
        Timestamp = timestamp
      });
    }

    /// <summary>
    ///   Gets the timestamp as a <see cref="DateTime" />.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public static DateTime? GetTimestampAsDateTime(this Identifier id)
    {
      return AsDateTime(id.Timestamp);
    }

    /// <summary>
    ///   Gets the timestamp as an epoch timestamp.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public static long? GetTimestampAsEpoch(this Identifier id)
    {
      return AsEpoch(id.Timestamp);
    }

    /// <summary>
    ///   Gets the begin timestamp as a <see cref="DateTime" />.
    /// </summary>
    /// <param name="query">The query.</param>
    public static DateTime? GetBeginTimestampAsDateTime(this CellQuery query)
    {
      return AsDateTime(query.BeginTimestamp);
    }

    /// <summary>
    ///   Gets the end timestamp as a <see cref="DateTime" />.
    /// </summary>
    /// <param name="query">The query.</param>
    public static DateTime? GetEndTimestampAsDateTime(this CellQuery query)
    {
      return AsDateTime(query.EndTimestamp);
    }

    /// <summary>
    ///   Gets the begin timestamp as an epoch timestamp.
    /// </summary>
    /// <param name="query">The query.</param>
    public static long? GetBeginTimestampAsEpoch(this CellQuery query)
    {
      return AsEpoch(query.BeginTimestamp);
    }

    /// <summary>
    ///   Gets the end timestamp as an epoch timestamp.
    /// </summary>
    /// <param name="query">The query.</param>
    public static long? GetEndTimestampAsEpoch(this CellQuery query)
    {
      return AsEpoch(query.EndTimestamp);
    }

    private static DateTime? AsDateTime(long? timestamp)
    {
      if (!timestamp.HasValue)
      {
        return null;
      }

      return _epoch.AddMilliseconds(timestamp.Value);
    }

    private static long? AsEpoch(long? timestamp)
    {
      if (!timestamp.HasValue)
      {
        return null;
      }

      return timestamp.Value/1000;
    }
  }
}