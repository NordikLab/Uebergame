//==============================================================================
// Boost! --> SimObject Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================

//==============================================================================
//ScripObject Creator
function newScriptObject( %name, %target,%superClass,%class ) {
	delObj(%name);
	%obj = new ScriptObject(%name);
	%obj.internalName = %name;
	%obj.superClass = %superClass;
	if (%class !$= "")
			%obj.class = %class;
	eval("$"@%name@" = %obj;");

	if (isObject(%target))
		%target.add(%obj);

	return %obj;
}
//------------------------------------------------------------------------------

//==============================================================================
// ScriptObject Manipulation
//==============================================================================
//==============================================================================
// SetVal -> Set a field value for a type
function ScriptObject::setVal(%this, %field,%value ,%type) {
	if (%type $="")
		%type = "Default";

	%this.setFieldType(%field, %type);
	%this.setFieldValue(%field, %value);

	return true;

}
//------------------------------------------------------------------------------
//==============================================================================
//GetVal -> Return the value of field
function ScriptObject::getVal(%this, %field) {
	if (%type $="")
		%type = "Default";

	%return = %this.getFieldValue(%field);
	return %return;
}
//------------------------------------------------------------------------------
//==============================================================================
//GetType -> Return field type of obj field
function ScriptObject::getType(%this, %field) {
	if (%type $="")
		%type = "Default";

	%return = %this.getFieldType(%field, %type);
	return %return;

}
//------------------------------------------------------------------------------

/*
assignFieldsFrom(SimObject fromObject)	SimObject
assignPersistentId()	SimObject
call(string method, string args...)	SimObject
canSave	SimObject
canSaveDynamicFields	SimObject
class	SimObject
className	SimObject
clone()	SimObject
deepClone()	SimObject
delete()	SimObject
dump(bool detailed=false)	SimObject
dumpClassHierarchy()	SimObject
dumpGroupHierarchy()	SimObject
dumpMethods()	SimObject
getCanSave()	SimObject
getClassName()	SimObject
getClassNamespace()	SimObject
getDebugInfo()	SimObject
getDeclarationLine()	SimObject
getDynamicField(int index)	SimObject
getDynamicFieldCount()	SimObject
getField(int index)	SimObject
getFieldCount()	SimObject
getFieldType(string fieldName)	SimObject
getFieldValue(string fieldName, int index=-1)	SimObject
getFilename()	SimObject
getGroup()	SimObject
getId()	SimObject
getInternalName()	SimObject
getName()	SimObject
getSuperClassNamespace()	SimObject
hidden	SimObject
internalName	SimObject
isChildOfGroup(SimGroup group)	SimObject
isEditorOnly()	SimObject
isExpanded()	SimObject
isField(string fieldName)	SimObject
isInNamespaceHierarchy(string name)	SimObject
isMemberOfClass(string className)	SimObject
isMethod(string methodName)	SimObject
isNameChangeAllowed()	SimObject
isSelected()	SimObject
locked	SimObject
name	SimObject
onAdd(SimObjectId ID)	ScriptObject
onRemove(SimObjectId ID)	ScriptObject
parentGroup	SimObject
persistentId	SimObject
save(string fileName, bool selectedOnly=false, string preAppendString="")	SimObject
schedule(float time, string method, string args...)	SimObject
setCanSave(bool value=true)	SimObject
setClassNamespace(string name)	SimObject
setEditorOnly(bool value=true)	SimObject
setFieldType(string fieldName, string type)	SimObject
setFieldValue(string fieldName, string value, int index=-1)	SimObject
setFilename(string fileName)	SimObject
setHidden(bool value=true)	SimObject
setInternalName(string newInternalName)	SimObject
setIsExpanded(bool state=true)	SimObject
setIsSelected(bool state=true)	SimObject
setLocked(bool value=true)	SimObject
setName(string newName)	SimObject
setNameChangeAllowed(bool value=true)	SimObject
setSuperClassNamespace(string name)	SimObject
superClass	SimObject	*/
