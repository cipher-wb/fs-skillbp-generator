using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    [Flags]
    public enum TemplateParamsChangeType
    {
        None = 0,
        AddOrRename = 1 << 0,
        Remove = 1 << 1,
        ChangeType = 1 << 2,
        ChangeDefaultParams = 1 << 3,
    }

    [Flags]
    public enum TemplateFlagsType
    {
        [LabelText("默认模板-外显")]
        None = 0,

        [LabelText("子模版-列表不显示")]
        IsChildTemplate = 1 << 0,

        [LabelText("不常用")]
        IsNotCommonlyUsed = 1 << 1,

        [LabelText("Marker编辑器使用")]
        MarkerEditor = 1 << 2,
    }

    /// <summary>
    /// 节点分类标记，用于节点，模板简易分类
    /// </summary>
    [Flags]
    public enum NodeTypeFlags
    {
        [LabelText("无")]
        None = 0,

        [LabelText("不常用")]
        IsNotCommonlyUsed = 1 << 0,
    }

    public partial class ConfigBaseNode
    {
        protected Action<string> onNodeChanged;
        public event Action<string> OnNodeChanged
        {
            add { onNodeChanged -= value; onNodeChanged += value; }
            remove { onNodeChanged -= value; }
        }

        protected Action<string,string> onConfigNodeChanged;
        public event Action<string,string> OnConfigNodeChanged
        {
            add { onConfigNodeChanged -= value; onConfigNodeChanged += value; }
            remove { onConfigNodeChanged -= value; }
        }
        /// <summary>
        /// 部分节点默认描述添加处理
        /// </summary>
        public static string GetNodeViewDesc(object config, string baseDesc)
        {
            if (config == null) return baseDesc;

            string viewDesc = null;
            string customDesc = null;
            if (!string.IsNullOrEmpty(baseDesc))
            {
                viewDesc = baseDesc;
            }
            try
            {
                switch (config)
                {
                    case SkillConfig skillConfig:
                        customDesc = $"[技能名:{skillConfig.SkillNameEditor}]_[类型:{skillConfig.SkillSubType.GetDescription(false)}]";
                        if (skillConfig.SkillTagsList?.Count > 0)
                        {
                            foreach (var tag in skillConfig.SkillTagsList)
                            {
                                customDesc += $"\n[参数]{tag.GetNodeViewDesc()}";
                            }
                        }
                        if (skillConfig.SkillDamageTagsList?.Count > 0)
                        {
                            foreach (var tag in skillConfig.SkillDamageTagsList)
                            {
                                customDesc += $"\n[伤害参数]{tag.GetNodeViewDesc()}";
                            }
                        }
                        if (skillConfig.SkillTipsConditionSkillTagsList?.Count > 0)
                        {
                            foreach (var tag in skillConfig.SkillTipsConditionSkillTagsList)
                            {
                                customDesc += $"\n[心法参数]{tag.GetNodeViewDesc()}";
                            }
                        }
                        break;
                    case SkillTagsConfig skillTagsConfig:
                        customDesc = $"[默认:{skillTagsConfig.DefaultValue}] {skillTagsConfig.Desc}";
                        break;
                    case BuffConfig buffConfig:
                        customDesc = null;
                        if (!string.IsNullOrEmpty(buffConfig.BuffNameEditor))
                        {
                            customDesc += $"[{buffConfig.BuffNameEditor}]";
                        }
                        if (buffConfig.AddState != TEntityState.TEST_NULL)
                        {
                            customDesc += $"[{buffConfig.AddState.GetDescription(false)}]";
                        }
                        if (buffConfig.LastTime == -1)
                        {
                            customDesc += $"[永久]";
                        }
                        else if (buffConfig.LastTime > 0)
                        {
                            customDesc += $"[{buffConfig.LastTime}帧]";
                        }
                        if (buffConfig.Attrs?.Count > 0)
                        {
                            foreach (var attr in buffConfig.Attrs)
                            {
                                if (attr == null)
                                {
                                    continue;
                                }
                                customDesc += $"\n{attr.GetNodeViewDesc()}";
                            }
                        }
                        if (buffConfig.SkillTags?.Count > 0)
                        {
                            foreach (var tag in buffConfig.SkillTags)
                            {
                                if (tag == null)
                                {
                                    continue;
                                }
                                customDesc += $"\n{tag.GetNodeViewDesc()}";
                            }
                        }
                        break;
                    case BattleUnitConfig battleUnitConfig:
                        if (battleUnitConfig.RoleCommonProperty != null)
                        {
                            var unitName = battleUnitConfig.RoleCommonProperty.LastName + battleUnitConfig.RoleCommonProperty.FirstName;
                            if (unitName.Length > 0)
                            {
                                customDesc += $"[{unitName}]";
                            }
                        }
                        break;
                    case ModelConfig modelConfig:
                        if (!string.IsNullOrEmpty(modelConfig.ModelPath))
                        {
                            customDesc += $"[{modelConfig.ModelPath}]";
                        }
                        break;
                    case BehaviorConfig behaviorConfig:
                        // TODO 多语言编辑器下显示问题处理，导表模板修改
                        if (!string.IsNullOrEmpty(behaviorConfig.Type))
                        {
                            customDesc += $"[{behaviorConfig.Type}]";
                            if (!string.IsNullOrEmpty(behaviorConfig.Loc1Editor))
                            {
                                int legnth = Math.Min(15, behaviorConfig.Loc1Editor.Length);
                                customDesc += $"\n[{behaviorConfig.Loc1Editor.Substring(0, legnth)}]";
                            }
                        }
                        break;
                    case TextConfig textConfig:
                        {
                            if (!string.IsNullOrEmpty(textConfig.TextEditor))
                            {
                                int legnth = Math.Min(15, textConfig.TextEditor.Length);
                                customDesc += $"[{textConfig.TextEditor.Substring(0, legnth)}]";
                            }
                            break;
                        }
                    case BattleAIConfig battleAIConfig:
                        {
                            if (!string.IsNullOrEmpty(battleAIConfig.AIName))
                            {
                                customDesc += $"[{battleAIConfig.AIName}]";
                            }
                            if (battleAIConfig.AISkillTagsList?.Count > 0)
                            {
                                foreach (var tag in battleAIConfig.AISkillTagsList)
                                {
                                    customDesc += $"\n[参数]{tag.GetNodeViewDesc()}";
                                }
                            }
                            break;
                        }
                    case SkillInterruptConfig skillInterruptConfig:
                        {
                            if (skillInterruptConfig.SkillTagID?.Value > 0)
                            {
                                customDesc += $"[{Utils.GetDescription(skillInterruptConfig, nameof(skillInterruptConfig.SkillTagID))}_{skillInterruptConfig.SkillTagID.Value}]";
                            }
                            break;
                        }
                    case SkillTargetCondTemplateConfig skillTargetCondTemplateConfig:
                        {
                            if (!string.IsNullOrEmpty(skillTargetCondTemplateConfig.Name))
                            {
                                customDesc += $"[{skillTargetCondTemplateConfig.Name}]";
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
            }
            if (!string.IsNullOrEmpty(customDesc))
            {
                if (!string.IsNullOrEmpty(viewDesc))
                {
                    viewDesc += "\n";
                }
                viewDesc += customDesc;
            }
            return viewDesc;
        }
    }
    public partial class ConfigBaseNode<T>
    {
        public override string name => GetConfigName(); // 节点名
        public override bool isRenamable => true;       // 可重命名_暂时
        public override bool needsInspector => true;    // 面板上显示
        public override Color color { get { return TableAnnotation.Inst.GetNodeColor(ConfigType); } }   // 节点颜色
        public virtual bool ReadOnly => false;          // 表格数据是否只读

        [Output("参数引用列表")]
        public PackedParamsData PackedParamsOutput;     // 参数列表节点端口，如：SkillEffectConfig.Params，主要用于技能编辑器效果，条件，选择节点

        /// <summary>
        /// 参数节点列表
        /// </summary>
        [Output("成员列表")]
        public PackedMembersData PackedMembersOutput;    // 成员变量节点端口，如：SkillConfig.SkillEffectExecuteInfo等

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, HideLabel, ShowIf("@this.BaseTransData != null")]
        [FoldoutGroup("参数设置", true)]
        [OnValueChanged("@OnChangedTransData($property)", true)]
        public ConfigBaseTransData BaseTransData { get; protected set; }

        //[DelayedProperty]
        [Sirenix.OdinInspector.ShowInInspector, FoldoutGroup("表格数据", true), HideLabel, HideReferenceObjectPicker, OnValueChanged("@OnConfigChanged()", true), EnableIf("@!ReadOnly && this.BaseTransData == null")]
        [InfoBox("@infoMessage", InfoMessageType.Info, "IsVisbileInfoMessage")]
        [InfoBox("@infoMessage", InfoMessageType.Error, "IsVisbileErrorMessage")]
        //[NonSerialized] 改成属性，方便odin后处理修改标签
        public T Config { get; set; } = new T();              // 表格对象

        [HideInInspector, NonSerialized]
        protected Type ConfigType = typeof(T);  // 表格类型

        [HideInInspector, SerializeField]
        protected string TableTash;             // 表格版本记录

        [HideInInspector, SerializeField]
        protected string ConfigJson;            // 表格json格式记录

        [HideInInspector, SerializeField]
        protected string Config2ID;             // 表格名_ID记录，方便ID统计,如：SkillConfig_1360000

        protected bool IsPostProcessed = false;
        protected InfoMessageType infoMessageType = InfoMessageType.Info;
        protected string infoMessage;
        protected bool IsVisbileInfoMessage { get { return !string.IsNullOrEmpty(infoMessage) && infoMessageType == InfoMessageType.Info; } }
        protected bool IsVisbileErrorMessage { get { return !string.IsNullOrEmpty(infoMessage) && infoMessageType == InfoMessageType.Error; } }

        /// <summary>
        /// 缓存ID数据，用作节点刷新
        /// </summary>
        protected int cacheID;

        private MethodInfo configDeserialize;
        protected MethodInfo ConfigDeserialize 
        { 
            get 
            {
                if (configDeserialize == null)
                {
                    configDeserialize = ConfigType.GetMethod("DeserializeBytes", BindingFlags.Static | BindingFlags.Public);
                }
                return configDeserialize; 
            }
        }
        private MethodInfo configSerialize;
        protected MethodInfo ConfigSerialize 
        { 
            get 
            {
                if (configSerialize == null)
                {
                    configSerialize = ConfigType.GetMethod("SerializeToBytes", BindingFlags.Static | BindingFlags.Public);
                }
                return configSerialize; 
            }
        }
        protected List<PortData> paramsPortDataCache;

        protected List<PortData> membersPortDataCache;
        /// <summary>
        /// 自定义节点信息
        /// </summary>
        protected virtual List<PortData> CustomPortDatas => new List<PortData>();
        protected virtual void OnChangedTransData(InspectorProperty property)
        {
            if (BaseTransData != null)
            {
                BaseTransData.CheckError();
                BaseTransData.ToConfig();
            }
        }

        private IEnumerator enumeratorRepaint;

        private static JsonSerializerSettings defaultJsonSetting = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Include/*IgnoreAndPopulate*/,
            NullValueHandling = NullValueHandling.Ignore,
        };
        private JsonSerializerSettings customJsonSetting = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Include/*IgnoreAndPopulate*/,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new JsonConverter[] { new CustomJsonConverter<T>() },
            //ContractResolver = new ForceMutableContractResolver(),
        };
    }
}
