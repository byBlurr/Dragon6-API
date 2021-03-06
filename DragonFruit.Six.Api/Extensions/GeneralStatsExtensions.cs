﻿// Dragon6 API Copyright 2020 DragonFruit Network <inbox@dragonfruit.network>
// Licensed under Apache-2. Please refer to the LICENSE file for more info

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DragonFruit.Six.Api.Entities;
using DragonFruit.Six.Api.Deserializers;
using DragonFruit.Six.Api.Requests;
using DragonFruit.Six.Api.Utils;
using Newtonsoft.Json.Linq;

namespace DragonFruit.Six.Api.Extensions
{
    public static class GeneralStatsExtensions
    {
        /// <summary>
        /// Get the <see cref="GeneralStats"/> (non-seasonal) for an <see cref="AccountInfo"/>
        /// </summary>
        public static GeneralStats GetStats<T>(this T client, AccountInfo account, CancellationToken token = default) where T : Dragon6Client
            => GetStats(client, account.Yield(), token).For(account);

        /// <summary>
        /// Get the <see cref="GeneralStats"/> (non-seasonal) for an array of <see cref="AccountInfo"/>s
        /// </summary>
        public static ILookup<string, GeneralStats> GetStats<T>(this T client, IEnumerable<AccountInfo> accounts, CancellationToken token = default) where T : Dragon6Client
        {
            var filteredGroups = accounts.GroupBy(x => x.Platform);
            JObject data = null;

            foreach (var group in filteredGroups)
            {
                var request = new StatsRequest(group);
                var platformResponse = client.Perform<JObject>(request, token);

                if (data == null)
                {
                    data = platformResponse;
                }
                else
                {
                    data.Merge(platformResponse);
                }
            }

            return data.DeserializeGeneralStats();
        }
    }
}
