using System.Collections;
using Archon.SwissArmyLib.Pooling;
using UnityEngine;
using UnityEngine.Networking;

namespace Archon.SwissArmyLib.Coroutines
{
    internal sealed class WaitForWebRequest : CustomYieldInstruction, IPoolableYieldInstruction
    {
        private static readonly Pool<WaitForWebRequest> Pool =
            new Pool<WaitForWebRequest>(() => new WaitForWebRequest());

        public static IEnumerator Create(UnityWebRequestAsyncOperation webRequestOperation)
        {
            var waiter = Pool.Spawn();
            waiter._webRequestOperation = webRequestOperation;
            return waiter;
        }

        private UnityWebRequestAsyncOperation _webRequestOperation;

        private WaitForWebRequest()
        {
        }

        public override bool keepWaiting
        {
            get { return !_webRequestOperation.isDone; }
        }

        public void Despawn()
        {
            _webRequestOperation = null;
            Pool.Despawn(this);
        }
    }
}