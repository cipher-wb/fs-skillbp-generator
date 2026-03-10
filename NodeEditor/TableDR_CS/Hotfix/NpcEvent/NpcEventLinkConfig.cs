#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Funny.Base.Utils;
using Sirenix.Utilities;

namespace TableDR
{
    public partial class NpcEventLinkConfig
    {
        private int randomPointID;
        [ShowInInspector, DelayedProperty, LabelText("随机点坐标"), JsonProperty, 
            ValueDropdownAttribute("GetRandomPointItems"), OnValueChanged("OnActorParamChanged")]
        public int RandomPointID
        {
            get
            {
                return ActorParam?.Count > 0 ? ActorParam[0] : 0;
            }
            set
            {
                randomPointID = value;
            }
        }

        private int relativeCoordX;
        [ShowInInspector, DelayedProperty, LabelText("相对坐标X"), JsonProperty, OnValueChanged("OnActorParamChanged")]
        public int RelativeCoordX
        {
            get
            {
                return ActorParam?.Count > 0 ? ActorParam[1] : 0;
            }
            set
            {
                relativeCoordX = value;
            }
        }

        private int relativeCoordY;
        [ShowInInspector, DelayedProperty, LabelText("相对坐标Y"), JsonProperty, OnValueChanged("OnActorParamChanged")]
        public int RelativeCoordY
        {
            get
            {
                return ActorParam?.Count > 0 ? ActorParam[2] : 0;
            }
            set
            {
                relativeCoordY = value;
            }
        }

        private int screenRange;
        [ShowInInspector, DelayedProperty, LabelText("搜索范围屏幕数"), JsonProperty, OnValueChanged("OnActorParamChanged")]
        public int ScreenRange
        {
            get
            {
                return ActorParam?.Count > 0 ? ActorParam[3] : 0;
            }
            set
            {
                screenRange = value;
            }
        }

        /// <summary>
        /// 随机时间间隔
        /// </summary>
        public void OnActorParamChanged()
        {
            ActorParam ??= new List<int>();
            ActorParam.GetListRef().Clear();

            if(randomPointID == 0) { return; }

            ActorParam.GetListRef().Add(randomPointID);
            ActorParam.GetListRef().Add(relativeCoordX);
            ActorParam.GetListRef().Add(relativeCoordY);
            ActorParam.GetListRef().Add(screenRange);
        }

        private IEnumerable<ValueDropdownItem> GetRandomPointItems()
        {
            var items = MapRandomPointManager.Instance.ItemArray;
            if(items?.Items?.Count > 0)
            {
                foreach (var item in items.Items)
                {
                    yield return new ValueDropdownItem($"{item.ID}-{item.Name}", item.ID);
                }
            }
        }
    }
}
#endif
