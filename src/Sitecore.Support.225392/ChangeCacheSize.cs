namespace Sitecore.Support.Analytics.Pipelines.StartAnalytics
{

  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using Sitecore.Analytics;
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
      if (locationDic == null)
        Log.Info("Support: location dictionary doesn't exist!", this);
      else
      {
        ResetCache(locationDic);//reset cache based on config settings node
      }
    }

    public static DictionaryBase GetDictionaryData()
    {
      GetDictionaryDataPipelineArgs getDictionaryDataPipelineArgs = new GetDictionaryDataPipelineArgs();
      GetDictionaryDataPipeline.Run(getDictionaryDataPipelineArgs);
      return Assert.ResultNotNull<DictionaryBase>(getDictionaryDataPipelineArgs.Result, "Check configuration, 'getDictionaryDataStorage' pipeline  must set args.Result property with instance of DictionaryBase type.");
    }

    private void ResetCache(LocationsDictionary dictionary)
    {
      PropertyInfo cacheProperty = dictionary.GetType().BaseType.GetProperty("Cache", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
      Caching.Cache origin = (Caching.Cache)cacheProperty.GetValue(dictionary);
      if (origin.MaxSize < 100000000)
      {
        int maxCache = Settings.GetIntSetting("LocationsDictionaryCacheSize", 100000000);
        //Sitecore.Caching.Cache para = new Caching.Cache("LocationsDictionaryCache", (long)maxCache);
        // cacheProperty.SetValue(dictionary, para);
        origin.MaxSize = maxCache;
        Log.Info("Invoke finished! New cache size is:" + maxCache, this);
      }
      
    }
  }
}
