using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Array.Rules;

namespace Liffom.Formulas.Array
{
	/// <summary>
	/// 多次元配列を表します。
	/// </summary>
	[Serializable()]
	public class MultiArray : Formula
	{
		Dictionary<ArrayIndex, Formula> _value = new Dictionary<ArrayIndex,Formula>(); // 値

		int[] _size; // 各次元のサイズ

		/// <summary>
		/// 多次元配列を初期化します。
		/// </summary>
		/// <param name="size">次元数。</param>
		public MultiArray(string name, int size)
		{
			Name = name;

			DimensionSize = size;
			_size = new int[size];
			for (int i = 1; i <= size; i++) SetSize(i, 1);
		}

		/// <summary>
		/// 可変数の数式を指定して、子数式を有する数式を初期化します。
		/// </summary>
		/// <param name="fs">数式を構成する可変数の子数式。</param>
		/// <returns>初期化された数式。</returns>
		public override Formula CreateFromChildren(params Formula[] fs)
		{
			var result = new MultiArray(Name, DimensionSize);
			for (int i = 1; i <= DimensionSize; i++) result.SetSize(i, GetSize(i));

			result.Index = Index;
			int k = 0;
			while (this[k] != null)
			{
				result[k] = this[k];
				k++;
			}
			return result;
		}

		/// <summary>
		/// 次元数を取得します。
		/// </summary>
		public int DimensionSize { get; private set; }

		/// <summary>
		/// 名前を設定もしくは取得します。
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 指定次元のサイズを取得します。
		/// </summary>
		/// <param name="dimension">対象の1から始まる次元番号。</param>
		/// <returns>次元のサイズ。</returns>
		public int GetSize(int dimension) { return _size[dimension - 1]; }

		/// <summary>
		/// 指定次元のサイズを設定します。
		/// </summary>
		/// <param name="dimension">対象の1から始まる次元番号。</param>
		/// <param name="size">次元のサイズ。</param>
		public void SetSize(int dimension, int size) { _size[dimension - 1] = size; }

		/// <summary>
		/// この数式において使用するインデックスを取得します。
		/// </summary>
		public ArrayIndex Index { get; private set; }


		/// <summary>
		/// この数式において使用する配列内インデックスを指定します。
		/// 指定されたインデックスを整数に変換できる場合、当該インデックス位置の数式を返します。
		/// それ以外の場合、この配列を返します。
		/// </summary>
		/// <param name="indexs">設定インデックス。</param>
		/// <returns>取得される該当数式、もしくはこの配列。</returns>
		public virtual Formula At(params Formula[] indexs)
		{
			if (indexs != null && indexs.Length != DimensionSize) throw new FormulaProcessException("次元数が一致しません。");

			if (indexs == null)
			{
				Index = null;
				return this;
			}
			Index = new ArrayIndex(DimensionSize);
			for (int i = 1; i <= DimensionSize; i++) Index[i] = indexs[i - 1];

			var result = GetAt(Index);
			if (result == null) return this;
			else return result;
		}

		/// <summary>
		/// この数式において使用する配列内インデックスを指定します。
		/// 指定されたインデックスを整数に変換できる場合、当該インデックス位置の数式を返します。
		/// それ以外の場合、この配列を返します。
		/// </summary>
		/// <param name="indexs">設定インデックス。</param>
		/// <returns>取得される該当数式、もしくはこの配列。</returns>
		public Formula At(ArrayIndex index)
		{
			if (index.DimensionSize != DimensionSize) throw new FormulaProcessException("次元数が異なります。");

			Formula[] indexs = new Formula[DimensionSize];
			for (int i = 1; i <= DimensionSize; i++) indexs[i - 1] = Index[i];

			return At(indexs);
		}

		/// <summary>
		/// 指定配列インデックスに数式を設定します。
		/// </summary>
		/// <param name="value">設定する数式。</param>
		/// <param name="indexs">設定先の配列インデックス。</param>
		public void Set(Formula value, params int[] indexs)
		{
			if (indexs.Length != DimensionSize) throw new FormulaProcessException("次元数が異なります。");

			for (int i = 1; i <= DimensionSize; i++)
				if (GetSize(i) < indexs[i - 1]) throw new IndexOutOfRangeException();

			ArrayIndex ai = new ArrayIndex(DimensionSize);
			for (int i = 1; i <= DimensionSize; i++) ai[i] = indexs[i - 1];

			if (_value.ContainsKey(ai)) _value[ai] = value;
			else _value.Add(ai, value);
		}

		/// <summary>
		/// 指定配列インデックスの数式を取得します。
		/// </summary>
		/// <param name="indexs">取得先の配列インデックス。</param>
		public Formula GetAt(params int[] indexs)
		{
			if (indexs.Length != DimensionSize) throw new FormulaProcessException("次元数が異なります。");

			for (int i = 1; i <= DimensionSize; i++)
				if (GetSize(i) < indexs[i - 1]) throw new IndexOutOfRangeException();

			ArrayIndex ai = new ArrayIndex(DimensionSize);
			for (int i = 1; i <= DimensionSize; i++) ai[i] = indexs[i - 1];

			if (_value.ContainsKey(ai)) return _value[ai];
			else return 0;
		}

		/// <summary>
		/// 指定配列インデックスの数式を取得します。整数
		/// </summary>
		/// <param name="indexs">取得先の配列インデックス。</param>
		public Formula GetAt(ArrayIndex index)
		{
			if (index.DimensionSize != DimensionSize) throw new FormulaProcessException("次元数が異なります。");

			int[] indexs = new int[DimensionSize];
			for (int i = 1; i <= DimensionSize; i++)
			{
				var n = index[i] as Numeric;
				if (n == null || !n.IsInteger) return null;
				indexs[i - 1] = (int)n.Data;
			}

			return GetAt(indexs);
		}

		protected override Formula OnSubstitute(Formula oldValue, Formula newValue)
		{
			if (Index != null)
			{
				for (int i = 1; i <= DimensionSize; i++)
					Index[i] = Index[i].Substitute(oldValue, newValue);
			}
			return base.OnSubstitute(oldValue, newValue);
		}


		public override Formula this[int i]
		{
			get
			{
				int allSize = 1;
				for (int k = 1; k <= DimensionSize; k++) allSize *= GetSize(k);
				if (i >  allSize) return null;

				ArrayIndex index = GetIndexFromSerialIndex(i);

				if (_value.ContainsKey(index)) return _value[index];
				return 0;
			}
			set
			{
				ArrayIndex index = GetIndexFromSerialIndex(i);

				if (_value.ContainsKey(index)) _value[index] = value;
				else _value.Add(index, value);
			}
		}

		/// <summary>
		/// 指定した直列インデックスを、配列インデックスに変換して取得します。
		/// </summary>
		/// <param name="serialIndex">変換対象の直列インデックス。</param>
		/// <returns>対応する配列インデックス。</returns>
		ArrayIndex GetIndexFromSerialIndex(int serialIndex)
		{
			ArrayIndex result = new ArrayIndex(DimensionSize);
			ArrayIndex size = new ArrayIndex(DimensionSize);
			for (int i = 1; i <= DimensionSize; i++) size.SetPosition(i, GetSize(i));
			result.SetPositionFromSerialPosition(serialIndex, size);
			return result;
		}

		public override string GetText()
		{
			string result = Name;
			if (Index != null) result += Index.ToString();
			return result;
		}

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText()
		{
			StringBuilder sb = new StringBuilder("");
			ArrayIndex ai = new ArrayIndex(DimensionSize);

			int[] index = new int[DimensionSize];
			for (int i = 0; i < DimensionSize;i++) index[i] = 1;
			do
			{
				for (int i = 1; i <= DimensionSize; i++) ai[i] = index[i - 1];

				sb.Append(ai.ToString() + ":" + GetAt(index).ToString() + ", ");
			}
			while (GetNextIndex(1, ref index));

			return sb.ToString().TrimEnd(',', ' ');
		}

		/// <summary>
		/// 指定処理に関連するルールをすべて返す反復子を取得します。
		/// </summary>
		/// <param name="deformToken">変形識別トークン。</param>
		/// <param name="sender">ルール適用対象となる最上位親数式。</param>
		/// <param name="history">変形履歴情報。</param>
		/// <returns>変形に関連するルールを返す反復子。</returns>
		public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
		{
			foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

			if (deformToken.Has<CalculateToken>()) yield return ReferenceMultiArrayRule.Entity; // 計算時、インデックスの指定があるならそのインデックス位置の値とする。
		}

		/// <summary>
		/// 指定したインデックス位置の次の位置を順次返します。
		/// </summary>
		/// <param name="dimension">1を指定します。</param>
		/// <param name="index">インデックス位置を表す整数配列。</param>
		/// <returns>指定インデックス位置の次の位置が見つかったか否か。</returns>
		/// <example>[1,1,1][2,1,1][3,1,1][1,2,1][2,2,1]....[2,3,3][3,3,3]の順に返す。</example>
		bool GetNextIndex(int dimension, ref int[] index)
		{
			foreach (int check in index) if (check < 1) throw new IndexOutOfRangeException();

			if (index[dimension - 1] == GetSize(dimension))
			{
				// 次のdimensionを上げる
				if (dimension == DimensionSize) return false;

				return GetNextIndex(dimension + 1, ref index);
			}
			else
			{
				index[dimension - 1]++;
				for (int i = 0; i < dimension - 1; i++) index[i] = 1;
				return true;
			}
		}
	}

}
