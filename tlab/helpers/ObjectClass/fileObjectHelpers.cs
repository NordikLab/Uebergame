//==============================================================================
// Boost! --> Helpers for console reports logs
// Copyright NordikLab Studio, 2013
//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================
// Info Log - Show official game informations to the console
function getFileReadObj(%file) {
	// Create a file object for reading
	%fileRead = new FileObject();
	// Open a text file, if it exists
	%fileRead.OpenForRead(%file);

	return %fileRead;
}
//------------------------------------------------------------------------------
//==============================================================================
// Info Log - Show official game informations to the console
function getFileWriteObj(%file,%append) {
	// Create a file object for reading
	%fileObj = new FileObject();
	// Open a text file, if it exists
	if (%append)
		%fileObj.OpenForAppend(%file);
	else
		%fileObj.OpenForWrite(%file);
	return %fileObj;
}
//------------------------------------------------------------------------------
//==============================================================================
// Info Log - Show official game informations to the console
function closeFileObj(%fileObj) {
	// Close the text file
	%fileObj.close();

	// Cleanup
	%fileObj.delete();
}
//------------------------------------------------------------------------------
