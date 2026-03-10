using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_SWITCH_EXECUTE
    {
        // 节点描述记录
        private ParamsAnnotation customAnno;

        // 缓存描述最大描述信息
        private static ParamsAnnotation customAnnoCache;

        // 缓存描述最大描述信息数量-必须双数
        private const int customAnnoCacheMaxCount = 20;

        private bool annoSyncToConfig = false;

        public override void OnNodeCreated()
        {
            base.OnNodeCreated();
            UpdateAllPortsLocal();
            (GetConfig() as SkillEffectConfig).OnParamsChanged += OnConfigChanged;
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
            for (int i = (paramsList.Count - 1); i > 4; i -= 2)
            {
                if (paramsList[i].Value == 0)
                {
                    paramsList.GetListRef().RemoveAt(i);
                    paramsList.GetListRef().RemoveAt(i - 1);
                }
            }
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

        // 更新声明anno
        private bool UpdateAnno(ParamsAnnotation paramsAnnotation = null)
        {
            var isChanged = false;
            var paramsList = GetParamsList();
            var baseAnno = paramsAnnotation; 
            if(baseAnno == null)
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
                        var copyAnno = CreateNewParamAnnotation(i,baseAnno);
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
            int copyAnnoIndex = index % 2 + 2;
            var copyAnno = baseAnno.paramsAnn.ExGet(copyAnnoIndex, null);
            TParamAnnotation copyResult;
            if (copyAnno == null)
            {
                copyResult = new TParamAnnotation();
                Log.Error("TSET_SWITCH_EXECUTE.GetParamsAnnotation failed");
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
                        int copyAnnoIndex = i % 2 + 2;
                        var copyAnno = baseAnno.paramsAnn.ExGet(copyAnnoIndex, null);
                        TParamAnnotation copyResult;
                        if (copyAnno == null)
                        {
                            copyResult = new TParamAnnotation();
                            Log.Error("TSET_SWITCH_EXECUTE.GetParamsAnnotation failed");
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
            if(baseAnno == null)
            {
                return;
            }
            for (int i = 2, length = baseAnno.paramsAnn.Count; i < length; i++)
            {
                var anno = baseAnno.paramsAnn[i];
                
                if (anno.Name.Contains("Case匹配值"))
                {
                    anno.Name = String.Format("Case匹配值{0}", GetShowIndex(i));
                }
                else
                {
                    anno.Name = String.Format("Case效果{0}", GetShowIndex(i));
                }
            }
        }

        private int GetShowIndex(int index)
        {
            return index / 2;
        }
    }
}
