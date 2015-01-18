using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Clapte.Solvers
{
	/// <summary>
	/// 定数の定義を表します。
	/// </summary>
	public class ConstantDefine
	{
		/// <summary>
		/// 定数定義を初期化します。
		/// </summary>
		public ConstantDefine() { }

		/// <summary>
		/// システム定義の定数に関する定数定義を初期化します。
		/// </summary>
		public ConstantDefine(Constant constant) 
		{
			Name = constant.DistinguishedName;
			Information = constant.Information;
			Define = constant.Value.ToString();

			_target = constant;
		}

		/// <summary>
		/// 定数定義を初期化します。
		/// </summary>
		/// <param name="name">定数の名前。</param>
		public ConstantDefine(string name) { Name = name; }


		/// <summary>
		/// 定義対象の定数名を設定もしくは取得します。
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 定義対象の定数の説明文を設定もしくは取得します。
		/// </summary>
		public string Information { get; set; }

		/// <summary>
		/// 定数の定義数式を設定もしくは取得します。
		/// </summary>
		public string Define { get; set; }

		Formula _target;

		/// <summary>
		/// 対象となる定数として扱うべき変数を取得します。
		/// </summary>
		public Formula Target 
		{ 
			get 
			{
				if (_target == null) _target = new Variable(Name);
				return _target;
			}
		}

	}
}
