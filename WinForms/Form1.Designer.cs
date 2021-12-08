
namespace WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnIniciar = new System.Windows.Forms.Button();
            this.loadingGif = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingGif)).BeginInit();
            this.SuspendLayout();
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(88, 169);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(182, 43);
            this.btnIniciar.TabIndex = 0;
            this.btnIniciar.Text = "Iniciar Proceso";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // loadingGif
            // 
            this.loadingGif.Image = ((System.Drawing.Image)(resources.GetObject("loadingGif.Image")));
            this.loadingGif.Location = new System.Drawing.Point(99, 245);
            this.loadingGif.Name = "loadingGif";
            this.loadingGif.Size = new System.Drawing.Size(211, 181);
            this.loadingGif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.loadingGif.TabIndex = 1;
            this.loadingGif.TabStop = false;
            this.loadingGif.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nombre:";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(164, 67);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(167, 26);
            this.txtInput.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loadingGif);
            this.Controls.Add(this.btnIniciar);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.loadingGif)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.PictureBox loadingGif;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInput;
    }
}

