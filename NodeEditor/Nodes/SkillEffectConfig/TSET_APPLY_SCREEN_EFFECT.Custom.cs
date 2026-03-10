using System.Collections.Generic;
using System.Security.AccessControl;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_APPLY_SCREEN_EFFECT
    {
        private const string invalidInfo = "无需填写";
        private static readonly Dictionary<int, List<string>> infoMap = new Dictionary<int, List<string>>
        {
            { (int)TScreenEffectType.TSCET_WAVE, new List<string> {
                    "速度" ,
                    "强度" ,
                    "范围" ,
                    "波纹宽度",
                }
            },
            { (int)TScreenEffectType.TSCET_SHOCK_WAVE, new List<string> {
                    "震波类型(0:默认 1:根据时间周期)" ,
                    "速度" ,
                    "强度" ,
                }
            },
            { (int)TScreenEffectType.TSCET_CHARM_GLOD, new List<string> {
                    "颜色-R-默认(255,165,0)" ,
                    "颜色-G" ,
                    "颜色-B" ,
                }
            },
            { (int)TScreenEffectType.TSCET_DARK, new List<string> {
                    "淡入帧数" ,
                    "淡出帧数" ,
                    "整体Alpha-百分比" ,
                }
            },
            { (int)TScreenEffectType.TSCET_CUSTOM_EFFECT, new List<string> {
                    "自定义屏幕特效模型ID" ,
                }
            },
            { (int)TScreenEffectType.TSCET_BLUR, new List<string> {
                    "速度" ,
                }
            },
            { (int)TScreenEffectType.TSCET_RGB_SPLIT, new List<string> {
                    "强度" ,
                    "偏移值" ,
                    "增加偏移速度",
                    "减少偏移速度",
                }
            },
        };
        private static readonly Dictionary<int, List<int>> defaultValueMap = new Dictionary<int, List<int>>
        {
            
        };

        private const int paramsStatrIndex = 6;
        private ParamsAnnotation paramsAnnotation;
        // TODO 改为TableDR消息处理
        private int cacheValue2;
        protected override void Disable()
        {
            base.Disable();
            paramsAnnotation = null;
        }
        protected override void OnConfigChanged()
        {
            var curValue2 = Config?.Params?.ExGet(1)?.Value ?? cacheValue2;
            if (cacheValue2 != curValue2)
            {
                int tmpCacheVal = cacheValue2;
                cacheValue2 = curValue2;
                SyncPortDatas();
                //// 重置默认值
                if (tmpCacheVal != 0 && paramsAnnotation != null && Config.Params != null)
                {
                    defaultValueMap.TryGetValue(cacheValue2, out var defaultVal);
                    for (int i = paramsStatrIndex, length = paramsAnnotation.paramsAnn.Count; i < length; i++)
                    {
                        if (defaultVal != null && (i - paramsStatrIndex) < defaultVal.Count)
                        {
                            Config?.Params.ExGet(i).ExSetValue("Value", defaultVal[i - paramsStatrIndex]);
                        }
                        else
                        {
                            Config?.Params.ExGet(i).ExSetValue("Value", 0);
                        }
                    }
                }
                //TryRepaint();
                //Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
            }
            //这个需要放在最后面，因为上面重置默认值之后需要再次刷新
            base.OnConfigChanged();
        }

        public override ParamsAnnotation GetParamsAnnotation()
        {
            try
            {
                if (paramsAnnotation == null || paramsAnnotation.paramsAnn.Count == 0)
                {
                    var baseAnno = base.GetParamsAnnotation();
                    paramsAnnotation = Utils.DeepCopyByBinary(baseAnno);
                }
                // TODO 可优化效率
                if (paramsAnnotation != null && Config.Params != null)
                {
                    var effectType = Config?.Params.ExGet(2)?.Value ?? 0;
                    infoMap.TryGetValue(effectType, out var customAnn);
                    for (int i = paramsStatrIndex, length = paramsAnnotation.paramsAnn.Count; i < length; i++)
                    {
                        var paramAnn = paramsAnnotation.paramsAnn[i];
                        paramAnn.Name = customAnn?.ExGet(i - paramsStatrIndex, invalidInfo) ?? invalidInfo;
                    }
                }
                return paramsAnnotation;
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
                return base.GetParamsAnnotation();
            }
        }
    }
}
