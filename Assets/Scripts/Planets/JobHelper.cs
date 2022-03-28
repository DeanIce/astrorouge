using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace Planets
{
// Created by Domen Koneski
// https://github.com/domenkoneski/unity-jobs-callback
    public class JobHelper : MonoBehaviour
    {
        public delegate void OnJobComplete(JobExecution execution);

        public static bool DebugLog = false;

        /// <summary>
        ///     JobHelper instance. Get instance with GetInstance().
        /// </summary>
        private static JobHelper instance;

        private readonly List<JobExecution> _completedJobs = new();

        private readonly List<JobExecution> _scheduledJobs = new();

        private void Update()
        {
            for (var i = 0; i < _scheduledJobs.Count; i++)
            {
                JobExecution execution = _scheduledJobs[i];
                execution.FramesTaken++;
                execution.Duration += Time.unscaledDeltaTime;
            }

            for (int i = _completedJobs.Count - 1; i >= 0; i--)
            {
                JobExecution execution = _completedJobs[i];
                if (execution.Disposed) _completedJobs.RemoveAt(i);
            }
        }

        private void LateUpdate()
        {
            for (int i = _scheduledJobs.Count - 1; i >= 0; i--)
            {
                JobExecution execution = _scheduledJobs[i];
                if (execution.Handle.IsCompleted && execution.JobWorking || execution.CompleteInLateUpdate)
                {
                    execution.Complete();

                    if (DebugLog)
                        Debug.LogFormat("Job {0} has been completed. Removing it from scheduled jobs.",
                            execution.JobTag);

                    _completedJobs.Add(execution);
                    _scheduledJobs.RemoveAt(i);
                }
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _scheduledJobs.Count; i++)
            {
                _scheduledJobs[i].Handle.Complete();
                _scheduledJobs[i].Dispose();
            }

            for (var i = 0; i < _completedJobs.Count; i++)
            {
                _completedJobs[i].Dispose();
            }
        }

        /// <summary>
        ///     Gets or creates JobHelper instance. Spawns GameObject with JobHelper component.
        /// </summary>
        /// <returns></returns>
        public static JobHelper GetInstance()
        {
            if (instance == null)
            {
                if (DebugLog) Debug.Log("JobHelper instance is null. Creating instance.");
                var jobHelperGameObject = new GameObject("JobHelper_Internal");
                jobHelperGameObject.hideFlags |= HideFlags.HideInHierarchy;
                instance = jobHelperGameObject.AddComponent<JobHelper>();
            }

            return instance;
        }

        private JobExecution AddJob(IJobDisposable job, JobHandle handle, OnJobComplete onJobComplete,
            bool completeImmediatelly = false, string tag = null)
        {
            var execution = new JobExecution(job, handle, onJobComplete, completeImmediatelly, tag);
            _scheduledJobs.Add(execution);
            if (DebugLog) Debug.LogFormat("Job {0} has been scheduled and waiting for completion.", execution.JobTag);
            return execution;
        }

        /// <summary>
        ///     Adds scheduled job to the system.
        /// </summary>
        /// <param name="job">IJobDisposable</param>
        /// <param name="handle">Job Handle with scheduled job</param>
        /// <param name="onJobComplete">Delegate which is invoked after job is completed</param>
        /// <param name="completeImmediatelly">Complete this job immediately in the next LateUpdate() call?</param>
        public static JobExecution AddScheduledJob(IJobDisposable job, JobHandle handle, OnJobComplete onJobComplete,
            bool completeImmediatelly = false, string tag = null)
        {
            return GetInstance().AddJob(job, handle, onJobComplete, completeImmediatelly, tag);
        }

        /// <summary>
        ///     Clears and disposes all completed jobs.
        /// </summary>
        public static void ClearAndDisposeCompletedJobs()
        {
            for (var i = 0; i < GetInstance()._completedJobs.Count; i++)
            {
                GetInstance()._completedJobs[i].Dispose();
            }

            GetInstance()._completedJobs.Clear();
        }

        public class JobExecution
        {
            /// <summary>
            ///     Complete this job immediatelly in LateUpdate()?
            /// </summary>
            public bool CompleteInLateUpdate;

            /// <summary>
            ///     Was IJobDisposable.OnDispose() called?
            /// </summary>
            public bool Disposed;

            /// <summary>
            ///     Job duration time from schedule to completion.
            /// </summary>
            public float Duration;

            /// <summary>
            ///     Frames taken for this job to complete.
            /// </summary>
            public int FramesTaken = -1;

            /// <summary>
            ///     Job handle
            /// </summary>
            public JobHandle Handle;

            private readonly IJobDisposable Job;

            /// <summary>
            ///     Job tag.
            /// </summary>
            public string JobTag;

            /// <summary>
            ///     Is job working?
            /// </summary>
            public bool JobWorking;

            /// <summary>
            ///     OnJobComplete delegate. Called after the job is completed.
            /// </summary>
            public OnJobComplete OnJobComplete;

            public JobExecution(IJobDisposable job, JobHandle handle, OnJobComplete onJobComplete,
                bool completeInLateUpdate, string jobTag)
            {
                Job = job;
                Handle = handle;
                OnJobComplete = onJobComplete;
                CompleteInLateUpdate = completeInLateUpdate;
                JobTag = jobTag != null ? jobTag : GetHashCode().ToString();
                JobWorking = true;
            }

            public bool Complete()
            {
                if (JobWorking)
                {
                    Handle.Complete();
                    JobWorking = false;
                    OnJobComplete(this);
                    return true;
                }

                return false;
            }

            public void Dispose()
            {
                if (Disposed) return;

                Job.OnDispose();
                Disposed = true;
            }
        }

        public interface IJobDisposable
        {
            void OnDispose();
        }
    }
}