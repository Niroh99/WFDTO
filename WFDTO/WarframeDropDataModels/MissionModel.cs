using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WFDTO.WarframeDropDataModels
{
    public class MissionModel
    {
        public string GameMode { get; set; }

        public bool IsEvent { get; set; }
        
        public JToken Rewards { get; set; }

        private MissionRewardsModel _missionRewards;
        [JsonIgnore]
        public MissionRewardsModel MissionRewards
        {
            get
            {
                if (_missionRewards == null) _missionRewards = ParseRewards();

                return _missionRewards;
            }
        }

        private MissionRewardsModel ParseRewards()
        {
            var missionRewards = new MissionRewardsModel();

            if (Rewards.Type == JTokenType.Array)
            {
                var children = Rewards.Children<JObject>().ToList();

                missionRewards.AddRange(GetItemModels(children));
            }
            else if (Rewards.Type == JTokenType.Object)
            {
                var jObject = Rewards as JObject;

                var properties = jObject.Properties().ToList();

                if (properties.Count == 3)
                {
                    var children = properties[0].Value.Children<JObject>().ToList();

                    missionRewards.A = GetItemModels(children);

                    children = properties[1].Value.Children<JObject>().ToList();

                    missionRewards.B = GetItemModels(children);

                    children = properties[2].Value.Children<JObject>().ToList();

                    missionRewards.C = GetItemModels(children);
                }
            }

            return missionRewards;
        }

        private List<ItemModel> GetItemModels(List<JObject> children)
        {
            var response = new List<ItemModel>();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                response.Add(child.ToObject<ItemModel>());
            }

            return response;
        }
    }
}