//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// String Manipulation Helpers
//==============================================================================

//==============================================================================
// Validate a string of specific needs
//==============================================================================
//==============================================================================
// Validate a string used for a filename
function checkFilenameString(%filename) {
	%filename = strreplace(%filename," ","_");
	%filename = stripChars(%filename,"/?<>\\@:*|\"");
	return %filename;
}
//------------------------------------------------------------------------------
//==============================================================================
// Validate a string used for a filename
function validateObjectName(%name) {
	%name = strreplace(%name," ","_");
	%name = stripChars(%name,"/?<>\\@:*|\"");
	return %name;
}
//------------------------------------------------------------------------------
//==============================================================================
// Validate a string used for a filename
function addObjToString(%obj,%str) {
	//Check if we already have this obj
	%name = %obj.getName();

	if (%name $="") %name = %obj;

	foreach$(%word in %str) {
		if (%word $= %name) {
			%found = true;
			break;
		}
	}

	if (!%found) {
		%str = trim(%str SPC %name);
	}

	return %str;
}
//------------------------------------------------------------------------------


//==============================================================================
// Characters Manipulations
//==============================================================================
//==============================================================================
/// Get the characters at end of string for the specified count
/// Optional: %count -> Will return that amount of characters. (default = 1)
function strLastChars(%str,%count) {
	%len = strlen(%str);
	%chars = getSubStr(%str,%len-%count);
	return %chars;
}
//------------------------------------------------------------------------------
//==============================================================================
/// Get the characters at end of string for the specified count
/// Optional: %count -> Will return that amount of characters. (default = 1)
function strStartChars(%str,%count) {
	return getSubStr(%str,0,%count);
}
//------------------------------------------------------------------------------
//==============================================================================
// Words Manipulations
//==============================================================================
//==============================================================================
//
function strAddWord(%str,%word,%unique,%prepend) {
	
	if (%unique) {	
		foreach$(%w in %str) {			
			if (%w $= %word){				
				return %str;
			}
		}
	}

	if (%prepend)
		%newStr = trim(%word SPC %str);
	else
		%newStr = trim(%str SPC %word);

	return %newStr;
}
//------------------------------------------------------------------------------

//==============================================================================
//
function strRemoveWord(%str,%word) {
	%id = 0;

	foreach$(%w in %str) {
		if (%w $= %word) {
			//Reverse order to remove words from last to not affect index ordering
			%removeIds = strAddWord(%removeIds,%id,true,true);
		}

		%id++;
	}

	foreach$(%id in %removeIds) {
		%preStr = %str;
		%str = removeWord(%str,%id);
	}

	return %str;
}
//------------------------------------------------------------------------------
//==============================================================================
/// Get the characters at end of string for the specified count
/// Optional: %count -> Will return that amount of characters. (default = 1)
function getLastWord(%str) {
	%lastWord = getWord(%str,getWordCount(%str)-1);
	return %lastWord;
}
//------------------------------------------------------------------------------
//==============================================================================
// Check if the word contain any of the supplied words. Return true if found.
// Optional: %all -> If %all is true, must find all words to return true;
function strFindWords(%str,%words,%all) {
	foreach$(%word in %words) {
		%result = strstr(%str,%word);

		//If not found and all is required, return false
		if (%result $= "-1" && %all)
			return false;

		//If word is found, return true;
		if (%result !$= "-1")
			return true;
	}

	//If %all is true and still here, it mean all words have been found
	if (%all)
		return true;

	//If still here, it mean we found not word
	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
/// Check if the word contain any of the supplied words. Return true if found.
/// Optional: %all -> If %all is true, must find all words to return true;
function strFind(%str,%find) {
	//Add a space before string to fix the first char not checked issue
	%result = strstr(" "@%str,%find);
	

	if (%result $= "-1" || %result $= "" || !%result )
		return false;

	return true;
}
//------------------------------------------------------------------------------

//==============================================================================
//
function strAddField(%str,%field,%addEmpty) {
	if (%addEmpty && %field $= "")
		%field = " ";
	
	if (%str !$= "" )
		%str = %str @"\t"@ %field;
	else
		%str = %field;
	if (!%addEmpty)
	%str = trim(%str);
	
	return %str;
}
function strAddRecord(%str,%line) {
	if (%str !$= "")
		%str = %str NL %line;
	else
		%str = %line;
	%str = trim(%str);
	return %str;
}


//------------------------------------------------------------------------------
// Convert an HTML string into a game text
function strBeforeChar(%str,%search) {
	%searchPos = strpos(%str,%search);
	%newStr = getSubStr(%str,0,%searchPos);
	return %newStr;
}


//==============================================================================
// Advanced string replacements
//==============================================================================
function strReplaceList(%str,%replaces) {
	%count = getRecordCount(%replaces);

	for(%i=0; %i<%count; %i++) {
		%record = getRecord(%replaces,%i);
		%find = getField(%record,0);
		%replace = getField(%record,1);
		%str = strreplace(%str,%find,%replace);
	}

	return %str;
}

//------------------------------------------------------------------------------
// Convert an HTML string into a game text
function convertHtmlToGame(%html) {
	//Replace all </p> with new line
	%newText = strreplace(%html , "</p>" , "\n" );
	%newText = strreplace(%newText , "<p>" , "" );
	return %newText;
}

//------------------------------------------------------------------------------
// Convert the extra fields of K2
function convertExtraFields(%fields) {
	//"[{\"id\":\"1\",\"value\":\"1\"},{\"id\":\"2\",\"value\":\"1\"}]";
	//Replace all </p> with new line
	%newText = strreplace(%fields , "</p>" , "\n" );
	%newText = strreplace(%newText , "<p>" , "" );
	return %newText;
}

//--------------------------------------------------------------------------
// Finds location of %word in %text, starting at %start.  Works just like strPos
//--------------------------------------------------------------------------

function wordPos(%text, %word, %start) {
	if (%start $= "") %start = 0;

	if (strpos(%text, %word, 0) == -1) return -1;

	%count = getWordCount(%text);

	if (%start >= %count) return -1;

	for (%i = %start; %i < %count; %i++) {
		if (getWord( %text, %i) $= %word) return %i;
	}

	return -1;
}

//--------------------------------------------------------------------------
// Finds location of %field in %text, starting at %start.  Works just like strPos
//--------------------------------------------------------------------------

function fieldPos(%text, %field, %start) {
	if (%start $= "") %start = 0;

	if (strpos(%text, %field, 0) == -1) return -1;

	%count = getFieldCount(%text);

	if (%start >= %count) return -1;

	for (%i = %start; %i < %count; %i++) {
		if (getField( %text, %i) $= %field) return %i;
	}

	return -1;
}

//--------------------------------------------------------------------------
// returns the text in a file with "\n" at the end of each line
//--------------------------------------------------------------------------

//==============================================================================
// Color Conversion
//==============================================================================
//==============================================================================
/// Convert %color from float to int. Ex "0 0.5 1" => "0 128 255"
function ColorFloatToInt( %color ) {
	%red     = getWord( %color, 0 );
	%green   = getWord( %color, 1 );
	%blue    = getWord( %color, 2 );
	%alpha   = getWord( %color, 3 );
	return mCeil( %red * 255 ) SPC mCeil( %green * 255 ) SPC mCeil( %blue * 255 ) SPC mCeil( %alpha * 255 );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Convert %color from int to float. Ex "0 128 255" => "0 0.5 1"
function ColorIntToFloat( %color ) {
	%red     = getWord( %color, 0 );
	%green   = getWord( %color, 1 );
	%blue    = getWord( %color, 2 );
	%alpha   = getWord( %color, 3 );
	return ( %red / 255 ) SPC ( %green / 255 ) SPC ( %blue / 255 ) SPC ( %alpha / 255 );
}
//------------------------------------------------------------------------------
/*
function isInList( %word, %list ) {
	%count = getWordCount( %list );

	for( %i = 0; %i < %count; %i++ ) {
		%entry = getWord( %list, %i );

		if( %word $= %entry )
			return true;
	}

	return false;
}

function isInFieldList(%word, %list) {
	%count = getFieldCount( %list );

	for( %i = 0; %i < %count; %i++ ) {
		%entry = getField( %list, %i );

		if( %word $= %entry )
			return true;
	}

	return false;
}*/