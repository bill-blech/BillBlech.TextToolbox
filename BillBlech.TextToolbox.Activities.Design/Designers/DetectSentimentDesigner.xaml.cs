using BillBlech.TextToolbox.Activities.Activities;
using System;
using System.Windows;


namespace BillBlech.TextToolbox.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for DetectSentimentDesigner.xaml
    /// </summary>
    public partial class DetectSentimentDesigner
    {
        public DetectSentimentDesigner()
        {
            InitializeComponent();
        }

        //Detect Language
        private void Call_DetectSentiment(object sender, System.Windows.RoutedEventArgs e)
        {
            //Fill in the Variables

            //Input Text
            string inputText = ReturnInputText();

            #region Validation
            //Validation Message
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
            #endregion

            //Run the Activity
            string SentimentResult = textApiSentiment.ReturnTextSentiment(inputText);

            //Message Result
            MessageBox.Show("The sentiment analysis is '" + SentimentResult + "'");

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

    }  
}
