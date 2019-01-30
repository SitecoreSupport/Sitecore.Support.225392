using System;
using Sitecore.Analytics.DataAccess;

namespace Sitecore.Support.Analytics.DataAccess.Dictionaries
{
  public class ReferringSitesDictionary : Sitecore.Analytics.DataAccess.Dictionaries.ReferringSitesDictionary
  {
    public ReferringSitesDictionary(DictionaryBase dictionary) : base(dictionary)
    {
      Cache.MaxSize = Sitecore.Configuration.Settings.GetIntSetting("ReferringSitesDictionaryCacheSize", 100000000);
    }

    public override TimeSpan CacheExpirationTimeout => TimeSpan.FromSeconds(Sitecore.Configuration.Settings.GetDoubleSetting("ReferringSitesDictionaryCacheExpiration", 1800));
  }
}