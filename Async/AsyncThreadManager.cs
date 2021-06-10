using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBoyEngine.Async
{
    public class AsyncThreadManager
    {
        static object _lock = new object();

        public static AsyncCallBack ErrorCallBack;


        public static void Start(AsyncThreadData data)
        {
            lock(_lock)
            {
                Thread thread = new Thread(_ =>
                {
                    try
                    {
                        data.Method?.Invoke();
                    }
                    catch (Exception) { ErrorCallBack?.Invoke(); }
                    {

                    }
                    data.CallBack?.Invoke();
                });

                thread.IsBackground = true;
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start();
            }
        }
    }
}
