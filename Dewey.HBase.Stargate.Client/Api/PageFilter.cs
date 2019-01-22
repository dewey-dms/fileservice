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
	///    Implementation of Filter interface that limits results to a specific page size. It terminates
	///    scanning once the number of filter-passed rows is > the given page size. Note that this filter
	///    cannot guarantee that the number of results returned to a client are &lt;= page size. This is
	///    because the filter is applied separately on different region servers. It does however optimize
	///    the scan of individual HRegions by making sure that the page size is never exceeded locally.
	/// </summary>
	public class PageFilter : TypeValueFilterBase
	{
		private readonly long _pageSize;

		/// <summary>
		///    Initializes a new instance of the <see cref="PageFilter" /> class.
		/// </summary>
		/// <param name="pageSize">Size of the page.</param>
		public PageFilter(long pageSize)
		{
			_pageSize = pageSize;
		}

		/// <summary>
		/// Gets the token to use as the value.
		/// </summary>
		/// <param name="codec">The codec to use for encoding values.</param>
		protected override JToken GetValueJToken(ICodec codec)
		{
			return new JValue(_pageSize);
		}
	}
}