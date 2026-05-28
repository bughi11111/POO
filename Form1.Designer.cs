partial class Form1
{
    private System.ComponentModel.IContainer components = null;

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
        richAfisare = new RichTextBox();
        pnlDecisions = new FlowLayoutPanel();
        btnLoadStory = new Button();
        SuspendLayout();
        // 
        // richAfisare
        // 
        richAfisare.Location = new Point(12, 12);
        richAfisare.Name = "richAfisare";
        richAfisare.ReadOnly = true;
        richAfisare.Size = new Size(407, 77);
        richAfisare.TabIndex = 1;
        richAfisare.Text = "";
        // 
        // pnlDecisions
        // 
        pnlDecisions.AutoScroll = true;
        pnlDecisions.Location = new Point(93, 116);
        pnlDecisions.Name = "pnlDecisions";
        pnlDecisions.Size = new Size(264, 73);
        pnlDecisions.TabIndex = 2;
        // 
        // btnLoadStory
        // 
        btnLoadStory.Location = new Point(148, 260);
        btnLoadStory.Name = "btnLoadStory";
        btnLoadStory.Size = new Size(168, 49);
        btnLoadStory.TabIndex = 3;
        btnLoadStory.Text = "Încarcă Poveste";
        btnLoadStory.UseVisualStyleBackColor = true;
        btnLoadStory.Click += btnLoadStory_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(609, 450);
        Controls.Add(btnLoadStory);
        Controls.Add(pnlDecisions);
        Controls.Add(richAfisare);
        Name = "Form1";
        Text = "Story Reader";
        ResumeLayout(false);
    }

    #endregion

    private RichTextBox richAfisare; // Pentru afișarea textului [cite: 75, 95]
    private FlowLayoutPanel pnlDecisions; // Pentru butoanele de decizie generate dinamic [cite: 105, 111]
    private Button btnLoadStory; // Butonul principal de încărcare [cite: 98]
}
