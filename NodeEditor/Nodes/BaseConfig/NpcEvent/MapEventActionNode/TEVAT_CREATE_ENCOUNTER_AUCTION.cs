using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public class ActionEncounterAuctionData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("拍卖会"), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddAuctionData")]
        public List<TableSelectData> AuctionDatas = new List<TableSelectData>();

        public ActionEncounterAuctionData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            AuctionDatas.Clear();
            param?.ForEach(auctionID =>
            {
                var auctionTable = new TableSelectData(typeof(AuctionConfig).FullName, param[0]);
                auctionTable.OnSelectedID();
                AuctionDatas.Add(auctionTable);
            });
        }

        public override List<int> ToParam()
        {
            var auctionDatas = new List<int>();
            AuctionDatas?.ForEach(auctionData =>
            {
                auctionDatas.Add(auctionData.ID);
            });
            return auctionDatas;
        }

        public TableSelectData OnAddAuctionData()
        {
            return new TableSelectData(typeof(AuctionConfig).FullName, 0);
        }

        public override void CheckError()
        {
            AuctionDatas?.ForEach(auctionData =>
            {
                if(auctionData.TableConfig == default)
                {
                    BaseNode.InspectorError += $"配置表不存在 {auctionData.ID} \n";
                }
            });
        }
    }

    [Serializable]
    [NodeMenuItem("主行为/创建奇遇拍卖会", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_CREATE_ENCOUNTER_AUCTION : NpcEventActionConfigNode
    {
        // 创建奇遇拍卖会
        // 参数0 : 拍卖会配置ID
        public TEVAT_CREATE_ENCOUNTER_AUCTION() : base(NpcEventActionConfig_TEventActionType.TEVAT_CREATE_ENCOUNTER_AUCTION)
        {
            ActionData = new ActionEncounterAuctionData(this);
        }
    }
}