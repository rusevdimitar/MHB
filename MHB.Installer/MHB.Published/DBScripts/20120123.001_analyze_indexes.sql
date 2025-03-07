DECLARE @IndexName VARCHAR(100)

SET @IndexName = 'IDX_UserID_mt2'

DECLARE @IndexID SMALLINT

SET @IndexID =

  (SELECT index_id FROM sys.indexes

    WHERE name = @IndexName)
    
    PRINT @IndexID

SELECT object_id AS ObjectID,

  index_id AS IndexID,

  avg_fragmentation_in_percent AS PercentFragment,

  fragment_count AS TotalFrags,

  avg_fragment_size_in_pages AS PagesPerFrag,

  page_count AS NumPages

FROM sys.dm_db_index_physical_stats(DB_ID('smetkieu_db1'),

  OBJECT_ID('dbo.tbMainTable01'), @IndexID, NULL , 'DETAILED')

WHERE avg_fragmentation_in_percent > 0

ORDER BY ObjectID, IndexID
