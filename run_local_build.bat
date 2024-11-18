@echo off

REM Define the root directory where the script is executed
set rootDir=%cd%

REM Step 1: Check and create the deploy folder structure
echo Step 1: Setting up folder structure...
if not exist "%rootDir%\deploy" (
    mkdir "%rootDir%\deploy"
    echo Folder 'deploy' created.
) else (
    echo Folder 'deploy' already exists.
)

if not exist "%rootDir%\deploy\client" (
    mkdir "%rootDir%\deploy\client"
    echo Folder 'client' created in 'deploy'.
) else (
    echo Folder 'client' already exists in 'deploy'.
)

if not exist "%rootDir%\deploy\server" (
    mkdir "%rootDir%\deploy\server"
    echo Folder 'server' created in 'deploy'.
) else (
    echo Folder 'server' already exists in 'deploy'.
)

if not exist "%rootDir%\deploy\client_test" (
    mkdir "%rootDir%\deploy\client_test"
    echo Folder 'client_test' created in 'deploy'.
) else (
    echo Folder 'client_test' already exists in 'deploy'.
)

set clientArtifactZipPath=C:\rps_project\client_build_artifacts.zip
set clientTestArtifactZipPath=C:\rps_project\client_test_artifacts.zip
set serverArtifactZipPath=C:\rps_project\server_build_artifacts.zip

echo Checking for existing artifacts to remove...
if exist "%clientArtifactZipPath%" (
    echo Removing existing client build artifact: %clientArtifactZipPath%
    del /f /q "%clientArtifactZipPath%"
) else (
    echo Client build artifact not found, skipping removal.
)

if exist "%clientTestArtifactZipPath%" (
    echo Removing existing client test artifact: %clientTestArtifactZipPath%
    del /f /q "%clientTestArtifactZipPath%"
) else (
    echo Client test artifact not found, skipping removal.
)

if exist "%serverArtifactZipPath%" (
    echo Removing existing server build artifact: %serverArtifactZipPath%
    del /f /q "%serverArtifactZipPath%"
) else (
    echo Server build artifact not found, skipping removal.
)

echo All necessary artifact checks and removals completed.

REM Initialize variables for the status table
set step1Status=NOT STARTED
set step2Status=NOT STARTED
set step3Status=NOT STARTED
set step4Status=NOT STARTED
set step5Status=NOT STARTED
set step6Status=NOT STARTED
set step7Status=NOT STARTED
set step8Status=NOT STARTED
set step9Status=NOT STARTED
set step10Status=NOT STARTED
set step11Status=NOT STARTED
set step12Status=NOT STARTED
set step13Status=NOT STARTED
set step14Status=NOT STARTED
set step15Status=NOT STARTED
set step1Status=PASSED
echo Step 1 completed successfully. [PASSED]

REM Define local directories and paths for client and server
set projectFolder=%cd%
set clientBuildOutput=%projectFolder%\deploy\client
set clientTestResultsPath=%projectFolder%\deploy\client_test
set clientTestResultFile=%clientTestResultsPath%\test_results.trx
set clientArtifactZipPath=%projectFolder%\client_build_artifacts.zip
set clientTestArtifactZipPath=%projectFolder%\client_test_artifacts.zip

set arduinoSketchFolder=%projectFolder%\server
set arduinoBoard=arduino:avr:uno
set arduinoPort=COM5
set serverOutputFolder=%projectFolder%\deploy\server
set serverArtifactZipPath=%projectFolder%\server_build_artifacts.zip


REM CLIENT SECTION
echo ---------------------------
echo CLIENT BUILD AND TEST START
echo ---------------------------

REM Step 2: Check and build the client project
echo Step 2: Checking project folder...
if not exist "%projectFolder%" (
    echo Error: Project folder not found: %projectFolder%
    set step2Status=FAILED
    goto FinalReport
) else (
    echo Project folder found: %projectFolder%.
    set step2Status=PASSED   
    echo Step 2 completed successfully. [PASSED]
)

REM Step 3: Restore NuGet packages
echo Step 3: Restoring NuGet packages...
nuget restore client\gameclient.sln
if %errorlevel% neq 0 (
    echo Error: Failed to restore NuGet packages.
    set step3Status=FAILED
    goto FinalReport
) else (
    echo NuGet packages restored successfully.
    set step3Status=PASSED
    echo Step 3 completed successfully. [PASSED]
)

REM Step 4: Check if Visual Studio Build Tools are installed
echo Step 4: Checking for Visual Studio Build Tools...

REM Check if running in GitHub Actions
if defined GITHUB_ACTIONS (
    echo Running in GitHub Actions...
    REM Check for VS Build Tools using a relative or system-wide path
    where MSBuild >nul 2>nul
    if errorlevel 1 (
        echo Error: Visual Studio Build Tools not found in GitHub Actions. Please install them.
        set step4Status=FAILED
        goto FinalReport
    ) else (
        echo Visual Studio Build Tools found in GitHub Actions.
        set step4Status=PASSED
        echo Step 4 completed successfully. [PASSED]
    )
) else (
    echo Running locally...
    REM Check for VS Build Tools locally using absolute path
    set vsInstallationPath=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\bin
    if not exist "%vsInstallationPath%" (
        echo Error: Visual Studio Build Tools not found. Please install them.
        set step4Status=FAILED
        goto FinalReport
    ) else (
        echo Visual Studio Build Tools found locally.
        set step4Status=PASSED
        echo Step 4 completed successfully. [PASSED]
    )
)

:FinalReport
REM Your final report logic here

REM Step 5: Build the client project
echo Step 5: Building the client project...
msbuild client\gameclient.sln /p:Configuration=Release /p:OutputPath=%clientBuildOutput%
if exist "%clientBuildOutput%\game client.exe" (
    echo Client build completed successfully. Artifacts located at: %clientBuildOutput%
    set step5Status=PASSED
    echo Step 5 completed successfully. [PASSED]
) else (
    echo Error: Client build failed. Please check the error messages.
    set step5Status=FAILED
    goto FinalReport
)


REM Step 6: Archive client build artifacts
echo Step 6: Creating build artifact archive...
powershell -Command "Compress-Archive -Path %clientBuildOutput% -DestinationPath %clientArtifactZipPath%"
if %errorlevel% neq 0 (
    echo Error: Failed to create build artifact archive.
    set step6Status=FAILED
    goto FinalReport
) else (
    echo Build artifacts saved at: %clientArtifactZipPath%.
    set step6Status=PASSED
    echo Step 6 completed successfully. [PASSED]
)


REM Step 7: Run client tests
echo Step 7: Running client tests...
dotnet test C:\rps_project\client\UnitTestProject1\bin\Debug\UnitTestProject1.dll --logger "trx;LogFileName=%clientTestResultFile%"
if %errorlevel% neq 0 (
    echo Client tests failed.
    set step7Status=FAILED
    goto FinalReport
) else (
    echo Client tests completed successfully.
    set step7Status=PASSED
)
echo Step 7 completed with status: [%step7Status%]

REM Step 8: Check test results
echo Step 8: Checking client test results...
findstr /i "<Failure>" "%clientTestResultFile%" >nul
if %errorlevel% equ 0 (
    echo Some client tests failed.
    set clientTestsStatus=Failed
    set step8Status=FAILED
    goto FinalReport
) else (
    echo All client tests passed successfully.
    set clientTestsStatus=Passed
    set step8Status=PASSED
)
echo Step 8 completed successfully. [%step8Status%]


REM Step 9: Archive client test artifacts
echo Step 9: Creating client test artifacts archive...
powershell -Command "Compress-Archive -Path %clientTestResultsPath% -DestinationPath %clientTestArtifactZipPath%"
if %errorlevel% neq 0 (
    echo Error: Failed to create client test artifacts archive.
    set step9Status=FAILED
    goto FinalReport
) else (
    echo Client test artifacts saved at: %clientTestArtifactZipPath%.
    set step9Status=PASSED
)
echo Step 9 completed successfully. [%step9Status%]


REM Client report
echo.
echo ---------------------------
echo CLIENT BUILD AND TEST REPORT
echo ---------------------------
echo Client Build Output Path: "%clientBuildOutput%"
echo Test Results Path: "%clientTestResultsPath%"
echo Test Status: %clientTestsStatus%
echo Artifact Zip Path: "%clientArtifactZipPath%"
echo Client Test Artifact Zip Path: "%clientTestArtifactZipPath%"
echo ---------------------------

REM SERVER SECTION
echo ---------------------------
echo SERVER BUILD AND UPLOAD START
echo ---------------------------

REM Step 10: Verify Arduino CLI installation
echo Step 10: Verifying Arduino CLI installation...
arduino-cli version >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Arduino CLI is not installed. Please install it first.
    set step10Status=FAILED
    goto FinalReport
) else (
    echo Arduino CLI installed successfully.
    set step10Status=PASSED
    echo Step 10 completed successfully. [PASSED]
)

REM Step 11: Install board definitions
echo Step 11: Installing board definitions...
arduino-cli core update-index >nul 2>&1
arduino-cli core install arduino:avr >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Failed to install board definitions for "%arduinoBoard%".
    set step11Status=FAILED
    goto FinalReport
) else (
    echo Board definitions installed successfully.
    set step11Status=PASSED
    echo Step 11 completed successfully. [PASSED]
)


REM Step 12: Compile the Arduino sketch
echo Step 12: Compiling Arduino sketch...
arduino-cli compile -b arduino:avr:uno --output-dir %serverOutputFolder% %arduinoSketchFolder%\server.ino >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Failed to compile Arduino sketch.
    set step12Status=FAILED
    goto FinalReport
) else (
    echo Arduino sketch compiled successfully and saved to "%serverOutputFolder%".
    set step12Status=PASSED
    echo Step 12 completed successfully. [PASSED]
)

REM Step 13: Request COM port from user
echo Step 13: Requesting Arduino COM port from user...
set /p arduinoPort=Enter the COM port for your Arduino (e.g., COM5): 
set arduinoPort=%arduinoPort:\\.\=%

REM Check if the entered port exists (validate by checking if COM port is available)
mode %arduinoPort% >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Failed to connect to %arduinoPort%. The port may not exist or be available.
    set step13Status=FAILED
    goto FinalReport
) else (
    echo COM port %arduinoPort% connected successfully.
    set step13Status=PASSED
    echo Step 13 completed successfully. [PASSED]
)

REM Step 14: Upload firmware to Arduino board
echo Step 14: Uploading firmware to Arduino board...
arduino-cli upload -p "%arduinoPort%" -b arduino:avr:uno --input-dir %serverOutputFolder%

REM Check if the upload was successful
if %errorlevel% neq 0 (
    echo Error: Failed to upload firmware to Arduino on port %arduinoPort%.
    set step14Status=FAILED
    goto FinalReport
) else (
    echo Firmware uploaded successfully to "%arduinoPort%".
    set step14Status=PASSED
    echo Step 14 completed successfully. [PASSED]
)

REM Step 15: Archive server build artifacts
echo Step 15: Creating server build artifact archive...

REM Create archive using PowerShell
powershell -Command "Compress-Archive -Path %serverOutputFolder% -DestinationPath %serverArtifactZipPath%"

REM Check if archiving was successful
if %errorlevel% neq 0 (
    echo Error: Failed to create server build artifact archive.
    set step15Status=FAILED
    goto FinalReport
) else (
    echo Server build artifacts saved at: %serverArtifactZipPath%.
    set step15Status=PASSED
    echo Step 15 completed successfully. [PASSED]
)


REM Server report
echo.
echo ---------------------------
echo SERVER BUILD AND UPLOAD REPORT
echo ---------------------------
echo Arduino Sketch Folder: "%arduinoSketchFolder%"
echo Arduino Board: "%arduinoBoard%"
echo Arduino Port: "%arduinoPort%"
echo Server Output Folder: "%serverOutputFolder%"
echo Server Artifact Zip Path: "%serverArtifactZipPath%"
echo ---------------------------

:FinalReport
REM FINAL REPORT
echo.
echo ===========================
echo FINAL BUILD AND DEPLOY REPORT
echo ===========================
echo CLIENT SECTION:
echo - Build Output Path: "%clientBuildOutput%"
echo - Test Results: %clientTestsStatus%
echo - Artifact Zip: "%clientArtifactZipPath%"
echo - Test Artifact Zip: "%clientTestArtifactZipPath%"
echo SERVER SECTION:
echo - Sketch Folder: "%arduinoSketchFolder%"
echo - Board: %arduinoBoard%
echo - Port: %arduinoPort%
echo - Output Folder: "%serverOutputFolder%"
echo - Server Artifact Zip: "%serverArtifactZipPath%"
echo ===========================
echo All tasks completed successfully on %date% at %time%
echo ===========================
REM Display the table of step statuses

echo.
echo =========================================================
echo                       STATUS TABLE
echo =========================================================
echo Step   Description                          Status
echo =========================================================
echo   1    Set up folder structure              %step1Status%
echo =========================================================
echo   2    Check and build client project       %step2Status%
echo =========================================================
echo   3    Restore NuGet packages               %step3Status%
echo =========================================================
echo   4    Check Visual Studio Build Tools      %step4Status%
echo =========================================================
echo   5    Build client project                 %step5Status%
echo =========================================================
echo   6    Archive client build artifacts       %step6Status%
echo =========================================================
echo   7    Run client tests                     %step7Status%
echo =========================================================
echo   8    Check test results                   %step8Status%
echo =========================================================
echo   9    Archive client test artifacts        %step9Status%
echo =========================================================
echo  10    Verify Arduino CLI installation      %step10Status%
echo =========================================================
echo  11    Install board definitions            %step11Status%
echo =========================================================
echo  12    Compile Arduino sketch               %step12Status%
echo =========================================================
echo  13    Request COM port from user           %step13Status%
echo =========================================================
echo  14    Upload firmware to Arduino           %step14Status%
echo =========================================================
echo  15    Archive server build artifacts       %step15Status%
echo =========================================================
echo ---------------------------------------------------------
echo All tasks completed successfully on %date% at %time%
echo ---------------------------------------------------------

set indexHtmlFile=%rootDir%\index.html

echo Creating final report table in %indexHtmlFile%...

(
    echo ^<html^>
    echo ^<head^>
    echo ^<title^>Build and Test Report^</title^>
    echo ^</head^>
    echo ^<body^>
    echo ^<h1^>Build and Test Report^</h1^>

    REM Таблиця для звіту по кроках
    echo ^<h2^>Step Report^</h2^>
    echo ^<table border="1" cellpadding="5" cellspacing="0"^>
    echo ^<tr^>^<th^>Step^</th^>^<th^>Description^</th^>^<th^>Status^</th^>^</tr^>
    
    echo ^<tr^>^<td^>1^</td^>^<td^>Set up folder structure^</td^>^<td^>%step1Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>2^</td^>^<td^>Check and build client project^</td^>^<td^>%step2Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>3^</td^>^<td^>Restore NuGet packages^</td^>^<td^>%step3Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>4^</td^>^<td^>Check Visual Studio Build Tools^</td^>^<td^>%step4Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>5^</td^>^<td^>Build client project^</td^>^<td^>%step5Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>6^</td^>^<td^>Archive client build artifacts^</td^>^<td^>%step6Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>7^</td^>^<td^>Run client tests^</td^>^<td^>%step7Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>8^</td^>^<td^>Check test results^</td^>^<td^>%step8Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>9^</td^>^<td^>Archive client test artifacts^</td^>^<td^>%step9Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>10^</td^>^<td^>Verify Arduino CLI installation^</td^>^<td^>%step10Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>11^</td^>^<td^>Install board definitions^</td^>^<td^>%step11Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>12^</td^>^<td^>Compile Arduino sketch^</td^>^<td^>%step12Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>13^</td^>^<td^>Request COM port from user^</td^>^<td^>%step13Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>14^</td^>^<td^>Upload firmware to Arduino^</td^>^<td^>%step14Status%^</td^>^</tr^>
    echo ^<tr^>^<td^>15^</td^>^<td^>Archive server build artifacts^</td^>^<td^>%step15Status%^</td^>^</tr^>
    
    echo ^</table^>
    
    echo ^<h2^>Final Report^</h2^>
    echo ^<table border="1" cellpadding="5" cellspacing="0"^>
    echo ^<tr^>^<th^>Section^</th^>^<th^>Details^</th^>^</tr^>
    echo ^<tr^>^<td^>CLIENT SECTION^</td^>^<td^>^<ul^>
    echo ^<li^>Build Output Path: %clientBuildOutput%^</li^>
    echo ^<li^>Test Results: %clientTestsStatus%^</li^>
    echo ^<li^>Artifact Zip: %clientArtifactZipPath%^</li^>
    echo ^<li^>Test Artifact Zip: %clientTestArtifactZipPath%^</li^>
    echo ^</ul^>^</td^>^</tr^>
    
    echo ^<tr^>^<td^>SERVER SECTION^</td^>^<td^>^<ul^>
    echo ^<li^>Sketch Folder: %arduinoSketchFolder%^</li^>
    echo ^<li^>Board: %arduinoBoard%^</li^>
    echo ^<li^>Port: %arduinoPort%^</li^>
    echo ^<li^>Output Folder: %serverOutputFolder%^</li^>
    echo ^<li^>Server Artifact Zip: %serverArtifactZipPath%^</li^>
    echo ^</ul^>^</td^>^</tr^>

    echo ^</table^>
    echo ^<p^>All tasks completed successfully on %date% at %time%.^</p^>
    echo ^</body^>
    echo ^</html^>
) > "%indexHtmlFile%"

echo HTML final report created successfully at: %indexHtmlFile%

pause