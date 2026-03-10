using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GraphProcessor
{
    public class RefNodeInfo
    {
        public string TableName;
        public string ConfigID;
        public int RefCount;
        public List<string> PathsList;
        
        public RefNodeInfo(string tableName, string configID)
        {
            TableName = tableName;
            ConfigID = configID;
            RefCount = 0;
            PathsList = new List<string>();
        }

        public void AddRef(string path)
        {
            RefCount++;
            PathsList.Add(path);
        }
    }
    
    partial class BaseGraph
    {
        [HideInInspector, System.NonSerialized]
        public long graphChangeFlag = 0;

        [HideInInspector]
        public int curTab;

        [HideInInspector]
        public string path;
        public string FileName { get { return !string.IsNullOrEmpty(path) ? Path.GetFileName(path) : string.Empty; } }

        [System.NonSerialized]
        public StringBuilder SaveRet = new StringBuilder();

        [System.NonSerialized]
        public HashSet<(string, object)> fatalInfos = new HashSet<(string, object)>();

        [System.NonSerialized]
        public Action<string,object> graphFatalCallback;

        [System.NonSerialized]
        public bool isDuringPaste = false;

        public bool AddGraphFatalInfo(string info, object obj = null,bool isSaveRet = true)
        {
            bool isAdd = false;
            if (!fatalInfos.Contains((info,obj)))
            {
                fatalInfos.Add((info, obj));
                isAdd = true;
            }

            if (isSaveRet) 
                SaveRet.AppendLine(info);

            graphFatalCallback?.Invoke(info,obj);
            return isAdd;
        }

        // TODO
        //public string version;

        //public virtual string Name => GetType().Name;

        public virtual void Enable()
        {
            OnEnable();
        }

        public virtual void Disable()
        {
            OnDisable();
        }

        #region 引用节点查询
        public Dictionary<(string tableName, int configID), RefNodeInfo> RefNodeInfoDict { get; protected set; } = new();

        public virtual void UpdateRefNodeInfo()
        {
            
        }

        public virtual List<(string tableName, int configID)> GetRefConfigBaseNodeIds(string jsonString)
        {
            return null;
        }

        public virtual RefNodeInfo GetRefInfo(string tableName, int configID)
        {
            var refNodeInfo = RefNodeInfoDict.GetValueOrDefault((tableName,configID));
            return refNodeInfo;
        }
        #endregion
    }
}
