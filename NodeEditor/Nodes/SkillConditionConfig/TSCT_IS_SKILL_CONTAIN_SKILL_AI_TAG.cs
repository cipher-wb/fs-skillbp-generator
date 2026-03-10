using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    public partial class TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG
    {
        // 节点描述记录
        private ParamsAnnotation customAnno;

        // 缓存描述最大描述信息
        private static ParamsAnnotation customAnnoCache;

        // 缓存描述最大描述信息数量
        private const int customAnnoCacheMaxCount = 20;

        private bool annoSyncToConfig = false;

        public override void OnNodeCreated()
        {
            base.OnNodeCreated();
            UpdateAllPortsLocal();
            (GetConfig() as SkillConditionConfig).OnParamsChanged += OnConfigChanged;
            RemoveInvalidPatam();
        }

        private void RemoveInvalidPatam()
        {
            //清理无效的
            var paramsList = GetParamsList();
            if (paramsList == null || paramsList.Count == 0)
            {
                return;
            }
            for (int i = (paramsList.Count - 1); i >= 3; i--)
            {
                if (paramsList[i].Value == 0)
                {
                    ((List<TParam>)paramsList).RemoveAt(i);
                }
            }
        }

        public override bool OnPostProcessing()
        {
            bool ret = base.OnPostProcessing();
            (GetConfig() as SkillConditionConfig).OnParamsChanged += OnConfigChanged;
            return ret;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            (GetConfig() as SkillConditionConfig).OnParamsChanged -= OnConfigChanged;
        }

        protected override void OnConfigChanged()
        {
            UpdateAnno();
            base.OnConfigChanged();
        }

        // 更新声明anno
        private bool UpdateAnno(ParamsAnnotation paramsAnnotation = null)
        {
            var isChanged = false;
            var paramsList = GetParamsList();
            var baseAnno = paramsAnnotation;
            if (baseAnno == null)
            {
                baseAnno = GetParamsAnnotation();
            }
            if (baseAnno != null && paramsList != null)
            {
                var paramsCount = paramsList.Count;
                var curAnnoCount = baseAnno.paramsAnn.Count;
                if (curAnnoCount > paramsCount)
                {
                    baseAnno.paramsAnn.RemoveRange(paramsCount, curAnnoCount - paramsCount);
                    isChanged = true;
                }
                else if (curAnnoCount < paramsCount)
                {
                    for (int i = curAnnoCount; i < paramsCount; i++)
                    {
                        var copyAnno = CreateNewParamAnnotation(i, baseAnno);
                        baseAnno.paramsAnn.Add(copyAnno);
                        isChanged = true;
                    }
                }
            }
            if (isChanged)
            {
                RefreshNameAnnoName();
                UpdateAllPortsLocal();
            }
            return isChanged;
        }

        private TParamAnnotation CreateNewParamAnnotation(int index, ParamsAnnotation baseAnno)
        {
            int copyAnnoIndex = 2;
            var copyAnno = baseAnno.paramsAnn.ExGet(copyAnnoIndex, null);
            TParamAnnotation copyResult;
            if (copyAnno == null)
            {
                copyResult = new TParamAnnotation();
                Log.Error("TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG.GetParamsAnnotation failed");
            }
            else
            {
                copyResult = Utils.DeepCopyByBinary(copyAnno);
            }
            copyResult.Name = String.Format(copyResult.Name, GetShowIndex(index));
            return copyResult;
        }

        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = base.GetParamsAnnotation();
            IReadOnlyList<TParam> paramList = GetParamsList();
            if (baseAnno != null && (customAnno == null || customAnno.paramsAnn == null || customAnno.paramsAnn.Count == 0))
            {
                annoSyncToConfig = false;
                if (customAnnoCache == null)
                {
                    customAnnoCache = Utils.DeepCopyByBinary(baseAnno);
                    for (int i = 0; i < customAnnoCacheMaxCount; i++)
                    {
                        int copyAnnoIndex = 2;
                        var copyAnno = baseAnno.paramsAnn.ExGet(copyAnnoIndex, null);
                        TParamAnnotation copyResult;
                        if (copyAnno == null)
                        {
                            copyResult = new TParamAnnotation();
                            Log.Error("TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG.GetParamsAnnotation failed");
                        }
                        else
                        {
                            copyResult = Utils.DeepCopyByBinary(copyAnno);
                        }
                        customAnnoCache.paramsAnn.Add(copyResult);
                    }
                }
                customAnno = Utils.DeepCopyByBinary(customAnnoCache);
                RefreshNameAnnoName();
            }

            if (!annoSyncToConfig && paramList != null && paramList.Count > 0)
            {
                UpdateAnno(customAnno);
                annoSyncToConfig = true;
            }
            //else if (paramList == null)
            //{
            //    var paramName = GetParamsName();
            //    paramList = new List<TParam>();
            //    for (int i = 0, count = baseAnno.paramsAnn.Count; i < count; i++)
            //    {
            //        // 按照预设默认值创建
            //        var paramAnn = baseAnno.paramsAnn[i];
            //        var tParam = paramAnn.CopyDefaultParam();
            //        paramList.Add(tParam);
            //    }
            //    Config.ExSetValue(paramName, paramList);
            //}
            return customAnno;
        }

        private void RefreshNameAnnoName()
        {
            var baseAnno = GetParamsAnnotation();
            if (baseAnno == null)
            {
                return;
            }
            for (int i = 2, length = baseAnno.paramsAnn.Count; i < length; i++)
            {
                var anno = baseAnno.paramsAnn[i];
                if (anno.Name.Contains("技能AI标签"))
                {
                    anno.Name = String.Format("技能AI标签{0}", GetShowIndex(i));
                }
            }
        }

        private int GetShowIndex(int index)
        {
            return index - 1;
        }
    }
}
