# Project description

MyHomeBills is a web-based system where monthly bills, purchased 
products and the deadline for their payment are registered; different 
expenditures can be planned so that the right sum for them can be 
set; planned sums can be compared with actually paid sums and 
reminders can be set. Receipts can be scanned and parsed using 
custom OCR. Expenditures and budgets are automatically categorized. Additional categories can be added and managed.
Various statistics and reports are generated. 

# Some technical details

- Machine bound custom licensing is implemented /MHB.Licensing
- Scanning and parsing of different receipt formats based on Tesseract OCR engine https://github.com/charlesw/tesseract/


# Requirements

- .NET 8.0
- .NET 3.5 SP1
- Microsoft Enterprise Library 5.0 (you can find it in the Prerequisites folder along with or download from https://www.microsoft.com/en-us/download/details.aspx?id=15104)
- Check out \MHB.Web\FAQ.txt, \MHB.Web\Changes.txt and Prerequisites\Setup.ps1 if any issues arrise during compilation


# Running the project

Currently supported environment:
- Windows 11
- Visual Studio 2022
- SQL Serves 2022
