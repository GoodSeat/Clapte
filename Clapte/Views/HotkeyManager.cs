// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;

namespace GoodSeat.Clapte.Views
{
    /// <summary>
    /// ホットキー登録の管理を行うクラスです。
    /// 参考：http://devlights.hatenablog.com/entry/20070416/p1
    /// </summary>
    public sealed class HotkeyManager : ISerializable, IDisposable
    {
        //
        // モディファイアキー
        //
        const int MOD_ALT = 0x0001;
        const int MOD_CONTROL = 0x0002;
        const int MOD_SHIFT = 0x0004;
        //
        // HotKeyのイベントを示すメッセージID
        //
        const int WM_HOTKEY = 0x0312;
        //
        // HotKey登録の際に指定するID
        // 解除の際や、メッセージ処理を行う際に識別する値となります。
        //
        // 0x0000～0xbfff 内の適当な値を指定.
        //
        const int HOTKEY_ID = 0x0001;

        //
        // HotKey登録関数
        //
        // 登録に失敗(他のアプリが使用中)の場合は、0が返却されます。
        //
        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr hWnd, int id, int modKey, int key);

        //
        // HotKey解除関数
        //
        // 解除に失敗した場合は、0が返却されます。
        //
        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr HWnd, int ID);

        IntPtr _handle;
        EventHandler _hotkeyPressed;
        string _hotkey = "C";
        bool _alt = true;
        bool _ctrl = true;
        bool _enable = false;

        /// <summary>
        /// ホットキー管理クラスを初期化します。
        /// </summary>
        /// <param name="handle">コントロールのハンドル。</param>
        public HotkeyManager(IntPtr handle)
        {
            _handle = handle;
        }

        /// <summary>
        /// ホットキーの使用可否を設定もしくは取得します。
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set 
            {
                bool backUp = _enable;
                _enable = value;

                if (!backUp && _enable) // 有効にした
                {
                    RegisterHotkey(_hotkey, _alt, _ctrl);
                }
                else if (!_enable)
                {
                    UnregisterHotKey();
                }
            }
        }

        /// <summary>
        /// ホットキーのメインキーを表す文字列を取得します。
        /// </summary>
        public string Hotkey { get { return _hotkey; } }

        /// <summary>
        /// 現在有効なホットキーで、Altキーを必要とするか否かを取得します。
        /// </summary>
        public bool Alt { get { return _alt; } }

        /// <summary>
        /// 現在有効なホットキーで、Ctrlキーを必要とするか否かを取得します。
        /// </summary>
        public bool Ctrl { get { return _ctrl; } }

        /// <summary>
        /// ホットキーが押されたときに呼び出されます。
        /// </summary>
        public event EventHandler HotkeyPressed
        {
            add { _hotkeyPressed += value; }
            remove { _hotkeyPressed -= value; }
        }

        /// <summary>
        /// ホットキーを登録します。
        /// </summary>
        /// <param name="hotkey">キーを表す文字</param>
        /// <param name="alt">Altキーを使用するか否か</param>
        /// <param name="ctrl">Ctrlキーを使用するか否か</param>
        /// <returns>登録に成功したか。</returns>
        public bool RegisterHotkey(string hotkey, bool alt, bool ctrl)
        {
            KeysConverter key = new KeysConverter();
            
            if (string.IsNullOrEmpty(hotkey)) return false;
            Keys keycode = (Keys)key.ConvertFromString(hotkey);

            // 先にあるのを解除
            UnregisterHotKey();

            bool result = false;

            if (alt && ctrl)
            {
                if (RegisterHotKey(_handle, HOTKEY_ID, MOD_ALT | MOD_CONTROL, (int)keycode) != 0)
                    result = true;
            }
            else if (alt)
            {
                if (RegisterHotKey(_handle, HOTKEY_ID, MOD_ALT, (int)keycode) != 0)
                    result = true;
            }
            else if (ctrl)
            {
                if (RegisterHotKey(_handle, HOTKEY_ID, MOD_CONTROL, (int)keycode) != 0)
                    result = true;
            }

            if (result)
            {
                _hotkey = hotkey;
                _alt = alt;
                _ctrl = ctrl;
            }

            return result;
        }

        /// <summary>
        /// ホットキーの登録を解除します。
        /// </summary>
        public void UnregisterHotKey()
        {
            UnregisterHotKey(_handle, HOTKEY_ID);
            _enable = false;
        }

        /// <summary>
        /// 管理コントロールのウインドウメッセージを通知します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        public void OnWndProc(Message message)
        {
            if (message.Msg == WM_HOTKEY)
            {
                if (((int)message.WParam) == HOTKEY_ID)
                {
                    if (_hotkeyPressed != null)
                        _hotkeyPressed(this, EventArgs.Empty);
                }
            }
        }
        
        #region ISerializable メンバー

        /// <summary>
        /// 指定したXmlElementから状態を復元します。
        /// </summary>
        /// <param name="xmlElement">復元元Xml要素</param>
        public void OnDeserialize(XmlElement xmlElement)
        {
            _hotkey = xmlElement["Key"].Value;
            _alt = bool.Parse(xmlElement.GetAttribute("Alt"));
            _ctrl = bool.Parse(xmlElement.GetAttribute("Ctrl"));
        }

        /// <summary>
        /// 状態をXmlElementに保存します。
        /// </summary>
        /// <param name="xmlElement">保存先のXmlElement</param>
        public void OnSerialize(XmlElement xmlElement)
        {
            xmlElement.AddElements(new XmlElement("Key", _hotkey));
            xmlElement.AddAttribute("Alt", _alt.ToString());
            xmlElement.AddAttribute("Ctrl", _ctrl.ToString());
        }

        #endregion

        #region IDisposable メンバー

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            UnregisterHotKey();
        }

        #endregion
    }
}
