using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Design;
using BillBlech.TextToolbox.Activities.Design.Designers;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//https://stackoverflow.com/questions/4796109/how-to-move-item-in-listbox-up-and-down
//https://stackoverflow.com/questions/33146505/listbox-multiple-selection-get-all-selected-values

namespace ExcelTut
{
    public partial class FormSelectData : Form
    {
        List<string> _itemsAvailableItems = new List<string>();
        List<string> _itemsSelectedItems = new List<string>();
        List<string> _itemsTemp = new List<string>();
        string[] arrayAvailableItems = null;
        string MyIDText = null;
        string MyIDTextParent = null;
        string MyArgument = null;
        string templateFilePath = null;
        Dictionary<string, string> DicArgumentsParent = new Dictionary<string, string>();
        Encoding encoding = Encoding.Default;

        public FormSelectData(string MyArgument, string TemplateFilePath, string MyIDText, string MyIDTextParent, Encoding encoding)
        {
            InitializeComponent();

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ControlBox = false;
            this.Text = MyArgument;
            this.MyArgument = MyArgument;
            this.MyIDText = MyIDText;
            this.MyIDTextParent = MyIDTextParent;
            this.encoding = encoding;

            this.Show();
            this.templateFilePath = TemplateFilePath;

            //Inicialize form procedures
            Inicialize_form_procedures();

        }

        //Inicialize form procedures
        private void Inicialize_form_procedures()
        {

            //#region Available Items
            ////Loop throught the items
            //foreach (string str in arrayAvailableItems)
            //{
            //    _itemsAvailableItems.Add(str);
            //}

            ////Update LstAvailableItems_Update
            //LstAvailableItems_Update();
            //#endregion

            //Parent Arguments
            DicArgumentsParent = LoadFormArguments(MyIDTextParent, encoding);

            ////get Encoding
            //string strEncoding = DicArgumentsParent["Encoding"];

            //encoding = Utils.ConvertStringToEncoding(strEncoding);

            #region Selected Items

            //Set Default

            //Selected Items
            this.groupLstSelectedItemsSelectionMulti.Checked = true;
            this.LstSelectedItems.SelectionMode = SelectionMode.MultiSimple;
            this.GroupLstSelectedItemsMoveItem.Visible = false;
        
            //Available Items
            this.groupLstAvailableItemsSourceWords.Checked = true;

            //Read Text File
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Check if File Exists
            bool bExists = File.Exists(FilePath);

            //Case it is found
            if (bExists == true)
            {

                string source = System.IO.File.ReadAllText(FilePath, encoding);
                //string source = Utils.ReadTextFileStream(FilePath);
                //MessageBox.Show(source);

                //Get all lines
                string[] Lines = System.IO.File.ReadAllLines(FilePath, encoding);

                //Loop through the lines: Search for the Argument
                foreach (string Line in Lines)
                {

                    //MessageBox.Show(Line);

                    //Split the Line
                    string[] MyArray = Strings.Split(Line, Utils.DefaultSeparator());

                    //Check if it matches Argument 
                    if (MyArgument == MyArray[0])
                    {

                        //Get the Argument
                        string MyArgument = MyArray[1];

                        //Remove Braces {} and "
                        MyArgument = MyArgument.Replace("{", "");
                        MyArgument = MyArgument.Replace("}", "");
                        MyArgument = MyArgument.Replace("\"", "");

                        //Split the Items
                        string[] MyItems = MyArgument.Split(',');

                        //Case there are multiple items
                        if (MyItems.Length > 0)
                        {
                            //Loop through the Items
                            foreach (string MyItem in MyItems)
                            {

                                //Add Item to the List
                                _itemsSelectedItems.Add(MyItem.Trim());
                            }
                        }
                        else
                        {

                            //Input Single Items
                            _itemsSelectedItems.Add(MyArgument.Trim());
                        }

                        //Update LstSelectedItems
                        LstSelectedItems_Update();

                        //Exit the Loop
                        break;
                    }

                }


            }

            #endregion

        }

        //Load Form Arguments
        private Dictionary<string, string> LoadFormArguments(string MyIDText, Encoding encoding)
        {

            Dictionary<string, string> MyDic = new Dictionary<string, string>();

            //Get File Path
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";

            //Remove Empty Rows from Text File
            Utils.TextFileRemoveEmptyRows(FilePath, encoding);

            //Read Text File and Split by new line
            string[] lines = System.IO.File.ReadAllLines(FilePath, encoding);

            //Loop through the Lines
            for (int i = 0; i < lines.Length; i++)
            {

                //Get the Line
                string line = lines[i];

                //Split the Line
                string[] MyArray = Strings.Split(line, Utils.DefaultSeparator());

                //Fill in the Variables
                string MyArgument = MyArray[0];
                string MyValue = MyArray[1];

                //Add to the Dictionary
                MyDic.Add(MyArgument, MyValue);

            }

            //Return the Dictionary
            return MyDic;

        }

        #region ClickButtons

        //Add Item to LstSelectedItems => Remove Item from LstAvailableItems
        private void Add_btnClickRun()
        {
            //Loop through the Selected Items
            var lst = LstAvailableItems.SelectedItems.Cast<string>();

            //Update LstSelectedItems
            foreach (string SelectedItem in lst)
            {
                //Add Item to LstSelectedItems
                _itemsSelectedItems.Add(SelectedItem);

            }

            //Update LstSelectedItems
            LstSelectedItems_Update();

            ////Update LstAvailableItems
            //foreach (string SelectedItem in lst)
            //{
            //    //Remove Item to LstAvailableItems
            //    _itemsAvailableItems.Remove(SelectedItem);

            //}

            ////Update LstAvailableItems_Update
            //LstAvailableItems_Update();

            //Clear Search Box
            SearchLstAvailableItems.Text = "";
            SearchLstAvailableItems.Focus();
        }

        //Call Add Item to LstSelectedItems => Remove Item from LstAvailableItems
        private void btnAdd_Click(object sender, EventArgs e)
        {

            //Add Item to LstSelectedItems => Remove Item from LstAvailableItems
            Add_btnClickRun();

        }

        //Call Add Item to LstSelectedItems => Remove Item from LstAvailableItems
        private void LstAvailableItems_DoubleClick(object sender, EventArgs e)
        {

            //Add Item to LstSelectedItems => Remove Item from LstAvailableItems
            Add_btnClickRun();
        }

        //Add Item to LstAvailableItems => Remove Item to LstSelectedItems
        private void Remove_btnClickRun()
        {
            //Loop through the Selected Items
            var lst = LstSelectedItems.SelectedItems.Cast<string>();

            //Update LstAvailableItems
            foreach (string SelectedItem in lst)
            {
                //Add Item to LstAvailableItems
                _itemsAvailableItems.Add(SelectedItem);

            }

            //Update LstAvailableItems_Update
            LstAvailableItems_Update();

            //Update LstSelectedItems
            foreach (string SelectedItem in lst)
            {
                //Remove Item to LstSelectedItems
                _itemsSelectedItems.Remove(SelectedItem);

            }

            //Update LstSelectedItems
            LstSelectedItems_Update();

            //Clear Search Box
            SearchLstSelectedItems.Text = "";
            SearchLstSelectedItems.Focus();
        }

        //Call Add Item to LstAvailableItems => Remove Item to LstSelectedItems
        private void btnRemove_Click(object sender, EventArgs e)
        {

            //Add Item to LstAvailableItems => Remove Item to LstSelectedItems
            Remove_btnClickRun();
        }

        //Call Add Item to LstAvailableItems => Remove Item to LstSelectedItems
        private void LstSelectedItems_DoubleClick(object sender, EventArgs e)
        {
            //Add Item to LstAvailableItems => Remove Item to LstSelectedItems
            Remove_btnClickRun();
        }

        //Reset Form
        private void btnReset_Click(object sender, EventArgs e)
        {

            //Clear Search TextBoxes
            this.SearchLstAvailableItems.Text = null;
            this.SearchLstSelectedItems.Text = null;

            //Clear Available Items
            _itemsAvailableItems.Clear();

            //Clear Selected Items
            _itemsSelectedItems.Clear();
            LstSelectedItems.Items.Clear();

            //Inicialize form procedures
            Inicialize_form_procedures();


        }

        //Merge Items
        private void btnMerge_Click(object sender, EventArgs e)
        {
            //Loop through the Selected Items
            var lst = LstSelectedItems.SelectedItems.Cast<string>();

            string NewString = null;

            //Update LstAvailableItems
            foreach (string SelectedItem in lst)
            {

                //Create New String
                NewString += " " + SelectedItem;

                //Remove Item to LstSelectedItems
                _itemsSelectedItems.Remove(SelectedItem);

            }

            //Case string is found
            if (NewString != null)
            {

                //Add Item to LstSelectedItems
                _itemsSelectedItems.Add(NewString.Trim());

                //Update LstSelectedItems
                LstSelectedItems_Update();
            }


        }

        //Split Items
        private void btnSplit_Click(object sender, EventArgs e)
        {
            //Loop through the Selected Items
            var lst = LstSelectedItems.SelectedItems.Cast<string>();

            //Update LstAvailableItems
            foreach (string SelectedItem in lst)
            {
                //Get Multiple Words from Selected Item
                string[] Words = SelectedItem.Split();

                //Check Array Lenght
                if (Words.Length > 1)
                {
                    //Case it is a 'Multi Word' Word
                    //Remove Item to LstSelectedItems
                    _itemsSelectedItems.Remove(SelectedItem);

                    //Loop through the Words
                    foreach (string Word in Words)
                    {
                        //Add Item to LstSelectedItems
                        _itemsSelectedItems.Add(Word.Trim());
                    }
                }

            }

            //Update LstSelectedItems
            LstSelectedItems_Update();
        }

        #endregion

        #region ListBoxes

        //Update LstAvailableItems
        private void LstAvailableItems_Update()
        {

            //Clear all the items
            LstAvailableItems.Items.Clear();

            //Add Items to LstAllItems
            //_itemsAvailableItens.AddRange(ArrayAvailableItems);
            foreach (string str in _itemsAvailableItems)
            {
                LstAvailableItems.Items.Add(str);
            }

            //Update ListBox
            //LstAvailableItems.Sorted = true;
            //LstAvailableItems.DataSource = null;
            //LstAvailableItems.DataSource = _itemsAvailableItens;
            LstAvailableItems.SelectedIndex = -1;
            //Clear Manual
            txtManual.Text = null;

        }

        //Update LstSelectedItems
        private void LstSelectedItems_Update()
        {

            //Clear all the items
            LstSelectedItems.Items.Clear();

            //_itemsAvailableItens.AddRange(ArrayAvailableItems);
            foreach (string str in _itemsSelectedItems)
            {
                LstSelectedItems.Items.Add(str);
            }

            //Update ListBox
            //LstSelectedItems.Sorted = true;
            //LstSelectedItems.DataSource = null;
            //LstSelectedItems.DataSource = _itemsSelectedItens;
            LstSelectedItems.SelectedIndex = -1;
            //Clear Manual
            txtManual.Text = null;
        }

        //Search LstAvailableItems
        private void SearchLstAvailableItems_TextChanged(object sender, EventArgs e)
        {


            //Case it is filled
            if (string.IsNullOrEmpty(SearchLstAvailableItems.Text) == false)
            {

                //Clear the Temporary List
                _itemsTemp.Clear();

                //Loop through the ListItems
                foreach (string str in LstAvailableItems.Items)
                {

                    //Check if List Item contains Search Text
                    if (str.ToUpper().Contains(SearchLstAvailableItems.Text.ToUpper()))
                    {

                        //Add Item to Temporary List
                        _itemsTemp.Add(str);
                    }
                }

                //Clear the ListBox
                LstAvailableItems.Items.Clear();

                //Add Items to the ListBox
                foreach (string str in _itemsTemp)
                {
                    LstAvailableItems.Items.Add(str);
                }

            }
            //Case it is null
            else if (SearchLstAvailableItems.Text == "")
            {

                //Clear the ListBox
                LstAvailableItems.Items.Clear();

                //Update LstAvailableItems
                LstAvailableItems_Update();

            }

        }

        //Search LstSelectedItems
        private void SearchLstSelectedItems_TextChanged(object sender, EventArgs e)
        {
            //Case it is filled
            if (string.IsNullOrEmpty(SearchLstSelectedItems.Text) == false)
            {

                //Clear the Temporary List
                _itemsTemp.Clear();

                //Loop through the ListItems
                foreach (string str in LstSelectedItems.Items)
                {

                    //Check if List Item contains Search Text
                    if (str.ToUpper().Contains(SearchLstSelectedItems.Text.ToUpper()))
                    {

                        //Add Item to Temporary List
                        _itemsTemp.Add(str);

                    }
                }

                //Clear the ListBox
                LstSelectedItems.Items.Clear();

                //Add Items to the ListBox
                foreach (string str in _itemsTemp)
                {
                    LstSelectedItems.Items.Add(str);
                }


            }
            //Case it is null
            else if (SearchLstSelectedItems.Text == "")
            {

                //Clear the ListBox
                LstSelectedItems.Items.Clear();

                //Update LstAvailableItems
                LstSelectedItems_Update();

            }
        }

        //LstSelectedItems: Move Down
        private void btnLstSelectedItemsDown_Click(object sender, EventArgs e)
        {
            //LstSelectedItems: Move Item
            MoveItemLstSelectedItems(1);
        }

        //LstSelectedItems: Move Up
        private void btnLstSelectedItemsUp_Click(object sender, EventArgs e)
        {
            //LstSelectedItems: Move Item
            MoveItemLstSelectedItems(-1);
        }

        //LstSelectedItems: Move Item
        public void MoveItemLstSelectedItems(int direction)
        {
            // Checking selected item
            if (LstSelectedItems.SelectedItem == null || LstSelectedItems.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = LstSelectedItems.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= LstSelectedItems.Items.Count)
                return; // Index out of range - nothing to do

            object selected = LstSelectedItems.SelectedItem;

            // Removing removable element
            LstSelectedItems.Items.Remove(selected);
            // Insert it in new position
            LstSelectedItems.Items.Insert(newIndex, selected);
            // Restore selection
            LstSelectedItems.SetSelected(newIndex, true);
        }

        #endregion

        private void FormSelectData_Load(object sender, EventArgs e)
        {

        }

        //LstSelectedItems: Selection Single
        private void groupLstSelectedItemsSelectionSingle_CheckedChanged(object sender, EventArgs e)
        {
            LstSelectedItems.SelectionMode = SelectionMode.One;
            LstSelectedItems.SelectedIndex = -1;
            this.GroupLstSelectedItemsMoveItem.Visible = true;
        }

        //LstSelectedItems: Selection Multi
        private void groupLstSelectedItemsSelectionMulti_CheckedChanged(object sender, EventArgs e)
        {
            LstSelectedItems.SelectionMode = SelectionMode.MultiSimple;
            LstSelectedItems.SelectedIndex = -1;
            this.GroupLstSelectedItemsMoveItem.Visible = false;
        }

        //Source: Words
        private void groupLstAvailableItemsSourceWords_CheckedChanged(object sender, EventArgs e)
        {

            //Clear Available Items
            _itemsAvailableItems.Clear();

            //Get Unique Words in File
            arrayAvailableItems = DesignUtils.GetUniqueWordsInFile(templateFilePath, encoding);

            //Sort Array
            Array.Sort(arrayAvailableItems);

            //Loop throught the items
            foreach (string str in arrayAvailableItems)
            {
                _itemsAvailableItems.Add(str);
            }

            //Update LstAvailableItems_Update
            LstAvailableItems_Update();

        }

        //Source: Lines
        private void groupLstAvailableItemsSourceLines_CheckedChanged(object sender, EventArgs e)
        {

            //Clear Available Items
            _itemsAvailableItems.Clear();

            string InputText = System.IO.File.ReadAllText(templateFilePath, encoding);
            //string InputText = Utils.ReadTextFileStream(templateFilePath);
            arrayAvailableItems = Utils.SplitTextNewLine(InputText);

            //Loop throught the items
            foreach (string str in arrayAvailableItems)
            {
                _itemsAvailableItems.Add(str);
            }

            //Update LstAvailableItems_Update
            LstAvailableItems_Update();

        }

        //Copy Data to the ClipBoard
        private void btnCopyToClipBoard_Click(object sender, EventArgs e)
        {

            string OutputString = null;
            string OutputText = null;
            int i = 0;
            string FilePath = Directory.GetCurrentDirectory() + "/StorageTextToolbox/Infos/" + MyIDText + ".txt";
            bool bNoItems = false;

            #region OutputString

            //Fill in Variable OutputString
            foreach (string SelectedItem in LstSelectedItems.Items)
            {

                if (i == 0)
                {
                    OutputString = "\"" + SelectedItem + "\"";
                }
                else
                {
                    OutputString += ", \"" + SelectedItem + "\"";
                }

                //Update the Counter
                i++;

            }

            //Fnish the Clause
            if (OutputString != null)
            {

                //Case items are found
                OutputString = "{" + OutputString + "}";
            }
            else
            {
                //Invalid
                bNoItems = true;
            }

            #endregion

            //Update Text File Row Argument
            DesignUtils.CallUpdateTextFileRowArgument(FilePath, MyArgument, OutputString, encoding);

            //Copy to Clipboard
            Clipboard.SetText(OutputString);

        CloseForm:
            //Close the Form
            this.Close();

        }

        //Close the Form
        private void btnClose_Click_1(object sender, EventArgs e)
        {

            //Copy to Clipboard
            Clipboard.SetText(Utils.DefaultSeparator());

            this.Close();
        }


        #region Manual

        //Load TextBox Manual
        private void LstSelectedItems_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Case item is found
            if (LstSelectedItems.SelectedIndex != -1)
            {
                this.txtManual.Text = this.LstSelectedItems.SelectedItem.ToString();
            }
            else
            {
                this.txtManual.Text = null;
            }

        }

        //Add Manual Item
        private void btnManualAdd_Click(object sender, EventArgs e)
        {
            //Get Data from the TextBox
            string ManualValue = this.txtManual.Text;

            if (ManualValue!= null)
            {
                //Add Item to LstSelectedItems
                _itemsSelectedItems.Add(ManualValue);

                //Update LstSelectedItems
                LstSelectedItems_Update();
            }

            
    }

        #endregion

        //Edit Item
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Get Data from the TextBox
            string ManualValue = this.txtManual.Text;

            if (ManualValue != null)
            {
                string CurrentValue = this.LstSelectedItems.SelectedItem.ToString();

                //Trim Variable
                CurrentValue = CurrentValue.Trim();

                //Remove Item to LstSelectedItems
                _itemsSelectedItems.Remove(CurrentValue);

                //Update Selected ITem
                LstSelectedItems.Items[LstSelectedItems.SelectedIndex] = ManualValue;

                //Add Item to LstSelectedItems
                _itemsSelectedItems.Add(ManualValue);

                //Update LstSelectedItems
                LstSelectedItems_Update();

            }

                
        }

        //Paste Data from Clipboard
        private void btnPaste_Click(object sender, EventArgs e)
        {

            //Get Text from the Clipboard
            string ClipboardText = Clipboard.GetText();

            //Case it is not Null
            if (ClipboardText != null)
            {
                //Get Data between brackets {}
                string[] Results = Utils.ExtractTextBetweenTags(ClipboardText, "{", "}",System.Text.RegularExpressions.RegexOptions.Singleline,false,false);

                //Case Items are found
                if (Results.Length > 0)
                {

                    //Get Words from the Text
                    string Text = Results[0];
                    string[] Words = Strings.Split(Text, ",");

                    //Loop through the Words
                    foreach (string Word in Words)
                    {
                        string WordAdj = Strings.Replace(Word, "\"", "");
                        LstSelectedItems.Items.Add(WordAdj.Trim());
                    }

                }


            }
            else 
            {

                //Validation Error Message
                System.Windows.MessageBox.Show("There is no data in the Clipboard", "Copy from Clipboard", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

            }    



        }

        
    }
}