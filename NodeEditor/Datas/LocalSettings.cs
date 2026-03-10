using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NodeEditor
{
    [Serializable]
    public class LocalSettings : ISetting
    {
        [Flags, ShowInInspector]
        public enum RolePosition
        {
            [ShowInInspector, LabelText("访客")]
            访客 = 0,
            [ShowInInspector, LabelText("程序")]
            程序 = 1 << 0,
            [ShowInInspector, LabelText("策划")]
            策划 = 1 << 1,
            [ShowInInspector, LabelText("美术")]
            美术 = 1 << 2,
        }
        public const string name = "【本地配置信息】";
        private static LocalSettings inst;
        public static LocalSettings Inst
        {
            get
            {
                try
                {
                    if (inst == null)
                    {
                        if (!File.Exists(Constants.SkillEditor.PathLocalSettings))
                        {
                            inst = CreateDefault(true);
                        }
                        else
                        {
                            var content = File.ReadAllText(Constants.SkillEditor.PathLocalSettings);
                            inst = JsonConvert.DeserializeObject<LocalSettings>(content);
                            //Log.Debug($"{name} 初始化本地配置 ：{Constants.SkillEditor.PathLocalSettings}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                    inst = CreateDefault(false);
                }
                return inst;
            }
        }

        [InfoBox("$WarningInfo", InfoMessageType.Warning)]
        [LabelText("使用者-姓名"), BoxGroup(name)]
        public string Name;

        private string warningInfo;
        public string WarningInfo
        {
            get
            {
                if (warningInfo == null)
                {
                    var ip = Utils.IP(Utils.AddressType.IPv4);
                    var ip_end = Utils.IP_End();
                    warningInfo =
                        $"\n本地IPv4: {ip}" +
                        $"\n分配ID段规则：" +
                        $"  按照本机IP，取末尾段作为节点起始ID，避免不同同事编辑器导致ID冲突问题\n" +
                        $"  如：本机IP: {ip}, 对应ID : {ip_end}\n";
                }
                return warningInfo;
            }
        }

        [LabelText("分配 ID 段"), BoxGroup(name), PropertyRange(0, 255), InlineButton("ResetID", "重置ID为IP末段")]
        public int ID;

        [LabelText("岗位"), BoxGroup(name)]
        public RolePosition Role;

        /// <summary>
        /// 创建默认配置
        /// </summary>
        private static LocalSettings CreateDefault(bool write)
        {
            Log.Info($"LocalSettings.CreateDefault({write}) 创建默认本地配置");
            var ip = Convert.ToInt32(Utils.IP_End());
            inst = new LocalSettings()
            {
                ID = ip,
            };
            if (write)
            {
                inst.SaveSetting(null);
            }
            return inst;
        }

        private void ResetID()
        {
            ID = Convert.ToInt32(Utils.IP_End());
        }

        //[Button("保存配置", ButtonSizes.Large), PropertyOrder(-1)]
        public bool SaveSetting(StringBuilder saveInfo)
        {
            try
            {
                var fileInfo = new FileInfo(PathSetting);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                var content = JsonConvert.SerializeObject(inst, Formatting.Indented);
                File.WriteAllText(fileInfo.FullName, content);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            return false;
        }

        public string PathSetting => Constants.SkillEditor.PathLocalSettings;
        public List<string> PathCommitSetting => new List<string> { PathSetting };
        public static bool IsProgramer()
        {
            return Inst.Role.HasFlag(RolePosition.程序);
        }
    }
}
