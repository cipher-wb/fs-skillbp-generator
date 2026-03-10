#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Funny.Base.Utils;

namespace TableDR
{
    public partial class NpcTalkConfig
    {
        private int fixIntervalSecs;
        [ShowInInspector, DelayedProperty, LabelText("间隔时间-固定"), SuffixLabel("秒"), JsonProperty, OnValueChanged("OnFixIntervalSec")]
        public int FixIntervalSecs
        {
            get
            {
                return IntervalSecs?.Count > 0 ? IntervalSecs[0] : 0;
            }
            set
            {
                fixIntervalSecs = value;
            }
        }

        /// <summary>
        /// 编辑器使用
        /// </summary>
        private int randomIntervalSecMin;
        [ShowInInspector, DelayedProperty, LabelText("间隔时间-随机-最小"), SuffixLabel("秒"), JsonProperty, OnValueChanged("OnRandomIntervalSec")]
        public int RandomIntervalSecMin
        {
            get 
            {
                return IntervalSecs?.Count > 0 ? IntervalSecs[0] : 0;
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
                return IntervalSecs?.Count > 1 ? IntervalSecs[1] : 0;
            }
            set
            {
                randomIntervalSecMax = value;
            }
        }

        /// <summary>
        /// 固定时间间隔
        /// </summary>
        public void OnFixIntervalSec()
        {
            IntervalSecs ??= new List<int>();
            IntervalSecs.GetListRef().Clear();

            RandomIntervalSecMin = 0;
            RandomIntervalSecMax = 0;

            IntervalSecs.GetListRef().Add(fixIntervalSecs);
        }

        /// <summary>
        /// 随机时间间隔
        /// </summary>
        public void OnRandomIntervalSec()
        {
            IntervalSecs ??= new List<int>();
            IntervalSecs.GetListRef().Clear();

            FixIntervalSecs = 0;

            IntervalSecs.GetListRef().Add(randomIntervalSecMin);
            IntervalSecs.GetListRef().Add(randomIntervalSecMax);
        }
    }
}
#endif
