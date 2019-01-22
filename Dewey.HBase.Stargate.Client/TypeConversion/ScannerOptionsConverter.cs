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

using System.Xml.Linq;
using Dewey.HBase.Stargate.Client.Api;

namespace Dewey.HBase.Stargate.Client.TypeConversion
{
	/// <summary>
	///    Provides a standard implementation of the
	///    <see cref="IScannerOptionsConverter" /> interface.
	/// </summary>
	public class ScannerOptionsConverter : IScannerOptionsConverter
	{
		private const string _scannerName = "Scanner";
		private const string _startRowName = "startRow";
		private const string _stopRowName = "stopRow";
		private const string _batchSizeName = "batch";
		private const string _filterName = "filter";
		private readonly ICodec _codec;

		/// <summary>
		///    Initializes a new instance of the <see cref="ScannerOptionsConverter" /> class.
		/// </summary>
		/// <param name="codec">The codec to use while encoding values.</param>
		public ScannerOptionsConverter(ICodec codec)
		{
			_codec = codec;
		}

		/// <summary>
		///    Converts the specified options to an XML document with
		///    filter options as embedded JSON.
		/// </summary>
		/// <param name="options">The options.</param>
		public string Convert(ScannerOptions options)
		{
			var xml = new XElement(_scannerName);

			if (!string.IsNullOrEmpty(options.StartRow))
			{
				xml.Add(new XAttribute(_startRowName, _codec.Encode(options.StartRow)));
			}

			if (!string.IsNullOrEmpty(options.StopRow))
			{
				xml.Add(new XAttribute(_stopRowName, _codec.Encode(options.StopRow)));
			}

			if (options.BatchSize.HasValue)
			{
				xml.Add(new XAttribute(_batchSizeName, options.BatchSize));
			}

			if (options.Filter != null)
			{
				xml.Add(new XElement(_filterName)
				{
					Value = options.Filter.ConvertToJson(_codec).ToString()
				});
			}

			return xml.ToString();
		}
	}
}