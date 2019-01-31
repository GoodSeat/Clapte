// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Matrices.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Matrices
{
    /// <summary>
    /// 行列を表します。
    /// </summary>
    [Serializable()]
    public class Matrix : Formula
    {
        /// <summary>
        /// 指定サイズの単位行列を生成して取得します。
        /// </summary>
        /// <param name="size">単位行列の行・列数。</param>
        public static Matrix E(int size)
        {
            var e = new Matrix(size, size);

            for (int r = 1; r <= size; r++)
            {
                for (int c = 1; c <= size; c++)
                {
                    if (r == c) e[r, c] = 1;
                    else e[r, c] = 0;
                }
            }
            return e;
        }


        /// <summary>
        /// 行列を初期化します。
        /// </summary>
        /// <param name="rowSize">行数。</param>
        /// <param name="coulumnSize">列数。</param>
        public Matrix(int rowSize, int columnSize)
        { 
            RowSize = rowSize;
            ColumnSize = columnSize;
            Elements = new Formula[rowSize, columnSize];

            for (int r = 1; r <= rowSize; r++)
                for (int c = 1; c <= columnSize; c++)
                    this[r, c] = 0;
        }

        /// <summary>
        /// 行列を初期化します。
        /// </summary>
        /// <param name="vectors">可変数の行ベクトル。</param>
        public Matrix(params Vector[] vectors)
            : this(vectors.Length, vectors[0].ColumnSize)
        { 
            for (int i = 0; i < vectors.Length; i++)
            {
                if (vectors[i].ColumnSize != ColumnSize) throw new FormulaAssertionException();

                for (int k = 1; k <= ColumnSize; k++) this[i + 1, k] = vectors[i][1, k];
            }
        }

        /// <summary>
        /// 行列を初期化します。
        /// </summary>
        /// <param name="matrix">コピー元のマトリクス。</param>
        public Matrix(Matrix matrix)
            : this(matrix.RowSize, matrix.ColumnSize)
        { 
            for (int i = 1; i <= RowSize; i++)
            {
                for (int k = 1; k <= ColumnSize; k++)
                {
                    this[i, k] = matrix[i, k];
                }
            }
        }

        /// <summary>
        /// 可変数の数式を指定して、子数式を有する数式を初期化します。
        /// </summary>
        /// <param name="fs">数式を構成する可変数の子数式。</param>
        /// <returns>初期化された数式。</returns>
        public override Formula CreateFromChildren(params Formula[] fs)
        {
            var result = new Matrix(RowSize, ColumnSize);
            for (int i = 1; i <= RowSize * ColumnSize; i++)
                result[i] = fs[i];
            return result;
        }

        /// <summary>
        /// 行列の行数を取得します。
        /// </summary>
        public int RowSize { get; protected set; }

        /// <summary>
        /// 行列の列数を取得します。
        /// </summary>
        public int ColumnSize { get; protected set; }

        /// <summary>
        /// 行列の各要素データを設定もしくは取得します。
        /// r行、c列のデータは、[r-1, c-1]の要素となることに注意してください。
        /// </summary>
        protected Formula[,] Elements { get; set; }




        /// <summary>
        /// 指定インデックスの要素を設定もしくは取得します。
        /// </summary>
        /// <param name="row">1から始まる行番号。</param>
        /// <param name="column">1から始まる列番号。</param>
        public Formula this[int row, int column]
        {
            get { return Elements[row - 1, column - 1]; }
            set { Elements[row - 1, column - 1] = value; }
        }

        /// <summary>
        /// 指定インデックスの要素を設定もしくは取得します。
        /// </summary>
        /// <param name="i">1から始まる要素番号。</param>
        public override Formula this[int i]
        {
            get
            {
                if (i == 0) return Null.Empty;

                i--;
                int allCount = RowSize * ColumnSize;
                int row    = (i / ColumnSize) + 1;
                int column = (i - (row - 1) * ColumnSize) + 1;

                if (row > RowSize || column > ColumnSize) return null;

                return this[row, column];
            }
            set
            {
                if (i == 0) return;

                i--;
                int allCount = RowSize * ColumnSize;
                int row    = (i / ColumnSize) + 1;
                int column = (i - (row - 1) * ColumnSize) + 1;

                if (row > RowSize || column > ColumnSize) return;
                this[row, column] = value;
            }
        }

        public override string GetText()
        {
            string result = "";
            if (RowSize != 1) result = "{";

            for (int r = 1; r <= RowSize; r++)
            {
                string rowText = "[";
                if (ColumnSize == 1)
                {
                    rowText = this[r, 1].ToString();
                }
                else
                {
                    for (int c = 1; c <= ColumnSize; c++)
                        rowText += this[r, c].ToString() + ", ";
                    rowText = rowText.TrimEnd(' ', ',') + "]";
                }
                result += rowText + ", ";
            }

            result = result.TrimEnd(' ', ',');
            if (RowSize != 1) result = result + "}";

            return result;
        }

        /// <summary>
        /// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
        /// </summary>
        /// <returns>数式を一意に区別する文字列。</returns>
        protected override string OnGetUniqueText() 
        {
            List<string> children = new List<string>();
            children.Add(string.Format("R:{0},C:{1}", RowSize, ColumnSize));

            foreach (var child in this) children.Add(child.GetUniqueText());

            return string.Join(",", children);
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

            if (deformToken.Has<CombineToken>() || deformToken.Has<NumerateToken>())
            {
                if (sender is Product)
                {
                    yield return MatrixElementMultiplicationRule.Entity;
                    yield return ScalarMultiplicationRule.Entity;
                }
                else if (sender is ProductOfMatrix)
                {
                    yield return MatrixMultiplicationRule.Entity;
                    yield return new CombineProductOfMatrixToPowerRule();
                }
                else if (sender is PowerOfMatrix)
                {
                    yield return ExpandNumericPowerOfMatrixRule.Entity;
                    yield return new FactorOutMinusOneOfExponentOfMatrixRule();
                }
                else if (sender is Sum)
                {
                    yield return MatrixAdditionRule.Entity;
                }
            }
        }


        /// <summary>
        /// この行列が正方行列か否かを取得します。
        /// </summary>
        public bool IsSquareMatrix { get { return RowSize == ColumnSize; } }

        /// <summary>
        /// マトリクスが対角行列か否かを取得します。
        /// </summary>
        public bool IsDiagonal
        {
            get
            {
                if (!IsSquareMatrix) return false;
                return AreAllZeroOn((r, c) => (r != c));
            }
        }

        /// <summary>
        /// マトリクスが上三角行列か否かを取得します。
        /// </summary>
        public bool IsUpperTriangular
        {
            get
            {
                if (!IsSquareMatrix) return false;
                return AreAllZeroOn((r, c) => (r > c));
            }
        }

        /// <summary>
        /// マトリクスが下三角行列か否かを取得します。
        /// </summary>
        public bool IsLowerTriangular
        {
            get
            {
                if (!IsSquareMatrix) return false;
                return AreAllZeroOn((r, c) => (r < c));
            }
        }

        private delegate bool IsCheckTargetElement(int row, int column);

        /// <summary>
        /// マトリクス内の指定要素が、すべて0であるか否かを取得します。
        /// </summary>
        /// <param name="isTarget">検査対象要素の判定メソッド。</param>
        /// <returns>検査対象がすべて0だったか否か。</returns>
        private bool AreAllZeroOn(IsCheckTargetElement isTarget)
        {
            for (int r = 1; r <= RowSize; r++)
            {
                for (int c = 1; c <= ColumnSize; c++)
                {
                    if (!isTarget(r, c)) continue;
                    if (this[r, c] != 0) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// マトリクスが対称行列か否かを取得します。
        /// </summary>
        public bool IsSymmetric
        {
            get
            {
                for (int r = 1; r <= RowSize; r++)
                    for (int c = r + 1; c <= ColumnSize; c++)
                        if (this[r, c] != this[c, r]) return false;

                return true;
            }
        }


        /// <summary>
        /// 転置行列を取得します。
        /// </summary>
        /// <returns>転置行列。</returns>
        public Matrix Transposed()
        {
            Matrix t = new Matrix(ColumnSize, RowSize);
            for (int r = 1; r <= RowSize; r++)
                for (int c = 1; c <= ColumnSize; c++)
                    t[c, r] = this[r, c];
            return t;
        }


    }
}
