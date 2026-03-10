#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Funny.Base.Utils;

namespace TableDR
{
    public partial class NpcTalkGroupConfig
    {
        /// <summary>
        /// 编辑器使用
        /// </summary>
        private int randomIntervalSecMin;
        [ShowInInspector, DelayedProperty, LabelText("间隔时间-随机-最小"), SuffixLabel("秒"), JsonProperty, OnValueChanged("OnRandomIntervalSec")]
        public int RandomIntervalSecMin
        {
            get 
            {
                return RandomIntervalSecs?.Count > 0 ? RandomIntervalSecs[0] : 0;
            }
            set
            {
                randomIntervalSecMin = value;
            }
        }

        /// <summary>
        /// 编辑器使用
        /// </summary>
        private int randomIntervalSecMax;
        [ShowInInspector, DelayedProperty, LabelText("间隔时间-随机-最大"), SuffixLabel("秒"), JsonProperty(), OnValueChanged("OnRandomIntervalSec")]
        public int RandomIntervalSecMax
        {
            get
            {
                return RandomIntervalSecs?.Count > 1 ? RandomIntervalSecs[1] : 0;
            }
            set
            {
                randomIntervalSecMax = value;
            }
        }

        public void OnRandomIntervalSec()
        {
            RandomIntervalSecs ??= new List<int>();
            RandomIntervalSecs.GetListRef().Clear();

            RandomIntervalSecs.GetListRef().Add(randomIntervalSecMin);
            RandomIntervalSecs.GetListRef().Add(randomIntervalSecMax);

            
        }
    }
}
#endif
