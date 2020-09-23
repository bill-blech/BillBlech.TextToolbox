﻿using BillBlech.TextToolbox.Activities.Activities.Encryption;
using ExcelTut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


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

        // List the words in the file.
        public static string[] GetUniqueWordsInFile(string filePath)
        {
            //Open Text File
            String txt = System.IO.File.ReadAllText(filePath);

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
        public static void WriteTextFileSingleFile(string FileFullPath, string MyIDText)
        {

            //Get the File Name
            string fileName = Path.GetFileNameWithoutExtension(FileFullPath);

            //File Path
            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames/" + fileName + ".txt", FileFullPath);

            //File Path for Preview
            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FilePathPreview/" + MyIDText + ".txt", FileFullPath);

        }

        //Remove Special Characters
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
        public static void DeleteTextFileRowArgument(string filePath, string MyArgument)
        {

            //Convert the Text File into a list of strings
            List<string> linesList = File.ReadAllLines(filePath).ToList();

            //Loop through the Lines
            for (int i = 0; i < linesList.Count; i++)
            {

                //Get Line
                string Line = linesList[i];

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

            //Reset the Text File
            System.IO.File.WriteAllLines(filePath, linesList.ToArray());

        }

        //Delete File
        public static bool DeleteStorageTextFileRun(string fileName)
        {
            //File Name
            File.Delete(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames/" + fileName + ".txt");

            return true;
        }

        //Open Form Select Data Open
        public static void CallformSelectDataOpen(string Label, string MyIDText)
        {

            //Get File Path
            string FilePath = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt");

            //Get Available Items
            string[] ArrayAvailableItems = GetUniqueWordsInFile(FilePath);

            //Open Form
            FormSelectData formSelectData;
            formSelectData = new FormSelectData(Label, ArrayAvailableItems, MyIDText);
            formSelectData.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            formSelectData.ControlBox = false;
            formSelectData.Show();

        }

        //Open Form Preview Extraction
        public static void CallformPreviewExtraction(string MyIDText, string Label)
        {
            //Open Form
            FormDisplayPreview formDisplayPreview;
            formDisplayPreview = new FormDisplayPreview(MyIDText, Label);

        }

        //Log ComboBox
        public static void CallLogComboBox(string MyIDText, string MyArgument, string MyValue)
        {

            //Start the Variable
            string OutputText = null;

            //Set Excel File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Update Text File Row Argument
            CallUpdateTextFileRowArgument(FilePath, MyArgument, MyValue);

        }

        //Update Text File Row Argument
        public static void CallUpdateTextFileRowArgument(string FilePath, string MyArgument, string MyValue)
        {

            string OutputText = null;

            //Delete Previous Argument, in case found
            DesignUtils.DeleteTextFileRowArgument(FilePath, MyArgument);

            //Read All Lines
            string[] Lines = System.IO.File.ReadAllLines(FilePath);

            //Case there are lines there
            if (Lines.Length > 1)
            {
                OutputText += Environment.NewLine;

            }

            //Add Argument Name & Value
            OutputText += MyArgument + "@" + MyValue + "@" + DateTime.Now.ToString();

            //Store Info to TextFile
            System.IO.File.AppendAllText(FilePath, OutputText);
        }

        //'Convert' String to Array
        public static string[] ConvertStringToArray(string ArrayText)
        {
            //Remove Braces {} and "
            ArrayText = ArrayText.Replace("{", "");
            ArrayText = ArrayText.Replace("}", "");
            ArrayText = ArrayText.Replace("\"", "");

            //Split the Items
            string[] ArrayOutput = ArrayText.Split(',');

            return ArrayOutput;

        }

    }
}