//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
/// Make a number char length by filling 0 before
function setNumberLenght( %number,%length ) {  
   while (strlen(%number) < %lenth)
      %number = "0"@%number;
   return %number;
}
//------------------------------------------------------------------------------
//==============================================================================
// Check is a string is numeric
//------------------------------------------------------------------------------
function strIsNumeric(%str) {
	%numeric = "0123456789";
	%dot = false;
	for (%i = strlen(%str) - 1; %i > -1; %i --) {
		%char = getSubStr(%str, %i, 1);
		if (strPos(%numeric, %char) == -1) {
			if (%char $= ".") {
				if (%dot)
					return false;
				%dot = true;
				continue;
			} else if (%char $= "-") {
				if (%i != 0)
					return false;
				continue;
			}
			return false;
		}
	}
	return true;
}
//==============================================================================
// Validate a string used for a filename
function setFloatPrecision(%value,%decimalCount) {

	//Not interested with value of more than 1 word
	if (getWordCount(%value) > 1 ) {
		return %value;
	}

	//check if a dot is found
	%checkVal = strreplace(%value,"."," ");

	if (getWordCount(%checkVal) == 1) {
		return %value;
	}


	%checkMain = getWord(%checkVal,0);
	%checkDecimal = getWord(%checkVal,1);
	if (%checkDecimal $= "png" || %checkDecimal $= "dds" || %checkDecimal $= "dae" || %checkDecimal $= "dts") {
		return %value;
	}
	%decimals = getSubStr(%checkDecimal,0,%decimalCount);

	%newValue = %checkMain@"."@%decimals;

	return %newValue;
}
//------------------------------------------------------------------------------

//==============================================================================
// Validate a string used for a filename
function setWordsFloatPrecision(%value,%decimalCount) {
	%i = 0;
	foreach$(%word in %value) {
		%newWord = setFloatPrecision(%word,%decimalCount);
		%newValue = trim(%newValue SPC %newWord);
		%i++;
	}
	return %newValue;
}
//------------------------------------------------------------------------------