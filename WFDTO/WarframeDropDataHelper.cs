using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace WFDTO
{
    public static class WarframeDropDataHelper
    {
        public static void Initialize()
        {
            var httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://drops.warframestat.us/data/all.json");

            var httpResponse = httpClient.SendAsync(request);
            httpResponse.Wait();
            var result = httpResponse.Result;
            var content = result.Content.ReadAsStringAsync().Result;

            var response = JsonConvert.DeserializeObject<WarframeDropDataModels.AllDataReponse>(content);

            Missions.Clear();

            var missionRewardsEnumerator = response.MissionRewards.GetEnumerator();

            while (missionRewardsEnumerator.MoveNext())
            {
                var planetKeyValuePair = missionRewardsEnumerator.Current;

                var missionsEnumerator = planetKeyValuePair.Value.GetEnumerator();

                while (missionsEnumerator.MoveNext())
                {
                    var missionModelKeyValuePair = missionsEnumerator.Current;

                    var mission = new SearchResultModels.Mission
                    {
                        Planet = planetKeyValuePair.Key,
                        Name = missionModelKeyValuePair.Key,
                        Type = missionModelKeyValuePair.Value.GameMode
                    };

                    for (int i = 0; i < missionModelKeyValuePair.Value.MissionRewards.Count; i++)
                    {
                        var missionReward = missionModelKeyValuePair.Value.MissionRewards[i];

                        if (mission.Rewards == null) mission.Rewards = new List<SearchResultModels.Item>();

                        mission.Rewards.Add(missionReward.ToItem());
                    }

                    if (missionModelKeyValuePair.Value.MissionRewards.A != null)
                    {
                        for (int i = 0; i < missionModelKeyValuePair.Value.MissionRewards.A.Count; i++)
                        {
                            var missionReward = missionModelKeyValuePair.Value.MissionRewards.A[i];

                            if (mission.RotationA == null) mission.RotationA = new List<SearchResultModels.Item>();

                            mission.RotationA.Add(missionReward.ToItem());
                        }
                    }

                    if (missionModelKeyValuePair.Value.MissionRewards.B != null)
                    {
                        for (int i = 0; i < missionModelKeyValuePair.Value.MissionRewards.B.Count; i++)
                        {
                            var missionReward = missionModelKeyValuePair.Value.MissionRewards.B[i];

                            if (mission.RotationB == null) mission.RotationB = new List<SearchResultModels.Item>();

                            mission.RotationB.Add(missionReward.ToItem());
                        }
                    }

                    if (missionModelKeyValuePair.Value.MissionRewards.C != null)
                    {
                        for (int i = 0; i < missionModelKeyValuePair.Value.MissionRewards.C.Count; i++)
                        {
                            var missionReward = missionModelKeyValuePair.Value.MissionRewards.C[i];

                            if (mission.RotationC == null) mission.RotationC = new List<SearchResultModels.Item>();

                            mission.RotationC.Add(missionReward.ToItem());
                        }
                    }

                    Missions.Add(mission);
                }
            }

            Relics.Clear();

            for (int i = 0; i < response.Relics.Count; i++)
            {
                var relicModel = response.Relics[i];

                if (i == 0 || i % 4 != 0) continue;

                var relic = new SearchResultModels.Relic
                {
                    Era = relicModel.Tier,
                    Description = relicModel.RelicName
                };

                relic.Rewards = relicModel.Rewards.Select(j => j.ToItem()).OrderByDescending(i => i.DropChance).ToList();

                var locations = Missions.Where(j => j.Rewards?.Any(k => k.Name == $"{relic.Era} {relic.Description} Relic") == true).ToList();

                for (int j = 0; j < locations.Count; j++)
                {
                    var location = locations[j];

                    if (relic.Locations == null) relic.Locations = new List<SearchResultModels.ItemLocationGroup>();

                    var itemLocationGroup = relic.Locations.FirstOrDefault(i => i.Planet == location.Planet);

                    if (itemLocationGroup == null)
                    {
                        itemLocationGroup = new SearchResultModels.ItemLocationGroup
                        {
                            Planet = location.Planet,
                            ItemLocations = new List<SearchResultModels.ItemLocation>()
                        };

                        relic.Locations.Add(itemLocationGroup);
                    }

                    var chance = location.Rewards.FirstOrDefault(k => k.Name == $"{relic.Era} {relic.Description} Relic").DropChance;

                    itemLocationGroup.ItemLocations.Add(new SearchResultModels.ItemLocation
                    {
                        Mission = location,
                        Chance = chance
                    });
                }

                locations = Missions.Where(j => j.RotationA?.Any(k => k.Name == $"{relic.Era} {relic.Description} Relic") == true).ToList();

                for (int j = 0; j < locations.Count; j++)
                {
                    var location = locations[j];

                    if (relic.Locations == null) relic.Locations = new List<SearchResultModels.ItemLocationGroup>();

                    var itemLocationGroup = relic.Locations.FirstOrDefault(i => i.Planet == location.Planet);

                    if (itemLocationGroup == null)
                    {
                        itemLocationGroup = new SearchResultModels.ItemLocationGroup
                        {
                            Planet = location.Planet,
                            ItemLocations = new List<SearchResultModels.ItemLocation>()
                        };

                        relic.Locations.Add(itemLocationGroup);
                    }

                    var chance = location.RotationA.FirstOrDefault(k => k.Name == $"{relic.Era} {relic.Description} Relic").DropChance;

                    itemLocationGroup.ItemLocations.Add(new SearchResultModels.ItemLocation
                    {
                        Rotation = "A",
                        Chance = chance,
                        Mission = location
                    });
                }

                locations = Missions.Where(j => j.RotationB?.Any(k => k.Name == $"{relic.Era} {relic.Description} Relic") == true).ToList();

                for (int j = 0; j < locations.Count; j++)
                {
                    var location = locations[j];

                    if (relic.Locations == null) relic.Locations = new List<SearchResultModels.ItemLocationGroup>();

                    var itemLocationGroup = relic.Locations.FirstOrDefault(i => i.Planet == location.Planet);

                    if (itemLocationGroup == null)
                    {
                        itemLocationGroup = new SearchResultModels.ItemLocationGroup
                        {
                            Planet = location.Planet,
                            ItemLocations = new List<SearchResultModels.ItemLocation>()
                        };

                        relic.Locations.Add(itemLocationGroup);
                    }

                    var chance = location.RotationB.FirstOrDefault(k => k.Name == $"{relic.Era} {relic.Description} Relic").DropChance;

                    itemLocationGroup.ItemLocations.Add(new SearchResultModels.ItemLocation
                    {
                        Rotation = "B",
                        Chance = chance,
                        Mission = location
                    });
                }

                locations = Missions.Where(j => j.RotationC?.Any(k => k.Name == $"{relic.Era} {relic.Description} Relic") == true).ToList();

                for (int j = 0; j < locations.Count; j++)
                {
                    var location = locations[j];

                    if (relic.Locations == null) relic.Locations = new List<SearchResultModels.ItemLocationGroup>();

                    var itemLocationGroup = relic.Locations.FirstOrDefault(i => i.Planet == location.Planet);

                    if (itemLocationGroup == null)
                    {
                        itemLocationGroup = new SearchResultModels.ItemLocationGroup
                        {
                            Planet = location.Planet,
                            ItemLocations = new List<SearchResultModels.ItemLocation>()
                        };

                        relic.Locations.Add(itemLocationGroup);
                    }

                    var chance = location.RotationC.FirstOrDefault(k => k.Name == $"{relic.Era} {relic.Description} Relic").DropChance;

                    itemLocationGroup.ItemLocations.Add(new SearchResultModels.ItemLocation
                    {
                        Rotation = "C",
                        Chance = chance,
                        Mission = location
                    });
                }

                Relics.Add(relic);
            }
        }

        public static readonly List<SearchResultModels.Mission> Missions = new List<SearchResultModels.Mission>();

        public static readonly List<SearchResultModels.Relic> Relics = new List<SearchResultModels.Relic>();
    }
}