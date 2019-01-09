namespace Sitecore.Support.Analytics.Pipelines.StartAnalytics
{
  using Sitecore.Analytics;
  using Sitecore.Analytics.Data.Dictionaries;
  using Sitecore.Analytics.Data.Dictionaries.DictionaryData;
  using Sitecore.Analytics.DataAccess;
  using Sitecore.Caching.Generics;
  using Sitecore.Configuration;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using System;
  using System.Reflection;

  public class ChangeCacheSize
  {

    private static PropertyInfo UserAgentCacheProperty;
    private static PropertyInfo DeviceCacheProperty;

    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      UserAgentsDictionary userAgentsDic = Tracker.Dictionaries.BrowserUserAgents;
      DeviceDictionary deviceDictionary = Tracker.Dictionaries.Devices;

      if (userAgentsDic == null)
      {
        Log.Info("Support: BrowserUserAgents dictionary doesn't exist!", this);
      }
      if (deviceDictionary == null)
      {
        Log.Info("Support: device dictionary doesn't exist!", this);
      }
      else
      {
        this.ResetCache(userAgentsDic, deviceDictionary);
      }
    }

    private void ResetCache(UserAgentsDictionary userAgDic, DeviceDictionary devDic)
    {
      if (((UserAgentCacheProperty == null) || (DeviceCacheProperty == null)))
      {
        UserAgentCacheProperty = userAgDic.GetType().BaseType.GetProperty("Cache", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        DeviceCacheProperty = devDic.GetType().BaseType.GetProperty("Cache", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        Cache<Guid> UserAgentCache = (Cache<Guid>)UserAgentCacheProperty.GetValue(userAgDic);
        Cache<Guid> DeviceCache = (Cache<Guid>)DeviceCacheProperty.GetValue(devDic);

        UserAgentCache.MaxSize = Settings.GetIntSetting("UserAgentDictionaryCacheSize", 0xf4240);
        DeviceCache.MaxSize = Settings.GetIntSetting("DeviceDictionaryCacheSize", 0xf4240);

        Log.Info($"Size of UserAgentDictionaryCache was changed. UserAgentDictionary.Cache.MaxSize={UserAgentCache.MaxSize}", this);
        Log.Info($"Size of DeviceDictionaryCache was changed.  DeviceDictionary.Cache.MaxSize = {DeviceCache.MaxSize} ", this);
        Log.Info("Invoke finished! ", this);

      }
    }
  }
}
