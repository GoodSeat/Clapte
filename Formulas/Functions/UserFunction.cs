using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// ユーザー定義関数を表します。
	/// </summary>
	[Serializable()]
	public class UserFunction : Function
	{
		string _name = "userFunction";
		Formula _useFormula = new Numeric(0);
		List<Variable> _variableList = new List<Variable>();

		string _information = "";
		Dictionary<Variable, string> _argsInformation = new Dictionary<Variable,string>();

		/// <summary>
		/// ユーザー定義関数を初期化します。
		/// </summary>
		public UserFunction() : base() { }

		/// <summary>
		/// ユーザー定義関数を初期化します。
		/// </summary>
		/// <param name="name">関数名</param>
		public UserFunction(string name) : base() { Name = name; }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			var result = this.Copy() as UserFunction;
			result.Argument = new Argument(args);
			return result;
		}

		/// <summary>
		/// 関数名を設定もしくは取得します。
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// 関数の識別用文字列を取得します。たとえば、f(x,y)といった文字列です。
		/// </summary>
		public string NameForView
		{
			get
			{
				string arg = "";
				foreach (Variable v in UseVariable) arg += v.ToString() + ", ";
				arg = arg.TrimEnd(' ', ',');

				return DistinguishedName + "(" + arg + ")";
			}
		}

		/// <summary>
		/// 関数の変数として使用する変数を設定もしくは取得します。 f(x,y)=ax+y+bでいう、x,yの部分です。
		/// </summary>
		public List<Variable> UseVariable
		{
			get { return _variableList; }
			set { _variableList = value; }
		}

		/// <summary>
		/// 関数の値として使用する式を設定もしくは取得します。 f(x,y)=ax+y+bでいう、ax+y+bの部分です。
		/// </summary>
		public Formula UseFormula
		{
			get { return _useFormula; }
			set { _useFormula = value; }
		}

		/// <summary>
		/// 関数の説明を設定もしくは取得します。
		/// </summary>
		public string Information
		{
			get { return _information; }
			set { _information = value; }
		}

		/// <summary>
		/// 引数の説明リストを取得します。
		/// </summary>
		public Dictionary<Variable, string> ArgumentInformation { get { return _argsInformation; } }

		public override string DistinguishedName { get { return _name; } }

		public override Formula CalculateFunction()
		{
			Formula ret = _useFormula.Copy();

			for (int i = 0; i < Argument.Count; i++)
				ret = ret.Substitute(_variableList[i], Argument[i]);

			return ret.Calculate();
		}

		public override int MinimumArgumentQty { get { return _variableList.Count; } }

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>();
			foreach (Variable v in UseVariable)
			{
				if (ArgumentInformation.ContainsKey(v))
					args.Add(ArgumentInformation[v]);
				else 
					args.Add(v.ToString());
			}

			if (Information != "") 
				return Information;
			else
				return UseFormula.ToString() + "を返します。";
		}

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText()
		{
			return string.Format("{0}({1})", DistinguishedName, base.OnGetUniqueText());
		}
	}

}
