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

namespace Dewey.HBase.Stargate.Client.Models
{
	/// <summary>
	///    Encapsulates the options available for new column creation.
	/// </summary>
	public class ColumnSchema
	{
		/// <summary>
		///    Gets or sets the name.
		/// </summary>
		/// <value>
		///    The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the block size.
		/// </summary>
		/// <value>
		///    The block size.
		/// </value>
		public int? BlockSize { get; set; }

		/// <summary>
		///    Gets or sets the bloom filter.
		/// </summary>
		/// <value>
		///    The bloom filter.
		/// </value>
		public BloomFilters? BloomFilter { get; set; }

		/// <summary>
		///    Gets or sets the minimum number of versions to track.
		/// </summary>
		/// <value>
		///    The minimum number of versions to track.
		/// </value>
		public int? MinVersions { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether to keep deleted cells.
		/// </summary>
		/// <value>
		///    <c>true</c> if HBase should keep deleted cells; otherwise, <c>false</c>.
		/// </value>
		public bool? KeepDeletedCells { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether to encode on disk.
		/// </summary>
		/// <value>
		///    <c>true</c> if HBase should encode on disk; otherwise, <c>false</c>.
		/// </value>
		public bool? EncodeOnDisk { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether to use the block cache.
		/// </summary>
		/// <value>
		///    <c>true</c> if HBase should use the block cache; otherwise, <c>false</c>.
		/// </value>
		public bool? BlockCache { get; set; }

		/// <summary>
		///    Gets or sets the compression type.
		/// </summary>
		/// <value>
		///    The compression type.
		/// </value>
		public CompressionTypes? Compression { get; set; }

		/// <summary>
		///    Gets or sets the maximum number of versions to track.
		/// </summary>
		/// <value>
		///    The maximum number of versions to track.
		/// </value>
		public int? Versions { get; set; }

		/// <summary>
		///    Gets or sets the replication scope.
		/// </summary>
		/// <value>
		///    The replication scope.
		/// </value>
		public int? ReplicationScope { get; set; }

		/// <summary>
		///    Gets or sets the time to live.
		/// </summary>
		/// <value>
		///    The time to live.
		/// </value>
		public int? TimeToLive { get; set; }

		/// <summary>
		///    Gets or sets the data block encoding.
		/// </summary>
		/// <value>
		///    The data block encoding.
		/// </value>
		public DataBlockEncodings? DataBlockEncoding { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether the column should be held in memory.
		/// </summary>
		/// <value>
		///    <c>true</c> if HBase should hold the column in memory; otherwise, <c>false</c>.
		/// </value>
		public bool? InMemory { get; set; }
	}
}