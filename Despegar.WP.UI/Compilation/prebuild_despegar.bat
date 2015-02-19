
echo == Compiling for DESPEGAR ==

echo Copying manifest files
copy "%~1Compilation\Despegar\Despegar.appxmanifestX" "%~1Package.appxmanifest"

echo Copying Splash assets
copy "%~1Compilation\Despegar\despegarsplash-white.scale-100.png" "%~1Assets\Splash.scale-100.png"
copy "%~1Compilation\Despegar\despegarsplash-white.scale-140.png" "%~1Assets\Splash.scale-140.png"
copy "%~1Compilation\Despegar\despegarsplash-white.scale-240.png" "%~1Assets\Splash.scale-240.png"

echo Copy Top Logo - Brand
copy "%~1Compilation\Despegar\despegar-logo-white.scale-100.png" "%~1Assets\brandLogo.scale-100.png"
copy "%~1Compilation\Despegar\despegar-logo-white.scale-140.png" "%~1Assets\brandLogo.scale-140.png"
copy "%~1Compilation\Despegar\despegar-logo-white.scale-240.png" "%~1Assets\brandLogo.scale-240.png"