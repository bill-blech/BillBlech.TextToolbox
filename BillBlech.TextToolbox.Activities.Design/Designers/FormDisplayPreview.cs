using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Design;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ExcelTut
{
    public partial class FormDisplayPreview : Form
    {
        Dictionary<string, string> DicArguments = new Dictionary<string, string>();
        Dictionary<int, string> DicResults = new Dictionary<int, string>();

        public FormDisplayPreview(string MyIDText, string Label)
        {
            InitializeComponent();

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ControlBox = false;
            this.Text = Label;

            this.Show();

            //Load Form Arguments
            LoadFormArguments(MyIDText);

            //Run Extraction
            RunExtraction(Label);

        }

        //Load Form Arguments
        private void LoadFormArguments(string MyIDText)
        {
            //Get File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Remove Empty Rows from Text File
            Utils.TextFileRemoveEmptyRows(FilePath);

            //Read Text File and Split by new line
            string[] lines = System.IO.File.ReadAllLines(FilePath);

            //Loop through the Lines
            for (int i = 0; i < lines.Length; i++)
            {

                //Get the Line
                string line = lines[i];

                    //Split the Line
                    string[] MyArray = line.Split('@');

                    //Fill in the Variables
                    string MyArgument = MyArray[0];
                    string MyValue = MyArray[1];

                    //Set the Form Controls
                    Control ctnArgument = this.Controls["Argument" + (i + 1) + "_Label"];
                    Control ctnValue = this.Controls["Argument" + (i + 1) + "_Text"];

                    //Change Visibility to True
                    ctnArgument.Visible = true;
                    ctnValue.Visible = true;

                    //Add to the Dictionary
                    DicArguments.Add(MyArgument, MyValue);

                    //Fill in the Controls
                    ctnArgument.Text = MyArgument;
                    ctnValue.Text = MyValue;
                
            }

        }

        //Run Extraction
        private void RunExtraction(string Label)
        {

            //Read Data from Current File
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt";
            string Source = System.IO.File.ReadAllText(FilePath);
            string inputText = System.IO.File.ReadAllText(Source);

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

            #endregion

            #region Run Extraction
            switch (Label)
            {

                #region Extract Text Between Two Anchor Words
                case "Extract Text Between Two Anchor Words":

                    //Load Arguments from Dictionary
                    
                    //Beg Words
                    ArrayText = DicArguments["Beg Words"];
                    begWords = DesignUtils.ConvertStringToArray(ArrayText);

                    //End Words
                    if (DicArguments.ContainsKey("End Words") == true)
                    {
                        ArrayText = DicArguments["End Words"];
                        //DicArguments.TryGetValue("End Words", out ArrayText);
                        endWords = DesignUtils.ConvertStringToArray(ArrayText);
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
                    anchorText = DesignUtils.ConvertStringToArray(ArrayText);

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
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText);

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
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText);

                    LinesBelow = Convert.ToInt32(DicArguments["Lines Below"]);
                    NumLines = Convert.ToInt32(DicArguments["Number of Lines"]);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractTextBelowAnchorWords(inputText, anchorWords, anchorTextParamText, LinesBelow, NumLines, true, true);


                    break;
                #endregion

                #region Extract All Characters Until White Space Designer
                case "Extract All Characters Until White Space Designer":

                    //Load Arguments from Dictionary
                    ArrayText = DicArguments["Anchor Words"];
                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText);

                    //Run Extraction
                    OutputResults = CallExtractions.CallExtractAllCharactersUntilWhiteSpace(inputText, anchorWords, false, false);

                    break;


                #endregion

                #region Remove Words
                case "Remove Words":

                    //Load Arguments from Dictionary
                    ArrayText = DicArguments["Words"];

                    //Split the Items
                    anchorWords = DesignUtils.ConvertStringToArray(ArrayText);

                    //Occurence Parameter
                    MyOccurenceParameter = DicArguments["Occurence Parameter"];

                    //Occurence Position
                    if (DicArguments.ContainsKey("Occurence Position") == true)
                    {
                        MyOccurencePosition = Convert.ToInt32(DicArguments["Occurence Position"]);
                    }

                    //Run Extraction
                    string TextResult = Utils.RemoveWordsFromText(inputText, anchorWords,MyOccurenceParameter, MyOccurencePosition, false);

                    //Display Result
                    this.DisplayResult.Text = TextResult;

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
                int ResultsMatches = OutputResults.Length;

                //Case Items are found
                if (ResultsMatches > 0)
                {
                    //Results Counter
                    this.ResultsMatches.Text = ResultsMatches.ToString();

                    //Load the ComboBox
                    for (int i = 0; i < ResultsMatches; i++)
                    {

                        //Add to the ComboBox
                        SelectResult.Items.Add(i + 1);

                        //Add to the Dic Results
                        DicResults.Add((i + 1), OutputResults[i]);

                    }

                    //Set Default value case item is found
                    this.SelectResult.SelectedItem = 1;

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

            if (FileName != null)
            {
                string selectedFolder = null;

                //Open File Picker
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.Description = "Select Folder to Save Text File  (Current Template File Location is Default)";
                    //dialog.SelectedPath = Directory.GetCurrentDirectory();
                    dialog.SelectedPath = FileDirectory;

                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        selectedFolder = dialog.SelectedPath;

                        if (selectedFolder != null)
                        {
                            //Set File Path
                            string filePath = selectedFolder + "/" + NewFileName + ".txt";

                            //Write Text File
                            System.IO.File.WriteAllText(filePath, DisplayResult.Text);
                        }

                    }

                }
            }

          

            
        }
    }
}
