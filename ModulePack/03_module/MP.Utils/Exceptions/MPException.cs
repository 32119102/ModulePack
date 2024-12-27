namespace MP.Exceptions
{
    /// <summary>
    /// OSharp框架异常类
    /// </summary>
    [Serializable]
    public class OsharpException : Exception
    {
        /// <summary>
        /// 初始化<see cref="OsharpException"/>类的新实例
        /// </summary>
        public OsharpException()
        { }

        /// <summary>
        /// 使用指定错误消息初始化<see cref="OsharpException"/>类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public OsharpException(string message)
            : base(message)
        { }

        /// <summary>
        /// 使用异常消息与一个内部异常实例化一个<see cref="OsharpException"/>类的新实例
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="inner">用于封装在<see cref="OsharpException"/>内部的异常实例</param>
        public OsharpException(string message, Exception inner)
            : base(message, inner)
        { }


    }
}
