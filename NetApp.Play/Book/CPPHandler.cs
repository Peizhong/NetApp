using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace NetApp.Play.Book
{
    internal static class NativeMethods
    {
        [DllImport("NetApp.PlayCPP.dll", EntryPoint = "HelloWorld")]
        public extern static int HelloWorld(StringBuilder rntStr);

        [DllImport("NetApp.PlayCPP.dll", EntryPoint = "ShowMe")]
        public extern static int ShowMe();
    }

    /// <summary>
    /// 若一个类A有一个实现了 IDisposable 接口类型的成员并创建（创建而不是接收，必须是由类A创建）它的实例对象，
    /// 则类A也应该实现 IDisposable 接口并在 Dispose 方法中调用所有实现了 IDisposable 接口的成员的 Dispose 方法
    /// </summary>
    class CPPHandler : IDisposable
    {
        /// <summary>
        /// Have we been disposed
        /// </summary>
        private bool disposed;

        private IntPtr unmanagedResource;
        private FileStream managedResource;

        public static int Counter;

        public string Name { get; }

        public CPPHandler(string name)
        {
            unmanagedResource = Marshal.AllocHGlobal(1024);
            Name = name + (Counter++);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before  garbage collection.
        /// 调用者可能并不会主动调用Dispose方法，而终结器会被垃圾回收器调用调用，所以它作为释放资源的补救措施。
        /// </summary>
        ~CPPHandler()
        {
            // Our finalizer should call our Dispose(bool) method with false
            Console.WriteLine($"{Name} ~CPPHandler()");
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="includeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool includeManaged)
        {
            Console.WriteLine($"{Name} Dispose({includeManaged})");
            // Use our disposed flag to allow us to call this method multiple times safely.  
            // This is a requirement when implementing IDisposable
            if (!this.disposed)
            {
                if (includeManaged)
                {
                    if (managedResource != null)
                    {
                        managedResource.Dispose();
                        managedResource = null;
                    }
                    // If we have any managed, IDisposable resources, Dispose of them here.
                    // In this case, we don't, so this was unneeded.
                    // Later, we will subclass this class, and use this section.
                }

                // Always dispose of undisposed unmanaged resources in Dispose(bool)
                if (unmanagedResource != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(unmanagedResource);
                    unmanagedResource = IntPtr.Zero;
                }
            }
            // Mark us as disposed, to prevent multiple calls to dispose from having an effect, 
            // and to allow us to handle ObjectDisposedException
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // We start by calling Dispose(bool) with true
            Console.WriteLine($"{Name} Dispose()");
            this.Dispose(true);
            // 通知垃圾回收器不需要再释放
            GC.SuppressFinalize(this);
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder(100);
            var res = NativeMethods.HelloWorld(sb);
            return res.ToString();
        }
    }
}
