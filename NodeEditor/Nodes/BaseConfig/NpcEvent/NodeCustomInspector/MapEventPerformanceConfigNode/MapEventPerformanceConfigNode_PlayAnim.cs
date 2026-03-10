using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;
using static NodeEditor.MapEventPerformanceConfigNode;

namespace NodeEditor
{
    public class PlayAnimData
    {
        /// <summary>
        /// 动作类型
        /// 1.常规动作 2.override动作
        /// </summary>
        [LabelText("动作类型")]
        public PlayAnimType PlayAnimType;

        /// <summary>
        /// 常规动作 动作名
        /// </summary>
        [LabelText("常规动作类型"), ShowIf("@PlayAnimType == MapEventPerformanceConfigNode.PlayAnimType.Normal")]
        public TMapAnimatorType MapAnimatorType;

        /// <summary>
        /// override动作名字
        /// </summary>
        [LabelText("动作名字"), ShowIf("@PlayAnimType == MapEventPerformanceConfigNode.PlayAnimType.Override")]
        public string OverrideAnimName;

        /// <summary>
        /// 等待动作播完
        /// </summary>
        [LabelText("等待结束")]
        public bool WaitFinished;

        /// <summary>
        /// 等待动作播完
        /// </summary>
        [LabelText("结束切回待机")]
        public bool EndToStandby = true;

        public PlayAnimData() { }

        public PlayAnimData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            if(PlayAnimType == PlayAnimType.Normal)
            {
                return $"{(int)PlayAnimType}|{(int)MapAnimatorType}|{(WaitFinished ? 1 : 0)}|{(EndToStandby ? 0 : 1)}";
            }
            else
            {
                return $"{(int)PlayAnimType}|{OverrideAnimName}|{(WaitFinished ? 1 : 0)}|{(EndToStandby ? 0 : 1)}";
            }
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                PlayAnimType = PlayAnimType.Override;
                return;
            }

            var split = param.Split('|');
            if (split.Length < 3) { return; }

            if (int.TryParse(split[0], out var param0))
            {
                PlayAnimType = (PlayAnimType)param0;
            }

            //常规动作
            if (PlayAnimType == PlayAnimType.Normal)
            {
                if (int.TryParse(split[1], out var param1))
                {
                    MapAnimatorType = (TMapAnimatorType)param1;
                }
            }
            //override动作
            else if (PlayAnimType == PlayAnimType.Override)
            {
                MapAnimatorType = TMapAnimatorType.TMA_Override;
                OverrideAnimName = split[1];
            }

            if (int.TryParse(split[2], out var param2))
            {
                WaitFinished = param2 != 0 ? true : false;
            }

            EndToStandby = true;
            if (split.Length > 3 && int.TryParse(split[3], out var param3))
            {
                EndToStandby = param3 == 0 ? true : false;
            }
        }
    }

    public class MapEventPerformanceConfigNode_PlayAnim : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_PlayAnim(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlayAnimData perfData = new PlayAnimData();

        private void OnParamChanged()
        {
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;
        }

        public void ConfigToData()
        {
            perfData = new PlayAnimData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PlayAnimData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}
