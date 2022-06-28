@ECHO off

SETLOCAL EnableDelayedExpansion

ECHO Available plugins:
CD "%~dp0Plugins\"
FOR /D  %%a IN (*.*) DO ECHO %%a

SET /P plugin="Select Plugin: "

ECHO %cd%

IF NOT EXIST "%cd%\%plugin%\" (
	ECHO Invalid plugin path 
	PAUSE
	EXIT /b
)

CD "%cd%\%plugin%\Source"

FOR /D %%a IN (*.*) DO ECHO %%a

ECHO Available modules:

SET /P moduleName="Module Name: "

IF EXIST "%cd%\%moduleName%" (
	SET /P removeDir="The directory already exists. Do you want to delete it and it's whole content to generate default? (Y/N) "
	IF !removeDir!==Y (
		@RD /S /Q "%cd%\%moduleName%"
		MKDIR "%cd%\%moduleName%"
	)
) ELSE (
	MKDIR "%cd%\%moduleName%"
)

SET buildCsFilePath="%cd%\%moduleName%\%moduleName%.Build.cs"

IF EXIST !buildCsFilePath! (
	SET /P removeBuildCS="!moduleName!.Build.cs does already exist. Do you want to overwrite it with the default? (Y/N) "
	IF !removeBuildCS!==Y (
		DEL /Q %buildCsFilePath%
		CALL :createBuildCsFile buildCsFilePath moduleName
	)
) ELSE (
	CALL :createBuildCsFile buildCsFilePath moduleName
)

SET publicFolderPath="%cd%\%moduleName%\Public"
SET privateFolderPath="%cd%\%moduleName%\Private"

SET headerFilePath=!publicFolderPath!\!moduleName!.h
SET /A headerFileGenerated=0

IF EXIST !publicFolderPath! (
	SET /P removePublicFolder="!publicFolderPath! does already exist. Do you want to remove the folder and it's content and generate default? (Y/N) "
	IF !removePublicFolder!==Y (
		@RD /S /Q !publicFolderPath!
		MKDIR !publicFolderPath!
		CALL :createModuleHeader headerFilePath moduleName
		SET /A headerFileGenerated=1
	)
) ELSE (
	MKDIR !publicFolderPath!
	CALL :createModuleHeader headerFilePath moduleName
	SET /A headerFileGenerated=1
)

IF !headerFileGenerated!==0 (
	ECHO Generate header file
	IF EXIST !headerFilePath! (
		SET /P removeHeaderFile="!headerFilePath! does already exist. Do you want to overwrite the existing file and generate the default one? (Y/N) "
		DEL /Q !headerFilePath!
		CALL :createModuleHeader headerFilePath moduleName
	) ELSE (
		ECHO !headerFilePath!
		CALL :createModuleHeader headerFilePath moduleName
	)
)

SET cppFilePath=!privateFolderPath!\!moduleName!.cpp
SET /A cppFileGenerated=0

IF EXIST !privateFolderPath! (
	SET /P removePublicFolder="!privateFolderPath! does already exist. Do you want to remove the folder and it's content and generate default? (Y/N) "
	IF !removePublicFolder!==Y (
		@RD /S /Q !privateFolderPath!
		MKDIR !privateFolderPath!
		CALL :createModuleCpp cppFilePath moduleName
		SET /A cppFileGenerated=1
	)
) ELSE (
	MKDIR !privateFolderPath!
	CALL :createModuleCpp cppFilePath moduleName
	SET /A cppFileGenerated=1
)

IF !cppFileGenerated!==0 (
	IF EXIST !cppFilePath! (
		SET /P removeCppFile="!cppFilePath! does already exist. Do you want to overwrite the existing file and generate the default one? (Y/N) "
		IF !removeCppFile!==Y (
			DEL /Q !cppFilePath!
			CALL :createModuleCpp cppFilePath moduleName
		)
	) ELSE (
		CALL :createModuleCpp cppFilePath moduleName
	)
)

ENDLOCAL

PAUSE

EXIT /b


REM *********  createBuildCsFile function *************************************************************
:createBuildCsFile <filepath> <moduleName>

SETLOCAL EnableDelayedExpansion

SET "TAB=	"

ECHO //Copyright . All rights reserved>>!%~1!
ECHO.>>!%~1!
ECHO using UnrealBuildTool;>>!%~1!
ECHO.>>!%~1!
ECHO public class !%~2! : ModuleRules>>!%~1!
ECHO {>>!%~1!
ECHO !TAB!public !%~2!(ReadOnlyTargetRules Target) : base(Target)>>!%~1!
ECHO !TAB!{>>!%~1!
ECHO !TAB!!TAB!PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;>>!%~1!
ECHO.>>!%~1!
ECHO !TAB!!TAB!PublicIncludePaths.AddRange(>>!%~1!
ECHO !TAB!!TAB!!TAB!new string[]>>!%~1!
ECHO !TAB!!TAB!!TAB!{>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!// ... add public include paths required here ...>>!%~1!
ECHO !TAB!!TAB!!TAB!}>>!%~1!
ECHO !TAB!!TAB!!TAB!);>>!%~1!
ECHO.>>!%~1!
ECHO.>>!%~1!
ECHO !TAB!!TAB!PrivateIncludePaths.AddRange(>>!%~1!
ECHO !TAB!!TAB!!TAB!new string[]>>!%~1!
ECHO !TAB!!TAB!!TAB!{>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!// ... add other private include paths required here ...>>!%~1!
ECHO !TAB!!TAB!!TAB!}>>!%~1!
ECHO !TAB!!TAB!!TAB!);>>!%~1!
ECHO.>>!%~1!
ECHO.>>!%~1!
ECHO !TAB!!TAB!PublicDependencyModuleNames.AddRange(>>!%~1!
ECHO !TAB!!TAB!!TAB!new string[] {>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!"Core",>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!// ... add other public dependencies that you statically link with here ...>>!%~1!
ECHO !TAB!!TAB!!TAB!}>>!%~1!
ECHO !TAB!!TAB!!TAB!);>>!%~1!
ECHO.>>!%~1!
ECHO.>>!%~1!
ECHO !TAB!!TAB!PrivateDependencyModuleNames.AddRange(>>!%~1!
ECHO !TAB!!TAB!!TAB!new string[]>>!%~1!
ECHO !TAB!!TAB!!TAB!{>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!"CoreUObject",>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!"Engine",>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!"Slate",>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!"SlateCore",>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!// ... add private dependencies that you statically link with here ...>>!%~1!
ECHO !TAB!!TAB!!TAB!}>>!%~1!
ECHO !TAB!!TAB!!TAB!);>>!%~1!
ECHO.>>!%~1!
ECHO.>>!%~1!
ECHO !TAB!!TAB!DynamicallyLoadedModuleNames.AddRange(>>!%~1!
ECHO !TAB!!TAB!!TAB!new string[]>>!%~1!
ECHO !TAB!!TAB!!TAB!{>>!%~1!
ECHO !TAB!!TAB!!TAB!!TAB!// ... add any modules that your module loads dynamically here ...>>!%~1!
ECHO !TAB!!TAB!!TAB!}>>!%~1!
ECHO !TAB!!TAB!!TAB!);>>!%~1!
ECHO !TAB!}>>!%~1!
ECHO }>>!%~1!

ENDLOCAL

EXIT /b


REM *********  createModuleHeader function *************************************************************
:createModuleHeader <filepath> <moduleName>

SETLOCAL EnableDelayedExpansion

SET "TAB=	"

ECHO //Copyright . All rights reserved>>!%~1!
ECHO.>>!%~1!
ECHO #pragma once>>!%~1!
ECHO.>>!%~1!
ECHO #include "CoreMinimal.h">>!%~1!
ECHO #include "Modules/ModuleManager.h">>!%~1!
ECHO.>>!%~1!
ECHO class F!%~2!Module : public IModuleInterface>>!%~1!
ECHO {>>!%~1!
ECHO public:>>!%~1!
ECHO.>>!%~1!
ECHO !TAB!/** IModuleInterface implementation */>>!%~1!
ECHO !TAB!virtual void StartupModule() override;>>!%~1!
ECHO !TAB!virtual void ShutdownModule() override;>>!%~1!
ECHO };>>!%~1!

ENDLOCAL

EXIT /b

REM *********  createModuleCpp function *************************************************************
:createModuleCpp <filepath> <moduleName>

SETLOCAL EnableDelayedExpansion

SET "TAB=	"

ECHO //Copyright . All rights reserved>>!%~1!
ECHO.>>!%~1!
ECHO #include "!%~2!.h">>!%~1!
ECHO.>>!%~1!
ECHO #define LOCTEXT_NAMESPACE "!%~2!Module">>!%~1!
ECHO.>>!%~1!
ECHO void F!%~2!Module^:^:StartupModule()>>!%~1!
ECHO {>>!%~1!
ECHO !TAB!// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file.>>!%~1!
ECHO }>>!%~1!
ECHO.>>!%~1!
ECHO void F!%~2!Module^:^:ShutdownModule()>>!%~1!
ECHO {>>!%~1!
ECHO !TAB!// This function may be called during shutdown to clean up your module. ^For modules that support dynamic reloading.>>!%~1!
ECHO !TAB!// we ^call this function before unloading the module.>>!%~1!
ECHO }>>!%~1!
ECHO.>>!%~1!
ECHO #undef LOCTEXT_NAMESPACE>>!%~1!
ECHO.>>!%~1!
ECHO IMPLEMENT_MODULE(F!%~2!Module, !%~2!)>>!%~1!

EXIT /b