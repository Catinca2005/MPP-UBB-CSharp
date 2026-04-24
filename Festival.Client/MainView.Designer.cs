namespace Festival.Client
{
    partial class MainView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView gridShows;
        private System.Windows.Forms.DateTimePicker dtpSearch;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtBuyerName;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Label lblBuyer;
        private System.Windows.Forms.Label lblQty;

        private void InitializeComponent()
        {
            this.gridShows = new System.Windows.Forms.DataGridView();
            this.dtpSearch = new System.Windows.Forms.DateTimePicker();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtBuyerName = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnBuy = new System.Windows.Forms.Button();
            this.lblBuyer = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridShows)).BeginInit();
            this.SuspendLayout();

            // gridShows
            this.gridShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridShows.Location = new System.Drawing.Point(20, 50);
            this.gridShows.Size = new System.Drawing.Size(640, 250);
            this.gridShows.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridShows_CellFormatting);

            // dtpSearch
            this.dtpSearch.Location = new System.Drawing.Point(20, 320);
            this.dtpSearch.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // btnFilter
            this.btnFilter.Location = new System.Drawing.Point(230, 320);
            this.btnFilter.Text = "Filter by Date";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);

            // Purchase section
            this.lblBuyer.Text = "Buyer Name:";
            this.lblBuyer.Location = new System.Drawing.Point(20, 370);

            this.txtBuyerName.Location = new System.Drawing.Point(120, 370);
            this.txtBuyerName.Size = new System.Drawing.Size(150, 20);

            this.lblQty.Text = "Quantity:";
            this.lblQty.Location = new System.Drawing.Point(20, 400);

            this.txtQuantity.Location = new System.Drawing.Point(120, 400);
            this.txtQuantity.Size = new System.Drawing.Size(50, 20);

            this.btnBuy.Location = new System.Drawing.Point(180, 400);
            this.btnBuy.Size = new System.Drawing.Size(100, 30);
            this.btnBuy.Text = "Buy Tickets";
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);

            // MainView Form
            this.ClientSize = new System.Drawing.Size(680, 480);
            this.Controls.Add(this.gridShows);
            this.Controls.Add(this.dtpSearch);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.lblBuyer);
            this.Controls.Add(this.txtBuyerName);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.btnBuy);
            this.Text = "Festival Management System";
            ((System.ComponentModel.ISupportInitialize)(this.gridShows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}