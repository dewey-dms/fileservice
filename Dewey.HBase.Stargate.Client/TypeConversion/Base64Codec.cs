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
using System.IO;
using System.Text;

namespace Dewey.HBase.Stargate.Client.TypeConversion
{
	/// <summary>
	///    Provides a standard base64 codec for HBase.
	/// </summary>
	public class Base64Codec : ICodec
	{
		/// <summary>
		///    Encodes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		public virtual string Encode(string text)
		{
			return Convert.ToBase64String(GetEncoding().GetBytes(text));
		}

		/// <summary>
		///    Decodes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		public virtual string Decode(string text)
		{
			using (var reader = new StreamReader(new MemoryStream(Convert.FromBase64String(text)), GetEncoding()))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		///    Gets the <see cref="Encoding" /> object to use during <see cref="Encode" /> and <see cref="Decode" /> operations.
		/// </summary>
		protected virtual Encoding GetEncoding()
		{
			return Encoding.UTF8;
		}
	}
}