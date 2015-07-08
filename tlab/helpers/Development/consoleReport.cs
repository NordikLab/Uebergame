//==============================================================================
// GameLab Helpers -> Console Reporting helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$pref::Dev::TraceLogLevel = 5;
$pref::Console::ShowNotes = 1;
$pref::Console::ShowInfos = 1;
$pref::Dev::ReportLevel = 1;

//==============================================================================
// Advanced echo functions that display up to 9 arguments in color
//------------------------------------------------------------------------------
// -> They all use a way to enable/disable console reporting
//==============================================================================

//==============================================================================
// Global arguments color setup (related to the console profile text colors)
//==============================================================================
$LogCol[1] = "\c1-\c1";
$LogCol[2] = "\c1-\c2";
$LogCol[3] = "\c1-\c3";
$LogCol[4] = "\c1-\c4";
$LogCol[5] = "\c1-\c5";
$LogCol[6] = "\c1-\c6";
$LogCol[7] = "\c1-\c7";
$LogCol[8] = "\c1-\c8";
$LogCol[9] = "\c1-\c9";
$LogCol[10] = "\c1-\c0";
$LogCol[11] = "\c1-\c1";
$LogCol[12] = "\c1-\c2";
$LogColMax = 12;
//------------------------------------------------------------------------------


//==============================================================================
// Info Log - Show official game informations to the console
//------------------------------------------------------------------------------
// Enable/Disable using $pref::Console::ShowInfos
//==============================================================================
function info(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
   //Exit if info console reporting	is disabled
	if (!$pref::Console::ShowInfos) return;  
   %echo = "\c2->\c5" SPC %text;	   
   %i = 1;	   
	while(%a[%i] !$="") {		
		%echo = %echo SPC $LogCol[%i] SPC %a[%i];
		%i++;
	}		
   echo(%echo);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Note Log - Info Log Copy that can be use for custom needs
//------------------------------------------------------------------------------
// Enable/Disable using $pref::Console::ShowNotes
//==============================================================================
function note(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
   //Exit if info console reporting	is disabled
	if (!$pref::Console::ShowNotes) return;  
	
   %echo = "\c4->\c3" SPC %text;	   
   %i = 1;	   
	while(%a[%i] !$="") {		
		%echo = %echo SPC $LogCol[%i] SPC %a[%i];
		%i++;
	}		
   echo(%echo);
}
//------------------------------------------------------------------------------

//==============================================================================
// Trace Log - Basic logging with 5 level used to trace the code when needed
//------------------------------------------------------------------------------
// Set reporting trace log level with $pref::Console::TraceLogLevel
//  Level 0 --> No trace log reported to console
//  Level 1 --> log() reported
//  Level 2 --> log()+ loga() reported
//  Level 3 --> log()+ loga()+ logc() reported
//  Level 4 --> log()+ loga()+ logc()+ logd() reported
//  Level 5 --> log()+ loga()+ logc()+ logd()+ loge() reported
//==============================================================================

//==============================================================================
//Log Tracer report system (With predefined levels of reports)
//==============================================================================
//==============================================================================
// Log Tracer - Base level of log report
function log(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::TraceLogLevel <= 0)
		return;

	%echo = "\c3log->\c4" SPC %text;
	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------
//==============================================================================
// Log Tracer - First Level of log report
function loga(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::TraceLogLevel <= 1)
		return;

	%echo = "\c3log\c1A\c3->\c4" SPC %text;
	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------
//==============================================================================
// Log Tracer - Second Level of log report
function logb(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::TraceLogLevel <= 2)
		return;

	%echo = "\c3log\c1B\c3->\c4" SPC %text;
	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------
//==============================================================================
// Log Tracer - Third Level of log report
function logc(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::TraceLogLevel <= 3)
		return;

	%echo = "\c3log\c1C\c3->\c4" SPC %text;
	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------
//==============================================================================
// Log Tracer - Fourth Level of log report
function logd(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::TraceLogLevel <= 4)
		return;

	%echo = "\c5log\c7D\c2->\c5" SPC %text;
	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------
//==============================================================================
// Log Tracer - Fifth Level of log report
function loge(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::TraceLogLevel <= 5)
		return;

	%echo = "\c1log\c5E\c6->\c9" SPC %text;
	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------

//==============================================================================
//DevLog: Show a log to the console for development
function devLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if ($pref::Console::DevLogLevel <= 0)
		return;

	%echo = "\c3DLOG==>\c4" SPC %text;

	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);

}
//------------------------------------------------------------------------------
//==============================================================================
//DevLog: Show a log to the console for development
function mouseLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if (!$pref::Console::MouseLog)
		return;
   devLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12);
}
//------------------------------------------------------------------------------
//==============================================================================
//DevLog: Show a log to the console for development
function mouseDragLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if (!$pref::Console::MouseDragLog)
		return;
   if ($Sim::Time -  $MouseDragLogLast < $pref::Console::MouseDragLogDelay ) return;
   
   $MouseDragLogLast = $Sim::Time;
   
   devLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12);
}
//------------------------------------------------------------------------------
//==============================================================================
//DevLog: Show a log to the console for development
function mouseMoveLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	if (!$pref::Console::MouseMoveLog)
		return;
   if ($Sim::Time -  $MouseMoveLogLast < $pref::Console::MouseMoveLogDelay ) return;
   
   $MouseMoveLogLast = $Sim::Time;
   
   devLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12);
}
//------------------------------------------------------------------------------
//==============================================================================
//WarnLog: Show a warning log about something wrong
function warnLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {

	%echo = "\c3Warning==>\c4" SPC %text;

	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------

//==============================================================================
//WarnLog: Show a warning log about something wrong
function debugLog(%text,%a1, %a2, %a3, %a4,%a5, %a6, %a7, %a8,%a9, %a10, %a11, %a12) {
	
	if (!isDebugBuild())
		return;
	%echo = "\c3Debug Log==>\c4" SPC %text;

	%i = 1;
	while(%a[%i] !$="") {
		%id = %i;
		if (%id > 9) %id = %id - 9;
		%echo = %echo SPC $LogCol[%id] SPC %a[%i];
		%i++;
	}
	echo(%echo);
}
//------------------------------------------------------------------------------


// Writes out all script functions to a file.
function writeOutFunctions() {
	new ConsoleLogger(logger, "scriptFunctions.txt", false);
	dumpConsoleFunctions();
	logger.delete();
}

// Writes out all script classes to a file.
function writeOutClasses() {
	new ConsoleLogger(logger, "scriptClasses.txt", false);
	dumpConsoleClasses();
	logger.delete();
}
//------------------------
//Display all value of object in console
function logObj(%obj,%prefix, %dynamicOnly) {
	if (!%dynamicOnly) {
		%count = %obj.getFieldCount();
		for(%i=0; %i<%count; %i++) {
			%field = %obj.getField(%i);
			%value = %obj.getFieldValue(%field);
		}
	}
	%count = %obj.getDynamicFieldCount();
	for(%i=0; %i<%count; %i++) {
		%field = %obj.getDynamicField(%i);
		%value = %obj.getFieldValue(%field);
	}
}
