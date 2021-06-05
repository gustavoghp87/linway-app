
namespace linway_app.Forms
{
    partial class FormClientes
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
            this.gbBorrar = new System.Windows.Forms.GroupBox();
            this.textBoxDireEnBorrar = new System.Windows.Forms.TextBox();
            this.BusqPorDireLabel = new System.Windows.Forms.Label();
            this.cbSeguroBorrar = new System.Windows.Forms.CheckBox();
            this.button23 = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.gbAgregar = new System.Windows.Forms.GroupBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.gbModificar = new System.Windows.Forms.GroupBox();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.textBox24 = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gbBorrar.SuspendLayout();
            this.gbAgregar.SuspendLayout();
            this.gbModificar.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBorrar
            // 
            this.gbBorrar.Controls.Add(this.textBoxDireEnBorrar);
            this.gbBorrar.Controls.Add(this.BusqPorDireLabel);
            this.gbBorrar.Controls.Add(this.cbSeguroBorrar);
            this.gbBorrar.Controls.Add(this.button23);
            this.gbBorrar.Controls.Add(this.label47);
            this.gbBorrar.Controls.Add(this.textBox22);
            this.gbBorrar.Controls.Add(this.label48);
            this.gbBorrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbBorrar.Location = new System.Drawing.Point(592, 12);
            this.gbBorrar.Name = "gbBorrar";
            this.gbBorrar.Size = new System.Drawing.Size(277, 290);
            this.gbBorrar.TabIndex = 42;
            this.gbBorrar.TabStop = false;
            this.gbBorrar.Text = "Borrar Cliente";
            // 
            // textBoxDireEnBorrar
            // 
            this.textBoxDireEnBorrar.Location = new System.Drawing.Point(159, 63);
            this.textBoxDireEnBorrar.Name = "textBoxDireEnBorrar";
            this.textBoxDireEnBorrar.Size = new System.Drawing.Size(100, 24);
            this.textBoxDireEnBorrar.TabIndex = 7;
            this.textBoxDireEnBorrar.TextChanged += new System.EventHandler(this.BorrarPorDire_textBox_TextChanged);
            // 
            // BusqPorDireLabel
            // 
            this.BusqPorDireLabel.AutoSize = true;
            this.BusqPorDireLabel.Location = new System.Drawing.Point(9, 66);
            this.BusqPorDireLabel.Name = "BusqPorDireLabel";
            this.BusqPorDireLabel.Size = new System.Drawing.Size(71, 18);
            this.BusqPorDireLabel.TabIndex = 6;
            this.BusqPorDireLabel.Text = "Dirección";
            this.BusqPorDireLabel.Click += new System.EventHandler(this.label6_Click);
            // 
            // cbSeguroBorrar
            // 
            this.cbSeguroBorrar.AutoSize = true;
            this.cbSeguroBorrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSeguroBorrar.Location = new System.Drawing.Point(12, 176);
            this.cbSeguroBorrar.Name = "cbSeguroBorrar";
            this.cbSeguroBorrar.Size = new System.Drawing.Size(259, 19);
            this.cbSeguroBorrar.TabIndex = 5;
            this.cbSeguroBorrar.Text = "Estoy seguro que quiero borrar este cliente";
            this.cbSeguroBorrar.UseVisualStyleBackColor = true;
            // 
            // button23
            // 
            this.button23.Enabled = false;
            this.button23.Location = new System.Drawing.Point(107, 238);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(75, 29);
            this.button23.TabIndex = 4;
            this.button23.Text = "Borrar";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.EliminarCliente_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(8, 105);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(0, 18);
            this.label47.TabIndex = 2;
            // 
            // textBox22
            // 
            this.textBox22.Location = new System.Drawing.Point(159, 30);
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new System.Drawing.Size(100, 24);
            this.textBox22.TabIndex = 1;
            this.textBox22.TextChanged += new System.EventHandler(this.BorrarPorId_textBox_TextChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(6, 29);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(122, 18);
            this.label48.TabIndex = 0;
            this.label48.Text = "Codigo de cliente";
            // 
            // gbAgregar
            // 
            this.gbAgregar.Controls.Add(this.label44);
            this.gbAgregar.Controls.Add(this.label1);
            this.gbAgregar.Controls.Add(this.textBox18);
            this.gbAgregar.Controls.Add(this.textBox1);
            this.gbAgregar.Controls.Add(this.button2);
            this.gbAgregar.Controls.Add(this.textBox2);
            this.gbAgregar.Controls.Add(this.radioButton2);
            this.gbAgregar.Controls.Add(this.textBox3);
            this.gbAgregar.Controls.Add(this.radioButton1);
            this.gbAgregar.Controls.Add(this.textBox4);
            this.gbAgregar.Controls.Add(this.label2);
            this.gbAgregar.Controls.Add(this.textBox5);
            this.gbAgregar.Controls.Add(this.label3);
            this.gbAgregar.Controls.Add(this.button1);
            this.gbAgregar.Controls.Add(this.label4);
            this.gbAgregar.Controls.Add(this.label5);
            this.gbAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAgregar.Location = new System.Drawing.Point(12, 12);
            this.gbAgregar.Name = "gbAgregar";
            this.gbAgregar.Size = new System.Drawing.Size(270, 290);
            this.gbAgregar.TabIndex = 43;
            this.gbAgregar.TabStop = false;
            this.gbAgregar.Text = "Agregar nuevo cliente";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(36, 79);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(61, 15);
            this.label44.TabIndex = 1;
            this.label44.Text = "Localidad";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nombre";
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(102, 76);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(131, 21);
            this.textBox18.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(102, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 21);
            this.textBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(148, 238);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Limpiar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.limpiar_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(102, 50);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(131, 21);
            this.textBox2.TabIndex = 3;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(59, 194);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(83, 19);
            this.radioButton2.TabIndex = 19;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "R.Inscripto";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(102, 102);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(131, 21);
            this.textBox3.TabIndex = 5;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(148, 194);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(103, 19);
            this.radioButton1.TabIndex = 18;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "R.Monotributo";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(102, 128);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(131, 21);
            this.textBox4.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Dirección";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(102, 154);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(131, 21);
            this.textBox5.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "CUIT";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(59, 235);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 26);
            this.button1.TabIndex = 16;
            this.button1.Text = "Agregar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AgregarCliente_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Codigo Postal";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Telefono";
            // 
            // gbModificar
            // 
            this.gbModificar.Controls.Add(this.label6);
            this.gbModificar.Controls.Add(this.textBox6);
            this.gbModificar.Controls.Add(this.textBox25);
            this.gbModificar.Controls.Add(this.label50);
            this.gbModificar.Controls.Add(this.textBox24);
            this.gbModificar.Controls.Add(this.label49);
            this.gbModificar.Controls.Add(this.textBox23);
            this.gbModificar.Controls.Add(this.button8);
            this.gbModificar.Controls.Add(this.label25);
            this.gbModificar.Controls.Add(this.radioButton3);
            this.gbModificar.Controls.Add(this.textBox14);
            this.gbModificar.Controls.Add(this.radioButton4);
            this.gbModificar.Controls.Add(this.textBox11);
            this.gbModificar.Controls.Add(this.label24);
            this.gbModificar.Controls.Add(this.textBox10);
            this.gbModificar.Controls.Add(this.label23);
            this.gbModificar.Controls.Add(this.button9);
            this.gbModificar.Controls.Add(this.label22);
            this.gbModificar.Controls.Add(this.label21);
            this.gbModificar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbModificar.Location = new System.Drawing.Point(288, 12);
            this.gbModificar.Name = "gbModificar";
            this.gbModificar.Size = new System.Drawing.Size(298, 290);
            this.gbModificar.TabIndex = 44;
            this.gbModificar.TabStop = false;
            this.gbModificar.Text = "Modificar Cliente";
            // 
            // textBox25
            // 
            this.textBox25.Location = new System.Drawing.Point(69, 218);
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new System.Drawing.Size(91, 21);
            this.textBox25.TabIndex = 7;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(37, 221);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(26, 15);
            this.label50.TabIndex = 23;
            this.label50.Text = "CP:";
            // 
            // textBox24
            // 
            this.textBox24.Location = new System.Drawing.Point(81, 128);
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new System.Drawing.Size(198, 21);
            this.textBox24.TabIndex = 4;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(11, 131);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(58, 15);
            this.label49.TabIndex = 22;
            this.label49.Text = "Telefono:";
            // 
            // textBox23
            // 
            this.textBox23.Location = new System.Drawing.Point(81, 100);
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new System.Drawing.Size(198, 21);
            this.textBox23.TabIndex = 3;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(165, 257);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 20;
            this.button8.Text = "Cancelar";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.limpiar_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(11, 27);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(105, 15);
            this.label25.TabIndex = 7;
            this.label25.Text = "Código de cliente:";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(176, 188);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(83, 19);
            this.radioButton3.TabIndex = 19;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "R.Inscripto";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(134, 24);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(131, 21);
            this.textBox14.TabIndex = 2;
            this.textBox14.TextChanged += new System.EventHandler(this.textBox14_TextChanged);
            this.textBox14.Leave += new System.EventHandler(this.PrepararEditar_Leave);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(176, 217);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(103, 19);
            this.radioButton4.TabIndex = 18;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "R.Monotributo";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(81, 156);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(198, 21);
            this.textBox11.TabIndex = 5;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(8, 103);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(62, 15);
            this.label24.TabIndex = 8;
            this.label24.Text = "Dirección:";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(69, 188);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(91, 21);
            this.textBox10.TabIndex = 6;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label23.Location = new System.Drawing.Point(39, 76);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(0, 15);
            this.label23.TabIndex = 9;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(39, 257);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(101, 23);
            this.button9.TabIndex = 16;
            this.button9.Text = "Modificar";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Editar_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(15, 157);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(55, 15);
            this.label22.TabIndex = 10;
            this.label22.Text = "Nombre:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(26, 191);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(37, 15);
            this.label21.TabIndex = 11;
            this.label21.Text = "CUIT:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(134, 49);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(131, 21);
            this.textBox6.TabIndex = 24;
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 15);
            this.label6.TabIndex = 25;
            this.label6.Text = "Dirección:";
            // 
            // FormClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 314);
            this.Controls.Add(this.gbBorrar);
            this.Controls.Add(this.gbModificar);
            this.Controls.Add(this.gbAgregar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormClientes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clientes";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormClientes_Load);
            this.gbBorrar.ResumeLayout(false);
            this.gbBorrar.PerformLayout();
            this.gbAgregar.ResumeLayout(false);
            this.gbAgregar.PerformLayout();
            this.gbModificar.ResumeLayout(false);
            this.gbModificar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBorrar;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox textBox22;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.GroupBox gbAgregar;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbModificar;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox textBox24;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox cbSeguroBorrar;
        private System.Windows.Forms.Label BusqPorDireLabel;
        private System.Windows.Forms.TextBox textBoxDireEnBorrar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
    }
}
