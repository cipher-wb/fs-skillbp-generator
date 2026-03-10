using System.Collections.Generic;
using Funny.Base.Utils;
using Sirenix.OdinInspector;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public partial class TSET_CHANGE_AI
    {
        // 缓存描述最大描述信息数量
        private const int customAnnoCacheMaxCount = 22;
        // 缓存描述最大描述信息
        private ParamsAnnotation customAnnoCache;
        // 节点描述记录
        private ParamsAnnotation customAnno;

        private AITaskNodeConfig aiTaskConfig = null;

        private int baseParamCount = 2;

        [System.ComponentModel.Description("AI参数列表")]
        [ShowInInspectorAttribute, OnValueChanged("OnUseExtraParamChanged", true), DelayedProperty, LabelText("AI参数列表"), Newtonsoft.Json.JsonProperty]
        public bool UseExtraParam = false;

        public override void OnNodeCreated()
        {
            base.OnNodeCreated();
            CleanInvalidParams();
        }

        [Button("刷新AI参数")]
        public void RefreashAIParam()
        {
            UpdateAnno();
        }

        private void RefreashAIParamNames()
        {
            if (aiTaskConfig != null && aiTaskConfig.SkillTagsList != null && aiTaskConfig.SkillTagsList.Count > 0)
            {
                bool resetName = false;
                int n = customAnno.paramsAnn.Count;
                for (int i = baseParamCount; i < n; i++)
                {
                    SkillTagInfo skillTagInfo = aiTaskConfig.SkillTagsList[i - baseParamCount];
                    var tagsConfig = SkillTagsConfigManager.Instance.GetItem(skillTagInfo.SkillTagConfigID);
                    if (tagsConfig != null)
                    {
                        customAnno.paramsAnn[i].Name = $"{tagsConfig.ID}:{tagsConfig.Desc}";
                        resetName = true;
                    }
                    else
                    {
                        customAnno.paramsAnn[i].Name = $"AI参数{(i - baseParamCount)}";
                    }
                }
                if (resetName)
                {
                    UpdateAllPortsLocal();
                }
            }
        }

        public void OnUseExtraParamChanged()
        {
            if (UseExtraParam == true)
            {
                if (aiTaskConfig == null)
                {
                    FindAITaskConfig();
                }
                if (aiTaskConfig == null)
                {
                    UseExtraParam = false;
                    Debug.LogError("请先设置要切换的AI!");
                }
                UpdateAnno();
                onNodeChanged?.Invoke(nameof(UseExtraParam));
            }
            else
            {
                CleanInvalidParams();
            }
        }

        protected override bool CustomParamsPostProcessing()
        {
            try
            {
                CleanInvalidParams();
                CheckSetDefaultParamVal();
                var paramsList = GetParamsList();
                if (paramsList != null)
                {
                    var curCount = paramsList.Count;
                    for (int i = baseParamCount; i < curCount; i++)
                    {
                        paramsList[i].SetCustomDescription(nameof(TParam.Factor), "技能标签(SkillTagsConfig)配置ID");
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error($"{GetLogPrefix()} 预处理参数失败!\n{ex}");
            }
            return true;
        }

        private void CheckSetDefaultParamVal()
        {
            var paramsList = GetParamsList();
            var anno = GetParamsAnnotation();
            if (paramsList == null || anno == null)
            {
                return;
            }
            var curCount = paramsList.Count;
            var newCount = anno.paramsAnn.Count;
            if (curCount < newCount)
            {
                for (int i = curCount; i < newCount; i++)
                {
                    if (i < baseParamCount)
                    {
                        continue;
                    }
                    // 按照预设默认值创建
                    var paramAnn = anno.paramsAnn[i];
                    var tParam = paramAnn.CopyDefaultParam();
                    if (aiTaskConfig != null && aiTaskConfig.SkillTagsList != null && aiTaskConfig.SkillTagsList.Count > (i - curCount))
                    {
                        tParam.ExSetValue(nameof(tParam.Factor), aiTaskConfig.SkillTagsList[i - curCount].SkillTagConfigID);
                    }
                    tParam.SetCustomDescription(nameof(tParam.Factor), "技能标签(SkillTagsConfig)配置ID");
                    paramsList.GetListRef().Add(tParam);
                }
            }
            else if (curCount > newCount)
            {
                for (int i = curCount - 1; i >= newCount; --i)
                {
                    paramsList.GetListRef().RemoveAt(i);
                }
            }
        }

        protected override void OnConfigChanged()
        {
            UpdateAnno();
            base.OnConfigChanged();
        }

        private void FindAITaskConfig(bool showNotFoundTip = true)
        {
            if (aiTaskConfig != null)
            {
                return;
            }
            int aiConfigId = (int)Config.Params[1].ExGetValue("Value");
            if (aiConfigId == 0)
            {
                return;
            }
            AITaskNodeConfig taskConfig = null;
            var windows = Utils.GetAllWindow<ConfigGraphWindow>();
            foreach (var window in windows)
            {
                var nodeViews = window.GetGraphView().nodeViews;
                foreach (var nodeView in nodeViews)
                {
                    if (nodeView.nodeTarget is AITaskNodeConfigNode configBaseNode)
                    {
                        if (configBaseNode.Config.ID == aiConfigId)
                        {
                            taskConfig = (AITaskNodeConfig)configBaseNode.Config;
                            nodeView.nodeTarget.SyncPortDatas();
                            break;
                        }
                    }
                }
                if (taskConfig != null)
                {
                    break;
                }
            }

            if (taskConfig == null)
            {
                if (showNotFoundTip)
                {
                    EditorUtility.DisplayDialog("错误", $"请打开包含ID{aiConfigId}的AI任务节点的AIGraph以关联额外参数!", "确定");
                }
                else
                {
                    Debug.LogError($"请打开包含ID{aiConfigId}的AI任务节点的AIGraph以关联额外参数!");
                }
                return;
            }
            aiTaskConfig = taskConfig;
        }

        private void UpdateAnno()
        {
            if (aiTaskConfig == null)
            {
                FindAITaskConfig(false);
            }

            var isChanged = false;
            if (customAnno != null)
            {
                int baseCount = baseParamCount;
                if (aiTaskConfig != null && UseExtraParam)
                {
                    baseCount = aiTaskConfig.SkillTagsList.Count + baseParamCount;
                }
                else
                {
                    baseCount = GetParamsList().Count;
                }
                var curAnnoCount = customAnno.paramsAnn.Count;
                if (curAnnoCount > baseCount)
                {
                    customAnno.paramsAnn.RemoveRange(baseCount, curAnnoCount - baseCount);
                    isChanged = true;
                }
                else if (curAnnoCount < baseCount)
                {
                    for (int i = curAnnoCount; i < baseCount; i++)
                    {
                        var copyAnno = customAnnoCache.paramsAnn.ExGet(i, null);
                        if (copyAnno == null)
                        {
                            copyAnno = new TParamAnnotation();
                            Log.Fatal($"参数数量过大，需扩容，可能造成连线断开:{customAnnoCache.paramsAnn.Count}->{curAnnoCount}");
                        }
                        customAnno.paramsAnn.Add(copyAnno);
                        isChanged = true;
                    }
                    CheckSetDefaultParamVal();
                }
                RefreashAIParamNames();
            }

            if (isChanged)
            {
                UpdateAllPortsLocal();
            }
        }

        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = base.GetParamsAnnotation();

            if (baseAnno != null && (customAnno == null || customAnno.paramsAnn == null || customAnno.paramsAnn.Count == 0))
            {
                baseParamCount = baseAnno.paramsAnn.Count;
                // 为保证打开数据未加载，连线断开，预分配
                if (customAnnoCache == null)
                {
                    customAnnoCache = Utils.DeepCopyByBinary(baseAnno);
                    for (int i = 0; i < customAnnoCacheMaxCount; i++)
                    {
                        TParamAnnotation anno = new TParamAnnotation();
                        if (aiTaskConfig == null || i >= aiTaskConfig.SkillTagsList.Count)
                        {
                            anno.Name = $"AI参数{(i + 1)}";
                        }
                        else
                        {
                            SkillTagInfo skillTagInfo = aiTaskConfig.SkillTagsList[i];
                            var tagsConfig = SkillTagsConfigManager.Instance.GetItem(skillTagInfo.SkillTagConfigID);
                            if (tagsConfig != null)
                            {
                                anno.Name = $"{tagsConfig.ID}:{tagsConfig.Desc}";
                            }
                            else
                            {
                                anno.Name = $"AI参数{(i + 1)}";
                            }
                        }
                        customAnnoCache.paramsAnn.Add(anno);
                    }
                }
                customAnno = Utils.DeepCopyByBinary(customAnnoCache);
            }

            var paramsList = GetParamsList();
            if (paramsList != null)
            {
                var curCount = paramsList.Count;
                for (int i = baseParamCount; i < curCount; i++)
                {
                    paramsList[i].SetCustomDescription(nameof(TParam.Factor), "技能标签(SkillTagsConfig)配置ID");
                }
            }

            return customAnno;
        }

        private void CleanInvalidParams()
        {
            var paramsList = GetParamsList();
            int num = 0;
            if (!UseExtraParam)
            {
                num = baseParamCount;
            }
            else
            {
                if (aiTaskConfig == null)
                {
                    num = paramsList.Count;
                }
                else
                {
                    num = aiTaskConfig.SkillTagsList.Count + baseParamCount;
                }
            }

            if (paramsList.Count > num)
            {
                for (int i = (paramsList.Count - 1); i >= num; i--)
                {
                    paramsList.GetListRef().RemoveAt(i);
                }
            }

            UpdateAnno();
        }
    }
}
