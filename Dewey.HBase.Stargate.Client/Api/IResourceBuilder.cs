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

using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.HBase.Stargate.Client.Api
{
	/// <summary>
	///    Defines a URI builder for HBase resources.
	/// </summary>
	public interface IResourceBuilder
	{
		/// <summary>
		///    Builds a cell or row query URI.
		/// </summary>
		/// <param name="query"></param>
		string BuildCellOrRowQuery(CellQuery query);

		/// <summary>
		///    Builds a single value storage URI.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <param name="forReading">
		///    if set to <c>true</c> this resource will be used for reading.
		/// </param>
		string BuildSingleValueAccess(Identifier identifier, bool forReading = false);

		/// <summary>
		///    Builds a delete-item URI.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		string BuildDeleteItem(Identifier identifier);

		/// <summary>
		///    Builds a batch insert URI.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		string BuildBatchInsert(Identifier identifier);

		/// <summary>
		///    Builds a table creation URI.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		string BuildTableSchemaAccess(TableSchema tableSchema);

		/// <summary>
		/// Builds a scanner creation URI.
		/// </summary>
		/// <param name="scannerOptions">Name of the table.</param>
		string BuildScannerCreate(ScannerOptions scannerOptions);
	}
}