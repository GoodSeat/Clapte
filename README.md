# Liffom #

Liffomは、C# 3.5で書かれた数式処理ライブラリです。

### 実行に必要なライブラリ ###

* [SerialWorks](https://bitbucket.org/GoodSeat/serialworks/overview)

### 使い方 ###

名前空間の宣言

```
#!c#
using Liffom;
```

数式オブジェクトの作成
```
#!c#
Formula f1 = Formula.parse("3/2(5+1)");
Formula f2 = Formula.parse("(x+y)(x+y-2y)^2");
```

数式の簡単化（約分は行いますが、小数計算は行いません）
```
#!c#
Formula f1Simplified = f1.Simplified;
Console.WriteLine(f1Simplified); // "1/4"
```

数式の数値化（小数計算まで行います）
```
#!c#
Formula f1Numerated = f1.Numerated;
Console.WriteLine(f1Numerated); // "0.25"
```

数式の展開（和の積を積の和に展開）
```
#!c#
Formula f2Expanded = f2.Expanded;
Console.WriteLine(f2Expanded); // "x*x*x+y*y*y-x*y*y-y*x*x"
```

数式の整理（展開は行われない）
```
#!c#
Formula f2Combined = f2.Combined;
Console.WriteLine(f2Combined); // "(x+y)*(x-y)^2"
```

数式の簡単化（展開＋整理）
```
#!c#
Formula f2Simplified =f2.Simplified;
Console.WriteLine(f2Simplified); // "x^3+y^3-x*y^2-y*x^2"

var result = (f1 + f2).Simplified;
Console.WriteLine(result); //  "(4*x^3+4*y^3-4*x*y^2-4*y*x^2+1)/4"
```

代入して計算
```
#!c#
result = result.Replace(new Variable("x"), 5d); // 名前の一致する変数は同一とみなす
Console.WriteLine(result.Simplified); // "(4*y^3-20*y^2-100*y+501)/4"
```

単位を考慮した計算

関数の計算

方程式の求解

行列計算
