using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Fiche.Extensions;
using Fiche.Threading.Tasks;

using NUnit.Framework;

namespace Fiche.Tests.Threading.Tasks
{
    [TestFixture]
    public class CachableTask_Tests
    {
        #region Setup

        [SetUp]
        public void Setup() => CachableLongRunningTask = new CachableTask<int>(LongRunningTaskFactory);

        private const int longDelay = 1000;
        private static async Task<int> DelayedTask(int milliseconds)
        {
            Task task = Task.Delay(milliseconds);
            await task;
            return task.Id;
        }
        private static Task<int> LongRunningTaskFactory()
            => DelayedTask(longDelay);

        private static CachableTask<int> CachableLongRunningTask;
        #endregion

        [Test]
        public async Task MultipleCalls_AreGrouped()
        {
            //Arrange
            int threadCompletionCounter = 0;
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            HashSet<int> taskIds = new HashSet<int>();
            void threadStart()
            {
                int taskId = ((Task<int>)CachableLongRunningTask).WaitSync();
                taskIds.Add(taskId);
                ++threadCompletionCounter;
                if (threadCompletionCounter == 3) //I'm about to start 3 threads below
                    tcs.SetResult(null);
                throw new System.Exception(); //make sure thread is closed
            };
            Thread thread1 = new Thread(threadStart), thread2 = new Thread(threadStart), thread3 = new Thread(threadStart);

            //Act
            thread1.Start(); thread2.Start(); thread3.Start();
            await tcs.Task;

            //Assert
            Assert.AreEqual(1, taskIds.Count);
        }
        [Test]
        public async Task CoupleMultipleCalls_AreGroupedIntoTwoTasks()
        {
            //Arrange1
            object syncLock = new object();
            int threadCompletionCounter = 0;
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            HashSet<int> taskIds = new HashSet<int>();
            void threadStart()
            {
                int taskId = ((Task<int>)CachableLongRunningTask).WaitSync();
                lock (syncLock)
                {
                    taskIds.Add(taskId);
                    ++threadCompletionCounter;
                    if (threadCompletionCounter == 3) //I'm about to start 3 threads below
                    {
                        threadCompletionCounter = 0; //I will then start another 3, so I need to reset the counter
                        tcs.SetResult(null);
                    }
                }
                throw new System.Exception(); //make sure thread is closed
            };
            Thread thread1 = new Thread(threadStart), thread2 = new Thread(threadStart), thread3 = new Thread(threadStart);

            //Act1
            thread1.Start(); thread2.Start(); thread3.Start();
            await tcs.Task;

            //Arrange2
            tcs = new TaskCompletionSource<object>();
            Thread thread4 = new Thread(threadStart), thread5 = new Thread(threadStart), thread6 = new Thread(threadStart);

            //Act2
            thread4.Start(); thread5.Start(); thread6.Start();
            await tcs.Task;

            //Assert
            Assert.AreEqual(2, taskIds.Count);
        }
    }
}
