namespace Jalan.Utils.Common
{
    public class CacheThread<T> where T : new()
    {
        /// <summary>
        /// 默认2分半钟
        /// </summary>
        private double _defaultTime = 1000 * 30 * 5;
        /// <summary>
        /// 默认启用自动清理数据
        /// </summary>
        private bool _enableDataClean = true;
        /// <summary>
        /// 是否启用数据过期清理(默认清理)
        /// </summary>
        public bool EnableDataClean { get { return _enableDataClean; } set { _enableDataClean = value; } }
        /// <summary>
        /// 设置数据保留时间（毫秒）
        /// </summary>
        public double Intervar { get { return _defaultTime; } set { _defaultTime = value; } }
        private System.Timers.Timer _timer;
        private T _obj;
        private static object _lockObj = new object();
        public T DictionaryData
        {
            get
            {
                _timer.Interval = this.Intervar;
                if (_obj == null)
                    _obj = new T();
                return _obj;
            }
            set
            {
                lock (_lockObj)
                {
                    if (value == null)
                        _obj = new T();
                    else
                        _obj = value;
                    if (this.EnableDataClean)
                        _timer.Start();
                }
            }
        }
        public CacheThread()
        {
            _obj = new T();
            if (this.EnableDataClean)
            {
                _timer = new System.Timers.Timer(this.Intervar);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            }
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.ClearCache();
        }
        /// <summary>
        /// 手动清理当前缓存
        /// </summary>
        public void ClearCache()
        {
            this._obj = default(T);
            System.GC.Collect();
            _timer.Stop();
        }
    }
}
