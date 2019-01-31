// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Linq;
using System.Collections.Generic;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Parse;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Formulas
{    
    /// <summary>
    /// 数式を表します。
    /// </summary>
    /// <remarks>
    /// TODO: MathML構成、解析の抽象メソッド
    /// TODO: TEX構成、解析の抽象メソッド
    /// </remarks>
    [Serializable()]
    public abstract partial class Formula : IComparable<Formula>, IEnumerable<Formula>
    {
        static Formula()
        {
            // 括弧は常に書式に考慮する
            Format.RegistPermanentApply<Bracket>();

            // 既定の変形トークンを準備
            CombineToken = new CombineToken();
            ExpandToken = new ExpandToken();
            NumerateToken = new NumerateToken();
            CalculateToken = new CalculateToken();
            SimplifyToken = new SimplifyToken();

            // 既定の構文解析オブジェクトを準備
            DefaultFormulaParser = new FormulaParser();
            DefaultFormulaParser.Lexers.Add(new NumericLexer()); // 数値
            DefaultFormulaParser.Lexers.Add(new ParenthesesLexer()); // 丸括弧
            DefaultFormulaParser.Lexers.Add(new BracketLexer(true, true)); // 角括弧
            DefaultFormulaParser.Lexers.Add(new BraceLexer()); // 波括弧
            DefaultFormulaParser.Lexers.Add(new AbsolutePunctuationLexer()); // 絶対値記号
            DefaultFormulaParser.Lexers.Add(new SpaceLexer()); // 空白トークン
            DefaultFormulaParser.Lexers.Add(new FunctionLexer(DefaultFormulaParser, true)); // 関数
            DefaultFormulaParser.Lexers.Add(new ConstantLexer(true)); // 定数
            DefaultFormulaParser.Lexers.Add(new VariableLexer(DefaultFormulaParser)); // 変数

            DefaultFormulaParser.AddOperatorParsers(new PowerOperatorParser()); // 累乗
            DefaultFormulaParser.AddOperatorParsers(new PowerOfMatrixOperatorParser()); // 行列の累乗
            DefaultFormulaParser.AddOperatorParsers(new PlusMinusOperatorParser()); // 正負記号
            DefaultFormulaParser.AddOperatorParsers(new AbbreviatedProductOperatorParser()); // 積算記号の省略を許可(通常の積算より優先度を上げる)
            DefaultFormulaParser.AddOperatorParsers(new ProductOfMatrixOperatorParser()); // 行列の乗算
            DefaultFormulaParser.AddOperatorParsers(new ProductOperatorParser()); // 乗算・除算
            DefaultFormulaParser.AddOperatorParsers(new SumOperatorParser()); // 和算・減算
            DefaultFormulaParser.AddOperatorParsers(new FactorialOperatorParser()); // 階乗記号(!)
            DefaultFormulaParser.AddOperatorParsers(new ComparerOperatorParser()); // 比較演算子
            DefaultFormulaParser.AddOperatorParsers(new BooleanOperatorParser()); // 論理演算子
            DefaultFormulaParser.AddOperatorParsers(new ArgumentOperatorParser()); // 引数演算子
        }

        /// <summary>
        /// <see cref="Parse"/>メソッドで使用する既定の数式構文解析オブジェクトを設定もしくは取得します。
        /// </summary>
        public static FormulaParser DefaultFormulaParser { get; set; }

        /// <summary>
        /// 指定した数式の文字列を数式に変換します。
        /// </summary>
        /// <param name="formula">数式に変換する文字列。</param>
        /// <returns>変換された数式。</returns>
        public static Formula Parse(string s)
        {
            return DefaultFormulaParser.Parse(s);
        }

        /// <summary>
        /// 指定文字列が数式に変換できるか否かを取得します。
        /// </summary>
        /// <param name="s">対象文字列。</param>
        /// <param name="f">変換に成功した場合、変換後の数式。</param>
        /// <returns>数式に変換可能か否か。</returns>
        public static bool TryParse(string s, out Formula f)
        {
            f = null;
            try
            {
                f = Parse(s);
                return true;
            }
            catch { return false; }
        }


        /// <summary>
        /// Combineメソッドで使用される変形識別トークンを設定もしくは取得します。
        /// </summary>
        public static DeformToken CombineToken { get; set; }

        /// <summary>
        /// Expandメソッドで使用される変形識別トークンを設定もしくは取得します。
        /// </summary>
        public static DeformToken ExpandToken { get; set; }

        /// <summary>
        /// Numerateメソッドで使用される変形識別トークンを設定もしくは取得します。
        /// </summary>
        public static DeformToken NumerateToken { get; set; }

        /// <summary>
        /// Calculateメソッドで使用される変形識別トークンを設定もしくは取得します。
        /// </summary>
        public static DeformToken CalculateToken { get; set; }

        /// <summary>
        /// Simplifyメソッドで使用される変形識別トークンを設定もしくは取得します。
        /// </summary>
        public static DeformToken SimplifyToken { get; set; }


        Format _format;

        /// <summary>
        /// 数式を初期化します。
        /// </summary>
        public Formula() { }

        /// <summary>
        /// 可変数の数式を指定して、子数式を有する数式を初期化します。
        /// </summary>
        /// <param name="fs">数式を構成する可変数の子数式。</param>
        /// <returns>初期化された数式。</returns>
        public virtual Formula CreateFromChildren(params Formula[] fs)
        {
            throw new NotImplementedException("子数式を有する数式では、CreateFromChildrenメソッドをオーバーライドする必要があります。");
        }

        #region プロパティ

        /// <summary>
        /// 数式の書式設定を設定もしくは取得します。
        /// </summary>
        public Format Format
        {
            get 
            {
                if (_format == null) 
                {
                    _format = new Format();
                }
                _format.Target = this;
                return _format;
            }
            set
            {
                if (_format != null) _format.Target = null;

                if (value == null)
                {
                    _format = null;
                    return;
                }
                _format = Clone.GetClone<Format>(value);
            }
        }

        /// <summary>
        /// 初期化された書式設定を保持するか否かを取得します。
        /// </summary>
        internal bool HasFormat { get { return _format != null ; } }

        /// <summary>
        /// この数式に対して最後に適用された変形を取得します。
        /// </summary>
        public DeformToken LastDeformToken { get; internal set; }

        #endregion
        
        #region Event関連

        /// <summary>
        /// 処理中の確認動作です。数式の処理中に、定期的に呼び出されます。
        /// </summary>
        public static event FormulaProcessEventHandler FormulaProcessing;

        /// <summary>
        /// ユーザーにより数式処理が中止されたときに呼び出されます。
        /// </summary>
        public static event EventHandler FormulaProcessCanceled;

        /// <summary>
        /// ユーザーから処理の中止命令が出ているかを確認します。
        /// </summary>
        /// <param name="f">処理中の数式。</param>
        /// <exception cref="FormulaOperationCanceledExceptions">ユーザーが処理をキャンセルした場合に投げられます。</exception>
        internal static void CheckCancelOperation(Formula f)
        {
            bool stop = false;

            if (FormulaProcessing != null)
                FormulaProcessing(f, EventArgs.Empty, ref stop);

            if (stop)
            {
                if (FormulaProcessCanceled != null) FormulaProcessCanceled(f, EventArgs.Empty);
                throw new FormulaOperationCanceledExceptions();
            }
        }

        #endregion

        #region デリゲート
        
        /// <summary>
        /// 数式に対する変形処理メソッドを表します。
        /// </summary>
        /// <param name="f">処理対象の数式。</param>
        /// <returns>処理後の数式。</returns>
        public delegate Formula DeformFormulas(Formula f);
        
        /// <summary>
        /// 指定数式が対象となる数式か否かを返すメソッドを表します。
        /// </summary>
        /// <param name="f">対象数式。</param>
        /// <returns>対象と判定されたか否か。</returns>
        public delegate bool IsTargetFormula(Formula f);
        
        #endregion

        #region 数式処理

        /// <summary>
        /// 同一項の和算を単一項に変換することで、数式を簡単化して取得します。
        /// </summary>
        /// <returns>変形後の数式。</returns>
        public Formula Combine() { return DeformFormula(CombineToken); }

        /// <summary>
        /// 数式を、展開（展開定理の適用）して取得します。
        /// </summary>
        /// <returns>展開後の数式。</returns>
        public Formula Expand() { return DeformFormula(ExpandToken); }

        /// <summary>
        /// 数式内の関数を計算して取得します。
        /// </summary>
        /// <returns>関数計算後の数式。</returns>
        public Formula Calculate() { return DeformFormula(CalculateToken); }

        /// <summary>
        /// 数式内の変数・定数を数値化します。
        /// </summary>
        /// <returns>数値化後の数式。</returns>
        public Formula Numerate() { return DeformFormula(NumerateToken); }

        /// <summary>
        /// 数式の展開、整理を行い、簡単化します。
        /// </summary>
        /// <returns>簡単化後の数式。</returns>
        public Formula Simplify() { return DeformFormula(SimplifyToken); }
        

        /// <summary>
        /// 変形識別トークンを指定して、数式の変形処理を実行します。
        /// </summary>
        /// <param name="deformToken">変形処理識別トークン。</param>
        /// <returns>処理後の数式。</returns>
        public Formula DeformFormula(DeformToken deformToken) { return Deform.Apply(deformToken, this); }

        /// <summary>
        /// 変形識別トークンを指定して、数式の変形処理を実行します。
        /// </summary>
        /// <param name="deformToken">変形処理識別トークン。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>処理後の数式。</returns>
        public Formula DeformFormula(DeformToken deformToken, out DeformHistory history)
        {
            history = new DeformHistory(this);
            return Deform.Apply(deformToken, this, history, null);
        }

        /// <summary>
        /// 指定処理に関連する変形ルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public virtual IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            if (object.ReferenceEquals(sender, this) && this[0] != null) yield return new DeformChildrenRule(deformToken, history.CurrentNode);
        }

        /// <summary>
        /// 指定した可変数の変形ルールに基づいて変形します。
        /// </summary>
        /// <param name="autoSort">ルール自体に設定されたソート順に基づいて自動ソートを行うか否かを指定します。</param>
        /// <param name="rules">適用する可変数のルール。</param>
        /// <returns>変形後の数式。</returns>
        public Formula DeformWithRules(bool autoSort, params Rule[] rules)
        {
            CheckCancelOperation(this);

            var ruleList = new List<Rule>(rules);
            if (autoSort) Utilities.Sort.StableSort<Rule>(ruleList);

            int i = 0;
            while (this[i] != null)
            {
                this[i] = this[i].DeformWithRules(false, ruleList.ToArray());
                i++;
            }

            foreach (var rule in ruleList)
            {
                Formula result = this;
                if (rule.TryMatchRule(ref result)) return result.DeformWithRules(false, ruleList.ToArray());
            }
            return this;
        }


        /// <summary>
        /// 指定した数式に、新しい数式を代入します。
        /// </summary>
        /// <param name="oldValue">代入対象とする数式。</param>
        /// <param name="newValue">代入する数式。</param>
        /// <returns>代入操作後の数式。</returns>
        internal protected Formula Substitute(Formula oldValue, Formula newValue) { return Substitute(oldValue, newValue, true); }

        /// <summary>
        /// 指定した数式に、新しい数式を代入します。
        /// </summary>
        /// <param name="oldValue">代入対象とする数式。</param>
        /// <param name="newValue">代入する数式。</param>
        /// <param name="formatReplace">書式も置き換えるか。</param>
        /// <returns>代入操作後の数式。</returns>
        internal protected Formula Substitute(Formula oldValue, Formula newValue, bool formatReplace)
        {
            Formula result = null;
            if (oldValue == newValue) result = this;
            else if (this == oldValue) result = newValue;

            if (result != null)
            {
                if (!formatReplace) result.Format = oldValue.Format;
                return result;
            }

            CheckCancelOperation(this);

            return OnSubstitute(oldValue, newValue, formatReplace);
        }

        /// <summary>
        /// 指定した数式に、新しい数式を代入します。
        /// </summary>
        /// <param name="oldValue">代入対象とする数式。</param>
        /// <param name="newValue">代入する数式。</param>
        /// <param name="formatReplace">書式も置き換えるか。</param>
        /// <returns>代入操作後の数式。</returns>
        protected virtual Formula OnSubstitute(Formula oldValue, Formula newValue, bool formatReplace)
        {
            int index = 0;
            while (this[index] != null)
            {
                this[index] = this[index].Substitute(oldValue, newValue, formatReplace);
                index++;
            }
            return this;
        }

        /// <summary>
        /// 指定した数式に、新しい数式を代入して取得します。
        /// </summary>
        /// <param name="oldValue">代入対象の数式。</param>
        /// <param name="newValue">代入する数式。</param>
        /// <returns>代入操作後の数式。</returns>
        public Formula Substituted(Formula oldValue, Formula newValue) { return Substituted(oldValue, newValue, true); }

        /// <summary>
        /// 指定した数式に、新しい数式を代入して取得します。
        /// </summary>
        /// <param name="oldValue">代入対象の数式。</param>
        /// <param name="newValue">代入する数式。</param>
        /// <param name="formatReplace">書式も置き換えるか。</param>
        /// <returns>代入操作後の数式。</returns>
        public Formula Substituted(Formula oldValue, Formula newValue, bool formatReplace)
        {
            return Copy().Substitute(oldValue, newValue, formatReplace);
        }

        /// <summary>
        /// 指定した数式に、新しい数式を代入して取得します。
        /// </summary>
        /// <param name="values">代入対象と代入数式を表す可変数の等式。</param>
        /// <returns>代入操作後の数式。</returns>
        public Formula Substituted(params Equal[] values) { return Substituted(true, values); }

        /// <summary>
        /// 指定した数式に、新しい数式を代入して取得します。
        /// </summary>
        /// <param name="formatReplace">書式も置き換えるか。</param>
        /// <param name="values">代入対象と代入数式を表す可変数の等式。</param>
        /// <returns>代入操作後の数式。</returns>
        public Formula Substituted(bool formatReplace, params Equal[] values) 
        {
            var result = Copy();
            foreach (var value in values)
                result = result.Substitute(value.LeftHandSide, value.RightHandSide, formatReplace);
            return result;
        }

        /// <summary>
        /// 構成要素を並べ替えます。
        /// </summary>
        public void Sort() { foreach (Formula f in this) f.Sort(); OnSort(); }
        
        /// <summary>
        /// 構成要素を並び替えます。
        /// </summary>
        protected virtual void OnSort() { }
        
        #endregion

        #region 操作

        /// <summary>
        /// 数式中の指定要素一覧を取得します。
        /// </summary>
        /// <typeparam name="T">探索対象要素の型。</typeparam>
        /// <returns>指定タイプの数式リスト。</returns>
        public IEnumerable<T> GetExistFactors<T>() where T : Formula
        {
            List<T> already = new List<T>();
            foreach (T f in onGetExistFactors<T>(already)) yield return f;
        }

        /// <summary>
        /// 数式中の指定要素一覧を取得します。
        /// </summary>
        /// <typeparam name="T">探索対象要素の型。</typeparam>
        /// <param name="already">既に返した数式要素。</param>
        /// <returns>指定タイプの数式リスト。</returns>
        private IEnumerable<T> onGetExistFactors<T>(List<T> already) where T : Formula
        {
            if (this is T && !already.Contains(this as T))
            {
                yield return (this as T);
                already.Add(this as T);
            }

            foreach (Formula f in this)
            {
                foreach (T f_ in f.onGetExistFactors<T>(already))
                    yield return f_;
            }
        }

        /// <summary>
        /// 数式中の条件に見合う全ての数式を取得します。
        /// </summary>
        /// <param name="isTarget">判定デリゲート。</param>
        /// <returns>指定条件に一致する数式リスト。</returns>
        public IEnumerable<Formula> GetExistFactors(IsTargetFormula isTarget)
        {
            List<Formula> already = new List<Formula>();
            foreach (var f in onGetExistFactors(isTarget, already)) yield return f;
        }

        /// <summary>
        /// 数式中の指定要素一覧を取得します。
        /// </summary>
        /// <typeparam name="T">探索対象要素の型。</typeparam>
        /// <param name="isTarget">判定デリゲート。</param>
        /// <param name="already">既に返した数式要素。</param>
        /// <returns>指定タイプの数式リスト。</returns>
        private IEnumerable<Formula> onGetExistFactors(IsTargetFormula isTarget, List<Formula> already)
        {
            if (isTarget(this) && !already.Contains(this))
            {
                yield return (this);
                already.Add(this);
            }

            foreach (Formula f in this)
            {
                foreach (Formula f_ in f.onGetExistFactors(isTarget, already))
                    yield return f_;
            }
        }


        /// <summary>
        /// 数式中に指定数式が含まれるか否かを取得します。
        /// </summary>
        /// <param name="f">探索対象とする数式。</param>
        /// <returns>判定結果。</returns>
        public bool Contains(Formula f) { return GetExistFactors((check) => f == check).Any(f_ => true); }

        /// <summary>
        /// 数式中に指定要素が含まれるか否かを取得します。
        /// </summary>
        /// <typeparam name="T">探索対象要素の型。</typeparam>
        /// <returns>判定結果。</returns>
        public bool Contains<T>() where T : Formula { return GetExistFactors<T>().Any(f => true); }

        /// <summary>
        /// 指定条件に見合う数式が、数式中に存在するか否かを取得します。
        /// </summary>
        /// <param name="isTarget">判定デリゲート。</param>
        /// <returns>判定結果。</returns>
        public bool Contains(IsTargetFormula isTarget) { return GetExistFactors(isTarget).Any(f => true); }

        /// <summary>
        /// リスト内の重複する要素を削除します。
        /// </summary>
        /// <param name="target">対象リスト。</param>
        private void SieveDouble<T>(List<T> target) where T : Formula
        {
            for (int i = 0; i < target.Count; i++)
                for (int k = i + 1; k < target.Count; k++)
                    if (target[i] == target[k])
                    {
                        target.RemoveAt(k);
                        k--;
                    }
        }


        /// <summary>
        /// 指定したルールパターンに一致するか否かを取得します。
        /// </summary>
        /// <param name="rule">判定対象のルールパターン数式。</param>
        /// <returns>ルールパターン数式に一致するか否か。</returns>
        public bool PatternMatch(Formula rule) { return rule.IsPatternRuleMatchWith(this); }

        /// <summary>
        /// 指定したルールパターンに一致するか否かを取得します。
        /// </summary>
        /// <param name="rule">判定対象のルールパターン数式。</param>
        /// <param name="clearMatched">一致済みのルール変数の記録変数を削除するか。</param>
        /// <returns>ルールパターン数式に一致するか否か。</returns>
        public bool PatternMatch(Formula rule, bool clearMatched) { return rule.IsPatternRuleMatchWith(this, clearMatched); }

        /// <summary>
        /// この数式が、指定数式で一致するルールパターン数式か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>指定数式に一致するルールパターン数式か否か。</returns>
        internal bool IsPatternRuleMatchWith(Formula f) { return IsPatternRuleMatchWith(f, true); }

        /// <summary>
        /// この数式が、指定数式で一致するルールパターン数式か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <param name="clearMatched">一致済みのルール変数の記録変数を削除するか。</param>
        /// <returns>指定数式に一致するルールパターン数式か否か。</returns>
        internal bool IsPatternRuleMatchWith(Formula f, bool clearMatched)
        {
            // チェック前の一致数式
            Dictionary<RulePatternVariable, Formula> cachePre = new Dictionary<RulePatternVariable, Formula>();
            if (!clearMatched)
            {
                foreach (RulePatternVariable r in GetExistFactors<RulePatternVariable>())
                    if (r.MatchedFormula != null) cachePre.Add(r, r.MatchedFormula);
            }
            else
            {
                foreach (RulePatternVariable r in GetExistFactors<RulePatternVariable>()) r.ClearMatchedFormula();
            }

            // 未設定のルール変数を記録
            List<RulePatternVariable> unsetRuleVariables = new List<RulePatternVariable>();
            foreach (RulePatternVariable r in GetExistFactors<RulePatternVariable>())
                if (r.MatchedFormula == null) unsetRuleVariables.Add(r);

            bool check = OnCheckPatternMatch(f);

            if (!check) // 一致しなかったら一致数式をクリア、復元
            {
                foreach (RulePatternVariable r in unsetRuleVariables) r.ClearMatchedFormula();
                foreach (RulePatternVariable r in cachePre.Keys) r.MatchedFormula = cachePre[r];
            }
#if DEBUG
            else
                foreach (RulePatternVariable r in unsetRuleVariables) FormulaAssertionException.Assert(r.MatchedFormula != null);
#endif
            return check;
        }
        
        /// <summary>
        /// この数式が、指定数式で一致するルールパターン数式か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>指定数式に一致するルールパターン数式か否か。</returns>
        protected virtual bool OnCheckPatternMatch(Formula f)
        {
            if (this[0] == null) return this == f;
            else
            {
                if (this.GetEqualBaseType() != f.GetEqualBaseType()) return false;
                int index = 0;
                while (this[index] != null)
                {
                    if (!this[index].OnCheckPatternMatch(f[index])) return false;
                    index++;
                }
                if (f[index] != null) return false;

                return true;
            }
        }

        
        /// <summary>
        /// 数式のコピーを作成します。
        /// </summary>
        /// <returns>ディープコピーされた数式。</returns>
        public Formula Copy() 
        {
            var copy = Clone.GetClone<Formula>(this);
            copy.Format.Target = copy;
            return copy;
        }

        /// <summary>
        /// 数式を構成する数式を設定もしくは取得します。
        /// </summary>
        /// <param name="i">0から始まる要素番号。番号に対応する構成要素は、数式ごとに異なります。該当数式が存在しない場合、nullを返します。</param>
        /// <returns>指定要素番号の数式。</returns>
        public virtual Formula this[int i] { get { return null; } set { } }
        
        /// <summary>
        /// 数式を認識可能な文字列に変換して取得します。
        /// </summary>
        /// <returns>数式を表す文字列。</returns>
        public override string ToString() 
        {
            foreach (var child in this) child.Format.Parent = Format;

            string text = GetText();
            text = Format.ModifyOutputText(text);

            foreach (var child in this) child.Format.Parent = null;
            return text;
        }

        /// <summary>
        /// 数式を認識可能な文字列に変換して取得します。
        /// </summary>
        /// <returns>数式を表す文字列。</returns>
        public abstract string GetText();

        /// <summary>
        /// 数式の一意性評価に用いる文字列を取得します。
        /// </summary>
        /// <returns>数式を一意に区別する文字列。</returns>
        public string GetUniqueText()
        {
            return string.Format("{0}({1})", GetEqualBaseType().Name, OnGetUniqueText());
        }

        /// <summary>
        /// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
        /// </summary>
        /// <returns>数式を一意に区別する文字列。</returns>
        protected abstract string OnGetUniqueText();

        /// <summary>
        /// 数式の一意性評価時に参照すべき、この数式の基本となるタイプを取得します。
        /// </summary>
        /// <returns>一意性評価に用いるタイプ。</returns>
        /// <remarks>
        /// 例えばsqrtクラスでは、rootクラスのタイプを返すべきです。
        /// </remarks>
        public virtual Type GetEqualBaseType() { return GetType(); }

        #endregion

    }

    /// <summary>
    /// 数式の処理の進行を表します。
    /// </summary>
    /// <param name="Cancel">処理を中止する場合、trueを返します。</param>
    public delegate void FormulaProcessEventHandler(Formula sender, EventArgs e, ref bool Cancel);
    

    
}
