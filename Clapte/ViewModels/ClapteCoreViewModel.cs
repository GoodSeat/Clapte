using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.ViewModels.ClapteCommands;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Clapte.Models.Windows;

namespace GoodSeat.Clapte.ViewModels
{
	/// <summary>
	/// ClapteCommandの実行の管理を行います。
	/// </summary>
	public class ClapteCoreViewModel : ISerializable
	{
		ClapteCommandResult _message;

		/// <summary>
		/// ClapteCommandの実行の管理を行うClapteCoreModelViewを初期化します。
		/// </summary>
		public ClapteCoreViewModel()
		{
			this.LimitTime = 10;
			this.ActionWithSameCopy = true;

            ExcludeTargetList = new List<WindowIdentifyInfo>();
			Commands = new List<ClapteCommand>();
			CalculateCommand = new CalculateCommand(this);
			SplitDataCountCommand = new ClapteCommands.SplitDataCountCommand(this);

			Commands.Add(CalculateCommand); // 計算実行コマンド
			Commands.Add(SplitDataCountCommand); // 区切り数値の集計コマンド
			Commands.Add(new DefineConstantCommand(this)); // ユーザー定義定数の定義コマンド
			Commands.Add(new DefineFunctionCommand(this)); // ユーザー定義関数の定義コマンド
			Commands.Add(new DeleteConstantCommand(this)); // ユーザー定義定数の削除コマンド
			Commands.Add(new DeleteFunctionCommand(this)); // ユーザー定義関数の削除コマンド
			Commands.Add(new MultiLineCommand(this));      // 複数行の定義コマンド解釈
		}



		/// <summary>
		/// コマンドの初期化からコマンドに関連付けられたアクション実行までの最大間隔時間[sec]を設定もしくは取得します。
		/// </summary>
		public int LimitTime { get; set; }

		/// <summary>
		/// 同じテキストの連続コピーで、コマンドに関連付けられたアクションを実行するかを設定もしくは取得します。
		/// </summary>
		public bool ActionWithSameCopy {get; set;}

        /// <summary>
        /// 監視対象より除外するウインドウ情報リストを取得します。
        /// </summary>
        public List<WindowIdentifyInfo> ExcludeTargetList { get; private set; }


		/// <summary>
		/// 計算の実行コマンドを設定もしくは取得します。
		/// </summary>
		CalculateCommand CalculateCommand { get; set; }

		/// <summary>
		/// 区切り数値の集計コマンドを取得します。
		/// </summary>
		public SplitDataCountCommand SplitDataCountCommand { get; private set; }


		/// <summary>
		/// 候補となる実行コマンドのリストを設定もしくは取得します。
		/// </summary>
		List<ClapteCommand> Commands { get; set; }

		/// <summary>
		/// 現在有効な実行コマンドを設定もしくは取得します。
		/// </summary>
		ClapteCommand CurrentCommand { get; set; }

		/// <summary>
		/// 最後に通知された命令テキストを設定もしくは取得します。
		/// </summary>
		string LastTextCommand { get; set; }

		/// <summary>
		/// 最後にコマンドが更新された日時を設定もしくは取得します。
		/// </summary>
		DateTime LastUpdateTime { get; set; }

		/// <summary>
		/// ClapteCommandで使用されるソルバを取得します。
		/// </summary>
		public SolverViewModel Solver { get { return CalculateCommand.Solver; } }

		/// <summary>
		/// 最後に発行されたコマンド実行結果もしくはコマンド初期化結果を取得します。
		/// </summary>
		public ClapteCommandResult Message
		{
			get { return _message; }
			private set
			{
				_message = value;
				LastUpdateTime = DateTime.Now;

				if (value != null && MessageUpdate != null) MessageUpdate(this, EventArgs.Empty);
			}
		}



		/// <summary>
		/// コマンド実行結果もしくはコマンド初期化結果が更新されたときに呼び出されます。
		/// </summary>
		public event EventHandler MessageUpdate;



		/// <summary>
		/// ClapteCommand用の命令テキストを通知します。
		/// </summary>
		/// <param name="text">命令テキスト。</param>
		public void InformTextCommand(string text)
		{
			if (ActionWithSameCopy && text == LastTextCommand && IsValidTime())
			{
				InformAction();
			}
			else
			{
				var targetText = ClapteCommand.RemoveCommentTextFrom(text);
				for (int i = 0; i < Commands.Count; i++)
				{
					var result = Commands[i].TryInitializeCommand(targetText);
					if (result != null)
					{
						Message = result;
						LastTextCommand = text;
						CurrentCommand = Commands[i];
						break;
					}
				}
			}
		}

		/// <summary>
		/// 最後に初期化されたコマンドに関連付けられたアクションを実行します。
		/// </summary>
		public void InformAction()
		{
			if (!IsValidTime()) return;
			if (CurrentCommand == null) return;

			Message = CurrentCommand.DoAction();
		}

		/// <summary>
		/// 現在時刻が、現在のコマンドの有効期間内にあるか否かを取得します。
		/// </summary>
		/// <returns>コマンドの有効期間内か否か。</returns>
		public bool IsValidTime()
		{
			return (DateTime.Now - LastUpdateTime).TotalSeconds <= LimitTime;
		}



		#region ISerializable メンバー

		public void OnDeserialize(XmlElement xmlElement)
		{
			ActionWithSameCopy = bool.Parse(xmlElement.GetAttribute("ActionWithSameCopy"));
			LimitTime = int.Parse(xmlElement.GetAttribute("LimitTime"));

			CalculateCommand.OnDeserialize(xmlElement["CalculateCommand"]);
			SplitDataCountCommand.OnDeserialize(xmlElement["SplitDataCountCommand"]);

            // 監視除外の設定の復元
            XmlElement excludeSetting = xmlElement.GetElement("ExcludeSetting");
            if (excludeSetting != null)
            {
                ExcludeTargetList.Clear();
                foreach (XmlElement elm in excludeSetting.GetElements("Target"))
                {
                    WindowIdentifyInfo wid = new WindowIdentifyInfo();
                    wid.OnDeserialize(elm);
                    ExcludeTargetList.Add(wid);
                }
            }
		}

		public void OnSerialize(XmlElement xmlElement)
		{
			xmlElement.AddAttribute("ActionWithSameCopy", ActionWithSameCopy.ToString());
			xmlElement.AddAttribute("LimitTime", LimitTime.ToString());

			XmlElement calculateCommandElement = new XmlElement("CalculateCommand");
			CalculateCommand.OnSerialize(calculateCommandElement);
			xmlElement.AddElements(calculateCommandElement);

			XmlElement splitDataCountCommandElement = new XmlElement("SplitDataCountCommand");
			SplitDataCountCommand.OnSerialize(splitDataCountCommandElement);
			xmlElement.AddElements(splitDataCountCommandElement);

            // 監視除外の設定の保存
            XmlElement excludeSetting = new XmlElement("ExcludeSetting");
            foreach (var wid in ExcludeTargetList)
            {
                XmlElement elm = new XmlElement("Target");
                wid.OnSerialize(elm);
                excludeSetting.AddElements(elm);
            }
            xmlElement.AddElements(excludeSetting);
		}

		#endregion
	}
}
