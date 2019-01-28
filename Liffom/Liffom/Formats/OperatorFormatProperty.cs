using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GoodSeat.Liffom.Formats
{
    /// <summary>
    /// 演算記号を空白で囲うか否かの書式情報を表します。
    /// </summary>
    [Serializable()]
    public class OperatorFormatProperty : FormatProperty
    {
        /// <summary>
        /// 対象とする演算を表します。
        /// </summary>
        [Flags()]
        public enum OperatorType
        {
            /// <summary>
            /// 対象はありません。
            /// </summary>
            None = 0,
            /// <summary>
            /// 累乗。
            /// </summary>
            Power = 1,
            /// <summary>
            /// 乗算。
            /// </summary>
            Product = 2,
            /// <summary>
            /// 加算。
            /// </summary>
            Sum = 4,
            /// <summary>
            /// 比較。
            /// </summary>
            Compare = 8,
            /// <summary>
            /// 論理。
            /// </summary>
            Boolean = 16,
        }


        /// <summary>
        /// 演算記号を空白で囲うか否かの書式情報を初期化します。
        /// </summary>
        public OperatorFormatProperty() : this(OperatorType.None) { }

        /// <summary>
        /// 演算記号を空白で囲うか否かの書式情報を初期化します。
        /// </summary>
        /// <param name="target">対象とする演算タイプ。</param>
        public OperatorFormatProperty(OperatorType target)
        {
            Target = target;
            RegexTable = new Dictionary<OperatorType, Regex>();
        }


        /// <summary>
        /// 演算記号を空白で囲う対象とする演算タイプを設定もしくは取得します。
        /// </summary>
        public OperatorType Target { get; set; }

        private Dictionary<OperatorType, Regex> RegexTable;

        private Regex GetRegexPattern(OperatorType type)
        {
            if (!RegexTable.ContainsKey(type))
            {
                string mark = null;
                switch (type)
                {
                    case OperatorType.Power: mark = @"\^"; break;
                    case OperatorType.Product: mark = @"[*/]"; break;
                    case OperatorType.Sum: mark = @"[+-]"; break;
                    case OperatorType.Compare: mark = @"(\<|\<\=|\>|\>\=|\=|\!\=)"; break;
                    case OperatorType.Boolean: mark = @"[&|]"; break;
                    default: throw new NotImplementedException();
                }

                Regex pattern;
                if (type == OperatorType.Sum) // +と-に対する例外処理
                    pattern = new Regex(@"(?<forword>^.*[^ +-/*^<>=&|(](?<![0-9.]+[eE])) ?(?<mark>" + mark + @")(?<backword>[^ +-/*^<>=&|])");
                else
                    pattern = new Regex(@"(?<forword>^.*[^ /*^<>=&|]) ?(?<mark>" + mark + @")(?<backword>[^ /*^<>=&|])");

                RegexTable.Add(type, pattern);
            }
            return RegexTable[type];
        }

        /// <summary>
        /// OperatorFormatProperty型への暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>指定した属性に対応する属性情報。</returns>
        public static implicit operator OperatorFormatProperty(OperatorType property)
        {
            return new OperatorFormatProperty(property);
        }

        /// <summary>
        /// OperatorType型への暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>演算記号を空白で囲う対象とする演算タイプ。</returns>
        public static implicit operator OperatorType(OperatorFormatProperty property) { return property.Target; }

        /// <summary>
        /// 数式の出力文字列を修正します。ただし、一意識別用の文字列出力では、この処理は省略されます。
        /// </summary>
        /// <param name="ownerFormat">この書式情報を保持する書式オブジェクト。</param>
        /// <param name="baseText">変更前の出力文字列。</param>
        protected internal override string  OnModifyOutputText(Format ownerFormat, string baseText)
        {
            // これより親の数式に設定がある場合、そちらを優先する
            if (ownerFormat.Parent != null)
            {
                foreach (var setting in ownerFormat.Parent.GetAllValidSetting())
                {
                    if (setting is OperatorFormatProperty) return baseText;
                }
            }

            foreach (var type in Enum.GetValues(typeof(OperatorType)))
            {
                OperatorType opType = (OperatorType)type;
                if (opType == OperatorType.None) continue;
                if ((opType & Target) != opType) continue;

                var pattern = GetRegexPattern((OperatorType)type);
                while (pattern.IsMatch(baseText)) baseText = pattern.Replace(baseText, @"${forword} ${mark} ${backword}");
            }
            return baseText; 
        }

    }
}



