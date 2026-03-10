using System.Collections.Generic;
using System.Security.AccessControl;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_APPLY_ENTITY_EFFECT
    {
        private const string invalidInfo = "无需填写";
        private static readonly Dictionary<int, List<string>> infoMap = new Dictionary<int, List<string>>
        {
            { (int)TEntityEffectType.TEET_COLOR_ALPHA, new List<string> {
                    "透明度-Alpha（0~255）" ,
                    "淡入帧数",
                    "淡出帧数",
                }
            },
            { (int)TEntityEffectType.TEET_COLOR_RGB, new List<string> {
                    "颜色-R（0~255）",
                    "颜色-G（0~255）" ,
                    "颜色-B（0~255）",
                }
            },
            { (int)TEntityEffectType.TEET_HUE, new List<string> {
                    "FresnelPower（万分比值）",
                    "FresnelColor-R（万分比值）",
                    "FresnelColor-G（万分比值）" ,
                    "FresnelColor-B（万分比值）",
                    "FresnelColor-A（万分比值）",
                }
            },
            { (int)TEntityEffectType.TEET_DISSOLVE, new List<string> {
                    "曲线索引",
                }
            },
            { (int)TEntityEffectType.TEET_FROZEN, new List<string> {
                    "冰冻变化时间（帧数）",
                    "冰冻变化上下数值（-1,0,1)",
                    "冰冻变化左右数值（-1,0,1)",
                }
            },
            { (int)TEntityEffectType.TEFT_EMISSION, new List<string> {
                    "颜色-R（0~255）",
                    "颜色-G（0~255）" ,
                    "颜色-B（0~255）",
                    "颜色-A（0~255）",
                }
            },
            { (int)TEntityEffectType.TEFT_BURNING, new List<string> {
                    "颜色-R（0~255）",
                    "颜色-G（0~255）" ,
                    "颜色-B（0~255）",
                    "颜色-A（0~255）",
                    "FresnelsScale-百分比",
                    "FresnelBianyuan-百分比",
                }
            },
            { (int)TEntityEffectType.TEFT_PHANTOM, new List<string> {
                    "截取帧画面间隔（毫秒）",
                    "帧画面消失时间（毫秒）",
                    "帧画面淡出延迟（毫秒）",
                    "帧画面初始Aplha(百分比)",
                    "是否需要换材质球",
                    "静止不动时是否持续"
                }
            },
            { (int)TEntityEffectType.TEFT_EXTERNAL_LIGHT, new List<string> {
                    "颜色-R（0~255）",
                    "颜色-G（0~255）" ,
                    "颜色-B（0~255）",
                    "颜色-A（0~255）",
                }
            },
            { (int)TEntityEffectType.TEFT_BONE_IK, new List<string> {
                    "目标实例ID",
                    "是否开启IK" ,
                    "目标点X（无目标单位生效）" ,
                    "目标点Y（无目标单位生效）" ,
                }
            },
            { (int)TEntityEffectType.TEFT_FADE, new List<string> {
                    "目标Alpha（0~255）",
                }
            },
            { (int)TEntityEffectType.TEFT_OUTLINE, new List<string> {
                    "颜色-R（0~255）",
                    "颜色-G（0~255）" ,
                    "颜色-B（0~255）",
                    "颜色-I（0~255）",
                    "描边宽度-百分比",
                }
            },
            { (int)TEntityEffectType.TEFT_UV2_ANIM, new List<string> {
                    "起始UV2值-百分比（-100~100）",
                    "目标UV2值-百分比（-100~100）",
                    "动画时长-帧" ,
                }
            },
        };
        private static readonly Dictionary<int, List<int>> defaultValueMap = new Dictionary<int, List<int>>
        {
            // 燃烧颜色的默认值
            { (int)TEntityEffectType.TEFT_BURNING, new List<int> {
                    238, // R
                    68 , // G
                    6,   // B
                    0,   // A
                    130, // FresnelsScale-百分比
                    10,  // FresnelBianyuan-百分比
                }
            }
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
                    var effectType = Config?.Params.ExGet(1)?.Value ?? 0;
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
