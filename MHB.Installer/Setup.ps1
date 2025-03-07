echo off
clear

Function SetPermissions($installerRootDir)
{
    echo ("Granting permissions to: {0}...`n" -f $installerRootDir)

    $Acl = Get-Acl $installerRootDir

    $Ar = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")

    $Acl.SetAccessRule($Ar)

    Set-Acl $installerRootDir $Acl
    
    Write-Host ("Permissions to: {0} set successfully`n" -f $installerRootDir) -ForegroundColor "Green"
} 

Function InstallNETFramework($products)
{
    echo "Checking if .NET Framework 3.5 is installed...`n"
    $netFramework35SP1=($products | Where-Object -FilterScript {$_.Name -like "Microsoft .NET Framework 3.5*"} | Format-List -Property Name)

    if($netFramework35SP1 -eq $null)
    {
        # Secondary check:
        $exists = Test-Path (Join-Path $env:windir "Microsoft.NET\Framework\v3.5")

        if(-not $exists)
        {
            echo ".NET Framework 3.5 is not installed. Installing...`n"        
            #Start-Process "DotNetFx35Client.exe" -ArgumentList "/q /passive /lang:ENU" -Wait
            Start-Process "Dism.exe" -ArgumentList "/online /enable-feature /featurename:NetFX3 /All /Source:microsoft-windows-netfx3-ondemand-package.cab /LimitAccess" -Wait
            Write-Host ".NET Framework 3.5 installed successfully" -ForegroundColor "Green"            
        }
        else 
        {
            Write-Host ".NET Framework 3.5 already installed`n" -ForegroundColor "Green"
        }
    }

    echo "Checking if .NET Framework 4.5.2 is installed...`n"
    $netFramework452=($products | Where-Object -FilterScript {$_.Name -like "Microsoft .NET Framework 4.5.2*"} | Format-List -Property Name)

    if($netFramework452 -eq $null)
    {
        # Secondary check:
        $val = Get-ItemProperty -Path "hklm:HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" -Name "Release"

        if($val -eq $null -or ($val.Release -ne $null -and $val.Release -le 379893))
        {            
            echo ".NET Framework 4.5.2 is not installed. Installing...`n"
            Start-Process "NDP452-KB2901907-x86-x64-AllOS-ENU.exe" -ArgumentList "/passive /norestart" -Wait            
            Write-Host ".NET Framework 4.5.2 installed successfully" -ForegroundColor "Green"
        }
        else
        {
            Write-Host ".NET Framework 4.5.2 already installed`n" -ForegroundColor "Green"
        }
    }
}

Function InstallIISExpress($products)
{
    $iisExpress = Get-Process iisexpress -ErrorAction SilentlyContinue
    echo "Checking if IIS Express is running...`n"
    
    if($iisExpress -eq $null)
    {
        echo "IIS Express is not running. Checking if IIS Express is installed...`n"
        
        $iisExpress=($products | Where-Object -FilterScript {$_.Name -like "*IIS Express*" -or $_.Name -like "*IIS 10.0 Express*"} | Format-List -Property Name)

        if($iisExpress -eq $null)
        {
            echo "IIS Express is not installed. Installing...`n"
            Start-Process "msiexec" -ArgumentList "/i iisexpress_amd64_en-US.msi /passive" -Wait
            Write-Host "IIS Express install has ended`n" -ForegroundColor "Green"            
        }
    }
    else 
    {
        Write-Host "IIS Express already installed. Moving on...`n" -ForegroundColor "Green"        
    }
}

Function InstallLocalDB($products)
{
    echo "Checking if SQL Server LocalDB is installed...`n"
    $localDB=($products | Where-Object -FilterScript {$_.Name -like "*LocalDB*"} | Format-List -Property Name)

    if($localDB -eq $null)
    {
        echo "SQL Server LocalDB is not installed. Installing...`n"
        Start-Process "msiexec" -ArgumentList "/i SqlLocalDB.msi IACCEPTSQLLOCALDBLICENSETERMS=YES /passive" -Wait
        Write-Host "SQL Server LocalDB installed successfully`n" -ForegroundColor "Green"        
    }
}

Function InstallMSChards()
{
    Start-Process "MSChart.exe" -Wait
}

Function InstallODBCDrivers()
{
    echo "Checking if Microsoft ODBC Driver 11 for SQL Server is installed...`n"
    $odbcDrivers=($products | Where-Object -FilterScript {$_.Name -eq "Microsoft ODBC Driver 11 for SQL Server"} | Format-List -Property Name)

    if($odbcDrivers -eq $null)
    {
        echo "Microsoft ODBC Driver 11 for SQL Server is not installed. Installing..."
        
        Start-Process "msiexec" -ArgumentList "/quiet /passive /i msodbcsql.msi IACCEPTMSODBCSQLLICENSETERMS=YES ADDLOCAL=ALL" -Wait
        Write-Host "Microsoft ODBC Driver 11 for SQL Server installed successfully`n" -ForegroundColor "Green"                
    }      
}

Function InstallCommandLineUtilitiesForSQLServer($products)
{    
    echo "Checking if Microsoft Command Line Utilities 11 for SQL Server is installed...`n"
    $odbcDrivers=($products | Where-Object -FilterScript {$_.Name -eq "Microsoft Command Line Utilities 11 for SQL Server"} | Format-List -Property Name)

    if($odbcDrivers -eq $null)
    {
        echo "Microsoft Command Line Utilities 11 for SQL Server is not installed. Installing...`n"
        Start-Process "msiexec" -ArgumentList "/quiet /passive /i MsSqlCmdLnUtils.msi IACCEPTMSSQLCMDLNUTILSLICENSETERMS=YES" -Wait        
        Write-Host "Microsoft Command Line Utilities 11 for SQL Server installed successfully`n" -ForegroundColor "Green"          
    } 
}

Function InstallIfNotExisting($installedProducts, $productName, $msiName)
{    
    echo ("Checking if {0} is installed...`n" -f $productName)

    $result=($products | Where-Object -FilterScript {$_.Name -eq $productName} | Format-List -Property Name)

    if($result -eq $null)
    {
        echo ("{0} is not installed. Installing...`n" -f $productName)
        Start-Process "msiexec" -ArgumentList ("/i `"{0}`" /passive" -f $msiName) -Wait
        Write-Host  ("{0} installed successfully`n" -f $productName) -ForegroundColor "Green"                  
    }   
}

Function InstallPrerequisites()
{       
    echo "Installing prerequisites...`n"

    $installedProducts=(Get-WmiObject -Class Win32_Product -Computer . )

    InstallNETFramework $installedProducts
    
    InstallIISExpress $installedProducts

    InstallLocalDB $installedProducts

    InstallIfNotExisting $installedProducts "Microsoft Enterprise Library 5.0" "Enterprise Library 5.0.msi"

    InstallIfNotExisting $installedProducts "IIS Url Rewrite Module 2" "rewrite_2.0_rtw_x64.msi"

    InstallODBCDrivers $installedProducts

    InstallCommandLineUtilitiesForSQLServer $installedProducts

    InstallMSChards

    Write-Host "Prerequisites install has ended.`n" -ForegroundColor "Green"
}

Function RestoreMyHomeBillsDatabase()
{
    echo "Restoring MyHomeBills database...`n"
    
    $restoreQuery=("RESTORE DATABASE Test01Db FROM DISK='{0}\MHB.bak' WITH
                    MOVE 'smetkieu_db1' TO 'C:\Program Files\MyHomeBills\Test01Db.mdf', 
                    MOVE 'smetkieu_db1_log' TO 'C:\Program Files\MyHomeBills\Test01Db_log.ldf'" -f (Resolve-Path .\).Path)

    
    & "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\SQLCMD.exe" -Q "DROP DATABASE Test01Db" -S "(LocalDB)\MSSQLLocalDB"

    Start-Sleep -s 5

    & "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\SQLCMD.exe" -Q $restoreQuery -S "(LocalDB)\MSSQLLocalDB"
    & "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\SQLCMD.exe" -Q "USE Test01Db DBCC SHRINKFILE (smetkieu_db1)" -S "(LocalDB)\MSSQLLocalDB"
    & "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\SQLCMD.exe" -Q "USE Test01Db DBCC SHRINKFILE (smetkieu_db1_log)" -S "(LocalDB)\MSSQLLocalDB"

    Write-Host "Database restored successfully!`n" -ForegroundColor "Green"    
}

Function CopyMyHomeBillsFiles($installerRootDir)
{    
    $currentPath=(Resolve-Path .\).Path

    if(Test-Path $installerRootDir)
    {
        Remove-Item $installerRootDir -Force -Recurse
        Write-Host ("Successfully deleted existing MyHomeBills folder at: [{0}]`n" -f $installerRootDir) -ForegroundColor "Green"        
    }

    Copy-Item $currentPath $installerRootDir -Recurse
    Write-Host ("Successfully copied installer files from: [{0}] to: [{1}]`n" -f $currentPath, $installerRootDir) -ForegroundColor "Green"    

    Copy-Item $currentPath\MyHomeBills.lnk C:\Users\$env:USERNAME\Desktop
    Write-Host "Created shortcut on desktop`n" -ForegroundColor "Green"
}

Function StartIISExpress()
{
    $MyHomeBillsPath= Join-Path $installerRootDir "MHB.Published"    
    Write-Host ("MyHomeBills is at: {0}`n" -f $MyHomeBillsPath) -ForegroundColor "Green"

    cd $installerRootDir

    $iisExpressProcessPath="C:\Program Files\IIS Express\iisexpress.exe"

    Write-Host ("Starting MyHomeBills using iisexpress.exe at: {0}`n" -f $iisExpressProcessPath) -ForegroundColor "Green"    
    $iisExpressProcessArguments='/path:"{0}"' -f $MyHomeBillsPath

    echo "Open browser window"
    & explorer http://localhost:8080

    echo "Starting IIS Express and MyHomeBills...`n"
    $iisExpressProcess= Start-Process $iisExpressProcessPath -ArgumentList $iisExpressProcessArguments -Wait

    Write-Host ("IIS Express quit with exit code: {0}`n" -f $iisExpressProcess.ExitCode) -ForegroundColor "Red"
}

Function CleanUpInstallationLeftovers($installerRootDir)
{
	echo "Removing setup leftovers..."
	Get-ChildItem $installerRootDir | Where-Object {
		$_.Name -notmatch 'MHB.Published' -and
		$_.Name -notmatch 'Test01Db.mdf' -and
		$_.Name -notmatch 'Test01Db_log.ldf' -and
		$_.Name -notmatch 'StartMyHomeBills.bat' -and
		$_.Name -notmatch 'MyHomeBills.lnk'
} | Foreach-Object {
    echo ("Deleting {0} ...`n" -f $_.Name)
    Remove-Item $_.FullName		
}
}
#cd C:\Users\DIR\Desktop\Installer

$installerRootDir="C:\Program Files\MyHomeBills"

InstallPrerequisites

CopyMyHomeBillsFiles $installerRootDir

SetPermissions $installerRootDir

RestoreMyHomeBillsDatabase

CleanUpInstallationLeftovers $installerRootDir

StartIISExpress