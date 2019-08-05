namespace EnemyTableEditor
{
	partial class EnemyTableEditor_frm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.DataTable_dgv = new System.Windows.Forms.DataGridView();
			this.Execution_btn = new System.Windows.Forms.Button();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.DataTable_dgv)).BeginInit();
			this.SuspendLayout();
			// 
			// DataTable_dgv
			// 
			this.DataTable_dgv.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
			this.DataTable_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataTable_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataTable_dgv.GridColor = System.Drawing.SystemColors.ActiveBorder;
			this.DataTable_dgv.Location = new System.Drawing.Point(12, 12);
			this.DataTable_dgv.Name = "DataTable_dgv";
			this.DataTable_dgv.RowTemplate.Height = 21;
			this.DataTable_dgv.Size = new System.Drawing.Size(776, 397);
			this.DataTable_dgv.TabIndex = 0;
			// 
			// Execution_btn
			// 
			this.Execution_btn.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.Execution_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.Execution_btn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Execution_btn.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed;
			this.Execution_btn.FlatAppearance.BorderSize = 0;
			this.Execution_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
			this.Execution_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Execution_btn.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Execution_btn.ForeColor = System.Drawing.SystemColors.Menu;
			this.Execution_btn.Location = new System.Drawing.Point(713, 415);
			this.Execution_btn.Name = "Execution_btn";
			this.Execution_btn.Size = new System.Drawing.Size(75, 23);
			this.Execution_btn.TabIndex = 1;
			this.Execution_btn.Text = "実行";
			this.Execution_btn.UseVisualStyleBackColor = false;
			this.Execution_btn.Click += new System.EventHandler(this.Execution_btn_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// EnemyTableEditor_frm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.Execution_btn);
			this.Controls.Add(this.DataTable_dgv);
			this.Name = "EnemyTableEditor_frm";
			this.Text = "EnemyTableEditor";
			this.Load += new System.EventHandler(this.EnemyTableEditor_frm_Load);
			((System.ComponentModel.ISupportInitialize)(this.DataTable_dgv)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView DataTable_dgv;
		private System.Windows.Forms.Button Execution_btn;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
	}
}

