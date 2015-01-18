using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Sio.Utilities
{
	/// <summary>
	/// リスト内の要素が変更した時のイベントデータを表します。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListChangeEventArgs<T> : EventArgs
	{
			/// <summary>
			/// リストイベントデータを初期化します。
			/// </summary>
			/// <param name="item">追加もしくは削除されたアイテム</param>
			public ListChangeEventArgs(params T[] items)
			{ Items = items; }

			/// <summary>
			/// リストイベントデータを初期化します。
			/// </summary>
			/// <param name="index">追加先インデックス</param>
			/// <param name="item">追加されたアイテム</param>
			public ListChangeEventArgs(int index, params T[] items)
			{
				Items = items;
				Index = index;
			}

			/// <summary>
			/// イベントに関連したインデックスを設定もしくは取得します。
			/// </summary>
			public int Index { get; set; }

			/// <summary>
			/// イベントの原因となったアイテムを設定もしくは取得します。
			/// </summary>
			public T[] Items { get; set; }

			/// <summary>
			/// イベントをキャンセルするか否かを設定もしくは取得します（直前イベントでのみ意味を持ちます）。
			/// </summary>
			public bool Cancel { get; set; }
	}
}
