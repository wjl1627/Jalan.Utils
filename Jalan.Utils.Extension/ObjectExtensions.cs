namespace Jalan.Utils.Extension
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static T TryParse<T>(this object obj, TryParseHandler<T> handler) where T : struct
        {
            if (obj == null)
                return default(T);
            T result;
            if (handler == null)
                return (T)obj;
            if (handler(obj.ToString(), out result))
                return result;
            return default(T);
        }
        public delegate bool TryParseHandler<T>(string value, out T result);
    }
}
