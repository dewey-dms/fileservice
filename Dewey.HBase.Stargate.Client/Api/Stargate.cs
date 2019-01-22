#region FreeBSD

// Copyright (c) 2014, The Tribe
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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dewey.HBase.Stargate.Client.Models;
using Dewey.HBase.Stargate.Client.RestSharp;
using Dewey.HBase.Stargate.Client.TypeConversion;
using RestSharp;
//using RestSharp.Injection;

namespace Dewey.HBase.Stargate.Client.Api
{
  /// <summary>
  ///   Provides a default implementation <see cref="IStargate" />.
  /// </summary>
  public class Stargate : IStargate
  {
    /// <summary>
    ///   The default false row key.
    /// </summary>
    public const string DefaultFalseRowKey = "row";

    /// <summary>
    ///   The default content type.
    /// </summary>
    public const string DefaultContentType = HBaseMimeTypes.Xml;

    /// <summary>
    ///   The REST client.
    /// </summary>
    protected readonly IRestClient Client;

    /// <summary>
    ///   The MIME converter.
    /// </summary>
    protected readonly IMimeConverter Converter;

    /// <summary>
    ///   The error provider.
    /// </summary>
    protected readonly IErrorProvider ErrorProvider;

    /// <summary>
    ///   The resource builder.
    /// </summary>
    protected readonly IResourceBuilder ResourceBuilder;

    /// <summary>
    ///   The RestSharp factory.
    /// </summary>
    protected readonly IRestSharpFactory RestSharp;

    /// <summary>
    ///   The scanner converter.
    /// </summary>
    protected readonly IScannerOptionsConverter ScannerConverter;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Stargate" /> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="resourceBuilderFactory">The resource builder factory.</param>
    /// <param name="restSharp">The RestSharp factory.</param>
    /// <param name="converterFactory">The converter factory.</param>
    /// <param name="errorProvider">The error provider.</param>
    /// <param name="scannerConverter">The scanner converter.</param>
    public Stargate(IStargateOptions options, Func<IStargateOptions, IResourceBuilder> resourceBuilderFactory, IRestSharpFactory restSharp,
      IMimeConverterFactory converterFactory, IErrorProvider errorProvider, IScannerOptionsConverter scannerConverter)
    {
      ResourceBuilder = resourceBuilderFactory(options);
      RestSharp = restSharp;
      ErrorProvider = errorProvider;
      ScannerConverter = scannerConverter;
      Client = RestSharp.CreateClient(options.ServerUrl);
      options.ContentType = string.IsNullOrEmpty(options.ContentType) ? DefaultContentType : options.ContentType;
      Converter = converterFactory.CreateConverter(options.ContentType);
      options.FalseRowKey = string.IsNullOrEmpty(options.FalseRowKey) ? DefaultFalseRowKey : options.FalseRowKey;
      Options = options;
    }

    /// <summary>
    ///   Gets the options.
    /// </summary>
    /// <value>
    ///   The options.
    /// </value>
    protected virtual IStargateOptions Options { get; private set; }


    /// <summary>
    ///   Writes the value to HBase using the identifier.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <param name="value">The value.</param>
    public virtual Task WriteValueAsync(Identifier identifier, string value)
    {
      return WriteValueInternal(identifier, value, SendRequestAsync);
    }

    /// <summary>
    ///   Writes the value to HBase using the identifier.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <param name="value">The value.</param>
    public virtual void WriteValue(Identifier identifier, string value)
    {
      WriteValueInternal(identifier, value, SendRequest);
    }

    T WriteValueInternal<T>(Identifier identifier, string value, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string contentType = Options.ContentType;
      string resource = ResourceBuilder.BuildSingleValueAccess(identifier);
      string content = Converter.ConvertCell(new Cell(identifier, value));

      return action(Method.POST, resource, contentType, contentType, content, new[]{HttpStatusCode.OK});
    }

    /// <summary>
    ///   Writes the cells to HBase.
    /// </summary>
    /// <param name="cells">The cells.</param>
    public virtual Task WriteCellsAsync(CellSet cells)
    {
      return WriteCellsInternal(cells, SendRequestAsync);
    }

    /// <summary>
    ///   Writes the cells to HBase.
    /// </summary>
    /// <param name="cells">The cells.</param>
    public virtual void WriteCells(CellSet cells)
    {
      WriteCellsInternal(cells, SendRequest);
    }

    T WriteCellsInternal<T>(CellSet cells, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string contentType = Options.ContentType;
      var tableIdentifier = new Identifier { Table = cells.Table };
      string resource = ResourceBuilder.BuildBatchInsert(tableIdentifier);
      
      return action(Method.POST, resource, contentType, contentType, Converter.ConvertCells(cells), new[] { HttpStatusCode.OK });
    }

    /// <summary>
    ///   Deletes the item with the matching identifier from HBase.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    public virtual Task DeleteItemAsync(Identifier identifier)
    {
      return DeleteItemInternal(identifier, SendRequestAsync);
    }

    /// <summary>
    ///   Deletes the item with the matching identifier from HBase.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    public virtual void DeleteItem(Identifier identifier)
    {
      DeleteItemInternal(identifier, SendRequest);
    }

    T DeleteItemInternal<T>(Identifier identifier, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = ResourceBuilder.BuildDeleteItem(identifier);

      return action(Method.DELETE, resource, Options.ContentType, null, null, new[] { HttpStatusCode.OK });
    }

    /// <summary>
    ///   Reads the value with the matching identifier.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    public virtual async Task<string> ReadValueAsync(Identifier identifier)
    {
      return ProcessReadValueResponse(await ReadValueInternal(identifier, SendRequestAsync), identifier);
    }

    /// <summary>
    ///   Reads the value with the matching identifier.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    public virtual string ReadValue(Identifier identifier)
    {
      return ProcessReadValueResponse(ReadValueInternal(identifier, SendRequest), identifier);
    }

    T ReadValueInternal<T>(Identifier identifier, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = identifier.Timestamp.HasValue
        ? ResourceBuilder.BuildCellOrRowQuery(identifier.ToQuery())
        : ResourceBuilder.BuildSingleValueAccess(identifier, true);

      return action(Method.GET, resource, Options.ContentType, null, null, new[] { HttpStatusCode.OK, HttpStatusCode.NotFound });
    }

    string ProcessReadValueResponse(IRestResponse response, Identifier identifier)
    {
      return response.StatusCode == HttpStatusCode.OK
        ? Converter.ConvertCells(response.Content, identifier.Table).Select(cell => cell.Value).FirstOrDefault()
        : null;
    }

    /// <summary>
    ///   Finds the cells matching the query.
    /// </summary>
    /// <param name="query"></param>
    public virtual async Task<CellSet> FindCellsAsync(CellQuery query)
    {
      return ProcessFindCellResponse(await FindCellsInternal(query, SendRequestAsync), query);
    }

    /// <summary>
    ///   Finds the cells matching the query.
    /// </summary>
    /// <param name="query"></param>
    public virtual CellSet FindCells(CellQuery query)
    {
      return ProcessFindCellResponse(FindCellsInternal(query, SendRequest), query);
    }

    T FindCellsInternal<T>(CellQuery query, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = ResourceBuilder.BuildCellOrRowQuery(query);

      return action(Method.GET, resource, Options.ContentType, null, null, new[] { HttpStatusCode.OK, HttpStatusCode.NotFound });
    }

    CellSet ProcessFindCellResponse(IRestResponse response, CellQuery query)
    {
      var set = new CellSet
      {
        Table = query.Table
      };

      if (response.StatusCode == HttpStatusCode.OK)
      {
        set.AddRange(Converter.ConvertCells(response.Content, query.Table));
      }

      return set;
    }

    /// <summary>
    ///   Creates the table.
    /// </summary>
    /// <param name="tableSchema">The table schema.</param>
    public virtual void CreateTable(TableSchema tableSchema)
    {
      CreateTableInternal(tableSchema, SendRequest);
    }

    /// <summary>
    ///   Creates the table.
    /// </summary>
    /// <param name="tableSchema">The table schema.</param>
    public virtual Task CreateTableAsync(TableSchema tableSchema)
    {
      return CreateTableInternal(tableSchema, SendRequestAsync);
    }

    T CreateTableInternal<T>(TableSchema tableSchema, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = ResourceBuilder.BuildTableSchemaAccess(tableSchema);
      ErrorProvider.ThrowIfSchemaInvalid(tableSchema);
      string data = Converter.ConvertSchema(tableSchema);

      return action(Method.PUT, resource, Options.ContentType, Options.ContentType, data, new[] { HttpStatusCode.OK });
    }

    /// <summary>
    ///   Gets the table names.
    /// </summary>
    public virtual IEnumerable<string> GetTableNames()
    {
      IRestResponse response = SendRequest(Method.GET, string.Empty, HBaseMimeTypes.Text);

      return ParseLines(response.Content);
    }

    /// <summary>
    ///   Gets the table names.
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IEnumerable<string>> GetTableNamesAsync()
    {
      IRestResponse response = await SendRequestAsync(Method.GET, string.Empty, HBaseMimeTypes.Text);

      return ParseLines(response.Content);
    }

    /// <summary>
    ///   Deletes the table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public virtual void DeleteTable(string tableName)
    {
      DeleteTableInternal(tableName, SendRequest);
    }

    /// <summary>
    ///   Deletes the table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public virtual Task DeleteTableAsync(string tableName)
    {
      return DeleteTableInternal(tableName, SendRequestAsync);
    }

    T DeleteTableInternal<T>(string tableName, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = ResourceBuilder.BuildTableSchemaAccess(new TableSchema { Name = tableName });

      return action(Method.DELETE, resource, Options.ContentType, null, null, new[] { HttpStatusCode.OK });
    }

    /// <summary>
    ///   Gets the table schema async.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public virtual async Task<TableSchema> GetTableSchemaAsync(string tableName)
    {
      return ProcessGetTableSchemaResponse(await GetTableSchemaInternal(tableName, SendRequestAsync));
    }

    /// <summary>
    ///   Gets the table schema.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public virtual TableSchema GetTableSchema(string tableName)
    {
      return ProcessGetTableSchemaResponse(GetTableSchemaInternal(tableName, SendRequest));
    }

    T GetTableSchemaInternal<T>(string tableName, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = ResourceBuilder.BuildTableSchemaAccess(new TableSchema { Name = tableName });

      return action(Method.GET, resource, Options.ContentType, null, null, new[] { HttpStatusCode.OK });
    }

    TableSchema ProcessGetTableSchemaResponse(IRestResponse response)
    {
      return Converter.ConvertSchema(response.Content);
    }

    /// <summary>
    ///   Creates the scanner.
    /// </summary>
    /// <param name="options">The options.</param>
    public virtual async Task<IScanner> CreateScannerAsync(ScannerOptions options)
    {
      return ProcessCreateScannerResponse(await CreateScannerInternal(options, SendRequestAsync), options);
    }

    /// <summary>
    ///   Creates the scanner.
    /// </summary>
    /// <param name="options">The options.</param>
    public virtual IScanner CreateScanner(ScannerOptions options)
    {
      return ProcessCreateScannerResponse(CreateScannerInternal(options, SendRequest), options);
    }

    T CreateScannerInternal<T>(ScannerOptions options, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      string resource = ResourceBuilder.BuildScannerCreate(options);

      return action(Method.PUT, resource, HBaseMimeTypes.Xml, null, ScannerConverter.Convert(options), new[] { HttpStatusCode.Created });
    }

    IScanner ProcessCreateScannerResponse(IRestResponse response, ScannerOptions options)
    {
      string scannerLocation =
        response.Headers.Where(header => header.Type == ParameterType.HttpHeader && header.Name == RestConstants.LocationHeader)
          .Select(header => header.Value.ToString())
          .FirstOrDefault();

      return string.IsNullOrEmpty(scannerLocation) ? null : new Scanner(options.TableName, new Uri(scannerLocation).PathAndQuery.Trim('/'), this);
    }

    /// <summary>
    ///   Deletes the scanner.
    /// </summary>
    /// <param name="scanner">The scanner.</param>
    public virtual void DeleteScanner(IScanner scanner)
    {
      SendRequest(Method.DELETE, scanner.Resource, Options.ContentType);
    }

    /// <summary>
    ///   Deletes the scanner.
    /// </summary>
    /// <param name="scanner">The scanner.</param>
    public virtual Task DeleteScannerAsync(IScanner scanner)
    {
      return SendRequestAsync(Method.DELETE, scanner.Resource, Options.ContentType);
    }

    /// <summary>
    ///   Gets the scanner result.
    /// </summary>
    /// <param name="scanner">The scanner.</param>
    public virtual CellSet GetScannerResult(IScanner scanner)
    {
      return ProcessScannerResultResponse(GetScannerResultInternal(scanner, SendRequest), scanner);
    }

    /// <summary>
    ///   Gets the scanner result.
    /// </summary>
    /// <param name="scanner">The scanner.</param>
    public virtual async Task<CellSet> GetScannerResultAsync(IScanner scanner)
    {
      return ProcessScannerResultResponse(await GetScannerResultInternal(scanner, SendRequestAsync), scanner);
    }

    T GetScannerResultInternal<T>(IScanner scanner, Func<Method, string, string, string, string, HttpStatusCode[], T> action)
    {
      return action(Method.GET, scanner.Resource, Options.ContentType, null, null, new[] { HttpStatusCode.OK, HttpStatusCode.NoContent });
    }

    CellSet ProcessScannerResultResponse(IRestResponse response, IScanner scanner)
    {
      return response.StatusCode == HttpStatusCode.NoContent ? null : new CellSet(Converter.ConvertCells(response.Content, scanner.Table));
    }

    /// <summary>
    ///   Creates a new stargate with the specified options.
    /// </summary>
    /// <param name="serverUrl">The server URL.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="falseRowKey">The false row key.</param>
    public static IStargate Create(string serverUrl, string contentType = DefaultContentType, string falseRowKey = DefaultFalseRowKey)
    {
      return Create(new StargateOptions { ServerUrl = serverUrl, ContentType = contentType, FalseRowKey = falseRowKey });
    }

    /// <summary>
    ///   Creates a new stargate with the specified options.
    /// </summary>
    /// <param name="options">The options.</param>
    public static IStargate Create(IStargateOptions options)
    {
      Func<IStargateOptions, IResourceBuilder> resourceBuilderFactory = opt => new ResourceBuilder(opt);
      var restSharp = new RestSharpFactory(url => new RestClient(url), (resource, method) => new RestRequest(resource, method));
      var codec = new Base64Codec();
      var mimeConverters = new MimeConverterFactory(new[]
      {
        new XmlMimeConverter(new SimpleValueConverter(), codec)
      });
      var errors = new ErrorProvider();
      var scannerConverter = new ScannerOptionsConverter(codec);

      options.ContentType = string.IsNullOrEmpty(options.ContentType)
        ? DefaultContentType
        : options.ContentType;

      options.FalseRowKey = string.IsNullOrEmpty(options.FalseRowKey)
        ? DefaultFalseRowKey
        : options.FalseRowKey;

      return new Stargate(options, resourceBuilderFactory, restSharp, mimeConverters, errors, scannerConverter);
    }

    /// <summary>
    ///   Sends the request.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="acceptType">Type of the accept.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="content">The content.</param>
    /// <param name="validStatuses">Acceptable Http status codes.</param>
    protected virtual async Task<IRestResponse> SendRequestAsync(Method method, string resource, string acceptType,
      string contentType = null, string content = null, HttpStatusCode[] validStatuses = null)
    {
      IRestRequest request = BuildRequest(method, resource, acceptType, contentType, content);

      return GetValidatedResponse(await Client.ExecuteTaskAsync(request), validStatuses);
    }

    IRestResponse GetValidatedResponse(IRestResponse response, HttpStatusCode[] validStatuses)
    {
      if (response.ResponseStatus == ResponseStatus.Error && response.ErrorException != null)
      {
        throw response.ErrorException;
      }

      if (validStatuses == null)
        ErrorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
      else
        ErrorProvider.ThrowIfStatusMismatch(response, validStatuses);

      return response;
    }

    /// <summary>
    ///   Sends the request.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="acceptType">Type of the accept.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="content">The content.</param>
    /// <param name="validStatuses">Acceptable Http status codes.</param>
    protected virtual IRestResponse SendRequest(Method method, string resource, string acceptType,
      string contentType = null, string content = null, HttpStatusCode[] validStatuses = null)
    {
      IRestRequest request = BuildRequest(method, resource, acceptType, contentType, content);

      IRestResponse response = Client.Execute(request);

      return GetValidatedResponse(response, validStatuses);
    }

    /// <summary>
    ///   Builds the request.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="acceptType">Type of the accept.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="content">The content.</param>
    protected IRestRequest BuildRequest(Method method, string resource, string acceptType, string contentType, string content)
    {
      IRestRequest request = RestSharp.CreateRequest(resource, method)
        .AddHeader(HttpRequestHeader.Accept.ToString(), acceptType);

      if (!string.IsNullOrEmpty(content))
      {
        contentType = string.IsNullOrEmpty(contentType) ? acceptType : contentType;
        request.AddParameter(contentType, content, ParameterType.RequestBody);
      }
      return request;
    }

    //TODO: get rid of this (it's been written in 5 different places, so maybe it's time for code-patterns)
    private static IEnumerable<string> ParseLines(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        yield break;
      }

      using (var reader = new StringReader(text))
      {
        string line;
        while ((line = reader.ReadLine()) != null) yield return line;
      }
    }
  }
}