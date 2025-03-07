INSERT INTO tbLanguage VALUES ('LabelAddToShoppingListAmount', 'Count:', 'Брой:', 'Anzahl:')

INSERT INTO tbLanguage VALUES ('LabelFilterByNameText', 'Search by name:', 'Търси по име:', 'Suche nach Name:')

INSERT INTO tbLanguage VALUES ('LabelFilterByCategoryText', 'Filter by category:', 'Филтрирай по категория:', 'Nach Kategorie filtern:')

INSERT INTO tbLanguage VALUES ('LabelFilterBySupplierText', 'Filter by supplier:', 'Филтрирай по доставчик:', 'Filtern nach Lieferant:')

INSERT INTO tbLanguage VALUES ('ButtonFilter', 'Filter', 'Филтрирай', 'Filtern')

DELETE FROM tbLanguage WHERE ControlID = 'LinkButtonShowShoppingListDialog'

UPDATE tbLanguage SET ControlID = 'LinkButtonShowShoppingListDialog' where ControlID = 'LinkButtonAddToList'

INSERT INTO tbLanguage VALUES ('DuplicateMonthRecordsDestMonthMatchesNotify', 'The target month matches current month! Please select a different month to prevent data loss.', 'Месецът в който желаете да копирате съвпада с текущия месец. Моля изберете друг месец за да избегнете загуба на данни.', 'Bitte wählen Sie einen anderen Zielmonat um Datenverlust zu vermeiden.')
