namespace PROG6221
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            BtnStart = new Button();
            BtnAsk = new Button();
            BtnViewTasks = new Button();
            TxtName = new TextBox();
            TxtQuestion = new TextBox();
            RtbConversation = new RichTextBox();
            SuspendLayout();

            // BtnStart
            BtnStart.Location = new Point(280, 20);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(100, 23);
            BtnStart.TabIndex = 0;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;

            // BtnAsk
            BtnAsk.Enabled = false;
            BtnAsk.Location = new Point(280, 60);
            BtnAsk.Name = "BtnAsk";
            BtnAsk.Size = new Size(100, 23);
            BtnAsk.TabIndex = 1;
            BtnAsk.Text = "Ask";
            BtnAsk.UseVisualStyleBackColor = true;
            BtnAsk.Click += BtnAsk_Click;

            // BtnViewTasks
            BtnViewTasks.Location = new Point(20, 100);
            BtnViewTasks.Name = "BtnViewTasks";
            BtnViewTasks.Size = new Size(360, 23);
            BtnViewTasks.TabIndex = 2;
            BtnViewTasks.Text = "View Tasks";
            BtnViewTasks.UseVisualStyleBackColor = true;
            BtnViewTasks.Click += BtnViewTasks_Click;

            // TxtName
            TxtName.Location = new Point(20, 20);
            TxtName.Name = "TxtName";
            TxtName.Size = new Size(250, 23);
            TxtName.TabIndex = 3;

            // TxtQuestion
            TxtQuestion.Enabled = false;
            TxtQuestion.Location = new Point(20, 60);
            TxtQuestion.Name = "TxtQuestion";
            TxtQuestion.Size = new Size(250, 23);
            TxtQuestion.TabIndex = 4;

            // RtbConversation
            RtbConversation.Location = new Point(20, 140);
            RtbConversation.Name = "RtbConversation";
            RtbConversation.ReadOnly = true;
            RtbConversation.Size = new Size(360, 200);
            RtbConversation.TabIndex = 5;
            RtbConversation.Text = "";

            // MainForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 360);
            Controls.Add(RtbConversation);
            Controls.Add(TxtQuestion);
            Controls.Add(TxtName);
            Controls.Add(BtnViewTasks);
            Controls.Add(BtnAsk);
            Controls.Add(BtnStart);
            Name = "MainForm";
            Text = "CyberBot Assistant";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnStart;
        private Button BtnAsk;
        private Button BtnViewTasks;
        private TextBox TxtName;
        private TextBox TxtQuestion;
        private RichTextBox RtbConversation;
    }
}