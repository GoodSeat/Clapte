using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liffom.Formulas.Matrices
{
	/// <summary>
	/// 行ベクトル、もしくは列ベクトルを表します。
	/// </summary>
	[Serializable()]
	public class Vector : Matrix
	{
		/// <summary>
		/// 行ベクトルを初期化します。
		/// </summary>
		/// <param name="values">可変数のベクトル要素。</param>
		public Vector(params Formula[] values) : this(true, values) { }

		/// <summary>
		/// ベクトルを初期化します。
		/// </summary>
		/// <param name="vector">コピー元のベクトル。</param>
		public Vector(Vector vector) : base(vector as Matrix) { }
		
		/// <summary>
		/// ベクトルを初期化します。
		/// </summary>
		/// <param name="asRowVector">行ベクトルとして初期化するか否か。</param>
		/// <param name="size">ベクトルのサイズ。</param>
		public Vector(bool asRowVector, int size) 
			: base(1, size) 
		{
			if (!asRowVector)
			{
				RowSize = size;
				ColumnSize = 1;
				Elements = new Formula[RowSize, ColumnSize];

				for (int r = 1; r <= RowSize; r++)
					for (int c = 1; c <= ColumnSize; c++)
						this[r, c] = 0;
			}
		}

		/// <summary>
		/// ベクトルを初期化します。
		/// </summary>
		/// <param name="asRowVector">行ベクトルとして初期化するか否か。</param>
		/// <param name="values">可変数のベクトル要素。</param>
		public Vector(bool asRowVector, params Formula[] values)
			: base(1, values.Length)
		{ 
			if (asRowVector)
			{
				for (int c = 1; c <= ColumnSize; c++)
					this[1, c] = values[c - 1];
			}
			else
			{
				RowSize = values.Length;
				ColumnSize = 1;
				Elements = new Formula[RowSize, ColumnSize];

				for (int r = 1; r <= RowSize; r++)
					for (int c = 1; c <= ColumnSize; c++)
						this[r, c] = 0;

				for (int r = 1; r <= RowSize; r++)
					this[r, 1] = values[r - 1];
			}
		}

		/// <summary>
		/// このベクトルが行ベクトルか否かを取得します。
		/// </summary>
		public bool IsRowVector { get { return RowSize == 1; } }

		/// <summary>
		/// このベクトルが列ベクトルか否かを取得します。
		/// </summary>
		public bool IsColumnVector { get { return ColumnSize == 1; } }

		public override Type GetEqualBaseType() { return typeof(Matrix); }

	}
}

