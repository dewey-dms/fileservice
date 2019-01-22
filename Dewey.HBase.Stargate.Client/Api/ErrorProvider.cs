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
using System.Net;
using System.Net.Mime;
using System.Xml;
using System.Xml.Linq;
using Dewey.HBase.Stargate.Client.Models;
using Dewey.HBase.Stargate.Client.Properties;
using RestSharp;

namespace Dewey.HBase.Stargate.Client.Api
{
	/// <summary>
	/// Provides a default <see cref="IErrorProvider"/>.
	/// </summary>
	public class ErrorProvider : IErrorProvider
	{
		private readonly IDictionary<HttpStatusCode, Func<IRestResponse, Exception>> _cannedErrors
			= new Dictionary<HttpStatusCode, Func<IRestResponse, Exception>>
			{
				{HttpStatusCode.BadRequest, response => new InvalidOperationException()},
				{HttpStatusCode.NotFound, response => new KeyNotFoundException()},
				{HttpStatusCode.InternalServerError, response => new ApplicationException(GetResponseContent(response))}
			};

		/// <summary>
		/// Creates an exception from the response.
		/// </summary>
		/// <param name="response">The response.</param>
		public Exception CreateFromResponse(IRestResponse response)
		{
			return !_cannedErrors.ContainsKey(response.StatusCode)
				? null
				: _cannedErrors[response.StatusCode](response);
		}

		/// <summary>
		/// Throws an exception from the response.
		/// </summary>
		/// <param name="response">The response.</param>
		public void ThrowFromResponse(IRestResponse response)
		{
			Exception error = CreateFromResponse(response);
			if (error != null) throw error;
		}

		/// <summary>
		/// Throws an exception if the schema is invalid.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		public void ThrowIfSchemaInvalid(TableSchema tableSchema)
		{
			if(tableSchema.Columns.Any(column => string.IsNullOrEmpty(column.Name))) throw new ArgumentException(Resources.ErrorProvider_ColumnNameMissing);
		}

		private static string GetResponseContent(IRestResponse response)
		{
			try
			{
				if (response.ContentType == MediaTypeNames.Text.Html)
				{
					var markup = XDocument.Parse(response.Content);
					return string.Join(" ", markup.DescendantNodes().Where(node => node.NodeType == XmlNodeType.Text).Select(node => ((XText) node).Value));
				}

				return GetRawResponseContent(response);
			}
			catch
			{
				return GetRawResponseContent(response);
			}
		}

		private static string GetRawResponseContent(IRestResponse response)
		{
			return string.IsNullOrEmpty(response.Content) ? response.StatusDescription : response.Content;
		}
	}
}