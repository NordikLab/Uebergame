//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
/// Screenshot function linked to a bind that take screenshot on key press
function screenShotBind( %val ) {
	// This can be bound, so skip key up events.
	if ( %val == 0 )
		return;

	takeScreenShot( 1 );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Screenshot function linked to a bind that take screenshot on key press
function hudlessScreenShotBind( %val ) {
	// This can be bound, so skip key up events.
	if ( %val == 0 ) {
		return;
	}

	$preShotGui = Canvas.getContent();
	setGui(HudlessPlayGui);
	takeScreenShot( 1 );
	schedule(500,0,"setGui",$preShotGui);
}
//------------------------------------------------------------------------------

/// A counter for screen shots used by _screenShot().
$screenshotNumber = 0;

//==============================================================================
/// Internal function which generates a screenshot with unique filename
function takeScreenShot( %tiles, %overlap ) {
	if ($LabNextScreenshotIsPreview)
		takeLevelScreenShot(%file);

	%name = getScreenShotName();

	if (( $pref::Video::screenShotFormat $= "JPEG" ) || ( $pref::video::screenShotFormat $= "JPG" ))
		screenShot( %name, "JPEG", %tiles, %overlap );
	else
		screenShot( %name, "JPEG", %tiles, %overlap );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Internal function which generates a screenshot and store it as level preview
function takeLevelScreenShot( ) {
	popDlg(ConsoleDlg);
	setGui(HudlessPlayGui);
	%name = $LevelObj.folder@"/"@fileBase($LevelObj.levelFile);
	%name = expandFileName( %name );
	screenShot( %name, "JPEG", false, %overlap );
	$LabNextScreenshotIsPreview = false;
	schedule(500,0,"setGui",$hudCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
/// This will close the console and take a large format
/// screenshot by tiling the current backbuffer and save
/// it to the root game folder.
///
/// For instance a tile setting of 4 with a window set to
/// 800x600 will output a 3200x2400 screenshot.
function tiledScreenShot( %tiles, %overlap ) {
	// Pop the console off before we take the shot.
	Canvas.popDialog( ConsoleDlg );
	takeScreenShot( %tiles, %overlap );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Function that set a unique name for the next screenshot
function getScreenShotName() {
	if ( $pref::Video::screenShotSession $= "" )
		$pref::Video::screenShotSession = 0;

	if ( $screenshotNumber == 0 )
		$pref::Video::screenShotSession++;

	if ( $pref::Video::screenShotSession > 999 )
		$pref::Video::screenShotSession = 1;

	%sessionNumber = setNumberLenght($pref::Video::screenShotSession , 3);
	%imageNumber = setNumberLenght($screenshotNumber , 3);
	%name = "screenshot_" @ %sessionNumber @ "-" @ %imageNumber;
	%name = expandFileName( %name );
	$screenshotNumber++;
	return %name;
}
//------------------------------------------------------------------------------

//==============================================================================
// RECORDING MOVIE
//==============================================================================

//==============================================================================
/// Records a movie file from the Canvas content using the specified fps.
/// Possible encoder values are "PNG" and "THEORA" (default).
function recordMovie(%movieName, %fps, %encoder) {
	// If the canvas doesn't exist yet, setup a flag so it'll
	// start capturing as soon as it's created
	if (!isObject(Canvas))
		return;

	if (%encoder $= "")
		%encoder = "THEORA";

	%resolution = Canvas.getVideoMode();
	startVideoCapture(Canvas, %movieName, %encoder, %fps);
}
//------------------------------------------------------------------------------
//==============================================================================
// Stope movie recording
function stopMovie() {
	stopVideoCapture();
}
//------------------------------------------------------------------------------