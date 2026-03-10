using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class NpcTalkOptionConfigNode
    {
        /// <summary>
        /// 当一个节点没有连线时要不要设置为null
        /// </summary>
        public override bool IsPortNoEdgeSetNull => false;

        public NpcTalkOptionConfigNode(TNpcEventDialogOptionType type) : base()
        {
            Config.ExSetValue(nameof(Config.NpcEventDialogOptionType), type);
        }

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            if(TalkOptionData == null)
            {
                SetCustomName($"[{Config.ID}][选项]");
                return;
            }

            TalkOptionData.OnRefreshCustomName();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            ConfigToOptionData();

            ConfigToCondition();

            ConfigToShowItem();
        }

        public List<NpcTalkGroupConfigNode> GetNpcTalkGroupConfigNodes()
        {
            if (inputPorts == null || inputPorts.Count == 0) { return null; }

            //获取TalkNode
            var npcTalkConfigNode = GetPreviousNode<NpcTalkConfigNode>();
            if (npcTalkConfigNode == null) { return null; }

            //获取TalkGroupNode
            var npcTalkGroupConfigNode = npcTalkConfigNode.GetPreviousNode<NpcTalkGroupConfigNode>();
            if (npcTalkGroupConfigNode == null) { return null; }

            //获取TEVAT_DIALOG
            var dialogNode = npcTalkGroupConfigNode.GetPreviousNode<TEVAT_DIALOG>();
            if (dialogNode == null) { return null; }

            NodePort talkGroupPort = null;
            foreach (var port in dialogNode.outputPorts)
            {
                if (port.portData.displayType == typeof(ConfigPortType_NpcTalkGroupConfig))
                {
                    talkGroupPort = port;
                }
            }

            List<NpcTalkGroupConfigNode> nodeList = new List<NpcTalkGroupConfigNode>();
            talkGroupPort?.GetEdges()?.ForEach(edge =>
            {
                if (edge.inputNode is NpcTalkGroupConfigNode myNode)
                {
                    nodeList.Add(myNode);
                }
            });

            return nodeList;
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("类型"), HideReferenceObjectPicker]
        [FoldoutGroup("选项", true, 0)]
        [OnValueChanged("OnOptionTypeChanged"), DelayedProperty]
        [ValueDropdown("@NpcTalkOptionConfigNode.VD_TNpcEventDialogOptionType")]
        private TNpcEventDialogOptionType optionType = TNpcEventDialogOptionType.TNEDOT_NULL;

        /// <summary>
        /// 切换optionType
        /// </summary>
        private void OnOptionTypeChanged()
        {
            //NpcEventDialogOptionType
            SetConfigValue(nameof(Config.NpcEventDialogOptionType), optionType);

            if (optionTypeMapping.TryGetValue(optionType, out var inspectorType))
            {
                TalkOptionData = Activator.CreateInstance(inspectorType, this) as TalkOptionData;
                TalkOptionData.SetDefault();
                TalkOptionData.CheckError();
            }

            //BlockInteractionType
            blockInteractionType = optionType == TNpcEventDialogOptionType.TNEDOT_TRADE ? TNpcInteractionType.TNI_TRADE : TNpcInteractionType.TNI_NULL;
            SetConfigValue(nameof(Config.BlockInteractionType), blockInteractionType);

            OnRefreshCustomName();
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, HideLabel]
        [ShowIf("@optionType != TNpcEventDialogOptionType.TNEDOT_NULL"), OnValueChanged("OnChangedData", true), DelayedProperty]
        [InfoBox("@InspectorError", InfoMessageType.Error, "IsExitInspectorError")]
        [FoldoutGroup("选项", true, order: 0)]
        public TalkOptionData TalkOptionData { get; private set; }

        /// <summary>
        /// 数据修改保存
        /// </summary>
        private void OnChangedData()
        {
            TalkOptionData?.OnValueChanged();
            TalkOptionData?.CheckError();
        }

        /// <summary>
        /// 还原选项数据
        /// </summary>
        private void ConfigToOptionData()
        {
            optionType = Config?.NpcEventDialogOptionType ?? TNpcEventDialogOptionType.TNEDOT_NULL;

            if (optionTypeMapping.TryGetValue(optionType, out var inspectorType))
            {
                TalkOptionData ??= Activator.CreateInstance(inspectorType, this) as TalkOptionData;
                TalkOptionData.ConfigToData();
                TalkOptionData.CheckError();
            }
        }

        /// <summary>
        /// 截断NPC原交互 通过选项类型自动设置
        /// </summary>
        private TNpcInteractionType blockInteractionType = TNpcInteractionType.TNI_NULL;
    }
}
