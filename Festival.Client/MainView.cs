using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Festival.Controller;
using Festival.Model;

namespace Festival.Client
{
    public partial class MainView : Form
    {
        private readonly MainController _controller;
        private BindingSource _showsBindingSource = new BindingSource();

        public MainView(MainController controller)
        {
            InitializeComponent();
            _controller = controller;
            _controller.OnTicketSold += OnTicketSoldHandler;
            LoadAllShows();
        }

        private void LoadAllShows()
        {
            try
            {
                // Fetch data from the controller
                var shows = _controller.HandleGetAllShows();
                UpdateTable(shows);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shows: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTable(IEnumerable<Show> shows)
        {
            _showsBindingSource.DataSource = shows.ToList();
            gridShows.DataSource = _showsBindingSource;
        }

        // Requirement 2: Filter by Date
        private void btnFilter_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = dtpSearch.Value.Date;
            var filteredShows = _controller.HandleSearch(selectedDate);
            UpdateTable(filteredShows);
        }

        // Requirement 3: Buy Ticket logic
        private void btnBuy_Click(object sender, EventArgs e)
        {
            if (gridShows.CurrentRow == null)
            {
                MessageBox.Show("Please select a show first.");
                return;
            }

            try
            {
                // Get the selected show from the grid
                Show selectedShow = (Show)gridShows.CurrentRow.DataBoundItem;
                string buyer = txtBuyerName.Text;
                string quantity = txtQuantity.Text;

                _controller.HandleBuyTicket(selectedShow.Id, buyer, quantity);
                
                MessageBox.Show("Purchase successful!");
                
                txtBuyerName.Clear();
                txtQuantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchase Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Requirement 3: Color rows RED if sold out
        private void gridShows_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridShows.Rows[e.RowIndex].DataBoundItem is Show show)
            {
                if (show.AvailableSeats == 0)
                {
                    gridShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    gridShows.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    gridShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    gridShows.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close(); // Return to Login or close app
        }
        
        /// <summary>
        /// Event handler triggered when the server notifies about a ticket sale.
        /// Ensures UI updates occur on the main thread safely to prevent cross-thread exceptions.
        /// </summary>
        private void OnTicketSoldHandler(object sender, Show updatedShow)
        {
            // Check if the current thread is a background network thread
            if (this.InvokeRequired)
            {
                // Send the execution back to the Main UI Thread
                this.BeginInvoke(new Action(() => OnTicketSoldHandler(sender, updatedShow)));
                return;
            }

            // Once on the Main Thread, refresh the table with the latest data
            LoadAllShows();
        }
    }
}