using BillBlech.TextToolbox.Activities.Activities;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for CountWordsInTextDesigner.xaml
    /// </summary>
    public partial class CountWordsInTextDesigner
    {
        string MyIDText = null;
        string MyArgument = null;

        public CountWordsInTextDesigner()
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

            //Fill in Global Variable
            MyArgument = "Search Words";

            //Setup Wizard Button
            CallButton_SetupWizard();
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
                //Warning Message
                MessageBox.Show("Please click the 'Warning Button' 'Wizard' and 'Preview'", "Enable Functionalities", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        //Button Open Wizard
        private void Button_OpenFormSelectData(object sender, RoutedEventArgs e)
        {

            //Open Form Select Data
            DesignUtils.CallformSelectDataOpen(MyArgument, MyIDText);

        }

        //Button Open Preview
        private void Button_OpenPreview(object sender, RoutedEventArgs e)
        {

            string FilePath = null;
            string Source = null;

            //Read Data from Current File
            FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/CurrentFile.txt";
            Source = System.IO.File.ReadAllText(FilePath);
            string inputText = System.IO.File.ReadAllText(Source);

            //Get File Path
            FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";
            Source = System.IO.File.ReadAllText(FilePath);

            //Split the Line
            string[] MyArray = Source.Split('@');

            //Fill in the Variables
            string MyArgument = MyArray[0];
            string MyValue = MyArray[1];
            
            //'Convert' Array to String
            string[] SearchWords = DesignUtils.ConvertStringToArray(MyValue);

            //Find Words in String
            double PercResults = Utils.FindWordsInString(inputText, SearchWords, false);

            //Display Result
            MessageBox.Show($"Percentage: {PercResults.ToString("P", CultureInfo.InvariantCulture)}", "Count Words in String");

        }

       
    }
}