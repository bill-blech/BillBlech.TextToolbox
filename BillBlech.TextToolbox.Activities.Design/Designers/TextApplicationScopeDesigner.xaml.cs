using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for TextApplicationScopeDesigner.xaml
    /// </summary>
    public partial class TextApplicationScopeDesigner
    {
        public TextApplicationScopeDesigner()
        {
            InitializeComponent();
        }

        private void Button_LoadDocument(object sender, RoutedEventArgs e)
        {

            OpenFileDialog _openFileDialog = new OpenFileDialog
            {
                Title = "Select Text File",
                InitialDirectory = Directory.GetCurrentDirectory()
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
            string FilePath = Utils.TrimFilePath(FileFullPath, Directory.GetCurrentDirectory());

            ////Fill File Path to the Form
            ModelProperty property = this.ModelItem.Properties["InputText"];
            property.SetValue(new InArgument<string>(FilePath));

        }

    }
}
