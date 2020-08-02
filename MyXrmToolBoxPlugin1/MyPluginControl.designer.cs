namespace SecurityRolesSync
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.syncRoles = new System.Windows.Forms.Button();
            this.Source = new System.Windows.Forms.ComboBox();
            this.Target = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearchSourceUser = new System.Windows.Forms.TextBox();
            this.txtSearchTargetUser = new System.Windows.Forms.TextBox();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(601, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // syncRoles
            // 
            this.syncRoles.Location = new System.Drawing.Point(220, 96);
            this.syncRoles.Margin = new System.Windows.Forms.Padding(2);
            this.syncRoles.Name = "syncRoles";
            this.syncRoles.Size = new System.Drawing.Size(141, 47);
            this.syncRoles.TabIndex = 8;
            this.syncRoles.Text = "Sync Security Roles";
            this.syncRoles.UseVisualStyleBackColor = true;
            this.syncRoles.Click += new System.EventHandler(this.syncRoles_Click);
            // 
            // Source
            // 
            this.Source.FormattingEnabled = true;
            this.Source.Location = new System.Drawing.Point(21, 68);
            this.Source.Margin = new System.Windows.Forms.Padding(2);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(187, 21);
            this.Source.TabIndex = 9;
            // 
            // Target
            // 
            this.Target.FormattingEnabled = true;
            this.Target.Location = new System.Drawing.Point(372, 68);
            this.Target.Margin = new System.Windows.Forms.Padding(2);
            this.Target.Name = "Target";
            this.Target.Size = new System.Drawing.Size(194, 21);
            this.Target.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Source User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Target User";
            // 
            // txtSearchSourceUser
            // 
            this.txtSearchSourceUser.Location = new System.Drawing.Point(108, 43);
            this.txtSearchSourceUser.Name = "txtSearchSourceUser";
            this.txtSearchSourceUser.Size = new System.Drawing.Size(100, 20);
            this.txtSearchSourceUser.TabIndex = 13;
            this.txtSearchSourceUser.Text = "Search...";
            this.txtSearchSourceUser.TextChanged += new System.EventHandler(this.txtSearchSourceUser_TextChanged);
            this.txtSearchSourceUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSearchSourceUser_MouseDown);
            // 
            // txtSearchTargetUser
            // 
            this.txtSearchTargetUser.Location = new System.Drawing.Point(466, 40);
            this.txtSearchTargetUser.Name = "txtSearchTargetUser";
            this.txtSearchTargetUser.Size = new System.Drawing.Size(100, 20);
            this.txtSearchTargetUser.TabIndex = 14;
            this.txtSearchTargetUser.Text = "Search...";
            this.txtSearchTargetUser.TextChanged += new System.EventHandler(this.txtServerTargetUser_TextChanged);
            this.txtSearchTargetUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtServerTargetUser_MouseDown);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSearchTargetUser);
            this.Controls.Add(this.txtSearchSourceUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Target);
            this.Controls.Add(this.Source);
            this.Controls.Add(this.syncRoles);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(601, 343);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.Button syncRoles;
        private System.Windows.Forms.ComboBox Source;
        private System.Windows.Forms.ComboBox Target;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearchSourceUser;
        private System.Windows.Forms.TextBox txtSearchTargetUser;
    }
}
