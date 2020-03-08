//The code used here is inspired by this stackoverfow answer: https://stackoverflow.com/a/55519246/3602352
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Fiche.Misc;

namespace Fiche.Threading.Tasks
{
    /// <summary>
    /// Groups multiple calls to a single task. Meaning: if <see cref="CachableTask.Task"/> was requested and there were no prior requests that are still being waited, the factory method is executed and a new task is created; otherwise, if <see cref="CachableTask.Task"/> was requested and there were prior requests that are still being waited, the same waited task will be returned instead of creating a new task.
    /// <para>
    /// This class could implicitly be converted to <see cref="System.Threading.Tasks.Task"/> (by requesting to get the value of <see cref="Task"/>). An object of type <see cref="Func{Task}"/> (of <see cref="System.Threading.Tasks.Task"/>) could implicitly be converted to this class (by using it as a factory and creating a new instance).
    /// </para>
    /// </summary>
    public class CachableTask : IAsyncResult
    {
        private readonly object syncLock;
        private readonly Func<Task> factory;
        private Task currentInvocation;
        private CachableTask() => this.syncLock = new object();
        /// <exception cref="ArgumentNullException"/>
        public CachableTask(Func<Task> factory) : this()
        {
            Error.ThrowIfNull(factory, nameof(factory));
            this.factory = factory;
        }
        public Task Task
        {
            get
            {
                lock (this.syncLock)
                {
                    if (this.currentInvocation is null)
                    {
                        this.currentInvocation = this.factory();
                        this.currentInvocation?.ContinueWith(_ =>
                        {
                            lock (this.syncLock)
                                this.currentInvocation = null;
                        }, TaskContinuationOptions.None);
                    }
                    return this.currentInvocation;
                }
            }
        }
        #region Allow await
        public TaskAwaiter GetAwaiter() => Task.GetAwaiter();
        public object AsyncState => ((IAsyncResult)Task).AsyncState;

        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        public bool IsCompleted => ((IAsyncResult)Task).IsCompleted;
        #endregion
        public static implicit operator Task(CachableTask cachableTask) => cachableTask.Task;
        public static implicit operator CachableTask(Func<Task> taskFactory) => new CachableTask(taskFactory);
    }
    /// <summary>
    /// Groups multiple calls to a single task. Meaning: if <see cref="CachableTask{T}.Task"/> was requested and there were no prior requests that are still being waited, the factory method is executed and a new task is created; otherwise, if <see cref="CachableTask{T}.Task"/> was requested and there were prior requests that are still being waited, the same waited task will be returned instead of creating a new task.
    /// <para>
    /// This class could implicitly be converted to <see cref="System.Threading.Tasks.Task{TResult}"/> (by requesting to get the value of <see cref="Task"/>). An object of type <see cref="Func{Task{TResult}}"/> (of <see cref="Task{TResult}"/>) could implicitly be converted to this class (by using it as a factory and creating a new instance).
    /// </para>
    /// <para>
    /// This class inherits from <see cref="CachableTask"/>.
    /// </para>
    /// </summary>
    public class CachableTask<T> : CachableTask
    {
        /// <exception cref="ArgumentNullException"/>
        public CachableTask(Func<Task<T>> factory) : base(factory) { }
        public new Task<T> Task => (Task<T>)base.Task;
        public new TaskAwaiter<T> GetAwaiter() => Task.GetAwaiter();
        public static implicit operator Task<T>(CachableTask<T> cachableTask) => cachableTask.Task;
        public static implicit operator CachableTask<T>(Func<Task<T>> factory) => new CachableTask<T>(factory);
    }
}
