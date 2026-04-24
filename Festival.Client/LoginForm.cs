using System;
using System.Windows.Forms;
using Festival.Controller;
using Festival.Model;

namespace Festival.Client
{
    // Partial class allows the UI code to stay in the Designer file
    public partial class LoginForm : Form
    {
        private readonly LoginController _loginController;
        private readonly MainController _mainController;

        // Constructor receiving dependencies from Program.cs
        public LoginForm(LoginController loginController, MainController mainController)
        {
            InitializeComponent();
            _loginController = loginController;
            _mainController = mainController;
        }

        // Event handler for the Login button
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            try
            {
                // Delegates authentication logic to the Controller
                Employee authenticatedEmployee = _loginController.HandleLogin(username, password);

                if (authenticatedEmployee != null)
                {
                    MessageBox.Show($"Login successful! Welcome {authenticatedEmployee.Username}.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    this.Hide(); // Hide login window
    
                    // Create and show MainView, passing the controller
                    MainView mainView = new MainView(_mainController);
                    mainView.ShowDialog();
    
                    this.Close(); // Close the whole app when MainView closes
                }
            }
            catch (Exception ex)
            {
                // Display business logic errors (e.g., "Invalid credentials")
                MessageBox.Show(ex.Message, "Authentication Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}