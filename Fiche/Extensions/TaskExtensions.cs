using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class TaskExtensions
    {
        private static readonly FieldInfo m_stateFlags;
        private static readonly MethodInfo optionsMethod;

        private const int continuationTask = 0x0200;
        private const int promiseTask = 0x0400;
        private static readonly Dictionary<Type, FieldInfo> continuationTaskTypes;

        static TaskExtensions()
        {
            m_stateFlags = StaticTypes.taskType.GetField("m_stateFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            optionsMethod = StaticTypes.taskType.GetMethod("OptionsMethod", BindingFlags.Static | BindingFlags.NonPublic);

            //ContinuationTaskFromTask
            //ContinuationResultTaskFromTask<TResult>
            //ContinuationTaskFromResultTask<TAntecedentResult>
            //ContinuationResultTaskFromResultTask<TAntecedentResult, TResult>
            continuationTaskTypes = new Dictionary<Type, FieldInfo>(4);
        }
        private static TaskCreationOptions InternalTaskCreationOptions(this Task task) => (TaskCreationOptions)optionsMethod
                .Invoke(task, new object[1] { m_stateFlags.GetValue(task) });
        /// <summary>
        /// Indicates whether or not the specified <see cref="Task"/> is a promise-style task.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsPromiseStyleTask(this Task task)
        {
            Error.ThrowIfNull(task, nameof(task));
            return task.InternalTaskCreationOptions().HasFlag((TaskCreationOptions)promiseTask);
        }
        /// <summary>
        /// Indicates whether or not the specified <see cref="Task"/> is a continuation task.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsContinuationTask(this Task task)
        {
            Error.ThrowIfNull(task, nameof(task));
            return task.InternalTaskCreationOptions().HasFlag((TaskCreationOptions)continuationTask);
        }
        private static Task InternalAntecedentTask(this Task task)
        {
            Type taskType = task.GetType();
            FieldInfo field;
            if (!continuationTaskTypes.ContainsKey(taskType))
                continuationTaskTypes.Add(taskType, field = taskType.GetField("m_antecedent", BindingFlags.Instance | BindingFlags.NonPublic));
            else
                field = continuationTaskTypes[taskType];
            return (Task)field.GetValue(task);
        }
        /// <summary>
        /// Returns the antecedent task of the specified continuation task.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static Task AntecedentTask(this Task task)
        {
            Error.ThrowIfNull(task, nameof(task));
            Error.ThrowArgumentException(!task.InternalTaskCreationOptions().HasFlag((TaskCreationOptions)continuationTask),
                nameof(task),
                $"The task specified is not a continuation task. {nameof(AntecedentTask)} may only be called on continuation tasks.");
            return task.InternalAntecedentTask();
        }
        /// <summary>
        /// Indicates whether or not the specified task is (or was) scheduled to run.
        /// <para>
        /// This is a short-hand for: (task.Status != TaskStatus.Created)
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsScheduledToRun(this Task task)
        {
            Error.ThrowIfNull(task, nameof(task));
            return !(task.Status == TaskStatus.Created);
        }
        /// <summary>
        /// Synchronously waits for the specified task to complete and returns the result. It starts the task if it was in the state of <see cref="TaskStatus.Created"/; when starting the task, <see cref="TaskScheduler.Current"/> is preferred but if it was null, <see cref="TaskScheduler.Default"/> is used. For promise-style tasks, unless they were expected to be notified by a different executing thread, a deadlock will be produced. For continuation tasks; <see cref="WaitSync(Task)"/> will be called for all antecedent tasks before the continuation of the current call.
        /// </summary>
        /// <typeparam name="T">The type of the result produced by the extended <see cref="System.Threading.Tasks.Task{TResult}"/>.</typeparam>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="TaskCanceledException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="TaskSchedulerException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="NullReferenceException"/>
        /// <exception cref="AbandonedMutexException"/>
        public static T WaitSync<T>(this Task<T> task)
        {
            Error.ThrowIfNull(task, nameof(task));
            TaskCreationOptions options = task.InternalTaskCreationOptions();
            if (options.HasFlag((TaskCreationOptions)continuationTask))
                task.InternalAntecedentTask().WaitSync();
            if (options.HasFlag((TaskCreationOptions)promiseTask))
                Debug.WriteLine($"WARNING: {nameof(WaitSync)} was called for a promise-style task; if this task was not expected to be nodified by a different executing thread, a deadlock will be produced.");
            if (task.Status == TaskStatus.Created)
                task.Start(TaskScheduler.Current ?? TaskScheduler.Default);
            if (task.Status.In(TaskStatus.WaitingForActivation, TaskStatus.WaitingToRun, TaskStatus.Running, TaskStatus.WaitingForChildrenToComplete))
            {
                TaskAwaiter<T> awaiter = task.GetAwaiter();
                //To handle the rare case of instanteous awaiter completion
                if (!awaiter.IsCompleted)
                {
                    //Has less chances for producing a deadlock compared with Task.Wait().
                    ((IAsyncResult)task).AsyncWaitHandle.WaitOne();
                }
            }
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    return task.Result;
                case TaskStatus.Faulted:
                    throw task.Exception;
                case TaskStatus.Canceled:
                    Error.ThrowTaskCanceled(task);
                    return default; //unreachable code
                default:
                    Error.ThrowNotSupported("Unrecognized task status.");
                    return default; //unreachable code
            }
        }
        /// <summary>
        /// Synchronously waits for the speicified task to complete. It starts the task if it was in the state of <see cref="TaskStatus.Created"/; when starting the task, <see cref="TaskScheduler.Current"/> is preferred but if it was null, <see cref="TaskScheduler.Default"/> is used. For promise-style tasks, unless they were expected to be notified by a different executing thread, a deadlock will be produced. For continuation tasks; <see cref="WaitSync(Task)"/> will be called for all antecedent tasks before the continuation of the current call.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="TaskCanceledException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="TaskSchedulerException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="NullReferenceException"/>
        /// <exception cref="AbandonedMutexException"/>
        public static void WaitSync(this Task task)
        {
            Error.ThrowIfNull(task, nameof(task));
            TaskCreationOptions options = task.InternalTaskCreationOptions();
            if (options.HasFlag((TaskCreationOptions)continuationTask))
                task.InternalAntecedentTask().WaitSync();
            if (options.HasFlag((TaskCreationOptions)promiseTask))
                Debug.WriteLine($"WARNING: {nameof(WaitSync)} was called for a promise-style task; if this task was not expected to be nodified by a different executing thread, a deadlock will be produced.");
            if (task.Status == TaskStatus.Created)
                task.Start(TaskScheduler.Current ?? TaskScheduler.Default);
            if (task.Status.In(TaskStatus.WaitingForActivation, TaskStatus.WaitingToRun, TaskStatus.Running, TaskStatus.WaitingForChildrenToComplete))
            {
                TaskAwaiter awaiter = task.GetAwaiter();
                //To handle the rare case of instanteous awaiter completion
                if (!awaiter.IsCompleted)
                {
                    //Has less chances for producing a deadlock compared with Task.Wait().
                    ((IAsyncResult)task).AsyncWaitHandle.WaitOne();
                }
            }
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    return;
                case TaskStatus.Faulted:
                    throw task.Exception;
                case TaskStatus.Canceled:
                    Error.ThrowTaskCanceled(task);
                    return; //unreachable code
                default:
                    Error.ThrowNotSupported("Unrecognized task status.");
                    return; //unreachable code
            }
        }
    }
}
