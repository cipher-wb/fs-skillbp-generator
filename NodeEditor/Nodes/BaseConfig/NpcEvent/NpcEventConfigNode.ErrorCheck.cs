using GraphProcessor;
using HotFix.Game.MapEvent;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class NpcEventConfigNode
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        private bool IsExitInspectorError => !string.IsNullOrEmpty(InspectorError);

        public string InspectorError { get; set; }

        public override bool OnSaveCheck()
        {
            if (!base.OnSaveCheck()) { return false; }

            CheckError();

            if (IsExitInspectorError)
            {
                AppendSaveMapEventRet(InspectorError);
                return false;
            }

            return true;
        }

        public void CheckError()
        {
            InspectorError = string.Empty;

            var actorNodes = GetNextNodes<NpcEventLinkConfigNode>(typeof(ConfigPortType_NpcEventLinkConfig));
            var actorCount = actorNodes?.Count ?? 0;
            if (actorCount == 0)
            {
                InspectorError += "【演员数量为0】\n";
            }

            CheckActorFormation();

            CheckActorPerformance();
        }

        /// <summary>
        /// 检测演员数量，站位数量
        /// </summary>
        private void CheckActorFormation()
        {
            //演员数量
            var actorNodes = GetNextNodes<NpcEventLinkConfigNode>(typeof(ConfigPortType_NpcEventLinkConfig));
            var actorCount = actorNodes?.Count ?? 0;

            //获取站位表
            var formationConfig = MapEventActorFormationConfigManager.Instance.GetItem(Config.ActorFormation);
            if(formationConfig == default)
            {
                var formationNode = GetNextNodes<MapEventActorFormationConfigNode>(typeof(ConfigPortType_MapEventActorFormationConfig)).FirstOrDefault();
                formationConfig = formationNode?.Config;
            }
            if(formationConfig == default)
            {
                var formationRefNode = GetNextNodes<RefConfigBaseNode>(typeof(ConfigPortType_MapEventActorFormationConfig)).FirstOrDefault();
                formationConfig = formationRefNode?.Config as MapEventActorFormationConfig;      
            }

            if (formationConfig == default)
            {
                InspectorError += "【没有设置站位表】\n";
                return;
            }

            if (actorCount > 0 && formationConfig != default)
            {
                var formationCount = formationConfig.Formations.Count;
                if (actorCount != formationCount)
                {
                    InspectorError += $"【演员数量{actorCount}】【站位数量{formationCount}】\n";
                }
            }
        }

        private void CheckActorPerformance()
        {
            var standbyCount = Config.StandbyPerformanceGroupID?.Count ?? 0;

            var actorNodes = GetNextNodes<NpcEventLinkConfigNode>(typeof(ConfigPortType_NpcEventLinkConfig));
            var actorCount = actorNodes?.Count ?? 0;
            if (actorCount != standbyCount)
            {
                InspectorError += $"【演员数量{actorCount}】【待机表演配置数量{standbyCount}】\n";
            }
        }
    }
}
