using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BillBlech.TextToolbox.Activities.Activities
{
    class Utils
    {
        #region Regex

        
        //https://stackoverflow.com/questions/7709337/extract-the-text-between-two-tag-in-c-sharp
        //https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.matchcollection.count?view=netcore-3.1

        //Extract Text Between two text TAGs ***
        public static string[] ExtractTextBetweenTags(string InputString, string StartTAG, string EndTAG, RegexOptions regexOptionsoptions, bool displayLog, bool displayRegex)
        {

            #region Adjust Start & End TAG
            //if (StartTAG == null) StartTAG = @"\n";
            //if (EndTAG == null) EndTAG = @"\n";

            //Start TAG
            if (StartTAG == null)
            {
                //Case it is null
                StartTAG = @"\n";
            }
            else
            {
                //Adjust Special Characters, if needed
                StartTAG = AdjustSpecialCharacters(StartTAG);
            }

            //End TAG
            if (EndTAG == null)
            {

                //Case it is null
                EndTAG = @"\n";
            }
            else
            {
                //Adjust Special Characters, if needed
                EndTAG = AdjustSpecialCharacters(EndTAG);
            }

            #endregion

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract Text Between Tags: [Beg Word: '{StartTAG.Trim()}' End Word: '{EndTAG.Trim()}]'");

            //Set the Search Criteria
            string SearchCriteria = null;

            switch (regexOptionsoptions)
            {

                //Same Line
                case RegexOptions.Multiline:
                    SearchCriteria = StartTAG + @"(.*)" + EndTAG;
                    break;

                //Different Lines
                case RegexOptions.Singleline:
                SearchCriteria = StartTAG + @"((.|\n)*?)" + EndTAG;
                    break;

            }

            //Write Regex Expression
            if (displayRegex ==true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, regexOptionsoptions, displayLog);
        }

        //Extract Text Below Anchor Line ***
        public static string[] ExtractTextLineBelowAnchorText(string InputString, string StartTAG, int linesBelow, int NumLines, bool displayLog, bool displayRegex)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract '{linesBelow}' Line(s) Below Anchor Word: [Anchor Word: '{StartTAG.Trim()}']'");

            //Build Regex Clause

            //Star the Clause
            string SearchCriteria = StartTAG;

            //Lines Below
            for (int x = 0; x < linesBelow; x++)
            {
                SearchCriteria += @".*\n";
            }

            //Intermediate
            SearchCriteria += @"(.*";

            //Number of lines
            for (int z = 1; z < NumLines; z++)
            {
                SearchCriteria += @"\n.*";
            }

            //Finish the Clause
            SearchCriteria += @")";

            //Write Regex Expression
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None,displayLog);

            ////Get Matches via Regex
            //MatchCollection matches = Regex.Matches(InputString, SearchCriteria);

            //if (displayLog==true)
            //    Console.WriteLine($"Matches count: {matches.Count}");

            ////In case items are found
            //if (matches.Count > 0)
            //{

            //    //Start the Counter
            //    int i = 0;

            //    //Start the Output Array
            //    string[] Results = new string[matches.Count];

            //    //Loop through the Matches
            //    foreach (Match match in matches)
            //    {

            //        ////Add Item to Output Array
            //        //Results[i] = match.Groups[1].Value;

            //        //Get Item from Regex
            //        string MyString = match.Groups[1].Value.Trim();

            //        //Split the Items via New Lines
            //        string[] lines = MyString.Split(
            //        new[] { Environment.NewLine },
            //        StringSplitOptions.RemoveEmptyEntries
            //        );

            //        ////Add Item to Output Array
            //        Results[i] = lines[lines.Length - 1].ToString().Trim();

            //        //Update the counter
            //        i++;

            //    }

            //    //Write Log Message
            //    if(displayLog == true)
            //        WriteLogMessage("Results: " + Environment.NewLine + string.Join(Environment.NewLine, Results));

            //    return Results;

            //}
            //else
            //{
            //    string[] NullArray = new string[0];
            //    return NullArray;
            //}
        }

        //Extract Text Below Anchor Text Array of Strings ****
        public static string[] ExtractTextLineBelowAnchorArrayText(string InputString, string[] ArrayTAG, int linesBelow, int NumLines, bool displayLog, bool displayRegex)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract '{linesBelow}' Line(s) Below  Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}]'");

            //Build Regex Clause
            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {

                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                if (SearchCriteria == null)
                {
                    SearchCriteria = @"^(?=.*\b" + MyWordAdj + @"\b)";
                }
                else
                {
                    SearchCriteria = SearchCriteria + @"(?=.*\b" + MyWordAdj + @"\b)";
                }
            }

            //Finish the Clause First Part
            SearchCriteria += @".*$";

            //Lines Bellow
            for (int x = 0; x < linesBelow; x++)
            {
                SearchCriteria += @".*\n";
            }

            //Intermediate
            SearchCriteria += @"(.*";

            //Number of lines
            for (int z = 1; z < NumLines; z++)
            {
                SearchCriteria += @"\n.*";
            }

            //Finish the Clause
            SearchCriteria += @")";


            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);

        }


        //https://forum.omz-software.com/topic/4526/how-to-capture-the-line-before-a-regex-match
        //Extract Text Previous Anchor Line ***
        public static string[] ExtractTextLinePreviousAnchorText(string InputString, string StartTAG, int linesAbove, int NumLines, bool displayLog, bool displayRegex)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract '{linesAbove}' Line(s) Above Anchor Word: [Anchor Word: '{StartTAG.Trim()}']'");

            //Build Regex Clause

            //Star the Clause
            string StartClause = @"(.*";
            string SearchCriteria = StartClause;

            //Number of lines
            for (int z = 1; z < NumLines; z++)
            {
                SearchCriteria += @"\n.*";
            }

            //Finish First Part
            SearchCriteria += ")";

            //Lines Above
            for (int x = 0; x < linesAbove; x++)
            {
                SearchCriteria += @"\n.*";
            }

            //Finish the Clause
            SearchCriteria = SearchCriteria + StartTAG + ".*";

            //Write Regex Expression
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);

            ////Get Matches via Regex

            ////string SearchCriteria = StartTAG + "\n(.+)";
            ////string SearchCriteria = StartTAG + "[\r\n]+([^\r\n]+)";
            ////string SearchCriteria = "(.*)\n.*" + StartTAG + ".*";

            //MatchCollection matches = Regex.Matches(InputString, SearchCriteria);

            //if (displayLog == true)
            //    Console.WriteLine($"Matches count: {matches.Count}");

            ////In case items are found
            //if (matches.Count > 0)
            //{

            //    //Start the Counter
            //    int i = 0;

            //    //Start the Output Array
            //    string[] Results = new string[matches.Count];

            //    //Loop through the Matches
            //    foreach (Match match in matches)
            //    {

            //        //Get Item from Regex
            //        string MyString = match.Groups[1].Value.Trim();

            //        //Add Item to Output Array
            //        Results[i] = MyString;

            //        //Update the counter
            //        i++;

            //    }

            //    //Write Log Message
            //    if (displayLog == true)
            //        WriteLogMessage("Results: " + Environment.NewLine + string.Join(Environment.NewLine, Results));

            //    return Results;

            //}
            //else
            //{
            //    string[] NullArray = new string[0];
            //    return NullArray;
            //}
        }

        
        //All Lines Below Start TAG Until End ***
        public static string[] ExtractTextAllLinesBelowStartTAGUntilEnd(string InputString, string StartTAG, bool displayLog, bool displayRegex)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Lines Below Anchor Word: [Anchor Word: '{StartTAG.Trim()}']");

            //Set the Search Criteria
            string SearchCriteria = StartTAG + @".*\n((?:\n|.+)*)";

            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None,displayLog);

        }

        //Extract Last Line Below Anchor Text Array of Strings 
        public static string[] ExtractTextLastLinesBelowArrayTAGUntilEnd(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Line(s) Below  Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}]'");

            //Build Regex Clause
            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {

                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                if (SearchCriteria == null)
                {
                    SearchCriteria = @"^(?=.*\b" + MyWordAdj + @"\b)";
                }
                else
                {
                    SearchCriteria = SearchCriteria + @"(?=.*\b" + MyWordAdj + @"\b)";
                }
            }

            //Finish the Clause First Part
            SearchCriteria += @".*$";

            //Finish the Clause
            SearchCriteria += @"[\r\n]((?:[\r\n]|.)*)";

            //SearchCriteria += @".*$[\r\n]((?:[\r\n]|.)*)";

            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Get Matches via Regex
            MatchCollection matches = Regex.Matches(InputString, SearchCriteria, RegexOptions.Multiline);

            if (displayLog == true)
                Console.WriteLine($"Matches count: {matches.Count}");

            //In case items are found
            if (matches.Count > 0)
            {

                //Start the Counter
                int i = 0;

                //Start the Output Array
                string[] Results = new string[matches.Count];

                //Loop through the Matches
                foreach (Match match in matches)
                {
                    //Get Item from Regex
                    string MyString = match.Groups[1].Value.Trim();

                    //Split the Items via New Lines
                    string[] lines = MyString.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries
                    );

                    //Add Item to Output Array
                    Results[i] = lines[lines.Length - 1];

                    //Update the counter
                    i++;

                }

                //Write Log Message
                if (displayLog == true)
                    WriteLogMessage("Results: " + Environment.NewLine + string.Join(Environment.NewLine, Results));

                return Results;

            }
            else
            {
                string[] NullArray = new string[0];
                return NullArray;
            }

        }

        //Extract All Lines Below Anchor Text Array of Strings
        public static string[] ExtractTextAllLinesBelowArrayTAGUntilEnd(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Line(s) Below  Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}]'");

            //Build Regex Clause
            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {

                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                if (SearchCriteria == null)
                {
                    SearchCriteria = @"^(?=.*\b" + MyWordAdj + @"\b)";
                }
                else
                {
                    SearchCriteria = SearchCriteria + @"(?=.*\b" + MyWordAdj + @"\b)";
                }
            }

            //Finish the Clause First Part
            SearchCriteria += @".*$";

            //Finish the Clause
            SearchCriteria += @"[\r\n]((?:[\r\n]|.)*)";
            //SearchCriteria += @".*$[\r\n]((?:[\r\n]|.)*)";
            //SearchCriteria += @"(.*)";

            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);

        }

        //Run Regex Expression **
        public static string[] RunRegexExtraction(string InputString, string SearchCriteria, RegexOptions regexOptionsoptions, bool displayLog)
        {

            //Run Regex Extraction
            MatchCollection matches = Regex.Matches(InputString, SearchCriteria, regexOptionsoptions);

            //Write Log Message
            if (displayLog==true)
                WriteLogMessage($"Matches count: {matches.Count}");

            //In case items are found
            if (matches.Count > 0)
            {

                //Start the Counter
                int i = 0;

                //Start the Output Array
                string[] Results = new string[matches.Count];

                //Loop through the Matches
                foreach (Match match in matches)
                {
                    //Get Item from Regex
                    string MyString = match.Groups[1].Value.Trim();

                    //Add Item to Output Array
                    Results[i] = MyString;

                    //Update the counter
                    i++;

                }

                //Write Log Message
                if (displayLog == true)
                    WriteLogMessage("Results: " + Environment.NewLine + string.Join(Environment.NewLine, Results));

                return Results;

            }
            else
            {
                string[] NullArray = new string[0];
                return NullArray;
            }
        }

        #endregion

        //Find Text Cordinates
        public static int[] FindTextCordinates(string InputString, string SearchText)
        {
            //Start Results Array
            int[] Results = new int[2];

            //Start the Variable
            int StartPosition = -1;

            //Get Start Position
            StartPosition = InputString.IndexOf(SearchText);

            if (StartPosition != -1)
            {

                //Fill in the Variables
                Results[0] = StartPosition;
                Results[1] = StartPosition + SearchText.Length;

                //Log Message
                WriteLogMessage(string.Join(" ", Results));
            }

            return Results;
        }

        //Extract Text Via Cordinates
        public static string ExtractTextViaCordinates(string InputString, int startCordinate, int EndCordinate)
        {

            //Get the SubString
            String substring = InputString.Substring(startCordinate, EndCordinate);

            //Log Message
            WriteLogMessage(substring);

            //return Result;
            return substring;

        }

        //Split Text using 'Big Spaces'
        public static string[] SplitTextBigSpaces(string InputRowText, int NullValLimit, bool bSupressNulls)
        {

            //Declare Variables
            string MyWord = null;
            int MyNullCounter = 0;
            bool bValidCharFound = false;

            //Create a list of strings
            List<string> ListWords = new List<string>();

            //Loop through the Characters
            foreach (char c in InputRowText)
            {
                //Get the Character
                string MyCharact = c.ToString();

                //Check if Character is null
                if (MyCharact != " ")
                {
                    //Set the TAG = True
                    bValidCharFound = true;
                    //Clear the Counter
                    MyNullCounter = 0;
                }
                else
                {
                    //Upgrade the Counter
                    MyNullCounter++;
                }

                //Add Character to Word
                MyWord += MyCharact;

                //Add Character to Words Collection
                if (MyNullCounter == NullValLimit)
                {

                    //Check Supress Nulls variable
                    if (bSupressNulls == false)
                    {
                        //Add item to List
                        ListWords.Add(MyWord.Trim());
                    }
                    else
                    {
                        //In case at least one Char is found
                        if (bValidCharFound == true)
                        {
                            //Add item to List
                            ListWords.Add(MyWord.Trim());
                        }
                    }

                    //Clear the Variables
                    MyWord = null;
                    MyNullCounter = 0;
                    bValidCharFound = false;

                }

            }

            //In case at least one Char is found
            if (bValidCharFound == true)
            {
                //Add item to List
                ListWords.Add(MyWord.Trim());
            }

            //Add Items to Output Array
            int i = 0;
            string[] OutputArraWords = new string[ListWords.Count];

            //Loop through the Words
            foreach (string Word in ListWords)
            {

                //Add Item to the Array
                OutputArraWords[i] = Word.Trim();

                //Update the Counter
                i++;
            }

            return OutputArraWords;

        }
        
        //Remove Empty Rows from String
        public static string TextRemoveEmptyRows(string Text)
        {
            string Result = Regex.Replace(Text, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();

            return Result;
        }

        //Count Specific Word Occurences in a String
        public static int CountSpecificWordInString(string InputText, string Word)
        {
            return Regex.Matches(InputText, Regex.Escape(Word)).Count;
        }

        //Find Words in String
        public static double FindWordsInString(string InputText, string[] SearchWords, bool displayLog)
        {
            double SearchWordsFound = 0;

            //Write Log Message
            if (displayLog == true)
                WriteLogMessage("Count Words in Text");

            //GetArray Lenght
            double SearchWordsCount = SearchWords.Length;

            //Loop through the Array
            foreach (string SearchWord in SearchWords)
            {

                //Search Text in String
                int Counter = Utils.CountSpecificWordInString(InputText, SearchWord);

                //Write Log Message
                if (displayLog == true)
                    WriteLogMessage("'" + SearchWord + "' counter :" + Counter);
                
                if (Counter >= 1)
                {
                    //Updgrade the Counter
                    SearchWordsFound++;
                }
            }

            //Set Percentage Result
            double SearchWordsResult = SearchWordsFound / SearchWordsCount;

            //Write Log Message
            if (displayLog == true)
                WriteLogMessage($"Percentage of words found: {SearchWordsResult}");

            return SearchWordsResult;
        }

        //Split String by New Line
        public static string[] SplitTextNewLine(string MyInputText)
        {
            //Split the Items via New Lines
            string[] lines = MyInputText.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.RemoveEmptyEntries
            );

            return lines;

        }

        //Insert "\" before any special character
        public static string AdjustSpecialCharacters(string MyString)
        {
            //Adjust the Variable
            string AdjString = MyString;

            Dictionary<string, string> SpecialChars =
            new Dictionary<string, string>();

            string MyChar;

            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;\<>,";
            foreach (var MyChar1 in specialChar)
            {

                //Check if each Char is a special character
                if (MyString.Contains(MyChar1) == true)
                {

                    bool bExists = SpecialChars.ContainsKey(MyChar1.ToString());

                    //Add Item to the Dictionary, in case it is not there
                    if (bExists == false)
                        SpecialChars.Add(MyChar1.ToString(), MyChar1.ToString());

                }

            }

            ////Set the Searching Criteria
            //string SearchCriteria = @"[^a-zA-Z0-9_. ]";

            //MatchCollection matches = Regex.Matches(MyString, SearchCriteria);

            //if (matches.Count > 0)
            //{

            //    //Loop through the Matches
            //    foreach (Match match in matches)
            //    {

            //        //Get item from the Collection
            //        MyChar = match.Value;

            //        bool bExists = SpecialChars.ContainsKey(MyChar);

            //        //Add Item to the Dictionary, in case it is not there
            //        if (bExists == false)
            //            SpecialChars.Add(MyChar, MyChar);

            //        //Console.WriteLine("Found '{0}' at position {1}",
            //        //match.Value, match.Index);

            //    }

            //Loop through the Dictionary
            foreach (KeyValuePair<string, string> kvp in SpecialChars)
                {

                    //Get item from the Dictionary
                    MyChar = kvp.Key.ToString();


                    AdjString = AdjString.Replace(MyChar, @"\" + MyChar);


                    //Console.WriteLine("Key = {0}, Value = {1}",
                    //    kvp.Key, kvp.Value);
                }
                ////Console.WriteLine(AdjString);

            //}

            return AdjString;
        }

        //Remove Last Row
        public static string RemoveLastRow(string MyString)
        {

            //Count the Number of lines
            int numLines = MyString.Split('\n').Length;

            //Remove LastRow
            MyString = MyString.Remove(MyString.TrimEnd().LastIndexOf(Environment.NewLine));

            return MyString;

        }


        //Write Log Message
        public static void WriteLogMessage(string LogMessage)
        {
            Console.WriteLine(DateTime.Now.ToString() + " " + LogMessage);
        }

        //Update Data Row
        public static System.Data.DataRow CallUpdateDataRow2(System.Data.DataRow row, string Col, String Val)
        {
            //Utils.WriteLogMessage($"Column: {Col} Value: {Val}");

            row[Col] = Val;

            return row;
        }

    }
}
