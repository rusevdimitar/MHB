echo off
clear

Function SetPermissions
{
    echo ("Granting permissions to: {0}...`n" -f $installerRootDir)

    $Acl = Get-Acl $installerRootDir

    $Ar = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")

    $Acl.SetAccessRule($Ar)

    Set-Acl $installerRootDir $Acl

    echo ("Permissions to: {0} set successfully`n" -f $installerRootDir)
} 

Function InstallNETFramework($products)
{
    echo "Checking if .NET Framework 3.5 is installed..."
    $netFramework35SP1=($products | Where-Object -FilterScript {$_.Name -like "Microsoft .NET Framework 3.5*"} | Format-List -Property Name)

    if($netFramework35SP1 -eq $null)
    {
        echo ".NET Framework 3.5 is not installed. Installing..."        
        #Start-Process "DotNetFx35Client.exe" -ArgumentList "/q /passive /lang:ENU" -Wait
        Start-Process "Dism.exe" -ArgumentList "/online /enable-feature /featurename:NetFX3ServerFeatures /All /Source:\sxs /LimitAccess" -Wait
        Start-Process "Dism.exe" -ArgumentList "/online /enable-feature /featurename:NetFX3 /All /Source:\sxs /LimitAccess" -Wait
        echo ".NET Framework 3.5 intalled successfully"
    }

    echo "Checking if .NET Framework 4.5.2 is installed..."
    $netFramework452=($products | Where-Object -FilterScript {$_.Name -like "Microsoft .NET Framework 4.5.2*"} | Format-List -Property Name)

    if($netFramework452 -eq $null)
    {
        echo ".NET Framework 4.5.2 is not installed. Installing..."
        Start-Process "NDP452-KB2901907-x86-x64-AllOS-ENU.exe" -ArgumentList "/passive /norestart" -Wait
        echo ".NET Framework 4.5.2 intalled successfully"
    }
}

Function InstallIISExpress($products)
{
    $iisExpress = Get-Process iisexpress -ErrorAction SilentlyContinue
    echo "Checking if IIS Express is running...`n"
    
    if($iisExpress -eq $null)
    {
        echo "IIS Express is not running. Checking if IIS Express is installed...`n"
        
        $iisExpress=($products | Where-Object -FilterScript {$_.Name -like "*IIS Express*"} | Format-List -Property Name)

        if($iisExpress -eq $null)
        {
            echo "IIS Express is not installed. Installing...`n"
            Start-Process "msiexec" -ArgumentList "/i iisexpress_amd64_en-US.msi /passive" -Wait
            echo "IIS Express install has ended`n"
        }
    }
    else 
    {
        echo "IIS Express already installed. Moving on...`n"
    }
}

Function InstallLocalDB($products)
{
    echo "Checking if SQL Server LocalDB is installed..."
    $localDB=($products | Where-Object -FilterScript {$_.Name -like "*LocalDB*"} | Format-List -Property Name)

    if($localDB -eq $null)
    {
        echo "SQL Server LocalDB is not installed. Installing..."
        Start-Process "msiexec" -ArgumentList "/i SqlLocalDB.msi IACCEPTSQLLOCALDBLICENSETERMS=YES /passive" -Wait
        echo "SQL Server LocalDB installed successfully"
    }
}

Function InstallEnterpriseLibrary($products)
{
    echo "Checking if Microsoft Enterprise Library 5.0 is installed..."
    $enterpriseLibrary=($products | Where-Object -FilterScript {$_.Name -eq "Microsoft Enterprise Library 5.0"} | Format-List -Property Name)

    if($enterpriseLibrary -eq $null)
    {
        echo "Microsoft Enterprise Library 5.0 is not installed. Installing..."
        Start-Process "msiexec" -ArgumentList "/i `"Enterprise Library 5.0.msi`" /passive" -Wait
        echo "Microsoft Enterprise Library 5.0 installed successfully"
    }    
}

Function InstallIISRewriteModule($products)
{
    echo "Checking if IIS Url Rewrite Module 2 is installed..."
    $iisRewriteModule=($products | Where-Object -FilterScript {$_.Name -eq "IIS Url Rewrite Module 2"} | Format-List -Property Name)

    if($iisRewriteModule -eq $null)
    {
        echo "IIS Url Rewrite Module 2 is not installed. Installing..."
        Start-Process "msiexec" -ArgumentList "/i rewrite_2.0_rtw_x64.msi /passive" -Wait
        echo "IIS Url Rewrite Module 2 installed successfully"
    }        
}

Function InstallMSChards()
{
    Start-Process "MSChart.exe" -Wait
}

Function InstallPrerequisites()
{       
    echo "Installing prerequisites...`n"

    $installedProducts=(Get-WmiObject -Class Win32_Product -Computer . )

    InstallNETFramework $installedProducts
    
    InstallIISExpress $installedProducts

    InstallLocalDB $installedProducts

    InstallEnterpriseLibrary $installedProducts

    InstallIISRewriteModule $installedProducts

    InstallMSChards
    
    echo "Prerequisites install has ended.`n" 
}

#cd C:\Users\DIR\Desktop\Installer

InstallPrerequisites

$installerRootDir=Join-Path $env:ProgramFiles "MyHomeBills"

$currentPath=(Resolve-Path .\).Path

if(Test-Path $installerRootDir)
{
    Remove-Item $installerRootDir -Force -Recurse
    echo ("Successfully deleted existing MyHomeBills folder at: [{0}]`n" -f $installerRootDir)
}

Copy-Item $currentPath $installerRootDir -Recurse
echo ("Successfully copied installer files from: [{0}] to: [{1}]`n" -f $currentPath, $installerRootDir)

SetPermissions

$MyHomeBillsPath= Join-Path $installerRootDir "MHB.Published"
echo ("MyHomeBills is at: {0}`n" -f $MyHomeBillsPath)

cd $installerRootDir

$iisExpressProcessPath= Join-Path $env:ProgramFiles "IIS Express\iisexpress.exe"
echo ("Starting MyHomeBills using iisexpress.exe at: {0}`n" -f $iisExpressProcessPath)

$iisExpressProcessArguments='/path:"{0}"' -f $MyHomeBillsPath

echo "Starting IIS Express and MyHomeBills...`n"
$iisExpressProcess= Start-Process $iisExpressProcessPath -ArgumentList $iisExpressProcessArguments -Wait

echo ("IIS Express quit with exit code: {0}`n" -f $iisExpressProcess.ExitCode)