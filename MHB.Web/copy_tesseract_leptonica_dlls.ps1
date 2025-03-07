$files = Get-ChildItem 'C:\Jenkins\workspace\MyHomeBills\MHB.Web\My Project\PublishProfiles\*.pubxml' -recurse

$binFolder = 'bin'

foreach ($file in $files) 
{
	$file = [xml](Get-Content $file)
	
	$dest = $file.Project.PropertyGroup.publishUrl + '\'  + $binFolder
	
	XCOPY C:\Jenkins\workspace\MyHomeBills\MHB.ReceiptScanner\x64\*.dll $dest /y /s
}