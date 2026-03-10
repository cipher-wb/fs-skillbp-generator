using System;
using System.Collections.Generic;
using System.Reflection;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_REGISTER_SKILL_EVENT
    {
        // SkillEventConfig索引
        public const int ParamIndex = 0;
        private Dictionary<TParam, TParamValueAttributeDrawer> tParam2AttrDrawer = new Dictionary<TParam, TParamValueAttributeDrawer>();
        int penultimateVal = 0;
        bool reloadLastDropdown = false;

        private bool isTParamValueAttributeDrawerInit = false;

        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);
            
            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config.Params.IndexOf(param);
                    if (index == ParamIndex)
                    {
                        attributes.Add(TParam.VD_EventValue);
                    }
                    break;
            }
        }

        protected override void OnConfigChanged()
        {
            base.OnConfigChanged();
            CheckLastParamDropColections();
        }

        public override void OnSelected()
        {
            InitTParamValueAttributeDrawer();
            CheckLastParamDropColections();
        }

        public override void OnUnSelected()
        {
            DeInitTParamValueAttributeDrawer();
        }

        protected override void Enable()
        {
            base.Enable();
            InitTParamValueAttributeDrawer();
        }

        protected override void Disable()
        {
            base.Disable();
            DeInitTParamValueAttributeDrawer();
        }

        private void InitTParamValueAttributeDrawer()
        {
            if (isTParamValueAttributeDrawerInit)
            {
                return;
            }
            isTParamValueAttributeDrawerInit = true;
            TParamValueAttributeDrawer.OnInit += OnTParamValueAttributeDrawerInit;
            TParamValueAttributeDrawer.OnReloadDropdownCollections += OnReloadDropdownCollections;
        }

        private void DeInitTParamValueAttributeDrawer()
        {
            if (isTParamValueAttributeDrawerInit)
            {
                TParamValueAttributeDrawer.OnInit -= OnTParamValueAttributeDrawerInit;
                TParamValueAttributeDrawer.OnReloadDropdownCollections -= OnReloadDropdownCollections;
                isTParamValueAttributeDrawerInit = false;
            }
        }

        public override bool OnPostProcessing()
        {
            bool ret = base.OnPostProcessing();
            CheckLastParamDropColections();
            return ret;
        }

        private void CheckLastParamDropColections()
        {
            var paramList = GetParamsList();
            if (paramList == null) return;

            int iParamCount = paramList.Count;
            if (iParamCount == 0) return;

            // 倒数第二个
            TParam paramPenultimate = paramList[iParamCount - 2];
            if(penultimateVal!=paramPenultimate.Value)
            {
                if(tParam2AttrDrawer.TryGetValue(paramList[iParamCount - 1],out var attributeDrawer))
                {
                    reloadLastDropdown = true;
                    penultimateVal = paramPenultimate.Value;
                    attributeDrawer.ReloadDropdownCollections();
                }
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            InitTParamValueAttributeDrawer();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            DeInitTParamValueAttributeDrawer();
        }

        void OnTParamValueAttributeDrawerInit(TParamValueAttributeDrawer attributeDrawer)
        {
            TParam param = attributeDrawer.Attribute.Param;
            tParam2AttrDrawer[param] = attributeDrawer;
            var paramList = GetParamsList();
            if (paramList != null && param == paramList[^1])
            {
                CheckLastParamDropColections();
            }
        }

        private void UpdateSkillSlotType()
        {
            var typeT = typeof(TableDR.TSkillSlotType);
            foreach (var enumValue in Enum.GetValues(typeT))
            {
                var type = (TableDR.TSkillSlotType)enumValue;
                var desc = ((int)type).ToString() + "-" + Utils.GetEnumDescription(type);
                CustomEnumUtility.VD_TSkillSlotType_Read.Add(desc,type);
            }
        }

        private void UpdateEntitySubType()
        {
            var typeT = typeof(TableDR.TEntitySubType);
            foreach (var enumValue in Enum.GetValues(typeT))
            {
                var type = (TableDR.TEntitySubType)enumValue;
                var desc = ((int)type).ToString() + "-" + Utils.GetEnumDescription(type);
                CustomEnumUtility.VD_TEntitySubType_Read.Add(desc, type);
            }
        }

        bool OnReloadDropdownCollections(TParamValueAttributeDrawer attributeDrawer)
        {
            // 存在非本节点触发消息 TODO 待查
            if (attributeDrawer.Attribute.ParentValue != this)
            {
                return false;
            }
            TParam param = attributeDrawer.Attribute.Param;
            IReadOnlyList<TParam> paramList = GetParamsList();
            if (paramList == null || paramList.Count == 0)
            {
                return false;
            }
            if (reloadLastDropdown && paramList[ ^1] == param)
            {
                TParam paramPenultimate = paramList[ ^2];
                attributeDrawer.ForceDrawDropdown = true;
                switch (paramPenultimate.Value)
                {
                    case (int)TSkillEventSubType.TSEST_Attribute:
                        {
                            var desc = param.ParamType.GetDescription(false);
                            attributeDrawer.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Read";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {desc}...";
                        }
                        break;
                    case (int)TSkillEventSubType.TSEST_SkillSlot:
                        {
                            attributeDrawer.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TSkillSlotType_Read";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {TSkillEventSubType.TSEST_SkillSlot.GetDescription(false)}...";
                            if (CustomEnumUtility.VD_TSkillSlotType_Read.Count == 0)
                            {
                                UpdateSkillSlotType();
                            }
                        }
                        break;
                    case (int)TSkillEventSubType.TSEST_SkillId:
                        {
                            attributeDrawer.ForceDrawDropdown = false;
                            return false;
                        }
                    case (int)TSkillEventSubType.TSEST_State:
                        {
                            attributeDrawer.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TEntityState_Read";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {TSkillEventSubType.TSEST_State.GetDescription(false)}...";
                        }
                        break;
                    case (int)TSkillEventSubType.TSEST_UNIT_SUBTYPE:
                        {
                            attributeDrawer.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TEntitySubType_Read";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {TSkillEventSubType.TSEST_UNIT_SUBTYPE.GetDescription(false)}...";
                            if (CustomEnumUtility.VD_TEntitySubType_Read.Count == 0)
                            {
                                UpdateEntitySubType();
                            }
                        }
                        break;
                    case (int)TSkillEventSubType.TSEST_OPERATE_TYPE:
                        {
                            attributeDrawer.Attribute.MemberName = $"@TableDR.EnumUtility.VD_TSimulateButtonType";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {TSkillEventSubType.TSEST_OPERATE_TYPE.GetDescription(false)}...";
                        }
                        break;
                    case (int)TSkillEventSubType.TSEST_ENTITY_TYPE:
                        {
                            attributeDrawer.Attribute.MemberName = $"@TableDR.EnumUtility.VD_TEntityType";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {TSkillEventSubType.TSEST_ENTITY_TYPE.GetDescription(false)}...";
                        }
                        break;
                    case (int)TSkillEventSubType.TSEST_UNIT_TYPE:
                        {
                            attributeDrawer.Attribute.MemberName = $"@TableDR.EnumUtility.VD_UnitType";
                            attributeDrawer.Attribute.DropdownTitle = $"请选择 {TSkillEventSubType.TSEST_UNIT_TYPE.GetDescription(false)}...";
                        }
                        break;
                    default:
                        attributeDrawer.ForceDrawDropdown = false;
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
