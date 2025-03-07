#most important step that lost me hours it that the StartCom IIS certificate was in certstorename WebHosting and I copied it to Personal and then executed all this;

#dir cert:\localmachine\WebHosting

----------------

#netsh http delete sslcert ipport=0.0.0.0:443 maybe also execute that one too
netsh http delete sslcert ipport=[::]:443

netsh http show sslcert

#get this hash from the error message on the client in eventviewer
netsh http add sslcert ipport=[::]:443 certhash=EDB9F978013180EE3F72CC0E48FB459576910C52 appid='{214124cd-d05b-4309-9af9-9caa44b2b74a}' certstorename=MY

reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SstpSvc\Parameters /v SHA1CertificateHash /t REG_BINARY /d EDB9F978013180EE3F72CC0E48FB459576910C52 /f

reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SstpSvc\Parameters /v SHA256CertificateHash /t REG_BINARY /d FFEB24457314DB7EE131783FA28E14DF2C91C34CD81BFDE5693E7D2F6D0FBC18 /f


net stop sstpsvc /y
net start remoteaccess