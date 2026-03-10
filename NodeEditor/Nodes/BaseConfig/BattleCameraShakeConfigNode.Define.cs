using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDR;

namespace NodeEditor
{
	public partial class BattleCameraShakeConfigNode : ConfigBaseNode<BattleCameraShakeConfig>
	{
        public override bool OnPostProcessing()
        {
            return base.OnPostProcessing() && Config.PostLoadAniCure();
		}

        protected override void OnSave()
        {
            Config.SaveCurveVal();
            base.OnSave();
        }

    }
}
