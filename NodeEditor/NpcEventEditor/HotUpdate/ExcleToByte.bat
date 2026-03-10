@echo on
rem cd %~dp0
chcp 65001
rem current dir F:\MyGod\ClientPublish\DreamRivakes2_U3DProj\Assets\Thirds\NodeEditor\NpcEventEditor\HotUpdate
set curDir=%~dp0
cd "../../../../../../../Design/Excel/table-tools"
rem enter dir F:\MyGod\Design\Excel\table-tools
set tableToolDir=%cd%

set tableCSharpDataDir=%tableToolDir%\..\..\..\ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table
set tableCSharpCodeDir=%tableToolDir%\..\..\..\ClientPublish\DreamRivakes2_U3DProj\Assets\Scripts\TableDR_CS
set excelDir=%tableToolDir%/../excel


rem CSharp
echo -------------------------------------------------------------------.
echo ---------------generate CSharp code and tables data----------------.
echo -------------------------------------------------------------------.
TableTool.exe -work_path %excelDir% -data_path %tableCSharpDataDir% -log_encoding UTF-8 -export_tables ^
NpcEventConfig^|^
MapEventPerformanceGroupConfig^|^
MapEventPerformanceConfig^|^
NpcEventPlayerConditionConfig^|^
MapEventFormulaConfig^|^
MapEventConditionConfig^|^
MapEventConditionGroupConfig^|^
NpcEventLinkConfig^|^
NpcEventActionGroupConfig^|^
NpcEventActionGroupIndexConfig^|^
NpcEventActionConfig^|^
NpcTalkGroupConfig^|^
NpcTalkConfig^|^
NpcTalkOptionConfig^|^
NpcEventExchangeItemConfig^|^
NpcEventBattle^|^
NpcEventDestinyConfig^|^
NpcEventModelConfig^|^
NpcEventActionExtraParamConfig^|^
NpcEventLifePathConfig^|^
MapEventGeneralFuncConfig^|^
MapEventGeneralFuncGroupConfig^|^
SubmitItemGroupConfig^|^
NpcTemplateRuleConfig^|^
MapEventActorFormationConfig^|^
TagConfig^|^
ConditionConfig^|^
QuestDescConfig^|^
QuestConfig^|^
QuestEdgeConfig^|^
BehaviorConfig^|^
ConditionConfig^|^
QuestRewards^|^
QuestRoleDialogConfig^|^
QuestAtmosphereGroupConfig^|^
QuestDirectorConfig^|^
QuestUnitConfig^|^
QuestEventCheck
cd /d %curDir%