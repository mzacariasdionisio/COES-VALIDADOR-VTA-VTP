using System;

namespace COES.WindowsFormsApp.ExtraccionSeniales
{
    partial class FormExtraccionSeniales
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
            this.dateTimePickerDesde = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerHasta = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonIniciarExtraccionSenales = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBoxExtraccionSenalDetalle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dateTimePickerDesde
            // 
            this.dateTimePickerDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDesde.Location = new System.Drawing.Point(56, 12);
            this.dateTimePickerDesde.Name = "dateTimePickerDesde";
            this.dateTimePickerDesde.Size = new System.Drawing.Size(98, 20);
            this.dateTimePickerDesde.TabIndex = 0;
            this.dateTimePickerDesde.Value = new System.DateTime(2023, 7, 12, 16, 18, 11, 515);
            // 
            // dateTimePickerHasta
            // 
            this.dateTimePickerHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerHasta.Location = new System.Drawing.Point(199, 12);
            this.dateTimePickerHasta.Name = "dateTimePickerHasta";
            this.dateTimePickerHasta.Size = new System.Drawing.Size(98, 20);
            this.dateTimePickerHasta.TabIndex = 1;
            this.dateTimePickerHasta.Value = new System.DateTime(2023, 7, 12, 16, 18, 11, 517);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Desde";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "hasta";
            // 
            // buttonIniciarExtraccionSenales
            // 
            this.buttonIniciarExtraccionSenales.Location = new System.Drawing.Point(15, 38);
            this.buttonIniciarExtraccionSenales.Name = "buttonIniciarExtraccionSenales";
            this.buttonIniciarExtraccionSenales.Size = new System.Drawing.Size(162, 28);
            this.buttonIniciarExtraccionSenales.TabIndex = 4;
            this.buttonIniciarExtraccionSenales.Text = "Iniciar extracción de señales";
            this.buttonIniciarExtraccionSenales.UseVisualStyleBackColor = true;
            this.buttonIniciarExtraccionSenales.Click += new System.EventHandler(this.buttonIniciarExtraccionSenales_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textBoxExtraccionSenalDetalle
            // 
            this.textBoxExtraccionSenalDetalle.Location = new System.Drawing.Point(15, 73);
            this.textBoxExtraccionSenalDetalle.Multiline = true;
            this.textBoxExtraccionSenalDetalle.Name = "textBoxExtraccionSenalDetalle";
            this.textBoxExtraccionSenalDetalle.ReadOnly = true;
            this.textBoxExtraccionSenalDetalle.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExtraccionSenalDetalle.Size = new System.Drawing.Size(856, 486);
            this.textBoxExtraccionSenalDetalle.TabIndex = 6;
            this.textBoxExtraccionSenalDetalle.WordWrap = false;
            // 
            // FormExtraccionSenales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 571);
            this.Controls.Add(this.textBoxExtraccionSenalDetalle);
            this.Controls.Add(this.buttonIniciarExtraccionSenales);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerHasta);
            this.Controls.Add(this.dateTimePickerDesde);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExtraccionSenales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extracción de señales";
            this.Load += new System.EventHandler(this.FormExtraccionSenales_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerDesde;
        private System.Windows.Forms.DateTimePicker dateTimePickerHasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonIniciarExtraccionSenales;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBoxExtraccionSenalDetalle;
    }
}

