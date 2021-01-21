namespace WindowsFormsApplication1
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(280, 56);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(82, 20);
            this.textBox20.TabIndex = 47;
            this.textBox20.Visible = false;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(258, 296);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(148, 17);
            this.checkBox4.TabIndex = 46;
            this.checkBox4.Text = "Enviar a listado de ventas";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(58, 295);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(139, 17);
            this.checkBox3.TabIndex = 45;
            this.checkBox3.Text = "Enviar a hoja de reparto";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(224, 320);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(48, 13);
            this.label33.TabIndex = 45;
            this.label33.Text = "Reparto:";
            this.label33.Visible = false;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(86, 320);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(26, 13);
            this.label34.TabIndex = 44;
            this.label34.Text = "Dia:";
            this.label34.Visible = false;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(281, 317);
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
            "Miercoles",
            "Jueves",
            "Viernes"});
            this.comboBox4.Location = new System.Drawing.Point(119, 317);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(99, 21);
            this.comboBox4.TabIndex = 42;
            this.comboBox4.Visible = false;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(193, 273);
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
            this.label35.Location = new System.Drawing.Point(35, 24);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(129, 15);
            this.label35.TabIndex = 1;
            this.label35.Text = "Numero de cliente:";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(156, 85);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(49, 13);
            this.label43.TabIndex = 17;
            this.label43.Text = "Subtotal:";
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(353, 269);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(88, 23);
            this.button16.TabIndex = 16;
            this.button16.Text = "Cancelar";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(166, 24);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(45, 20);
            this.textBox15.TabIndex = 2;
            this.textBox15.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.soloNumero_KeyPress);
            this.textBox15.Leave += new System.EventHandler(this.textBox15_Leave);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(259, 269);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(88, 23);
            this.button15.TabIndex = 15;
            this.button15.Text = "Confirmar";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(217, 28);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(0, 13);
            this.label36.TabIndex = 3;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(127, 272);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(19, 20);
            this.label42.TabIndex = 14;
            this.label42.Text = "0";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(37, 57);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(121, 15);
            this.label37.TabIndex = 4;
            this.label37.Text = "Agregar producto:";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(54, 272);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(54, 20);
            this.label41.TabIndex = 13;
            this.label41.Text = "Total:";
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(166, 53);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(45, 20);
            this.textBox16.TabIndex = 5;
            this.textBox16.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.soloNumero_KeyPress);
            this.textBox16.Leave += new System.EventHandler(this.textBox16_Leave);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(361, 90);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(73, 23);
            this.button14.TabIndex = 12;
            this.button14.Text = "Limpiar lista";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(217, 57);
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
            this.dataGridView4.Location = new System.Drawing.Point(38, 124);
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
            this.label39.Location = new System.Drawing.Point(40, 86);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(68, 15);
            this.label39.TabIndex = 7;
            this.label39.Text = "Cantidad:";
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(280, 90);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 10;
            this.button13.Text = "Añadir a lista";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(108, 81);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(41, 20);
            this.textBox17.TabIndex = 8;
            this.textBox17.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.soloNumero_KeyPress);
            this.textBox17.Leave += new System.EventHandler(this.textBox17_Leave);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(211, 86);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(0, 13);
            this.label40.TabIndex = 9;
            // 
            // FormCrearNota
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 353);
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
    }
}