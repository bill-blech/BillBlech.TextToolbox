using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Activities.Encryption;
using ExcelTut;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace BillBlech.TextToolbox.Activities.Design
{
    class DesignUtils
    {
        public static string TrimFilePath(string initialPath, string absolutePath)
        {
            if (initialPath.StartsWith(absolutePath))
            {
                return initialPath.Remove(0, absolutePath.Length).TrimStart('\\');
            }


            return initialPath;
        }

        // List the words in the file. (Create Activity)
        public static string[] GetUniqueWordsInFile(string filePath, Encoding encoding)
        {
            //Open Text File
            String txt = System.IO.File.ReadAllText(filePath, encoding);
            //String txt = Utils.ReadTextFileStream(filePath);

            //MessageBox.Show(txt);

            string[] words = txt.Split();

            // Use LINQ to get the unique words.
            var word_query =
                (from string word in words
                 orderby word
                 select word).Distinct();

            // Display the result.
            string[] result = word_query.ToArray();

            return result;

        }

        //Generate ID Text
        public static string GenerateIDText()
        {
            string MyIDText = null;

            //Get Now function
            string MyNow = DateTime.Now.ToString();

            //Encrypt
            MyIDText = Cipher.Encrypt(MyNow, "BillBlech");

            //Remove Special Characters
            MyIDText = RemoveSpecialCharacters(MyIDText);

            return MyIDText;

        }

        //Write Text Files
        public static void WriteTextFileSingleFile(string FilePath, string MyIDText)
        {

            //Get the File Name
            string fileName = Path.GetFileNameWithoutExtension(FilePath);

            //File Path
            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames/" + fileName + ".txt", FilePath);

            //File Path for Preview
            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FilePathPreview/" + MyIDText + ".txt", FilePath);

        }

        //Remove Special Characters (Create Activity)
        public static string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex(
                          "(?:[^a-zA-Z0-9]|(?<=['\"])s)",
                          RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);

        }

        //https://stackoverflow.com/questions/18863187/how-can-i-loop-through-a-listt-and-grab-each-item
        //Delete Text File Row
        public static void DeleteTextFileRow(string filePath, string DeleteItem)
        {

            //Convert the Text File into a list of strings
            List<string> linesList = File.ReadAllLines(filePath).ToList();

            //Loop through the Lines
            for (int i = 0; i < linesList.Count; i++)
            {

                //Look for the delete item
                if (DeleteItem == linesList[i])
                {
                    //Delete Row
                    linesList.RemoveAt(i);
                    break;
                }

            }

            //Reset the Text File
            System.IO.File.WriteAllLines(filePath, linesList.ToArray());

        }

        //Delete Text File Row: Argument
        public static void DeleteTextFileRowArgument(string filePath, string MyArgument, Encoding encoding)
        {

            //Convert the Text File into a list of strings
            List<string> linesList = File.ReadAllLines(filePath,encoding).ToList();

            //Loop through the Lines
            for (int i = 0; i < linesList.Count; i++)
            {
                
                //Get Line
                string Line = linesList[i];

                if (Line.Length > 0)
                {
                    //Split the Line
                    string[] MyArray = Line.Split('@');

                    //Look for the delete item
                    if (MyArgument == MyArray[0])
                    {
                        //Delete Row
                        linesList.RemoveAt(i);

                        //Exit the Loop
                        break;
                    }
                }
                ////Remove Blank Line
                //else
                //{
                //    //Delete Row
                //    linesList.RemoveAt(i);
                //}

               
            }

            //Reset the Text File
            System.IO.File.WriteAllLines(filePath, linesList.ToArray(), encoding);

        }

        //Delete File
        public static bool DeleteStorageTextFileRun(string fileName)
        {
            //File Name
            File.Delete(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames/" + fileName + ".txt");

            return true;
        }

        //Open Form Select Data Open
        public static void CallformSelectDataOpen(string Label, string MyIDText, string TemplateFilePath, string MyIDTextParent, Encoding encoding)
        {

            //Get Unique Words in File
            //string[] ArrayAvailableItems = GetUniqueWordsInFile(FilePath);

            //string InputText = System.IO.File.ReadAllText(FilePath);
            //string[] ArrayAvailableItems = Utils.SplitTextNewLine(InputText);

            //Open Form
            FormSelectData formSelectData;
            formSelectData = new FormSelectData(Label, TemplateFilePath, MyIDText, MyIDTextParent, encoding);
            formSelectData.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            formSelectData.ControlBox = false;
            formSelectData.Show();

        }

        //Open Form Preview Extraction
        public static void CallformPreviewExtraction(string MyIDText, string Label, string MyIDTextParent, Encoding encoding)
        {
            //Open Form
            FormDisplayPreview formDisplayPreview;
            formDisplayPreview = new FormDisplayPreview(MyIDText, Label, MyIDTextParent, encoding);

        }

        //Log ComboBox
        public static void CallLogComboBox(string MyIDText, string MyArgument, string MyValue, Encoding encoding)
        {

            //Set Excel File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Check if File Exists
            bool bExists = File.Exists(FilePath);

            if (bExists == true)
            {
                //Update Text File Row Argument
                CallUpdateTextFileRowArgument(FilePath, MyArgument, MyValue, encoding);
            }

        }

        //Update Text File Row Argument
        public static void CallUpdateTextFileRowArgument(string FilePath, string MyArgument, string MyValue, Encoding encoding)
        {

            string OutputText = null;

            //Delete Previous Argument, in case found
            DesignUtils.DeleteTextFileRowArgument(FilePath, MyArgument, encoding);

            //Get files row count
            string inputText = System.IO.File.ReadAllText(FilePath);
            string[] Lines = Utils.SplitTextNewLine(inputText);

            //Read All Lines
            //string[] Lines = System.IO.File.ReadAllLines(FilePath);

            //Case there are lines there
            if (Lines.Length > 1)
            {
                OutputText += Environment.NewLine;

            }

            //Add Argument Name & Value
            OutputText += MyArgument + Utils.DefaultSeparator() + MyValue + Utils.DefaultSeparator() + DateTime.Now.ToString();

            //Store Info to TextFile
            System.IO.File.AppendAllText(FilePath, OutputText, encoding);
        }

        //'Convert' String to Array
        public static string[] ConvertStringToArray(string ArrayText, bool bReplaceAllDoubleQuotes)
        {
            //Remove Braces {} and "
            ArrayText = ArrayText.Replace("{", "");
            ArrayText = ArrayText.Replace("}", "");

            //Split the Items
            string[] ArrayOutput = ArrayText.Split(',');

            //Loop through the Array
            for (int i = 0; i<ArrayOutput.Length; i++)
            {
                ArrayOutput[i] = TextAdjustDoubleQuotes(ArrayOutput[i], bReplaceAllDoubleQuotes);
            }

            //ArrayText = TextAdjustDoubleQuotes(ArrayText, bReplaceAllDoubleQuotes);

            return ArrayOutput;
        }

        //Wizard Button: Warning Message: Wizard & Preview
        public static void Wizard_WarningMessage_Wizard_Preview()
        {
            //Warning Message
            MessageBox.Show("Please click the 'Warning Button' to Enable 'Wizard' and 'Preview' Functionalities" + Environment.NewLine + "Case you cannot see button, go to Text Application Scope and Preview the Text", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Wizard Button: Warning Message: Preview
        public static void Wizard_WarningMessage_Preview()
        {
            //Warning Message
            MessageBox.Show("Please click the 'Warning Button' to Enable 'Preview' Functionalities", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Create Default Storage Foldders
        public static void CreateStorageTextToolboxFolders()
        {
            //Check if folder "StorageTextToolbox" exists
            bool bExists = Directory.Exists(Directory.GetCurrentDirectory() + "/StorageTextToolbox");

            //Create folders in case it is not found
            if (bExists == false)
            {

                //Main Folder
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/StorageTextToolbox");

                //File Names
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames");

                //File Path for Preview
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FilePathPreview");

                //Infos
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos");

            }
        }

        //Paste Argument from the Clipboard
        public static string PasteArgumentFromClipboard()
        {

            string OutputText = null;

            //Get Text from the Clipboard
            string ClipboardText = Clipboard.GetText();

            //Case it is not Null
            if (ClipboardText != null)
            {
                //Get Words from the Text
                string[] Words = Strings.Split(ClipboardText, ",");

                //Loop through the Words
                foreach (string Word in Words)
                {

                    //Remove ", in case there is
                    //string WordAdj = Word.Replace("\"", "");
                    string WordAdj = TextAdjustDoubleQuotes(Word, true);

                    //First Word
                    if (OutputText == null)
                    {
                        OutputText = "\"" + WordAdj + "\"";
                    }
                    //Following Words
                    else
                    {
                        OutputText += ",\"" + WordAdj + "\"";
                    }


                }

                //Finish the Variable
                OutputText = "{" + OutputText + "}";

                return OutputText;

            }
            else
            {
                return null;
            }

        }

        //Remove First and Last Characters if they are "
        public static string TextAdjustDoubleQuotes(string InputString, bool bReplaceAllDoubleQuotes)
        {
            string OutputText = null;
            int MyLen = 0;

            //Start the Variable
            OutputText = InputString.Trim();

            Console.WriteLine(OutputText);
            MyLen = OutputText.Length;

            //Get FirstChar
            String FirstChar = Strings.Left(OutputText, 1);

            if (FirstChar == "\"")
            {
                OutputText = OutputText.Remove(0, 1);
            }

            //Get Last Char
            String LastChar = Strings.Right(OutputText, 1);
            
            if (LastChar == "\"")
            {
                MyLen = OutputText.Length;   
                OutputText = OutputText.Remove(MyLen - 1, 1);
            }

            if (bReplaceAllDoubleQuotes == true)
            {
                //Replace one Double Quote with two double quotes
                OutputText = OutputText.Replace("\"", "\"\"");
            }
            else
            {
                //Replace Pair of Double Quotes for only one Double Quote
                OutputText = OutputText.Replace("\"\"", "\"");
            }

            return OutputText;
        }

        //Replace Pair of Double Quotes for only one Double Quote
        public static string TextAdjustRemovePairDoubleQuotes(string InputString)
        {
            return InputString.Replace("\"\"", "\"");
        }

        //Return CurrentFileIDText
        public static string ReturnCurrentFileIDText()
        {
            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFileIDText.txt";

            if (File.Exists(FilePath) == true)
            {
                string IDText = System.IO.File.ReadAllText(FilePath);

                return IDText;
            }
            else
            {
                return null;
            }

        }

        //Get Encoding Argument from IDText
        public static Encoding GetEncodingIDText(string MyIDTextParent)
        {

            Encoding encoding = Encoding.Default;

            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDTextParent + ".txt";

            string[] lines = System.IO.File.ReadAllLines(FilePath);

            //Loop through the Lines
            for (int i = 0; i < lines.Length; i++)
            {
                //Get the Line
                string line = lines[i];

                //Split the Line
                string[] MyArray = Strings.Split(line, Utils.DefaultSeparator());

                //Fill in the Variables
                string MyArgument = MyArray[0];

                if (MyArgument == "Encoding")
                {

                    //Get Encoding
                    string strEncoding = MyArray[1];
                    encoding = Utils.ConvertStringToEncoding(strEncoding);
                    //Exit the Loop
                    break;
                }
            }

            return encoding;

        }

    }
}