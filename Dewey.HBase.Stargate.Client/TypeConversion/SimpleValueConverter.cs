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

using System;
using System.Collections.Generic;
using System.Linq;
using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.HBase.Stargate.Client.TypeConversion
{
	/// <summary>
	///    Provides a standard implementation of <see cref="ISimpleValueConverter" />.
	/// </summary>
	public class SimpleValueConverter : ISimpleValueConverter
	{
		private static readonly IList<Tuple<string, BloomFilters>> _bloomFilters
			= new List<Tuple<string, BloomFilters>>
			{
				new Tuple<string, BloomFilters>("NONE", BloomFilters.None),
				new Tuple<string, BloomFilters>("ROW", BloomFilters.Row),
				new Tuple<string, BloomFilters>("ROWCOL", BloomFilters.RowColumn)
			};

		private static readonly IList<Tuple<string, CompressionTypes>> _compressionTypes
			= new List<Tuple<string, CompressionTypes>>
			{
				new Tuple<string, CompressionTypes>("NONE", CompressionTypes.None),
				new Tuple<string, CompressionTypes>("GZ", CompressionTypes.GZip),
				new Tuple<string, CompressionTypes>("LZ4", CompressionTypes.Lz4),
				new Tuple<string, CompressionTypes>("LZO", CompressionTypes.Lzo),
				new Tuple<string, CompressionTypes>("SNAPPY", CompressionTypes.Snappy)
			};

		private static readonly IList<Tuple<string, DataBlockEncodings>> _encodings
			= new List<Tuple<string, DataBlockEncodings>>
			{
				new Tuple<string, DataBlockEncodings>("NONE", DataBlockEncodings.None),
				new Tuple<string, DataBlockEncodings>("PREFIX", DataBlockEncodings.Prefix),
				new Tuple<string, DataBlockEncodings>("DIFF", DataBlockEncodings.Diff),
				new Tuple<string, DataBlockEncodings>("FAST_DIFF", DataBlockEncodings.FastDiff)
			};

		/// <summary>
		///    Converts the bloom filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		public string ConvertBloomFilter(BloomFilters? filter)
		{
			filter = filter ?? default(BloomFilters);
			return _bloomFilters.Where(item => item.Item2 == filter)
				.Select(item => item.Item1)
				.FirstOrDefault();
		}

		/// <summary>
		///    Converts the bloom filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		public BloomFilters ConvertBloomFilter(string filter)
		{
			return _bloomFilters.Where(item => item.Item1 == filter)
				.Select(item => item.Item2)
				.FirstOrDefault();
		}

		/// <summary>
		///    Converts the type of the compression.
		/// </summary>
		/// <param name="compressionType">Type of the compression.</param>
		public string ConvertCompressionType(CompressionTypes? compressionType)
		{
			compressionType = compressionType ?? default(CompressionTypes);
			return _compressionTypes.Where(item => item.Item2 == compressionType)
				.Select(item => item.Item1)
				.FirstOrDefault();
		}

		/// <summary>
		///    Converts the type of the compression.
		/// </summary>
		/// <param name="compressionType">Type of the compression.</param>
		public CompressionTypes ConvertCompressionType(string compressionType)
		{
			return _compressionTypes.Where(item => item.Item1 == compressionType)
				.Select(item => item.Item2)
				.FirstOrDefault();
		}

		/// <summary>
		///    Converts the data block encoding.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		public string ConvertDataBlockEncoding(DataBlockEncodings? encoding)
		{
			encoding = encoding ?? default(DataBlockEncodings);
			return _encodings.Where(item => item.Item2 == encoding)
				.Select(item => item.Item1)
				.FirstOrDefault();
		}

		/// <summary>
		///    Converts the data block encoding.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		public DataBlockEncodings ConvertDataBlockEncoding(string encoding)
		{
			return _encodings.Where(item => item.Item1 == encoding)
				.Select(item => item.Item2)
				.FirstOrDefault();
		}
	}
}