echo == Compiling for DECOLAR ==

echo Copying manifest files
copy "%~1Compilation\Decolar\Decolar.appxmanifestX" "%~1Package.appxmanifest"

echo Copying Splash assets
copy "%~1Compilation\Decolar\decolarsplash-white.scale-100.png" "%~1Assets\Splash.scale-100.png"
copy "%~1Compilation\Decolar\decolarsplash-white.scale-140.png" "%~1Assets\Splash.scale-140.png"
copy "%~1Compilation\Decolar\decolarsplash-white.scale-240.png" "%~1Assets\Splash.scale-240.png"

echo Copy Top Logo - Brand
copy "%~1Compilation\Decolar\decolar-logo-white.scale-100.png" "%~1Assets\brandLogo.scale-100.png"
copy "%~1Compilation\Decolar\decolar-logo-white.scale-140.png" "%~1Assets\brandLogo.scale-140.png"
copy "%~1Compilation\Decolar\decolar-logo-white.scale-240.png" "%~1Assets\brandLogo.scale-240.png"