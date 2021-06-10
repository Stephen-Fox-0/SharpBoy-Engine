using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Async
{
    public class AsyncThreadData
    {
        AsyncMethod method;
        AsyncCallBack callBack;

        public AsyncMethod Method => method;
        public AsyncCallBack CallBack => callBack;

        public AsyncThreadData(AsyncMethod method, AsyncCallBack callback)
        {
            this.method = method;
            this.callBack = callback;
        }
    }
}
