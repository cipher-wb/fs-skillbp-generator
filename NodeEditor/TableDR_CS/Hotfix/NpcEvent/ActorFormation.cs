#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;

namespace TableDR
{
    public partial class ActorFormation
    {
        public ActorFormation() { }

        public ActorFormation(int actorIndex, int offsetX, int offsetY, int rotate)
        {
            ActorIndex = actorIndex;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Rotate = rotate;
        }
        
        public ActorFormation Clone()
        {
            return new ActorFormation(ActorIndex, OffsetX, OffsetY, Rotate);
        }
    }
}
#endif
