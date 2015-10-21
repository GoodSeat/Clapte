using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
    // TODO: 例外をもっと丁寧に
    /// <summary>
    /// 数式の構文解析器を表します。
    /// </summary>
    public class FormulaParser
    {
        /// <summary>
        /// 数式の構文解析器を初期化します。
        /// </summary>
        public FormulaParser()
        {
            Lexers = new List<Lexer>();
            OperatorParsers = new List<OperatorParser>();
            OperatorLexers = new List<Lexer>();
        }

        /// <summary>
        /// 有効な字句解析器リストを取得します。
        /// </summary>
        /// <remarks>
        /// <see cref="OperatorParsers"/>に登録された<see cref="OperatorParser"/>から取得される演算子の字句解析器は
        /// 自動で取得されるため、ここに登録する必要ありません。
        /// </remarks>
        public List<Lexer> Lexers { get; private set; }

        /// <summary>
        /// 有効な演算構文解析器リストを設定もしくは取得します。
        /// </summary>
        /// <remarks>このリストの登録順が、そのまま演算の解決順となります。</remarks>
        private List<OperatorParser> OperatorParsers { get; set; }

        /// <summary>
        /// 演算構文解析器から取得された有効な演算字句解析器リストを設定もしくは取得します。
        /// </summary>
        private List<Lexer> OperatorLexers { get; set; }

        /// <summary>
        /// 演算構文解析器から取得された有効な演算字句解析器を返す反復子を取得します。
        /// </summary>
        public IEnumerable<Lexer> ValidOperatorLexers
        {
            get
            {
                foreach (var lexer in OperatorLexers) yield return lexer;
            }
        }

        /// <summary>
        /// 有効な演算字句解析器を全て返す反復子を取得します。
        /// </summary>
        public IEnumerable<Lexer> AllValidLexers
        {
            get
            {
                foreach (var lexer in Lexers) yield return lexer;
                foreach (var lexer in OperatorLexers) yield return lexer;
            }
        }


        /// <summary>
        /// 有効な演算構文解析器を追加します。
        /// </summary>
        /// <param name="operatorParser">追加する可変数の演算構文解析器。</param>
        /// <remarks>登録順が、そのまま演算の解決順となります。</remarks>
        public void AddOperatorParsers(params OperatorParser[] operatorParsers)
        {
            OperatorParsers.AddRange(operatorParsers);

            foreach (var operatorParser in operatorParsers)
                OperatorLexers.AddRange(operatorParser.GetAllBelongOperatorLexers());
        }

        /// <summary>
        /// 有効な演算構文解析器を削除します。
        /// </summary>
        /// <param name="operatorParser">削除する可変数の演算構文解析器。</param>
        public void RemoveOperatorParsers(params OperatorParser[] operatorParsers)
        {
            foreach (var operatorParser in operatorParsers)
            {
                OperatorParsers.Remove(operatorParser);

                for (int i = OperatorLexers.Count - 1; i >= 0; i--)
                {
                    OperatorLexer operatorLexer = OperatorLexers[i] as OperatorLexer;
                    if (operatorLexer == null) continue;
                    if (operatorLexer.BelongOperatorParser == operatorParser)
                        OperatorLexers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 登録済みの字句解析器から、指定した型の字句解析器を探索して取得します。
        /// </summary>
        /// <returns>登録済みの字句解析器。該当する字句解析器が見つからない場合、null。</returns>
        public T GetLexerOf<T>() where T : Lexer
        {
            foreach (var lexer in AllValidLexers)
            {
                if (lexer is T) return lexer as T;
            }
            return null;
        }

        
        /// <summary>
        /// 指定文字列が数式として構文解析可能か否かを取得します。
        /// </summary>
        /// <param name="text">構文解析対象の文字列。</param>
        /// <param name="formula">構文解析で得られた数式。</param>
        /// <returns>構文解析が可能か否か。</returns>
        public bool TryParse(string text, out Formula formula)
        {
            formula = null;
            try
            {
                formula = Parse(text);
                return true;
            }
            catch (FormulaParseException)
            {
                return false;
            }
        }

        /// <summary>
        /// 指定文字列の構文解析を実行し、解析で得られた数式を取得します。
        /// </summary>
        /// <param name="text">構文解析対象の文字列。</param>
        /// <returns>解析で得られた数式。</returns>
        /// <exception cref="FormulaParseException">何らかの理由で構文解析に失敗した場合にスローされます。</exception>
        public Formula Parse(string text)
        {
            Token topToken = AnalyzeLexical(text); // 字句解析
            while (topToken is StartToken)
            {
                var startToken = topToken as StartToken;
                ModifyTokens(startToken); // トークン間調整
                topToken = ParseInnerPunctuation(startToken); // 括弧内部の演算子構文解析
            }

            // トークン列は、FormulaTokenのみになっているはず
            if (topToken.PostTokenCount != 0) throw new FormulaParseException(string.Format("文字列「{0}」の構文解析に失敗しました。", text));

            FormulaToken parsedToken = topToken as FormulaToken;
            if (parsedToken == null) throw new FormulaParseException(string.Format("文字列「{0}」の構文解析に失敗しました。", text));
            return parsedToken.ParsedFormula;
        }

        /// <summary>
        /// 文字列の字句解析を実行し、トークン列の先頭のトークンを取得します。
        /// </summary>
        /// <param name="text">字句解析の対象文字列。</param>
        /// <returns>解析結果トークン列の先頭トークン。</returns>
        /// <exception cref="FormulaParseException">文字列のトークン化に失敗した場合にスローされます。</exception>
        private StartToken AnalyzeLexical(string text)
        {
            Token lastToken = new StartToken();
            lastToken.InsertNext(new EndToken());

            int length = 1;
            for (int startIndex = 0; startIndex < text.Length; startIndex += length)
            {
                string matchedText = "";
                List<Lexer> candidateLexers = new List<Lexer>(AllValidLexers);
                List<Lexer> lastHitLexers = new List<Lexer>();

                string target = "";
                for (length = 1; length <= text.Length - startIndex; length++)
                {
                    target = text.Substring(startIndex, length);
                    for (int i = 0; i < candidateLexers.Count; i++)
                    {
                        var lexer = candidateLexers[i];
                        Lexer.ScanResult result = lexer.Scan(target);

                        if (result == Lexer.ScanResult.NeverMatch)
                        {
                            candidateLexers.RemoveAt(i--);
                            continue;
                        }
                        else if (result == Lexer.ScanResult.Match)
                        {
                            if (matchedText != target) lastHitLexers.Clear();
                            matchedText = target;
                            lastHitLexers.Add(lexer);
                        }
                    }
                    if (candidateLexers.Count == 0) break;
                }
                if (lastHitLexers.Count == 0) throw new FormulaParseException(string.Format("文字列「{0}」のトークン化に失敗しました。トークン化可能な候補が存在しません。", target));
                if (lastHitLexers.Count != 1) throw new FormulaParseException(string.Format("文字列「{0}」のトークン化に失敗しました。トークン化可能な候補が複数存在します。", target));

                foreach (var addToken in lastHitLexers[0].Tokenize(matchedText))
                {
                    lastToken.InsertNext(addToken);
                    lastToken = addToken;
                }

                length = matchedText.Length;
            }

            return lastToken.TopToken as StartToken;
        }

        /// <summary>
        /// トークン列に対し、トークン、及び演算解析器によるトークン間調整を状態が終息するまで実行します。
        /// </summary>
        /// <param name="topToken">対象トークン列の先頭トークン。</param>
        /// <exception cref="FormulaParseException">トークン列が不正な状態となった場合にスローされます。</exception>
        private void ModifyTokens(StartToken topToken)
        {
            bool modified = true;
            while (modified)
            {
                modified = false;

                Token targetToken = topToken;
                if (!(targetToken is StartToken)) throw new FormulaParseException("解析中のトークン列の先頭トークンが、StartToken以外のトークンとなりました。不正な操作です。");
                targetToken = topToken.NextToken;

                while (!(targetToken is EndToken))
                {
                    if (targetToken == null) throw new FormulaParseException("解析中のトークン列の末尾トークンが、EndToken以外のトークンとなりました。不正な操作です。");

                    modified |= targetToken.ModifyTokenRelation();

                    foreach (var parser in OperatorParsers)
                    {
                        modified |= parser.ModifyTokenRelation(targetToken);
                        if (modified) break;
                    }

                    targetToken = targetToken.NextToken;
                }
            }
        }

        /// <summary>
        /// トークン列中で最初に見つかる末尾区切りトークンとその対となる区切りトークンの内部を、数式トークンに置き換えます。
        /// </summary>
        /// <param name="topToken">トークン列の先頭トークン</param>
        /// <returns>置き換え後トークン列の先頭トークン</returns>
        /// <exception cref="FormulaParseException">数式文字列中の区切りが正しく対応しない場合にスローされます。</exception>
        private Token ParseInnerPunctuation(StartToken topToken)
        {
            Token StartPunctuationToken = topToken;
            Token EndPunctuationToken = topToken.LastToken;

            Token endToken = topToken.NextToken;
            while (!IsEndPunctuationToken(endToken)) endToken = endToken.NextToken;

            Token startToken = endToken.PreviousToken;
            while (!IsStartPunctuationToken(startToken)) startToken = startToken.PreviousToken;

            var startPunctuation = startToken as PunctuationToken;
            var endPunctuation = endToken as PunctuationToken;
            if (!startPunctuation.IsValidSetPunctuation(endPunctuation)) throw new FormulaParseException("数式文字列中の区切りが正しく対応しませんでした。");

            var parsedToken = ParseInner(startPunctuation.NextToken, endPunctuation.PreviousToken);
            if (parsedToken == null) throw new FormulaParseException(startPunctuation.NextToken.GetBaseText(endPunctuation) + "の解析に失敗しました。");
            Formula parsed = parsedToken.ParsedFormula;

            parsed = startPunctuation.OnInnerParsed(parsed); // 区切りトークンに応じた数式加工
            FormulaToken newToken = new FormulaToken(startPunctuation.GetBaseText(endPunctuation), parsed);

            startPunctuation.InsertPrevious(newToken);
            while (newToken.NextToken != endPunctuation) newToken.NextToken.Remove();
            endPunctuation.Remove();

            return newToken.TopToken;
        }

        /// <summary>
        /// 指定トークンが、数式の開始区切りであるか否かを取得します。
        /// </summary>
        /// <param name="token">判定対象のトークン</param>
        /// <returns>判定結果</returns>
        private bool IsStartPunctuationToken(Token token)
        {
            if (token is PunctuationToken) return (token as PunctuationToken).Type == PunctuationToken.PunctuationType.Start;
            return false;
        }

        /// <summary>
        /// 指定トークンが、数式の終端区切りであるか否かを取得します。
        /// </summary>
        /// <param name="token">判定対象のトークン</param>
        /// <returns>判定結果</returns>
        private bool IsEndPunctuationToken(Token token)
        {
            if (token is PunctuationToken) return (token as PunctuationToken).Type == PunctuationToken.PunctuationType.End;
            return false;
        }

        /// <summary>
        /// 数式トークンと演算子トークンから成るトークン間を解析し、解析された数式トークンを取得します。
        /// </summary>
        /// <param name="start">開始位置の数式トークンもしくは演算子トークン。</param>
        /// <param name="end">終了位置の数式トークンもしくは演算子トークン。</param>
        /// <returns>解析で得られた数式トークン。</returns>
        /// <exception cref="FormulaParseException">演算トークンの関係が不正の場合にスローされます。</exception>
        private FormulaToken ParseInner(Token start, Token end)
        {
            foreach (var parser in OperatorParsers)
            {
                if (parser is UnaryOperatorParser)
                    ParseUnaryOperatorToken(parser as UnaryOperatorParser, ref start, ref end);
                else if (parser is SeriesOperatorParser)
                    ParseSeriesOperatorToken(parser as SeriesOperatorParser, ref start, ref end);
                else
                    throw new FormulaParseException();

                if (start == end) break;
            }

            if (start != end) throw new FormulaParseException("演算トークンを正しく解決できませんでした。");
            return start as FormulaToken;
        }

        /// <summary>
        /// 指定トークン範囲内の、継続する<paramref name="operatorParser"/>所属演算トークンを解釈し、数式トークンに置き換えます。
        /// </summary>
        /// <param name="operatorParser">解釈対象の演算解析器。</param>
        /// <param name="beginToken">解釈トークン範囲の開始トークン。</param>
        /// <param name="lastToken">解釈トークン範囲の終了トークン。</param>
        /// <exception cref="FormulaParseException">トークン列の状態が不正な場合にスローされます。</exception>
        private void ParseSeriesOperatorToken(SeriesOperatorParser operatorParser, ref Token beginToken, ref Token lastToken)
        {
            Token currentToken = beginToken;
            FormulaToken initialToken = null;

            bool lastFlag = false;
            while (!lastFlag)
            {
                if (currentToken == null) throw new FormulaParseException("トークン列が不正です。");
                if (currentToken == lastToken) lastFlag = true;

                var formulaToken = currentToken as FormulaToken;
                if (formulaToken == null)
                {
                    currentToken = currentToken.NextToken;
                    continue;
                }

                var operatorToken = currentToken.NextToken as OperatorToken;
                if (!lastFlag && operatorToken == null) throw new FormulaParseException();
                if (operatorToken == null) operatorToken = new OperatorToken(null, null);

                if (operatorParser.IsParseTarget(operatorToken))
                {
                    if (initialToken == null) initialToken = formulaToken;
                }
                else if (initialToken != null)
                {
                    bool isBegin = (initialToken == beginToken);
                    bool isLast  = (formulaToken == lastToken);

                    Formula parsed = ParseSeriesOperator(operatorParser, ref initialToken, ref formulaToken);
                    var newToken = new FormulaToken(initialToken.GetBaseText(formulaToken), parsed);
                    ReplaceToken(initialToken, formulaToken, newToken);

                    if (isBegin) beginToken = newToken;
                    if (isLast)  lastToken  = newToken;

                    currentToken = newToken;
                    initialToken = null;
                }

                currentToken = currentToken.NextToken;
            }
        }

        /// <summary>
        /// 開始位置の数式トークンと終了位置の数式トークンを指定して、その間の指定演算解析器に所属する演算トークンを解決した数式に変換します。
        /// </summary>
        /// <param name="operatorParser">構文解析対象とする演算構文解析器。</param>
        /// <param name="initialToken">解析対象の最初の数式トークン。</param>
        /// <param name="endToken">解析対象の最後の数式トークン。</param>
        /// <returns>解析で得られた数式。</returns>
        /// <exception cref="FormulaParseException">トークン列が想定しない状態となった場合に投げられます。</exception>
        private Formula ParseSeriesOperator(SeriesOperatorParser operatorParser, ref FormulaToken initialToken, ref FormulaToken endToken)
        {
            Token currentToken = initialToken;
            var subsequents = new List<KeyValuePair<OperatorToken, Formula>>();
            OperatorToken currentOperatorToken = null;
            OperatorToken otherOperatorToken = null;
            FormulaToken replacedEndToken = null;

            do
            {
                if (currentToken is OperatorToken &&
                    currentToken.NextToken is OperatorToken &&
                    operatorParser.IsParseTarget(currentToken as OperatorToken) && 
                    operatorParser.IsParseTarget(currentToken.NextToken as OperatorToken)) throw new FormulaParseException("演算子トークンが連続しています。");

                currentToken = currentToken.NextToken;

                if (currentToken is OperatorToken)
                {
                    var operatorToken = currentToken as OperatorToken;
                    if (!operatorParser.IsParseTarget(operatorToken))
                    {
                        if (otherOperatorToken == null) otherOperatorToken = operatorToken;
                    }
                    else
                        currentOperatorToken = operatorToken;
                }
                else if (currentToken is FormulaToken)
                {
                    var formulaToken = currentToken as FormulaToken;
                    Formula f = formulaToken.ParsedFormula;
                    if (otherOperatorToken != null)
                    {
                        var innerParseToken = ParseInner(otherOperatorToken, formulaToken);
                        f = innerParseToken.ParsedFormula;
                        if (formulaToken == endToken) replacedEndToken = innerParseToken;

                        otherOperatorToken = null;
                    }
                    subsequents.Add(new KeyValuePair<OperatorToken, Formula>(currentOperatorToken, f));

                    currentOperatorToken = null;
                }
                else
                    throw new FormulaParseException("想定外のトークンです。");
            }
            while (currentToken != endToken) ;

            if (replacedEndToken != null) endToken = replacedEndToken;

            return operatorParser.GetParsedFormula(initialToken.ParsedFormula, subsequents.ToArray());
        }

        /// <summary>
        /// 指定トークン範囲内の単項演算子トークンを解釈し、数式トークンに置き換えます。
        /// </summary>
        /// <param name="operatorParser">解釈対象の演算解析器。</param>
        /// <param name="beginToken">解釈トークン範囲の開始トークン。</param>
        /// <param name="lastToken">解釈トークン範囲の終了トークン。</param>
        /// <exception cref="FormulaParseException">トークン列が想定しない状態となった場合に投げられます。</exception>
        private void ParseUnaryOperatorToken(UnaryOperatorParser operatorParser, ref Token beginToken, ref Token lastToken)
        {
            var unaries = new List<OperatorToken>();

            FormulaToken previousFormulaToken = null;
            OperatorToken seriesOperatorToken = null;

            Token currentToken = beginToken;
            bool lastFlag = false;
            while (!lastFlag)
            {
                if (currentToken == lastToken) lastFlag = true;

                if (currentToken is OperatorToken)
                {
                    var operatorToken = currentToken as OperatorToken;

                    bool isTargetOperatorParser = (operatorParser.IsParseTarget(operatorToken) && 
                                                  (previousFormulaToken == null || seriesOperatorToken != null));
                    if (isTargetOperatorParser)
                    {
                        unaries.Add(operatorToken);
                    }
                    else
                    {
                        seriesOperatorToken = operatorToken;
                        unaries.Clear();
                    }
                }
                else if (currentToken is FormulaToken)
                {
                    var formulaToken = currentToken as FormulaToken;
                    if (unaries.Count != 0)
                    {
                        Token initialToken = unaries[0];
                        bool isBegin = (initialToken == beginToken);
                        bool isLast  = (formulaToken == lastToken);

                        Formula parsed = ParseUnaryOperator(operatorParser, unaries, formulaToken);
                        var newToken = new FormulaToken(initialToken.GetBaseText(formulaToken), parsed);
                        ReplaceToken(initialToken, formulaToken, newToken);

                        if (isBegin) beginToken = newToken;
                        if (isLast)  lastToken  = newToken;

                        unaries.Clear();
                        previousFormulaToken = newToken;
                        currentToken = newToken;
                    }
                    else
                    {
                        previousFormulaToken = formulaToken;
                        seriesOperatorToken = null;
                    }
                }
                else
                    throw new FormulaParseException("想定外のトークンです。");

                currentToken = currentToken.NextToken;
            }
        }

        /// <summary>
        /// 指定トークンリストを単項式として変換して取得します。
        /// </summary>
        /// <param name="operatorParser">対象の単項演算解析器。</param>
        /// <param name="unaries">変換対象の単項演算トークンリスト。</param>
        /// <param name="sectionToken">単項演算子のかかる数式。</param>
        /// <returns>解析された数式。</returns>
        /// <exception cref="FormulaParseException">トークン列が想定しない状態となった場合に投げられます。</exception>
        private Formula ParseUnaryOperator(UnaryOperatorParser operatorParser, List<OperatorToken> unaries, FormulaToken sectionToken)
        {
            if (sectionToken == null) throw new FormulaParseException("想定外のトークンです。");
            Formula section = sectionToken.ParsedFormula;

            for (int i = unaries.Count - 1; i >= 0; i--)
                section = operatorParser.GetUnaryParsedFormula(unaries[i], section);

            return section;
        }

        /// <summary>
        /// 指定範囲のトークンを新しいトークンで置き換えます。
        /// </summary>
        /// <param name="begin">置換範囲の開始トークン。</param>
        /// <param name="last">置換範囲の末尾トークン。</param>
        /// <param name="newToken">置換後の新しいトークン。</param>
        private void ReplaceToken(Token begin, Token last, Token newToken)
        {
            // トークン列修正
            begin.InsertPrevious(newToken);
            while (newToken.NextToken != last) newToken.NextToken.Remove();
            last.Remove();
        }


    }
}
