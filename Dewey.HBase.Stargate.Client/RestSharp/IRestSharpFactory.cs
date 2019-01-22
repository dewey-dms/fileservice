using RestSharp;

namespace Dewey.HBase.Stargate.Client.RestSharp
{
    public interface IRestSharpFactory
    {
        /// <summary>
        ///    Creates the client.
        /// </summary>
        /// <param name="url">The URL.</param>
        IRestClient CreateClient(string url);

        /// <summary>
        ///    Creates the request.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        IRestRequest CreateRequest(string resource, Method method);
    }
}