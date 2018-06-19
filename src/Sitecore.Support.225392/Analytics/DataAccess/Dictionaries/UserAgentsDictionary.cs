using System;
using Sitecore.Analytics.DataAccess.Diagnostics.PerformanceCounters;
using Sitecore.Analytics.Model;
using Sitecore.Framework.Conditions;
using Sitecore.StringExtensions;
using Sitecore.Analytics.DataAccess;
using Sitecore.Analytics;
using Sitecore.Analytics.DataAccess.Dictionaries;
using Sitecore.Caching.Generics;

namespace Sitecore.Support.Analytics.DataAccess.Dictionaries
{
  public class UserAgentsDictionary : Sitecore.Analytics.DataAccess.Dictionaries.UserAgentsDictionary
  {
    public UserAgentsDictionary(DictionaryBase dictionary) : base(dictionary)
    {
      Cache.MaxSize = Sitecore.Configuration.Settings.GetIntSetting("UserAgentDictionaryCacheSize", 100000000);
    }

    public override TimeSpan CacheExpirationTimeout => TimeSpan.FromSeconds(Sitecore.Configuration.Settings.GetDoubleSetting("UserAgentDictionaryCacheExpiration", 1800));
  }
}
