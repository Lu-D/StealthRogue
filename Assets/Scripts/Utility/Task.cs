using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Example Usage of Task

// Equivalent to StartCoroutine(SomeCoroutine())
new Task(SomeCoroutine());
 
// Equivalent to the above, but keeps a handle to the running coroutine
Task t = new Task(SomeCoroutine());
 
// Pause the coroutine at next yield
t.Pause();
 
// Resume it
t.Unpause();
 
// Terminate it
t.Stop();
 
// Test if it's still running.
if(t.Running) {}
 
// Test if it's paused.
if(t.Paused) {}
 
// Receive notification when it terminates.
t.Finished += delegate(bool manual) {
    if(manual)
        Debug.Log("t was stopped manually.");
    else
        Debug.Log("t completed execution normally.");
};
*/

/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
/// It is an error to attempt to start a task that has been stopped or which has
/// naturally terminated.
public class Task
{
    /// Returns true if and only if the coroutine is running.  Paused tasks
    /// are considered to be running.
    public bool Running
    {
        get
        {
            return task.Running;
        }
    }

    /// Returns true if and only if the coroutine is currently paused.
    public bool Paused
    {
        get
        {
            return task.Paused;
        }
    }

    /// Delegate for termination subscribers.  manual is true if and only if
    /// the coroutine was stopped with an explicit call to Stop().
    public delegate void FinishedHandler(bool manual);

    /// Termination event.  Triggered when the coroutine completes execution.
    public event FinishedHandler Finished;

    /// Creates a new Task object for the given coroutine.
    ///
    /// If autoStart is true (default) the task is automatically started
    /// upon construction.
    public Task(IEnumerator c, bool autoStart = true)
    {
        task = TaskManager.CreateTask(c);
        task.Finished += TaskFinished;
        if (autoStart)
            Start();
    }

    /// Begins execution of the coroutine
    public void Start()
    {
        task.Start();
    }

    /// Discontinues execution of the coroutine at its next yield.
    public void Stop()
    {
        task.Stop();
    }

    public void Pause()
    {
        task.Pause();
    }

    public void Unpause()
    {
        task.Unpause();
    }

    void TaskFinished(bool manual)
    {
        FinishedHandler handler = Finished;
        if (handler != null)
            handler(manual);
    }

    TaskManager.TaskState task;
}

public class TaskManager : MonoBehaviour
{
    public class TaskState
    {
        public bool Running
        {
            get
            {
                return running;
            }
        }

        public bool Paused
        {
            get
            {
                return paused;
            }
        }

        public delegate void FinishedHandler(bool manual);
        public event FinishedHandler Finished;

        IEnumerator coroutine;
        bool running;
        bool paused;
        bool stopped;

        public TaskState(IEnumerator c)
        {
            coroutine = c;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public void Start()
        {
            running = true;
            singleton.StartCoroutine(CallWrapper());
        }

        public void Stop()
        {
            stopped = true;
            running = false;
        }

        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = coroutine;
            while (running)
            {
                if (paused)
                    yield return null;
                else
                {
                    if (e != null && e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    else
                    {
                        running = false;
                    }
                }
            }

            FinishedHandler handler = Finished;
            if (handler != null)
                handler(stopped);
        }
    }

    static TaskManager singleton;

    public static TaskState CreateTask(IEnumerator coroutine)
    {
        if (singleton == null)
        {
            GameObject go = new GameObject("TaskManager");
            singleton = go.AddComponent<TaskManager>();
        }
        return new TaskState(coroutine);
    }
}

/* Example Usage of TaskList

//To create a new task, use a unique string key, otherwise error will be thrown
TaskList["Task Key"] = new Task(--coroutine);

*/

/// TaskList allows for the instantiation and deletion of coroutines, organized by dictionary keys
public class TaskList : MonoBehaviour
{
    private Dictionary<string, Task> taskList;

    public TaskList(){
        taskList = new Dictionary<string, Task>();
    }

    public bool Running(string key)
    {
        return taskList[key].Paused;
    }

    public bool Paused(string key)
    {
        return taskList[key].Running;
    }

    public void Pause(string key)
    {
        taskList[key].Pause();
    }

    public void Unpause(string key)
    {
        taskList[key].Unpause();
    }

    public void Start(string key)
    {
        taskList[key].Start();
    }

    public void Stop(string key)
    {
        taskList[key].Stop();
        taskList.Remove(key);
    }

    public Task this[string key]
    {
        set
        {
            createTask(value, key);
        }
    }

    private Task getTask(string key)
    {
        if (!taskList.ContainsKey(key))
        {
            Debug.LogError("Attempt to access nonexistant key: " + key);
            return null;
        }
        else
            return taskList[key];
    }

    private void createTask(Task newItem, string key)
    {
        if (taskList.ContainsKey(key)
        && taskList[key].Running == true)
        {
            Debug.LogError("Attempt to overwrite existing key in taskDic: " + key);
        }
        else
            taskList[key] = newItem;
    }
}
