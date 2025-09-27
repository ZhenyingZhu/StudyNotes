@echo off
echo Building Azure MSAL Example...
echo.

echo Restoring Server dependencies...
cd Server
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo Failed to restore Server dependencies
    pause
    exit /b 1
)

echo Building Server...
dotnet build --no-restore
if %ERRORLEVEL% neq 0 (
    echo Failed to build Server
    pause
    exit /b 1
)

cd ..

echo Restoring Client dependencies...
cd Client
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo Failed to restore Client dependencies
    pause
    exit /b 1
)

echo Building Client...
dotnet build --no-restore
if %ERRORLEVEL% neq 0 (
    echo Failed to build Client
    pause
    exit /b 1
)

cd ..

echo.
echo Build completed successfully!
echo.
echo To run the applications:
echo 1. Start the server: cd Server && dotnet run
echo 2. In another terminal, start the client: cd Client && dotnet run
echo.
pause