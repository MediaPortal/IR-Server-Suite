@Echo OFF

REM set paths
set MP1=E:\svnroot\Mediaportal\MediaPortal-1
set Ext=E:\svnroot\Mediaportal\IR Server Suite\External



 Copying MP1 External resources...
 xcopy /i /y "%MP1%\Common-MP-TVE3\Common.Utils\bin\Release\Common.Utils.dll" "%Ext%\"
 xcopy /i /y "%MP1%\mediaportal\Configuration\bin\Release\Core.dll "%Ext%\"
 xcopy /i /y "%MP1%\mediaportal\Configuration\bin\Release\Databases.dll "%Ext%\"
 xcopy /i /y "%MP1%\mediaportal\Configuration\bin\Release\Utils.dll" "%Ext%\"
 xcopy /i /y "%MP1%\mediaportal\Core\bin\Release\DirectShowLib.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\SetupTv\bin\Release\Gentle.Common.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\SetupTv\bin\Release\Gentle.Framework.dll" "%Ext%\"
 xcopy /i /y "%MP1%\mediaportal\Core\bin\Release\MediaPortal.Support.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\TvService\bin\Release\Plugins\PluginBase.dll" "%Ext%\"
 xcopy /i /y "%MP1%\mediaportal\Configuration\bin\Release\RemotePlugins.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\SetupControls\bin\Release\SetupControls.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\SetupTv\bin\Release\TvBusinessLayer.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\TvControl\obj\Release\TvControl.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\SetupTv\bin\Release\TVDatabase.dll" "%Ext%\"
 xcopy /i /y "%MP1%\TvEngine3\TVLibrary\SetupControls\bin\Release\TvLibrary.Interfaces.dll" "%Ext%\"

