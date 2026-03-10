using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    [HideInInspector]
    public struct PackedParamsData
    {
        public string paramName;
        public List<TParam> paramList;
    }
    [HideInInspector]
    public struct PackedMembersData
    {
        public Dictionary<string, PropertyInfo> propertyMap;
    }
}
