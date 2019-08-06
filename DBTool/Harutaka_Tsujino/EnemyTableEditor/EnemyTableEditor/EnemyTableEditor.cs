using MySql.Data.MySqlClient;
using NeoLethalBlast_MySQL.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnemyTableEditor
{
	public partial class EnemyTableEditor_frm : Form
	{
		private DataTable bindingDataTable;
		private string tableName = "EnemyTable";
		private int autoIncrement;

		public EnemyTableEditor_frm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// データテーブルのセット
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnemyTableEditor_frm_Load(object sender, EventArgs e)
		{
			try
			{
				SetAdvanceDataTable();
			}

			catch (MySqlException me)
			{
				Console.WriteLine("ERROR: " + me.Message);
			}
		}

		/// <summary>
		/// 機能が追加されたデータテーブルの作成
		/// </summary>
		private void SetAdvanceDataTable()
		{
			FetchPrimitiveDataTable();

			OperateEnumColumn();

			InsertDeleteColumn();

			FetchAutoIncrement();

			bindingDataTable.TableNewRow += new DataTableNewRowEventHandler(OnCreateNewRow);

			DataTable_dgv.Columns[0].ReadOnly = true;
			DataTable_dgv.AllowUserToDeleteRows = false;

			ToRowStateUnchanged();
		}

		/// <summary>
		/// データベースから取得してきたそのままのテーブルをdgvにセットする
		/// </summary>
		private void FetchPrimitiveDataTable()
		{
			MySqlDataAdapter tableSource = new MySqlDataAdapter("SELECT * FROM " + tableName, MySQLConnector.Connect());
			bindingDataTable = new DataTable();
			tableSource.Fill(bindingDataTable);

			//コンボボックスを挿入する際に以前のものを消さなければ挿入される場所がおかしくなる
			DataTable_dgv.Columns.Clear();
			DataTable_dgv.DataSource = bindingDataTable;
		}

		/// <summary>
		/// 型の文字列が列挙体を意味しているか
		/// </summary>
		/// <param name="typeString">データベースから取得した型情報</param>
		/// <returns>意味しているならばtrue</returns>
		private bool MeansEnumuration(string typeString)
		{
			return typeString.Contains("enum");
		}

		/// <summary>
		/// 列挙子を抽出しコンボボックスを作成する
		/// </summary>
		/// <param name="dataRow">テーブル詳細</param>
		/// <returns>列挙子のコンボボックス</returns>
		private DataGridViewComboBoxColumn CreateEnumComboBoxColumn(DataRow dataRow)
		{
			DataGridViewComboBoxColumn dgvComboBoxColumn = new DataGridViewComboBoxColumn();
			dgvComboBoxColumn.HeaderText = dataRow[0].ToString();

			var enumComponents = Regex.Matches(GetTypeString(dataRow), @"(\')(?<enumComponent>.+?)(\')");

			BindingSource bindingSource = new BindingSource();

			foreach (var enumComponent in enumComponents.Cast<Match>())
			{
				bindingSource.Add(enumComponent.Value.Trim('\''));
			}

			dgvComboBoxColumn.DataSource = bindingSource;

			return dgvComboBoxColumn;
		}

		/// <summary>
		/// 型情報文字列を取得する
		/// </summary>
		/// <param name="dataRow">テーブル詳細</param>
		/// <returns></returns>
		private string GetTypeString(DataRow dataRow)
		{
			const int TYPE_INFORMATION_INDEX = 1;

			return dataRow[TYPE_INFORMATION_INDEX].ToString();
		}

		/// <summary>
		/// ヘッダーテキストのインデックスを取得する
		/// </summary>
		/// <param name="dgvComboBoxColumn">列挙子のコンボボックス</param>
		/// <returns>インデックス</returns>
		private int GetRowIndexOfHeaderTexxt(DataGridViewComboBoxColumn dgvComboBoxColumn)
		{
			foreach (var dgvColumn in DataTable_dgv.Columns.Cast<DataGridViewColumn>())
			{
				if (dgvColumn.HeaderCell.Value.ToString() != dgvComboBoxColumn.HeaderText) continue;

				return dgvColumn.Index;
			}

			throw new Exception("インデックスが見つかりませんでした");
		}

		/// <summary>
		/// 列挙子のコンボボックスに置き換える
		/// </summary>
		/// <param name="dataRow">テーブル情報</param>
		private void ReplaceColumnWithComboBox(DataRow dataRow)
		{
			var dgvComboBoxColumn = CreateEnumComboBoxColumn(dataRow);

			var index = GetRowIndexOfHeaderTexxt(dgvComboBoxColumn);

			DataTable_dgv.Columns.Insert(index, dgvComboBoxColumn);

			foreach (var dataGridRow in DataTable_dgv.Rows.Cast<DataGridViewRow>())
			{
				dataGridRow.Cells[index].Value = dataGridRow.Cells[index + 1].Value;
			}

			DataTable_dgv.Columns.Remove(DataTable_dgv.Columns[index + 1]);
		}

		/// <summary>
		/// RowStateを全てUnchangedに変更する
		/// </summary>
		private void ToRowStateUnchanged()
		{
			foreach (var dataRow in bindingDataTable.Rows.Cast<DataRow>())
			{
				dataRow.AcceptChanges();
			}
		}

		/// <summary>
		/// データグリッドの列挙子をコンボボックスに変更する
		/// </summary>
		private void OperateEnumColumn()
		{
			MySqlConnection connection = MySQLConnector.Connect();
			MySqlDataAdapter tableInformation = new MySqlDataAdapter("DESCRIBE " + tableName, connection);
			DataTable tableInformationTable = new DataTable();
			tableInformation.Fill(tableInformationTable);

			foreach (var dataRow in tableInformationTable.Rows.Cast<DataRow>())
			{
				if (!MeansEnumuration(GetTypeString(dataRow))) continue;

				ReplaceColumnWithComboBox(dataRow);
			}
		}

		/// <summary>
		/// 物理削除を判断するためのチェックボックスの挿入
		/// </summary>
		private void InsertDeleteColumn()
		{
			DataGridViewCheckBoxColumn deleteColumn = new DataGridViewCheckBoxColumn();

			deleteColumn.Name = "DeleteColumn";
			deleteColumn.HeaderText = "物理削除";

			DataTable_dgv.Columns.Add(deleteColumn);
		}

		/// <summary>
		/// オートインクリメントの取得
		/// </summary>
		private void FetchAutoIncrement()
		{
			MySqlConnection connection = MySQLConnector.Connect();
			MySqlDataAdapter autoIncrementData = new MySqlDataAdapter(
				"SELECT auto_increment FROM information_schema.TABLES WHERE table_name = " + "\'" + tableName + "\';",
				connection);
			DataTable autoIncrementTable = new DataTable();
			autoIncrementData.Fill(autoIncrementTable);

			// テーブルの要素がまだ追加されたことがなかった場合
			if (autoIncrementTable.Rows[0][0].GetType() == typeof(DBNull))
			{
				autoIncrement = 1;

				return;
			}

			autoIncrement = int.Parse(autoIncrementTable.Rows[0][0].ToString());
		}

		/// <summary>
		/// 新しく行が追加された際にIDを自動で割り振る
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCreateNewRow(object sender, EventArgs e)
		{
			int lastRowIndex = bindingDataTable.Rows.Count - 1;

			if (lastRowIndex < 0) return;

			if (bindingDataTable.Rows[lastRowIndex].RowState != DataRowState.Added) return;

			const int PRIMARY_KEY_COLUMN_INDEX = 0;

			int prevIndex = 0;

			for (int i = lastRowIndex - 1; i >= 0; --i)
			{
				if (bindingDataTable.Rows[i].RowState == DataRowState.Deleted) continue;

				prevIndex = int.Parse(bindingDataTable.Rows[i][PRIMARY_KEY_COLUMN_INDEX].ToString()) + 1;

				break;
			}

			if (autoIncrement > prevIndex) prevIndex = autoIncrement;

			var objectValue = bindingDataTable.Rows[lastRowIndex][PRIMARY_KEY_COLUMN_INDEX];

			if (objectValue.GetType() != typeof(DBNull)) return;

			bindingDataTable.Rows[lastRowIndex][PRIMARY_KEY_COLUMN_INDEX] = prevIndex;

			autoIncrement = prevIndex + 1;
		}

		/// <summary>
		/// 値文字列をMySQL用に正規化する
		/// </summary>
		/// <param name="obj">値</param>
		/// <returns>正規化された文字列</returns>
		private string NormalizeValueString(Object obj)
		{
			string value = obj.ToString();

			switch (obj)
			{
				case bool b:

					if (value == "" || value == "False") value = "FALSE";

					if (value == "True") value = "TRUE";

					break;

				case DBNull dbn:

					value = "FALSE";

					break;

				case string s:

					if (value == "")
					{
						value = "FALSE";

						return value;
					}

					value = "'" + value + "'";

					break;
			}

			return value;
		}

		/// <summary>
		/// idを条件にする文を作成する
		/// </summary>
		/// <param name="dataRow">データテーブルの一行</param>
		/// <returns>idを条件にした文</returns>
		private string CreateWhereIdString(DataRow dataRow)
		{
			var commandTextBuider = new StringBuilder();

			commandTextBuider.Append(" WHERE (");
			commandTextBuider.Append("`id` = '");
			commandTextBuider.Append(dataRow[0].ToString());
			commandTextBuider.Append("');");

			return commandTextBuider.ToString();
		}

		/// <summary>
		/// 列の物理削除を行うコマンドの作成
		/// </summary>
		/// <param name="dataRow">データテーブルの一行</param>
		/// <returns>コマンド</returns>
		private string CreateDeleteRowCmd(DataRow dataRow)
		{
			var commandTextBuider = new StringBuilder();
			commandTextBuider.Append("DELETE FROM ");
			commandTextBuider.Append(tableName);

			commandTextBuider.Append(CreateWhereIdString(dataRow));

			return commandTextBuider.ToString();
		}

		/// <summary>
		/// 列の更新を行うコマンドの作成
		/// </summary>
		/// <param name="dataRow">データテーブルの一行</param>
		/// <returns>コマンド</returns>
		private string CreateModifyRowCmd(DataRow dataRow)
		{
			var commandTextBuider = new StringBuilder();
			commandTextBuider.Append("UPDATE ");
			commandTextBuider.Append(tableName);
			commandTextBuider.Append(" SET ");

			for (int columnIndex = 1; columnIndex < bindingDataTable.Columns.Count; ++columnIndex)
			{
				commandTextBuider.Append("`");
				commandTextBuider.Append(DataTable_dgv.Columns[columnIndex].HeaderText);
				commandTextBuider.Append("` = ");

				commandTextBuider.Append(NormalizeValueString(dataRow[columnIndex]));

				commandTextBuider.Append(", ");
			}

			commandTextBuider.Remove(commandTextBuider.Length - 2, 2);

			commandTextBuider.Append(CreateWhereIdString(dataRow));

			return commandTextBuider.ToString();
		}

		/// <summary>
		/// 列の追加を行うコマンドの作成
		/// </summary>
		/// <param name="dataRow">データテーブルの一行</param>
		/// <returns>コマンド</returns>
		private string CreateAddRowCmd(DataRow dataRow)
		{
			var commandTextBuider = new StringBuilder();
			commandTextBuider.Append("INSERT INTO ");
			commandTextBuider.Append(tableName);
			commandTextBuider.Append(" VALUES (");

			for (int columnIndex = 0; columnIndex < bindingDataTable.Columns.Count; ++columnIndex)
			{
				commandTextBuider.Append(NormalizeValueString(dataRow[columnIndex]));

				commandTextBuider.Append(", ");
			}

			commandTextBuider.Remove(commandTextBuider.Length - 2, 2);

			commandTextBuider.Append(")");
			commandTextBuider.Append(";");

			return commandTextBuider.ToString();
		}

		/// <summary>
		/// バインディングソースとデータグリッドとの値の同期
		/// </summary>
		private void SyncBindingSourceWithDataGridView()
		{
			for (var rowIndex = 0; rowIndex < bindingDataTable.Rows.Count; ++rowIndex)
			{
				for (var columnIndex = 1; columnIndex < bindingDataTable.Columns.Count; ++columnIndex)
				{
					bindingDataTable.Rows[rowIndex][columnIndex] = DataTable_dgv.Rows[rowIndex].Cells[columnIndex].Value;
				}
			}
		}

		/// <summary>
		/// コマンドの実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Execution_btn_Click(object sender, EventArgs e)
		{
			SyncBindingSourceWithDataGridView();

			MySqlConnection connection = MySQLConnector.Connect();

			for (var i = 0; i < bindingDataTable.Rows.Count; ++i)
			{
				MySqlCommand cmd = null;

				var dataRow = bindingDataTable.Rows[i];

				var deleteColumnIndex = DataTable_dgv.ColumnCount - 1;

				//削除CheckBoxはbindingSource
				if (Convert.ToBoolean(DataTable_dgv.Rows[i].Cells[deleteColumnIndex].Value))
				{
					cmd = new MySqlCommand(CreateDeleteRowCmd(dataRow), connection);

					if (cmd == null) continue;

					cmd.ExecuteNonQuery();

					continue;
				}

				switch (dataRow.RowState)
				{
					case DataRowState.Modified:
						cmd = new MySqlCommand(CreateModifyRowCmd(dataRow), connection);

						break;

					case DataRowState.Added:
						cmd = new MySqlCommand(CreateAddRowCmd(dataRow), connection);

						break;

					default:
						break;
				}

				if (cmd == null) continue;

				cmd.ExecuteNonQuery();
			}

			SetAdvanceDataTable();
		}
	}
}
