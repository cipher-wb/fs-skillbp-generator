using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TableDR;

namespace NodeEditor
{
	public class NpcEventActionAttribute : Attribute
	{
		public NpcEventActionConfig_TEventActionType NpcEventActionType;

		public NpcEventActionAttribute(NpcEventActionConfig_TEventActionType actionType)
		{
			this.NpcEventActionType = actionType;
		}
	}

	
}