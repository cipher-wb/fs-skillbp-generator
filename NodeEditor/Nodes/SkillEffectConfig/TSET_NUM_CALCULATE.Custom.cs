using System.Collections.Generic;
using System.Text;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_NUM_CALCULATE
    {
        // 缓存描述最大描述信息数量-必须双数
        private const int customAnnoCacheMaxCount = 22;
        // 缓存描述最大描述信息
        private static ParamsAnnotation customAnnoCache;
        // 节点描述记录
        private ParamsAnnotation customAnno;
        // 信息记录
        private static StringBuilder sbInfo = new StringBuilder();

        public override void OnNodeCreated()
        {
            base.OnNodeCreated();
            (GetConfig() as SkillEffectConfig).OnParamsChanged += OnConfigChanged;
            CleanInvalidParams();
            UpdateAllPortsLocal();
        }
        public override bool OnPostProcessing()
        {
            bool ret = base.OnPostProcessing();
            (GetConfig() as SkillEffectConfig).OnParamsChanged += OnConfigChanged;
            return ret;
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            (GetConfig() as SkillEffectConfig).OnParamsChanged -= OnConfigChanged;
        }
        protected override void OnConfigChanged()
        {
            UpdateAnno();
            base.OnConfigChanged();
        }
        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = base.GetParamsAnnotation();
            if (baseAnno != null && (customAnno == null || customAnno.paramsAnn == null || customAnno.paramsAnn.Count == 0))
            {
                // 为保证打开数据未加载，连线断开，预分配
                if (customAnnoCache == null)
                {
                    customAnnoCache = Utils.DeepCopyByBinary(baseAnno);
                    for (int i = 0; i < customAnnoCacheMaxCount; i++)
                    {
                        int copyAnnoIndex = i % 2 + 1;
                        var copyAnno = baseAnno.paramsAnn.ExGet(copyAnnoIndex, null);
                        TParamAnnotation copyResult;
                        if (copyAnno == null)
                        {
                            copyResult = new TParamAnnotation();
                            Log.Error("TSET_NUM_CALCULATE.GetParamsAnnotation failed");
                        }
                        else
                        {
                            copyResult = Utils.DeepCopyByBinary(copyAnno);
                        }
                        customAnnoCache.paramsAnn.Add(copyResult);
                    }
                    for (int i = 0, length = customAnnoCache.paramsAnn.Count; i < length; i++)
                    {
                        var anno = customAnnoCache.paramsAnn[i];
                        anno.Name += GetShowIndex(i);
                    }
                }
                customAnno = Utils.DeepCopyByBinary(customAnnoCache);
            }
            return customAnno;
        }
        private int GetShowIndex(int index)
        {
            return index % 2 == 0 ? index / 2 + 1 : (index - 1) / 2 + 1;
        }
        protected override bool CustomParamsPostProcessing()
        {
            try
            {
                CleanInvalidParams();
                // TODO 需注意参数顺序改变问题
                var paramsList = GetParamsList();
                var anno = GetParamsAnnotation();
                if (paramsList == null || anno == null)
                {
                    return true;
                }
                UpdateAnno();

                #region 更新参数预分配
                var curCount = paramsList.Count;
                var newCount = anno.paramsAnn.Count;
                if (paramsList.Count < anno.paramsAnn.Count)
                {
                    for (int i = curCount; i < newCount; i++)
                    {
                        // 按照预设默认值创建
                        var paramAnn = anno.paramsAnn[i];
                        var tParam = paramAnn.CopyDefaultParam();
                        paramsList.GetListRef().Add(tParam);
                    }
                }
                else if (paramsList.Count > anno.paramsAnn.Count)
                {
                    for (int i = paramsList.Count - 1; i >= anno.paramsAnn.Count; --i)
                    {
                        paramsList.GetListRef().RemoveAt(i);
                    }
                }
                #endregion
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error($"{GetLogPrefix()} 预处理参数失败!\n{ex}");
            }
            return true;
        }
        protected override void OnSave()
        {
            CleanInvalidParams();
            base.OnSave();
        }
        // 清理无效的参数信息，仅清理0的数据
        private void CleanInvalidParams()
        {
            var paramsList = GetParamsList();
            sbInfo.Length = 0;
            if (paramsList != null)
            {
                for (int i = paramsList.Count - 1; i >= 3; --i)
                {
                    var tParam = paramsList[i];
                    if (tParam == null)
                    {
                        sbInfo.AppendLine($"参数{GetShowIndex(i)}为空: 清理");
                        paramsList.GetListRef().RemoveAt(i);
                        continue;
                    }
                    if (i % 2 == 0)
                    {
                        var tParamOpr = paramsList[i - 1];
                        if (tParam.Value == 0 && tParam.ParamType == TableDR.TParamType.TPT_NULL &&
                            tParamOpr.Value == 0 && tParam.ParamType == TableDR.TParamType.TPT_NULL)
                        {
                            paramsList.GetListRef().RemoveAt(i);
                            paramsList.GetListRef().RemoveAt(i - 1);
                            sbInfo.AppendLine($"运算符{GetShowIndex(i)}为空: 清空{i}, {i - 1}参数");
                        }
                        --i;
                    }
                    else
                    {
                        paramsList.GetListRef().RemoveAt(i);
                        sbInfo.AppendLine($"运算符{GetShowIndex(i)}为空: 清空{i}-末尾运算符参数");
                    }
                }
            }
            if (sbInfo.Length > 0)
            {
                Log.Info($"{GetLogPrefix()}清理无效参数,详情如下:\n{sbInfo}");
                sbInfo.Length = 0;
            }
            UpdateAnno();
        }
        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                var paramsList = GetParamsList();
                if (paramsList != null)
                {
                    for (int i = paramsList.Count - 1; i >= 0; --i)
                    {
                        var tParam = paramsList[i];
                        if (tParam == null)
                        {
                            continue;
                        }
                        // 特殊处理，可能存在数值运算作为获取数据的做法，默认第一个运算符不做判断
                        if (i == 2)
                        {
                            continue;
                        }
                        if (i % 2 == 0 && i > 0)
                        {
                            var tParamOpr = paramsList[i - 1];
                            if (tParamOpr.ParamType != TableDR.TParamType.TPT_NULL ||
                                tParamOpr.Value == (int)TableDR.TNumOperators.TNO_NULL)
                            {
                                AppendSaveRet($"运算符{GetShowIndex(i)}为空，配置错误");
                                ret = false;
                            }
                        }
                    }
                }
            }
            return ret;
        }
        // 更新声明anno
        private bool UpdateAnno()
        {
            var isChanged = false;
            var paramsList = GetParamsList();
            if (customAnno != null && paramsList != null)
            {
                var baseCount = paramsList.Count;
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
                        var copyAnno = customAnnoCache.paramsAnn.ExGet(curAnnoCount, null);
                        if (copyAnno == null)
                        {
                            copyAnno = new TParamAnnotation();
                            Log.Fatal($"参数数量过大，需扩容，可能造成连线断开:{customAnnoCache.paramsAnn.Count}->{curAnnoCount}");
                        }
                        customAnno.paramsAnn.Add(copyAnno);
                        isChanged = true;
                    }
                }
            }
            if (isChanged)
            {
                UpdateAllPortsLocal();
            }
            return isChanged;
        }
    }
}
