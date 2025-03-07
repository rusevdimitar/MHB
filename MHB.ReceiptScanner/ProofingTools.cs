using System;
using Microsoft.Office.Interop.Word;

using Word = Microsoft.Office.Interop.Word;

namespace MHB.ReceiptScanner
{
    public class ProofingTools
    {
        private readonly Word.Application _wordApp;
        private readonly Word.Document _wordDoc;

        public ProofingTools()
        {
            _wordApp = new Word.Application { Visible = false };
            _wordDoc = _wordApp.Documents.Add();
        }

        public void Close()
        {
            object obj = Word.WdSaveOptions.wdDoNotSaveChanges;
            _wordDoc.Close(ref obj);
            _wordApp.Quit(ref obj);
        }

        public string Proof(string proofText)
        {
            string result = proofText;

            _wordDoc.Content.Text = proofText;

            ProofreadingErrors spellingErrors = _wordDoc.Content.SpellingErrors;

            var language = _wordApp.Languages[WdLanguageID.wdBulgarian];

            foreach (Range spellingError in spellingErrors)
            {
                SpellingSuggestions spellingSuggestions = _wordApp.GetSpellingSuggestions(spellingError.Text, IgnoreUppercase: true, MainDictionary: language.Name);

                foreach (SpellingSuggestion spellingSuggestion in spellingSuggestions)
                {
                    result = result.Replace(spellingError.Text, spellingSuggestion.Name);

                    spellingError.Text = spellingSuggestion.Name;

                    break;
                }
            }

            return result;
        }
    }
}