
namespace linway_app.Forms
{
    partial class FormCrearNota
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
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.button16 = new System.Windows.Forms.Button();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.button14 = new System.Windows.Forms.Button();
            this.label38 = new System.Windows.Forms.Label();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.label39 = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.labelClienteId = new System.Windows.Forms.Label();
            this.labelProductoId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(296, 184);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(82, 20);
            this.textBox20.TabIndex = 47;
            this.textBox20.Visible = false;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(630, 260);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(148, 17);
            this.checkBox4.TabIndex = 46;
            this.checkBox4.Text = "Enviar a listado de ventas";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(436, 198);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(139, 17);
            this.checkBox3.TabIndex = 45;
            this.checkBox3.Text = "Enviar a hoja de reparto";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(411, 285);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(48, 13);
            this.label33.TabIndex = 45;
            this.label33.Text = "Reparto:";
            this.label33.Visible = false;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(433, 246);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(26, 13);
            this.label34.TabIndex = 44;
            this.label34.Text = "Dia:";
            this.label34.Visible = false;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(476, 282);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(99, 21);
            this.comboBox3.TabIndex = 43;
            this.comboBox3.Visible = false;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "Lunes",
            "Martes",
            "Miércoles",
            "Jueves",
            "Viernes",
            "Sábado"});
            this.comboBox4.Location = new System.Drawing.Point(476, 243);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(99, 21);
            this.comboBox4.TabIndex = 42;
            this.comboBox4.Visible = false;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.EnviarA_HDR_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(630, 216);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(61, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Imprimir";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(35, 41);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(103, 15);
            this.label35.TabIndex = 1;
            this.label35.Text = "Código cliente:";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(189, 263);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(49, 13);
            this.label43.TabIndex = 17;
            this.label43.Text = "Subtotal:";
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(773, 338);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(88, 23);
            this.button16.TabIndex = 16;
            this.button16.Text = "Cancelar";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.CerrarBtn_Click);
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(166, 40);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(45, 20);
            this.textBox15.TabIndex = 2;
            this.textBox15.TextChanged += new System.EventHandler(this.textBox15_TextChanged);
            this.textBox15.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumero_KeyPress);
            this.textBox15.Leave += new System.EventHandler(this.CargarCliente_Leave);
            // 
            // button15
            // 
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.Location = new System.Drawing.Point(653, 320);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(105, 41);
            this.button15.TabIndex = 15;
            this.button15.Text = "Confirmar";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.ConfirmarCrearNota_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(37, 105);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(0, 13);
            this.label36.TabIndex = 3;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(807, 133);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(19, 20);
            this.label42.TabIndex = 14;
            this.label42.Text = "0";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(35, 145);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(116, 15);
            this.label37.TabIndex = 4;
            this.label37.Text = "Código producto:";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(813, 108);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(49, 20);
            this.label41.TabIndex = 13;
            this.label41.Text = "Total";
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(166, 144);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(45, 20);
            this.textBox16.TabIndex = 5;
            this.textBox16.TextChanged += new System.EventHandler(this.textBox16_TextChanged);
            this.textBox16.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumero_KeyPress);
            this.textBox16.Leave += new System.EventHandler(this.CargarProducto_Leave);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(802, 28);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(73, 23);
            this.button14.TabIndex = 12;
            this.button14.Text = "Limpiar lista";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.LimpiarLista_Click);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(37, 216);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(0, 13);
            this.label38.TabIndex = 6;
            // 
            // dataGridView4
            // 
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(392, 28);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.ReadOnly = true;
            this.dataGridView4.RowHeadersWidth = 10;
            this.dataGridView4.Size = new System.Drawing.Size(395, 125);
            this.dataGridView4.TabIndex = 11;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(35, 263);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(68, 15);
            this.label39.TabIndex = 7;
            this.label39.Text = "Cantidad:";
            // 
            // button13
            // 
            this.button13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(142, 320);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(127, 36);
            this.button13.TabIndex = 10;
            this.button13.Text = "Añadir a lista";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.NuevoProdVendidos_btn13_Click);
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(116, 260);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(41, 20);
            this.textBox17.TabIndex = 8;
            this.textBox17.TextChanged += new System.EventHandler(this.textBox17_TextChanged);
            this.textBox17.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumero_KeyPress);
            this.textBox17.Leave += new System.EventHandler(this.CargarSubtotal_Leave);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(237, 263);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(0, 13);
            this.label40.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 48;
            this.label1.Text = "Dirección:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(166, 73);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(115, 20);
            this.textBox1.TabIndex = 49;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 15);
            this.label2.TabIndex = 50;
            this.label2.Text = "Nombre producto:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(166, 184);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(115, 20);
            this.textBox2.TabIndex = 51;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // labelClienteId
            // 
            this.labelClienteId.AutoSize = true;
            this.labelClienteId.Location = new System.Drawing.Point(245, 46);
            this.labelClienteId.Name = "labelClienteId";
            this.labelClienteId.Size = new System.Drawing.Size(0, 13);
            this.labelClienteId.TabIndex = 52;
            // 
            // labelProductoId
            // 
            this.labelProductoId.AutoSize = true;
            this.labelProductoId.Location = new System.Drawing.Point(245, 147);
            this.labelProductoId.Name = "labelProductoId";
            this.labelProductoId.Size = new System.Drawing.Size(0, 13);
            this.labelProductoId.TabIndex = 53;
            // 
            // FormCrearNota
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 370);
            this.Controls.Add(this.labelProductoId);
            this.Controls.Add(this.labelClienteId);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.dataGridView4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.label36);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "FormCrearNota";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crear nota de envio";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormCrearNota_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label labelClienteId;
        private System.Windows.Forms.Label labelProductoId;
    }
}
