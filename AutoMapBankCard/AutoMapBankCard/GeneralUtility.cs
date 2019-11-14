﻿using System.Collections.Generic;
using System.Linq;

namespace AutoMapBankCard
{
    public static class GeneralUtility
    {
        public static string ConvertToKeyValueString(Dictionary<string, string> dictionary)
            => dictionary.Select(d => $"{d.Key}={d.Value}").Aggregate((result, next) => $"{result}&{next}");
    }
}