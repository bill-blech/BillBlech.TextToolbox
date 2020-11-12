using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Design;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace ExcelTut
{
    public partial class FormDisplayPreview : Form
    {
        Dictionary<string, string> DicArguments = new Dictionary<string, string>();
        Dictionary<string, string> DicArgumentsParent = new Dictionary<string, string>();
        Dictionary<int, string> DicResults = new Dictionary<int, string>();
        Encoding encoding = Encoding.Default;

        public FormDisplayPreview(string MyIDText, string Label, string MyIDTextParent, Encoding encoding)
        {
            InitializeComponent();

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ControlBox = false;
            this.Text = Label;
            this.encoding = encoding;

            this.Show();

            //Parent Arguments
            DicArgumentsParent = LoadFormArguments(MyIDTextParent, false, encoding);

            if (MyIDText!= null)
            {

                //Load Form Arguments
                DicArguments = LoadFormArguments(MyIDText, true, encoding);
            
            }

            //Run Extraction
            RunExtraction(Label);

        }

        //Load Form Arguments
        //private void LoadFormArguments(string MyIDText)
        private Dictionary<string, string> LoadFormArguments(string MyIDText, bool bUpdateFormControls, Encoding encoding)
        {

            Dictionary<string, string> MyDic = new Dictionary<string, string>();

            //Get File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Remove Empty Rows from Text File
            Utils.TextFileRemoveEmptyRows(FilePath, encoding);

            //Read Text File and Split by new line
            string[] lines = System.IO.File.ReadAllLines(FilePath, encoding);

            //Loop through the Lines
            for (int i = 0; i < lines.Length; i++)
            {

                //Get the Line
                string line = lines[i];

                    //Split the Line
                    string[] MyArray = Strings.Split(line, Utils.DefaultSeparator());

                    //Fill in the Variables
                    string MyArgument = MyArray[0];
                    string MyValue = MyArray[1];

                    if (bUpdateFormControls == true)
                    {
                        //Set the Form Controls
                        Control ctnArgument = this.Controls["Argument" + (i + 1) + "x_Label"];
                        Control ctnValue = this.Controls["Argument" + (i + 1) + "x_Text"];

                        //Change Visibility to True
                        ctnArgument.Visible = true;
                        ctnValue.Visible = true;

                        //Fill in the Controls
                        ctnArgument.Text = MyArgument;
                        ctnValue.Text = MyValue;
                    }

                    //Add to the Dictionary
                    MyDic.Add(MyArgument, MyValue);
                                   
            }

            //Return the Dictionary
            return MyDic;

        }

        //Run Extraction
        private void RunExtraction(string Label)
        {

            string Source = null;
            string inputText = null;
            
            //Read Data from IDTextFile

            //string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt";
            string FilePath = DicArgumentsParent["FileName"];

            if (File.Exists(FilePath)==true)
            {
                //Source = System.IO.File.ReadAllText(FilePath);
             
                //Get Data from the Text File
                inputText = System.IO.File.ReadAllText(FilePath, encoding);

                //Display Result
                this.DisplaySource.Text = inputText;

                //Add Dummy last line to InputText
                inputText += Environment.NewLine;

            }

            #region Variable Declarations

            string[] begWords = null;
            string[] endWords = null;
            string regexParameterText = null;
            string ArrayText = null;
            string[] anchorText = null;
            string[] anchorWords = null;
            string anchorTextParamText = null;
            string[] OutputResults = null;

            int LinesAbove = 0;
            int LinesBelow = 0;
            int NumLines = 0;

            string MyOccurenceParameter = null;
            int MyOccurencePosition = 0;

            string FilePathforPreview = null;
            string TextResult = null;

            string[] inputArray = null;

            #endregion

            #region Run Extraction
            switch (Label)
            {

                #region Extract Text Between Two Anchor Words
                case "Extract Text Between Two Anchor Words":

                    //Load Arguments from Dictionary
                    
                    //Beg Words
                    ArrayText = DicArguments["Beg Words"];
                    begWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    //End Words
                    if (DicArguments.ContainsKey("End Words") == true)
                    {
                        ArrayText = DicArguments["End Words"];

                        //DicArguments.TryGetValue("End Words", out ArrayText);
                        endWords = DesignUtils.ConvertStringToArray(ArrayText,false);
                    }
                    else
                    {

                        //Null Array
                        endWords = null;
                    }
                    
                    //Regex Parameter               
                    regexParameterText = DicArguments["Regex Parameter"];

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractTextBetweenTwoAnchorWords(inputText, begWords, endWords, regexParameterText, false, false);

                    break;

                #endregion

                #region Extract All Lines Below Anchor Words
                case "Extract All Lines Below Anchor Words":

                    //Load Arguments from Dictionary
                    anchorTextParamText = DicArguments["Anchor Words Parameter"];
                    ArrayText = DicArguments["Anchor Words"];

                    //Split the Items
                    anchorText = DesignUtils.ConvertStringToArray(ArrayText,false);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractAllLinesBelowAnchorText(inputText, anchorText, anchorTextParamText, false, false);

                    break;
                #endregion

                #region Extract Text Above Anchor Words
                case "Extract Text Above Anchor Words":

                    //Load Arguments from Dictionary
                    anchorTextParamText = DicArguments["Anchor Words Parameter"];
                    ArrayText = DicArguments["Anchor Words"];
                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    LinesAbove = Convert.ToInt32(DicArguments["Lines Above"]);
                    NumLines = Convert.ToInt32(DicArguments["Number of Lines"]);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractTextAboveAnchorWords(inputText, anchorWords, anchorTextParamText, LinesAbove, NumLines, false, false);

                    break;

                #endregion

                #region Extract Text Below Anchor Words
                case "Extract Text Below Anchor Words":

                    //Load Arguments from Dictionary
                    anchorTextParamText = DicArguments["Anchor Words Parameter"];

                    ArrayText = DicArguments["Anchor Words"];
                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    LinesBelow = Convert.ToInt32(DicArguments["Lines Below"]);
                    NumLines = Convert.ToInt32(DicArguments["Number of Lines"]);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractTextBelowAnchorWords(inputText, anchorWords, anchorTextParamText, LinesBelow, NumLines, true, true);


                    break;
                #endregion

                #region Extract Text Until White Space
                case "Extract Text Until White Space":

                    //Load Arguments from Dictionary
                    ArrayText = DicArguments["Anchor Words"];
                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractAllCharactersUntilWhiteSpace(inputText, anchorWords, false, false);

                    break;


                #endregion

                #region Extract Text Until Next Letter
                case "Extract Text Until Next Letter":

                    //Load Arguments from Dictionary
                    ArrayText = DicArguments["Anchor Words"];
                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractAllCharactersUntilLetterCharacter(inputText, anchorWords, false);

                    break;

                #endregion

                #region Remove Words
                case "Remove Words":

                    //Load Arguments from Dictionary
                    ArrayText = DicArguments["Words"];

                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    //Occurence Parameter
                    MyOccurenceParameter = DicArguments["Occurence Parameter"];

                    //Occurence Position
                    if (DicArguments.ContainsKey("Occurence Position") == true)
                    {
                        MyOccurencePosition = Convert.ToInt32(DicArguments["Occurence Position"]);
                    }

                    //Run Extraction
                    TextResult = Utils.RemoveWordsFromText(inputText, anchorWords,MyOccurenceParameter, MyOccurencePosition, false);

                    //Display Result
                    this.DisplayResult.Text = TextResult;

                    //Hide Controls
                    ResultsMatches_Label.Visible = false;
                    ResultsMatches.Visible = false;
                    SelectResult_Label.Visible = false;
                    SelectResult.Visible = false;

                    break;
                #endregion

                #region Replace Words
                case "Replace Words":

                    //Load Arguments from Dictionary
                    ArrayText = DicArguments["Search Words"];

                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    string ReplacedWords = DicArguments["Replaced Word"];

                    //Occurence Parameter
                    MyOccurenceParameter = DicArguments["Occurence Parameter"];

                    //Occurence Position
                    if (DicArguments.ContainsKey("Occurence Position") == true)
                    {
                        MyOccurencePosition = Convert.ToInt32(DicArguments["Occurence Position"]);
                    }

                    //Run Extraction
                    TextResult = Utils.ReplaceWordsFromText(inputText, anchorWords, ReplacedWords, MyOccurenceParameter, MyOccurencePosition, false);

                    //Display Result
                    this.DisplayResult.Text = TextResult;

                    //Hide Controls
                    ResultsMatches_Label.Visible = false;
                    ResultsMatches.Visible = false;
                    SelectResult_Label.Visible = false;
                    SelectResult.Visible = false;

                    break;

                #endregion

                #region Split Text Uneven Blank Spaces
                case "Split text Uneven Blank Spaces":

                    //Load Arguments from Dictionary
                    FilePathforPreview = DicArguments["FileName"];
                    inputText = System.IO.File.ReadAllText(FilePathforPreview, encoding);

                    //Null Limit
                    int nullLimit = Convert.ToInt32(DicArguments["Null Limit"]);

                    //Suppress Null Values
                    string SuppressNullValues = DicArguments["Suppress Null Values"];
                    bool bSuppressNullValues = false;
                    if (SuppressNullValues == "True")
                    {
                        bSuppressNullValues = true;
                    }
                    else if (SuppressNullValues == "False")
                    {
                        bSuppressNullValues = false;
                    }

                    //Run Extraction
                    OutputResults = Utils.SplitTextBigSpaces(inputText, nullLimit, bSuppressNullValues, false);

                    break;

                #endregion

                #region Remove Empty Rows
                case "Remove Empty Rows":

                    FilePathforPreview = DicArguments["FileName"];
                    inputText = System.IO.File.ReadAllText(FilePathforPreview, encoding);

                    //Run Extraction
                    TextResult = Utils.TextRemoveEmptyRows(inputText);

                    //Display Result
                    this.DisplayResult.Text = TextResult;

                    //Hide Controls
                    ResultsMatches_Label.Visible = false;
                    ResultsMatches.Visible = false;
                    SelectResult_Label.Visible = false;
                    SelectResult.Visible = false;

                    break;

                #endregion

                #region Read Text File Encoding
                case "Read Text File Encoding":
                case "Text Application Scope":

                    ////File Name
                    //FilePath = DicArguments["FileName"];

                    ////Encoding
                    //string strEncoding = DicArguments["Encoding"];

                    ////Run Extraction
                    //TextResult = Utils.ReadTextFileEncoding(FilePath, strEncoding,false);

                    ////Display Result
                    //this.DisplayResult.Text = TextResult;

                    //Remove TAG Source
                    this.tabText.TabPages.Remove(tabResults);                  

                    //Hide Controls
                    ResultsMatches_Label.Visible = false;
                    ResultsMatches.Visible = false;
                    SelectResult_Label.Visible = false;
                    SelectResult.Visible = false;

                    break;

                #endregion

                #region Extract Text until Blank Line
                case "Extract Text until Blank Line":

                    //Anchor Words
                    ArrayText = DicArguments["Anchor Words"];
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText, false);
                    
                    //Anchor Words Parameter
                    anchorTextParamText = DicArguments["Anchor Words Parameter"];

                    //Direction
                    string Direction = DicArguments["Direction"];

                    string IncludeAnchorWordsParameter = DicArguments["Include Anchor Words Parameter"];
                    bool bIncludeAnchorWordsParameter = false;

                    if (IncludeAnchorWordsParameter == "True")
                    {
                        bIncludeAnchorWordsParameter = true;
                    }
                    else if (IncludeAnchorWordsParameter == "False")
                    {
                        bIncludeAnchorWordsParameter = false;
                    }

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractTextUntilBlankLine(inputText, anchorWords, anchorTextParamText, Direction, bIncludeAnchorWordsParameter, false, false);

                    break;

                #endregion

                #region Split Text New Lines
                case "Split Text New Lines":

                    //Run Extraction
                    OutputResults = Utils.SplitTextNewLine(inputText);

                    break;

                #endregion

                #region Split Text By Blank Lines

                case "Split Text By Blank Lines":

                    //Run Extraction
                    OutputResults = Utils.SplitTextByBlankLines(inputText);


                    break;

                #endregion

                #region Find Array Items
                case "Find Array Items":

                    //File Path for Preview
                    FilePath = DicArguments["FileName"];
                    inputText = System.IO.File.ReadAllText(FilePath, encoding);

                    //Input Array
                    inputArray = Utils.SplitTextByBlankLines(inputText);

                    //Anchor Words
                    ArrayText = DicArguments["Filter Words"];
                    string [] filterWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    //Anchor Words Parameter
                    anchorTextParamText = DicArguments["Anchor Words Parameter"];

                    //Run Extraction
                    OutputResults = CallExtractions.CallFindArrayItems(inputArray, filterWords, anchorTextParamText, false);

                    break;
                #endregion

                #region Match Item in Array
                case "Match Item in Array":

                    //File Path for Preview
                    FilePath = DicArguments["FileName"];
                    inputText = System.IO.File.ReadAllText(FilePath, encoding);

                    //Input Array
                    inputArray = Utils.SplitTextNewLine(inputText);

                    ArrayText = DicArguments["SearchWord"];
                    
                    string[] searchWords = DesignUtils.ConvertStringToArray(ArrayText, false);

                    bool isFound = Utils.MatchItemInArrayOfStrings(inputArray, searchWords,false);

                    //Display Result
                    this.DisplayResult.Text = isFound.ToString();

                    //Hide Controls
                    ResultsMatches_Label.Visible = false;
                    ResultsMatches.Visible = false;
                    SelectResult_Label.Visible = false;
                    SelectResult.Visible = false;

                    break;

                #endregion


            }
            #endregion

            #region Display Results
            if (OutputResults!= null)
            {
                //Get Results Counter
                int ResultsCounter = OutputResults.Length;

                //Case Items are found
                if (ResultsCounter > 0)
                {
                    //Results Counter
                    this.ResultsMatches.Text = ResultsCounter.ToString();

                    //Load the ComboBox
                    for (int i = 0; i < ResultsCounter; i++)
                    {

                        //Add to the ComboBox
                        SelectResult.Items.Add(i + 1);

                        //Add to the Dic Results
                        DicResults.Add((i + 1), OutputResults[i]);

                    }

                    //Set Default value case item is found
                    this.SelectResult.SelectedItem = 1;

                }
                //Case items are not found
                else
                {

                    //Matches
                    this.ResultsMatches.Text = "0";

                    //Hide Select Result
                    SelectResult_Label.Visible = false;
                    SelectResult.Visible = false;

                }

            }

            #endregion

        }

        //Fill in Results
        private void SelectResult_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            //Get selected item from the ComboBox
            string selectedItem = this.SelectResult.SelectedItem.ToString();

            if (selectedItem != null)
            {
                //Get Selected Item
                int selected = Convert.ToInt32(selectedItem);

                //Get Item from the Dictionary
                this.DisplayResult.Text = DicResults[selected];
            }
        }

        //Close Form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Save Result Text in a Text File
        private void btnSaveText_Click(object sender, EventArgs e)
        {

            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt";
            string Source = System.IO.File.ReadAllText(FilePath);

            string FileDirectory = Path.GetDirectoryName(Source);
            string FileName = Path.GetFileNameWithoutExtension(Source) + "_";

            //Prompt Save
            string NewFileName = Interaction.InputBox("Set the file Name", "Save Text File", FileName);

            //Case there is a file
            if (FileName != null)
            {

                //Set File Path
                string filePath = FileDirectory + "/" + NewFileName + ".txt";

                //Write Text File
                System.IO.File.WriteAllText(filePath, DisplayResult.Text, encoding);

                //#region Folder Picker
                //string selectedFolder = null;

                ////Open File Picker
                //using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                //{
                //    dialog.Description = "Select Folder to Save Text File  (Current Template File Location is Default)";
                //    //dialog.SelectedPath = Directory.GetCurrentDirectory();
                //    dialog.SelectedPath = FileDirectory;

                //    System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                //    if (result == System.Windows.Forms.DialogResult.OK)
                //    {
                //        selectedFolder = dialog.SelectedPath;

                //        if (selectedFolder != null)
                //        {
                //            //Set File Path
                //            string filePath = selectedFolder + "/" + NewFileName + ".txt";

                //            //Write Text File
                //            System.IO.File.WriteAllText(filePath, DisplayResult.Text, encoding);
                //        }

                //    }

                //}

                //#endregion
            }




        }
    }
}
