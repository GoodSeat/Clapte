using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Collections.Specialized;

namespace GoodSeat.Liffom.Utilities
{
    /// <summary>
    /// イベントベースの非同期パターンを表します。
    /// </summary>
    /// <typeparam name="T">処理結果として得られるオブジェクトの型。</typeparam>
    /// <typeparam name="U">処理の入力に用いるオブジェクトの型。</typeparam>
    public abstract class EAP<T, U>
    {
        /// <summary>
        /// 一意のユーザー状態の指定がない時のデフォルトユーザー状態を取得します。
        /// </summary>
        protected Object DefaultState { get; private set; }

        Dictionary<object, bool> CanceledMap { get; set; }
        Dictionary<object, T> ResultMap { get; set; }
        Dictionary<object, IAsyncResult> IAsyncResultMap { get; set; }
        List<object> BusyStates { get; set; }

        private HybridDictionary userStateToLifetime = new HybridDictionary();
        
        private delegate void WorkerEventHandler(AsyncOperation asyncOp, params U[] targets);
        WorkerEventHandler WorkerDelegate { get; set; }

        /// <summary>
        /// 非同期の処理が進行したときに呼び出されます。
        /// </summary>
        public event EAPProgressChangedEventHandler<T> ProgressChanged;

        /// <summary>
        /// 非同期の処理完了時に呼び出されます。
        /// </summary>
        public event EAPCompletedEventHandler<T> ProcessCompleted;


        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        protected virtual void InitializeDelegates()
        {
            onProgressReportDelegate = new SendOrPostCallback(ReportProgress);
            onCompletedDelegate = new SendOrPostCallback(CalculateCompleted);
        }
        private void ReportProgress(object state)
        {
            var e = state as EAPProgressChangedEventArgs<T>;
            OnProgressChanged(e);
        }
        private void CalculateCompleted(object operationState)
        {
            var e = operationState as EAPCompletedEventArgs<T>;
            OnProcessCompleted(e);
        }


        /// <summary>
        /// イベントベースの非同期処理を初期化します。
        /// </summary>
        public EAP()
        {
            DefaultState = new Object();
            CanceledMap = new Dictionary<object, bool>();
            ResultMap = new Dictionary<object, T>();
            IAsyncResultMap = new Dictionary<object, IAsyncResult>();
            BusyStates = new List<object>();
            WorkerDelegate = new WorkerEventHandler(DoProcessWorker);

            InitializeDelegates();
        }

        /// <summary>
        /// 現在、非同期処理中か否かを取得します。
        /// </summary>
        public bool IsBusy() { return IsBusy(DefaultState); }

        /// <summary>
        /// 現在、非同期処理中か否かを取得します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態</param>
        public bool IsBusy(object userState) { return BusyStates.Contains(userState); }

        /// <summary>
        /// 処理のキャンセルが要求されたか否かを設定もしくは取得します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態</param>
        public bool IsCanceled(object userState)
        {
            if (!CanceledMap.ContainsKey(userState)) return false;
            return CanceledMap[userState];
        }

        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public abstract bool IsSupportMultipleConcurrentInvocations { get; }

        /// <summary>
        /// 処理対象とする引数数の下限値を取得します。
        /// </summary>
        public abstract int TargetArgumentsMinQty { get; }

        /// <summary>
        /// 処理対象とする引数数の上限値を取得します。
        /// </summary>
        public virtual int TargetArgumentsMaxQty { get { return TargetArgumentsMinQty; } }

        /// <summary>
        /// 非同期の処理結果を取得します。
        /// </summary>
        /// <returns>処理結果。処理に失敗した場合、null。</returns>
        public T GetAsyncResult() { return GetAsyncResult(DefaultState); }

        /// <summary>
        /// 非同期の処理結果を取得します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態</param>
        /// <returns>処理結果。処理に失敗した場合、null。</returns>
        public T GetAsyncResult(object userState) { return ResultMap[userState]; }



        /// <summary>
        /// 引数を指定して、処理を実行します。
        /// </summary>
        /// <param name="targets">処理対象の引数。</param>
        public T Do(params U[] targets) { return Do(DefaultState, targets); }

        /// <summary>
        /// 引数を指定して、処理を実行します。
        /// </summary>
        /// <param name="targets">処理対象の数式</param>
        public T Do(object userState, params U[] targets) 
        {
            if (targets.Length < TargetArgumentsMinQty || targets.Length > TargetArgumentsMaxQty) throw new ArgumentOutOfRangeException();

            T result = default(T);
            Exception e = null;
            try { result = OnDo(userState, targets); }
            catch (Exception exc) { e = exc; }

            if (ResultMap.ContainsKey(userState)) ResultMap.Remove(userState);
            ResultMap.Add(userState, result);

            BusyStates.Remove(userState);

            if (e != null) throw e;
            return result;
        }
        
        /// <summary>
        /// 引数を指定して、非同期に処理を実行します。
        /// </summary>
        /// <param name="targets">処理対象の数式</param>
        public void DoAsync(params U[] targets) { DoAsync(DefaultState, targets); }

        /// <summary>
        /// 引数を指定して、非同期に処理を実行します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態</param>
        /// <param name="targets">処理対象の数式</param>
        public void DoAsync(object userState, params U[] targets)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(userState);

            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(userState)) throw new ArgumentException( "ユーザー状態は一意でなければなりません。", "userState");
                userStateToLifetime[userState] = asyncOp;
            }

            if (!IsSupportMultipleConcurrentInvocations && BusyStates.Count != 0) throw new InvalidOperationException(GetType() + "は、複数の同時呼び出しをサポートしていません。");
            if (IsBusy(userState)) throw new InvalidOperationException(string.Format("現在、ユーザー状態「{0}」に関連する処理を実行中です。同じユーザー状態に対して、同時に処理を実行することはできません。", userState));
            BusyStates.Add(userState);

            if (!CanceledMap.ContainsKey(userState))
            {
                CanceledMap.Add(userState, false);
            }
            else
            {
                CanceledMap[userState] = false;
                IAsyncResultMap.Remove(userState);
            }

            var asyncResult = WorkerDelegate.BeginInvoke(asyncOp, targets, null, null);
            IAsyncResultMap.Add(userState, asyncResult);
        }



        /// <summary>
        /// 現在実行中の非同期処理の終了を待ち、その結果を取得します。
        /// </summary>
        /// <returns>非同期処理の結果。失敗した場合はnull。</returns>
        public T Wait() { return Wait(DefaultState); }

        /// <summary>
        /// 現在実行中の非同期処理の終了を待ち、その結果を取得します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態</param>
        /// <returns>非同期処理の結果。失敗した場合はnull。</returns>
        public T Wait(object userState)
        {
            if (IsBusy(userState)) WorkerDelegate.EndInvoke(IAsyncResultMap[userState]);
            return GetAsyncResult(userState);
        }

        /// <summary>
        /// 現在実行中の非同期処理がすべて終了するまで待機します。
        /// </summary>
        public void WaitAll()
        {
            while (BusyStates.Count != 0) WorkerDelegate.EndInvoke(IAsyncResultMap[BusyStates[0]]);
        }



        /// <summary>
        /// 非同期処理を中止します。
        /// </summary>
        public void CancelAsync() { CancelAsync(DefaultState); }

        /// <summary>
        /// 非同期処理を中止します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態</param>
        public void CancelAsync(object userState) { CanceledMap[userState] = true; }



        /// <summary>
        /// 設定に基づいて、一連の特性データ保持クラスに対して、特性データを初期化します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <param name="targets">処理対象。</param>
        private void DoProcessWorker(AsyncOperation asyncOp, params U[] targets)
        {
            object userState = asyncOp.UserSuppliedState;

            T result = default(T);
            Exception exc = null;
            try { result = Do(userState, targets); }
            catch (Exception exception) { exc = exception; }

            lock (userStateToLifetime.SyncRoot)
            {
                userStateToLifetime.Remove(asyncOp.UserSuppliedState);
            }

            var e = new EAPCompletedEventArgs<T>(exc, IsCanceled(userState), userState, result);
            asyncOp.PostOperationCompleted(onCompletedDelegate, e);
        }

        /// <summary>
        /// 指定された引数に対して、処理を実行します。
        /// </summary>
        protected abstract T OnDo(object userState, params U[] targets);


        /// <summary>
        /// 引数処理の進行状況の変化を通知します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <param name="progressPercentage">処理の進行状況。(1～100)</param>
        /// <param name="current">処理中の状態。</param>
        protected void OnProgressChanged(object userState, int progressPercentage, T current)
        {
            var e = new EAPProgressChangedEventArgs<T>(progressPercentage, userState, current);

            if (userStateToLifetime.Contains(userState))
            {
                AsyncOperation asyncOp = userStateToLifetime[userState] as AsyncOperation;
                asyncOp.Post(onProgressReportDelegate, e);
            }
            else
            {
                OnProgressChanged(e);
            }
        }

        /// <summary>
        /// 引数処理の進行状況の変化を通知します。
        /// </summary>
        /// <param name="e">イベント情報。</param>
        protected void OnProgressChanged(EAPProgressChangedEventArgs<T> e)
        {
            if (ProgressChanged != null) ProgressChanged(this, e);
        }

        /// <summary>
        /// 非同期処理の終了を通知します。
        /// </summary>
        /// <param name="e">イベント情報。</param>
        protected void OnProcessCompleted(EAPCompletedEventArgs<T> e)
        {
            if (ProcessCompleted != null) ProcessCompleted(this, e);
        }


    }


}
