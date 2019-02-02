// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Matrices;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Extensions
{
    /*
     * 参考資料
     * http://msdn.microsoft.com/ja-jp/magazine/jj863137.aspx
     *
     */
    /// <summary>
    /// 行列代数に関する処理を提供します。
    /// </summary>
    public static class MatrixAlgebra
    {
        /// <summary>
        /// 行列式を取得します。
        /// </summary>
        /// <param name="m">行列式を求める対象のマトリクス。</param>
        /// <returns>行列式。</returns>
        public static Formula Determinant(this Matrix m)
        {
            if (!(m.IsSquareMatrix)) throw new FormulaProcessException(m.ToString() + "は正方行列でないため、行列式を求めることはできません。");

            int toggle = 1;
            var lu = m.DecomposeLU(out toggle);

            Formula result = toggle;
            for (int i = 1; i <= m.RowSize; i++) result *= lu[i, i];

            return result.Simplify();
        }

        /// <summary>
        /// <paramref name="row"/>行、<paramref name="column"/>列の余因子を取得します。
        /// </summary>
        /// <param name="row">対象の行。</param>
        /// <param name="column">対象の列。</param>
        /// <returns>余因子。</returns>
        public static Formula Cofactor(this Matrix m, int row, int column)
        {
            if (!(m.IsSquareMatrix)) throw new FormulaProcessException(m.ToString() + "は正方行列でないため、余因子を求めることはできません。");

            int rAdd = 0, cAdd = 0;
            Matrix n = new Matrix(m.RowSize - 1, m.ColumnSize - 1);
            for (int r = 1; r <= m.RowSize; r++)
            {
                if (r == row) continue;
                if (r > row) rAdd = 1;

                for (int c = 1; c <= m.ColumnSize; c++)
                {
                    if (c == column) continue;
                    cAdd = (c > column) ? 1 : 0;
                    
                    n[r - rAdd, c - cAdd] = m[r, c];
                }
            }

            Formula det = n.Determinant();
            if ((row + column) % 2 == 1) det *= -1;

            return det.Simplify();
        }

        /// <summary>
        /// 逆行列を求めます。
        /// </summary>
        /// <param name="m">逆行列の算出対象とする行列。</param>
        /// <returns>逆行列。</returns>
        public static Matrix Inverse(this Matrix m)
        {
            int n = m.RowSize;
            var result = new Matrix(m);
            var swap = new List<int>();
            int toggle;
            var lu = m.DecomposeLU(out swap, out toggle);

            Vector b = new Vector(false, n);
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (i == swap[j - 1]) b[j] = 1;
                    else b[j] = 0;
                }
                var x = SolveVectorLU(lu, b);
                
                for (int j = 1; j <= n; j++) result[j, i] = x[j];
            }
            return result;
        }

        /// <summary>
        /// <paramref name="A"/> * x = <paramref name="B"/> を満たす列ベクトルxを求めます。
        /// </summary>
        /// <param name="A">係数マトリクス。</param>
        /// <param name="B">右辺の列ベクトル。</param>
        /// <returns><paramref name="A"/> * x = <paramref name="B"/>となる列ベクトルx。</returns>
        public static Vector SolveVector(Matrix A, Vector B)
        {
            int n = A.RowSize;
            int toggle;
            var swap = new List<int>();
            var lu = A.DecomposeLU(out swap, out toggle);

            Vector swapedB = new Vector(B);
            for (int i = 1; i <= n; i++) swapedB[i] = B[swap[i - 1]];
            
            return SolveVectorLU(lu, swapedB);
        }

        /// <summary>
        /// <paramref name="lu"/> * x = <paramref name="b"/> となる列ベクトルxを求めます。
        /// </summary>
        /// <param name="lu">対角要素を除くした三角形部分をL、対角要素を含む上三角形部分をUとしたLU分解行列。</param>
        /// <param name="b">列ベクトル。</param>
        /// <returns><paramref name="lu"/> * x = <paramref name="b"/>となる列ベクトルx。</returns>
        static Vector SolveVectorLU(Matrix lu, Vector b)
        {
            int n = lu.RowSize;
            Vector x = new Vector(b);

            // 下三角行列に対する前進代入
            for (int i = 2; i <= n; i++)
            {
                Formula sum = x[i];
                for (int j = 1; j < i; j++) sum -= lu[i, j] * x[j];
                x[i] = sum.Simplify();
            }
            x[n] = (x[n] / lu[n, n]).Simplify();
            
            // 上三角行列に対する後退代入
            for (int i = n - 1; i >= 1; i--)
            {
                Formula sum = x[i];
                for (int j = i + 1; j <= n; j++) sum -= lu[i, j] * x[j];
                x[i] = (sum / lu[i, i]).Simplify();
            }

            return x;
        }

        /// <summary>
        /// ドゥーリトルの方法により、LU分解します。
        /// </summary>
        /// <param name="A">対象のマトリクス。</param>
        /// <param name="L">分解済みの下三角行列(対角項を1とする)。</param>
        /// <param name="U">分解済みの上三角行列。</param>
        /// <returns>ピボット選択により変更された行の順序。</returns>
        public static List<int> DecomposeLU(this Matrix A, out Matrix L, out Matrix U)
        {
            var swap = new List<int>();
            int toggle;
            var result = A.DecomposeLU(out swap, out toggle);

            int n = result.RowSize;
            L = new Matrix(n, n);
            U = new Matrix(n, n);
            for (int r = 1; r <= n; r++)
            {
                for (int c = 1; c <= n; c++)
                {
                    if (r > c) L[r, c] = result[r, c];
                    else U[r, c] = result[r, c];

                    if (r == c) L[r, c] = 1;
                }
            }
            return swap;
        }

        /// <summary>
        /// ドゥーリトルの方法により、LU分解を実行します。
        /// </summary>
        /// <param name="A">対象のマトリクス。</param>
        /// <returns>LU分解の結果のマトリクス。対角要素を除く下三角形部分がL、対角要素を含む上三角形部分がU。</returns>
        public static Matrix DecomposeLU(this Matrix A)
        {
            int toggle;
            var swap = new List<int>();
            return A.DecomposeLU(out swap, out toggle);
        }

        /// <summary>
        /// ドゥーリトルの方法により、LU分解を実行します。
        /// </summary>
        /// <param name="A">対象のマトリクス。</param>
        /// <param name="toggle">行列式の符号。</param>
        /// <returns>LU分解の結果のマトリクス。対角要素を除く下三角形部分がL、対角要素を含む上三角形部分がU。</returns>
        public static Matrix DecomposeLU(this Matrix A, out int toggle)
        {
            var swap = new List<int>();
            return A.DecomposeLU(out swap, out toggle);
        }

        /// <summary>
        /// ドゥーリトルの方法により、LU分解を実行します。
        /// </summary>
        /// <param name="A">対象のマトリクス。</param>
        /// <param name="swap">ピボット選択による列順序の変更リスト。</param>
        /// <param name="toggle">行列式の符号。</param>
        /// <returns>LU分解の結果のマトリクス。対角要素を除く下三角形部分がL、対角要素を含む上三角形部分がU。</returns>
        public static Matrix DecomposeLU(this Matrix A, out List<int> swap, out int toggle)
        {
            if (!(A.IsSquareMatrix)) throw new FormulaProcessException(A.ToString() + "は正方行列でないため、LU分解をすることはできません。");

            int n = A.RowSize; // 行列サイズ
            Matrix result = new Matrix(A); // LとUの両方を保持

            toggle = 1;
            swap = new List<int>();
            for (int i = 1; i <= n; i++) swap.Add(i);

            for (int j = 1; j <= n; j++) // j:分解対象の列 LとUのj列目を計算する
            {
                // ピボット選択
                int pRow = result.SelectPivot(j, n);
                if (pRow != j) // swap情報を記録
                {
                    result.SwapRow(j, pRow);

                    int tmp = swap[pRow];
                    swap[pRow] = swap[j];
                    swap[j] = tmp;

                    toggle = -toggle;
                }

                if (result[j, j] == 0) throw new FormulaProcessException(A.ToString() + "は、LU分解できませんでした。");

                for (int i = j + 1; i <= n; i++)
                {
                    result[i, j] = (result[i, j] / result[j, j]).Simplify();

                    for (int k = j + 1; k <= n; k++)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                        result[i, k] = result[i, k].Simplify();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 指定行列の<paramref name="j"/>列目の対角要素の列番号について、ピボット選択します。
        /// </summary>
        /// <param name="A">対象の正方行列。</param>
        /// <param name="j">対象の列番号。</param>
        /// <param name="n">対象行列の行サイズ。</param>
        /// <returns>ピボット対象の列番号。</returns>
        static int SelectPivot(this Matrix A, int j, int n)
        {
            int pRow = j;
            double colMax = 0;
            var current = A[j, j];

            for (int i = j; i <= n; i++)
            {
                if (A[i, j] is Numeric)
                {
                    var num = A[i, j] as Numeric;
                    if (Math.Abs(num.Figure) > colMax)
                    {
                        colMax = Math.Abs(num.Figure);
                        pRow = j;
                        current = A[i, j];
                    }
                }
                else if (current == 0 && colMax == 0 && A[i, j] != 0)
                {
                    pRow = j;
                    current = A[i, j];
                }
            }
            return pRow;
        }

        /// <summary>
        /// 行列の行を入れ替えます。
        /// </summary>
        /// <param name="A">対象の行列。</param>
        /// <param name="r1">入れ替え対象の行番号1。</param>
        /// <param name="r2">入れ替え対象の行番号2。</param>
        static void SwapRow(this Matrix A, int r1, int r2)
        {
            for (int c = 1; c <= A.ColumnSize; c++)
            {
                var tmp = A[r1, c];
                A[r1, c] = A[r2, c];
                A[r2, c] = tmp;
            }
        }


    }
}
