namespace Festival.Client
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // UI Controls
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblUsername
            this.lblUsername.Text = "Username:";
            this.lblUsername.Location = new System.Drawing.Point(30, 30);
            this.lblUsername.Size = new System.Drawing.Size(100, 20);

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(30, 50);
            this.txtUsername.Size = new System.Drawing.Size(200, 20);

            // lblPassword
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(30, 80);
            this.lblPassword.Size = new System.Drawing.Size(100, 20);

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(30, 100);
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.PasswordChar = '*';

            // btnLogin
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point(30, 140);
            this.btnLogin.Size = new System.Drawing.Size(200, 30);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // LoginForm properties
            this.ClientSize = new System.Drawing.Size(270, 210);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Festival Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}