namespace Dewey.HBase.Stargate.Client.Api
{
  /// <summary>
  ///   A Filter that checks a single column value, but does not emit the tested column.
  ///   This will enable a performance boost over <see cref="SingleColumnValueFilter" />,
  ///   if the tested column value is not actually needed as input (besides for the filtering itself).
  /// </summary>
  public class SingleColumnValueExcludeFilter : SingleColumnValueFilter
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="SingleColumnValueExcludeFilter" /> class.
    /// </summary>
    /// <param name="family">The family.</param>
    /// <param name="qualifier">The qualifier.</param>
    /// <param name="value">The column.</param>
    /// <param name="comparison">The comparison.</param>
    /// <param name="latestVersion">
    ///   if set to <c>true</c>, only return the latest version.
    /// </param>
    public SingleColumnValueExcludeFilter(string family, string qualifier, string value, FilterComparisons comparison, bool latestVersion = true)
      : base(family, qualifier, value, comparison, latestVersion) {}
  }
}