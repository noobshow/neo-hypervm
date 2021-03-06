﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NeoSharp.VM.Interop.Extensions;
using NeoSharp.VM.Interop.Native;

namespace NeoSharp.VM.Interop.Types.Collections
{
    public class StackItemStack : Stack
    {
        /// <summary>
        /// Native handle
        /// </summary>
        private readonly IntPtr _handle;

        /// <summary>
        /// Engine
        /// </summary>
        private readonly ExecutionEngine _engine;

        /// <summary>
        /// Return the number of items in the stack
        /// </summary>
        public override int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));

                return NeoVM.StackItems_Count(_handle);
            }
        }

        /// <summary>
        /// Drop object from the stack
        /// </summary>
        /// <param name="count">Number of items to drop</param>
        /// <returns>Return the first element of the stack</returns>
        public override int Drop(int count = 0)
        {
            if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));

            return NeoVM.StackItems_Drop(_handle, count);
        }

        /// <summary>
        /// Pop object from the stack
        /// </summary>
        /// <param name="free">True for free object</param>
        /// <returns>Return the first element of the stack</returns>
        public override StackItemBase Pop()
        {
            if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));

            var ptr = NeoVM.StackItems_Pop(_handle);

            if (ptr == IntPtr.Zero) throw new IndexOutOfRangeException();

            return _engine.ConvertFromNative(ptr);
        }

        /// <summary>
        /// Push object to the stack
        /// </summary>
        /// <param name="item">Object</param>
        public override void Push(StackItemBase item)
        {
            if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));

            NeoVM.StackItems_Push(_handle, ((INativeStackItem)item).Handle);
        }

        /// <summary>
        /// Try to obtain the element at `index` position, without consume them
        /// -1=Last , -2=Last-1 , -3=Last-2
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="obj">Object</param>
        /// <returns>Return tru eif object exists</returns>
        public override bool TryPeek(int index, out StackItemBase obj)
        {
            if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));

            var ptr = NeoVM.StackItems_Peek(_handle, index);

            if (ptr == IntPtr.Zero)
            {
                obj = null;
                return false;
            }

            obj = _engine.ConvertFromNative(ptr);
            return true;
        }

        /// <summary>
        /// Try Pop object casting to this type
        /// </summary>
        /// <typeparam name="TStackItem">Object type</typeparam>
        /// <param name="item">Item</param>
        /// <returns>Return false if it is something wrong</returns>
        public override bool TryPop<TStackItem>(out TStackItem item)
        {
            if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));

            var ptr = NeoVM.StackItems_Pop(_handle);

            if (ptr == IntPtr.Zero)
            {
                item = default(TStackItem);
                return false;
            }

            var ret = _engine.ConvertFromNative(ptr);

            if (ret is TStackItem ts)
            {
                item = (TStackItem)ret;
                return true;
            }

            ret?.Dispose();
            item = default(TStackItem);

            return false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="engine">Engine</param>
        /// <param name="handle">Handle</param>
        internal StackItemStack(ExecutionEngine engine, IntPtr handle)
        {
            _handle = handle;
            _engine = engine;

            if (handle == IntPtr.Zero) throw new ExternalException();
            if (_engine.IsDisposed) throw new ObjectDisposedException(nameof(ExecutionEngine));
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() => Count.ToString();
    }
}