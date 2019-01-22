﻿#region FreeBSD

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

using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.HBase.Stargate.Client.TypeConversion
{
	/// <summary>
	///    Defines a converter for simple HBase-related values.
	/// </summary>
	public interface ISimpleValueConverter
	{
		/// <summary>
		///    Converts the bloom filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		string ConvertBloomFilter(BloomFilters? filter);

		/// <summary>
		///    Converts the bloom filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		BloomFilters ConvertBloomFilter(string filter);

		/// <summary>
		///    Converts the type of the compression.
		/// </summary>
		/// <param name="compressionType">Type of the compression.</param>
		string ConvertCompressionType(CompressionTypes? compressionType);

		/// <summary>
		///    Converts the type of the compression.
		/// </summary>
		/// <param name="compressionType">Type of the compression.</param>
		CompressionTypes ConvertCompressionType(string compressionType);

		/// <summary>
		///    Converts the data block encoding.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		string ConvertDataBlockEncoding(DataBlockEncodings? encoding);

		/// <summary>
		///    Converts the data block encoding.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		DataBlockEncodings ConvertDataBlockEncoding(string encoding);
	}
}