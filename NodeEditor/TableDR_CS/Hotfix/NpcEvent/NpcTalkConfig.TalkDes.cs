#if UNITY_EDITOR
using Funny.Base;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TableDR
{

    public partial class NpcTalkConfig
    {
        public enum TextColor
        {
            [LabelText("请选择")]
            空,
            [LabelText("白")]
            白,
            [LabelText("蓝")]
            蓝,
            [LabelText("紫")]
            紫,
            [LabelText("橙")]
            橙,
            [LabelText("绿")]
            绿,
            [LabelText("红")]
            红,
            [LabelText("褐")]
            褐,
        }

        public enum RichTextType
        {
            [LabelText("物品")]
            物品,
            [LabelText("Npc")]
            NPC,
            [LabelText("方向")]
            方向,
            [LabelText("NPC好感度增加")]
            NPC好感度增加,
            [LabelText("NPC好感度减少")]
            NPC好感度减少,
        }

        [Newtonsoft.Json.JsonIgnore]
        private Dictionary<TextColor, string> colorSelectDict = new Dictionary<TextColor, string>()
        {
            {TextColor.空, ""},
            {TextColor.白,"<color=#57605f>{0}</color>"},
            {TextColor.蓝,"<color=#1f4c7f>{0}</color>"},
            {TextColor.紫,"<color=#643491>{0}</color>"},
            {TextColor.橙,"<color=#944a01>{0}</color>"},
            {TextColor.绿,"<color=#3e6f13>{0}</color>"},
            {TextColor.红,"<color=#c93434>{0}</color>"},
            {TextColor.褐,"<color=#934715>{0}</color>"},
        };

        /// <summary>
        /// 文本选择
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        private Dictionary<RichTextType, string> desTypeSelectDict = new Dictionary<RichTextType, string>
        {
            {RichTextType.物品,"{{物品|{0}}}"},
            {RichTextType.NPC,"{{NPC|{0}}}"},
            {RichTextType.方向,"{方向|1}"},
            {RichTextType.NPC好感度增加,"{NPC好感度增加}"},
            {RichTextType.NPC好感度减少,"{NPC好感度减少}"},
        };

        private const string colorInfoBox = "选中文本后选择颜色，点击\"=\"改变颜色\n";

        /// <summary>
        /// 临时文本 infobox中显示
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        private string InfoBoxDes
        {
            get
            {
                if (SelectDesColor != TextColor.空)
                {
                    return string.Format(colorSelectDict[SelectDesColor], selectionText);
                }

                return selectionText;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private string SelectDesTypeInfo
        {
            get
            {
                return SelectDesType switch
                {
                    RichTextType.物品 => "添加道具编号，点击\"+\"添加到尾部",
                    RichTextType.NPC => "1=玩家、2=当前NPC、其他类型添加NPC编号，点击\"+\"添加到尾部",
                    _ => "点击\"+\"添加到尾部"
                };
            }
        }

        /// <summary>
        /// 选中文本
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        private string selectionText = "";

        [Newtonsoft.Json.JsonIgnore]
        private int selectionSelectedIndex = 0;

        [Newtonsoft.Json.JsonIgnore]
        private int selectionCursorIndex = 0;

        /// <summary>
        /// 气泡文本颜色
        /// </summary>
        [InfoBox("@colorInfoBox+InfoBoxDes")]
        [FoldoutGroup("文本内容"), PropertyOrder(1)]
        [InlineButton("OnChangeColor", "=")]
        [ShowInInspector, LabelText("选择颜色"), Newtonsoft.Json.JsonIgnore]
        public TextColor SelectDesColor = TextColor.空;

        /// <summary>
        /// 文本类型选择
        /// </summary>
        [InfoBox("@SelectDesTypeInfo")]
        [ShowInInspector, LabelText("选择类型"), Newtonsoft.Json.JsonIgnore]
        [FoldoutGroup("文本内容"), PropertyOrder(2)]
        [InlineButton("OnAddDesSection", "+")]
        public RichTextType SelectDesType = RichTextType.物品;

        /// <summary>
        /// 表格选择文本
        /// </summary>
        [FoldoutGroup("文本内容"), PropertyOrder(3)]
        [ShowInInspector, LabelText("选择配置"), Newtonsoft.Json.JsonIgnore, ShowIf("@SelectDesType == RichTextType.物品 || SelectDesType == RichTextType.NPC")]
        [ValueDropdown("OnGetConfigNames", DoubleClickToConfirm = true, AppendNextDrawer = true)]
        public string SelectDescConfig { get; private set; }

        /// <summary>
        /// 添加颜色转换
        /// </summary>
        private void OnChangeColor()
        {
            if (string.IsNullOrEmpty(selectionText)) { return; }

            if (SelectDesColor == TextColor.空)
            {
                return;
            }

            var startIndex = selectionCursorIndex <= selectionSelectedIndex ? selectionCursorIndex : selectionSelectedIndex;
            var endIndex = selectionCursorIndex > selectionSelectedIndex ? selectionCursorIndex : selectionSelectedIndex;

            var getSelectedText = TalkDescEditor.Substring(startIndex, endIndex - startIndex);
            if (getSelectedText != selectionText) { return; }

            var firstDesc = TalkDescEditor.Substring(0, startIndex);
            var endDesc = TalkDescEditor.Substring(endIndex, TalkDescEditor.Length - endIndex);
            TalkDescEditor = firstDesc + InfoBoxDes + endDesc;
        }

        /// <summary>
        /// 添加部分描述
        /// </summary>
        private void OnAddDesSection()
        {
            if (string.IsNullOrEmpty(SelectDescConfig)) { return; }

            switch (SelectDesType)
            {
                case RichTextType.物品:
                case RichTextType.NPC:
                    {
                        TalkDescEditor += string.Format(desTypeSelectDict[SelectDesType], SelectDescConfig);
                    }
                    break;
                case RichTextType.方向:
                case RichTextType.NPC好感度增加:
                case RichTextType.NPC好感度减少:
                    {
                        TalkDescEditor += desTypeSelectDict[SelectDesType];
                    }
                    break;
            }
        }

        /// <summary>
        /// 获取配置表选项
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> OnGetConfigNames()
        {
            if (SelectDesType == RichTextType.物品)
            {
                var items = ItemConfigManager.Instance.ItemArray.Items;
                foreach (var item in items)
                {
                    yield return new ValueDropdownItem($"{item.ID}", item.ID.ToString());
                }
            }
            else if (SelectDesType == RichTextType.NPC)
            {
                var items = NpcConfigManager.Instance.ItemArray.Items;
                foreach (var item in items)
                {
                    yield return new ValueDropdownItem($"{item.ID}", item.ID.ToString());
                }
            }
            else
            {
                yield return new ValueDropdownItem("", "");
            }
        }

        /// <summary>
        /// 获取TextArea内选中的文本
        /// </summary>
        [OnInspectorGUI("OnGUIGetSelectText", append: true)]
        private void OnGUIGetSelectText()
        {
            //获取EditorGUI下的TextEditor https://answers.unity.com/questions/411022/texteditor-in-editor-gui.html
            TextEditor tEditor = typeof(EditorGUI).GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as TextEditor;
            if (tEditor == null) { return; }

            if (tEditor.hasSelection)
            {
                selectionText = tEditor.SelectedText;
                selectionSelectedIndex = tEditor.selectIndex;
                selectionCursorIndex = tEditor.cursorIndex;
            }
        }
    }
}
#endif
