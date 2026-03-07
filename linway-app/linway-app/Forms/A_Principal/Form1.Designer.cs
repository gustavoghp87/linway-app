
namespace linway_app.Forms
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verClientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirProductosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notasDeEnvioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crearNotaDeEnvíoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verNotasDeEnvíoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ventaDeProductosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verVentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hojaDeRepartoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verHojasDeRepartoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remitosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verRecibosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelBuscarPorDireccion = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonMostrarFiltroProductos = new System.Windows.Forms.Button();
            this.buttonMostrarFiltroClientes = new System.Windows.Forms.Button();
            this.BuscadorProductos = new System.Windows.Forms.TextBox();
            this.dataGridViewClientes = new System.Windows.Forms.DataGridView();
            this.labelBuscarPorNombreCliente = new System.Windows.Forms.Label();
            this.BuscadorClientes = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dataGridViewProductos = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductos)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientesToolStripMenuItem,
            this.productosToolStripMenuItem,
            this.notasDeEnvioToolStripMenuItem,
            this.ventaDeProductosToolStripMenuItem,
            this.hojaDeRepartoToolStripMenuItem,
            this.remitosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(925, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clientesToolStripMenuItem
            // 
            this.clientesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verClientesToolStripMenuItem});
            this.clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            this.clientesToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.clientesToolStripMenuItem.Text = "Clientes";
            // 
            // verClientesToolStripMenuItem
            // 
            this.verClientesToolStripMenuItem.Name = "verClientesToolStripMenuItem";
            this.verClientesToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.verClientesToolStripMenuItem.Text = "Abrir Clientes";
            this.verClientesToolStripMenuItem.Click += new System.EventHandler(this.AbrirClientes_ToolStripMenuItem_Click);
            // 
            // productosToolStripMenuItem
            // 
            this.productosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirProductosToolStripMenuItem});
            this.productosToolStripMenuItem.Name = "productosToolStripMenuItem";
            this.productosToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.productosToolStripMenuItem.Text = "Productos";
            // 
            // abrirProductosToolStripMenuItem
            // 
            this.abrirProductosToolStripMenuItem.Name = "abrirProductosToolStripMenuItem";
            this.abrirProductosToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.abrirProductosToolStripMenuItem.Text = "Abrir Productos";
            this.abrirProductosToolStripMenuItem.Click += new System.EventHandler(this.AbrirProductos_ToolStripMenuItem_Click);
            // 
            // notasDeEnvioToolStripMenuItem
            // 
            this.notasDeEnvioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearNotaDeEnvíoToolStripMenuItem,
            this.verNotasDeEnvíoToolStripMenuItem});
            this.notasDeEnvioToolStripMenuItem.Name = "notasDeEnvioToolStripMenuItem";
            this.notasDeEnvioToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.notasDeEnvioToolStripMenuItem.Text = "Notas de envío";
            // 
            // crearNotaDeEnvíoToolStripMenuItem
            // 
            this.crearNotaDeEnvíoToolStripMenuItem.Name = "crearNotaDeEnvíoToolStripMenuItem";
            this.crearNotaDeEnvíoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.crearNotaDeEnvíoToolStripMenuItem.Text = "Crear nota de envío";
            this.crearNotaDeEnvíoToolStripMenuItem.Click += new System.EventHandler(this.AbrirCrearNotaDeEnvío_ToolStripMenuItem_Click);
            // 
            // verNotasDeEnvíoToolStripMenuItem
            // 
            this.verNotasDeEnvíoToolStripMenuItem.Name = "verNotasDeEnvíoToolStripMenuItem";
            this.verNotasDeEnvíoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.verNotasDeEnvíoToolStripMenuItem.Text = "Ver notas de envío";
            this.verNotasDeEnvíoToolStripMenuItem.Click += new System.EventHandler(this.AbrirNotasDeEnvio_ToolStripMenuItem_Click);
            // 
            // ventaDeProductosToolStripMenuItem
            // 
            this.ventaDeProductosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verVentasToolStripMenuItem});
            this.ventaDeProductosToolStripMenuItem.Name = "ventaDeProductosToolStripMenuItem";
            this.ventaDeProductosToolStripMenuItem.Size = new System.Drawing.Size(121, 20);
            this.ventaDeProductosToolStripMenuItem.Text = "Venta de productos";
            // 
            // verVentasToolStripMenuItem
            // 
            this.verVentasToolStripMenuItem.Name = "verVentasToolStripMenuItem";
            this.verVentasToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.verVentasToolStripMenuItem.Text = "Ver ventas";
            this.verVentasToolStripMenuItem.Click += new System.EventHandler(this.AbrirVentas_ToolStripMenuItem_Click);
            // 
            // hojaDeRepartoToolStripMenuItem
            // 
            this.hojaDeRepartoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verHojasDeRepartoToolStripMenuItem});
            this.hojaDeRepartoToolStripMenuItem.Name = "hojaDeRepartoToolStripMenuItem";
            this.hojaDeRepartoToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.hojaDeRepartoToolStripMenuItem.Text = "Hoja de reparto";
            // 
            // verHojasDeRepartoToolStripMenuItem
            // 
            this.verHojasDeRepartoToolStripMenuItem.Name = "verHojasDeRepartoToolStripMenuItem";
            this.verHojasDeRepartoToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.verHojasDeRepartoToolStripMenuItem.Text = "Ver hojas de reparto";
            this.verHojasDeRepartoToolStripMenuItem.Click += new System.EventHandler(this.AbrirHojasDeReparto_ToolStripMenuItem_Click);
            // 
            // remitosToolStripMenuItem
            // 
            this.remitosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verRecibosToolStripMenuItem});
            this.remitosToolStripMenuItem.Name = "remitosToolStripMenuItem";
            this.remitosToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.remitosToolStripMenuItem.Text = "Recibos";
            // 
            // verRecibosToolStripMenuItem
            // 
            this.verRecibosToolStripMenuItem.Name = "verRecibosToolStripMenuItem";
            this.verRecibosToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.verRecibosToolStripMenuItem.Text = "Ver recibos";
            this.verRecibosToolStripMenuItem.Click += new System.EventHandler(this.AbrirRecibos_ToolStripMenuItem_Click);
            // 
            // label26
            // 
            this.labelBuscarPorDireccion.AutoSize = true;
            this.labelBuscarPorDireccion.Location = new System.Drawing.Point(139, 50);
            this.labelBuscarPorDireccion.Name = "label26";
            this.labelBuscarPorDireccion.Size = new System.Drawing.Size(96, 13);
            this.labelBuscarPorDireccion.TabIndex = 27;
            this.labelBuscarPorDireccion.Text = "Buscar por nombre";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 379);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 18);
            this.label10.TabIndex = 21;
            this.label10.Text = "Clientes";
            // 
            // button10
            // 
            this.buttonMostrarFiltroProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.buttonMostrarFiltroProductos.Location = new System.Drawing.Point(104, 45);
            this.buttonMostrarFiltroProductos.Name = "button10";
            this.buttonMostrarFiltroProductos.Size = new System.Drawing.Size(29, 23);
            this.buttonMostrarFiltroProductos.TabIndex = 26;
            this.buttonMostrarFiltroProductos.Text = "<<";
            this.buttonMostrarFiltroProductos.UseVisualStyleBackColor = true;
            this.buttonMostrarFiltroProductos.Click += new System.EventHandler(this.Button10_Click);
            // 
            // button5
            // 
            this.buttonMostrarFiltroClientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.buttonMostrarFiltroClientes.Location = new System.Drawing.Point(104, 377);
            this.buttonMostrarFiltroClientes.Name = "button5";
            this.buttonMostrarFiltroClientes.Size = new System.Drawing.Size(29, 23);
            this.buttonMostrarFiltroClientes.TabIndex = 22;
            this.buttonMostrarFiltroClientes.Text = "<<";
            this.buttonMostrarFiltroClientes.UseVisualStyleBackColor = true;
            this.buttonMostrarFiltroClientes.Click += new System.EventHandler(this.Button5_Click);
            // 
            // BuscadorProductos
            // 
            this.BuscadorProductos.Location = new System.Drawing.Point(241, 48);
            this.BuscadorProductos.Name = "BuscadorProductos";
            this.BuscadorProductos.Size = new System.Drawing.Size(114, 20);
            this.BuscadorProductos.TabIndex = 28;
            this.BuscadorProductos.TextChanged += new System.EventHandler(this.BuscadorProductos_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridViewClientes.AllowUserToAddRows = false;
            this.dataGridViewClientes.AllowUserToDeleteRows = false;
            this.dataGridViewClientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewClientes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewClientes.Location = new System.Drawing.Point(12, 411);
            this.dataGridViewClientes.Name = "dataGridView1";
            this.dataGridViewClientes.ReadOnly = true;
            this.dataGridViewClientes.RowHeadersWidth = 10;
            this.dataGridViewClientes.Size = new System.Drawing.Size(898, 281);
            this.dataGridViewClientes.TabIndex = 1;
            // 
            // label11
            // 
            this.labelBuscarPorNombreCliente.AutoSize = true;
            this.labelBuscarPorNombreCliente.Location = new System.Drawing.Point(139, 383);
            this.labelBuscarPorNombreCliente.Name = "label11";
            this.labelBuscarPorNombreCliente.Size = new System.Drawing.Size(107, 13);
            this.labelBuscarPorNombreCliente.TabIndex = 23;
            this.labelBuscarPorNombreCliente.Text = "Buscar por dirección:";
            // 
            // BuscadorClientes
            // 
            this.BuscadorClientes.Location = new System.Drawing.Point(252, 379);
            this.BuscadorClientes.Name = "BuscadorClientes";
            this.BuscadorClientes.Size = new System.Drawing.Size(110, 20);
            this.BuscadorClientes.TabIndex = 20;
            this.BuscadorClientes.TextChanged += new System.EventHandler(this.TextBox8_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(12, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 18);
            this.label12.TabIndex = 24;
            this.label12.Text = "Productos";
            // 
            // dataGridView2
            // 
            this.dataGridViewProductos.AllowUserToAddRows = false;
            this.dataGridViewProductos.AllowUserToDeleteRows = false;
            this.dataGridViewProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewProductos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewProductos.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewProductos.Location = new System.Drawing.Point(12, 82);
            this.dataGridViewProductos.Name = "dataGridView2";
            this.dataGridViewProductos.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewProductos.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewProductos.RowHeadersWidth = 10;
            this.dataGridViewProductos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewProductos.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewProductos.Size = new System.Drawing.Size(635, 272);
            this.dataGridViewProductos.TabIndex = 19;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(925, 699);
            this.Controls.Add(this.dataGridViewProductos);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.BuscadorClientes);
            this.Controls.Add(this.labelBuscarPorNombreCliente);
            this.Controls.Add(this.dataGridViewClientes);
            this.Controls.Add(this.BuscadorProductos);
            this.Controls.Add(this.buttonMostrarFiltroClientes);
            this.Controls.Add(this.buttonMostrarFiltroProductos);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.labelBuscarPorDireccion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema Linway 15 (2026)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verClientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notasDeEnvioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crearNotaDeEnvíoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verNotasDeEnvíoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ventaDeProductosToolStripMenuItem;
        private System.Windows.Forms.Label labelBuscarPorDireccion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonMostrarFiltroProductos;
        private System.Windows.Forms.Button buttonMostrarFiltroClientes;
        private System.Windows.Forms.TextBox BuscadorProductos;
        private System.Windows.Forms.DataGridView dataGridViewClientes;
        private System.Windows.Forms.Label labelBuscarPorNombreCliente;
        private System.Windows.Forms.TextBox BuscadorClientes;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dataGridViewProductos;
        private System.Windows.Forms.ToolStripMenuItem remitosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hojaDeRepartoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verHojasDeRepartoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verRecibosToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem verVentasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirProductosToolStripMenuItem;
    }
}
