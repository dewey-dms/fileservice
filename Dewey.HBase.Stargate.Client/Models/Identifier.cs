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

namespace Dewey.HBase.Stargate.Client.Models
{
  /// <summary>
  ///   Defines an identifier in the HBase system.
  /// </summary>
  public class Identifier : HBaseDescriptor, IEquatable<Identifier>
  {
    /// <summary>
    ///   Gets or sets the cell descriptor.
    /// </summary>
    public HBaseCellDescriptor CellDescriptor { get; set; }

    /// <summary>
    ///   Gets or sets the timestamp.
    /// </summary>
    public long? Timestamp { get; set; }

    /// <summary>
    ///   Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    public bool Equals(Identifier other)
    {
      return base.Equals(other) && this.CheckedEquals(other,
        (left, right) => left.Timestamp == right.Timestamp
          && left.CellDescriptor == right.CellDescriptor);
    }

    /// <summary>
    ///   Returns a hash code for this instance.
    /// </summary>
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    /// <summary>
    ///   Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    /// </summary>
    public override bool Equals(object other)
    {
      return Equals(other as Identifier);
    }

    /// <summary>
    ///   Implements the operator ==.
    /// </summary>
    public static bool operator ==(Identifier left, Identifier right)
    {
      return left.CheckedEquals(right);
    }

    /// <summary>
    ///   Implements the operator !=.
    /// </summary>
    public static bool operator !=(Identifier left, Identifier right)
    {
      return !(left == right);
    }
  }
}