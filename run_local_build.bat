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

if not exist "%rootDir%\artefacts" (
    mkdir "%rootDir%\artefacts"
    echo Folder 'deploy' created.
) else (
    echo Folder 'deploy' already exists.
)

if not exist "%rootDir%\deploy\test_coverage" (
    mkdir "%rootDir%\deploy\test_coverage"
    echo Folder 'test_coverage' created in 'deploy'.
) else (
    echo Folder 'test_coverage' already exists in 'deploy'.
)
if not exist "%rootDir%\deploy\documentation" (
    mkdir "%rootDir%\deploy\documentation"
    echo Folder 'documentation' created in 'deploy'.
) else (
    echo Folder 'documentation' already exists in 'deploy'.
)
set clientArtifactZipPath=%~dp0\artefacts\client_build_artifacts.zip
set clientTestArtifactZipPath=%~dp0\artefacts\client_test_artifacts.zip
set serverArtifactZipPath=%~dp0\artefacts\server_build_artifacts.zip

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

set sourceFolder=%cd%\media\img
set destinationFolder=%cd%\deploy\client\img

REM Перевірка, чи існує папка в цільовому місці
if exist "%destinationFolder%" (
    echo Destination folder exists. Deleting...
    rmdir /S /Q "%destinationFolder%"
    echo Destination folder deleted.
)

REM Копіювання папки
if exist "%sourceFolder%" (
    xcopy "%sourceFolder%" "%destinationFolder%" /E /I
    echo Folder copied successfully!
) else (
    echo Source folder not found!
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
set step16Status=NOT STARTED
set step17Status=NOT STARTED
set step1Status=PASSED
echo Step 1 completed successfully. [PASSED]

REM Define local directories and paths for client and server
set projectFolder=%cd%
set clientBuildOutput=%projectFolder%\deploy\client
set clientTestResultsPath=%projectFolder%\deploy\client_test
set clientTestResultFile=%clientTestResultsPath%\test_results.trx
set clientArtifactZipPath=%projectFolder%\artefacts\client_build_artifacts.zip
set clientTestArtifactZipPath=%projectFolder%\artefacts\client_test_artifacts.zip

set arduinoSketchFolder=%projectFolder%\server
set arduinoBoard=arduino:avr:uno
set arduinoPort=
set baudRate=
set serverOutputFolder=%projectFolder%\deploy\server
set serverArtifactZipPath=%projectFolder%\artefacts\server_build_artifacts.zip


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
    echo Visual Studio Build Tools found.
    set step4Status=PASSED
    echo Step 4 completed successfully. [PASSED]
) else (
   echo Running locally...
   REM Check for Visual Studio Build Tools locally using absolute path
   where MSBuild.exe >nul 2>nul
if errorlevel 1 (
    echo Error: Visual Studio Build Tools not found. Please install them.
    set step4Status=FAILED
    goto FinalReport
) else (
    echo Visual Studio Build Tools found.
    set step4Status=PASSED
    echo Step 4 completed successfully. [PASSED]
)


)

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
dotnet test deploy\client\UnitTestProject1.dll --logger "trx;LogFileName=%clientTestResultFile%"
if %errorlevel% neq 0 (
    echo Client tests failed.
    set step7Status=FAILED
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

REM Step 10: Generate documentation using Doxygen
echo Step 10: Generating documentation with Doxygen...

REM Set project folder and code directory paths
set projectFolder=%cd%
set codeDirectory=%projectFolder%\client

REM Define paths
set doxyfilePath=%projectFolder%\deploy\documentation\Doxyfile
set doxyOutputFolder=%projectFolder%\deploy\documentation\docs

REM Check if Doxyfile exists
if not exist "%doxyfilePath%" (
    echo Doxyfile not found. Creating a default Doxyfile...

    REM Generate a default Doxyfile
    doxygen -g "%doxyfilePath%"
    if errorlevel 1 (
        echo Error: Failed to create default Doxyfile. Please check permissions and try again.
        set step10Status=FAILED
        goto FinalReport
    ) else (
        echo Default Doxyfile created at %doxyfilePath%.
    )
)

REM Modify Doxyfile to include project-specific paths
echo Configuring Doxyfile...

REM Ensure the output directory exists
if not exist "%doxyOutputFolder%" (
    mkdir "%doxyOutputFolder%"
    echo Output folder created: %doxyOutputFolder%.
) else (
    echo Output folder already exists: %doxyOutputFolder%.
)

REM Configure the Doxyfile with necessary settings
(
    for /f "tokens=1,* delims== " %%A in ('type "%doxyfilePath%"') do (
        if /i "%%A"=="OUTPUT_DIRECTORY" (
            echo OUTPUT_DIRECTORY = %doxyOutputFolder%
        ) else if /i "%%A"=="INPUT" (
            echo INPUT = %codeDirectory%
        ) else if /i "%%A"=="FILE_PATTERNS" (
            echo FILE_PATTERNS = *.cs
        ) else if /i "%%A"=="EXTRACT_ALL" (
            echo EXTRACT_ALL = YES
        ) else if /i "%%A"=="GENERATE_LATEX" (
            echo GENERATE_LATEX = NO  REM Disable LaTeX generation to avoid issues with epstopdf
        ) else if /i "%%A"=="USE_PLMU" (
            echo USE_PLMU = YES  REM Enable PlantUML support
        ) else if /i "%%A"=="IMAGE_PATH" (
            echo IMAGE_PATH = media\doc_img  REM Relative path to the image directory
        ) else (
            echo %%A=%%B
        )
    )
) > "%doxyfilePath%.tmp"

REM Replace original Doxyfile with updated version
move /Y "%doxyfilePath%.tmp" "%doxyfilePath%"
if errorlevel 1 (
    echo Error: Failed to update Doxyfile.
    set step10Status=FAILED
    goto FinalReport
) else (
    echo Doxyfile configured successfully.
)

REM Run Doxygen to generate documentation
echo Running Doxygen...
doxygen "%doxyfilePath%"
if errorlevel 1 (
    echo Error: Doxygen failed to generate documentation. Check the configuration and try again.
    set step10Status=FAILED
    goto FinalReport
) else (
    echo Documentation generated successfully in %doxyOutputFolder%.
    set step10Status=PASSED
)

REM Step 10 completed successfully.
echo Step 10 completed successfully. [PASSED]

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

REM Step 11: Run Unit Test Coverage
echo Step 11: Running Unit Test Coverage...
REM Define relative paths for coverage tools
set OpenCoverPath=.\Tools\OpenCover
set ReportGeneratorPath=.\Tools\ReportGenerator\net47
set TestRunnerPath="vstest.console.exe"
set TestAssembly=.\deploy\client\UnitTestProject1.dll
set CoverageOutput=.\deploy\test_coverage\coverage.xml
set ReportOutput=.\deploy\test_coverage\coverage-report


REM Run tests with OpenCover for code coverage
"%OpenCoverPath%\OpenCover.Console.exe" -register:user -target:%TestRunnerPath% -targetargs:"%TestAssembly%" -output:%CoverageOutput% -filter:"+[*UnitTestProject1.dll]*" -hideskipped:All

REM Generate HTML report from coverage
"%ReportGeneratorPath%\ReportGenerator.exe" -reports:%CoverageOutput% -targetdir:%ReportOutput% -reporttypes:Html

REM Check if the coverage report generation was successful
if %errorlevel% neq 0 (
    echo Error: Failed to generate coverage report.
    set step11Status=FAILED
) else (
    echo Code coverage report generated successfully.
    set step11Status=PASSED
    echo Step 11 completed successfully. [PASSED]
)

REM Extract overall coverage percentage from coverage.xml
for /f "tokens=3 delims== " %%i in ('findstr /i "summary numSequencePoints" "%CoverageOutput%"') do (
    set totalCoverage=%%i
)

REM Clean up extracted value
set totalCoverage=%totalCoverage:"=%
set totalCoverage=%totalCoverage:%%=%
echo.

REM SERVER SECTION - COVERAGE SUMMARY
echo ---------------------------
echo COVERAGE SUMMARY
echo ---------------------------
echo Overall Code Coverage: %totalCoverage% %
echo Coverage Output File: "%CoverageOutput%"
echo Report Output Folder: "%ReportOutput%"
echo ---------------------------

echo.


REM SERVER SECTION
echo ---------------------------
echo SERVER BUILD AND UPLOAD START
echo ---------------------------

REM Step 12: Verify Arduino CLI installation
echo Step 12: Verifying Arduino CLI installation...
arduino-cli version >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Arduino CLI is not installed. Please install it first.
    set step12Status=FAILED
    goto FinalReport
) else (
    echo Arduino CLI installed successfully.
    set step12Status=PASSED
    echo Step 12 completed successfully. [PASSED]
)

REM Step 13: Install board definitions
echo Step 13: Installing board definitions...
arduino-cli core update-index >nul 2>&1
arduino-cli core install arduino:avr >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Failed to install board definitions for "%arduinoBoard%".
    set step13Status=FAILED
    goto FinalReport
) else (
    echo Board definitions installed successfully.
    set step13Status=PASSED
    echo Step 13 completed successfully. [PASSED]
)


REM Step 14: Compile the Arduino sketch
echo Step 14: Compiling Arduino sketch...
arduino-cli compile -b arduino:avr:uno --output-dir %serverOutputFolder% %arduinoSketchFolder%\server.ino >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Failed to compile Arduino sketch.
    set step14Status=FAILED
    goto FinalReport
) else (
    echo Arduino sketch compiled successfully and saved to "%serverOutputFolder%".
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

REM Step 16: Request COM port from user (for local execution) or use provided COM port (for GitHub Actions)
echo Step 16: Checking for COM port and baud rate...
REM Check if running in GitHub Actions
    REM Use the COM port and baud rate passed as environment variables (e.g., COM_PORT and BAUD_RATE)
    set "arduinoPort=%1"
    set "baudRate=%2"
    echo COM port passed: %arduinoPort%
    echo Baud rate passed: %baudRate%

REM Validate COM port and baud rate
echo Trying to connect to %arduinoPort% at baud rate %baudRate%...
REM Check if baud rate is 9600
if not "%baudRate%"=="9600" (
    echo Error: Unsupported baud rate %baudRate%. Only 9600 is allowed.
    set step16Status=FAILED
    goto FinalReport
)

REM Check if the entered port exists (validate by checking if COM port is available)
mode %arduinoPort% >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Failed to connect to %arduinoPort%. The port may not exist or be available.
    set step16Status=FAILED
    goto FinalReport
)

REM If all checks pass
echo COM port %arduinoPort% connected successfully at baud rate %baudRate%.
set step16Status=PASSED
echo Step 16 completed successfully. [PASSED]

REM Step 17: Upload firmware to Arduino board
echo Step 17: Uploading firmware to Arduino board...
arduino-cli upload -p "%arduinoPort%" -b arduino:avr:uno --input-dir %serverOutputFolder%

REM Check if the upload was successful
if %errorlevel% neq 0 (
    echo Error: Failed to upload firmware to Arduino on port %arduinoPort%.
    set step16Status=FAILED
    goto FinalReport
) else (
    echo Firmware uploaded successfully to "%arduinoPort%".
    set step16Status=PASSED
    echo Step 16 completed successfully. [PASSED]
)

REM Server report
echo.
echo ---------------------------
echo SERVER BUILD AND UPLOAD REPORT
echo ---------------------------
echo Arduino Sketch Folder: "%arduinoSketchFolder%"
echo Arduino Board: "%arduinoBoard%"
echo Arduino Port: "%arduinoPort%"
echo Arduino Baud Rate: "%baudRate%"
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
echo - Baud Rate: %baudRate%
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
echo  10    Generate documentation using Doxygen %step10Status%
echo =========================================================
echo  11    Run Unit Test Coverage               %step11Status%
echo =========================================================
echo  12    Verify Arduino CLI installation      %step12Status%
echo =========================================================
echo  13    Install board definitions            %step13Status%
echo =========================================================
echo  14    Compile Arduino sketch               %step14Status%
echo =========================================================
echo  15    Archive server build artifacts       %step15Status%
echo =========================================================
echo  16    Request COM port and baud rate       %step16Status%
echo =========================================================
echo  17    Upload firmware to Arduino           %step17Status%
echo =========================================================
echo ---------------------------------------------------------
echo All tasks completed successfully on %date% at %time%
echo ---------------------------------------------------------


set indexHtmlFile=%rootDir%\artefacts\index.html

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
echo ^<tr^>^<td^>10^</td^>^<td^>Generate documentation using Doxygen^</td^>^<td^>%step10Status%^</td^>^</tr^>
echo ^<tr^>^<td^>11^</td^>^<td^>Run Unit Test Coverage^</td^>^<td^>%step11Status%^</td^>^</tr^>
echo ^<tr^>^<td^>12^</td^>^<td^>Verify Arduino CLI installation^</td^>^<td^>%step12Status%^</td^>^</tr^>
echo ^<tr^>^<td^>13^</td^>^<td^>Install board definitions^</td^>^<td^>%step13Status%^</td^>^</tr^>
echo ^<tr^>^<td^>14^</td^>^<td^>Compile Arduino sketch^</td^>^<td^>%step14Status%^</td^>^</tr^>
echo ^<tr^>^<td^>15^</td^>^<td^>Archive server build artifacts^</td^>^<td^>%step15Status%^</td^>^</tr^>
echo ^<tr^>^<td^>16^</td^>^<td^>Request COM port and baud rate^</td^>^<td^>%step16Status%^</td^>^</tr^>
echo ^<tr^>^<td^>17^</td^>^<td^>Upload firmware to Arduino^</td^>^<td^>%step17Status%^</td^>^</tr^>

echo ^</table^>
echo ^</body^>
echo ^</html^>

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
echo ^<li^>Baud Rate: %baudRate%^</li^>
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
