using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Sio.Utilities;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;

namespace GoodSeat.Clapte.ViewModels
{
	/// <summary>
	/// 定数定義リストを管理するモデルビューを表します。
	/// </summary>
	public class ConstantListViewModel : ISerializable
	{
		/// <summary>
		/// 定数定義リストを管理するモデルビューを初期化します。
		/// </summary>
		public ConstantListViewModel()
		{
			Target = new ObservableList<ConstantDefine>();
		}

		List<ConstantDefine> _systemConstants;
		ObservableList<ConstantDefine> _target;

		/// <summary>
		/// 設定対象となる定数定義リストを設定もしくは取得します。
		/// </summary>
		public ObservableList<ConstantDefine> Target
		{
			get { return _target; }
			set
			{
				if (_target != null)
				{
					_target.ItemAdding -= new EventHandler<ListChangeEventArgs<ConstantDefine>>(_target_ItemAdding);
					_target.ItemAdded -= new EventHandler<ListChangeEventArgs<ConstantDefine>>(_target_ItemAdded);
					_target.ItemRemoved -= new EventHandler<ListChangeEventArgs<ConstantDefine>>(_target_ItemRemoved);
				}
				_target = value;
				if (_target != null)
				{
					_target.ItemAdding += new EventHandler<ListChangeEventArgs<ConstantDefine>>(_target_ItemAdding);
					_target.ItemAdded += new EventHandler<ListChangeEventArgs<ConstantDefine>>(_target_ItemAdded);
					_target.ItemRemoved += new EventHandler<ListChangeEventArgs<ConstantDefine>>(_target_ItemRemoved);
				}
			}
		}

		/// <summary>
		/// 定義リストに登録された定義が編集されたことを通知します。
		/// </summary>
		/// <param name="defines">変更のあった可変数の定義。</param>
		public void InformDefineChanged(params ConstantDefine[] defines)
		{
			if (DefineChanged != null)
			{
				var e = new ListChangeEventArgs<ConstantDefine>(defines);
				if (defines.Length != 0) e.Index = IndexOf(defines[0]);
				DefineChanged(this, e);
			}
		}

		void _target_ItemAdding(object sender, ListChangeEventArgs<ConstantDefine> e)
		{
			foreach (var define in e.Items)
				foreach (var item in Target)
					if (item.Name == define.Name)
						throw new ArgumentException(string.Format("すでに{0}を対象とした定数定義が存在します。", define.Name));
		}

		void _target_ItemAdded(object sender, ListChangeEventArgs<ConstantDefine> e)
		{
			if (DefineAdded != null) DefineAdded(this, e);
		}

		void _target_ItemRemoved(object sender, ListChangeEventArgs<ConstantDefine> e)
		{
			if (DefineRemoved != null) DefineRemoved(this, e);
		}


		/// <summary>
		/// 定数定義が追加されたときに呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<ConstantDefine>> DefineAdded;

		/// <summary>
		/// 定数定義が削除されたときに呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<ConstantDefine>> DefineRemoved;

		/// <summary>
		/// 定数定義が更新されたときに呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<ConstantDefine>> DefineChanged;


		/// <summary>
		/// システム定義の定数定義をすべて返す反復子を取得します。
		/// </summary>
		/// <returns>システム定義の定数定義をすべて返す反復子。</returns>
		public IEnumerable<ConstantDefine> GetSystemConstants()
		{
			if (_systemConstants == null)
			{
				_systemConstants = new List<ConstantDefine>();
				foreach (var constant in Constant.GetEnableConstants())
				{
					var def = new ConstantDefine(constant.DistinguishedName);
					def.Information = constant.Information;
					def.Define = constant.Value.ToString();
					_systemConstants.Add(def);
				}
			}
			foreach (var def in _systemConstants) yield return def;
		}

		/// <summary>
		/// 指定定義のインデックスを取得します。
		/// </summary>
		/// <param name="target">判定対象の定数定義。</param>
		/// <returns>0から始まるインデックス番号。見つからなかった場合、-1。</returns>
		public int IndexOf(ConstantDefine target)
		{
			for (int i = 0; i < Target.Count; i++)
				if (target.Name == Target[i].Name) return i;

			return -1;
		}

		/// <summary>
		/// 指定インデックスの定数定義を設定もしくは取得します。
		/// </summary>
		/// <param name="index">対象のインデックス番号。</param>
		/// <returns>指定インデックスの定数定義。</returns>
		public ConstantDefine this[int index]
		{
			get { return Target[index]; }
			set 
			{
				Target[index] = value;
				InformDefineChanged(Target[index]); // 変更を通知
			}
		}



		#region ISerializable メンバー

		public void OnDeserialize(XmlElement xmlElement)
		{
			Target.Clear();
			foreach (var element in xmlElement.GetElements("UserConstant"))
			{
				ConstantDefine def = new ConstantDefine();
				def.Name = element.GetAttribute("Name");
				def.Define = element.GetAttribute("Define");
				def.Information = element.GetAttribute("Information");
				Target.Add(def);
			}
		}

		public void OnSerialize(XmlElement xmlElement)
		{
			foreach (var def in Target)
			{
				XmlElement element = new XmlElement("UserConstant");
				element.AddAttribute("Name", def.Name);
				element.AddAttribute("Define", def.Define);
				element.AddAttribute("Information", def.Information);
				xmlElement.AddElements(element);
			}
		}

		#endregion

	}
}
