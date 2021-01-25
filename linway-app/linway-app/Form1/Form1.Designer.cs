
namespace linway_app
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verClientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarClienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarClienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarProductoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarProductoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarProductoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarProductosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notasDeEnvioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crearNotaDeEnvíoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verNotasDeEnvíoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ventaDeProductosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verVentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hojaDeRepartoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verHojasDeRepartoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remitosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verRecibosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label26 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.BuscadorProductos = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.BuscadorClientes = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.botonActualizar = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
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
                this.verClientesToolStripMenuItem,
                this.modificarClienteToolStripMenuItem,
                this.borrarClienteToolStripMenuItem,
                this.exToolStripMenuItem,
                this.importarToolStripMenuItem
            });
            this.clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            this.clientesToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.clientesToolStripMenuItem.Text = "Clientes";
            // 
            // verClientesToolStripMenuItem
            // 
            this.verClientesToolStripMenuItem.Name = "verClientesToolStripMenuItem";
            this.verClientesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.verClientesToolStripMenuItem.Text = "Agregar Cliente";
            this.verClientesToolStripMenuItem.Click += new System.EventHandler(this.verClientesToolStripMenuItem_Click);
            // 
            // modificarClienteToolStripMenuItem
            // 
            this.modificarClienteToolStripMenuItem.Name = "modificarClienteToolStripMenuItem";
            this.modificarClienteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.modificarClienteToolStripMenuItem.Text = "Modificar cliente";
            this.modificarClienteToolStripMenuItem.Click += new System.EventHandler(this.modificarClienteToolStripMenuItem_Click);
            // 
            // borrarClienteToolStripMenuItem
            // 
            this.borrarClienteToolStripMenuItem.Name = "borrarClienteToolStripMenuItem";
            this.borrarClienteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.borrarClienteToolStripMenuItem.Text = "Borrar cliente";
            this.borrarClienteToolStripMenuItem.Click += new System.EventHandler(this.borrarClienteToolStripMenuItem_Click);
            // 
            // exToolStripMenuItem
            // 
            this.exToolStripMenuItem.Name = "exToolStripMenuItem";
            this.exToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exToolStripMenuItem.Text = "Exportar clientes";
            this.exToolStripMenuItem.Click += new System.EventHandler(this.exToolStripMenuItem_Click);
            // 
            // importarToolStripMenuItem
            // 
            this.importarToolStripMenuItem.Enabled = true;
            this.importarToolStripMenuItem.Name = "importarToolStripMenuItem";
            this.importarToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.importarToolStripMenuItem.Text = "Importar";
            this.importarToolStripMenuItem.Click += new System.EventHandler(this.importarToolStripMenuItem_Click);



            // 
            // productosToolStripMenuItem
            // 
            this.productosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.agregarProductoToolStripMenuItem,
            this.modificarProductoToolStripMenuItem,
            this.borrarProductoToolStripMenuItem,
            this.exportarProductosToolStripMenuItem});
            this.productosToolStripMenuItem.Name = "productosToolStripMenuItem";
            this.productosToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.productosToolStripMenuItem.Text = "Productos";
            // 
            // agregarProductoToolStripMenuItem
            // 
            this.agregarProductoToolStripMenuItem.Name = "agregarProductoToolStripMenuItem";
            this.agregarProductoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.agregarProductoToolStripMenuItem.Text = "Agregar producto";
            this.agregarProductoToolStripMenuItem.Click += new System.EventHandler(this.agregarProductoToolStripMenuItem_Click);
            // 
            // modificarProductoToolStripMenuItem
            // 
            this.modificarProductoToolStripMenuItem.Name = "modificarProductoToolStripMenuItem";
            this.modificarProductoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.modificarProductoToolStripMenuItem.Text = "Modificar producto";
            this.modificarProductoToolStripMenuItem.Click += new System.EventHandler(this.modificarProductoToolStripMenuItem_Click);
            // 
            // borrarProductoToolStripMenuItem
            // 
            this.borrarProductoToolStripMenuItem.Name = "borrarProductoToolStripMenuItem";
            this.borrarProductoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.borrarProductoToolStripMenuItem.Text = "Borrar producto";
            this.borrarProductoToolStripMenuItem.Click += new System.EventHandler(this.borrarProductoToolStripMenuItem_Click);
            // 
            // exportarProductosToolStripMenuItem
            // 
            this.exportarProductosToolStripMenuItem.Name = "exportarProductosToolStripMenuItem";
            this.exportarProductosToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.exportarProductosToolStripMenuItem.Text = "Exportar Productos";
            this.exportarProductosToolStripMenuItem.Click += new System.EventHandler(this.exportarProductosToolStripMenuItem_Click);



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
            this.crearNotaDeEnvíoToolStripMenuItem.Click += new System.EventHandler(this.crearNotaDeEnvíoToolStripMenuItem_Click);
            // 
            // verNotasDeEnvíoToolStripMenuItem
            // 
            this.verNotasDeEnvíoToolStripMenuItem.Name = "verNotasDeEnvíoToolStripMenuItem";
            this.verNotasDeEnvíoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.verNotasDeEnvíoToolStripMenuItem.Text = "Ver notas de envío";
            this.verNotasDeEnvíoToolStripMenuItem.Click += new System.EventHandler(this.verNotasDeEnvíoToolStripMenuItem_Click);



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
            this.verVentasToolStripMenuItem.Click += new System.EventHandler(this.verVentasToolStripMenuItem_Click);



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
            this.verHojasDeRepartoToolStripMenuItem.Click += new System.EventHandler(this.verHojasDeRepartoToolStripMenuItem_Click);



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
            this.verRecibosToolStripMenuItem.Click += new System.EventHandler(this.verRecibosToolStripMenuItem_Click);



            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(12, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 18);
            this.label12.TabIndex = 24;
            this.label12.Text = "Productos";
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button10.Location = new System.Drawing.Point(104, 66);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(29, 23);
            this.button10.TabIndex = 26;
            this.button10.Text = ">>";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(139, 71);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(96, 13);
            this.label26.TabIndex = 27;
            this.label26.Text = "Buscar por nombre";
            this.label26.Visible = false;
            // 
            // BuscadorProductos
            // 
            this.BuscadorProductos.Location = new System.Drawing.Point(241, 66);
            this.BuscadorProductos.Name = "BuscadorProductos";
            this.BuscadorProductos.Size = new System.Drawing.Size(114, 20);
            this.BuscadorProductos.TabIndex = 28;
            this.BuscadorProductos.Visible = false;
            this.BuscadorProductos.TextChanged += new System.EventHandler(this.BuscadorProductos_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 416);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 10;
            this.dataGridView1.Size = new System.Drawing.Size(898, 150);
            this.dataGridView1.TabIndex = 1;



            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 388);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 18);
            this.label10.TabIndex = 21;
            this.label10.Text = "Clientes";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button5.Location = new System.Drawing.Point(104, 383);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(29, 23);
            this.button5.TabIndex = 22;
            this.button5.Text = ">>";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(139, 388);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Buscar por dirección:";
            this.label11.Visible = false;
            // 
            // BuscadorClientes
            // 
            this.BuscadorClientes.Location = new System.Drawing.Point(252, 383);
            this.BuscadorClientes.Name = "BuscadorClientes";
            this.BuscadorClientes.Size = new System.Drawing.Size(110, 20);
            this.BuscadorClientes.TabIndex = 20;
            this.BuscadorClientes.Visible = false;
            this.BuscadorClientes.TextChanged += new System.EventHandler(this.textBox8_TextChanged);            
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 96);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidth = 10;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView2.Size = new System.Drawing.Size(380, 281);
            this.dataGridView2.TabIndex = 19;



            // 
            // botonActualizar
            // 
            this.botonActualizar.Location = new System.Drawing.Point(799, 350);
            this.botonActualizar.Name = "botonActualizar";
            this.botonActualizar.Size = new System.Drawing.Size(114, 51);
            this.botonActualizar.TabIndex = 38;
            this.botonActualizar.Text = "Actualizar";
            this.botonActualizar.UseVisualStyleBackColor = true;
            this.botonActualizar.Click += new System.EventHandler(this.botonActualizar_Click);



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
            this.ClientSize = new System.Drawing.Size(925, 576);
            this.Controls.Add(this.botonActualizar);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.BuscadorClientes);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.BuscadorProductos);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label26);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ShowInTaskbar = true;
            this.ShowIcon = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema Linway 2021";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verClientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificarClienteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarProductoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificarProductoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notasDeEnvioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crearNotaDeEnvíoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verNotasDeEnvíoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ventaDeProductosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarProductosToolStripMenuItem;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox BuscadorProductos;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox BuscadorClientes;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button botonActualizar;
        private System.Windows.Forms.ToolStripMenuItem remitosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hojaDeRepartoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verHojasDeRepartoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem borrarProductoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem borrarClienteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verRecibosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem verVentasToolStripMenuItem;
    }
}
