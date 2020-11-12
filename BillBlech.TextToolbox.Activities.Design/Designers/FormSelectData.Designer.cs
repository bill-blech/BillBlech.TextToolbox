namespace ExcelTut
{
    partial class FormSelectData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LstAvailableItems = new System.Windows.Forms.ListBox();
            this.LstSelectedItems = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.groupLstSelectedItemsSelection = new System.Windows.Forms.GroupBox();
            this.groupLstSelectedItemsSelectionMulti = new System.Windows.Forms.RadioButton();
            this.groupLstSelectedItemsSelectionSingle = new System.Windows.Forms.RadioButton();
            this.GroupLstSelectedItemsMoveItem = new System.Windows.Forms.GroupBox();
            this.btnLstSelectedItemsDown = new System.Windows.Forms.Button();
            this.btnLstSelectedItemsUp = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupLstSelectedItemsMergeSplit = new System.Windows.Forms.GroupBox();
            this.btnSplit = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnCopyToClipBoard = new System.Windows.Forms.Button();
            this.SearchLstAvailableItems = new System.Windows.Forms.TextBox();
            this.SearchLstSelectedItems = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxAvailableItems = new System.Windows.Forms.GroupBox();
            this.groupBoxSelectedItems = new System.Windows.Forms.GroupBox();
            this.groupBoxManual = new System.Windows.Forms.GroupBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnManualAdd = new System.Windows.Forms.Button();
            this.txtManual = new System.Windows.Forms.TextBox();
            this.groupActions = new System.Windows.Forms.GroupBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.groupLstAvailableItemsSource = new System.Windows.Forms.GroupBox();
            this.groupLstAvailableItemsSourceLines = new System.Windows.Forms.RadioButton();
            this.groupLstAvailableItemsSourceWords = new System.Windows.Forms.RadioButton();
            this.groupBoxSplit = new System.Windows.Forms.GroupBox();
            this.txtSplitSeparator = new System.Windows.Forms.TextBox();
            this.lblSplitSide = new System.Windows.Forms.Label();
            this.lblSplitSeparator = new System.Windows.Forms.Label();
            this.cbSplitSide = new System.Windows.Forms.ComboBox();
            this.cbSplitActivate = new System.Windows.Forms.CheckBox();
            this.groupLstSelectedItemsSelection.SuspendLayout();
            this.GroupLstSelectedItemsMoveItem.SuspendLayout();
            this.groupLstSelectedItemsMergeSplit.SuspendLayout();
            this.groupBoxAvailableItems.SuspendLayout();
            this.groupBoxSelectedItems.SuspendLayout();
            this.groupBoxManual.SuspendLayout();
            this.groupActions.SuspendLayout();
            this.groupLstAvailableItemsSource.SuspendLayout();
            this.groupBoxSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // LstAvailableItems
            // 
            this.LstAvailableItems.FormattingEnabled = true;
            this.LstAvailableItems.Location = new System.Drawing.Point(6, 62);
            this.LstAvailableItems.Name = "LstAvailableItems";
            this.LstAvailableItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LstAvailableItems.Size = new System.Drawing.Size(196, 290);
            this.LstAvailableItems.TabIndex = 0;
            this.LstAvailableItems.TabStop = false;
            this.LstAvailableItems.SelectedIndexChanged += new System.EventHandler(this.LstAvailableItems_SelectedIndexChanged);
            this.LstAvailableItems.DoubleClick += new System.EventHandler(this.LstAvailableItems_DoubleClick);
            // 
            // LstSelectedItems
            // 
            this.LstSelectedItems.FormattingEnabled = true;
            this.LstSelectedItems.Location = new System.Drawing.Point(6, 62);
            this.LstSelectedItems.Name = "LstSelectedItems";
            this.LstSelectedItems.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.LstSelectedItems.Size = new System.Drawing.Size(196, 199);
            this.LstSelectedItems.TabIndex = 1;
            this.LstSelectedItems.TabStop = false;
            this.LstSelectedItems.SelectedIndexChanged += new System.EventHandler(this.LstSelectedItems_SelectedIndexChanged);
            this.LstSelectedItems.DoubleClick += new System.EventHandler(this.LstSelectedItems_DoubleClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(271, 97);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(35, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = ">>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(237, 97);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(35, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.TabStop = false;
            this.btnRemove.Text = "<<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // groupLstSelectedItemsSelection
            // 
            this.groupLstSelectedItemsSelection.Controls.Add(this.groupLstSelectedItemsSelectionMulti);
            this.groupLstSelectedItemsSelection.Controls.Add(this.groupLstSelectedItemsSelectionSingle);
            this.groupLstSelectedItemsSelection.Location = new System.Drawing.Point(542, 15);
            this.groupLstSelectedItemsSelection.Name = "groupLstSelectedItemsSelection";
            this.groupLstSelectedItemsSelection.Size = new System.Drawing.Size(76, 77);
            this.groupLstSelectedItemsSelection.TabIndex = 10;
            this.groupLstSelectedItemsSelection.TabStop = false;
            this.groupLstSelectedItemsSelection.Text = "Selection";
            // 
            // groupLstSelectedItemsSelectionMulti
            // 
            this.groupLstSelectedItemsSelectionMulti.AutoSize = true;
            this.groupLstSelectedItemsSelectionMulti.Location = new System.Drawing.Point(6, 47);
            this.groupLstSelectedItemsSelectionMulti.Name = "groupLstSelectedItemsSelectionMulti";
            this.groupLstSelectedItemsSelectionMulti.Size = new System.Drawing.Size(47, 17);
            this.groupLstSelectedItemsSelectionMulti.TabIndex = 3;
            this.groupLstSelectedItemsSelectionMulti.TabStop = true;
            this.groupLstSelectedItemsSelectionMulti.Text = "Multi";
            this.groupLstSelectedItemsSelectionMulti.UseVisualStyleBackColor = true;
            this.groupLstSelectedItemsSelectionMulti.CheckedChanged += new System.EventHandler(this.groupLstSelectedItemsSelectionMulti_CheckedChanged);
            // 
            // groupLstSelectedItemsSelectionSingle
            // 
            this.groupLstSelectedItemsSelectionSingle.AutoSize = true;
            this.groupLstSelectedItemsSelectionSingle.Location = new System.Drawing.Point(6, 24);
            this.groupLstSelectedItemsSelectionSingle.Name = "groupLstSelectedItemsSelectionSingle";
            this.groupLstSelectedItemsSelectionSingle.Size = new System.Drawing.Size(54, 17);
            this.groupLstSelectedItemsSelectionSingle.TabIndex = 2;
            this.groupLstSelectedItemsSelectionSingle.TabStop = true;
            this.groupLstSelectedItemsSelectionSingle.Text = "Single";
            this.groupLstSelectedItemsSelectionSingle.UseVisualStyleBackColor = true;
            this.groupLstSelectedItemsSelectionSingle.CheckedChanged += new System.EventHandler(this.groupLstSelectedItemsSelectionSingle_CheckedChanged);
            // 
            // GroupLstSelectedItemsMoveItem
            // 
            this.GroupLstSelectedItemsMoveItem.Controls.Add(this.btnLstSelectedItemsDown);
            this.GroupLstSelectedItemsMoveItem.Controls.Add(this.btnLstSelectedItemsUp);
            this.GroupLstSelectedItemsMoveItem.Location = new System.Drawing.Point(542, 188);
            this.GroupLstSelectedItemsMoveItem.Name = "GroupLstSelectedItemsMoveItem";
            this.GroupLstSelectedItemsMoveItem.Size = new System.Drawing.Size(75, 84);
            this.GroupLstSelectedItemsMoveItem.TabIndex = 11;
            this.GroupLstSelectedItemsMoveItem.TabStop = false;
            this.GroupLstSelectedItemsMoveItem.Text = "Move Item";
            // 
            // btnLstSelectedItemsDown
            // 
            this.btnLstSelectedItemsDown.Location = new System.Drawing.Point(10, 51);
            this.btnLstSelectedItemsDown.Name = "btnLstSelectedItemsDown";
            this.btnLstSelectedItemsDown.Size = new System.Drawing.Size(49, 23);
            this.btnLstSelectedItemsDown.TabIndex = 11;
            this.btnLstSelectedItemsDown.Text = "Down";
            this.btnLstSelectedItemsDown.UseVisualStyleBackColor = true;
            this.btnLstSelectedItemsDown.Click += new System.EventHandler(this.btnLstSelectedItemsDown_Click);
            // 
            // btnLstSelectedItemsUp
            // 
            this.btnLstSelectedItemsUp.Location = new System.Drawing.Point(10, 22);
            this.btnLstSelectedItemsUp.Name = "btnLstSelectedItemsUp";
            this.btnLstSelectedItemsUp.Size = new System.Drawing.Size(49, 23);
            this.btnLstSelectedItemsUp.TabIndex = 10;
            this.btnLstSelectedItemsUp.Text = "Up";
            this.btnLstSelectedItemsUp.UseVisualStyleBackColor = true;
            this.btnLstSelectedItemsUp.Click += new System.EventHandler(this.btnLstSelectedItemsUp_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(240, 345);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(64, 23);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // groupLstSelectedItemsMergeSplit
            // 
            this.groupLstSelectedItemsMergeSplit.Controls.Add(this.btnSplit);
            this.groupLstSelectedItemsMergeSplit.Controls.Add(this.btnMerge);
            this.groupLstSelectedItemsMergeSplit.Location = new System.Drawing.Point(542, 98);
            this.groupLstSelectedItemsMergeSplit.Name = "groupLstSelectedItemsMergeSplit";
            this.groupLstSelectedItemsMergeSplit.Size = new System.Drawing.Size(75, 84);
            this.groupLstSelectedItemsMergeSplit.TabIndex = 13;
            this.groupLstSelectedItemsMergeSplit.TabStop = false;
            this.groupLstSelectedItemsMergeSplit.Text = "Adjust";
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(9, 50);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(49, 23);
            this.btnSplit.TabIndex = 12;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(9, 22);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(49, 23);
            this.btnMerge.TabIndex = 11;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnCopyToClipBoard
            // 
            this.btnCopyToClipBoard.Location = new System.Drawing.Point(10, 19);
            this.btnCopyToClipBoard.Name = "btnCopyToClipBoard";
            this.btnCopyToClipBoard.Size = new System.Drawing.Size(50, 23);
            this.btnCopyToClipBoard.TabIndex = 14;
            this.btnCopyToClipBoard.Text = "Save";
            this.btnCopyToClipBoard.UseVisualStyleBackColor = true;
            this.btnCopyToClipBoard.Click += new System.EventHandler(this.btnCopyToClipBoard_Click);
            // 
            // SearchLstAvailableItems
            // 
            this.SearchLstAvailableItems.Location = new System.Drawing.Point(6, 31);
            this.SearchLstAvailableItems.Name = "SearchLstAvailableItems";
            this.SearchLstAvailableItems.Size = new System.Drawing.Size(196, 20);
            this.SearchLstAvailableItems.TabIndex = 0;
            this.SearchLstAvailableItems.TextChanged += new System.EventHandler(this.SearchLstAvailableItems_TextChanged);
            // 
            // SearchLstSelectedItems
            // 
            this.SearchLstSelectedItems.Location = new System.Drawing.Point(6, 30);
            this.SearchLstSelectedItems.Name = "SearchLstSelectedItems";
            this.SearchLstSelectedItems.Size = new System.Drawing.Size(196, 20);
            this.SearchLstSelectedItems.TabIndex = 16;
            this.SearchLstSelectedItems.TextChanged += new System.EventHandler(this.SearchLstSelectedItems_TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(11, 77);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(49, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // groupBoxAvailableItems
            // 
            this.groupBoxAvailableItems.Controls.Add(this.SearchLstAvailableItems);
            this.groupBoxAvailableItems.Controls.Add(this.LstAvailableItems);
            this.groupBoxAvailableItems.Location = new System.Drawing.Point(12, 15);
            this.groupBoxAvailableItems.Name = "groupBoxAvailableItems";
            this.groupBoxAvailableItems.Size = new System.Drawing.Size(216, 361);
            this.groupBoxAvailableItems.TabIndex = 0;
            this.groupBoxAvailableItems.TabStop = false;
            this.groupBoxAvailableItems.Text = "Available Items";
            // 
            // groupBoxSelectedItems
            // 
            this.groupBoxSelectedItems.Controls.Add(this.SearchLstSelectedItems);
            this.groupBoxSelectedItems.Controls.Add(this.LstSelectedItems);
            this.groupBoxSelectedItems.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBoxSelectedItems.Location = new System.Drawing.Point(318, 15);
            this.groupBoxSelectedItems.Name = "groupBoxSelectedItems";
            this.groupBoxSelectedItems.Size = new System.Drawing.Size(216, 268);
            this.groupBoxSelectedItems.TabIndex = 19;
            this.groupBoxSelectedItems.TabStop = false;
            this.groupBoxSelectedItems.Text = "Selected Items";
            // 
            // groupBoxManual
            // 
            this.groupBoxManual.Controls.Add(this.btnEdit);
            this.groupBoxManual.Controls.Add(this.btnManualAdd);
            this.groupBoxManual.Controls.Add(this.txtManual);
            this.groupBoxManual.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBoxManual.Location = new System.Drawing.Point(318, 289);
            this.groupBoxManual.Name = "groupBoxManual";
            this.groupBoxManual.Size = new System.Drawing.Size(216, 87);
            this.groupBoxManual.TabIndex = 20;
            this.groupBoxManual.TabStop = false;
            this.groupBoxManual.Text = "Manual";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(61, 55);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(51, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnManualAdd
            // 
            this.btnManualAdd.Location = new System.Drawing.Point(10, 55);
            this.btnManualAdd.Name = "btnManualAdd";
            this.btnManualAdd.Size = new System.Drawing.Size(51, 23);
            this.btnManualAdd.TabIndex = 1;
            this.btnManualAdd.Text = "Add";
            this.btnManualAdd.UseVisualStyleBackColor = true;
            this.btnManualAdd.Click += new System.EventHandler(this.btnManualAdd_Click);
            // 
            // txtManual
            // 
            this.txtManual.Location = new System.Drawing.Point(10, 23);
            this.txtManual.Name = "txtManual";
            this.txtManual.Size = new System.Drawing.Size(200, 20);
            this.txtManual.TabIndex = 0;
            this.txtManual.TextChanged += new System.EventHandler(this.txtManual_TextChanged);
            // 
            // groupActions
            // 
            this.groupActions.Controls.Add(this.btnPaste);
            this.groupActions.Controls.Add(this.btnCopyToClipBoard);
            this.groupActions.Controls.Add(this.btnClose);
            this.groupActions.Location = new System.Drawing.Point(542, 278);
            this.groupActions.Name = "groupActions";
            this.groupActions.Size = new System.Drawing.Size(75, 106);
            this.groupActions.TabIndex = 21;
            this.groupActions.TabStop = false;
            this.groupActions.Text = "Actions";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(11, 48);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(49, 23);
            this.btnPaste.TabIndex = 22;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // groupLstAvailableItemsSource
            // 
            this.groupLstAvailableItemsSource.Controls.Add(this.groupLstAvailableItemsSourceLines);
            this.groupLstAvailableItemsSource.Controls.Add(this.groupLstAvailableItemsSourceWords);
            this.groupLstAvailableItemsSource.Location = new System.Drawing.Point(236, 15);
            this.groupLstAvailableItemsSource.Name = "groupLstAvailableItemsSource";
            this.groupLstAvailableItemsSource.Size = new System.Drawing.Size(76, 77);
            this.groupLstAvailableItemsSource.TabIndex = 22;
            this.groupLstAvailableItemsSource.TabStop = false;
            this.groupLstAvailableItemsSource.Text = "Source";
            // 
            // groupLstAvailableItemsSourceLines
            // 
            this.groupLstAvailableItemsSourceLines.AutoSize = true;
            this.groupLstAvailableItemsSourceLines.Location = new System.Drawing.Point(6, 47);
            this.groupLstAvailableItemsSourceLines.Name = "groupLstAvailableItemsSourceLines";
            this.groupLstAvailableItemsSourceLines.Size = new System.Drawing.Size(50, 17);
            this.groupLstAvailableItemsSourceLines.TabIndex = 3;
            this.groupLstAvailableItemsSourceLines.TabStop = true;
            this.groupLstAvailableItemsSourceLines.Text = "Lines";
            this.groupLstAvailableItemsSourceLines.UseVisualStyleBackColor = true;
            this.groupLstAvailableItemsSourceLines.CheckedChanged += new System.EventHandler(this.groupLstAvailableItemsSourceLines_CheckedChanged);
            // 
            // groupLstAvailableItemsSourceWords
            // 
            this.groupLstAvailableItemsSourceWords.AutoSize = true;
            this.groupLstAvailableItemsSourceWords.Location = new System.Drawing.Point(6, 24);
            this.groupLstAvailableItemsSourceWords.Name = "groupLstAvailableItemsSourceWords";
            this.groupLstAvailableItemsSourceWords.Size = new System.Drawing.Size(56, 17);
            this.groupLstAvailableItemsSourceWords.TabIndex = 2;
            this.groupLstAvailableItemsSourceWords.TabStop = true;
            this.groupLstAvailableItemsSourceWords.Text = "Words";
            this.groupLstAvailableItemsSourceWords.UseVisualStyleBackColor = true;
            this.groupLstAvailableItemsSourceWords.CheckedChanged += new System.EventHandler(this.groupLstAvailableItemsSourceWords_CheckedChanged);
            // 
            // groupBoxSplit
            // 
            this.groupBoxSplit.Controls.Add(this.txtSplitSeparator);
            this.groupBoxSplit.Controls.Add(this.lblSplitSide);
            this.groupBoxSplit.Controls.Add(this.lblSplitSeparator);
            this.groupBoxSplit.Controls.Add(this.cbSplitSide);
            this.groupBoxSplit.Controls.Add(this.cbSplitActivate);
            this.groupBoxSplit.Location = new System.Drawing.Point(236, 126);
            this.groupBoxSplit.Name = "groupBoxSplit";
            this.groupBoxSplit.Size = new System.Drawing.Size(76, 141);
            this.groupBoxSplit.TabIndex = 23;
            this.groupBoxSplit.TabStop = false;
            this.groupBoxSplit.Text = "Split";
            // 
            // txtSplitSeparator
            // 
            this.txtSplitSeparator.Location = new System.Drawing.Point(4, 59);
            this.txtSplitSeparator.Name = "txtSplitSeparator";
            this.txtSplitSeparator.Size = new System.Drawing.Size(66, 20);
            this.txtSplitSeparator.TabIndex = 25;
            // 
            // lblSplitSide
            // 
            this.lblSplitSide.AutoSize = true;
            this.lblSplitSide.Location = new System.Drawing.Point(7, 87);
            this.lblSplitSide.Name = "lblSplitSide";
            this.lblSplitSide.Size = new System.Drawing.Size(28, 13);
            this.lblSplitSide.TabIndex = 25;
            this.lblSplitSide.Text = "Side";
            // 
            // lblSplitSeparator
            // 
            this.lblSplitSeparator.AutoSize = true;
            this.lblSplitSeparator.Location = new System.Drawing.Point(6, 39);
            this.lblSplitSeparator.Name = "lblSplitSeparator";
            this.lblSplitSeparator.Size = new System.Drawing.Size(53, 13);
            this.lblSplitSeparator.TabIndex = 24;
            this.lblSplitSeparator.Text = "Separator";
            // 
            // cbSplitSide
            // 
            this.cbSplitSide.FormattingEnabled = true;
            this.cbSplitSide.Location = new System.Drawing.Point(4, 107);
            this.cbSplitSide.Name = "cbSplitSide";
            this.cbSplitSide.Size = new System.Drawing.Size(64, 21);
            this.cbSplitSide.TabIndex = 26;
            this.cbSplitSide.TabStop = false;
            // 
            // cbSplitActivate
            // 
            this.cbSplitActivate.AutoSize = true;
            this.cbSplitActivate.Location = new System.Drawing.Point(5, 19);
            this.cbSplitActivate.Name = "cbSplitActivate";
            this.cbSplitActivate.Size = new System.Drawing.Size(65, 17);
            this.cbSplitActivate.TabIndex = 24;
            this.cbSplitActivate.TabStop = false;
            this.cbSplitActivate.Text = "Activate";
            this.cbSplitActivate.UseVisualStyleBackColor = true;
            this.cbSplitActivate.CheckedChanged += new System.EventHandler(this.cbSplitActivate_CheckedChanged);
            // 
            // FormSelectData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 389);
            this.Controls.Add(this.groupBoxSplit);
            this.Controls.Add(this.groupLstAvailableItemsSource);
            this.Controls.Add(this.groupActions);
            this.Controls.Add(this.groupBoxManual);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBoxSelectedItems);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBoxAvailableItems);
            this.Controls.Add(this.groupLstSelectedItemsMergeSplit);
            this.Controls.Add(this.GroupLstSelectedItemsMoveItem);
            this.Controls.Add(this.groupLstSelectedItemsSelection);
            this.Controls.Add(this.btnRemove);
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Name = "FormSelectData";
            this.ShowIcon = false;
            this.Text = "FormSelectData";
            this.Load += new System.EventHandler(this.FormSelectData_Load);
            this.groupLstSelectedItemsSelection.ResumeLayout(false);
            this.groupLstSelectedItemsSelection.PerformLayout();
            this.GroupLstSelectedItemsMoveItem.ResumeLayout(false);
            this.groupLstSelectedItemsMergeSplit.ResumeLayout(false);
            this.groupBoxAvailableItems.ResumeLayout(false);
            this.groupBoxAvailableItems.PerformLayout();
            this.groupBoxSelectedItems.ResumeLayout(false);
            this.groupBoxSelectedItems.PerformLayout();
            this.groupBoxManual.ResumeLayout(false);
            this.groupBoxManual.PerformLayout();
            this.groupActions.ResumeLayout(false);
            this.groupLstAvailableItemsSource.ResumeLayout(false);
            this.groupLstAvailableItemsSource.PerformLayout();
            this.groupBoxSplit.ResumeLayout(false);
            this.groupBoxSplit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LstAvailableItems;
        private System.Windows.Forms.ListBox LstSelectedItems;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox groupLstSelectedItemsSelection;
        private System.Windows.Forms.RadioButton groupLstSelectedItemsSelectionMulti;
        private System.Windows.Forms.RadioButton groupLstSelectedItemsSelectionSingle;
        private System.Windows.Forms.GroupBox GroupLstSelectedItemsMoveItem;
        private System.Windows.Forms.Button btnLstSelectedItemsDown;
        private System.Windows.Forms.Button btnLstSelectedItemsUp;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupLstSelectedItemsMergeSplit;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Button btnCopyToClipBoard;
        private System.Windows.Forms.TextBox SearchLstAvailableItems;
        private System.Windows.Forms.TextBox SearchLstSelectedItems;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBoxAvailableItems;
        private System.Windows.Forms.GroupBox groupBoxSelectedItems;
        private System.Windows.Forms.GroupBox groupBoxManual;
        private System.Windows.Forms.TextBox txtManual;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnManualAdd;
        private System.Windows.Forms.GroupBox groupActions;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.GroupBox groupLstAvailableItemsSource;
        private System.Windows.Forms.RadioButton groupLstAvailableItemsSourceLines;
        private System.Windows.Forms.RadioButton groupLstAvailableItemsSourceWords;
        private System.Windows.Forms.GroupBox groupBoxSplit;
        private System.Windows.Forms.CheckBox cbSplitActivate;
        private System.Windows.Forms.ComboBox cbSplitSide;
        private System.Windows.Forms.Label lblSplitSide;
        private System.Windows.Forms.TextBox txtSplitSeparator;
        private System.Windows.Forms.Label lblSplitSeparator;
    }
}