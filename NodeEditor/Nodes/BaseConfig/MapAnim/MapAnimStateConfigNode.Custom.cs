using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;
using UnityEngine;
using GameApp;
using HotFix.Game.Entities.MapAnimState;
using ValueType = HotFix.Game.Entities.MapAnimState.ValueType;
using Funny.Base.Utils;

namespace NodeEditor
{
    public partial class MapAnimStateConfigNode : IParamsNode, IMapAnimStateConfigNode
    {
        public MapAnimStateConfig CurConfig => Config;
        public string SelfDesc
        {
            get => Desc;
            set => Desc = value; 
        }

        #region 自定义填入
        Dictionary<MapAnimStateConfig_PointType, MapAnimStateCustomData> typeDic = new Dictionary<MapAnimStateConfig_PointType, MapAnimStateCustomData>();
        Dictionary<MapAnimStateConfig_TJumpType, MapAnimStateCustomData> jumpDic = new Dictionary<MapAnimStateConfig_TJumpType, MapAnimStateCustomData>();

        [FoldoutGroup("自定义填入", true, 99), LabelText("状态参数"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.Default)]
        public DefaultType DefaultType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("动画参数"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.Anim)]
        public AnimType AnimType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("更新随机参数"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.UpdateRandom)]
        public UpdateRandomType UpdateRandomType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("帧事件"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.MapEvent)]
        public MapEventType MapEventType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("根节点"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.Root)]
        public RootType RootType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("创建效果节点"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.CreateEffect)]
        public CreateEffectType CreateEffectType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("筛选节点"), ShowIf("@this.Config.Type", MapAnimStateConfig_PointType.Select)]
        public SelectType selectType; 

        [FoldoutGroup("自定义填入", true, 99), LabelText("结束跳转参数"), ShowIf("@this.Config.JumpType", MapAnimStateConfig_TJumpType.End)]
        public EndJumpType EndJumpType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("随机跳转参数"), ShowIf("@this.Config.JumpType", MapAnimStateConfig_TJumpType.Random)]
        public RandomJumpType RandomJumpType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("条件跳转参数"), ShowIf("@this.IsConditionJump()", true)]
        public ConditionJumpType ConditionJumpType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("循环执行"), ShowIf("@this.Config.JumpType", MapAnimStateConfig_TJumpType.Loop)]
        public LoopJumpType LoopJumpType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("被父节点持有"), ShowIf("@this.Config.JumpType", MapAnimStateConfig_TJumpType.BeHold)]
        public BeHoldJumpType BeHoldJumpType;

        [FoldoutGroup("自定义填入", true, 99), LabelText("执行后跳转参数"), ShowIf("@this.Config.JumpType", MapAnimStateConfig_TJumpType.AfterDo)]
        public AfterDoType AfterDoType;

        bool IsConditionJump() => Config.JumpType is MapAnimStateConfig_TJumpType.Condition or MapAnimStateConfig_TJumpType.ConditionOrEnd;

        public MapAnimStateConfigNode(MapAnimStateConfig_PointType type) : base()
        {
            SetConfigValue(nameof(Config.Type), type);

            typeDic.Add(MapAnimStateConfig_PointType.Default, InitData(ref DefaultType));
            typeDic.Add(MapAnimStateConfig_PointType.Anim, InitData(ref AnimType));
            typeDic.Add(MapAnimStateConfig_PointType.UpdateRandom, InitData(ref UpdateRandomType));
            typeDic.Add(MapAnimStateConfig_PointType.MapEvent, InitData(ref MapEventType));
            typeDic.Add(MapAnimStateConfig_PointType.Root, InitData(ref RootType));
            typeDic.Add(MapAnimStateConfig_PointType.CreateEffect, InitData(ref CreateEffectType));

            jumpDic.Add(MapAnimStateConfig_TJumpType.End, InitData(ref EndJumpType));
            jumpDic.Add(MapAnimStateConfig_TJumpType.Random, InitData(ref RandomJumpType));
            jumpDic.Add(MapAnimStateConfig_TJumpType.Condition, InitData(ref ConditionJumpType));
            jumpDic.Add(MapAnimStateConfig_TJumpType.ConditionOrEnd, InitData(ref ConditionJumpType));
            jumpDic.Add(MapAnimStateConfig_TJumpType.Loop, InitData(ref LoopJumpType));
            jumpDic.Add(MapAnimStateConfig_TJumpType.BeHold, InitData(ref BeHoldJumpType));
            jumpDic.Add(MapAnimStateConfig_TJumpType.AfterDo, InitData(ref AfterDoType));
        }

        MapAnimStateCustomData InitData<T>(ref T data) where T : MapAnimStateCustomData, new()
        {
            data ??= new T();
            data.Parent = this;
            return data;
        }

        [FoldoutGroup("自定义填入", true, 99), Button("保存", ButtonSizes.Medium)]
        public void SaveAll()
        {
            base.Save();
        }

        public void SaveCustomData()
        {
            var strValues = new List<string>();
            var intValues = new List<int>();

            if (typeDic.TryGetValue(Config.Type, out var data))
            {
                data.Save(strValues, intValues);
            }
            else
            {
                Log.Error("no data");
            }

            if (Config.StrValues == null)
            {
                SetConfigValue(nameof(Config.StrValues), new List<string>());
            }
            Config.StrValues.GetListRef().Clear();
            Config.StrValues.GetListRef().AddRange(strValues);

            if (Config.Values == null)
            {
                SetConfigValue(nameof(Config.Values), new List<int>());
            }
            Config.Values.GetListRef().Clear();
            Config.Values.GetListRef().AddRange(intValues);

            intValues.Clear();
            if (jumpDic.TryGetValue(Config.JumpType, out data))
            {
                data.Save(null, intValues);
            }
            else
            {
                Log.Error("no data");
            }
            if (Config.JumpConditions == null)
            {
                SetConfigValue(nameof(Config.JumpConditions), new List<int>());
            }
            Config.JumpConditions.GetListRef().Clear();
            Config.JumpConditions.GetListRef().AddRange(intValues);
        }

        void UpdateCustomData()
        {
            int strIndex = 0;

            if (typeDic.TryGetValue(Config.Type, out var data))
            {
                data.LoadTable(ref strIndex, ValueType.StateType);
            }

            if (jumpDic.TryGetValue(Config.JumpType, out data))
            {
                data.LoadTable(ref strIndex, ValueType.JumpConition);
            }
        }

        protected override void OnSave()
        {
            SaveCustomData();
            base.OnSave();
        }
        #endregion

        /// <summary>
        /// 成员参数属性自定义处理
        /// </summary>
        public void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes) { }

        /// <summary>
        /// 重写node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            // 依据表格效果类型显示不同表现
            SetCustomName($"[{EnumUtility.GetDescription(Config.Type, false)}][{Config.ID}]");
        }

        public override bool OnPostProcessing()
        {
            var result = base.OnPostProcessing();
            UpdateCustomData();
            return result;
        }

        protected override void OnConfigChanged()
        {
            base.OnConfigChanged();
            UpdateCustomData();
        }

        public override bool OnSaveCheck()
        {
            if (!base.OnSaveCheck())
            {
                return false;
            }

            switch (Config.Type)
            {
                case MapAnimStateConfig_PointType.UpdateRandom:
                    if (!UpdateRandomType.IsRandomValue)
                    {
                        AppendSaveRet($"随机参数错误, 不是随机值!");
                        return false;
                    }

                    if (UpdateRandomType.UseMode == UpdateRandomType.Mode.TimerSetValue && 
                        UpdateRandomType.UpdateRate <= 0 &&
                        Config.JumpType == MapAnimStateConfig_TJumpType.BeHold)
                    {
                        AppendSaveRet($"随机间隔过短!");
                        return false;
                    }
                    break;
            }

            {
                int count = 0;

                foreach (var item in outputPorts)
                {
                    var edges = item.GetEdges();

                    if (item.owner == this && edges.Count > 0)
                    {
                        foreach (var nextNode in edges)
                        {
                            if (nextNode.inputPort.owner is MapAnimStateConfigNode or RefConfigBaseNode)
                            {
                                count++;
                            }
                        }
                    }
                }

                if (Config.JumpId.Count != count)
                {
                    if (Config.JumpId.Count > count)
                    {
                        int max = Config.JumpId.Count;

                        for (int i = count; i < max; i++)
                        {
                            Config.JumpId.GetListRef().RemoveAt(Config.JumpId.Count - 1);
                        }

                        AppendSaveRet($"错误的跳转数量{Config.ID}");
                    }
                    else
                    {
                        AppendSaveRet($"错误的跳转数量{Config.ID}");
                        return false;
                    }
                }
            }

            switch (Config.JumpType)
            {
                case MapAnimStateConfig_TJumpType.End:
                    List<int> animCount = new List<int>();

                    foreach (var item in outputPorts)
                    {
                        var edges = item.GetEdges();

                        if (item.owner == this && edges.Count > 0)
                        {
                            foreach (var nextNode in edges)
                            {
                                if (nextNode.inputPort.owner is MapAnimStateConfigNode nextConfig)
                                {
                                    if (nextConfig.Config.Type == MapAnimStateConfig_PointType.Anim)
                                    {
                                        animCount.Add(nextConfig.Config.ID);
                                    }
                                }
                            }
                        }
                    }

                    if (animCount.Count > 1)
                    {
                        AppendSaveRet($"EndJump不该有多个Anim衔接{Config.ID}..{animCount.Count}");
                        return false;
                    }
                    break;
                case MapAnimStateConfig_TJumpType.Condition:
                case MapAnimStateConfig_TJumpType.ConditionOrEnd:
                    foreach (var item in outputPorts)
                    {
                        var edges = item.GetEdges();

                        if (item.owner == this && edges.Count > 0)
                        {
                            foreach (var nextNode in edges)
                            {
                                if (nextNode.inputPort.owner is MapAnimStateConfigNode nextConfig)
                                {
                                    foreach (var jumpCondition in ConditionJumpType.JumpNexts)
                                    {
                                        if (jumpCondition.NextId == nextConfig.Config.ID &&
                                            nextConfig.Config.JumpType == MapAnimStateConfig_TJumpType.BeHold &&
                                            jumpCondition.Conditional != MapAnimCondition.Default)
                                        {
                                            AppendSaveRet($"{nextConfig.Config.ID}是BeHold, 跳转条件应该是Default(不跳转)");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            return true;
        }

        protected override PortData CustomPortBehavior_ID()
        {
            var result = base.CustomPortBehavior_ID();

            result.acceptMultipleEdges = true;
            return result;
        }

        #region JumpId
        [Output, HideInInspector]
        public int JumpId;

        /// <summary>
        /// SubAction的Port
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        [CustomPortBehavior(nameof(JumpId))]
        public IEnumerable<PortData> SubAction_Behavior(List<SerializableEdge> edges)
        {
            var desc = GetConfigDecription(nameof(JumpId));

            yield return new PortData
            {
                displayName = $"{desc}",
                displayType = typeof(IMapAnimStateConfig),
                identifier = nameof(JumpId),
                acceptMultipleEdges = true,
            };
        }

        /// <summary>
        /// SubAction port数据传输
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="outputPort"></param>
        [CustomPortOutput(nameof(JumpId), typeof(int), true)]
        public void SubAction_OutPut(List<SerializableEdge> edges, NodePort outputPort)
        {
            try
            {
                if (!graph.isEnabled) { return; }

                if (outputPort == null || outputPort.portData == null || edges?.Count <= 0 || Config.JumpId == null) { return; }

                //连线排序
                edges.Sort(SortInputNodes);

                var subActionList = new List<int>();

                foreach (var edge in edges)
                {
                    var inputNode = edge.inputNode;

                    if (inputNode == null) continue;

                    if (inputNode is ConfigBaseNode inputConfigNode)
                        subActionList.Add(inputConfigNode.ID);
                    else if (inputNode is RefConfigBaseNode refConfigNode)
                        subActionList.Add(refConfigNode.ID);
                }

                // 必须加这个判断才能防止错误调用 UpdateCustomData
                if (!Compare(Config.JumpId, subActionList))
                {
                    SetConfigValue(nameof(JumpId), subActionList);

                    if (Config.Values != null && Config.StrValues != null)
                    {
                        UpdateCustomData();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{GetLogPrefix()} SubAction \n{ex}");
            }
        }

        bool Compare(IReadOnlyList<int> a, IReadOnlyList<int> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (int i = 0; i < b.Count; i++)
            {
                if (!a.Contains(b[i]))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 显示节点信息
        [HideInInspector]
        public List<TParamAnnotation> paramsAnn = new List<TParamAnnotation>()
        {
            new TParamAnnotation() { Name = "测试用", },
            new TParamAnnotation() { Name = "测试用", },
        };

        /// <summary>
        /// 重写获取ParamsAnnotation
        /// </summary>
        /// <returns></returns>
        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = TableAnnotation.Inst.GetParamsAnnotation(Config.Type) ?? new ParamsAnnotation("Test");

            switch (Config.Type)
            {
                case MapAnimStateConfig_PointType.Default:
                    break;
                case MapAnimStateConfig_PointType.Anim:
                    break;
                case MapAnimStateConfig_PointType.UpdateRandom:
                    break;
                case MapAnimStateConfig_PointType.MapEvent:
                    SelfDesc = $"{MapEventType.GetDesc()}";
                    break;
                case MapAnimStateConfig_PointType.Root:
                    SelfDesc = $"{Config.UnitAnimType.GetDescription(false)}-{RootType.AnimType.GetDescription(false)}";
                    break;
                case MapAnimStateConfig_PointType.CreateEffect:
                    SelfDesc = $"{CreateEffectType.EffectId}, {CreateEffectType.Destroy.GetAttr<LabelTextAttribute>().Text}";
                    break;
            }

            paramsAnn.FitCount(2);
            paramsAnn[0].Name = SelfDesc;
            paramsAnn[1].Name = Config.JumpType.GetDescription(false);
            var templateParamsAnnotation = Utils.DeepCopyByBinary(baseAnno);
            templateParamsAnnotation.paramsAnn.AddRange(paramsAnn);
            return templateParamsAnnotation;
        }
        #endregion
    }

    [NodeCustomEditor(typeof(MapAnimStateConfigNode))]
    public class MapAnimStateConfigNodeView : ConfigBaseNodeView
    {
        protected override void ToggleCollapse()
        {

        }
    }
}
