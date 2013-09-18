using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class ktResourceTranslation
    {
        public string LanguageID { get; set; }
        public string ResourceID { get; set; }
        public string TranslationText { get; set; }

        public ktResourceTranslation()
        {
            LanguageID = "";
            ResourceID = "";
            TranslationText = "";
        }

    }
}
