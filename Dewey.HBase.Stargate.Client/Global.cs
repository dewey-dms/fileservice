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

namespace Dewey.HBase.Stargate.Client
{
  /// <summary>
  ///   Encapsulates "global" functionality typically used internally during low-level operations.
  /// </summary>
  public static class Global
  {
    static Global()
    {
      MutableHashCodeRetriever = GetDefaultMutableHashCodeRetriever();
      StringEqualityComparison = GetDefaultStringEqualityComparison();
    }

    /// <summary>
    ///   Gets or sets the hash code retriever for mutable (un-hashable) types.
    /// </summary>
    public static Func<int> MutableHashCodeRetriever { get; set; }

    /// <summary>
    ///   Gets or sets the string equality comparison used in low-level comparison operations.
    /// </summary>
    public static Func<string, string, bool> StringEqualityComparison { get; set; }

    /// <summary>
    ///   Checks for equality by running a reference comparison,
    ///   then by running the provided model comparison if both arguments
    ///   are non-null.
    /// </summary>
    internal static bool CheckedEquals<TObject>(this TObject self, TObject other,
      Func<TObject, TObject, bool> modelComparison = null) where TObject : IEquatable<TObject>
    {
      modelComparison = modelComparison ?? ((left, right) => left.Equals(right));
      return ReferenceEquals(self, other) ||
        (NullSafe(self, other) && modelComparison(self, other));
    }

    internal static bool CheckedSetsEqual<TObject>(this IEnumerable<TObject> self, IEnumerable<TObject> other,
      Func<TObject, TObject, bool> modelComparison = null) where TObject : IEquatable<TObject>
    {
      return ReferenceEquals(self, other) ||
        (NullSafe(self, other) && CollectionItemsMatch(self, other));
    }

    internal static bool EqualsString(this string text, string other)
    {
      Func<string, string, bool> comparison = StringEqualityComparison
        ?? GetDefaultStringEqualityComparison();

      return comparison(text, other);
    }

    internal static int GetMutableHashCode(this object self)
    {
      Func<int> retriever = MutableHashCodeRetriever
        ?? GetDefaultMutableHashCodeRetriever();

      return retriever();
    }

    internal static bool NullSafe(params object[] args)
    {
      return args.All(arg => arg != null);
    }

    private static bool CollectionItemsMatch<TObject>(IEnumerable<TObject> self, IEnumerable<TObject> other) where TObject : IEquatable<TObject>
    {
      TObject[] selfItems = self.ToArray();
      TObject[] otherItems = other.ToArray();
      return selfItems.Length == otherItems.Length && selfItems.All(otherItems.Contains);
    }

    private static Func<int> GetDefaultMutableHashCodeRetriever()
    {
      // force all mutable types to go through full equality comparison
      return () => 1;
    }

    private static Func<string, string, bool> GetDefaultStringEqualityComparison()
    {
      return (left, right) => StringComparer.Ordinal.Equals(left ?? string.Empty, right ?? string.Empty);
    }
  }
}