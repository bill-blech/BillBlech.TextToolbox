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
    /// Interaction logic for ExtractTextAboveAnchorWordsDesigner.xaml
    /// </summary>
    public partial class ExtractTextAboveAnchorWordsDesigner
    {

        string MyIDText = null;
        string MyArgument = null;

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

        //Anchor Text After Update Event
        private void AnchorTextParamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Encoding encoding = Encoding.Default;

            //Return IDText Parent
            string MyIDTextParent = DesignUtils.ReturnCurrentFileIDText();

            //Get Encoding
            encoding = DesignUtils.GetEncodingIDText(MyIDTextParent);

            //Update IDText
            UpdateIDText();

            //Fill in Global Variable
            MyArgument = "Anchor Words Parameter";

            //Get ITem from the ComboBox
            string MyAnchorTextParamComboBox = this.AnchorTextParamComboBox.SelectedItem.ToString();

            //Log ComboBox
            DesignUtils.CallLogComboBox(MyIDText, MyArgument, MyAnchorTextParamComboBox, encoding);
            
        }
        #endregion


        public ExtractTextAboveAnchorWordsDesigner()
        {
            InitializeComponent();
        }

        #region Set IDText
        //Update IDText
        private void UpdateIDText()
        {
            //Get IDText, if there is
            MyIDText = ReturnIDText();

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
                UpdateControl("AnchorWords",ClipBoardText);

                //Hide Update Call Control
                this.AnchorWords.Visibility = Visibility.Visible;
                this.UpdateCall.Visibility = Visibility.Hidden;
            }
            else
            {

                //Fill in Global Variable
                MyArgument = "Anchor Words";

                //Setup Wizard Button
                CallButton_SetupWizard();

            }

        }

        //Setup Wizard Button
        private void CallButton_SetupWizard()
        {

            //Update IDText
            UpdateIDText();

            //Check if Current File is Updated
            string bUpdated = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFileUpdated.txt");

            if (bUpdated == "-1")
            {

                #region Build Context Menu
                //Start Context Menu
                ContextMenu cm = new ContextMenu();

                //Paste from the CLipboard
                System.Windows.Controls.MenuItem menuPaste = new System.Windows.Controls.MenuItem();

                menuPaste.Header = "Paste";
                menuPaste.Click += Button_PasteFromClipboard;
                menuPaste.ToolTip = "Paste from the Clipboard";
                //Add Icon to the uri_menuItem
                var uri_menuPaste = new System.Uri("https://img.icons8.com/cotton/20/000000/clipboard--v5.png");
                var bitmap_menuPaste = new BitmapImage(uri_menuPaste);
                var image_menuPaste = new Image();
                image_menuPaste.Source = bitmap_menuPaste;
                menuPaste.Icon = image_menuPaste;

                cm.Items.Add(menuPaste);

                //Wizard
                System.Windows.Controls.MenuItem menuWizard = new System.Windows.Controls.MenuItem();

                menuWizard.Header = "Wizard";
                menuWizard.Click += Button_OpenFormSelectData;
                menuWizard.ToolTip = "Select Words from Text File selected as Preview";
                //Add Icon to the uri_menuItem
                var uri_menuWizard = new System.Uri("https://img.icons8.com/officexs/20/000000/edit-file.png");
                var bitmap_menuWizard = new BitmapImage(uri_menuWizard);
                var image_menuWizard = new Image();
                image_menuWizard.Source = bitmap_menuWizard;
                menuWizard.Icon = image_menuWizard;

                cm.Items.Add(menuWizard);

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

            //Lines Above
            MyArgument = "Lines Above";
            string LinesAbove = ReturnLinesAbove();

            if (LinesAbove!= null)
            {
                //Update Text File Row Argument
                DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, LinesAbove, encoding);
            }
            else
            {
                //Delete Argument in case it is null
                DesignUtils.DeleteTextFileRowArgument(FilePath, MyArgument, encoding);
            }


            //Number of Lines
            MyArgument = "Number of Lines";
            string NumberofLines = ReturnNumberofLines();

            if (NumberofLines != null)
            {
                //Update Text File Row Argument
                DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, NumberofLines, encoding);

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
            string[] searchWords = { "Anchor Words" + Utils.DefaultSeparator(), "Lines Above" + Utils.DefaultSeparator(), "Number of Lines" + Utils.DefaultSeparator(), "Anchor Words Parameter" + Utils.DefaultSeparator() };
            double PercResults = Utils.FindWordsInString(Source, searchWords, false);

            //Case all Parameters are found
            if (PercResults == 1)
            {
                //Open Form Preview Extraction
                DesignUtils.CallformPreviewExtraction(MyIDText, "Extract Text Above Anchor Words", MyIDTextParent, encoding);
            }
            else
            {
                //Error Message
                MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            #endregion

        }

        //Return Lines Above
        private string ReturnLinesAbove()
        {
            try
            {
                //Get the FilePath
                return this.LinesNumber.Expression.ToString();
            }
            catch (Exception ex)
            {

                return null;

            }
        }

        //Return Number of Lines
        private string ReturnNumberofLines()
        {
            try
            {
                //Get the FilePath
                return this.NumberLines.Expression.ToString();
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

                string MyOutput = "New Collection(Of String) From " + ClipBoardText;
                VisualBasicValue<Collection<string>> MyArgList = new VisualBasicValue<Collection<string>>(MyOutput);
                p2.SetValue(new InArgument<Collection<string>>(MyArgList));
            }

        }

    }

}