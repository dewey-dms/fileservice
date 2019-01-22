#region FreeBSD

// Copyright (c) 2013, The Tribe
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
	///    This filter is used for selecting only those keys with columns that are between a minimum and
	///    a maximum column. Column values are evaluated as prefixes (see <see cref="ColumnPrefixFilter" />).
	/// </summary>
	public class ColumnRangeFilter : ScannerFilterBase
	{
		private const string _minColumnPropertyName = "minColumn";
		private const string _maxColumnPropertyName = "maxColumn";
		private const string _minColumnInclusivePropertyName = "minColumnInclusive";
		private const string _maxColumnInclusivePropertyName = "maxColumnInclusive";
		private readonly string _maxColumn;
		private readonly bool _maxColumnInclusive;
		private readonly string _minColumn;
		private readonly bool _minColumnInclusive;

		/// <summary>
		///    Initializes a new instance of the <see cref="ColumnRangeFilter" /> class. If <paramref name="minColumn" /> is 'an',
		///    and <paramref name="maxColumn" /> is 'be', it will pass keys with columns like 'ana'/'bad', but not keys with
		///    columns like 'bed'/'eye'. If <paramref name="minColumn" /> is null, there is no lower bound. If
		///    <paramref name="maxColumn" /> is null, there is no upper bound. <paramref name="minColumnInclusive" /> and
		///    <paramref name="maxColumnInclusive" /> specify if the ranges are inclusive or not.
		/// </summary>
		/// <param name="minColumn">The min column.</param>
		/// <param name="maxColumn">The max column.</param>
		/// <param name="minColumnInclusive">
		///    if set to <c>true</c> <paramref name="minColumn" /> is inclusive. The default is true.
		/// </param>
		/// <param name="maxColumnInclusive">
		///    if set to <c>true</c> <paramref name="maxColumn" /> is inclusive. The default is true.
		/// </param>
		public ColumnRangeFilter(string minColumn, string maxColumn, bool minColumnInclusive = true, bool maxColumnInclusive = true)
		{
			_minColumn = minColumn;
			_maxColumn = maxColumn;
			_minColumnInclusive = minColumnInclusive;
			_maxColumnInclusive = maxColumnInclusive;
		}

		/// <summary>
		/// Converts the filter to its JSON representation.
		/// </summary>
		/// <param name="codec">The codec to use for encoding values.</param>
		public override JObject ConvertToJson(ICodec codec)
		{
			JObject json = base.ConvertToJson(codec);

			json[_minColumnPropertyName] = string.IsNullOrEmpty(_minColumn) ? null : new JValue(codec.Encode(_minColumn));
			json[_maxColumnPropertyName] = string.IsNullOrEmpty(_maxColumn) ? null : new JValue(codec.Encode(_maxColumn));
			json[_minColumnInclusivePropertyName] = _minColumnInclusive;
			json[_maxColumnInclusivePropertyName] = _maxColumnInclusive;

			return json;
		}
	}
}