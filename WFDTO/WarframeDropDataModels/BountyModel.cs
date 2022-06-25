using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFDTO.WarframeDropDataModels
{
    public class BountyModel
    {
        public string BountyLevel { get; set; }

        public JToken Rewards { get; set; }

        private BountyRewardsModel _bountyRewards;
        [JsonIgnore]
        public BountyRewardsModel BountyRewards
        {
            get
            {
                if (_bountyRewards == null) _bountyRewards = ParseRewards();

                return _bountyRewards;
            }
        }

        private BountyRewardsModel ParseRewards()
        {
            var bountyRewards = new BountyRewardsModel();

            if (Rewards.Type == JTokenType.Object)
            {
                var jObject = Rewards as JObject;

                var properties = jObject.Properties().ToList();

                if (properties.Count == 3)
                {
                    var children = properties[0].Value.Children<JObject>().ToList();

                    bountyRewards.A = GetItemModels(children);

                    children = properties[1].Value.Children<JObject>().ToList();

                    bountyRewards.B = GetItemModels(children);

                    children = properties[2].Value.Children<JObject>().ToList();

                    bountyRewards.C = GetItemModels(children);
                }
            }

            return bountyRewards;
        }

        private List<BountyItemModel> GetItemModels(List<JObject> children)
        {
            var response = new List<BountyItemModel>();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                response.Add(child.ToObject<BountyItemModel>());
            }

            return response;
        }
    }
}