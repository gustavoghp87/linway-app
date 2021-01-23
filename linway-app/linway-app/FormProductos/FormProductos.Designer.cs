namespace linway_app
{
    partial class FormProductos
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
            this.cbSeguroBorrar = new System.Windows.Forms.CheckBox();
            this.button22 = new System.Windows.Forms.Button();
            this.label46 = new System.Windows.Forms.Label();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.gbAgregar = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.gbModificar = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.bCopiaSeguridad = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.gbBorrar.SuspendLayout();
            this.gbAgregar.SuspendLayout();
            this.gbModificar.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBorrar
            // 
            this.gbBorrar.Controls.Add(this.cbSeguroBorrar);
            this.gbBorrar.Controls.Add(this.button22);
            this.gbBorrar.Controls.Add(this.label46);
            this.gbBorrar.Controls.Add(this.textBox21);
            this.gbBorrar.Controls.Add(this.label45);
            this.gbBorrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbBorrar.Location = new System.Drawing.Point(558, 12);
            this.gbBorrar.Name = "gbBorrar";
            this.gbBorrar.Size = new System.Drawing.Size(263, 148);
            this.gbBorrar.TabIndex = 41;
            this.gbBorrar.TabStop = false;
            this.gbBorrar.Text = "Borrar Producto";
            // 
            // cbSeguroBorrar
            // 
            this.cbSeguroBorrar.AutoSize = true;
            this.cbSeguroBorrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSeguroBorrar.Location = new System.Drawing.Point(9, 96);
            this.cbSeguroBorrar.Name = "cbSeguroBorrar";
            this.cbSeguroBorrar.Size = new System.Drawing.Size(238, 17);
            this.cbSeguroBorrar.TabIndex = 5;
            this.cbSeguroBorrar.Text = "Estoy seguro que quiero borrar este producto";
            this.cbSeguroBorrar.UseVisualStyleBackColor = true;
            // 
            // button22
            // 
            this.button22.Enabled = false;
            this.button22.Location = new System.Drawing.Point(169, 119);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(75, 23);
            this.button22.TabIndex = 4;
            this.button22.Text = "Aceptar";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(6, 59);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(0, 18);
            this.label46.TabIndex = 2;
            // 
            // textBox21
            // 
            this.textBox21.Location = new System.Drawing.Point(173, 31);
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new System.Drawing.Size(85, 24);
            this.textBox21.TabIndex = 1;
            this.textBox21.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumeros_KeyPress);
            this.textBox21.Leave += new System.EventHandler(this.textBox21_Leave);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(34, 35);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(147, 18);
            this.label45.TabIndex = 0;
            this.label45.Text = "Codigo de producto: ";
            // 
            // gbAgregar
            // 
            this.gbAgregar.Controls.Add(this.comboBox1);
            this.gbAgregar.Controls.Add(this.radioButton4);
            this.gbAgregar.Controls.Add(this.radioButton3);
            this.gbAgregar.Controls.Add(this.radioButton2);
            this.gbAgregar.Controls.Add(this.radioButton1);
            this.gbAgregar.Controls.Add(this.button4);
            this.gbAgregar.Controls.Add(this.button3);
            this.gbAgregar.Controls.Add(this.label8);
            this.gbAgregar.Controls.Add(this.textBox7);
            this.gbAgregar.Controls.Add(this.label9);
            this.gbAgregar.Controls.Add(this.textBox6);
            this.gbAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAgregar.Location = new System.Drawing.Point(12, 12);
            this.gbAgregar.Name = "gbAgregar";
            this.gbAgregar.Size = new System.Drawing.Size(268, 164);
            this.gbAgregar.TabIndex = 42;
            this.gbAgregar.TabStop = false;
            this.gbAgregar.Text = "Agregar Producto";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(157, 76);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(105, 23);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Enabled = false;
            this.radioButton4.Location = new System.Drawing.Point(96, 104);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(48, 19);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Otro";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.SeleccionarTipo_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Enabled = false;
            this.radioButton3.Location = new System.Drawing.Point(11, 105);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(78, 19);
            this.radioButton3.TabIndex = 9;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Unidades";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.SeleccionarTipo_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(96, 80);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(55, 19);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Polvo";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.SeleccionarTipo_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(12, 81);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(66, 19);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Liquido";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.SeleccionarTipo_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(128, 135);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Limpiar";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(Limpiar_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(47, 135);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Agregar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "Nombre";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(57, 52);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(94, 21);
            this.textBox7.TabIndex = 4;
            this.textBox7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloImporte_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 15);
            this.label9.TabIndex = 2;
            this.label9.Text = "Precio";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(57, 30);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(125, 21);
            this.textBox6.TabIndex = 3;
            // 
            // gbModificar
            // 
            this.gbModificar.Controls.Add(this.label19);
            this.gbModificar.Controls.Add(this.label14);
            this.gbModificar.Controls.Add(this.label18);
            this.gbModificar.Controls.Add(this.label13);
            this.gbModificar.Controls.Add(this.label17);
            this.gbModificar.Controls.Add(this.textBox9);
            this.gbModificar.Controls.Add(this.label16);
            this.gbModificar.Controls.Add(this.textBox8);
            this.gbModificar.Controls.Add(this.button6);
            this.gbModificar.Controls.Add(this.button7);
            this.gbModificar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbModificar.Location = new System.Drawing.Point(286, 12);
            this.gbModificar.Name = "gbModificar";
            this.gbModificar.Size = new System.Drawing.Size(266, 192);
            this.gbModificar.TabIndex = 43;
            this.gbModificar.TabStop = false;
            this.gbModificar.Text = "Modificar Producto";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(114, 84);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(0, 15);
            this.label19.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(24, 34);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 15);
            this.label14.TabIndex = 1;
            this.label14.Text = "Codigo de producto";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label18.Location = new System.Drawing.Point(92, 60);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(0, 15);
            this.label18.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(43, 117);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 15);
            this.label13.TabIndex = 2;
            this.label13.Text = "Nuevo Precio";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(24, 84);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(84, 15);
            this.label17.TabIndex = 8;
            this.label17.Text = "Precio actual: ";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(138, 114);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 21);
            this.textBox9.TabIndex = 4;
            this.textBox9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloImporte_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(24, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 15);
            this.label16.TabIndex = 7;
            this.label16.Text = "Producto: ";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(144, 34);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(94, 21);
            this.textBox8.TabIndex = 3;
            this.textBox8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumeros_KeyPress);
            this.textBox8.Leave += new System.EventHandler(this.textBox8_Leave);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(154, 147);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "Limpiar";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Limpiar_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(63, 147);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 5;
            this.button7.Text = "Modificar";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // bCopiaSeguridad
            // 
            this.bCopiaSeguridad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bCopiaSeguridad.Location = new System.Drawing.Point(12, 182);
            this.bCopiaSeguridad.Name = "bCopiaSeguridad";
            this.bCopiaSeguridad.Size = new System.Drawing.Size(191, 27);
            this.bCopiaSeguridad.TabIndex = 44;
            this.bCopiaSeguridad.Text = "Crear copia de seguridad";
            this.bCopiaSeguridad.UseVisualStyleBackColor = true;
            this.bCopiaSeguridad.Click += new System.EventHandler(this.CrearCopiaSeguridad_Click);
            // 
            // bExit
            // 
            this.bExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExit.Location = new System.Drawing.Point(654, 182);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(81, 27);
            this.bExit.TabIndex = 45;
            this.bExit.Text = "Salir";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bSalir_Click);
            // 
            // FormProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 216);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bCopiaSeguridad);
            this.Controls.Add(this.gbModificar);
            this.Controls.Add(this.gbAgregar);
            this.Controls.Add(this.gbBorrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProductos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Productos";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormProductos_Load);
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
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox gbAgregar;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.GroupBox gbModificar;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox cbSeguroBorrar;
        private System.Windows.Forms.Button bCopiaSeguridad;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
