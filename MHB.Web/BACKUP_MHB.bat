rmdir /s /q "E:\MHB.Web"

del /s /q C:\Users\mipka\Desktop\MHB.Web.rar


"C:\Program Files\WinRAR\rar.exe" a -m5 -ed -pTest C:\Users\mipka\Desktop\MHB.Web.rar C:\Users\mipka\Desktop\MHB.Web


XCOPY C:\Users\mipka\Desktop\MHB.Web.rar E:\ /y


SET date="%date:~10,4%-%date:~4,2%-%date:~7,2%"
md E:\MHB_SOURCE_BACKUP\%date%
 

XCOPY /h /r /k /y /S /E C:\Users\mipka\Desktop\MHB.Web E:\MHB_SOURCE_BACKUP\%date%

ECHO MHB Backup operations completed on: %Date% - %Time% >> E:\backup_log.txt

pause