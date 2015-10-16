# Liffom #

Liffomは、C#で書かれた簡易数式処理ライブラリです。

### ライセンス ###

Liffom は MIT License に従います。

すなわち、以下の2点を守っていただければ、目的を問わず自由に利用してかまいません。

1. 著作権表示および本許諾表示をソフトウェアのすべての複製または重要な部分に記載しなければならない。
2. 作者または著作権者は、ソフトウェアに関して何ら責任を負わない。

### 使い方 ###

#### 名前空間の宣言 ####

```
#!c#
using GoodSeat.Liffom.Formulas;
```

#### 数式の作成 ####
```
#!c#
Formula f1 = Formula.Parse("3/2(5+1)");
Formula f2 = Formula.Parse("(x+y)(x+y-2y)^2");
```

#### 数式の簡単化（約分は行うが、小数計算は行わない） ####
```
#!c#
Formula f1Simplified = f1.Simplify();
Console.WriteLine(f1Simplified); // "1/4"
```

#### 数式の数値化（小数計算まで行う） ####
```
#!c#
Formula f1Numerated = f1.Numerate();
Console.WriteLine(f1Numerated); // "0.25"
```

#### 数式の展開（和の積を積の和に展開） ####
```
#!c#
Formula f2Expanded = f2.Expand();
Console.WriteLine(f2Expanded); // "x*x*x+y*y*y-x*y*y-y*x*x"
```

#### 数式の整理（展開は行わない） ####
```
#!c#
Formula f2Combined = f2.Combine();
Console.WriteLine(f2Combined); // "(x+y)*(x-y)^2"
```

#### 数式の簡単化（展開＋整理） ####
```
#!c#
Formula f2Simplified =f2.Simplify();
Console.WriteLine(f2Simplified); // "x^3+y^3-x*y^2-y*x^2"

var result = (f1 + f2).Simplify();
Console.WriteLine(result); //  "(4*x^3+4*y^3-4*x*y^2-4*y*x^2+1)/4"
```

#### 代入して計算 ####
```
#!c#
resultSub = result.Substituted(new Variable("x"), 5d); // 名前の一致する変数は同一とみなす
Console.WriteLine(resultSub.Simplify()); // "(4*y^3-20*y^2-100*y+501)/4"
```

#### 単位を考慮した計算 ####
```
#!c#
using GoodSeat.Liffom.Formulas.Units;

// 単位変換テーブルを作成
UnitConvertTable forceTable = new UnitConvertTable(new Unit("N"), "Force"); // "N"を基準とした"Force"の単位変換テーブルを作成
{
    forceTable.AddRecord(new UnitConvertRecord(Formula.Parse("[kg*m/s^2]"), 1.0)); // "kg*m/s^2" -> "N" に対する変換率を登録
    forceTable.AddRecord(new UnitConvertRecord(new Unit("kgf"), Formula.Parse("9.80665"))); // "kgf" -> "N" に対する変換率を登録
}
UnitConvertTable.AddTable(forceTable); // 単位変換時に参照する単位変換テーブルリスト(静的フィールド)に登録

// 単位変換の実行 ----------------------------------------------------------------------------

// 既定のFormula.Parseでは、以下のものを単位として構文解析します。
//  ・角括弧([])で囲われた文字列
//  ・UnitConvertTableのValidTables に登録された単位
var test = Formula.Parse("5[kgf] + 5[N]"); // 単位変換対象とする数式
var targetUnit = new Unit("kN"); // 変換先の目標単位

var proc = new ConvertToTargetUnit();
Console.WriteLine(proc.Do(test, targetUnit).Numerate()); // "0.04903325[kN] + 0.005[kN]"
```

#### 関数の計算 ####

#### 方程式の求解 ####

#### 行列計算 ####


#### その他の例 ####
その他の使用例については、

* テストプロジェクト(LiffomTestProject)や、
* Clapte[https://bitbucket.org/GoodSeat/clapte/overview]

を参照してください。
