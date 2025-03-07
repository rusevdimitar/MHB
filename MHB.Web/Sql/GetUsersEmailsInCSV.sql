declare @maillist varchar(8000)
declare @s varchar(50)
declare @mailcount int

set @maillist = ''


declare cur cursor for 
	select email from tbusers
	
	open cur
	
	fetch next from cur into @s
	
	while @@fetch_status = 0
	begin 
	
	set @maillist = @maillist + ';' + @s
	
	fetch next from cur into @s
	end
	
	close cur
	deallocate cur
	
	print @maillist
