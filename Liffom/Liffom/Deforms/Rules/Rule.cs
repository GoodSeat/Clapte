using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Deforms.Rules
{        
    /// <summary>
    /// 数式の形状に応じた、適用可能な数式の変形処理を規定するルールを表します。
    /// このクラスから派生するクラスでは、既定のコンストラクタを実装してください。
    /// </summary>
    [Serializable()]
    public abstract class Rule : IComparable<Rule>
    {
        static Dictionary<Type, Rule> s_ruleMap;
        static List<Reflector<Rule>> s_cacheRuleReflectors;

        /// <summary>
        /// アセンブリと同ディレクトリ内のライブラリで検知されるすべてのルールのリフレクタを返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Reflector<Rule>> GetAllRuleReflectors()
        {
            if (s_cacheRuleReflectors == null) s_cacheRuleReflectors = new List<Reflector<Rule>>(Reflector<Rule>.FindReflectors());
            foreach (Reflector<Rule> rule in s_cacheRuleReflectors) yield return rule;
        }

        static object s_lock = new object();
        static object s_lockSort = new object();

        /// <summary>
        /// 指定した型の処理ルールを取得します。
        /// </summary>
        /// <param name="ruleType">取得対象の型</param>
        /// <returns>取得された処理</returns>
        private static Rule GetRuleOf(Type ruleType)
        {
            lock (s_lock)
            {
                if (s_ruleMap == null) s_ruleMap = new Dictionary<Type, Rule>();

                if (!s_ruleMap.ContainsKey(ruleType))
                {
                    foreach (Reflector<Rule> ruleReflector in GetAllRuleReflectors())
                    {
                        if (ruleReflector.Type == ruleType)
                        {
                            s_ruleMap.Add(ruleType, ruleReflector.CreateInstance());
                            break;
                        }
                    }
                    if (!s_ruleMap.ContainsKey(ruleType)) throw new FormulaRuleException(ruleType.ToString() + "の型をルールとして正常に初期化できませんでした。");
                }
            }
            return s_ruleMap[ruleType];
        }


        static Dictionary<string, Formula> s_ruledCache = new Dictionary<string, Formula>();
        static bool s_useCache = false;
//        static bool s_useCache = true;

        /// <summary>
        /// ルールマッチのキャッシュを利用するか否かを設定もしくは取得します。
        /// </summary>
        public static bool UseCache
        {
            get { return s_useCache; }
            set { s_useCache = value; }
        }

        /// <summary>
        /// ルールマッチのキャッシュを削除します。
        /// </summary>
        public static void ClearCache() { s_ruledCache.Clear(); }



        
        List<Rule> _cacheReverseRules;
        List<Rule> _cacheAllPatternSample;

        /// <summary>
        /// 処理ルールに従って数式に対する変換処理を試みます。
        /// </summary>
        /// <param name="f">処理後の数式</param>
        /// <returns>処理を行ったか否か</returns>
        public bool TryMatchRule(ref Formula f)
        {
            Formula result = TryMatchRule(f);
            if (result == null) return false;

            f = result;
            return true;
        }

        /// <summary>
        /// 処理ルールに従って数式に対する変換処理を試みます。
        /// </summary>
        /// <param name="target">処理対象とする数式</param>
        /// <returns>処理後の数式。処理されない場合、null。</returns>
        public Formula TryMatchRule(Formula target)
        {
            if (!IsTargetTypeFormula(target)) return null; 

            string formulaText = null;
            if (UseCache) formulaText = "【" + DistinguishedText +"】" + target.GetUniqueText();
            if (UseCache && s_ruledCache.ContainsKey(formulaText))
            {
#if DEBUG
                string result = "null";
                if (s_ruledCache[formulaText] != null)
                {
                    result = s_ruledCache[formulaText].ToString();
                    Console.WriteLine(this.GetType().ToString() + "  キャッシュ::" + formulaText + " → " + result);
                }
#endif
                return s_ruledCache[formulaText];
            }

            try
            {
#if DEBUG
                string preText = target.GetUniqueText();
                Type preType = target.GetType();
#endif
                Formula.CheckCancelOperation(target); // ユーザーからの操作中止に対応
                Formula result = OnTryMatchRule(target);
#if DEBUG
                if (result != null && preText == result.GetUniqueText() && preType == result.GetType())
                    throw new FormulaAssertionException(string.Format("数式形状に変化がないにも拘らず、ルールの適用があったと判定されました。{0}の実装を確認してください。", GetType()));
                else if (result != null && object.ReferenceEquals(result, target))
                    throw new FormulaAssertionException(string.Format("数式に変化がありましたが、変形前後で数式の参照先に変化がありません。数式に変形があった場合、別の数式オブジェクトを返す必要があります。{0}の実装を確認してください。", GetType()));
                else if (result == null && preType != target.GetType())
                    throw new FormulaAssertionException(string.Format("数式形状に変化があったにも拘らず、ルールの適用がなかったと判定されました。{0}の実装を確認してください。", GetType()));
#endif
                if (UseCache) s_ruledCache.Add(formulaText, result);
                return result;
            }
            catch (FormulaRuleException) { throw; }
            finally {}
        }

        /// <summary>
        /// 指定数式が本クラスの処理対象となる数式か否かを簡易判定します。
        /// </summary>
        /// <remarks>
        /// この判定処理は頻繁に使用されるので、処理を重くしないよう注意してください。
        /// 複雑な判定処理はOnTryMatchRuleメソッドで行い、対象外となる場合には同メソッドにてnullを返してください。
        /// </remarks>
        internal protected abstract bool IsTargetTypeFormula(Formula target);
        
        /// <summary>
        /// 処理ルールに従って数式に対する変換処理を試みます。
        /// </summary>
        /// <remarks>
        /// 戻り値は、null、もしくは<paramref cref="formula"/>に対して、Equalsメソッドの等価判断によりfalseと判定される数式とする必要があります。
        /// </remarks>
        /// <param name="formula">処理対象の数式</param>
        /// <returns>変形後の数式。変形の対象とならない、もしくは結果的に変形のない場合、null。</returns>
        protected abstract Formula OnTryMatchRule(Formula target);

        static Dictionary<Type, List<Type>> s_cachePreDemandRules = new Dictionary<Type,List<Type>>();
        static Dictionary<Type, List<Type>> s_cachePostDemandRules = new Dictionary<Type,List<Type>>();

        /// <summary>
        /// このルールの処理適用前に適用される必要のあるルールを順次返す反復を取得します。
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetPreDemandRules()
        {
            Type selfType = GetType();

            lock (s_lockSort)
            {
                if (!s_cachePreDemandRules.ContainsKey(selfType))
                {
                    s_cachePreDemandRules.Add(selfType, new List<Type>());
                    foreach (Type type in OnGetPreDemandRules())
                    {
                        if (s_cachePreDemandRules[selfType].Contains(type)) continue;
                        s_cachePreDemandRules[selfType].Add(type);

                        // 先に処理する必要のあるルールの、先に処理する必要のあるルールも遡って登録
                        foreach (Type preType in GetRuleOf(type).GetPreDemandRules())
                            if (!s_cachePreDemandRules[selfType].Contains(preType)) s_cachePreDemandRules[selfType].Add(preType);
                    }
                }
            }

            foreach (var type in s_cachePreDemandRules[selfType]) yield return type;
        }

        /// <summary>
        /// このルールより先に処理されてはならないルールを順次返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetPostDemandRules()
        {
            Type selfType = GetType();

            lock (s_lockSort)
            {
                if (!s_cachePostDemandRules.ContainsKey(selfType))
                {
                    s_cachePostDemandRules.Add(selfType, new List<Type>());
                    foreach (Type type in OnGetPostDemandRules())
                    {
                        if (s_cachePostDemandRules[selfType].Contains(type)) continue;
                        s_cachePostDemandRules[selfType].Add(type);

                        // 後に処理する必要のあるルールの、後に処理する必要のあるルールも遡って登録
                        foreach (Type postType in GetRuleOf(type).GetPostDemandRules())
                            if (!s_cachePostDemandRules[selfType].Contains(postType)) s_cachePostDemandRules[selfType].Add(postType);
                    }
                }
            }

            foreach (var type in s_cachePostDemandRules[selfType]) yield return type;
        }



        /// <summary>
        /// このルールの処理適用前に適用される必要のあるルールを順次返す反復を取得します。
        /// ここで返す値は、型にだけ依存するものとし、プロパティなどに依存しないようにしてください。
        /// </summary>
        /// <remarks>
        /// <para>このメソッドは、次の判断に使用されます。</para>
        /// <list type="bullet">
        /// <item><description>ある一つの数式に対して、このルールと同時に適用可能となる可能性のあるルールに対し、優先的に試行するルールの決定。</description></item>
        /// <item><description>逆変換のルールに対する適用抑止の判定。</description></item>
        /// </list>
        /// <para>OnGetReverseRuleにて逆変換を行うルールを登録した場合には、必ずそのルールをOnGetPreDemandRulesメソッド、もしくはOnGetPostDemandRulesメソッドで返すようにしてください。
        /// </remarks>
        /// <returns></returns>
        protected virtual IEnumerable<Type> OnGetPreDemandRules() { yield break; }

        /// <summary>
        /// このルールより先に処理されてはならないルールを順次返す反復子を取得します。
        /// ここで返す値は、型にだけ依存するものとし、プロパティなどに依存しないようにしてください。
        /// </summary>
        /// <remarks>
        /// <para>このメソッドは、次の判断に使用されます。</para>
        /// <list type="bullet">
        /// <item><description>ある一つの数式に対して、このルールと同時に適用可能となる可能性のあるルールに対し、優先的に試行するルールの決定。</description></item>
        /// <item><description>逆変換のルールに対する適用抑止の判定。</description></item>
        /// </list>
        /// <para>OnGetReverseRuleにて逆変換を行うルールを登録した場合には、必ずそのルールをOnGetPreDemandRulesメソッド、もしくはOnGetPostDemandRulesメソッドで返すようにしてください。
        /// </remarks>
        /// <returns></returns>
        protected virtual IEnumerable<Type> OnGetPostDemandRules() { yield break; }


        /// <summary>
        /// ルール挙動を一意に決定する識別名を取得します。通常、型のFullNameですが、ルールに設定されたプロパティなどにより挙動が変わる場合には、状態に応じて一意の文字列を返すよう、オーバーライドしてください。
        /// </summary>
        public virtual string DistinguishedText { get { return GetType().FullName; } }

        /// <summary>
        /// このルール変形を適用した数式に対して、再帰的な変形ルールの適用を抑制するか否かを取得します。
        /// </summary>
        public virtual bool DeformConclude { get { return false; } }

        /// <summary>
        /// <see cref="OnGetReverseRule"/>メソッドの結果をキャッシュして再利用するか否かを取得します。プロパティなどの状態に応じて同メソッドの結果を可変とする場合には、falseを返してください。
        /// </summary>
        protected virtual bool UseReveseRuleCache { get { return true; } }

        /// <summary>
        /// 同じ型の数式、もしくは異なる型の数式に対して、このルールによる変形の逆向き変換を適用しうるルールをすべて返す反復子を取得します。
        /// </summary>
        /// <remarks>
        /// 逆向き変換のルールとは、このルールの適用後の数式形状に対して適用可能性があり、そのルール適用後の数式が元の数式形状となりうるルールのことを指します。
        /// 本メソッドで返すルールについては、必ずOnGetPreDemandRulesメソッド、もしくはOnGetPostDemandRulesメソッドを利用して、ルールの共存した変形においてどちらを優先して適用するかを明示してください。
        /// 原則として、展開傾向のルールでは整理傾向のルールを<see cref="OnGetPostDemandRules"/>で、整理傾向のルールでは展開傾向のルールを<see cref="OnGetPreDemandRules"/>で返すようにしてください。
        /// </remarks>
        /// <returns></returns>
        protected virtual IEnumerable<Rule> OnGetReverseRule() { yield break; }

        /// <summary>
        /// このルールの変形に対し、逆向きの変換を適用するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rule> GetReverseRule()
        {
            if (_cacheReverseRules == null || !UseReveseRuleCache)
            {
                _cacheReverseRules = new List<Rule>();
                foreach (var rule in OnGetReverseRule()) _cacheReverseRules.Add(rule);
            }
            foreach (var rule in _cacheReverseRules) yield return rule;
        }



        /// <summary>
        /// このルールの説明を取得します。
        /// </summary>
        public abstract string Information { get; }

        /// <summary>
        /// 変形前数式と変形後数式から成る、このルールの変形例を順次返す反復子を取得します。ここで取得される例は、単体テストで自動検証されます。
        /// </summary>
        /// <returns>変形例(変形前の数式と、変形適用後の数式を表す組合せ)を返す反復子。</returns>
        public abstract IEnumerable<KeyValuePair<Formula, Formula>> GetExamples();


        /// <summary>
        /// この型のルールのプロパティで、考えうるすべてのパターンのインスタンスを返す反復子を取得します。
        /// </summary>
        /// <returns>このルールで考えられる全てのパターンのインスタンスを返す反復子。</returns>
        public IEnumerable<Rule> GetAllPatternSample()
        {
            if (_cacheAllPatternSample == null) _cacheAllPatternSample = new List<Rule>(OnGetAllPatternSample());
            foreach (var sample in _cacheAllPatternSample) yield return sample;
        }

        /// <summary>
        /// この型のルールのプロパティで、考えうるすべてのパターンのインスタンスを生成して返す反復子を取得します。複数パターンのない場合、単にthisを返してください。
        /// </summary>
        /// <returns>このルールで考えられる全てのパターンのインスタンスを返す反復子。</returns>
        protected abstract IEnumerable<Rule> OnGetAllPatternSample();

        /// <summary>
        /// クローンを取得します。再帰呼び出し可能なルールである場合、単にthisを返してください。
        /// </summary>
        /// <returns>複製されたルール。</returns>
        public abstract Rule GetClone();



        /// <summary>
        /// ルールのセルフテストを実行します。
        /// </summary>
        /// <returns>検知された例外を返す反復子。</returns>
        public IEnumerable<Exception> SelfCheckTest()
        {
            // GetReverseルールと、Pre、Postの関係
            // GetReverse先のルールとの関係矛盾性チェック
            foreach (var reverse in GetReverseRule())
            {
                // このreverseに対して、Previous、もしくはPostの指定があるか
                bool isOk = false;
                for (int i = 0; i < 2; i++)
                {
                    bool checkPrevious = (i == 0);

                    foreach (var next in checkPrevious ? GetPreDemandRules() : GetPostDemandRules())
                    {
                        if (reverse.GetType() != next) continue;

                        if (isOk) yield return new FormulaRuleException(string.Format("{0}において、逆変換のルール{1}との優先順位を解決できません。", DistinguishedText, reverse.DistinguishedText));
                        isOk = true; // GetReverseのルールに対して、少なくともPreかPostの一つが指定されている

                        foreach (var chk in checkPrevious ? reverse.GetPreDemandRules() : reverse.GetPostDemandRules())
                        {
                            if (GetType() == chk)
                            {
                                // このルールにとってreverseはPreなのに、reverseにとってもこれがPre → 優先順指定に不整合
                                yield return new FormulaRuleException(string.Format("逆変換ルールの優先順指定に不整合があります。{0}に対する{1}。", DistinguishedText, reverse.DistinguishedText));
                            }
                        }
                    }
                }
                if (!isOk) yield return new FormulaRuleException(string.Format("{0}において、逆変換のルールとして登録されている{1}に対して優先順が指定されていません。", DistinguishedText, reverse.DistinguishedText));
            }

            // Exampleの動作チェック
            foreach (var pair in GetExamples())
            {
                var f = pair.Key;
                var rule = GetClone();
                if (!rule.IsTargetTypeFormula(f)) yield return new FormulaRuleException(string.Format("ルールに登録されたサンプル「{0}」がルール変形の対象となりませんでした。", pair.Key));

                var result = rule.TryMatchRule(f.Copy());
                if (result != pair.Value) yield return new FormulaRuleException(string.Format("ルールに登録されたサンプルと異なる結果が得られました。{0}に対し、{1}への変形が期待されますが、{2}となりました。", pair.Key, pair.Value, result));
            }
        }

        #region IComparable<Rule> メンバー

        /// <summary>
        /// ルールの適用順序を定義する優先順序の比較結果を取得します。
        /// </summary>
        /// <param name="other">比較対象のルール。</param>
        /// <returns>比較結果を表す数字。</returns>
        public int CompareTo(Rule other)
        {
            var otherType = other.GetType();
            var thisType = GetType();
            if (otherType == thisType) return 0;

            foreach (var type in GetPreDemandRules())
                if (type == otherType) return 1;
            foreach (var type in GetPostDemandRules())
                if (type == otherType) return -1;

            foreach (var type in other.GetPreDemandRules())
                if (type == thisType) return -1;
            foreach (var type in other.GetPostDemandRules())
                if (type == thisType) return 1;

            return 0;
        }

        #endregion

        /// <summary>
        /// 指定ルールと等価か否かを判断し、その結果を取得します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト。</param>
        /// <returns>比較結果。</returns>
        public override bool Equals(object obj)
        {
            Rule other = obj as Rule;
            if (other == null) return false;

            return DistinguishedText == other.DistinguishedText;
        }

        /// <summary>
        /// このルールのハッシュコードを返します。
        /// </summary>
        /// <returns>ルールのハッシュコード。</returns>
        public override int GetHashCode()
        {
            return DistinguishedText.GetHashCode();
        }

        
        /// <summary>
        /// ルールの比較結果を返します。
        /// </summary>
        /// <remarks>
        /// ルールでは、Equalsメソッドと==演算子で、いずれも値の等価判定が行われます。
        /// 参照の等価判定を行うには、object.ReferenceEqualsメソッドを用いてください。
        /// </remarks>
        /// <param name="r1">ルール1。</param>
        /// <param name="r2">ルール2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator ==(Rule r1, Rule r2) 
        {
            if (Object.Equals(r1, null) && Object.Equals(r2, null)) return true;
            if (Object.Equals(r1, null) || Object.Equals(r2, null)) return false;
            return r1.Equals(r2);
        }

        /// <summary>
        /// ルールの比較結果を返します。
        /// </summary>
        /// <remarks>
        /// ルールでは、Equalsメソッドと==演算子で、いずれも値の等価判定が行われます。
        /// 参照の等価判定を行うには、object.ReferenceEqualsメソッドを用いてください。
        /// </remarks>
        /// <param name="r1">ルール1。</param>
        /// <param name="r2">ルール2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator !=(Rule r1, Rule r2) { return !(r1 == r2); }


    }
}
