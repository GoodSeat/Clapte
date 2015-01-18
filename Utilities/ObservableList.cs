using System;
using System.Collections.Generic;

namespace GoodSeat.Sio.Utilities
{
	/// <summary>
	/// アイテムの追加、削除を検知し、対応するイベント通知を行うリストを表します。
	/// </summary>
	[Serializable()]
	public class ObservableList<T> : IList<T>
	{
		List<T> _innerList = new List<T>();
		
		/// <summary>
		/// ItemAddingイベントを呼び出します。
		/// </summary>
		/// <param name="index">追加先インデックス</param>
		/// <param name="cancel">追加をキャンセルするか否か</param>
		/// <param name="items">追加アイテム</param>
		protected virtual void OnBeforeAdd(int index, ref bool cancel, params T[] items)
		{
			if (ItemAdding != null)
			{
				ListChangeEventArgs<T> e = new ListChangeEventArgs<T>(index, items);
				ItemAdding(this, e);
				cancel = e.Cancel;
			}
		}

		/// <summary>
		/// ItemAddedイベントを呼び出します。
		/// </summary>
		/// <param name="index">追加先インデックス</param>
		/// <param name="items">追加アイテム</param>
		protected virtual void OnAfterAdd(int index, params T[] items)
		{
			if (ItemAdded != null) ItemAdded(this, new ListChangeEventArgs<T>(index, items));
		}

		/// <summary>
		/// ItemRemovingイベントを呼び出します。
		/// </summary>
		/// <param name="cancel">削除をキャンセルするか否か</param>
		/// <param name="items">削除対象のアイテム</param>
		protected virtual void OnBeforeRemove( ref bool cancel, params T[] items)
		{
			if (ItemRemoving != null)
			{
				ListChangeEventArgs<T> e = new ListChangeEventArgs<T>(items);
				ItemRemoving(this, e);
				cancel = e.Cancel;
			}
		}

		/// <summary>
		/// ItemRemovedイベントを呼び出します。
		/// </summary>
		/// <param name="items">削除されたアイテム</param>
		protected virtual void OnAfterRemove(params T[] items)
		{
			if (ItemRemoved != null) ItemRemoved(this, new ListChangeEventArgs<T>(items));
		}

		#region イベント

		/// <summary>
		/// リストにアイテムが追加される直前に呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<T>> ItemAdding;

		/// <summary>
		/// リストにアイテムが追加された直後に呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<T>> ItemAdded;

		/// <summary>
		/// リストにアイテムが削除される直前に呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<T>> ItemRemoving;

		/// <summary>
		/// リストにアイテムが削除された直後に呼び出されます。
		/// </summary>
		public event EventHandler<ListChangeEventArgs<T>> ItemRemoved;

		#endregion

		#region IList<T> メンバー

		/// <summary>
		/// List 内またはその一部にある値のうち、最初に出現する値の、0 から始まるインデックス番号を返します。 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			return _innerList.IndexOf(item);
		}

		/// <summary>
		/// List 内の指定したインデックスの位置に要素を挿入します。 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item)
		{
			bool cancel = false;
			OnBeforeAdd(index, ref cancel, item);
			if (cancel) return;

			_innerList.Insert(index, item);

			OnAfterAdd(index, item);
		}

		/// <summary>
		/// List の指定したインデックスにある要素を削除します。 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			bool cancel = false;
			OnBeforeRemove(ref cancel, _innerList[index]);
			if (cancel) return;

			T removed = _innerList[index];
			_innerList.RemoveAt(index);

			OnAfterRemove(removed);
		}

		/// <summary>
		/// 指定したインデックスにある要素を取得または設定します。 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				return _innerList[index];
			}
			set
			{
				bool cancel = false;
				OnBeforeRemove(ref cancel, _innerList[index]);
				OnBeforeAdd(index, ref cancel, value);
				if (cancel) return;

				T removed = _innerList[index];
				_innerList[index] = value;

				OnAfterAdd(index, value);
				OnAfterRemove(removed);
			}
		}

		#endregion

		#region ICollection<T> メンバー

		/// <summary>
		/// List の末尾にオブジェクトを追加します。 
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			int index = _innerList.Count;

			bool cancel = false;
			OnBeforeAdd(index, ref cancel, item);
			if (cancel) return;

			_innerList.Insert(index, item);

			OnAfterAdd(index, item);
		}

		/// <summary>
		/// 指定したコレクションの要素を List の末尾に追加します。 
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(params T[] items)
		{
			if (items.Length == 0) return;

			int index = _innerList.Count;

			bool cancel = false;
			OnBeforeAdd(index, ref cancel, items);
			if (cancel) return;

			_innerList.AddRange(items);

			OnAfterAdd(index, items);
		}

		/// <summary>
		/// List から、すべての要素の削除を試みます。 
		/// </summary>
		public void Clear()
		{
			if (Count == 0) return;

			bool cancel = false;
			T[] items = _innerList.ToArray();
			OnBeforeRemove(ref cancel, items);
			if (cancel) return;

			_innerList.Clear();

			OnAfterRemove(items);
		}

		/// <summary>
		/// ある要素が List 内に存在するかどうかを判断します。 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return _innerList.Contains(item);
		}

		/// <summary>
		/// List またはその一部を配列にコピーします。 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			_innerList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// List に実際に格納されている要素の数を取得します。 
		/// </summary>
		public int Count
		{
			get { return _innerList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// List 内で最初に見つかった特定のオブジェクトを削除します。 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			bool cancel = false;
			OnBeforeRemove(ref cancel, item);
			if (cancel) return false;

			bool remove = _innerList.Remove(item);

			if (remove) OnAfterRemove(item);

			return remove;
		}

		#endregion

		/// <summary>
		/// 配列に格納して取得します。
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			T[] array = new T[Count];

			for (int t = 0; t < Count; t++)
				array[t] = this[t];

			return array;
		}

		/// <summary>
		///  List 内の要素を並べ替えます。
		/// </summary>
		public void Sort()
		{
			_innerList.Sort();
		}

		#region IEnumerable<T> メンバー

		public IEnumerator<T> GetEnumerator()
		{
			return _innerList.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _innerList.GetEnumerator();
		}

		#endregion
	}
}
