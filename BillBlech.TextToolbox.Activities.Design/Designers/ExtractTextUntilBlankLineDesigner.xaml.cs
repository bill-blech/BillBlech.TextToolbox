using BillBlech.TextToolbox.Activities.Activities;
using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for ExtractTextUntilBlankLineDesigner.xaml
    /// </summary>
    public partial class ExtractTextUntilBlankLineDesigner
    {

        string MyIDText = null;
        string MyArgument = null;

        public ExtractTextUntilBlankLineDesigner()
        {
            InitializeComponent();
        }


        #region ComboBox
        public List<string> LstAnchorTypeParam
        {
            get
            {
                return new List<string>
                {
                    "Any", "All"
                };
            }
            set { }
        }

        public List<string> LstDirection
        {
            get
            {
                return new List<string>
                {
                  "Above", "Below"
                };
            }
            set { }
        }



        //Anchor Text After Update Event
        private void AnchorTextParamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Update IDText
            UpdateIDText();

            Encoding encoding = Encoding.Default;

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Fill in Global Variable
            MyArgument = "Anchor Words Parameter";

            //Get ITem from the ComboBox
            string MyAnchorTextParamComboBox = this.AnchorTextParamComboBox.SelectedItem.ToString();

            //Log ComboBox
            DesignUtils.CallLogComboBox(MyIDText, MyArgument, MyAnchorTextParamComboBox, encoding);
            
        }

        //Direction After Update Event
        private void DirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Update IDText
            UpdateIDText();

            Encoding encoding = Encoding.Default;

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Fill in Global Variable
            MyArgument = "Direction";

            //Get ITem from the ComboBox
            string MyDirectionComboBox = this.DirectionComboBox.SelectedItem.ToString();

            //Log ComboBox
            DesignUtils.CallLogComboBox(MyIDText, MyArgument, MyDirectionComboBox, encoding);
           
        }

        #endregion

        #region Set IDText
        //Update IDText
        private void UpdateIDText()
        {
            //Get IDText, if there is
            MyIDText = ReturnIDText();

            if (MyIDText != null)
            {
                string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

                //Case there is no file, create it!
                if (File.Exists(FilePath) == false)
                {
                    //Create Blank Text File
                    System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt", "");
                }
            }


            if (MyIDText == null)
            {

                //Generate IDText
                MyIDText = DesignUtils.GenerateIDText();

                //Create Blank Text File
                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt", "");

                //Write data to the Form
                ModelProperty property = this.ModelItem.Properties["IDText"];
                property.SetValue(new InArgument<string>(MyIDText));
            }

        }

        //Get ID Text
        private string ReturnIDText()
        {
            try
            {
                //Get the FilePath
                return this.IDText.Expression.ToString();
            }
            catch (Exception ex)
            {

                return null;

            }

        }
        #endregion

        //Setup Wizard Button
        private void CallCallButton_SetupWizard(object sender, System.Windows.RoutedEventArgs e)
        {

            string ClipBoardText = Clipboard.GetText();

            if (this.UpdateCall.Visibility == Visibility.Visible)
            {

                //Update Search Words
                UpdateControl("AnchorWords", ClipBoardText);

                //Hide Update Call Control
                this.AnchorWords.Visibility = Visibility.Visible;
                this.UpdateCall.Visibility = Visibility.Hidden;
            }
            else
            {

                //Update IDText
                UpdateIDText();

                //Fill in Global Variable
                MyArgument = "Anchor Words";

                //Setup Wizard Button
                CallButton_SetupWizard();

            }
        }

        //Setup Wizard Button
        private void CallButton_SetupWizard()
        {
            //Check if Current File is Updated
            string bUpdated = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFileUpdated.txt");

            if (bUpdated == "-1")
            {

                #region Build Context Menu
                //Start Context Menu
                ContextMenu cm = new ContextMenu();

                #region Create New IDText
                //Create New IDText
                System.Windows.Controls.MenuItem menuCreateNewIDText = new System.Windows.Controls.MenuItem();

                menuCreateNewIDText.Header = "Create New ID";
                menuCreateNewIDText.Click += CreateNewIDText;
                menuCreateNewIDText.ToolTip = "Create New IDText";
                //Add Icon to the uri_menuItem
                var uri_CreateNewIDText = new System.Uri("https://img.icons8.com/officexs/20/000000/add-file.png");
                var bitmap_CreateNewIDText = new BitmapImage(uri_CreateNewIDText);
                var image_CreateNewIDText = new Image();
                image_CreateNewIDText.Source = bitmap_CreateNewIDText;
                menuCreateNewIDText.Icon = image_CreateNewIDText;

                cm.Items.Add(menuCreateNewIDText);
                #endregion

                //Add Separator
                cm.Items.Add(new Separator());

                #region Paste from the Clipboard
                //Paste from the Clipboard

                //Get Text from the Clipboard
                string ClipboardText = Clipboard.GetText();

                if (ClipboardText != null)
                {
                    System.Windows.Controls.MenuItem menuPaste = new System.Windows.Controls.MenuItem();

                    menuPaste.Header = "Paste";
                    menuPaste.Click += Button_PasteFromClipboard;
                    menuPaste.ToolTip = $"Paste '{ClipboardText}' from the Clipboard to the Control";
                    //Add Icon to the uri_menuItem
                    var uri_menuPaste = new System.Uri("https://img.icons8.com/cotton/20/000000/clipboard--v5.png");
                    var bitmap_menuPaste = new BitmapImage(uri_menuPaste);
                    var image_menuPaste = new Image();
                    image_menuPaste.Source = bitmap_menuPaste;
                    menuPaste.Icon = image_menuPaste;

                    cm.Items.Add(menuPaste);
                }

                #endregion

                #region Wizard
                //Wizard
                System.Windows.Controls.MenuItem menuWizard = new System.Windows.Controls.MenuItem();

                //CurrentFile
                string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt";
                string FilePeview = System.IO.File.ReadAllText(FilePath);

                menuWizard.Header = "Wizard";
                menuWizard.Click += Button_OpenFormSelectData;
                menuWizard.ToolTip = "Select Words from Preview Text File '" + FilePeview + "'";
                //Add Icon to the uri_menuItem
                var uri_menuWizard = new System.Uri("https://img.icons8.com/officexs/20/000000/edit-file.png");
                var bitmap_menuWizard = new BitmapImage(uri_menuWizard);
                var image_menuWizard = new Image();
                image_menuWizard.Source = bitmap_menuWizard;
                menuWizard.Icon = image_menuWizard;

                cm.Items.Add(menuWizard);
                #endregion

                #region Preview
                //Preview
                System.Windows.Controls.MenuItem menuPreview = new System.Windows.Controls.MenuItem();

                menuPreview.Header = "Preview";
                menuPreview.Click += Button_OpenPreview;
                menuPreview.ToolTip = "Preview Data Extraction With Current Activity Arguments";
                //Add Icon to the uri_menuItem
                var uri_menuPreview = new System.Uri("https://img.icons8.com/officexs/20/000000/new-file.png");
                var bitmap_menuPreview = new BitmapImage(uri_menuPreview);
                var image_menuPreview = new Image();
                image_menuPreview.Source = bitmap_menuPreview;
                menuPreview.Icon = image_menuPreview;

                cm.Items.Add(menuPreview);
                #endregion

                //Open the Menu
                cm.IsOpen = true;

                #endregion

            }
            else
            {
                //Wizard Button: Warning Message: Wizard & Preview
                DesignUtils.Wizard_WarningMessage_Wizard_Preview();
            }

        }

        //Button Open Wizard
        private void Button_OpenFormSelectData(object sender, RoutedEventArgs e)
        {

            //Show Update Call Control
            this.AnchorWords.Visibility = Visibility.Hidden;
            this.UpdateCall.Visibility = Visibility.Visible;
            this.UpdateCall.Content = Utils.DefaultUpdateControl();

            //Get File Path
            string FilePath = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt");

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            Encoding encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Open Form Select Data
            DesignUtils.CallformSelectDataOpen(MyArgument, MyIDText, FilePath, MyIDTextParent, encoding);

        }

        //Button Open Preview
        private void Button_OpenPreview(object sender, RoutedEventArgs e)
        {
            Encoding encoding = Encoding.Default;

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Include Anchor Words Parameter
            MyArgument = "Include Anchor Words Parameter";
            string bIncludeAnchorWordsRow = ReturnIncludeAnchorWordsRow();

            if (bIncludeAnchorWordsRow != null)
            {
                //Update Text File Row Argument
                DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, bIncludeAnchorWordsRow, encoding);
            }
            else
            {
                //Delete Argument in case it is null
                DesignUtils.DeleteTextFileRowArgument(FilePath, MyArgument, encoding);
            }

            #region Open Preview Extraction

            //Read Text File
            string Source = System.IO.File.ReadAllText(FilePath, encoding);

            //Check if all Parameters are in the File
            string[] searchWords = { "Anchor Words" + Utils.DefaultSeparator(), "Anchor Words Parameter" + Utils.DefaultSeparator(), "Direction" + Utils.DefaultSeparator(), "Include Anchor Words Parameter" + Utils.DefaultSeparator()};
            double PercResults = Utils.FindWordsInString(Source, searchWords, false);

            //Case all Parameters are found
            if (PercResults == 1)
            {

                //Open Form Preview Extraction
                DesignUtils.CallformPreviewExtraction(MyIDText, "Extract Text until Blank Line", MyIDTextParent, encoding);
            }
            else
            {
                //Error Message
                MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            #endregion
        }

        //Create New TextID
        private void CreateNewIDText(object sender, RoutedEventArgs e)
        {

            string FilePath = null;

            //Get Encoding Parent

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            Encoding encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Get Data from Current Text File

            MyIDText = ReturnIDText();

            //Get the File Path
            FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Check if file exists
            if (File.Exists(FilePath) == true)
            {
                //Get Data from Text File
                string Source = System.IO.File.ReadAllText(FilePath, encoding);

                //New IDText

                //Clear the Current IDText
                ModelProperty property = this.ModelItem.Properties["IDText"];
                property.SetValue(null);

                //Update IDText
                UpdateIDText();

                //Set the New File Path
                FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

                //Write New Text File
                System.IO.File.WriteAllText(FilePath, Source);
            }

        }

        //Return IncludeAnchorWordsRow
        private string ReturnIncludeAnchorWordsRow()
        {
            try
            {
                //Get the FilePath
                return this.IncludeAnchorWordsRow.Expression.ToString();
            }
            catch (Exception ex)
            {

                return null;

            }
        }

        //Paste to from the Clipboard
        private void Button_PasteFromClipboard(object sender, RoutedEventArgs e)
        {

            Encoding encoding = Encoding.Default;

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Paste Argument from the Clipboard
            string OutputText = DesignUtils.PasteArgumentFromClipboard();

            //Update Control
            UpdateControl(MyArgument.Replace(" ", ""), OutputText);

            //Update Text File Row Argument
            DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, OutputText, encoding);

        }

        //Update Control
        public void UpdateControl(string ControlName, string ClipBoardText)
        {
            //Case it is not a Close Click
            if (ClipBoardText != Utils.DefaultSeparator())
            {
                //Reference the Control
                ModelProperty p2 = this.ModelItem.Properties[ControlName];

                //Case it is not null
                if (ClipBoardText.Length > 0)
                {
                    string MyOutput = "New Collection(Of String) From " + ClipBoardText;
                    VisualBasicValue<Collection<string>> MyArgList = new VisualBasicValue<Collection<string>>(MyOutput);
                    p2.SetValue(new InArgument<Collection<string>>(MyArgList));
                }
                else
                {
                    //Case it is null
                    p2.SetValue(null);
                }

            }

        }



    }
}