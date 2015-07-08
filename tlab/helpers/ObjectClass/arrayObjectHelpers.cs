//==============================================================================
// Boost! --> SimObject Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================

//==============================================================================
//ArrayObject Creator
function newArrayObject( %name, %target,%superClass ) {
	delObj(%name);
	%obj = new ArrayObject(%name);
	%obj.internalName = %name;
	%obj.superClass = %superClass;
	eval("$"@%name@" = %obj;");

	if (isObject(%target))
		%target.add(%obj);

	return %obj;
}
//------------------------------------------------------------------------------

//==============================================================================
// ArrayObject Manipulation
//==============================================================================
//==============================================================================
// Get the Value of a key for ArrayObject
function ArrayObject::getVal(%this, %key) {
	//Get the index
	%index = %this.getIndexFromKey(%key);
	%value =  %this.getValue(%index);

	return %value;

}
//------------------------------------------------------------------------------
//==============================================================================
// Get the Value of a key for ArrayObject
function ArrayObject::getKeyValue(%this, %value) {
	//Get the index
	%index = %this.getIndexFromValue(%value);
	%key =  %this.getKey(%index);

	return %key;

}
//------------------------------------------------------------------------------
//==============================================================================
// Set the Key/Value for ArrayObject
function ArrayObject::setVal(%this, %key,%value) {
	//Check for index, if not, we will add the new value
	%index = %this.getIndexFromKey(%key);
	%newkey = %this.getKey(%index);
	if (%newkey $="") {
		%this.add(%key,%value);
	} else {
		%this.setValue(%value,%index);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Increment the value of a key (making sure it's numerical)
function ArrayObject::Inc(%this,%key, %inc) {
	loge("ArrayObject::Inc(%this,%key,%inc)",%this,%key,%inc);

	//Make sure we have a numerical value
	if (!strIsNumeric(%inc)) {
		error("Trying to increment a non-numeric value for a DB object");
		return;
	}
	%curVal = %this.getVal(%key);
	%newVal = %curVal + %inc;
	%this.setVal(%key,%newVal);

	return %this.getVal(%key);
}
//------------------------------------------------------------------------------

//==============================================================================
// ArrayObject Debugging
//==============================================================================
//==============================================================================
// Dump Array Content
function ArrayObject::dumpContent(%this) {

	%count = %this.count();
	for(%i = 0; %i< %count; %i++) {
		%key = %this.getKey(%i);
		%value = %this.getValue(%i);
		info("Array dump: Index:",%i,"Key=",%key ,"Value=",%value);
	}

}
//------------------------------------------------------------------------------