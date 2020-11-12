using BillBlech.TextToolbox.Activities.Activities;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Activities;
using Microsoft.Win32;
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
    /// Interaction logic for MatchItemInArrayDesigner.xaml
    /// </summary>
    public partial class MatchItemInArrayDesigner
    {
        string MyIDText = null;
        string MyArgument = null;

        #region ComboBox
        public List<string> LstEncoding
        {
            get
            {
                return new List<string>
                {
                    "Default", "UTF8", "ASCII", "iso-8859-1"
                };
            }
            set { }
        }

        //Anchor Words Parameter Update Event
        private void EncodingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Update IDText
            UpdateIDText();

            //Get IDText, if there is
            MyIDText = ReturnIDText();

            //Case there is an IDText
            if (MyIDText != null)
            {
                //Auto Fill Controls
                AutoFillControls();
            }

        }

        #endregion

        public MatchItemInArrayDesigner()
        {
            InitializeComponent();

            //Create Default Storage Folders (if needed)
            DesignUtils.CreateStorageTextToolboxFolders();

        }

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
                UpdateControl("SearchWord", ClipBoardText);

                //Hide Update Call Control
                this.SearchWord.Visibility = Visibility.Visible;
                this.UpdateCall.Visibility = Visibility.Hidden;
            }
            else
            {

                //Update IDText
                UpdateIDText();

                //Fill in Global Variable
                MyArgument = "SearchWord";

                //Setup Wizard Button
                CallButton_SetupWizard();

            }
        }

        //Setup Wizard Button
        private void CallButton_SetupWizard()
        {
            //Get Data from Control
            string CurrentTextFilePath = ReturnCurrentFile();

            #region Build Context Menu
            //Start Context Menu
            ContextMenu cm = new ContextMenu();

            #region Create New ID

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

            //Get all StorageFiles
            string[] StorageFilesList = Directory.GetFiles(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames");

            //Check if menu is valid
            bool bValid = false;

            //Set boolean variable
            if (StorageFilesList.Length > 0)
            {
                bValid = true;
            }

            //Run Menu in case items are found
            if (bValid == true)
            {

                #region Load Recent Files

                //Storage Files: Recent
                //Create Menu
                System.Windows.Controls.MenuItem SubmenuItem = new System.Windows.Controls.MenuItem();
                SubmenuItem.Header = "Recent Files";
                //Add Menu Icon
                var uri_SubmenuItem = new System.Uri("https://img.icons8.com/officexs/20/000000/versions.png");
                var bitmap_SubmenuItem = new BitmapImage(uri_SubmenuItem);
                var image_SubmenuItem = new Image();
                image_SubmenuItem.Source = bitmap_SubmenuItem;
                SubmenuItem.Icon = image_SubmenuItem;

                //Loop through the Text Files
                foreach (string StorageFile in StorageFilesList)
                {

                    //Get the full file path
                    string FilePath = System.IO.File.ReadAllText(StorageFile);

                    //Get file Name
                    string fileName = Path.GetFileName(FilePath);

                    //Create Menu
                    System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
                    menuItem.Header = fileName;
                    menuItem.Click += Button_MenuItem_fileName_Click;
                    menuItem.Tag = StorageFile;
                    //Add Menu Icon
                    var uri_menuItem = new System.Uri("https://img.icons8.com/officexs/20/000000/file.png");
                    var bitmap_menuItem = new BitmapImage(uri_menuItem);
                    var image_menuItem = new Image();
                    image_menuItem.Source = bitmap_menuItem;
                    menuItem.Icon = image_menuItem;

                    //Add Sub Menu Item to Menu Item
                    SubmenuItem.Items.Add(menuItem);


                }

                //Add SubMenu
                cm.Items.Add(SubmenuItem);

                //Add Separator
                cm.Items.Add(new Separator());

                #endregion

                #region Open File
                //Get Current Excel File Name


                //Case it is a Variable
                if (CurrentTextFilePath == "1.5: VisualBasicValue<String>")
                {

                    //Go
                    goto selectfile;
                }

                //Get the File Name
                string SelectedfileName = Path.GetFileNameWithoutExtension(CurrentTextFilePath);


                //Case there is file
                if (SelectedfileName != null)
                {

                    //Open File
                    System.Windows.Controls.MenuItem SubmenuItemOpen = new System.Windows.Controls.MenuItem();
                    SubmenuItemOpen.Header = "Open File";
                    SubmenuItemOpen.Tag = SelectedfileName;
                    SubmenuItemOpen.Click += Button_MenuItem_OpenFile;
                    //Add Menu Icon
                    var uri_SubmenuItemOpen = new System.Uri("https://img.icons8.com/officexs/20/000000/check-file.png");
                    var bitmap_SubmenuItemOpen = new BitmapImage(uri_SubmenuItemOpen);
                    var image_SubmenuItemOpen = new Image();
                    image_SubmenuItemOpen.Source = bitmap_SubmenuItemOpen;
                    SubmenuItemOpen.Icon = image_SubmenuItemOpen;

                    cm.Items.Add(SubmenuItemOpen);

                }


                #endregion

            }

        #region Select File
        selectfile:
            //Select File
            System.Windows.Controls.MenuItem MenuItemSelectFile = new System.Windows.Controls.MenuItem();

            //Check if there are already recent items
            if (bValid == true)
            {
                //there is already files in recent

                //Header
                MenuItemSelectFile.Header = "Select New File";

                //Toll Tip Text
                MenuItemSelectFile.ToolTip = "Select a file not on the list";

            }
            else
            {

                //no files in recent
                MenuItemSelectFile.Header = "Select File";

                //Toll Tip Text
                MenuItemSelectFile.ToolTip = "Select a file";
            }

            MenuItemSelectFile.Click += Button_LoadDocument;
            //Add Icon to the Button
            var uri_MenuItemSelectFile = new System.Uri("https://img.icons8.com/officexs/20/000000/view-file.png");
            var bitmap_MenuItemSelectFile = new BitmapImage(uri_MenuItemSelectFile);
            var image_MenuItemSelectFile = new Image();
            image_MenuItemSelectFile.Source = bitmap_MenuItemSelectFile;
            MenuItemSelectFile.Icon = image_MenuItemSelectFile;

            //Add to Context Menu
            cm.Items.Add(MenuItemSelectFile);

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

            //Wizard & Preview
            if (CurrentTextFilePath != null)
            {

                if (File.Exists(CurrentTextFilePath) == true)
                {

                    //Add Separator
                    cm.Items.Add(new Separator());

                    #region Wizard
                    //Wizard
                    System.Windows.Controls.MenuItem menuWizard = new System.Windows.Controls.MenuItem();

                    //CurrentFile
                    string FilePeview = ReturnCurrentFile();

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

                }
            }

            //Open the Menu
            cm.IsOpen = true;

            #endregion

        }

        //Return Current File
        private string ReturnCurrentFile()
        {
            try
            {
                //Get the FilePath
                return this.FilePathPreview.Expression.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }



        }

        //Return Encoding
        private string ReturnEncoding()
        {
            try
            {
                //Get Encoding
                return this.EncodingComboBox.SelectedItem.ToString();

            }
            catch (Exception ex)
            {

                return null;
            }

        }

        //Select File Name
        private void Button_MenuItem_fileName_Click(object sender, RoutedEventArgs e)
        {
            //Get the Click Item
            var item = e.OriginalSource as System.Windows.Controls.MenuItem;

            //Get Storage File
            string StorageFile = item.Tag.ToString();

            //Read Sheets Names from the File
            string FilePath = System.IO.File.ReadAllText(StorageFile);

            //Check if file exists
            bool bExists = File.Exists(FilePath);

            //Print file in case it is found
            if (bExists == true)
            {

                ////Print Excel File Name
                PrintFileName(FilePath);
            }
            else
            {

                //Get the File Name
                string FileName = Path.GetFileNameWithoutExtension(FilePath);

                //Delete in case it is not found
                DesignUtils.DeleteStorageTextFileRun(FileName);

                //Error Message
                MessageBox.Show($"File '{FileName}' was not found and deleted from the list", "Select File", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //Open File (Standard)
        private void Button_MenuItem_OpenFile(object sender, RoutedEventArgs e)
        {

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";
            string Source = System.IO.File.ReadAllText(FilePath);

            //Check if all Parameters are in the File
            string[] searchWords = { "FilePathforPreview" + Utils.DefaultSeparator() };
            double PercResults = Utils.FindWordsInString(Source, searchWords, false);

            //Case it is found
            if (PercResults == 1)
            {

                //Get Lines from the File
                string[] Lines = Utils.SplitTextNewLine(Source);

                //Check if there is a File Path for the Preveiw
                string[] filterWords = { "FilePathforPreview" + Utils.DefaultSeparator() };
                string[] OutputResults = CallExtractions.CallFindArrayItems(Lines, filterWords, "Any", false);

                if (OutputResults.Length > 0)
                {
                    FilePath = OutputResults[0];

                    //Get File Path for the Preview
                    string[] MyArray = Strings.Split(FilePath, Utils.DefaultSeparator());
                    string CurrentFile = MyArray[1];

                    /////////////////////////////////////////////
                    ////Open File
                    System.Diagnostics.Process.Start(CurrentFile);
                    /////////////////////////////////////////////

                }

            }
        }

        private void Button_LoadDocument(object sender, RoutedEventArgs e)
        {

            OpenFileDialog _openFileDialog = new OpenFileDialog
            {
                Title = "Select Text File",
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "Text Files (*.txt)|*.txt"
            };

            if (_openFileDialog.ShowDialog() == true)
            {

                //Print File Name to Interface
                PrintFileName(_openFileDialog.FileName);

            }

        }

        //Print Excel File Name
        public void PrintFileName(string FileFullPath)
        {

            //Get the File Name
            string FileName = Path.GetFileNameWithoutExtension(FileFullPath);

            //Get the File Path
            string FilePath = DesignUtils.TrimFilePath(FileFullPath, Directory.GetCurrentDirectory());

            //Fill File Path to the Form
            ModelProperty property = this.ModelItem.Properties["FilePathPreview"];
            property.SetValue(new InArgument<string>(FilePath));

            //Check if Storage Files exists
            bool bExists = File.Exists(Directory.GetCurrentDirectory() + "/" + "StorageTextToolbox/FileNames/" + FileName + ".txt");

            //If false create directory files
            if (bExists == false)
            {

                //Write Excel File Sheets to Text File in Storage
                DesignUtils.WriteTextFileSingleFile(FilePath, MyIDText);

            }

            //Button Refresh Current File
            Button_RefreshCurrentFile();
        }

        public void Button_RefreshCurrentFile()
        {

            //Get Data from Control
            //string CurrentExcelFilePath = this.FilePath.Expression.ToString();
            string CurrentTextFilePath = ReturnCurrentFile();

            //Case it is a Variable
            if (CurrentTextFilePath == "1.5: VisualBasicValue<String>")
            {

                MessageBox.Show("File Path as Variable" + Environment.NewLine + "Please select Text file to use functionality ", "Select Text File", MessageBoxButton.OK, MessageBoxImage.Warning);

                //Exit the Procedure
                return;
            }

            //If Current File is not found, exit the procedure
            if (File.Exists(CurrentTextFilePath) == false)
            {

                //Error Message
                MessageBox.Show("The Preview file cannt be found:" + Environment.NewLine + CurrentTextFilePath, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                //Exit the Procedure
                return;
            }

            //In case file is found
            if (CurrentTextFilePath != null)
            {

                #region Update Text Files
                //Get the full file path
                string filePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames/" + Path.GetFileNameWithoutExtension(CurrentTextFilePath) + ".txt";

                //Check if file exists
                bool bExists = File.Exists(filePath);

                //Case File Exists
                switch (bExists)
                {

                    //File Exists
                    case true:

                        //Get the File Path
                        string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

                        //Get Current File
                        string CurrentFile = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/StorageTextToolbox/FileNames/" + Path.GetFileNameWithoutExtension(CurrentTextFilePath) + ".txt");

                        //Get Item from the ComboBox
                        string MyEncoding = ReturnEncoding();

                        if (MyEncoding != null)
                        {
                            Encoding encoding = Utils.ConvertStringToEncoding(MyEncoding);

                            //Update Text File Row Argument
                            MyArgument = "FileName";
                            DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, CurrentFile, encoding);

                        }

                        break;

                    //Case File does not exists
                    case false:
                        //Warning Message
                        MessageBox.Show("This funcionality is not available", "File does not exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;

                }

            }
            #endregion
            else
            {
                //Error Message
                MessageBox.Show("Please select template file", "Select Text File", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        //Button Open Wizard
        private void Button_OpenFormSelectData(object sender, RoutedEventArgs e)
        {

            //Show Update Call Control
            this.SearchWord.Visibility = Visibility.Hidden;
            this.UpdateCall.Visibility = Visibility.Visible;
            this.UpdateCall.Content = Utils.DefaultUpdateControl();

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Get Item from the ComboBox
            string MyEncoding = ReturnEncoding();

            if (MyEncoding != null)
            {
                Encoding encoding = Utils.ConvertStringToEncoding(MyEncoding);

                string Source = System.IO.File.ReadAllText(FilePath, encoding);

                //Check if all Parameters are in the File
                string[] searchWords = { "FileName" + Utils.DefaultSeparator()};
                double PercResults = Utils.FindWordsInString(Source, searchWords, false);

                //Case it is found
                if (PercResults == 1)
                {

                    //Get Current File
                    FilePath = ReturnCurrentFile();

                    //Open Form Select Data
                    DesignUtils.CallformSelectDataOpen(MyArgument, MyIDText, FilePath, MyIDText, encoding);


                }
                else
                {
                    //Error Message
                    MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                //Error Message
                MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Button Open Preview
        private void Button_OpenPreview(object sender, RoutedEventArgs e)
        {

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Auto Fill Controls
            AutoFillControls();

            #region Open Preview Extraction

            //Get Item from the ComboBox
            string MyEncoding = ReturnEncoding();

            if (MyEncoding != null)
            {
                Encoding encoding = Utils.ConvertStringToEncoding(MyEncoding);

                //Read Text File
                string Source = System.IO.File.ReadAllText(FilePath, encoding);

                //Check if all Parameters are in the File
                string[] searchWords = { "FileName" + Utils.DefaultSeparator(), "SearchWord" + Utils.DefaultSeparator() };
                double PercResults = Utils.FindWordsInString(Source, searchWords, false);

                //Case all Parameters are found
                if (PercResults == 1)
                {
                    //Open Form Preview Extraction
                    DesignUtils.CallformPreviewExtraction(MyIDText, "Match Item in Array", MyIDText, encoding);
                }
                else
                {
                    //Error Message
                    MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //Error Message
                MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }  

            #endregion

        }

        //Paste to from the Clipboard
        private void Button_PasteFromClipboard(object sender, RoutedEventArgs e)
        {

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Get Item from the ComboBox
            string MyEncoding = ReturnEncoding();

            if (MyEncoding != null)
            {
                Encoding encoding = Encoding.Default;

                //Get Encoding
                encoding = DesignUtils.GetEncodingIDText(MyIDText);

                //Paste Argument from the Clipboard
                string OutputText = DesignUtils.PasteArgumentFromClipboard();

                //Update Control
                UpdateControl(MyArgument.Replace(" ", ""), OutputText);

                //Update Text File Row Argument
                DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, OutputText, encoding);

            }

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

        //Create New TextID
        private void CreateNewIDText(object sender, RoutedEventArgs e)
        {
            string FilePath = null;

            //Get Item from the ComboBox
            string MyEncoding = ReturnEncoding();

            if (MyEncoding != null)
            {

                //Get Encoding
                Encoding encoding = Utils.ConvertStringToEncoding(MyEncoding);

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
        }

        //Auto Fill Controls
        public void AutoFillControls()
        {
            //Check if File Exists, if not create it

            //Get the File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //If false create it
            if (File.Exists(FilePath) == false)
            {
                //Create File
                System.IO.File.WriteAllText(FilePath, "");
            }

            #region Encoding

            string MyArgument = "Encoding";

            //Get Item from the ComboBox
            string MyEncoding = ReturnEncoding();

            if (MyEncoding != null)
            {
                Encoding encoding = Utils.ConvertStringToEncoding(MyEncoding);

                //Log ComboBox
                DesignUtils.CallLogComboBox(MyIDText, MyArgument, MyEncoding, encoding);

                #region File Name

                //Copy the Arguments, in case there is
                string FilePathPreview = ReturnCurrentFile();

                if (FilePathPreview != null)
                {
                    //Update Text File Row Argument
                    DesignUtils.CallUpdateTextFileRowArgument(FilePath, "FileName", FilePathPreview, encoding);
                }

                #endregion
            }

            #endregion

        }



    }
}
