#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;

namespace TableDR
{
    public partial class MapEventTarget
    {
        public MapEventTarget() { }

        public MapEventTarget(MapEventTargetType targetType)
        {
            TargetType = targetType;
        }

        public MapEventTarget(MapEventTargetType targetType, int targetIndex)
        {
            TargetType = targetType;
            TargetIndex = targetIndex;
        }

        public void Update(MapEventTargetType targetType, int targetIndex)
        {
            TargetType = targetType;
            TargetIndex = targetIndex;
        }

        public MapEventTarget Clone()
        {
            return new MapEventTarget(TargetType, TargetIndex);
        }

        [JsonIgnore]
        public bool IsShowTargetIndex => TargetType == MapEventTargetType.MapEventTargetType_SpecificActor;

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<MapEventTargetType> VDL_Type1= new ValueDropdownList<MapEventTargetType>
        {
            {"玩家", MapEventTargetType.MapEventTargetType_Player},
            {"主演", MapEventTargetType.MapEventTargetType_Leader},
            {"指定演员", MapEventTargetType.MapEventTargetType_SpecificActor},
            {"当前演员", MapEventTargetType.MapEventTargetType_MineActor },
        };

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<MapEventTargetType> VDL_Type2 = new ValueDropdownList<MapEventTargetType>
        {
            {"玩家", MapEventTargetType.MapEventTargetType_Player},
            {"主演", MapEventTargetType.MapEventTargetType_Leader},
            {"所有配角", MapEventTargetType.MapEventTargetType_AllCostar},
            {"指定演员", MapEventTargetType.MapEventTargetType_SpecificActor},
            {"当前演员", MapEventTargetType.MapEventTargetType_MineActor },
        };

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<MapEventTargetType> VDL_Type3 = new ValueDropdownList<MapEventTargetType>
        {
            {"主演", MapEventTargetType.MapEventTargetType_Leader},
            {"所有配角", MapEventTargetType.MapEventTargetType_AllCostar},
            {"指定演员", MapEventTargetType.MapEventTargetType_SpecificActor},
            {"当前演员", MapEventTargetType.MapEventTargetType_MineActor },
        };

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<MapEventTargetType> VDL_Type4 = new ValueDropdownList<MapEventTargetType>
        {
            {"当前演员", MapEventTargetType.MapEventTargetType_MineActor},
            {"玩家", MapEventTargetType.MapEventTargetType_Player},
            {"主演", MapEventTargetType.MapEventTargetType_Leader},
            {"所有配角", MapEventTargetType.MapEventTargetType_AllCostar},
            {"指定演员", MapEventTargetType.MapEventTargetType_SpecificActor},
        };

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<MapEventTargetType> VDL_Type5 = new ValueDropdownList<MapEventTargetType>
        {
            {"主演", MapEventTargetType.MapEventTargetType_Leader},
            {"指定演员", MapEventTargetType.MapEventTargetType_SpecificActor},
        };
    }
}
#endif
