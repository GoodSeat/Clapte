using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio;
using GoodSeat.Sio.Utilities;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
	/// <summary>
	/// ユーザー定義関数の設定パネルを表します。
	/// </summary>
	public partial class UserFunctionPanel : SettingPanel
	{
		/// <summary>
		/// ユーザー定義関数の設定パネルを初期化します。
		/// </summary>
		/// <param name="clapteWatcher"></param>
		public UserFunctionPanel(FunctionListViewModel target, ClapteCoreViewModel clapteCore)
		{
			InitializeComponent();

			Target = target;
			ClapteCore = clapteCore;
			IgnoreEvent = false;

			DownloadSetting();

			RegistEditGridView(_dataGridFunction, _labelError); // データグリッドの使用登録

			_cmbFilter.SelectedIndex = 0;
			_cmbFilter.SelectedIndexChanged += new EventHandler(_cmbFilter_SelectedIndexChanged);

			this.Disposed += new EventHandler(UserFunctionPanel_Disposed);
			Target.DefineAdded += new EventHandler<ListChangeEventArgs<FunctionDefine>>(Target_DefineAdded);
			Target.DefineRemoved += new EventHandler<ListChangeEventArgs<FunctionDefine>>(Target_DefineRemoved);
			Target.DefineChanged += new EventHandler<ListChangeEventArgs<FunctionDefine>>(Target_DefineChanged);

			// データグリッドビュー上の数式インテリセンス構成
//			_formulaIntelisence = new DataGridIntellisenceSupport(_dataGridFunction);
//			_formulaIntelisence.CellEndEdit += new DataGridViewCellEventHandler(_dataGrid_CellEndEdit);
		}

		/// <summary>
		/// 設定対象となる関数定義リストを設定もしくは取得します。
		/// </summary>
		FunctionListViewModel Target { get; set; }

		/// <summary>
		/// 対象となるClapteCoreViewModelオブジェクトを設定もしくは取得します。
		/// </summary>
		ClapteCoreViewModel ClapteCore { get; set; }

		/// <summary>
		/// 定数定義変更時に、数式として解釈可能かを検査するか否かを設定もしくは取得します。
		/// </summary>
		private bool CheckDefineAsFormula { get; set; }

		/// <summary>
		/// 関数定義リストからの変更通知イベントを無視するか否かを設定もしくは取得します。
		/// </summary>
		private bool IgnoreEvent { get; set; }
		


		/// <summary>
		/// 設定からコントロールに状態を反映します。
		/// </summary>
		void DownloadSetting()
		{
			if (IgnoreEvent) return;

			_dataGridFunction.Rows.Clear();
			foreach (var def in Target.GetSystemFunctions()) AddFunctionToGridView(def, true);
			foreach (var def in Target.Target) AddFunctionToGridView(def, false);
		}

		/// <summary>
		/// 関数をデータグリッドビューに追加します。
		/// </summary>
		/// <param name="define">登録する関数定義。</param>
		/// <param name="asSystem">システム定義の関数として登録するか否か。</param>
		void AddFunctionToGridView(FunctionDefine define, bool asSystem)
		{
			Function function = define.Target;
			AddColumnsFor(function);

			DataGridViewRow row = new DataGridViewRow();
			row.Tag = define;

			// 必要分だけ列を追加
			for (int i = 0; i < _dataGridFunction.Columns.Count; i++) row.Cells.Add(new DataGridViewTextBoxCell());

			// 関数の名前、定義、説明
			List<string> argsInformation;
			string information = define.Information;
			define.Target.GetInformation(out argsInformation);

			row.Cells[2].Value = information;
			if (!asSystem)
			{
				UserFunction userFunction = function as UserFunction;

				row.Cells[0].Value = userFunction.NameForView;
				row.Cells[1].Value = define.Define; // userFunction.UseFormula;
			}
			else
			{
				List<Variable> listTmpVariables = new List<Variable>();
				for (int i = 0; i < function.MaximumArgumentQty; i++)
					listTmpVariables.Add(new Variable(((char)('a' + i)).ToString()));
				row.Cells[0].Value = function.DistinguishedName + "(" + new Argument(listTmpVariables.ToArray()).ToString() + ")";
				row.Cells[0].ReadOnly = row.Cells[1].ReadOnly = row.Cells[2].ReadOnly = true;
				row.Cells[0].Style.BackColor = row.Cells[1].Style.BackColor = row.Cells[2].Style.BackColor = Color.WhiteSmoke;
			}
			_dataGridFunction.Rows.Add(row);

			// 引数のコメント
			for (int i = 0; i < function.MaximumArgumentQty; i++)
			{
                if (argsInformation.Count > i) row.Cells[3 + i].Value = argsInformation[i];

				if (function is UserFunction)
				{
					row.Cells[3 + i].ReadOnly = false;
					row.Cells[3 + i].Style.BackColor = Color.White;
				}
				else
				{
					row.Cells[3 + i].ReadOnly = true;
					row.Cells[3 + i].Style.BackColor = Color.WhiteSmoke;
				}
			}
		}

		/// <summary>
		/// 指定関数を表示させるに当たって、必要に応じて列を追加します。
		/// </summary>
		/// <param name="function">判定対象の関数。</param>
		void AddColumnsFor(Function function)
		{
			while (_dataGridFunction.Columns.Count < function.MaximumArgumentQty + 3) // 3は名前、定義、コメント分
			{
				DataGridViewColumn column;
				if (_dataGridFunction.Columns.Count != 0) column = new DataGridViewColumn(_dataGridFunction.Columns[0].CellTemplate);
				else column = new DataGridViewColumn();

				column.HeaderText = "引数" + (_dataGridFunction.Columns.Count - 2).ToString() + "のコメント";
				_dataGridFunction.Columns.Add(column);
				column.DefaultCellStyle.BackColor = Color.WhiteSmoke;
				column.ReadOnly = true;
			}
		}

		/// <summary>
		/// 指定インデックス行の関数データを削除し、ソルバからの登録を解除します。
		/// </summary>
		/// <param name="index">削除対象の行番号。</param>
		void RemoveUserFunction(int index)
		{
			var def = _dataGridFunction.Rows[index].Tag as FunctionDefine;
			if (!(def.Target is UserFunction)) return;

			_dataGridFunction.Rows.RemoveAt(index);
			RemoveInvalidColumns();

			IgnoreEvent = true;
			Target.Target.Remove(def);
			IgnoreEvent = false;
		}

		/// <summary>
		/// 不要な列を削除します。
		/// </summary>
		void RemoveInvalidColumns()
		{
			int maxLength = 0;
			foreach (DataGridViewRow row in _dataGridFunction.Rows)
			{
				var def = row.Tag as FunctionDefine;
				List<string> args = def.ArgumentInformation;
				maxLength = Math.Max(maxLength, args.Count);
			}

			while (_dataGridFunction.Columns.Count > maxLength + 3)
				_dataGridFunction.Columns.RemoveAt(_dataGridFunction.Columns.Count - 1);
		}

		/// <summary>
		/// 定義名称からユーザー定義関数を生成して取得します。生成に失敗した場合、例外が投げられます。
		/// </summary>
		/// <param name="define">関数定義文字列。</param>
		/// <returns>生成されたユーザー定義関数。</returns>
		UserFunction CreateUserFunctionFrom(string define, DataGridViewCellEventArgs e)
		{
			string[] split = define.Split('(');

			UserFunction function = null;
			if (split.Length == 1)
			{
				function = new UserFunction(define);
			}
			else
			{
				function = new UserFunction(split[0]);
				string args = split[1].TrimEnd(')');

				Formula formula = Formula.Parse(args);
				Argument arg = formula as Argument;
				if (formula is Variable) arg = new Argument(formula);
				if (arg != null)
				{
					foreach (Formula f in arg)
					{
						if (!(f is Variable)) throw new Exception(define + "は関数名として有効ではありません。");
						if (function.UseVariable.Contains(f as Variable)) throw new Exception("引数に同名の変数を複数使用することはできません。");
						function.UseVariable.Add(f as Variable);
					}
				}
			}

			// 名前の重複チェック
			for (int i = 0; i < _dataGridFunction.Rows.Count; i++)
			{
				if (i == e.RowIndex) continue;
				if ((_dataGridFunction.Rows[i].Tag as FunctionDefine).Name == function.DistinguishedName)
					throw new Exception(function.DistinguishedName + "という名前の関数が既に存在します。");
			}
			return function;
		}

		/// <summary>
		/// 定義の存在しない適当な関数名を取得します。
		/// </summary>
		/// <returns>定義の存在しない適当な関数名。</returns>
		string GetNotExistFuctionName()
		{
			// 変数名探索
			int count = 1;
			string name = "NewFunction";
			bool existSame = false;
			do
			{
				existSame = false;
				name = "NewFunction" + (count++).ToString();
				foreach (DataGridViewRow rowCheck in _dataGridFunction.Rows)
				{
					if ((rowCheck.Tag as FunctionDefine).Name == name)
					{
						existSame = true;
						break;
					}
				}
			}
			while (existSame);

			return name;
		}



		// 関数定義リストに定義が追加された時
		void Target_DefineAdded(object sender, ListChangeEventArgs<FunctionDefine> e)
		{
			DownloadSetting();
			if (e.Items.Length == 0) return;

			_dataGridFunction.ClearSelection();
			var item = e.Items[0];
			foreach (DataGridViewRow rowCheck in _dataGridFunction.Rows)
			{
				if (rowCheck.Tag == item)
				{
					rowCheck.Selected = true;
					break;
				}
			}
			if (!_dataGridFunction.SelectedCells[0].Displayed)
			{
				_dataGridFunction.FirstDisplayedScrollingRowIndex = _dataGridFunction.SelectedCells[0].RowIndex - _dataGridFunction.DisplayedRowCount(false);
			}
		}

		// 関数定義リストから定義が削除された時
		void Target_DefineRemoved(object sender, ListChangeEventArgs<FunctionDefine> e)
		{
			DownloadSetting();
		}

		// 関数定義リストの定義が変更された時
		void Target_DefineChanged(object sender, ListChangeEventArgs<FunctionDefine> e)
		{
			DownloadSetting();
		}



		// 表示アイテムフィルターの変更
		void _cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in _dataGridFunction.Rows)
			{
				var def = row.Tag as FunctionDefine;
				if (!(def.Target is UserFunction)) row.Visible = (_cmbFilter.SelectedIndex != 2);
				else row.Visible = (_cmbFilter.SelectedIndex != 1);
			}
		}

		// 新規関数追加
		private void _btnAdd_Click(object sender, EventArgs e)
		{
			if (_cmbFilter.SelectedIndex == 1) _cmbFilter.SelectedIndex = 0;

			string name = GetNotExistFuctionName();

			var newFunction = new UserFunction(name);
			FunctionDefine def = new FunctionDefine(newFunction);
			Target.Target.Add(def);
		}

		// 関数削除
		private void _btnDelete_Click(object sender, EventArgs e)
		{
			_labelError.Text = "削除対象の行ヘッダーをクリックして、行を選択してください。";
			_labelError.Visible = true;
			foreach (DataGridViewRow row in _dataGridFunction.SelectedRows)
			{
				_labelError.Visible = false;
				RemoveUserFunction(row.Index);
			}
		}

		// 行の削除
		private void _dataGridFunction_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			e.Cancel = true;
			_btnDelete_Click(sender, e);
		}


		// 編集開始時
		protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
		{
			if (e.ColumnIndex == 1) // 定義編集時
			{
				//TargetWatcher.Solver.RenewIntellisence(_formulaIntelisence.Intelisence, Core.Solver.IntellisenceType.All);

				//// 引数をインテリセンスに登録
				//UserFunction userFunction = _dataGridFunction.Rows[e.RowIndex].Tag as UserFunction;
				//foreach (Variable v in userFunction.UseVariable)
				//{
				//    _formulaIntelisence.Intelisence.VariableList.Remove(v.Mark);

				//    if (userFunction.ArgumentInformation.ContainsKey(v))
				//        v.Information = userFunction.ArgumentInformation[v];
				//    _formulaIntelisence.Intelisence.VariableList.Add(v.Mark, v);
				//}

				//_formulaIntelisence.StartInputFormula(e);
			}
		}

		// 編集終了時
		protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
		{
			var def = _dataGridFunction.Rows[e.RowIndex].Tag as FunctionDefine;
			UserFunction editTarget = (def.Target as UserFunction);

			if (e.ColumnIndex == 0) // 関数名称変更
			{
				UserFunction createdFunction = CreateUserFunctionFrom(GetStringFrom(_dataGridFunction[e.ColumnIndex, e.RowIndex].Value), e);
				if (createdFunction.Name == "") throw new Exception("入力された関数名は無効です。");

				editTarget.Name = createdFunction.Name;
				editTarget.UseVariable.Clear();
				editTarget.UseVariable.AddRange(createdFunction.UseVariable);

				_dataGridFunction[e.ColumnIndex, e.RowIndex].Value = createdFunction.NameForView;
				AddColumnsFor(editTarget);
				{
					List<string> args; editTarget.GetInformation(out args);
					for (int i = 0; i < args.Count; i++)
					{
						if (GetStringFrom(_dataGridFunction[i + 3, e.RowIndex].Value) == "")
							_dataGridFunction[i + 3, e.RowIndex].Value = args[i];
						_dataGridFunction[i + 3, e.RowIndex].ReadOnly = false;
						_dataGridFunction[i + 3, e.RowIndex].Style.BackColor = Color.White;
					}
					for (int i = args.Count; i < _dataGridFunction.Columns.Count - 3; i++)
					{
						_dataGridFunction[i + 3, e.RowIndex].ReadOnly = true;
						_dataGridFunction[i + 3, e.RowIndex].Style.BackColor = Color.WhiteSmoke;
						_dataGridFunction[i + 3, e.RowIndex].Value = "";
					}
				}
				RemoveInvalidColumns();
			}
			else if (e.ColumnIndex == 1) // 定義変更時
			{
				string value = GetStringFrom(_dataGridFunction[e.ColumnIndex, e.RowIndex].Value);
				if (CheckDefineAsFormula) ClapteCore.Solver.Target.Parser.Parse(value); // 数式チェック

				def.Define = value;
			}
			else if (e.ColumnIndex == 2) // コメント
			{
				def.Information = GetStringFrom(_dataGridFunction[e.ColumnIndex, e.RowIndex].Value);
			}
			else // 引数のコメント
			{
				int argsIndex = e.ColumnIndex - 3;
				var f = editTarget.UseVariable[argsIndex];
				editTarget.ArgumentInformation[f] = GetStringFrom(_dataGridFunction[e.ColumnIndex, e.RowIndex].Value);
			}
		}
		
		// リソースの破棄
		void UserFunctionPanel_Disposed(object sender, EventArgs e)
        {
			Target.DefineAdded -= new EventHandler<ListChangeEventArgs<FunctionDefine>>(Target_DefineAdded);
			Target.DefineRemoved -= new EventHandler<ListChangeEventArgs<FunctionDefine>>(Target_DefineRemoved);
			Target.DefineChanged -= new EventHandler<ListChangeEventArgs<FunctionDefine>>(Target_DefineChanged);
//			_formulaIntelisence.Dispose();
		}

		// ソート時比較関数
		private void _dataGridFunction_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{			
			e.SortResult = GetStringFrom(e.CellValue1).CompareTo(GetStringFrom(e.CellValue2));
		}

		// 定義参照受付
		public override bool OpenDefine(object target)
		{
			if (!(target is Function)) return false;

			Function targetFunction = target as Function;

			_dataGridFunction.ClearSelection();
			foreach (DataGridViewRow row in _dataGridFunction.Rows)
			{
				FunctionDefine def = row.Tag as FunctionDefine;
				if (def.Name == targetFunction.DistinguishedName)
				{
					row.Selected = true;
					_dataGridFunction.FirstDisplayedScrollingRowIndex = row.Index;
					return true;
				}
			}
			return base.OpenDefine(target);
		}

	}
}
