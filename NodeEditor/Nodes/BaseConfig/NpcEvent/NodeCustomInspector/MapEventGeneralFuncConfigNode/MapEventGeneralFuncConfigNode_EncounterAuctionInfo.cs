using GameApp.Editor;
using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TableDR;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_EncounterAuctionInfo : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("选择拍卖会")]
        [OnValueChanged("OAuctionChanged", true)]
        public TableSelectData AuctionTable = new TableSelectData(typeof(AuctionConfig).FullName, 0);

        public MapEventGeneralFuncConfigNode_EncounterAuctionInfo(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        private void OAuctionChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { AuctionTable.ID });

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            var auctionConfig = AuctionConfigManager.Instance.GetItem(AuctionTable.ID);
            if (auctionConfig == default)
            {
                baseNode.InspectorError = $"{AuctionTable.ID} 拍卖会不存在\n";
            }

            if (auctionConfig != default && auctionConfig.CreateType != AuctionConfig_TAuctionCreateType.TAUCT_ENCOUNTER)
            {
                baseNode.InspectorError = $"{auctionConfig.Name} 不是奇遇拍卖会\n";
            }

            if (!baseNode.IsLastNode())
            {
                baseNode.InspectorError += $"【必须是列表的最后一个】\n";
            }
        }

        public void ConfigToData()
        {
            if (baseNode.Config != null && baseNode.Config.IntParams1.Count > 0)
            {
                AuctionTable = new TableSelectData(typeof(AuctionConfig).FullName, baseNode.Config.IntParams1[0]);
            }
        }

        public void SetDefault()
        {
            AuctionTable = new TableSelectData(typeof(AuctionConfig).FullName, 0);
        }
    }
}
