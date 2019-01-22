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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dewey.HBase.Stargate.Client.Models;
using RestSharp;

namespace Dewey.HBase.Stargate.Client.Api
{
	/// <summary>
	///    Provides Stargate API extensions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		///    Writes the value to HBase using the identifier values.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="value">The value.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
		[Obsolete("Use Task.Run(() => gate.WriteValue(value,table,row,column,qualifier)) instead")]
		public static Task WriteValueAsync(this IStargate gate, string value, string table, string row, string column, string qualifier = null,
			long? timestamp = null)
		{
			return gate.WriteValueAsync(BuildIdentifier(table, row, column, qualifier, timestamp), value);
		}

		/// <summary>
		///    Writes the value to HBase using the identifier values.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="value">The value.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
		public static void WriteValue(this IStargate gate, string value, string table, string row, string column, string qualifier = null,
			long? timestamp = null)
		{
			gate.WriteValue(BuildIdentifier(table, row, column, qualifier, timestamp), value);
		}

		/// <summary>
		///    Deletes the item with matching values from HBase.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
    [Obsolete("Use Task.Run(() => gate.DeleteItem(table,row,column,qualifier,timestamp)) instead")]
    public static Task DeleteItemAsync(this IStargate gate, string table, string row, string column = null, string qualifier = null,
			long? timestamp = null)
		{
			return gate.DeleteItemAsync(BuildIdentifier(table, row, column, qualifier, timestamp));
		}

		/// <summary>
		///    Deletes the item with matching values from HBase.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
		public static void DeleteItem(this IStargate gate, string table, string row, string column = null, string qualifier = null, long? timestamp = null)
		{
			gate.DeleteItem(BuildIdentifier(table, row, column, qualifier, timestamp));
		}

		/// <summary>
		///    Reads the value at the matching location.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
    [Obsolete("Use Task.Run(() => gate.ReadValue(table,row,column,qualifier,timestamp)) instead")]
    public static Task<string> ReadValueAsync(this IStargate gate, string table, string row, string column, string qualifier = null,
			long? timestamp = null)
		{
			return gate.ReadValueAsync(BuildIdentifier(table, row, column, qualifier, timestamp));
		}

		/// <summary>
		///    Reads the value at the matching location.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
		public static string ReadValue(this IStargate gate, string table, string row, string column, string qualifier = null, long? timestamp = null)
		{
			return gate.ReadValue(BuildIdentifier(table, row, column, qualifier, timestamp));
		}

		/// <summary>
		/// Finds the cells with the matching attributes.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="beginTimestamp">The begin timestamp (inclusive).</param>
		/// <param name="endTimestamp">The end timestamp (exclusive).</param>
		/// <param name="maxVersions">The maximum number of versions to return.</param>
    [Obsolete("Use Task.Run(() => gate.FindCells(table,row,column,qualifier,beginTimestamp,endTimestamp,maxVersions)) instead")]
    public static Task<CellSet> FindCellsAsync(this IStargate gate, string table, string row = null, string column = null, string qualifier = null,
			long? beginTimestamp = null, long? endTimestamp = null, int? maxVersions = null)
		{
			return gate.FindCellsAsync(BuildQuery(table, row, column, qualifier, beginTimestamp, endTimestamp, maxVersions));
		}

		/// <summary>
		/// Finds the cells with the matching attributes.
		/// </summary>
		/// <param name="gate">The gate.</param>
		/// <param name="table">The table.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="beginTimestamp">The begin timestamp (exclusive).</param>
		/// <param name="endTimestamp">The end timestamp (exclusive).</param>
		/// <param name="maxVersions">The maximum versions to return.</param>
		public static CellSet FindCells(this IStargate gate, string table, string row = null, string column = null, string qualifier = null,
			long? beginTimestamp = null, long? endTimestamp = null, int? maxVersions = null)
		{
			return gate.FindCells(BuildQuery(table, row, column, qualifier, beginTimestamp, endTimestamp, maxVersions));
		}

		/// <summary>
		///    Throws an exception for the response if none of the statuses match.
		/// </summary>
		/// <param name="errorProvider">The error provider.</param>
		/// <param name="response">The response.</param>
		/// <param name="validStatuses">The valid statuses.</param>
		public static void ThrowIfStatusMismatch(this IErrorProvider errorProvider, IRestResponse response, params HttpStatusCode[] validStatuses)
		{
			bool mismatch = validStatuses.All(status => response.StatusCode != status);
			if (mismatch)
			{
				errorProvider.ThrowFromResponse(response);
			}
		}

		/// <summary>
		/// Executes the request.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="request">The request.</param>
		public static Task<IRestResponse> ExecuteAsync(this IRestClient client, IRestRequest request)
		{
			var completionSource = new TaskCompletionSource<IRestResponse>();
			client.ExecuteAsync(request, completionSource.SetResult);
			return completionSource.Task;
		}

		private static Identifier BuildIdentifier(string table, string row, string column, string qualifier, long? timestamp)
		{
			return new Identifier
			{
				Table = table,
				Row = row,
				CellDescriptor = new HBaseCellDescriptor
				{
					Column = column,
					Qualifier = qualifier
				},
				Timestamp = timestamp
			};
		}

		private static CellQuery BuildQuery(string table, string row, string column, string qualifier, long? beginTimestamp, long? endTimestamp, int? maxVersions)
		{
			return new CellQuery
			{
				Table = table,
				Row = row,
				Cells = new[]
				{
					new HBaseCellDescriptor
					{
						Column = column,
						Qualifier = qualifier
					}
				},
				BeginTimestamp = beginTimestamp,
				EndTimestamp = endTimestamp,
				MaxVersions = maxVersions
			};
		}
	}
}