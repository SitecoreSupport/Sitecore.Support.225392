using Sitecore.Analytics.DataAccess.Dictionaries;
using Sitecore.Analytics.DataAccess.Pipelines.GetDictionaryDataStorage;
using Sitecore.Framework.Conditions;
using Sitecore.Analytics.DataAccess.Pipelines.InitializeKnownDataDictionaries;

namespace Sitecore.Support.Analytics.DataAccess.Pipelines.InitializeKnownDataDictionaries
{
  public class InitializeKnownDataDictionariesProcessor : InitializeKnownDataDictionariesProcessorBase
  {
    public override void Process([NotNull] InitializeKnownDataDictionariesArgs args)
    {
      Condition.Requires(args, nameof(args)).IsNotNull();
      var dictionaryDataArgs = new GetDictionaryDataPipelineArgs();
      GetDictionaryDataPipeline.Run(dictionaryDataArgs);
      Condition.Ensures(dictionaryDataArgs.Result).IsNotNull("Check configuration, 'getDictionaryDataStorage' pipeline  must set args.Result property with instance of DictionaryBase type.");
      args.UserAgentsDictionary = new Sitecore.Support.Analytics.DataAccess.Dictionaries.UserAgentsDictionary(dictionaryDataArgs.Result);
    }
  }
}
