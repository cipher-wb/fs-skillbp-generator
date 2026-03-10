@echo on
chcp 936
setlocal enabledelayedexpansion

rem 执行目录 F:\MyGod\ClientPublish\DreamRivakes2_U3DProj\Assets\Thirds\NodeEditor\NpcEventEditor\HotUpdate

rem 转表
START /B /WAIT cmd /c "ExcleToByte.bat"

rem 回退到项目根目录 F:\MyGod
cd "../../../../../../../"
set dir=%cd%\

rem 获取私服名
set dstServerName=null
if exist "%dir%私服名称配置.txt" (
   for /f "delims=" %%a in (%dir%私服名称配置.txt) do (
	set /a num+=1
        //获取第num行赋值给变量
	if !num! equ 1 (
	  set dstServerName=data\server\wdfs_%%a\game\resources\tabledata
	)
   ) 
)

if %dstServerName% ==null (
  echo "没有配置要更新的私服名称，无法更新"
)else (

  rem 连接到远程服务器
  rem net use * /del /y
  endlocal
  net use \\192.168.0.95\%dstServerName% "a12J9AY+W" /user:supreme
  setlocal enabledelayedexpansion

  rem 复制并创建文件夹（如果文件夹不存在将会创建文件夹，使用/I参数时，如果share下面有多文件，则默认share为文件夹）
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventPerformanceGroupConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventPerformanceConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventPlayerConditionConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventFormulaConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventConditionConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventConditionGroupConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventLinkConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventActionGroupConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventActionGroupIndexConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventActionConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcTalkGroupConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcTalkConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcTalkOptionConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventExchangeItemConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventBattle.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventDestinyConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventModelConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventActionExtraParamConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcEventLifePathConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventGeneralFuncConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventGeneralFuncGroupConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\SubmitItemGroupConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\NpcTemplateRuleConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\MapEventActorFormationConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\TagConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\ConditionConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestDescConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestEdgeConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\BehaviorConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\ConditionConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestRewards.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestRoleDialogConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestUnitConfig.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K
  xcopy %dir%ClientPublish\DreamRivakes2_U3DProj\Assets\PackRes\Table\QuestEventCheck.bytes \\192.168.0.95\%dstServerName%\ /D /E /Y /H /K

  rem 更新复制到远程服务器表格目录下
  echo %date:~3,13% %time% notifyClient > \\192.168.0.95\%dstServerName%\hot-table.txt

  rem 删除连接
  net use \\192.168.0.95\%dstServerName% /delete

  echo "远程服务器更新成功"
)

pause