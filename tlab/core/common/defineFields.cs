//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
//==============================================================================
function LevelInfo::onDefineFieldTypes( %this ) {
	%this.setFieldType("Desc", "TypeString");
	%this.setFieldType("DescLines", "TypeS32");
}
function SimObject::onDefineFieldTypes( %this ) {
	%this.setFieldType("Locked", "TypeBool");
}