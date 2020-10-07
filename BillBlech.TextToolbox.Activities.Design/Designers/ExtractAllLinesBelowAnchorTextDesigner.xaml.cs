using BillBlech.TextToolbox.Activities.Activities;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for ExtractAllLinesBelowAnchorTextDesigner.xaml
    /// </summary>
    public partial class ExtractAllLinesBelowAnchorTextDesigner
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

        //Anchor Words Parameter Update Event
        private void AnchorWordsParamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Update IDText
            UpdateIDText();

            //Fill in Global Variable
            MyArgument = "Anchor Words Parameter";

            //Get ITem from the ComboBox
            string MyAnchorTextParamComboBox = this.AnchorWordsParamComboBox.SelectedItem.ToString();

            //Log ComboBox
            DesignUtils.CallLogComboBox(MyIDText, MyArgument, MyAnchorTextParamComboBox);
            
        }
        #endregion

        public ExtractAllLinesBelowAnchorTextDesigner()
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
            MyArgument = "Anchor Words";

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

                #region Hidden
                //MessageBox.Show(this.IDText.Expression.ToString());

                //string MyNow = DateTime.Now.ToString();
                //ModelProperty property = this.ModelItem.Properties["IDText"];
                //property.SetValue(new InArgument<string>(MyNow));

                //this.lblTimer.Content = "Anchor Words: Press (Ctrl+V)";            
                //Clipboard.SetText("{\"A\"}");

                //string myText = Clipboard.GetText();

                //// Step 1: create list.
                //List<string> list = new List<string>();
                //list.Add("one");
                //list.Add("two");
                //list.Add("three");
                //list.Add("four");
                //list.Add("five");

                //// Step 2: convert to string array.
                //System.Activities.InArgument<string[]> Myarray = new System.Activities.InArgument<string[]>();
                //Myarray = list.ToArray();


                //string z = AnchorText.ToString();
                //MessageBox.Show(z);

                //CSharpValue<string[]> Myarray1 = { "A", "b" };

                ////System.Activities.InArgument<string[]> xx = { "A", "b" };

                //ModelProperty property = this.ModelItem.Properties["AnchorText"];
                //property.SetValue(Myarray);

                //var z = AnchorText.Expression.ItemType;

                //MessageBox.Show(z.ToString());

                //System.Activities.Expressions.Literal<string[]> xx = new System.Activities.Expressions.Literal<string[]> { "A", "B" };

                //xx = AnchorText.Expression;


                //var zzz = AnchorText.Expression.Properties.ToArray(); { } ;

                //MessageBox.Show(zzz[0].ToString());

                ////System.Activities.Expressions.Literal<string[]> ww = AnchorText.Expression.Properties.ToArray() <string>;



                //        Variable<int> n = new Variable<int>
                //        {
                //            Name = "n"
                //        };

                //        Activity wf = new Sequence
                //        {
                //            Variables = { n },
                //            Activities =
                //{
                //    new Assign<int>
                //    {
                //        To = new CSharpReference<int>("n"),
                //        Value = new CSharpValue<int>("new Random().Next(1, 101)")
                //    },
                //    new WriteLine
                //    {
                //        Text = new CSharpValue<string>("\"The number is \" + n")
                //    }
                //}
                //        };

                //        CompileExpressions(wf);

                //        WorkflowInvoker.Invoke(wf);

                //var x = AnchorText.Expression;

                //VisualBasicValue<string[]> y = new VisualBasicValue<string[]>(x);

                //string y = string.Join(Environment.NewLine, x);

                ////MessageBox.Show(y);


                //Variable<string[]> MyArray1 = new Variable<string[]>("n");

                //MyArray1 = new CSharpValue<string[]>{"Hello","World"}; 




                ////Variable<int> n = new Variable<int>;

                ////var x = new CSharpValue<string[]>();
                ////x = {"Hello"};


                ////ModelProperty p1 = this.ModelItem.Properties["AnchorText"];
                ////p1.SetValue(MyArray);

                //////property.SetValue(new InArgument<string>(FilePath));


                //ModelProperty property = this.ModelItem.Properties["AnchorText"];
                //property.SetValue(new InArgument<string[]>(MyArray));



            }

            ////Return Anchor Text
            //private string ReturnAnchorText()
            //{
            //    try
            //    {

            //        //Get the FilePath
            //        return this.AnchorText.Expression.ToString();
            //    }
            //    catch (Exception ex)
            //    {

            //        return null;
            //    }



            //}

            //private void AnchorText_TextChanged(object sender, RoutedEventArgs e)
            //{
            //    this.lblTimer.Content = "Anchor Words";
            //    MessageBox.Show("Hey");

            //}

            //private void YourTextEventHandler(object sender, System.Windows.Data.DataTransferEventArgs e)
            //{
            //    this.lblTimer.Content = "Anchor Words";
            //    MessageBox.Show("Hello");
            //}
            #endregion

            else
            {
                //Wizard Button: Warning Message: Wizard & Preview
                DesignUtils.Wizard_WarningMessage_Wizard_Preview();
            }

        }

        //Button Open Wizard
        private void Button_OpenFormSelectData(object sender, RoutedEventArgs e)
        {

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
            string[] searchWords = { "Anchor Words"+ Utils.DefaultSeparator(), "Anchor Words Parameter" + Utils.DefaultSeparator()};
            double PercResults = Utils.FindWordsInString(Source, searchWords, false);

            //Case all Parameters are found
            if (PercResults == 1)
            {
                //Open Form Preview Extraction
                DesignUtils.CallformPreviewExtraction(MyIDText, "Extract All Lines Below Anchor Words");
            }
            else
            {
                //Error Message
                MessageBox.Show("Please fill in all arguments", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            #endregion


        }


    }
}