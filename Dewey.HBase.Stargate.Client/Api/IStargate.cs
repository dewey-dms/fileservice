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
using System.Threading.Tasks;
using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.HBase.Stargate.Client.Api
{
	/// <summary>
	///    Provides operations against a Stargate server.
	/// </summary>
	public interface IStargate
	{
		/// <summary>
		///    Writes the value to HBase using the identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <param name="value">The value.</param>
		Task WriteValueAsync(Identifier identifier, string value);

		/// <summary>
		///    Writes the value to HBase using the identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <param name="value">The value.</param>
		void WriteValue(Identifier identifier, string value);

		/// <summary>
		///    Writes the cells to HBase.
		/// </summary>
		/// <param name="cells">The cells.</param>
		Task WriteCellsAsync(CellSet cells);

		/// <summary>
		///    Writes the cells to HBase.
		/// </summary>
		/// <param name="cells">The cells.</param>
		void WriteCells(CellSet cells);

		/// <summary>
		///    Deletes the item with the matching identifier from HBase.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		Task DeleteItemAsync(Identifier identifier);

		/// <summary>
		///    Deletes the item with the matching identifier from HBase.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		void DeleteItem(Identifier identifier);

		/// <summary>
		///    Reads the value with the matching identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		Task<string> ReadValueAsync(Identifier identifier);

		/// <summary>
		///    Reads the value with the matching identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		string ReadValue(Identifier identifier);

		/// <summary>
		///    Finds the cells matching the query.
		/// </summary>
		/// <param name="query"></param>
		Task<CellSet> FindCellsAsync(CellQuery query);

		/// <summary>
		///    Finds the cells matching the query.
		/// </summary>
		/// <param name="query"></param>
		CellSet FindCells(CellQuery query);

		/// <summary>
		///    Creates the table.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		void CreateTable(TableSchema tableSchema);

		/// <summary>
		///    Creates the table.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		Task CreateTableAsync(TableSchema tableSchema);

		/// <summary>
		///    Gets the table names.
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetTableNames();

		/// <summary>
		///    Gets the table names.
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<string>> GetTableNamesAsync();

		/// <summary>
		/// Deletes the table.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		void DeleteTable(string tableName);

		/// <summary>
		/// Deletes the table.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		Task DeleteTableAsync(string tableName);

		/// <summary>
		/// Gets the table schema.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		Task<TableSchema> GetTableSchemaAsync(string tableName);

		/// <summary>
		/// Gets the table schema.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		TableSchema GetTableSchema(string tableName);

		/// <summary>
		/// Creates the scanner.
		/// </summary>
		/// <param name="options">The options.</param>
		Task<IScanner> CreateScannerAsync(ScannerOptions options);

		/// <summary>
		/// Creates the scanner.
		/// </summary>
		/// <param name="options">The options.</param>
		IScanner CreateScanner(ScannerOptions options);

		/// <summary>
		/// Deletes the scanner.
		/// </summary>
		/// <param name="scanner">The scanner.</param>
		void DeleteScanner(IScanner scanner);

		/// <summary>
		/// Deletes the scanner.
		/// </summary>
		/// <param name="scanner">The scanner.</param>
		Task DeleteScannerAsync(IScanner scanner);

		/// <summary>
		/// Gets the scanner result.
		/// </summary>
		/// <param name="scanner">The scanner.</param>
		CellSet GetScannerResult(IScanner scanner);

		/// <summary>
		/// Gets the scanner result.
		/// </summary>
		/// <param name="scanner">The scanner.</param>
		Task<CellSet> GetScannerResultAsync(IScanner scanner);
	}
}