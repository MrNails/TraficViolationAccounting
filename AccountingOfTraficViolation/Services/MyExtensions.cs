using System;
using AccountingOfTraficViolation.Models;
using Newtonsoft.Json;

namespace AccountingOfTraficViolation.Services
{
    public static class CloneObject
    {
        public static T Clone<T>(this T mainTable)
        {
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(mainTable), deserializeSettings);
        }
    }

}
