//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
/// Delete an object by making sure it exist
function delObj(%obj) {
	if (isObject(%obj))
		%obj.delete();
}
//------------------------------------------------------------------------------


//==============================================================================
// SimObject Show and Hide Helpers
//==============================================================================
//==============================================================================
// Hide an object simply
function hide(%obj,%child) {
   //If %child specified, hide the %obj child
   %hideMe = %obj;
   if (%child !$=""){
      %hideMe = %obj.findObjectByInternalName(%child,true);
   }
	if (!isObject(%hideMe))
		return false;

	%hideMe.setVisible(false);
}
//------------------------------------------------------------------------------
//==============================================================================
// Show an object simply
function show(%obj,%child) {
    //If %child specified, hide the %obj child
   %showMe = %obj;
   if (%child !$=""){
      %showMe = %obj.findObjectByInternalName(%child,true);
   }
	if (!isObject(%showMe))
		return false;
	%showMe.setVisible(true);
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle an object visibility state
function toggleVisible(%obj,%child) {
     //If %child specified, hide the %obj child
   %toggleMe = %obj;
   if (%child !$=""){
      %toggleMe = %obj.findObjectByInternalName(%child,true);
   }
   if (!isObject(%toggleMe))
		return false;
	if (%toggleMe.visible)
		hide(%toggleMe);
	else
		show(%toggleMe);
}
//------------------------------------------------------------------------------
//==============================================================================
// SimObject / SimGroup Helpers
//==============================================================================
/// Clone an object and make source invisible
function cloneObject( %source,%name,%internalName,%parent ) {
   delObj(%name);
	%clone = %source.deepClone();
	%clone.setVisible(true);
	if (%internalName !$= "")
	   %clone.internalName = %internalName;   
	%clone.setName(%name);
	if (isObject(%parent))
	   %parent.add(%clone);  
	   
	return %clone;
}

//==============================================================================
// SimObject Function Helpers
//===============================================================================
//==============================================================================
function findIntName(%obj,%intName, %searchChild) {
	if (%searchChild $= "")
		%searchChild = true;
	if (!isObject(%obj)) {
		return;
	}
	%return = %obj.findObjectByInternalName(%intName,%searchChild);
	return %return;

}
//------------------------------------------------------------------------------

//==============================================================================
// Get child of obj by internal Name
function dumpMethods(%object) {
	%array = %object.dumpMethods();
	%array.dumpContent();
}
//------------------------------------------------------------------------------

function rotUnConv(%axisAngle)  
{  
   %angleOver2 = -getWord(%axisAngle,3)/2;  
   %sinThetaOver2 = mSin(%angleOver2);  
  
   %q0 = mCos(%angleOver2);  
   %q1 = getWord(%axisAngle,0) * %sinThetaOver2;  
   %q2 = getWord(%axisAngle,1) * %sinThetaOver2;  
   %q3 = getWord(%axisAngle,2) * %sinThetaOver2;  
   %q4 = %q0*%q0;  
  
   return vectorScale(mAsin(2*(%q2*%q3 + %q0*%q1))  
                  SPC mAtan(%q0*%q2 - %q1*%q3, (%q4+%q3*%q3)-0.5)  
                  SPC mAtan(%q0*%q3 - %q1*%q2, (%q4+%q2*%q2)-0.5) ,-180/$pi);  
}  

function testEulerFromAxisAngle(%eulerVec) // input Euler angles in X,Y,Z order in degrees.  
{  
   %eX = mDegToRad(getWord(%eulerVec,0));  
   %eY = mDegToRad(getWord(%eulerVec,1));  
   %eZ = mDegToRad(getWord(%eulerVec,2));  
     
   %transform = MatrixCreateFromEuler(%eX SPC %eY SPC %eZ);  
   echo("MatrixCreateFromEuler transform= " SPC %transform);  
     
   %eulerOut = eulerFromAxisAngle(getWords(%transform,3,6));  
     
   // convert to degrees for output comparison  
   %exOut = mRadToDeg(getWord(%eulerOut,0));  
   %eyOut = mRadToDeg(getWord(%eulerOut,1));  
   %ezOut = mRadToDeg(getWord(%eulerOut,2));  
   echo("eulerFromAxisAngle result = " SPC %exOut SPC %eyOut SPC %ezOut SPC "(degrees)");  
     
   MatrixCreateFromEuler(%eulerOut);  
   echo("MatrixCreateFromEuler(" @ %eulerOut @ ") transform = " SPC %transform);  
}  