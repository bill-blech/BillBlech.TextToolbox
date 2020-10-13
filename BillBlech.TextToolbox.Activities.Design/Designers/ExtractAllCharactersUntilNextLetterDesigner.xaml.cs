using BillBlech.TextToolbox.Activities.Activities;
using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for ExtractAllCharactersUntilNextLetterDesigner.xaml
    /// </summary>
    public partial class ExtractAllCharactersUntilNextLetterDesigner
    {
        string MyIDText = null;
        string MyArgument = null;

        public ExtractAllCharactersUntilNextLetterDesigner()
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
                UpdateControl("AnchorWords", ClipBoardText);

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

            //Open Form Select Data
            DesignUtils.CallformSelectDataOpen(MyArgument, MyIDText, FilePath);

        }

        //Button Open Preview
        private void Button_OpenPreview(object sender, RoutedEventArgs e)
        {

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            #region Open Preview Extraction

            //Read Text File
            string Source = System.IO.File.ReadAllText(FilePath);

            //Check if all Parameters are in the File
            string[] searchWords = { "Anchor Words" + Utils.DefaultSeparator() };
            double PercResults = Utils.FindWordsInString(Source, searchWords, false);

            //Case all Parameters are found
            if (PercResults == 1)
            {
                //Open Form Preview Extraction
                DesignUtils.CallformPreviewExtraction(MyIDText, "Extract Text Until Next Letter");
            }
            else
            {
                //Error Message
                MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            #endregion

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