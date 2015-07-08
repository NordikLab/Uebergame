//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function EditorPlugin::getCfg( %this,%field ) {
	%pattern = "Plugins/"@%this.plugin;
	LabCfg.beginGroup( %pattern, true );
	%value = LabCfg.value(%field);
	LabCfg.endGroup( );
	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Check config and if no value, add the one sent
function EditorPlugin::checkCfg( %this,%field, %defaultValue ) {
	%pattern = "Plugins/"@%this.plugin;
	LabCfg.beginGroup( %pattern, true );
	%value = LabCfg.value(%field);

	if (%value $= "") {
		LabCfg.setValue(  %field,%defaultValue);
		%value = LabCfg.value(%field);
	}

	LabCfg.endGroup( );
	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function EditorPlugin::setCfg( %this,%field,%value,%isDefault ) {
	%pattern = "Plugins/"@%this.plugin;
	LabCfg.beginGroup( %pattern, true );
	LabCfg.setCfg(%field,%value,%isDefault);
	LabCfg.endGroup( );

	if (%isDefault) return;

	%group = strreplace(%pattern,"/","_");
	$Cfg_[%group,%field] = %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function ParamArray::getCfg( %this,%field ) {
	%pattern = strreplace(%this.groupLink,"_","/");
	LabCfg.beginGroup( %pattern, true );
	%value = LabCfg.value(%field);
	LabCfg.endGroup( );
	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function ParamArray::setCfg( %this,%field,%value,%isDefault ) {
	%pattern = strreplace(%this.groupLink,"_","/");
	LabCfg.beginGroup( %pattern, true );
	LabCfg.setCfg(%field,%value,%isDefault);
	LabCfg.endGroup( );

	if (%isDefault) return;

	%group = strreplace(%pattern,"/","_");
	$Cfg_[%group,%field] = %value;
}
//------------------------------------------------------------------------------

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::setCfg( %this,%field,%value,%isDefault ) {
	if ( %isDefault )
		LabCfg.setDefaultValue(  %field,%value);
	else
		LabCfg.setValue(  %field,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function Lab::addDefaultSetting( %this,%field, %default ) {
	%current = LabCfg.value(%field);

	if (%current $= "")
		LabCfg.setValue(%field,%default);
	else
		LabCfg.setValue(%field,%current);

	LabCfg.setDefaultValue(%field,%default);
}
//------------------------------------------------------------------------------

//==============================================================================
// Get the variables list for a pattern
function Lab::getPatternSettings( %this,%pattern ) {
	devLog("getPatternSettings for pattern:",%pattern);
	%nextValue = LabCfg.findFirstValue(%pattern,  true,false );

	while(%nextValue !$= "") {
		%variable = strreplace(%nextValue,"/"," ");
		%variable = getWord(%variable,getWordCount(%variable) - 1);
		//Add variable to the list
		%settingList = trim(%settingList SPC %variable);
		//Check next variable
		%nextValue = LabCfg.findNextValue();
	}

	return %settingList;
}
//------------------------------------------------------------------------------
//==============================================================================
// Get the variables list for a pattern
function LabCfg::dumpPattern( %this,%pattern ) {
	devLog("dumpPattern:",%pattern);
	LabCfg.beginGroup( %pattern, true );
	%nextValue = LabCfg.findFirstValue(%pattern,  true,false );

	while(%nextValue !$= "") {
		devLog("NextValue:",%nextValue);
		%variable = strreplace(%nextValue,"/"," ");
		%variable = getWord(%variable,getWordCount(%variable) - 1);
		%value = LabCfg.value(%variable);
		devLog("Field:",%variable,"Value",%value);
		//Check next variable
		%nextValue = LabCfg.findNextValue();
	}

	LabCfg.endGroup( );
}
//------------------------------------------------------------------------------
//==============================================================================
// Reset the settings from a pattern to default values
function Lab::resetPatternSettings( %this,%pattern,%paramGroup ) {
	//Replace the current settings with default for a pattern
	devLog("resetPatternSettings for pattern:",%pattern);
	%nextValue = LabCfg.findFirstValue(%pattern,  true,false );

	while(%nextValue !$= "") {
		%current = LabCfg.value(%nextValue);
		%default = LabCfg.value(%nextValue@"_default");
		devLog(%nextValue,"Current:",%current,"Default",%default);
		LabCfg.setValue(%nextValue,%default);
		%nextValue = LabCfg.findNextValue();
	}

	if (isObject(%paramGroup )) {
		%this.syncLabSettingsParams(%paramGroup);
	}
}
//------------------------------------------------------------------------------

/*

DefineConsoleMethod(Settings, setValue, void, (const char * settingName, const char * value), (""), "settingObj.setValue(settingName, value);")
{
   StringTableEntry fieldName = StringTable->insert( settingName );

   if (!dStrIsEmpty(value))
      object->setValue( fieldName, value );
   else
      object->setValue( fieldName );
}

DefineConsoleMethod(Settings, setDefaultValue, void, (const char * settingName, const char * value), , "settingObj.setDefaultValue(settingName, value);")
{
   StringTableEntry fieldName = StringTable->insert( settingName );
   object->setDefaultValue( fieldName, value );
}

DefineConsoleMethod(Settings, value, const char*, (const char * settingName, const char * defaultValue), (""), "settingObj.value(settingName, defaultValue);")
{
   StringTableEntry fieldName = StringTable->insert( settingName );

   if (dStrcmp(defaultValue, "") != 0)
      return object->value( fieldName, defaultValue );
   else if (dStrcmp(settingName, "") != 0)
      return object->value( fieldName );

   return "";
}

DefineConsoleMethod(Settings, remove, void, (const char * settingName, bool includeDefaults), (false), "settingObj.remove(settingName, includeDefaults = false);")
{
   // there's a problem with some fields not being removed properly, but works if you run it twice,
   // a temporary solution for now is simply to call the remove twice

	object->remove( settingName, includeDefaults );
	object->remove( settingName, includeDefaults );
}

ConsoleMethod(Settings, write, bool, 2, 2, "%success = settingObj.write();")
{
   TORQUE_UNUSED(argc); TORQUE_UNUSED(argv);
   return object->write();
}

DefineConsoleMethod(Settings, read, bool, (), , "%success = settingObj.read();")
{
   return object->read();
}

DefineConsoleMethod(Settings, beginGroup, void, (const char * groupName, bool includeDefaults), (false), "settingObj.beginGroup(groupName, fromStart = false);")
{
	object->beginGroup( groupName, includeDefaults );
}

DefineConsoleMethod(Settings, endGroup, void, (), , "settingObj.endGroup();")
{
   object->endGroup();
}

DefineConsoleMethod(Settings, clearGroups, void, (), , "settingObj.clearGroups();")
{
   object->clearGroups();
}

DefineConsoleMethod(Settings, getCurrentGroups, const char*, (), , "settingObj.getCurrentGroups();")
{
   return object->getCurrentGroups();
}
DefineConsoleMethod(Settings, findFirstValue, const char*, ( const char* pattern, bool deepSearch, bool includeDefaults ), ("", false, false), "settingObj.findFirstValue();")
{
   return object->findFirstValue( pattern, deepSearch, includeDefaults );
}

DefineConsoleMethod(Settings, findNextValue, const char*, (), , "settingObj.findNextValue();")
{
	return object->findNextValue();
}
*/