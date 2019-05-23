using nng.Native;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace nng
{
    using static nng.Native.Aio.UnsafeNativeMethods;
    using static nng.Native.Ctx.UnsafeNativeMethods;

    public class SubAsyncCtx<T> : AsyncBase<T>, ISubAsyncContext<T>, IReceiveAsyncContext<T>, ICtx
    {
        public static NngResult<ISubAsyncContext<T>> Create(IMessageFactory<T> factory, ISocket socket)
        {
            var context = new SubAsyncCtx<T> { Factory = factory, Socket = socket };
            Console.WriteLine("1");
            var res = context.InitAio();
            if (res == 0)
            {
                Console.WriteLine("2");
                var ctx = AsyncCtx.Create(socket);
                if (ctx.IsOk())
                {
                    Console.WriteLine("3");
                    context.Ctx = ctx.Ok();
                    // Start receive loop
                    //context.AioCallback(IntPtr.Zero);
                    return NngResult<ISubAsyncContext<T>>.Ok(context);
                }
                Console.WriteLine("2 failed");
                return NngResult<ISubAsyncContext<T>>.Err(ctx.Err());
            }
            return NngResult<ISubAsyncContext<T>>.Fail(res);
        }

        public INngCtx Ctx { get; protected set; }

        /// <summary>
        /// Receive a message.
        /// </summary>
        /// <returns>The receive.</returns>
        /// <param name="token">Token.</param>
        public Task<NngResult<T>> Receive(CancellationToken token)
        {
            lock (sync)
            {
                CheckState();

                tcs = Extensions.CreateReceiveSource<T>(token);
                State = AsyncState.Recv;
                nng_ctx_recv(Ctx.NngCtx, aioHandle);
                return tcs.Task;
            }
        }

        protected override void AioCallback(IntPtr argument)
        {
            var res = 0;
            switch (State)
            {
                case AsyncState.Recv:
                    res = nng_aio_result(aioHandle);
                    if (res != 0)
                    {
                        State = AsyncState.Init;
                        tcs.TrySetNngError(res);
                        return;
                    }
                    State = AsyncState.Init;
                    nng_msg msg = nng_aio_get_msg(aioHandle);
                    var message = Factory.CreateMessage(msg);
                    tcs.TrySetResult(NngResult<T>.Ok(message));
                    break;

                case AsyncState.Init:
                default:
                    tcs.TrySetException(new Exception(State.ToString()));
                    break;
            }
        }

        CancellationTokenTaskSource<NngResult<T>> tcs;
    }
}