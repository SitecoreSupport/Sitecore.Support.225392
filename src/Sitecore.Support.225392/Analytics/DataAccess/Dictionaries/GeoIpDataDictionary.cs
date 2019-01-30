using System;
using Sitecore.Analytics.DataAccess;

namespace Sitecore.Support.Analytics.DataAccess.Dictionaries
{
  public class GeoIpDataDictionary : Sitecore.Analytics.DataAccess.Dictionaries.GeoIpDataDictionary
  {
    public GeoIpDataDictionary(DictionaryBase dictionary) : base(dictionary)
    {
      Cache.MaxSize = Sitecore.Configuration.Settings.GetIntSetting("GeoIpDataDictionaryCacheSize", 100000000);
    }

    public override TimeSpan CacheExpirationTimeout => TimeSpan.FromSeconds(Sitecore.Configuration.Settings.GetDoubleSetting("GeoIpDataDictionaryCacheExpiration", 1800));
  }
}