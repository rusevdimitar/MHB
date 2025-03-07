using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class Translation : TranslationBase
    {
        public string ControlID { get; set; }

        public Enums.Language SelectedLanguage { get; set; }

        public string CurrentTranslation
        {
            get
            {
                switch (this.SelectedLanguage)
                {
                    case Enums.Language.Bulgarian:
                        return this.Bulgarian;
                    case Enums.Language.English:
                        return this.English;
                    case Enums.Language.German:
                        return this.German;
                    default:
                        return this.English;
                }
            }
        }

        public string English { get; set; }

        public string German { get; set; }

        public string Bulgarian { get; set; }
    }
}
