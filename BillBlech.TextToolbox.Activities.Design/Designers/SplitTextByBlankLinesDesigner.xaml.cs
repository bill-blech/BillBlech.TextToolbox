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
    /// Interaction logic for SplitTextByBlankLinesDesigner.xaml
    /// </summary>
    public partial class SplitTextByBlankLinesDesigner
    {
        public SplitTextByBlankLinesDesigner()
        {
            InitializeComponent();
        }


        //Setup Wizard Button
        private void CallCallButton_SetupWizard(object sender, System.Windows.RoutedEventArgs e)
        {

            //Setup Wizard Button
            CallButton_SetupWizard();
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
                //Wizard Button: Warning Message: Preview
                DesignUtils.Wizard_WarningMessage_Preview();
            }

        }

        //Button Open Preview
        private void Button_OpenPreview(object sender, RoutedEventArgs e)
        {

            //Open Form Preview Extraction
            DesignUtils.CallformPreviewExtraction(null, "Split Text By Blank Lines");

        }
    }
}
