using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Deforms.Rules
{        
    /// <summary>
    /// 数式パターンを用いた数式処理ルールを表します。
    /// </summary>
    [Serializable()]
    public abstract class PatternRule : Rule
    {
        Formula _rulePatternFormula;
        Dictionary<string, RulePatternVariable> _usingPatternVariables = new Dictionary<string, RulePatternVariable>();

        /// <summary>
        /// 処理ルールを初期化します。
        /// </summary>
        public PatternRule() { }    
        
        #region ルールパターン変数
        
        /// <summary>
        /// ルールパターン変数aを設定もしくは取得します。
        /// </summary>
        protected Formula a
        {
            get { return GetPatternVariable("a"); }
            set { SetPatternVariable("a", value); }
        }
        
        /// <summary>
        /// ルールパターン変数bを設定もしくは取得します。
        /// </summary>
        protected Formula b
        {
            get { return GetPatternVariable("b"); }
            set { SetPatternVariable("b", value); }
        }
        
        /// <summary>
        /// ルールパターン変数cを設定もしくは取得します。
        /// </summary>
        protected Formula c
        {
            get { return GetPatternVariable("c"); }
            set { SetPatternVariable("c", value); }
        }
        
        /// <summary>
        /// ルールパターン変数dを設定もしくは取得します。
        /// </summary>
        protected Formula d
        {
            get { return GetPatternVariable("d"); }
            set { SetPatternVariable("d", value); }
        }
        
        /// <summary>
        /// ルールパターン変数eを設定もしくは取得します。
        /// </summary>
        protected Formula e
        {
            get { return GetPatternVariable("e"); }
            set { SetPatternVariable("e", value); }
        }
        
        /// <summary>
        /// ルールパターン変数fを設定もしくは取得します。
        /// </summary>
        protected Formula f
        {
            get { return GetPatternVariable("f"); }
            set { SetPatternVariable("f", value); }
        }
        
        /// <summary>
        /// ルールパターン変数gを設定もしくは取得します。
        /// </summary>
        protected Formula g
        {
            get { return GetPatternVariable("g"); }
            set { SetPatternVariable("g", value); }
        }
        
        /// <summary>
        /// ルールパターン変数hを設定もしくは取得します。
        /// </summary>
        protected Formula h
        {
            get { return GetPatternVariable("h"); }
            set { SetPatternVariable("h", value); }
        }
        
        /// <summary>
        /// ルールパターン変数iを設定もしくは取得します。
        /// </summary>
        protected Formula i
        {
            get { return GetPatternVariable("i"); }
            set { SetPatternVariable("i", value); }
        }
        
        /// <summary>
        /// ルールパターン変数jを設定もしくは取得します。
        /// </summary>
        protected Formula j
        {
            get { return GetPatternVariable("j"); }
            set { SetPatternVariable("j", value); }
        }
        
        /// <summary>
        /// ルールパターン変数kを設定もしくは取得します。
        /// </summary>
        protected Formula k
        {
            get { return GetPatternVariable("k"); }
            set { SetPatternVariable("k", value); }
        }
        
        /// <summary>
        /// ルールパターン変数lを設定もしくは取得します。
        /// </summary>
        protected Formula l
        {
            get { return GetPatternVariable("l"); }
            set { SetPatternVariable("l", value); }
        }
        
        /// <summary>
        /// ルールパターン変数mを設定もしくは取得します。
        /// </summary>
        protected Formula m
        {
            get { return GetPatternVariable("m"); }
            set { SetPatternVariable("m", value); }
        }
        
        /// <summary>
        /// ルールパターン変数nを設定もしくは取得します。
        /// </summary>
        protected Formula n
        {
            get { return GetPatternVariable("n"); }
            set { SetPatternVariable("n", value); }
        }
        
        /// <summary>
        /// ルールパターン変数oを設定もしくは取得します。
        /// </summary>
        protected Formula o
        {
            get { return GetPatternVariable("o"); }
            set { SetPatternVariable("o", value); }
        }
        
        /// <summary>
        /// ルールパターン変数pを設定もしくは取得します。
        /// </summary>
        protected Formula p
        {
            get { return GetPatternVariable("p"); }
            set { SetPatternVariable("p", value); }
        }
        
        /// <summary>
        /// ルールパターン変数qを設定もしくは取得します。
        /// </summary>
        protected Formula q
        {
            get { return GetPatternVariable("q"); }
            set { SetPatternVariable("q", value); }
        }
        
        /// <summary>
        /// ルールパターン変数rを設定もしくは取得します。
        /// </summary>
        protected Formula r
        {
            get { return GetPatternVariable("r"); }
            set { SetPatternVariable("r", value); }
        }
        
        /// <summary>
        /// ルールパターン変数sを設定もしくは取得します。
        /// </summary>
        protected Formula s
        {
            get { return GetPatternVariable("s"); }
            set { SetPatternVariable("s", value); }
        }
        
        /// <summary>
        /// ルールパターン変数tを設定もしくは取得します。
        /// </summary>
        protected Formula t
        {
            get { return GetPatternVariable("t"); }
            set { SetPatternVariable("t", value); }
        }
        
        /// <summary>
        /// ルールパターン変数uを設定もしくは取得します。
        /// </summary>
        protected Formula u
        {
            get { return GetPatternVariable("u"); }
            set { SetPatternVariable("u", value); }
        }
        
        /// <summary>
        /// ルールパターン変数vを設定もしくは取得します。
        /// </summary>
        protected Formula v
        {
            get { return GetPatternVariable("v"); }
            set { SetPatternVariable("v", value); }
        }
        
        /// <summary>
        /// ルールパターン変数wを設定もしくは取得します。
        /// </summary>
        protected Formula w
        {
            get { return GetPatternVariable("w"); }
            set { SetPatternVariable("w", value); }
        }
        
        /// <summary>
        /// ルールパターン変数xを設定もしくは取得します。
        /// </summary>
        protected Formula x
        {
            get { return GetPatternVariable("x"); }
            set { SetPatternVariable("x", value); }
        }
        
        /// <summary>
        /// ルールパターン変数yを設定もしくは取得します。
        /// </summary>
        protected Formula y
        {
            get { return GetPatternVariable("y"); }
            set { SetPatternVariable("y", value); }
        }
        
        /// <summary>
        /// ルールパターン変数zを設定もしくは取得します。
        /// </summary>
        protected Formula z
        {
            get { return GetPatternVariable("z"); }
            set { SetPatternVariable("z", value); }
        }
        
        /// <summary>
        /// ルールパターン変数の一致数式記録を消去します。
        /// </summary>
        protected void ClearPatternVariable()
        {
            foreach (RulePatternVariable r in UsingPatternVariables.Values)
                r.ClearMatchedFormula();
        }
            
        #endregion
        
        #region 内部処理
        
        /// <summary>
        /// 指定名のルールパターン変数を取得します。
        /// </summary>
        protected Formula GetPatternVariable(string s)
        {
            if (!UsingPatternVariables.ContainsKey(s)) UsingPatternVariables.Add(s, new RulePatternVariable(s)) ;
                
            if (UsingPatternVariables[s].MatchedFormula != null) return UsingPatternVariables[s].MatchedFormula;
            return UsingPatternVariables[s];
        }
        
        /// <summary>
        /// 指定名のルールパターン変数を設定します。
        /// </summary>
        protected void SetPatternVariable(string s, Formula f)
        {
            if (UsingPatternVariables.ContainsKey(s))
            {
                if (f is RulePatternVariable)
                {
                    RulePatternVariable ruleVariable = f as RulePatternVariable;
                    if (ruleVariable.Mark != s) throw new FormulaAssertionException(string.Format("ルールパターン変数に指定された名前が、命名済みの名前と整合しません（{0} と {1}）。", s, ruleVariable.Mark));
                    UsingPatternVariables[s].MatchedFormula = ruleVariable.MatchedFormula;
                }
                else
                {
                    UsingPatternVariables[s].MatchedFormula = f;
                }
            }
            else
            {
                if (f is RulePatternVariable)
                {
                    RulePatternVariable ruleVariable = f as RulePatternVariable;
                    if (ruleVariable.Mark != s) throw new FormulaAssertionException(string.Format("ルールパターン変数に指定された名前が、命名済みの名前と整合しません（{0} と {1}）。", s, ruleVariable.Mark));
                    UsingPatternVariables.Add(s, ruleVariable);
                }
                else
                {
                    RulePatternVariable ruleVariable = new RulePatternVariable(s);
                    ruleVariable.MatchedFormula = f;
                    UsingPatternVariables.Add(s, ruleVariable);
                }
            }
        }

        #endregion

        #region プロパティ
        
        /// <summary>
        /// 現在のインスタンスで対象とするルールパターン数式変数マップを取得します。
        /// </summary>
        protected Dictionary<string, RulePatternVariable> UsingPatternVariables { get { return _usingPatternVariables; } } 

        /// <summary>
        /// 処理対象を表すルール数式を取得します。
        /// </summary>
        protected Formula RulePatternFormula
        {
            private set { _rulePatternFormula = null; }
            get
            {
                if (_rulePatternFormula == null)
                    _rulePatternFormula = GetRulePatternFormula();
                return _rulePatternFormula;
            }
        }
        
        #endregion
                
        #region 操作

        /// <summary>
        /// 処理対象を表すルール数式をリセットします。ルール独自のプロパティが変化して、対象となる数式に変更があった場合には、本メソッドを呼び出して下さい。
        /// </summary>
        protected virtual void ResetRulePatternFormula() { RulePatternFormula = null; }
        
        /// <summary>
        /// 処理対象を表すルールパターン数式を取得します。
        /// </summary>
        protected abstract Formula GetRulePatternFormula();
                
        /// <summary>
        /// 処理後のルールパターン数式を取得します。
        /// </summary>
        /// <returns>処理後のルールパターン数式。付加条件に一致しない場合、null。</returns>
        protected abstract Formula GetRuledFormula();

        /// <summary>
        /// 処理ルールに従って数式に対する変換処理を試みます。
        /// </summary>
        /// <param name="target">処理対象の数式</param>
        /// <returns>変形後の数式。変形のない場合、null。</returns>
        protected override Formula OnTryMatchRule(Formula target)
        {
            bool hit = target.PatternMatch(RulePatternFormula);
            if (!hit) return null;
#if DEBUG
            foreach (RulePatternVariable r in UsingPatternVariables.Values)
                FormulaAssertionException.Assert(r.MatchedFormula != null);
#endif
            Formula ruled = GetRuledFormula();
#if DEBUG
            if (ruled != null) FormulaAssertionException.Assert(ruled.GetExistFactors<RulePatternVariable>().Count() == 0);
#endif                
            return ruled;
        }

        #endregion


    }
}
