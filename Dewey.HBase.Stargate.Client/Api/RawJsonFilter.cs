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

using Dewey.HBase.Stargate.Client.TypeConversion;
using Newtonsoft.Json.Linq;

namespace Dewey.HBase.Stargate.Client.Api
{
  /// <summary>
  ///   Provides a filter that can be used to configure a scanner
  ///   manually. Note that all value encoding must be done by the
  ///   caller. Not recommended for normal use.
  /// </summary>
  public class RawJsonFilter : IScannerFilter
  {
    private readonly JObject _jObject;

    /// <summary>
    ///   Initializes a new instance of the <see cref="RawJsonFilter" /> class.
    /// </summary>
    /// <param name="filterJson">The filter json.</param>
    public RawJsonFilter(string filterJson)
    {
      _jObject = JObject.Parse(filterJson);
    }

    /// <summary>
    ///   Converts the filter to its JSON representation.
    /// </summary>
    /// <param name="codec">Not used</param>
    public JObject ConvertToJson(ICodec codec)
    {
      return _jObject;
    }
  }
}