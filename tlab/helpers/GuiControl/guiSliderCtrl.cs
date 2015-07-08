//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================


function getTicksFromRange(%range,%stepSize) {
	%min = %range.x;
	%max = %range.y;
	%diff = %max - %min;
	%steps = %diff / %stepSize;
	%stepSafe = mRound(%steps);
	if (%steps !$= %stepSafe){
		%realStepSize = %diff /%stepSafe; 
		warnLog("The stepSize don't fit with range, resulting of a different stepSize of:",%realStepSize);
	}
	return %stepSafe - 1;
   
}