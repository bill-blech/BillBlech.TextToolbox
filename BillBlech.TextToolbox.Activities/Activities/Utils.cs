using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BillBlech.TextToolbox.Activities.Activities
{
    public class Utils
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
                WriteLogMessage($"Extract Text Between Tags: [Beg Word: '{StartTAG.Trim()}' End Word: '{EndTAG.Trim()}']");

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
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, regexOptionsoptions, displayLog);
        }

        #region Below Anchor Line

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
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

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

        #endregion

        #region Previous Anchor Line

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

        
        }

        //Extract Text Previous Anchor Text Array of Strings ***
        public static string[] ExtractTextLinePreviousAnchorArrayText(string InputString, string[] ArrayTAG, int linesAbove, int NumLines, bool displayLog, bool displayRegex)
        {

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract '{linesAbove}' Line(s) Above  Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}]'");

            //Build Regex Clause
            string SearchCriteria = null;

            //Star the Clause
            //string StartClause = @"(.*)\n.*";
            string StartClause = @"(.*";
            SearchCriteria = StartClause;

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

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {
                SearchCriteria += @"(?=.*\b" + MyWord + @"\b)";
            }

            //Finish the Clause
            SearchCriteria += @".*";

            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);
        }

        #endregion

        #region All Lines Below Start TAG Until End

        //Extract All Lines Below Start TAG Until End ***
        public static string[] ExtractTextAllLinesBelowStartTAGUntilEnd(string InputString, string StartTAG, bool displayLog, bool displayRegex)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Lines Below Anchor Word: [Anchor Word: '{StartTAG.Trim()}']");

            //Build Regex Clause
            string SearchCriteria = StartTAG + @".*\n((?:\n|.+)*)";

            //Display Log
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

        }

        //Extract All Lines Below Anchor Text Array of Strings
        public static string[] ExtractTextAllLinesBelowArrayTAGUntilEnd(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Line(s) Below Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}']");

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
            SearchCriteria += @".*\n((?:\n|.+)*)";
            //SearchCriteria += @"[\r\n]((?:[\r\n]|.)*)";
            //SearchCriteria += @".*$[\r\n]((?:[\r\n]|.)*)";
            //SearchCriteria += @"(.*)";

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);

        }

        #endregion

        #region All Lines Above Start TAG Until Beggining

        //Extract All Lines Above Start TAG Until Beginning
        public static string[] ExtractTextAllLinesAboveTextUntilStart(string InputString, string StartTAG, bool displayLog, bool displayRegex, bool includeAnchorWordsRow)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            string SearchCriteria = null;

            //Build Regex Clause
            if (includeAnchorWordsRow == true)
            {
                SearchCriteria = @"([\S+\n\r\s]+" + StartTAG + ".*)"; // Include first line
            }
            else
            {
                SearchCriteria = @"([\S+\n\r\s]+)\n.*" + StartTAG; // Do not include first line
            }

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

        }

        //Extract All Lines Below Anchor Text Array of Strings
        public static string[] ExtractTextAllLinesAboveArrayTAGUntilStart(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex, bool includeAnchorWordsRow)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Line(s) Above Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}]'");

            //Build Regex Clause
            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {

                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                SearchCriteria += @"(?=.*\b" + MyWordAdj + @"\b)";

            }

            //Finish the Clause
            if (includeAnchorWordsRow == true)
            {
                SearchCriteria = @"([\S+\n\r\s]+\n.*" + SearchCriteria + @".*)"; //include first line
            }
            else
            {
                SearchCriteria = @"([\S+\n\r\s]+)\n.*" + SearchCriteria; //Do not include first line
            }

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);

        }

        #endregion

        #region Extract Text until Blank Line

        //Extract All Lines Below Anchor Text Until Blank line 
        public static string[] ExtractAllLinesBelowAnchorTextUntilBlankline(string InputString, string StartTAG, bool displayLog, bool displayRegex, bool includeAnchorWordsRow)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract Text until Blank Line: [Anchor Word: '{StartTAG.Trim()}' Anchor Words Parameter: 'Any' Direction: 'Below' Include Anchor Words Row: '{includeAnchorWordsRow.ToString()}']");

            string SearchCriteria = null;

            //Set Search Criteria
            if (includeAnchorWordsRow == true)
            {
                //SearchCriteria = @"(.*" + StartTAG + @"(.|\n)*?)(?:(?:\n\r|\n|\r){3})"; //Include first line
                SearchCriteria = @"(.*" + StartTAG + @".*(.*(?:\r?\n(?!\r?\n).*)*))"; //Include first line
            }
            else
            {
                //SearchCriteria =  StartTAG + @".*\n((.|\n)*?)(?:(?:\n\r|\n|\r){3})"; //Do not include first line
                SearchCriteria = @".*" + StartTAG + @".*\n((.*(?:\r?\n(?!\r?\n).*)*))"; //Do not include first line
            }

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

        }

        //Extract All Lines Below Anchor Text Array of Strings Until Blank line 
        public static string[] ExtractAllLinesBelowAnchorArrayofTextUntilBlankline(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex, bool includeAnchorWordsRow)
        {

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract Text until Blank Line: [Anchor Words: '{String.Join(";", ArrayTAG)}' Anchor Words Parameter: 'All' Direction: 'Below' Include Anchor Words Row: '{includeAnchorWordsRow.ToString()}']");


            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {
                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                SearchCriteria += @"(?=.*\b" + MyWordAdj + @"\b)";

            }

            //Finish the Clause
            if (includeAnchorWordsRow == true)
            {

                SearchCriteria = @"(.*" + SearchCriteria + @".*(.*(?:\r?\n(?!\r?\n).*)*))"; //Include first line
            }
            else
            {

                SearchCriteria = @".*" + SearchCriteria + @".*\n((.*(?:\r?\n(?!\r?\n).*)*))"; //Do not include first line
            }


            //Display Regex
            if (displayRegex == true)
                Console.WriteLine($"Search Criteria: {SearchCriteria}");

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

        }

        //Extract All Lines Above Anchor Text Until Blank line 
        public static string[] ExtractAllLinesAboveAnchorTextUntilBlankline(string InputString, string StartTAG, bool displayLog, bool displayRegex, bool includeAnchorWordsRow)
        {
            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract Text until Blank Line: [Anchor Word: '{StartTAG.Trim()}' Anchor Words Parameter: 'Any' Direction: 'Above' Include Anchor Words Row: '{includeAnchorWordsRow.ToString()}']");


            string SearchCriteria = null;

            //Set Search Criteria
            if (includeAnchorWordsRow == true)
            {
                SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*)" + StartTAG + ".*)"; //Include first line
            }
            else
            {
                SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*))" + StartTAG; //Do not include first line
            }

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);
        }

        //Extract All Lines Above Anchor Text Array of Strings Until Blank line
        public static string[] ExtractAllLinesAboveAnchorArrayofTextUntilBlankline(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex, bool includeAnchorWordsRow)
        {

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract Text until Blank Line: [Anchor Words: '{String.Join(";", ArrayTAG)}' Anchor Words Parameter: 'All' Direction: 'Above' Include Anchor Words Row: '{includeAnchorWordsRow.ToString()}']");

            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {
                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                SearchCriteria += @"(?=.*\b" + MyWordAdj + @"\b)";

            }

            //Finish the Clause
            if (includeAnchorWordsRow == true)
            {
                SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*)" + SearchCriteria + ".*)"; //Include first line
            }
            else
            {
                SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*))" + SearchCriteria; //Do not include first line
            }


            //Display Regex
            if (displayRegex == true)
                Console.WriteLine($"Search Criteria {SearchCriteria}");

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);



            ////Get all Lines above Text Until Start
            //string[] OutputResults = ExtractTextAllLinesAboveArrayTAGUntilStart(InputString, ArrayTAG, false, false, includeAnchorWordsRow);

            ////Display Log
            //if (displayLog == true)
            //    WriteLogMessage($"Matches count: {OutputResults.Length}");

            ////Start the OutputVariable
            //string[] OutputArray = new string[OutputResults.Length];

            ////Start OutputArrayCounter
            //int j = 0;

            ////Loop through the OutputArray1 Array
            //foreach (string Text in OutputResults)
            //{

            //    //Populate OutputArray
            //    OutputArray[j] = Utils.ExtractTextAllLinesAboveUntilBlankLine(Text);

            //    //Update the Counter
            //    j++;

            //}

            ////Write Log Message
            //if (displayLog == true)
            //    WriteLogMessage("Results: " + Environment.NewLine + string.Join(Environment.NewLine, OutputArray));

            //return OutputArray;
        }

        //Extract All Lines Both Direction Anchor Text Between Blank Lines
        public static string[] ExtractAllLinesBothDirectionsAnchorTextBetweenBlanklines(string InputString, string StartTAG, bool displayLog, bool displayRegex)
        {
            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Set Search Criteria
            string SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*).*" + StartTAG + @".*(.*(?:\r?\n(?!\r?\n).*)*))";

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

        }

        //Extract All Lines Both Direction Anchor Text Array Between Blank Lines
        public static string[] ExtractAllLinesBothDirectionsAnchorArrayofTextBetweenBlanklines(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex)
        {
            string SearchCriteria = null;

            //Loop through the ArrayTAG
            foreach (string MyWord in ArrayTAG)
            {
                //Adjust Special Characters, if needed
                String MyWordAdj = AdjustSpecialCharacters(MyWord);

                SearchCriteria += @"(?=.*\b" + MyWordAdj + @"\b)";

            }

            //Set Search Criteria
            //SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*).*" + SearchCriteria + @".*(.*(?:\r?\n(?!\r?\n).*)*))";
            //SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*).*" + SearchCriteria +   @".*(.*(?:\r?\n(?!\r?\n).*)*))";
            SearchCriteria = @"(((?:\r?\n(?!\r?\n).*)*).*" + SearchCriteria + @".*((?:\r?\n(?!\r?\n).*)*))";

            //Display Regex
            if (displayRegex == true)
                Console.WriteLine($"Search Criteria {SearchCriteria}");

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);


            return null;
        }

        //Query Array of String
        public static string[] QueryArrayString(string[] InputArray, String[] SearchWords, bool displayLog)
        {

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Find Array Items: [Input Array: '{string.Join(";",InputArray)}' Search Words: '{string.Join(";", SearchWords)}]'");

            //string.Join(";", ArrayTAG)}

            string[] Results = null;
            int LenSourceArray = SearchWords.Length;
            int FilteredCount = 0;

            //Start Output variable
            System.Collections.Generic.IEnumerable<string> filtered = null;

            //Check Source Array Len
            switch (LenSourceArray)
            {

                //One Item Criteria Array
                case 1:
                    filtered = from line in InputArray where line.Contains(SearchWords[0]) select line;
                    break;
                //Two Items Criteria Array
                case 2:
                    filtered = from line in InputArray where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) select line;
                    break;
                //three Items Criteria Array
                case 3:
                    filtered = from line in InputArray where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) select line;
                    break;
                //Four Items Criteria Array
                case 4:
                    filtered = from line in InputArray where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3]) select line;
                    break;
                //Five Items Criteria Array
                case 5:
                    filtered = from line in InputArray
                               where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3])
                                  && line.Contains(SearchWords[4])
                               select line;
                    break;
                //Six Items Criteria Array
                case 6:
                    filtered = from line in InputArray
                               where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3])
                                  && line.Contains(SearchWords[4]) && line.Contains(SearchWords[5])
                               select line;
                    break;
                //Seven Items Criteria Array
                case 7:
                    filtered = from line in InputArray
                               where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3])
                                  && line.Contains(SearchWords[4]) && line.Contains(SearchWords[5]) && line.Contains(SearchWords[6])
                               select line;
                    break;
                //Eight Items Criteria Array
                case 8:
                    filtered = from line in InputArray
                               where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3])
                                  && line.Contains(SearchWords[4]) && line.Contains(SearchWords[5]) && line.Contains(SearchWords[6]) && line.Contains(SearchWords[7])
                               select line;
                    break;
                //Nine Items Criteria Array
                case 9:
                    filtered = from line in InputArray
                               where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3])
                                  && line.Contains(SearchWords[4]) && line.Contains(SearchWords[5]) && line.Contains(SearchWords[6]) && line.Contains(SearchWords[7])
                                  && line.Contains(SearchWords[8])
                               select line;
                    break;
                //Ten Items Criteria Array
                case 10:
                    filtered = from line in InputArray
                               where line.Contains(SearchWords[0]) && line.Contains(SearchWords[1]) && line.Contains(SearchWords[2]) && line.Contains(SearchWords[3])
                                  && line.Contains(SearchWords[4]) && line.Contains(SearchWords[5]) && line.Contains(SearchWords[6]) && line.Contains(SearchWords[7])
                                  && line.Contains(SearchWords[8]) && line.Contains(SearchWords[9])
                               select line;
                    break;
                //Other
                default:
                    //Display Error Message
                    Console.WriteLine($" '{LenSourceArray}' Words were selected. The Maximumn allowed is 10");

                    //Return Empty Array as Output Criteria
                    string[] EmptyArray = { };
                    return EmptyArray;

                    break;

            }

            //Get filtered items count
            FilteredCount = filtered.Count();

            //Get Output Array
            Results = new string[FilteredCount];

            //Load OutputArray
            for (int i = 0; i < filtered.Count(); i++)
                Results[i] = filtered.ElementAt(i);

            //Write Log Message
            if (displayLog == true)
                WriteLogMessage("Results: " + Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine +
                                  string.Join(Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine, Results) +
                                              Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------");

            return Results;

        }

        //Match item in Array of String
        public static bool MatchItemInArrayOfStrings(string[] InputArray, string[] SearchWords, bool displayLog)
        {

            bool isFound = false;

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Match Item in Array: [Input Array : '{string.Join(";", InputArray)}' Search Words: '{string.Join(";", SearchWords)}]'");


            //Start Output variable
            System.Collections.Generic.IEnumerable<string> filtered = null;

            //Loop through the Words
            foreach(string SearchWord in SearchWords)
            {
                //Check if item is found in the Array
                filtered = from line in InputArray where line.Equals(SearchWord) select line;

                if (filtered.Count() > 0)
                {
                    isFound = true;

                    goto ExitLoop;


                }
               
            }

            isFound = false;

        ExitLoop:

            //Log Message
            if (displayLog == true)
                WriteLogMessage("Results: " + isFound.ToString());

                return isFound;
        }

        //Extract All Lines Above Anchor Text Until Blank Line (No used)
        public static string ExtractTextAllLinesAboveUntilBlankLine(string Text)
        {

            //Start the Variables
            int i = 0;
            int counter = 0;
            string lineText = null;

            //Split the Items via New Lines
            string[] lines = Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            #region Search Empty Line

            //Loop through the lines (backwards)
            for (int x = lines.Length; x > 0; x--)
            {
                //line index
                i = x - 1;

                //line text
                lineText = lines[x - 1];

                //Exit the Loop
                if (lineText.Length == 0)
                {
                    goto ExitLoop;
                }

                //Update the Counter
                counter++;

            }

        #endregion

        ExitLoop:

            #region Build OutputArray

            //Build OutputArray

            //Start the Counter
            int j = 0;

            //Start the Output vairable
            string OutputString = null;

            //Loop through the Array Indexes
            for (int z = i + 1; z < lines.Length; z++)
            {

                //Get line Text from lines array
                lineText = lines[z];

                //Add Data to Output String
                if (j == 0)
                {
                    //Start the Variable
                    OutputString = lineText;
                }
                else
                {
                    //Start the Variable
                    OutputString += Environment.NewLine + lineText;
                }

                //Update the Counter
                j++;
            }

            #endregion

            return OutputString;

        }

        #endregion

        //Extract all Characters until next White Space
        public static string[] ExtractAllCharactersUntilWhiteSpace(string InputString, string StartTAG, bool displayLog, bool displayRegex)
        {

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract Text Until White Space Tags: [Anchor Words: '{StartTAG.Trim()}']");

            //Set Search Criteria
            string SearchCriteria = StartTAG + @"(.\S*)";

            //Display Regex
            if (displayRegex == true)
                WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

            //Run Regex Extraction
            return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.Multiline, displayLog);

        }

        ////Extract all Characters until next Letter Character
        //public static string[] ExtractAllCharactersUntilLetterCharacter(string InputString, string StartTAG, bool displayLog, bool displayRegex)
        //{

        //    //Adjust Special Characters, if needed
        //    StartTAG = AdjustSpecialCharacters(StartTAG);

        //    //Set Search Criteria
        //    string SearchCriteria = StartTAG + @"((.|\n)*?)[a-zA-Z]";

        //    //Display Regex
        //    if (displayRegex == true)
        //        WriteLogMessage("Regex Expression: " + Environment.NewLine + SearchCriteria);

        //    //Run Regex Extraction
        //    return RunRegexExtraction(InputString, SearchCriteria, RegexOptions.None, displayLog);

        //}

        //Extract all Characters until next Letter Character
        public static string[] ExtractAllCharactersUntilLetterCharacter(string InputString, string StartTAG, bool displayLog)
        {

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extrat Text Until Next Letter: [Search Word: '{StartTAG}']");

            int StartIndex = 0;
            int Index = 0;
            bool isChar = false;
            int ItemCounter = 0;

            //Adjust Special Characters, if needed
            StartTAG = AdjustSpecialCharacters(StartTAG);

            //Count Occurences in the Text
            int Counter = Utils.CountSpecificWordInString(InputString, StartTAG);
            //Console.WriteLine("Counter: " + Counter.ToString());

            //Start Output Variable
            string[] Results = new string[Counter];

            //Case Items are found
            if (Counter > 0)
            {

                //Get All Occurences in the Text
                List<int> LstPosition = AllIndexesOf(InputString, StartTAG);

                //Loop through the Positions
                foreach (int i in LstPosition)
                {
                    //Start the Index
                    Index = i + StartTAG.Length;
                    StartIndex = Index;

                    //Clear TAG Variable
                    isChar = false;

                    //Console.WriteLine("Start Index: " + Index);

                    //Do While Character is not a Letter
                    while (isChar == false)
                    {

                        //Update the Counter
                        Index++;

                        //Get the Character
                        string MyCharacter = InputString.Substring(Index, 1);

                        //Console.WriteLine(MyCharacter);

                        //Check if it is a Letter
                        isChar = Regex.IsMatch(MyCharacter, @"[a-zA-Z]");

                    }

                    //Console.WriteLine("End Index: "+ Index);

                    //Build Output String
                    string OutputString = InputString.Substring(StartIndex + 1, Index - StartIndex - 1);
                    //Console.WriteLine(OutputString);

                    //Fill in Output Array
                    Results[ItemCounter] = OutputString.Trim();

                    //Update the Counter
                    ItemCounter++;

                }

                //Write Log Message
                if (displayLog == true)
                {
                    //Counter
                    Console.WriteLine($"Matches count: {Results.Length}");

                    //Result
                    WriteLogMessage("Results: " + Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine +
                                          string.Join(Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine, Results) +
                                                      Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------");

                }

                return Results;

            }

            //Return Null Array
            string[] NullArray = new string[0];
            return NullArray;

        }

        //Extract Last Line Below Anchor Text Array of Strings 
        public static string[] ExtractTextLastLinesBelowArrayTAGUntilEnd(string InputString, string[] ArrayTAG, bool displayLog, bool displayRegex)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Extract All Line(s) Below Anchor Words: [Anchor Words: '{string.Join(";", ArrayTAG)}]'");

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

        //Run Regex Expression **
        public static string[] RunRegexExtraction(string InputString, string SearchCriteria, RegexOptions regexOptionsoptions, bool displayLog)
        {

            //Run Regex Extraction
            MatchCollection matches = Regex.Matches(InputString, SearchCriteria, regexOptionsoptions | RegexOptions.IgnoreCase);

            //Write Log Message
            if (displayLog == true)
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
                    WriteLogMessage("Results: " + Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine +
                                      string.Join(Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine, Results) +
                                                  Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------");

                return Results;

            }
            else
            {
                string[] NullArray = new string[0];
                return NullArray;
            }
        }

        #endregion


        #region Remove / Replace Text
        //Remove Words from Text
        public static string RemoveWordsFromText(string InputString, string[] RemoveWords, string OccurrencesText, int IndexOccurence, bool displayLog)
        {

            //Fill in Text Occurence Variable
            EnumTextOccurrence eEnumTextOccurrence = CallExtractions.ReturnEnumTextOccurrence(OccurrencesText);

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Remove Words: [Words: '{string.Join(";", RemoveWords)}']");

            //Start Output Variable
            string OuputString = InputString;

            //Occurrence Index Validation
            switch (eEnumTextOccurrence)
            {

                //Custom
                case EnumTextOccurrence.Custom:
                    if (IndexOccurence == 0)
                    {

                        //Error Message
                        WriteLogMessage("No Occurence postion was supplied");

                        //Exit the Procedure
                        return OuputString;
                    }


                    break;
            }

            //Loop through the Words
            foreach (string Word in RemoveWords)
            {
                //Start the Counter
                int i = 0;

                switch (eEnumTextOccurrence)
                {
                    //All Occurences
                    case EnumTextOccurrence.All:

                        //Find all occurences for the Word
                        List<int> indexes = Utils.AllIndexesOf(OuputString, Word);

                        //Loop through the Indexes
                        foreach (int MyIndex1 in indexes)
                        {
                            //Remove Word from string
                            OuputString = OuputString.Remove(MyIndex1 - Word.Length * (i), Word.Length);

                            //Update the Counter
                            i++;
                        }

                        break;

                    //First Occurence
                    case EnumTextOccurrence.First:

                        //Get First Index
                        int MyIndex2 = OuputString.IndexOf(Word);

                        //Checkf if Index is found
                        if (MyIndex2 != -1)
                        {
                            //Remove Word from String
                            OuputString = OuputString.Remove(MyIndex2, Word.Length);

                            //Log Message: Success
                            if (displayLog == true)
                                WriteLogMessage($"Word '{Word}' removed sucessfully");

                        }
                        else
                        {
                            //Log Message: Fail
                            if (displayLog == true)
                                WriteLogMessage($"Word '{Word}' not removed");
                        }

                        break;

                    //Last Occurence
                    case EnumTextOccurrence.Last:

                        //Get Last Index
                        int MyIndex3 = OuputString.LastIndexOf(Word);

                        //Checkf if Index is found
                        if (MyIndex3 != -1)
                        {
                            //Remove Word from String
                            OuputString = OuputString.Remove(MyIndex3, Word.Length);

                            //Log Message: Success
                            if (displayLog == true)
                                WriteLogMessage($"Word '{Word}' removed sucessfully");
                        }
                        else
                        {
                            //Log Message: Fail
                            if (displayLog == true)
                                WriteLogMessage($"Word '{Word}' not removed");
                        }

                        break;

                    //Custom Occurence
                    case EnumTextOccurrence.Custom:

                        //Find all occurences for the Word
                        List<int> indexes1 = Utils.AllIndexesOf(OuputString, Word);

                        //Check if Item exists in the List
                        if (IndexOccurence <= indexes1.Count)
                        {

                            //Get the Index
                            int MyIndex4 = indexes1[IndexOccurence - 1];

                            //Remove Word from String
                            OuputString = OuputString.Remove(MyIndex4, Word.Length);

                            //Log Message: Success
                            if (displayLog == true)
                                WriteLogMessage($"Word '{Word}' removed sucessfully");

                        }
                        else
                        {
                            //Log Message: Fail
                            if (displayLog == true)
                                WriteLogMessage($"Word '{Word}' not removed");
                        }


                        break;

                }


                //End For Each Word Loop
            }


            //Write Log
            if (displayLog == true)
                WriteLogMessage("Result Text:" + Environment.NewLine + OuputString);

            //Return Text Variable
            return OuputString;

        }

        //Replace Word from Text
        public static string ReplaceWordsFromText(string InputString, string[] SearchWords, string ReplacedWord, string OccurrencesText, int IndexOccurence, bool displayLog)
        {

            string LogMessage = null;

            //Fill in Text Occurence Variable
            EnumTextOccurrence eEnumTextOccurrence = CallExtractions.ReturnEnumTextOccurrence(OccurrencesText);

            //Start Log Message
            LogMessage = $"Replace Words: [Search Word: '{string.Join(";",SearchWords)}' Replaced Word '{ReplacedWord}' ";

            //Start Output Variable
            string OuputString = InputString;

            //Occurrence Index Validation
            switch (eEnumTextOccurrence)
            {

                //Custom
                case EnumTextOccurrence.Custom:
                    if (IndexOccurence == 0)
                    {

                        //Error Message
                        WriteLogMessage("No Occurence postion was supplied");

                        //Exit the Procedure
                        return OuputString;
                    }


                    break;
            }

            //Loop through Search Words
            foreach(string SearchWord in SearchWords)
            {

                int i = 0;
                bool bSuccess = false;

                //Create Dummy Word
                string DummyWord = CreateDummyWord(SearchWord.Length);

                switch (eEnumTextOccurrence)
                {
                    //All Occurences
                    case EnumTextOccurrence.All:

                        //Update Log Message
                        LogMessage += "Occurrence: 'All' ";

                        //Find all occurences for the Word
                        List<int> indexes = Utils.AllIndexesOf(InputString, SearchWord);

                        //Case items are found
                        if (indexes.Count > 0)
                        {
                            bSuccess = true;

                            //Update Log Message
                            LogMessage += $"Replaced: '{indexes.Count}' time(s) ";
                        }
                        else
                        {
                            //Update Log Message
                            LogMessage += $"No replacements ";
                        }

                        //Loop through the Indexes
                        foreach (int MyIndex1 in indexes)
                        {

                            //Remove Word from string
                            OuputString = OuputString.Remove(MyIndex1, SearchWord.Length);

                            //Add Dummy Word
                            OuputString = OuputString.Insert(MyIndex1, DummyWord);

                            //Update the Counter
                            i++;

                        }

                        break;

                    //First Occurence
                    case EnumTextOccurrence.First:

                        //Update Log Message
                        LogMessage += "Occurrence: 'First' ";

                        //Get First Index
                        int MyIndex2 = InputString.IndexOf(SearchWord);

                        //Case it is found
                        if (MyIndex2 != -1)
                        {
                            //Remove Word from String
                            OuputString = OuputString.Remove(MyIndex2, SearchWord.Length);

                            //Add Dummy
                            OuputString = OuputString.Insert(MyIndex2, DummyWord);

                            //Set TAG Sucess = True
                            bSuccess = true;

                            //Update Log Message
                            LogMessage += $"Replaced: '1' time(s) ";

                        }
                        else
                        {
                            //Update Log Message
                            LogMessage += $"No replacements ";
                        }

                        break;

                    //Last Occurence
                    case EnumTextOccurrence.Last:

                        //Update Log Message
                        LogMessage += "Occurrence: 'Last' ";

                        //Get Last Index
                        int MyIndex3 = InputString.LastIndexOf(SearchWord);

                        //Case it is found
                        if (MyIndex3 != -1)
                        {
                            //Remove Word from String
                            OuputString = OuputString.Remove(MyIndex3, SearchWord.Length);

                            //Add Dummy
                            OuputString = OuputString.Insert(MyIndex3, DummyWord);

                            //Set TAG Sucess = True
                            bSuccess = true;

                            //Update Log Message
                            LogMessage += $"Replaced: '1' time(s) ";

                        }
                        else
                        {
                            //Update Log Message
                            LogMessage += $"No replacements ";
                        }


                        break;

                    //Custom Occurence
                    case EnumTextOccurrence.Custom:

                        //Update Log Message
                        LogMessage += $"Custom Occurrence: '{IndexOccurence}' ";

                        //Find all occurences for the Word
                        List<int> indexes1 = Utils.AllIndexesOf(InputString, SearchWord);

                        //Check if Item exists in the List
                        if (IndexOccurence <= indexes1.Count)
                        {

                            //Get the Index
                            int MyIndex4 = indexes1[IndexOccurence - 1];

                            //Case it is found
                            if (MyIndex4 != -1)
                            {
                                //Remove Word from String
                                OuputString = OuputString.Remove(MyIndex4, SearchWord.Length);

                                //Add Dummy
                                OuputString = OuputString.Insert(MyIndex4, DummyWord);

                                //Set TAG Sucess = True
                                bSuccess = true;

                                //Update Log Message
                                LogMessage += $"Replaced: '1' time(s) ";

                            }

                        }
                        else
                        {
                            //Update Log Message
                            LogMessage += $"No replacements ";
                        }

                        break;

                }

                //Replace the Dummy Word with Replace Word

                //Check if change was made
                if (bSuccess == true)
                {
                    //Replace the Dummy Word
                    OuputString = OuputString.Replace(DummyWord, ReplacedWord);
                }

            }

            //Finish the Message
            LogMessage += "]";

            //Log Message
            if (displayLog == true)
                Console.WriteLine(LogMessage);

            return OuputString.Trim();
        }

        //Create Dummy Word
        public static string CreateDummyWord(int Lenght)
        {
            string DummyWord = null;

            for (int i = 1; i <= Lenght; i++)
            {
                DummyWord += "@";
            }

            return DummyWord;
        }

        //Text Occurence
        public enum EnumTextOccurrence
        {
            Null,
            All,
            First,
            Last,
            Custom,
        }

        #endregion

        //Split Text by Blank Lines
        public static string[] SplitTextByBlankLines(string InputString)
        {

            string[] OutputArray = InputString.Split(new string[] { "\r\n\r\n" },
                               StringSplitOptions.RemoveEmptyEntries);

            return OutputArray;
        }

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
        public static string[] SplitTextBigSpaces(string InputRowText, int NullValLimit, bool bSupressNulls, bool displayLog)
        {

            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Split Text Uneven Blank Spaces: [Null Limit: '{NullValLimit.ToString()}' bSupressNulls: '{bSupressNulls.ToString()}']");

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

            //Write Log Message
            if (displayLog == true)
                WriteLogMessage("Results: " + Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine +
                                  string.Join(Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine, OutputArraWords) +
                                              Environment.NewLine + "-----------------------------------------------------------------------------------------------------------------------------------------------");

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
            return Regex.Matches(InputText, Regex.Escape(Word),RegexOptions.IgnoreCase).Count;
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
                    WriteLogMessage("'" + SearchWord + "' counter: " + Counter);

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
                WriteLogMessage($"Percentage of words found: {SearchWordsResult.ToString("P", CultureInfo.InvariantCulture)}");

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
            string AdjString = MyString.Trim();

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

        //Find All Word Occurrences in String
        //https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp
        public static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static string TrimFilePath(string initialPath, string absolutePath)
        {
            if (initialPath.StartsWith(absolutePath))
            {
                return initialPath.Remove(0, absolutePath.Length).TrimStart('\\');
            }


            return initialPath;
        }

        //Remove Empty Rows from Text File (Create Activity)
        public static void TextFileRemoveEmptyRows(string FilePath, Encoding encoding)
        { 
            string InputText = null;

            //Read Text File

            if (encoding== Encoding.Default)
            {
                InputText = System.IO.File.ReadAllText(FilePath);
            }
            else
            {
                InputText = System.IO.File.ReadAllText(FilePath, encoding);
            }

            //MessageBox.Show(InputText);

            //Remove Empty Rows
            InputText = Utils.TextRemoveEmptyRows(InputText);

            if (encoding == Encoding.Default)
            {
                //Write New File without empty rows
                System.IO.File.WriteAllText(FilePath, InputText);
            }
            else
            {
                //Write New File without empty rows
                System.IO.File.WriteAllText(FilePath, InputText, encoding);
            }
                
            //Read Text File
            InputText = System.IO.File.ReadAllText(FilePath, encoding);

            //MessageBox.Show(InputText);

        }

        //Return Default Separator
        public static string DefaultSeparator()
        {
            return "@#$%";
        }

        //Return Default Update Control Text
        public static string DefaultUpdateControl()
        {
            return "Click on the button to update the control>>>>>>";
        }

        //Read Text File with Encoding Option
        public static string ReadTextFileEncoding(string FilePath, string strEncoding, bool displayLog)
        {
            //Log Message
            if (displayLog == true)
                WriteLogMessage($"Read Text File Encoding: [File Path: '{FilePath.Trim()}' Encoding: '{strEncoding}]'");

            //Get Encoding
            Encoding encoding = ConvertStringToEncoding(strEncoding);

            string InputText = System.IO.File.ReadAllText(FilePath, encoding);

            //Log Message
            if (displayLog == true)
                WriteLogMessage("Result Text: " + Environment.NewLine + InputText);

            //Read Text File
            return InputText;

        }

        public static Encoding ConvertStringToEncoding(string strEncoding)
        {
            Encoding encoding = Encoding.Default;

            //Chech Encoding Variable
            switch (strEncoding)
            {

                //Default
                case "Default":
                    encoding = Encoding.Default;
                    break;
                //UTF8
                case "UTF8":
                    encoding = Encoding.UTF8;
                    break;
                //ASCII
                case "ASCII":
                    encoding = Encoding.ASCII;
                    break;
                //iso-8859-1
                case "iso-8859-1":
                    encoding = Encoding.GetEncoding("iso-8859-1");
                    break;
            }

            return encoding;

        }

        //Convert Collection to Array
        public static string[] ConvertCollectionToArray(Collection<string> InputCollection)
        {

            string[] OutputResults = new string[InputCollection.Count()];

            //Loop through the Collection
            for (int i = 0; i < InputCollection.Count(); i++)
            {

                //Get Item from the Collection
                OutputResults[i] = InputCollection.ElementAt(i);

            }

            return OutputResults;

        }

        ////https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        ////Read Text File Using File Stream
        //public static string ReadTextFileStream(string FilePath)
        //{
        //    using (StreamReader sr = new StreamReader(FilePath, true))
        //    {
        //        while (sr.Peek() >= 0)
        //        {
        //            sr.Read();

        //        }

        //        //Get Encoding
        //        Encoding encoding = sr.CurrentEncoding;

        //        //Read Text File
        //        string source = System.IO.File.ReadAllText(FilePath, encoding);

        //        return source;

        //    }

        //}


    }
}
