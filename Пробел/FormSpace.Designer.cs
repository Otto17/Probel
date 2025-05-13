namespace Пробел
{
    partial class FormSpace
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSpace));
            labelInfo1 = new Label();
            labelInfo2 = new Label();
            InputTextBox = new TextBox();
            OutputTextBox = new TextBox();
            ButtonClear = new Button();
            CheckBoxCopy = new CheckBox();
            ButtonCopy = new Button();
            ButtonEraseBuf = new Button();
            PanelIndicator = new Panel();
            GorodDate = new Label();
            LinkLabel1 = new LinkLabel();
            NotifyIcon1 = new NotifyIcon(components);
            SuspendLayout();
            // 
            // labelInfo1
            // 
            labelInfo1.AutoSize = true;
            labelInfo1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelInfo1.Location = new Point(110, 9);
            labelInfo1.Name = "labelInfo1";
            labelInfo1.Size = new Size(154, 25);
            labelInfo1.TabIndex = 0;
            labelInfo1.Text = "Исходный текст";
            // 
            // labelInfo2
            // 
            labelInfo2.AutoSize = true;
            labelInfo2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelInfo2.Location = new Point(553, 9);
            labelInfo2.Name = "labelInfo2";
            labelInfo2.Size = new Size(96, 25);
            labelInfo2.TabIndex = 1;
            labelInfo2.Text = "Результат";
            // 
            // InputTextBox
            // 
            InputTextBox.Location = new Point(12, 37);
            InputTextBox.Multiline = true;
            InputTextBox.Name = "InputTextBox";
            InputTextBox.ScrollBars = ScrollBars.Vertical;
            InputTextBox.Size = new Size(368, 239);
            InputTextBox.TabIndex = 0;
            InputTextBox.TextChanged += InputTextBox_TextChanged;
            // 
            // OutputTextBox
            // 
            OutputTextBox.Location = new Point(420, 37);
            OutputTextBox.Multiline = true;
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ScrollBars = ScrollBars.Vertical;
            OutputTextBox.Size = new Size(368, 239);
            OutputTextBox.TabIndex = 1;
            OutputTextBox.TextChanged += OutputTextBox_TextChanged;
            // 
            // ButtonClear
            // 
            ButtonClear.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            ButtonClear.Location = new Point(420, 294);
            ButtonClear.Name = "ButtonClear";
            ButtonClear.Size = new Size(154, 43);
            ButtonClear.TabIndex = 4;
            ButtonClear.Text = "Очистить поля";
            ButtonClear.UseVisualStyleBackColor = true;
            ButtonClear.Click += ButtonClear_Click;
            // 
            // CheckBoxCopy
            // 
            CheckBoxCopy.AutoSize = true;
            CheckBoxCopy.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CheckBoxCopy.Location = new Point(217, 305);
            CheckBoxCopy.Name = "CheckBoxCopy";
            CheckBoxCopy.Size = new Size(163, 25);
            CheckBoxCopy.TabIndex = 3;
            CheckBoxCopy.Text = "Обновлять буфер";
            CheckBoxCopy.UseVisualStyleBackColor = true;
            CheckBoxCopy.CheckedChanged += CheckBoxCopy_CheckedChanged;
            // 
            // ButtonCopy
            // 
            ButtonCopy.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            ButtonCopy.Location = new Point(647, 294);
            ButtonCopy.Name = "ButtonCopy";
            ButtonCopy.Size = new Size(141, 43);
            ButtonCopy.TabIndex = 5;
            ButtonCopy.Text = "Скопировать";
            ButtonCopy.UseVisualStyleBackColor = true;
            ButtonCopy.Click += ButtonCopy_Click;
            // 
            // ButtonEraseBuf
            // 
            ButtonEraseBuf.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            ButtonEraseBuf.Location = new Point(12, 294);
            ButtonEraseBuf.Name = "ButtonEraseBuf";
            ButtonEraseBuf.Size = new Size(141, 43);
            ButtonEraseBuf.TabIndex = 2;
            ButtonEraseBuf.Text = "Стереть буфер";
            ButtonEraseBuf.UseVisualStyleBackColor = true;
            ButtonEraseBuf.Click += ButtonEraseBuf_Click;
            // 
            // PanelIndicator
            // 
            PanelIndicator.Location = new Point(160, 310);
            PanelIndicator.Name = "PanelIndicator";
            PanelIndicator.Size = new Size(14, 14);
            PanelIndicator.TabIndex = 6;
            // 
            // GorodDate
            // 
            GorodDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            GorodDate.AutoSize = true;
            GorodDate.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            GorodDate.Location = new Point(12, 345);
            GorodDate.Name = "GorodDate";
            GorodDate.Size = new Size(69, 12);
            GorodDate.TabIndex = 7;
            GorodDate.Text = "г. Омск 13.05.25";
            // 
            // LinkLabel1
            // 
            LinkLabel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LinkLabel1.AutoSize = true;
            LinkLabel1.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            LinkLabel1.Location = new Point(87, 345);
            LinkLabel1.Name = "LinkLabel1";
            LinkLabel1.Size = new Size(50, 12);
            LinkLabel1.TabIndex = 8;
            LinkLabel1.TabStop = true;
            LinkLabel1.Text = "Автор Otto";
            LinkLabel1.LinkClicked += LinkLabel1_LinkClicked;
            // 
            // NotifyIcon1
            // 
            NotifyIcon1.Icon = (Icon)resources.GetObject("NotifyIcon1.Icon");
            NotifyIcon1.Text = "Пробел";
            NotifyIcon1.MouseClick += NotifyIcon1_MouseClick;
            // 
            // FormSpace
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 364);
            Controls.Add(LinkLabel1);
            Controls.Add(GorodDate);
            Controls.Add(PanelIndicator);
            Controls.Add(ButtonEraseBuf);
            Controls.Add(ButtonCopy);
            Controls.Add(CheckBoxCopy);
            Controls.Add(ButtonClear);
            Controls.Add(OutputTextBox);
            Controls.Add(InputTextBox);
            Controls.Add(labelInfo2);
            Controls.Add(labelInfo1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(816, 403);
            MinimumSize = new Size(816, 403);
            Name = "FormSpace";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Пробел";
            Resize += FormSpace_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelInfo1;
        private Label labelInfo2;
        private TextBox InputTextBox;
        private TextBox OutputTextBox;
        private Button ButtonClear;
        private CheckBox CheckBoxCopy;
        private Button ButtonCopy;
        private Button ButtonEraseBuf;
        private Panel PanelIndicator;
        private Label GorodDate;
        private LinkLabel LinkLabel1;
        private NotifyIcon NotifyIcon1;
    }
}
