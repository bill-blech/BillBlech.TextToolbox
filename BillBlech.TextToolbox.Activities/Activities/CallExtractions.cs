using System.Text.RegularExpressions;
using static BillBlech.TextToolbox.Activities.Activities.Utils;

namespace BillBlech.TextToolbox.Activities.Activities
{
    public class CallExtractions
    {

        public enum EnumAnchorTextParam
        {
            Null,
            Any,
            All,

        }

        public enum EnumRegexParameter
        {
            Null,
            SameLine,
            DifferentLines

        }

        public static EnumAnchorTextParam ReturnAnchorTextParam(string anchorTextParamText)
        {
            //Fill in Variable anchorTextParam
            EnumAnchorTextParam anchorTextParam = EnumAnchorTextParam.Null;

            //Fill in Variable anchorTextParam
            switch (anchorTextParamText)
            {
                //Any
                case "Any":
                    anchorTextParam = EnumAnchorTextParam.Any;
                    break;
                //All
                case "All":
                    anchorTextParam = EnumAnchorTextParam.All;
                    break;
            }

            return anchorTextParam;
        }

        public static EnumTextOccurrence ReturnEnumTextOccurrence(string OccurrencesText)
        {
            EnumTextOccurrence Occurrences = EnumTextOccurrence.Null;

            //Fill in Variable Occurrences
            switch (OccurrencesText)
            {
                //All
                case "All":
                    Occurrences = EnumTextOccurrence.All;
                    break;
                //First
                case "First":
                    Occurrences = EnumTextOccurrence.First;
                    break;
                //Last
                case "Last":
                    Occurrences = EnumTextOccurrence.Last;
                    break;
                //Custom
                case "Custom":
                    Occurrences = EnumTextOccurrence.Custom;
                    break;
            }

            return Occurrences;

        }

        //Extract Text Between Two Anchor Words
        public static string[] CallExtractTextBetweenTwoAnchorWords(string inputText, string[] begWords, string[] endWords, string regexParameterText, bool displayLog, bool displayRegex)
        {

            string[] OutputResults = null;

            //Fill in regexParameter
            EnumRegexParameter regexParameter = EnumRegexParameter.Null;

            //Fill in Variable anchorTextParam
            switch (regexParameterText)
            {
                //Same Line
                case "Same Line":
                    regexParameter = EnumRegexParameter.SameLine;
                    break;
                //All
                case "Different Lines":
                    regexParameter = EnumRegexParameter.DifferentLines;
                    break;
            }

            //Start the Variable
            RegexOptions regexOptionsoptions = RegexOptions.None;

            //Fill in regexOptionsoptions
            switch (regexParameter)
            {
                //Same Line
                case EnumRegexParameter.SameLine:
                    regexOptionsoptions = RegexOptions.Multiline;
                    break;
                //Different Lines
                case EnumRegexParameter.DifferentLines:
                    regexOptionsoptions = RegexOptions.Singleline;
                    break;

            }

            #region BegWords & End Words
            if (begWords != null && endWords != null)
            {
                //Loop through the Beggining Words
                foreach (string begWord in begWords)
                {
                    //Loop through Ending Words
                    foreach (string endWord in endWords)
                    {

                        //Search the Text
                        OutputResults = Utils.ExtractTextBetweenTags(inputText, begWord, endWord, regexOptionsoptions, displayLog, displayRegex);

                        //Exit the Loop in case result is found
                        if (OutputResults.Length > 0)
                        {
                            //Exit the Loop
                            goto ExitLoop;
                        }

                    }
                }
            }

            #endregion

            #region BegWords Only
            if (begWords != null && endWords == null)
            {
                //Loop through the Beggining Words
                foreach (string begWord in begWords)
                {
                    //Search the Text
                    OutputResults = Utils.ExtractTextBetweenTags(inputText, begWord, null, regexOptionsoptions, displayLog, displayRegex);

                    //Exit the Loop in case result is found
                    if (OutputResults.Length > 0)
                    {
                        //Exit the Loop
                        goto ExitLoop;
                    }

                }
            }

            #endregion

            #region EndWords Only
            if (begWords == null && endWords != null)
            {
                //Loop through the Beggining Words
                foreach (string endWord in endWords)
                {
                    //Search the Text
                    OutputResults = Utils.ExtractTextBetweenTags(inputText, null, endWord, regexOptionsoptions, displayLog, displayRegex);

                    //Exit the Loop in case result is found
                    if (OutputResults.Length > 0)
                    {
                        //Exit the Loop
                        goto ExitLoop;
                    }

                }
            }

        #endregion

        ExitLoop:
            return OutputResults;

        }

        //Extract all Lines Below Anchor Text
        public static string[] CallExtractAllLinesBelowAnchorText(string inputText, string[] anchorText, string anchorTextParamText, bool displayLog, bool displayRegex)
        {

            string[] OutputResults = null;

            //Get Anchor Text Parameter
            EnumAnchorTextParam anchorTextParam = ReturnAnchorTextParam(anchorTextParamText);

            //Check the Variable
            switch (anchorTextParam)
            {

                //Any of the Anchor Words
                case EnumAnchorTextParam.Any:

                    //Loop through the Words
                    foreach (string Word in anchorText)
                    {
                        OutputResults = Utils.ExtractTextAllLinesBelowStartTAGUntilEnd(inputText, Word, displayLog, displayRegex);

                        //Exit the Loop in case result is found
                        if (OutputResults.Length > 0)
                        {
                            //Exit the Loop
                            goto ExitLoop;
                        }
                    }

                    break;
                //All of the Anchor Words
                case EnumAnchorTextParam.All:

                    //Extract All Lines Below Anchor Text Array of Strings 
                    OutputResults = Utils.ExtractTextAllLinesBelowArrayTAGUntilEnd(inputText, anchorText, displayLog, displayRegex);

                    break;

            }

        ExitLoop:
            return OutputResults;

        }

        //Extract Text Above Anchor Words
        public static string[] CallExtractTextAboveAnchorWords(string inputText, string[] anchorWords, string anchorTextParamText, int LinesAbove, int NumLines, bool displayLog, bool displayRegex)
        {
            string[] OutputResults = null;

            //Get Anchor Text Parameter
            EnumAnchorTextParam anchorTextParam = ReturnAnchorTextParam(anchorTextParamText);

            switch (anchorTextParam)
            {
                //Any of the Anchor Words
                case EnumAnchorTextParam.Any:

                    //Loop through the Words
                    foreach (string Word in anchorWords)
                    {
                        //Loop through last words
                        OutputResults = Utils.ExtractTextLinePreviousAnchorText(inputText, Word, LinesAbove, NumLines, displayLog, displayRegex);

                        //Exit the Loop in case result is found
                        if (OutputResults.Length > 0)
                        {
                            //Exit the Loop
                            goto ExitLoop;
                        }
                    }

                    break;


                //All of the Anchor Words
                case EnumAnchorTextParam.All:

                    //Extract text Below Line Array Tag
                    OutputResults = Utils.ExtractTextLinePreviousAnchorArrayText(inputText, anchorWords, LinesAbove, NumLines, displayLog, displayRegex);

                    break;

            }


        ExitLoop:
            return OutputResults;


        }

        //Extract Text Below Anchor Words
        public static string[] CallExtractTextBelowAnchorWords(string inputText, string[] anchorWords, string anchorTextParamText, int LinesBelow, int NumLines, bool displayLog, bool displayRegex)
        {
            string[] OutputResults = null;

            //Get Anchor Text Parameter
            EnumAnchorTextParam anchorTextParam = ReturnAnchorTextParam(anchorTextParamText);

            switch (anchorTextParam)
            {
                //Any of the Anchor Words
                case EnumAnchorTextParam.Any:

                    //Loop through the Words
                    foreach (string Word in anchorWords)
                    {
                        //Extract text Below Line Text
                        OutputResults = Utils.ExtractTextLineBelowAnchorText(inputText, Word, LinesBelow, NumLines, displayLog, displayRegex);

                        //Exit the Loop in case result is found
                        if (OutputResults.Length > 0)
                        {
                            //Exit the Loop
                            goto ExitLoop;
                        }
                    }

                    break;
                //All of the Anchor Words
                case EnumAnchorTextParam.All:

                    //Extract text Below Line Array Tag
                    OutputResults = Utils.ExtractTextLineBelowAnchorArrayText(inputText, anchorWords, LinesBelow, NumLines, displayLog, displayRegex);

                    break;
            }

        ExitLoop:
            return OutputResults;
        }

        //Extract all Characters until next White Space (Create Activity)
        public static string[] CallExtractAllCharactersUntilWhiteSpace(string inputText, string[] anchorWords, bool displayLog, bool displayRegex)
        {

            string[] OutputResults = null;

            //Loop through the Words
            foreach (string Word in anchorWords)
            {
                //Extract text Below Line Text
                OutputResults = Utils.ExtractAllCharactersUntilWhiteSpace(inputText, Word, displayLog, displayRegex);

                //Exit the Loop in case result is found
                if (OutputResults.Length > 0)
                {
                    //Exit the Loop
                    goto ExitLoop;
                }
            }

        ExitLoop:
            return OutputResults;
        }

    }
}