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
    public static DictionaryBase GetDictionaryData()
    {
      GetDictionaryDataPipelineArgs getDictionaryDataPipelineArgs = new GetDictionaryDataPipelineArgs();
      GetDictionaryDataPipeline.Run(getDictionaryDataPipelineArgs);
      return Assert.ResultNotNull<DictionaryBase>(getDictionaryDataPipelineArgs.Result, "Check configuration, 'getDictionaryDataStorage' pipeline  must set args.Result property with instance of DictionaryBase type.");
    }

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
      PropertyInfo userAgCacheProperty = userAgDic.GetType().BaseType.GetProperty("Cache", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
      PropertyInfo devCacheProperty = devDic.GetType().BaseType.GetProperty("Cache", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

      Cache<Guid> userAgCache = (Cache<Guid>)userAgCacheProperty.GetValue(userAgDic);
      Cache<Guid> devCache = (Cache<Guid>)devCacheProperty.GetValue(devDic);

      userAgCache.MaxSize = Settings.GetIntSetting("UserAgentDictionaryCacheSize", 0xf4240);
      devCache.MaxSize = Settings.GetIntSetting("DeviceDictionaryCacheSize", 0xf4240);
      // Sitecore.Support.22395 8.2.7.1
      Log.Debug($"Size of UserAgentDictionaryCache was changed. UserAgentDictionary.Cache.MaxSize={userAgCache.MaxSize}", this);
      Log.Debug($"Size of DeviceDictionaryCache was changed.  DeviceDictionary.Cache.MaxSize = {devCache.MaxSize} ", this);
      Log.Debug("Invoke finished! ", this);
    }
  }
}
