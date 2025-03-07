:: NOTE: Adjust paths on different installations
:: !!! NOT WORKING >
:: "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\signtool.exe" sign /f C:\PROJECTS\MHB.Licensing\Source\Televic.pfx /p "/@BJEF`FnG>R4$4z" BouncyCastle.Crypto.dll
:: !!! NOT WORKING <
:: "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe" -i Televic.pfx VS_KEY_4E33499D9F26C185

:: /@BJEF`FnG>R4$4z

:: Disassemble BouncyCastle.Crypto.dll
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\ildasm.exe" /all /out=BouncyCastle.Crypto.il BouncyCastle.Crypto.dll

:: Extract public key to snk required to use with ilasm
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -p D:\PROJECTS\MHB.Licensing\Source\Televic.pfx D:\PROJECTS\MHB.Licensing\Source\Televic-publickey.snk

:: Assemble and sign with the latter produced .snk public key
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe" /dll /key=D:\PROJECTS\MHB.Licensing\Source\Televic-publickey.snk BouncyCastle.Crypto.il
