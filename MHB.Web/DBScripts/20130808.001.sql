UPDATE tbLanguage SET ControlTextEN = 'Expected cost difference (to date):', ControlTextBG = 'Разлика планирани/бюджет (до дата):', ControlTextDE = 'Erwartete Kosten Unterschied (bis heute):' WHERE ControlID = 'LabelExpectedCostSumDifferenceText'

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE)
VALUES ('LabelExpectedCostTotalSumDifferenceText', 'Expected cost difference (total):', 'Разлика планирани/общo бюджет:', 'Erwartete Kosten Unterschied (total):')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE)
VALUES (
'LabelExpectedCostTotalSumDifferenceTooltip', 
'Displays the difference between budget and planned expenditures as the budget amount is calculated based on ALL INCOME ENTRIES entered in the budgets section REGARDLESS OF INCOME DATE', 
'Показва разликата между бюджет и планирани разходи, като сумата на бюджета е изчислена от всички въведени суми в полето бюджет, БЕЗ да се взима ПРЕДВИД ДАТАТА НА ПОЛУЧАВАНЕ', 
'Zeigt die Differenz zwischen Budget und geplanten Ausgaben, gemäß dem Gesamtbetrag, unabhängig von Einkommen eingegebene Datum berechnet')