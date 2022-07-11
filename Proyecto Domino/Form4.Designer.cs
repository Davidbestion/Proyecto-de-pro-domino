namespace Proyecto_Domino
{
    partial class Jugar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SiguienteJugada = new System.Windows.Forms.Button();
            this.JuegoAtomatico = new System.Windows.Forms.Button();
            this.MostrarFinal = new System.Windows.Forms.Button();
            this.Atras = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Resetear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SiguienteJugada
            // 
            this.SiguienteJugada.Location = new System.Drawing.Point(48, 45);
            this.SiguienteJugada.Name = "SiguienteJugada";
            this.SiguienteJugada.Size = new System.Drawing.Size(117, 23);
            this.SiguienteJugada.TabIndex = 0;
            this.SiguienteJugada.Text = "Siguiente Jugada";
            this.SiguienteJugada.UseVisualStyleBackColor = true;
            this.SiguienteJugada.Click += new System.EventHandler(this.SiguienteJugada_Click);
            // 
            // JuegoAtomatico
            // 
            this.JuegoAtomatico.Location = new System.Drawing.Point(51, 98);
            this.JuegoAtomatico.Name = "JuegoAtomatico";
            this.JuegoAtomatico.Size = new System.Drawing.Size(114, 23);
            this.JuegoAtomatico.TabIndex = 1;
            this.JuegoAtomatico.Text = "Juego Automatico";
            this.JuegoAtomatico.UseVisualStyleBackColor = true;
            this.JuegoAtomatico.Click += new System.EventHandler(this.JuegoAtomatico_Click);
            // 
            // MostrarFinal
            // 
            this.MostrarFinal.Location = new System.Drawing.Point(51, 146);
            this.MostrarFinal.Name = "MostrarFinal";
            this.MostrarFinal.Size = new System.Drawing.Size(114, 23);
            this.MostrarFinal.TabIndex = 2;
            this.MostrarFinal.Text = "Mostrar Final";
            this.MostrarFinal.UseVisualStyleBackColor = true;
            this.MostrarFinal.Click += new System.EventHandler(this.MostrarFinal_Click);
            // 
            // Atras
            // 
            this.Atras.Location = new System.Drawing.Point(51, 372);
            this.Atras.Name = "Atras";
            this.Atras.Size = new System.Drawing.Size(75, 23);
            this.Atras.TabIndex = 3;
            this.Atras.Text = "Atras";
            this.Atras.UseVisualStyleBackColor = true;
            this.Atras.Click += new System.EventHandler(this.Atras_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(213, 42);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(532, 289);
            this.listBox1.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Resetear
            // 
            this.Resetear.Location = new System.Drawing.Point(51, 201);
            this.Resetear.Name = "Resetear";
            this.Resetear.Size = new System.Drawing.Size(115, 23);
            this.Resetear.TabIndex = 5;
            this.Resetear.Text = "Resetear";
            this.Resetear.UseVisualStyleBackColor = true;
            this.Resetear.Click += new System.EventHandler(this.Resetear_Click);
            // 
            // Jugar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Resetear);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Atras);
            this.Controls.Add(this.MostrarFinal);
            this.Controls.Add(this.JuegoAtomatico);
            this.Controls.Add(this.SiguienteJugada);
            this.MaximizeBox = false;
            this.Name = "Jugar";
            this.Text = "Jugar";
            this.ResumeLayout(false);

        }

        #endregion

        private Button SiguienteJugada;
        private Button JuegoAtomatico;
        private Button MostrarFinal;
        private Button Atras;
        private ListBox listBox1;
        private System.Windows.Forms.Timer timer1;
        private Button Resetear;
    }
}