namespace Sitecore.Support.Analytics.Pipelines.StartAnalytics
{

  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using Sitecore.Analytics;
  using Sitecore.Analytics.Lookups;
  using Sitecore.Analytics.DataAccess;
  using Sitecore.Analytics.Data.Dictionaries;
  using Sitecore.Analytics.Data.Dictionaries.DictionaryData;
  using Sitecore.Configuration;
  using System.Reflection;
  using System;
  public class ChangeCacheSize
  {
    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      LocationsDictionary locationDic = Tracker.Dictionaries.Locations;
      GeoIpDataDictionary geoIpData = Tracker.Dictionaries.GeoIPs;
      DeviceDictionary deviceDictionary = Tracker.Dictionaries.Devices;

      if (locationDic == null)
        Log.Info("Support: location dictionary doesn't exist!", this);
      if (geoIpData == null)
        Log.Info("Support: geoIpData dictionary doesn't exist!", this);
      if (deviceDictionary == null)
        Log.Info("Support: device dictionary doesn't exist!", this);
      else
      {
        ResetCache(locationDic,geoIpData,deviceDictionary);//reset cache based on config settings node
      }
    }

    public static DictionaryBase GetDictionaryData()
    {
      GetDictionaryDataPipelineArgs getDictionaryDataPipelineArgs = new GetDictionaryDataPipelineArgs();
      GetDictionaryDataPipeline.Run(getDictionaryDataPipelineArgs);
      return Assert.ResultNotNull<DictionaryBase>(getDictionaryDataPipelineArgs.Result, "Check configuration, 'getDictionaryDataStorage' pipeline  must set args.Result property with instance of DictionaryBase type.");
    }

    private void ResetCache(LocationsDictionary locDic,GeoIpDataDictionary geoDic, DeviceDictionary devDic)
    {
      PropertyInfo locCacheProperty = locDic.GetType().BaseType.GetProperty("Cache", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
      PropertyInfo geoCacheProperty = geoDic.GetType().BaseType.GetProperty("Cache", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
      PropertyInfo devCacheProperty = devDic.GetType().BaseType.GetProperty("Cache", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
      Caching.Cache locCache = (Caching.Cache)locCacheProperty.GetValue(locDic);
      Caching.Cache geoCache = (Caching.Cache)geoCacheProperty.GetValue(geoDic);
      Caching.Cache devCache = (Caching.Cache)devCacheProperty.GetValue(devDic);
      if(locCache.MaxSize<=10000000)
         locCache.MaxSize = Settings.GetIntSetting("LocationsDictionaryCacheSize", 10000000);
      if (geoCache.MaxSize <= 10000000)
        geoCache.MaxSize = Settings.GetIntSetting("GeoIpDataDictionaryCacheSize", 10000000);
      if (devCache.MaxSize <= 10000000)
        devCache.MaxSize = Settings.GetIntSetting("DeviceDictionaryCacheSize", 10000000);
        //Sitecore.Caching.Cache para = new Caching.Cache("LocationsDictionaryCache", (long)maxCache);
        // cacheProperty.SetValue(dictionary, para);
      Log.Info("Invoke finished! ", this);
      
      
    }
  }
}
