using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTextCat;

namespace BillBlech.TextToolbox.Activities.Activities.Encryption
{
    public class LanguageDetection
    {

        //https://www.codeproject.com/Articles/43198/Detect-a-written-text-s-language
        //https://ivanakcheurov.github.io/ntextcat/
        //https://github.com/ivanakcheurov/ntextcat
        public static string RunDetectLanguage(string InputText, String ConfigFile)
        {

            string Result = null;
            // Don't forget to deploy a language profile (e.g. Core14.profile.xml) with your application.
            // (take a look at "content" folder inside of NTextCat nupkg and here: https://github.com/ivanakcheurov/ntextcat/tree/master/src/LanguageModels).
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load(ConfigFile.Replace("\\","/")); // can be an absolute or relative path. Beware of 260 chars limitation of the path length in Windows. Linux allows 4096 chars.
            var languages = identifier.Identify(InputText);
            var mostCertainLanguage = languages.FirstOrDefault();
            if (mostCertainLanguage != null)
                //Console.WriteLine("The language of the text is '{0}' (ISO639-3 code)", mostCertainLanguage.Item1.Iso639_3);
                Result = mostCertainLanguage.Item1.Iso639_3;

            else
                Result = "The language couldn’t be identified with an acceptable degree of certainty";
            //Console.WriteLine("The language couldn’t be identified with an acceptable degree of certainty");

            // outputs: The language of the text is 'eng' (ISO639-3 code)


            return Result;

        }
    }
}
