using System;
using System.Collections.Generic;
using System.Text;

namespace Liffom.Reals
{
	/// <summary>
	/// 最大推定値と最小推定値を追跡する有効桁数考慮実数を表します。
	/// </summary>
	[Serializable()]
	public class PrecisionValidReal : ValidReal
	{
		ValidReal _maximum, _minimum;

		/// <summary>
		/// 最大推定値と最小推定値ともに0として有効桁数考慮実数を初期化します。
		/// </summary>
		public PrecisionValidReal()
			: base()
		{
			_maximum = new ValidReal();
			_minimum = new ValidReal();
		}

		/// <summary>
		/// 有効桁数無限大として有効桁数考慮実数を初期化します。
		/// </summary>
		public PrecisionValidReal(double data)
			: base(data)
		{
			_maximum = new ValidReal(data);
			_minimum = new ValidReal(data);
		}

		/// <summary>
		/// 考慮誤差を考慮した有効桁数考慮実数を初期化します。
		/// </summary>
		/// <param name="value"></param>
		public PrecisionValidReal(string value)
			: base(value)
		{
			_maximum = new ValidReal(base.Maximum);
			_minimum = new ValidReal(base.Minimum);
		}

		public override double Maximum
		{
			get 
			{
				if (_maximum == null) return Data;
				else return _maximum; 
			}
		}

		public override double Minimum
		{
			get 
			{
				if (_minimum == null) return Data;
				else return _minimum;
			}
		}

		/// <summary>
		/// 推定最大値を設定します。
		/// </summary>
		/// <param name="value">推定最大値</param>
		public void SetMaximum(double value)
		{
			 _maximum = new ValidReal(value); 
		}

		/// <summary>
		/// 推定最小値を設定します。
		/// </summary>
		/// <param name="value">推定最小値</param>
		public void SetMinimum(double value)
		{
			_minimum = new ValidReal(value);
		}

		protected override ValidReal GetEstimated(double bestEstimate, double maxEstimate, double minEstimate)
		{
			ValidReal baseReal = base.GetEstimated(bestEstimate, maxEstimate, minEstimate);

			PrecisionValidReal newValue = new PrecisionValidReal(baseReal.Data);
			newValue.Precision = baseReal.Precision;
			newValue.SetMaximum(Math.Max(maxEstimate, minEstimate));
			newValue.SetMinimum(Math.Min(maxEstimate, minEstimate));
			return newValue;
		}

		public override Real AddTo(Real r)
		{
			PrecisionValidReal n1 = this;
			PrecisionValidReal n2 = r as PrecisionValidReal;
			if (n2 == null) n2 = new PrecisionValidReal(r.Data);

			return GetEstimated(n1.Data + n2.Data, n1.Maximum + n2.Maximum, n1.Minimum + n2.Minimum);
		}

		public override Real MultiplyTo(Real r)
		{
			PrecisionValidReal n1 = this;
			PrecisionValidReal n2 = r as PrecisionValidReal;
			if (n2 == null) n2 = new PrecisionValidReal(r.Data);

			double r1 = n1.Maximum * n2.Maximum;
			double r2 = n1.Maximum * n2.Minimum;
			double r3 = n1.Minimum * n2.Maximum;
			double r4 = n1.Minimum * n2.Minimum;

			double max = Math.Max(r1, r2);
			max = Math.Max(max, r3);
			max = Math.Max(max, r4);
			double min = Math.Min(r1, r2);
			min = Math.Min(min, r3);
			min = Math.Min(min, r4);

			return GetEstimated(n1.Data * n2.Data, max, min);
		}

		public override Real PowerWith(Real r)
		{
			PrecisionValidReal n1 = this;
			PrecisionValidReal n2 = r as PrecisionValidReal;
			if (n2 == null) n2 = new PrecisionValidReal(r.Data);

			double r1 = Math.Pow(n1.Maximum, n2.Maximum);
			double r2 = Math.Pow(n1.Maximum, n2.Minimum);
			double r3 = Math.Pow(n1.Minimum, n2.Maximum);
			double r4 = Math.Pow(n1.Minimum, n2.Minimum);

			double max = Math.Max(r1, r2);
			max = Math.Max(max, r3);
			max = Math.Max(max, r4);
			double min = Math.Min(r1, r2);
			min = Math.Min(min, r3);
			min = Math.Min(min, r4);

			return GetEstimated(Math.Pow(n1.Data, n2.Data), max, min);
		}

		public override bool IsEqualTo(Real r)
		{
			PrecisionValidReal n1 = this;
			PrecisionValidReal n2 = r as PrecisionValidReal;
			if (n2 == null) n2 = new PrecisionValidReal(r.Data);

			return n1.ToString() == n2.ToString();
		}

	}
}
