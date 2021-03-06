﻿// Dragon6 API Copyright 2020 DragonFruit Network <inbox@dragonfruit.network>
// Licensed under Apache-2. Please refer to the LICENSE file for more info

using System.Collections.Generic;
using System.Linq;
using DragonFruit.Common.Data.Extensions;
using DragonFruit.Six.Api.Entities;
using DragonFruit.Six.Api.Strings;
using DragonFruit.Six.Api.Utils;
using Newtonsoft.Json.Linq;

namespace DragonFruit.Six.Api.Deserializers
{
    public static class OperatorStatsDeserializer
    {
        public static ILookup<string, OperatorStats> DeserializeOperatorStats(this JObject json, IEnumerable<OperatorStats> data, bool training = false)
        {
            var results = (json[Misc.Results] as JObject)?.Properties();

            var enumeratedResults = training switch
            {
                true => results?.SelectMany(x => DeserializeTrainingInternal(x, data)),
                false => results?.SelectMany(x => DeserializeInternal(x, data))
            };

            enumeratedResults ??= Enumerable.Empty<OperatorStats>();

            return enumeratedResults.ToLookup(x => x.ProfileId);
        }

        private static IEnumerable<OperatorStats> DeserializeInternal(JProperty data, IEnumerable<OperatorStats> operators)
        {
            var property = (JObject)data.Value;

            foreach (var op in operators.Select(x => x.Clone()))
            {
                op.ProfileId = data.Name;

                op.Kills = property.GetUInt(Operator.Kills.ToIndexedStatsKey(op.Index));
                op.Deaths = property.GetUInt(Operator.Deaths.ToIndexedStatsKey(op.Index));

                op.Wins = property.GetUInt(Operator.Wins.ToIndexedStatsKey(op.Index));
                op.Losses = property.GetUInt(Operator.Losses.ToIndexedStatsKey(op.Index));

                op.RoundsPlayed = property.GetUInt(Operator.Rounds.ToIndexedStatsKey(op.Index));
                op.Duration = property.GetUInt(Operator.Time.ToIndexedStatsKey(op.Index));

                op.Headshots = property.GetUInt(Operator.Headshots.ToIndexedStatsKey(op.Index));
                op.Downs = property.GetUInt(Operator.Downs.ToIndexedStatsKey(op.Index));

                op.Experience = property.GetUInt(Operator.Experience.ToIndexedStatsKey(op.Index));
                op.ActionCount = (uint?)property[op.OperatorActionResultId];

                yield return op;
            }
        }

        public static IEnumerable<OperatorStats> DeserializeTrainingInternal(JProperty data, IEnumerable<OperatorStats> operators)
        {
            var property = (JObject)data.Value;

            foreach (var op in operators.Select(x => x.Clone()))
            {
                op.ProfileId = data.Name;

                op.Kills = property.GetUInt(Operator.KillsTraining.ToIndexedStatsKey(op.Index));
                op.Deaths = property.GetUInt(Operator.DeathsTraining.ToIndexedStatsKey(op.Index));

                op.Wins = property.GetUInt(Operator.WinsTraining.ToIndexedStatsKey(op.Index));
                op.Losses = property.GetUInt(Operator.LossesTraining.ToIndexedStatsKey(op.Index));

                op.RoundsPlayed = property.GetUInt(Operator.RoundsTraining.ToIndexedStatsKey(op.Index));
                op.Duration = property.GetUInt(Operator.TimeTraining.ToIndexedStatsKey(op.Index));

                op.Headshots = property.GetUInt(Operator.HeadshotsTraining.ToIndexedStatsKey(op.Index));
                op.Downs = property.GetUInt(Operator.DownsTraining.ToIndexedStatsKey(op.Index));

                op.Experience = property.GetUInt(Operator.ExperienceTraining.ToIndexedStatsKey(op.Index));

                // right now the keys for the actions are all pvp, so we'd need to add pve and be able to switch them if we want to get this to work
                op.ActionCount = null;

                yield return op;
            }
        }
    }
}
