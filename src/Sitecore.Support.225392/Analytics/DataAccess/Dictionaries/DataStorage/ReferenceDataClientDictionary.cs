using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Abstractions;

namespace Sitecore.Support.Analytics.DataAccess.Dictionaries.DataStorage
{
  public class ReferenceDataClientDictionary: Sitecore.Analytics.DataAccess.Dictionaries.DataStorage.ReferenceDataClientDictionary
  {
    public ReferenceDataClientDictionary(Sitecore.Xdb.ReferenceData.Core.IReferenceDataClient referenceDataClient, Sitecore.Xdb.ReferenceData.Core.Converter.IMonikerConverter<Guid> guidMonikerConverter, Sitecore.Xdb.ReferenceData.Core.Results.IDefinitionOperationResultDiagnostics definitionOperationResultDiagnostics, BaseLog log) : base(referenceDataClient, guidMonikerConverter, definitionOperationResultDiagnostics, log)
    {
      this.CacheSize = Sitecore.Configuration.Settings.GetSetting("ReferenceDataClientDictionaryCacheSize", "1MB");
    }
  }
}