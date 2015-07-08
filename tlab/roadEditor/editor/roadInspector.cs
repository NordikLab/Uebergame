//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function RoadInspector::inspect( %this, %obj ) {
	%name = "";

	if ( isObject( %obj ) )
		%name = %obj.getName();
	else
		RoadFieldInfoControl.setText( "" );

	//RoadInspectorNameEdit.setValue( %name );
	Parent::inspect( %this, %obj );
}

function RoadInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	// Same work to do as for the regular WorldEditor Inspector.
	Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
}

function RoadInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	RoadFieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

function RoadTreeView::onInspect(%this, %obj) {
	RoadInspector.inspect(%obj);
}