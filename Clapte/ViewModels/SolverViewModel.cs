using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Solvers.Processes;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Parse;
using GoodSeat.Liffom.Processes;
using GoodSeat.Liffom.Formats.Powers;
using GoodSeat.Liffom.Formulas.Constants.Rules;

namespace GoodSeat.Clapte.ViewModels
{
    /// <summary>
    /// 計算モードを表します。
    /// </summary>
    public enum CalculateMode
    {
        /// <summary>
        /// 小数計算モード。
        /// </summary>
        Decimal,
        /// <summary>
        /// 分数計算モード。
        /// </summary>
        Fraction
    }

    /// <summary>
    /// Clapteソルバのビューモデルを表します。
    /// </summary>
    public class SolverViewModel : ISerializable
    {
        static SolverViewModel()
        {
            Numeric.InnerRealType = Numeric.RealType.DoubleModified;
            Liffom.Reals.BigDecimalValue.MaxDigits = 30;
        }

        /// <summary>
        /// Clapteソルバのビューモデルを初期化します。
        /// </summary>
        /// <param name="target">設定対象とするソルバ。</param>
        public SolverViewModel()
        {
            Target = new Solver();

            this.MaxInputTextLength = 200;
            this.MaxVariableTextLength = 5;
            this.Mode = CalculateMode.Decimal;
            this.SolveAlgebraic = new SolveAlgebraicEquation();
            this.NewtonMethod = new NewtonMethod();
            this.BrentMethod = new BrentMethod();
            this.MaxTime = 3000;
            this.PermitOnlySingleTermResult = true;

            PermitOmitProductMark = true;
            DetectUnitMode = ReplaceVariableToUnitProcess.Mode.AllAutoDetect;

            UpdateSetting();
        }

        /// <summary>
        /// 対象とするソルバを設定もしくは取得します。
        /// </summary>
        public Solver Target { get; private set; }


        /// <summary>
        /// 対象のソルバの設定が更新されたときに呼び出されます。
        /// </summary>
        public event EventHandler SettingUpdated;

        /// <summary>
        /// ソルバの設定が更新されたことを通知します。
        /// </summary>
        protected void OnSettingUpdated()
        {
            if (SettingUpdated != null) SettingUpdated(this, EventArgs.Empty);
        }

        #region 入力の設定

        /// <summary>
        /// 数式として認識する最大文字列長を設定もしくは取得します。
        /// </summary>
        public int MaxInputTextLength { get; set; }

        /// <summary>
        /// 変数・単位名として認識する最大文字列長を設定もしくは取得します。
        /// </summary>
        public int MaxVariableTextLength { get; set; }

        /// <summary>
        /// 絶対値記号の区切りとして、"|"を認識するか否かを設定もしくは取得します。
        /// </summary>
        public bool ParseAbsPunctuation { get; set; }

        /// <summary>
        /// 階乗の記号として、"!"を認識するか否かを設定もしくは取得します。
        /// </summary>
        public bool ParseFactorial { get; set; }

        /// <summary>
        /// 積算記号の省略表記を許可するか否かを設定もしくは取得します。
        /// </summary>
        public bool PermitOmitProductMark { get; set; }

        /// <summary>
        /// 乗算記号の省略表記を許可するか否かを設定もしくは取得します。
        /// </summary>
        public bool PermitOmitPowerMark { get; set; }

        /// <summary>
        /// 単位の認識モードを設定若しくは取得します。
        /// </summary>
        public ReplaceVariableToUnitProcess.Mode DetectUnitMode { get; set; }

        #endregion

        #region 計算の設定

        /// <summary>
        /// 計算モードを設定もしくは取得します。
        /// </summary>
        public CalculateMode Mode { get; set; }

        /// <summary>
        /// 数値精度モードを設定もしくは取得します。
        /// </summary>
        public Numeric.RealType NumericPrecision
        {
            get { return Numeric.InnerRealType; }
            set { Numeric.InnerRealType = value; }
        }

        /// <summary>
        /// 任意精度時の考慮数値桁数を設定もしくは取得します。
        /// </summary>
        public int PrecisionDigitOfBigDecimal
        {
            get { return Liffom.Reals.BigDecimalValue.MaxDigits; }
            set { Liffom.Reals.BigDecimalValue.MaxDigits = value; }
        }

        /// <summary>
        /// 中間数値の丸め処理方法を設定もしくは取得します。
        /// </summary>
        public MidpointRounding MidpointRound
        {
            get { return Numeric.MidpointRound; }
            set { Numeric.MidpointRound = value; }
        }

        /// <summary>
        /// 解の公式による方程式の求解処理を設定もしくは取得します。
        /// </summary>
        public SolveAlgebraicEquation SolveAlgebraic { get; set; }

        /// <summary>
        /// ニュートン法による方程式の求解処理を設定もしくは取得します。
        /// </summary>
        public NewtonMethod NewtonMethod { get; set; }

        /// <summary>
        /// ブレント法による方程式の求解処理を設定もしくは取得します。
        /// </summary>
        public BrentMethod BrentMethod { get; set; }

        /// <summary>
        /// 計算を中止する時間[ms]を設定若しくは取得します。
        /// </summary>
        public double MaxTime { get; set; }

        #endregion

        #region 出力の設定

        /// <summary>
        /// 単項式のみを計算結果として許可するか否かを設定もしくは取得します。
        /// </summary>
        public bool PermitOnlySingleTermResult { get; set; }

        /// <summary>
        /// 計算結果の出力文字列タイプを設定もしくは取得します。
        /// </summary>
        public CharaType OutputCharaType { get; set; }

        /// <summary>
        /// 計算結果の出力文字列における単位表記のタイプを設定若しくは取得します。
        /// </summary>
        public UnitFormatType OutputUnitFormatType { get; set; }

        /// <summary>
        /// 有効数値を考慮するか否かを設定もしくは取得します。
        /// </summary>
        public bool ConsiderValidDigit { get; set; }

        #endregion

        /// <summary>
        /// 設定に基づいて、ソルバの設定を更新します。
        /// </summary>
        public void UpdateSetting()
        {
            Target.ProcessList = CreateProcessList(Target);
            Target.OutputFormat = CreateFormat();
            Target.Parser = CreateFormulaParser();

            OnSettingUpdated();
        }


        /// <summary>
        /// 設定に基づいて初期化したソルバの処理定義リストを取得します。
        /// </summary>
        /// <param name="solver">設定対象となるソルバ。</param>
        /// <returns>処理定義リスト。</returns>
        private List<Solvers.Processes.Process> CreateProcessList(Solver solver)
        {
            var result = new List<Solvers.Processes.Process>();

            // 文字列長チェック
            result.Add(new SieveTargetInputProcess(solver, MaxInputTextLength, MaxVariableTextLength));

            // 出力文字列の半角/全角判定、変換
            result.Add(new OutputTextByteFormatProcess(solver, OutputCharaType));

            // 全角文字を相当する文字列に置換
            result.Add(new ReplaceAlternateStringProcess(solver));

            // 数値の3桁区切りカンマの認識と出力書式への反映
            result.Add(new DetectSplitCommmaProcess(solver));
            
            // ユーザー定義定数・関数の置き換え
            result.Add(new EvaluateUserDefineProcess(solver));
            
            // 変数を単位で置き換え
            result.Add(new ReplaceVariableToUnitProcess(solver, DetectUnitMode));

            // 計算処理
            result.Add(CreateCalculateProcess(solver));

            // 出力単位表記調整
            result.Add(new SetUnitFormatProcess(solver, OutputUnitFormatType));

            // 計算結果の抽出処理
            result.Add(new SieveResultFormulaProcess(solver, PermitOnlySingleTermResult));

            return result;
        }

        /// <summary>
        /// 設定に基づいて数式の計算処理を初期化して取得します。
        /// </summary>
        /// <param name="solver">設定対象となるソルバ。</param>
        /// <returns>初期化された数式の計算処理。</returns>
        private Clapte.Solvers.Processes.Process CreateCalculateProcess(Solver solver)
        {
            var list = new List<Clapte.Solvers.Processes.Process>();

            // 目標単位の認識と変換
            if (Mode == CalculateMode.Fraction) list.Add(new ConvertToSpecifiedUnitProcess(solver, Formula.SimplifyToken));
            else list.Add(new ConvertToSpecifiedUnitProcess(solver, Formula.NumerateToken));

            // 等式に対する方程式の求解
            List<SolveEquation> solveList = new List<SolveEquation>();
            if (SolveAlgebraic != null) solveList.Add(SolveAlgebraic);
            if (NewtonMethod != null) solveList.Add(NewtonMethod);
            if (BrentMethod != null) solveList.Add(BrentMethod);
            if (solveList.Count != 0) list.Add(new SolveEquationProcess(solver, MaxTime, solveList.ToArray()));

            // 有効数字考慮設定
            solveList.ForEach(s => s.CheckSignificantDigits = ConsiderValidDigit);

            // 計算処理
            List<DeformToken> tokenList = new List<DeformToken>();
            if (Mode == CalculateMode.Fraction)
            {
                var token = new DeformToken(new SimplifyToken(), new CalculateToken());
                token.AdditionalTryRules.Add(ConvertToFractionRule.Entity);
                tokenList.Add(token);
            }
            else
            {
                // tan(pi/2)やe^ix 等を正しくルール変形するため、piやe等の定数と、累乗の数値化は後回しにする。
//                var token1 = new DeformToken(new SimplifyToken(), new NumerateToken(), new CalculateToken());
//                token1.NoTryRules.Add(CalculatePowerNumericRule.Entity); // 累乗の数値化を除外
//                token1.NoTryRules.Add(ConstantNumerateRule.Entity); // 定数の数値化を除外
//                token1.NoTryRules.Add(CalculateDivideNumericRule.Entity); // 数値の除算を除外
//                tokenList.Add(token1);

                // その後、小数まで計算しきる。
                var token2 = new DeformToken(new SimplifyToken(), new NumerateToken(), new CalculateToken());
                tokenList.Add(token2);
            }
            list.Add(new CalculateFormulaProcess(solver, MaxTime, tokenList.ToArray()));

            return new ProcessSet(solver, list.ToArray());
        }

        /// <summary>
        /// 設定に基づいて数式の構文解析器を初期化して取得します。
        /// </summary>
        /// <returns>初期化された数式の構文解析器。</returns>
        private FormulaParser CreateFormulaParser()
        {
            return CreateFormulaParser(ParseAbsPunctuation, PermitOmitPowerMark, PermitOmitProductMark, ParseFactorial);
        }

        /// <summary>
        /// Clapteにおける数式の構文解析器を初期化して取得します。
        /// </summary>
        /// <param name="parseAbsPunctuation">絶対値記号を解析するか。</param>
        /// <param name="permitOmitPowerMark">累乗記号の省略を許可するか。</param>
        /// <param name="permitOmitProductMark">乗算記号の省略を許可するか。</param>
        /// <param name="parseFactorial">階乗記号の解析するか。</param>
        /// <returns>初期化された数式の構文解析器。</returns>
        static public FormulaParser CreateFormulaParser(bool parseAbsPunctuation, bool permitOmitPowerMark, bool permitOmitProductMark, bool parseFactorial)
        {
            var parser = new FormulaParser();
            parser.Lexers.Add(new NumericLexer()); // 数値
            parser.Lexers.Add(new ParenthesesLexer()); // 丸括弧
            parser.Lexers.Add(new BracketLexer(true, true)); // 角括弧
            parser.Lexers.Add(new BraceLexer()); // 波括弧
            if (parseAbsPunctuation) parser.Lexers.Add(new AbsolutePunctuationLexer()); // 絶対値記号
            parser.Lexers.Add(new SpaceLexer()); // 空白トークン
            parser.Lexers.Add(new FunctionLexer(parser, true)); // 関数
            parser.Lexers.Add(new ConstantLexer(true)); // 定数
            parser.Lexers.Add(new VariableLexer(parser, false)); // 変数

            if (permitOmitPowerMark) parser.AddOperatorParsers(new AbbreviatedPowerOperatorParser(parser)); // 乗算記号の省略を許可(通常の乗算より優先度を上げる)
            parser.AddOperatorParsers(new PowerOperatorParser()); // 累乗
            parser.AddOperatorParsers(new PowerOfMatrixOperatorParser()); // 行列の累乗
            parser.AddOperatorParsers(new PlusMinusOperatorParser()); // 正負記号
            if (permitOmitProductMark) parser.AddOperatorParsers(new AbbreviatedProductOperatorParser()); // 積算記号の省略を許可(通常の積算より優先度を上げる)
            parser.AddOperatorParsers(new ProductOfMatrixOperatorParser()); // 行列の乗算
            parser.AddOperatorParsers(new ProductOperatorParser()); // 乗算・除算
            parser.AddOperatorParsers(new SumOperatorParser()); // 和算・減算
            if (parseFactorial) parser.AddOperatorParsers(new FactorialOperatorParser()); // 階乗記号(!)
            parser.AddOperatorParsers(new ComparerOperatorParser()); // 比較演算子
            parser.AddOperatorParsers(new BooleanOperatorParser()); // 論理演算子
            parser.AddOperatorParsers(new ArgumentOperatorParser()); // 引数演算子
            return parser;
        }

        /// <summary>
        /// 設定に基づいて、数式の出力書式を初期化して取得します。
        /// </summary>
        /// <returns>初期化された数式の出力書式。</returns>
        private Format CreateFormat()
        {
            Format format = new Format();

            // 有効数値の考慮
            var considerDigit = new ConsiderSignificantFiguresFormatProperty(ConsiderValidDigit);
            format.SetProperty(considerDigit);

            // x^-nは、1/x^n形式で表示
            var divisionFormat = new DivisionFormatProperty(true);
            format.SetProperty(divisionFormat);

            // +、-、=、< 等は空白で囲う
            var operatorFormat = new OperatorFormatProperty(OperatorFormatProperty.OperatorType.Sum | OperatorFormatProperty.OperatorType.Boolean | OperatorFormatProperty.OperatorType.Compare);
            format.SetProperty(operatorFormat);

            return format;
        }

        #region ISerializable メンバー

        public void OnDeserialize(XmlElement xmlElement)
        {
            XmlElement inputElement = xmlElement["InputSetting"];
            var inputVersion = double.Parse(inputElement.GetAttribute("Version", "1.0"));
            MaxInputTextLength = int.Parse(inputElement.GetAttribute("MaxInputTextLength"));
            MaxVariableTextLength = int.Parse(inputElement.GetAttribute("MaxVariableTextLength"));
            ParseAbsPunctuation = bool.Parse(inputElement.GetAttribute("ParseAbsPunctuation"));
            ParseFactorial = bool.Parse(inputElement.GetAttribute("ParseFactorial"));
            PermitOmitProductMark = bool.Parse(inputElement.GetAttribute("PermitOmitProductMark", "True"));
            PermitOmitPowerMark = bool.Parse(inputElement.GetAttribute("PermitOmitPowerMark", "False"));
            if (inputVersion >= 1.1)
            {
                DetectUnitMode = (ReplaceVariableToUnitProcess.Mode)Enum.Parse(typeof(ReplaceVariableToUnitProcess.Mode), inputElement.GetAttribute("DetectUnitMode"));
            }

            XmlElement calculateElement = xmlElement["CalculateSetting"];
            Mode = (CalculateMode)Enum.Parse(typeof(CalculateMode), calculateElement.GetAttribute("Mode"));
            NumericPrecision = (Numeric.RealType)Enum.Parse(typeof(Numeric.RealType), calculateElement.GetAttribute("NumericPrecision", "DoubleModified"));
            PrecisionDigitOfBigDecimal = int.Parse(calculateElement.GetAttribute("PrecisionDigitOfBigDecimal", "30"));
            if (NumericPrecision == Numeric.RealType.Decimal) // ver 2.1 におけるDecimalの廃止に対応
            {
                NumericPrecision = Numeric.RealType.BigDecimal;
                PrecisionDigitOfBigDecimal = 28;
            }

            MaxTime = double.Parse(calculateElement.GetAttribute("MaxTime", "3000"));
            MidpointRound = (MidpointRounding)Enum.Parse(typeof(MidpointRounding), calculateElement.GetAttribute("MidpointRound"));
            {
                XmlElement newtonElement = calculateElement["NewtonMethod"];
                NewtonMethod.InitialSolution = double.Parse(newtonElement.GetAttribute("InitialSolution"));
                NewtonMethod.ErrorTolerance = double.Parse(newtonElement.GetAttribute("ErrorTolerance"));
                NewtonMethod.MaxTryCount = int.Parse(newtonElement.GetAttribute("MaxTryCount"));

                XmlElement brentElement = calculateElement["BrentMethod"];
                BrentMethod.ErrorTolerance = double.Parse(brentElement.GetAttribute("ErrorTolerance"));
                BrentMethod.MaxTryCount = int.Parse(brentElement.GetAttribute("MaxTryCount"));
                BrentMethod.UpperLimit = new Numeric(double.Parse(brentElement.GetAttribute("UpperLimit")));
                BrentMethod.LowerLimit = new Numeric(double.Parse(brentElement.GetAttribute("LowerLimit")));
            }

            XmlElement outputElement = xmlElement["OutputSetting"];
            PermitOnlySingleTermResult = bool.Parse(outputElement.GetAttribute("PermitOnlySingleTermResult"));
            OutputCharaType = (CharaType)Enum.Parse(typeof(CharaType), outputElement.GetAttribute("OutputCharaType"));
            OutputUnitFormatType = (UnitFormatType)Enum.Parse(typeof(UnitFormatType), outputElement.GetAttribute("OutputUnitFormatType", "Auto"));
            ConsiderValidDigit = bool.Parse(outputElement.GetAttribute("ConsiderValidDigit"));

            UpdateSetting();
        }

        public void OnSerialize(XmlElement xmlElement)
        {
            XmlElement inputElement = new XmlElement("InputSetting");
            inputElement.AddAttribute("Version", "1.1");
            inputElement.AddAttribute("MaxInputTextLength", MaxInputTextLength.ToString());
            inputElement.AddAttribute("MaxVariableTextLength", MaxVariableTextLength.ToString());
            inputElement.AddAttribute("ParseAbsPunctuation", ParseAbsPunctuation.ToString());
            inputElement.AddAttribute("ParseFactorial", ParseFactorial.ToString());
            inputElement.AddAttribute("PermitOmitProductMark", PermitOmitProductMark.ToString());
            inputElement.AddAttribute("PermitOmitPowerMark", PermitOmitPowerMark.ToString());
            inputElement.AddAttribute("DetectUnitMode", DetectUnitMode.ToString());
            xmlElement.AddElements(inputElement);

            XmlElement calculateElement = new XmlElement("CalculateSetting");
            calculateElement.AddAttribute("Mode", Mode.ToString());
            calculateElement.AddAttribute("NumericPrecision", NumericPrecision.ToString());
            calculateElement.AddAttribute("PrecisionDigitOfBigDecimal", PrecisionDigitOfBigDecimal.ToString());
            calculateElement.AddAttribute("MaxTime", MaxTime.ToString());
            calculateElement.AddAttribute("MidpointRound", MidpointRound.ToString());
            {
                XmlElement newtonElement = new XmlElement("NewtonMethod");
                newtonElement.AddAttribute("InitialSolution", NewtonMethod.InitialSolution.ToString());
                newtonElement.AddAttribute("ErrorTolerance", NewtonMethod.ErrorTolerance.ToString());
                newtonElement.AddAttribute("MaxTryCount", NewtonMethod.MaxTryCount.ToString());
                calculateElement.AddElements(newtonElement);

                XmlElement brentElement = new XmlElement("BrentMethod");
                brentElement.AddAttribute("ErrorTolerance", BrentMethod.ErrorTolerance.ToString());
                brentElement.AddAttribute("MaxTryCount", BrentMethod.MaxTryCount.ToString());
                brentElement.AddAttribute("UpperLimit", BrentMethod.UpperLimit.ToString());
                brentElement.AddAttribute("LowerLimit", BrentMethod.LowerLimit.ToString());
                calculateElement.AddElements(brentElement);
            }
            xmlElement.AddElements(calculateElement);

            XmlElement outputElement = new XmlElement("OutputSetting");
            outputElement.AddAttribute("PermitOnlySingleTermResult", PermitOnlySingleTermResult.ToString());
            outputElement.AddAttribute("OutputCharaType", OutputCharaType.ToString());
            outputElement.AddAttribute("OutputUnitFormatType", OutputUnitFormatType.ToString());
            outputElement.AddAttribute("ConsiderValidDigit", ConsiderValidDigit.ToString());
            xmlElement.AddElements(outputElement);
        }

        #endregion
    }

}
