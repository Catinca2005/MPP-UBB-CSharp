using System;
using System.Windows.Forms;
using Festival.Controller;
using Festival.Networking;
using Festival.Services;

namespace Festival.Client
{
    /// <summary>
    /// Entry point for the client application.
    /// </summary>
    static class StartClient
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // 1. Establish connection to the server via the Protobuf Proxy
                IFestivalServices serverProxy = new FestivalServerProtobufProxy("127.0.0.1", 55555);

                // 2. Initialize controllers, injecting the server proxy as the business logic provider
                MainController mainController = new MainController(serverProxy);
                LoginController loginController = new LoginController(serverProxy, mainController);

                // 3. Launch the initial authentication view
                Application.Run(new LoginForm(loginController, mainController));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application failed to start: {ex.Message}", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}