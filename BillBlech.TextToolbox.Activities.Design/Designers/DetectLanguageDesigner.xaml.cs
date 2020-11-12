using BillBlech.TextToolbox.Activities.Activities.Encryption;
using System;
using System.Windows;

namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for DetectLanguageDesigner.xaml
    /// </summary>
    public partial class DetectLanguageDesigner
    {
        public DetectLanguageDesigner()
        {
            InitializeComponent();
        }

        //Detect Language
        private void Call_DetectLanguage(object sender, System.Windows.RoutedEventArgs e)
        {
            //Fill in the Variables
            
            //Input Text
            string inputText = ReturnInputText();

            #region Validation
            //Check if there are variables in each field

            //Input Text
            if (inputText == null)
            {
                //Warning Message
                MessageBox.Show("Please fill in the Input Text", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;

            }
            else
            {
                //Check for Variable
                if (inputText.Contains("VisualBasicValue") == true)
                {

                    //Validation Message
                    MessageBox.Show("Remove variables from 'Input Text' to Acess 'Preview'", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                    //Exit the Procedure
                    return;

                }
            }

            //Config File
            string configFile = ReturnConfigFile();

            //Validation Message
            if (configFile == null)
            {
                //Warning Message
                MessageBox.Show("Please fill in the Config File", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;

            }
            else
            {
                //Check for Variable
                if (configFile.Contains("VisualBasicValue") == true)
                {

                    //Validation Message
                    MessageBox.Show("Remove variables from 'Config File'  to Acess 'Preview'", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                    //Exit the Procedure
                    return;

                }

            }
            #endregion

            //Run the Activity
            string OutputLanguage = LanguageDetection.RunDetectLanguage(inputText, configFile);

            //Message Result
            MessageBox.Show("The output language is '" + OutputLanguage + "'", "Detect Language");

        }

        //Return Text
        private string ReturnInputText()
        {
            try
            {
                //Get the FilePath
                return this.InputText.Expression.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        //Return Config File
        private string ReturnConfigFile()
        {
            try
            {
                //Get the FilePath
                return this.ConfigFile.Expression.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }

        }


    }
}
