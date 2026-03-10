using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
#if !NodeExport
using UnityEditor;
using UnityEngine;
#endif

#pragma warning disable CS0162

namespace NodeEditor
{
    public enum LogLevel
    {
        None,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
    public class LogMessage
    {
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }
    }
    /// <summary>
    /// 日志，建议日志格式：ClassName.FunctionName Desc
    /// </summary>
    public sealed class Log
    {
        public readonly List<string> IgnoreShowEditorError = new List<string>()
        {
            "controls when doing repaint",
        };

        #region 开关定义
        public const bool IsDebugEnabled = true;
        public const bool IsInfoEnabled = true;
        public const bool IsWarningEnabled = true;
        public const bool IsErrorEnabled = true;
        public const bool IsFatalEnabled = true;
        public const bool IsLog2File = true;

        #endregion

        #region 文本定义
        public const string LOG_PREFIX = "【NodeEditor】";
        public const string LOG_SUFFIX = "NodeEditor.log";
        public const string LOG_PREFIX_EXCEPTION = "出现异常";
        // 改为项目工程外路径，避免文件高频import异常
        public static readonly string LOG_DIR = Application.dataPath + "/../NodeEditor/Logs/";

        public static bool LogFileAppend = true;

        private static readonly List<string> exceptionFilter = new List<string>
        {
            "SkillEditor", "NodeGraphProcessor", "NodeEditor", "AIEditor", "NpcEditor", "GamePlayEditor"
        };

        private static string logPath = $"{LOG_DIR}{DateTime.Now.ToString("yyyy-MM-dd")}-{LOG_SUFFIX}";
        /// <summary>
        /// 当前日志路径
        /// </summary>
        public static string LogPath
        {
            get { return logPath; }
            set { logPath = value; }
        }
        #endregion

        #region 变量定义
        private static Log inst;

        public static Log Inst
        {
            get
            {
                if (inst == null)
                {
                    inst = new Log();
#if !NodeExport
                    //将第GraphProcessor一些无关报错采用Log来输出
                    GraphProcessor.Utils.LogError = Error;
#endif
                }
                return inst;
            }
        }
        /// <summary>
        /// 记录日志队列
        /// </summary>
        private readonly ConcurrentQueue<LogMessage> logQueue;
        /// <summary>
        /// 信号
        /// </summary>
        private readonly ManualResetEvent mre;

        private StreamWriter streamWriter;

        public HashSet<string> fatalTroubles = new HashSet<string>();
        public Action<string, object> fatalLogCallBackEvent;

        #endregion
#if !NodeExport
        [InitializeOnLoadMethod]
        private static void OnLoadInit()
        {
            // 注册编辑器退出
            EditorApplication.quitting -= EditorQuit;
            EditorApplication.quitting += EditorQuit;
            // 注册异常信息捕获
            Application.logMessageReceived -= LogCallback;
            Application.logMessageReceived += LogCallback;
        }
#endif

        public static void EditorQuit()
        {
            if (inst != null)
            {
                inst.DoWriteLog();
                inst.Clear();
            }
        }

#if !NodeExport
        private static void LogCallback(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Exception || string.IsNullOrEmpty(stackTrace) || inst == null)
            {
                return;
            }

            try
            {
                foreach (var filter in exceptionFilter)
                {
                    if (stackTrace.Contains(filter))
                    {
                        Log.Fatal($"{condition}\n{stackTrace}");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }
#endif

        public Log()
        {
            if (!IsLog2File)
            {
                return;
            }
            try
            {
                var dirInfo = new DirectoryInfo(LOG_DIR);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    this.EnqueueLog($"日志初始化 创建日志文件夹 : {dirInfo.FullName}", LogLevel.Info);
                }
                if (!File.Exists(LogPath))
                {
                    File.Create(LogPath).Dispose();
                    this.EnqueueLog($"日志初始化 创建日志文件 : {Path.GetFullPath(LogPath)}", LogLevel.Info);
                }
                streamWriter = new StreamWriter(LogPath, LogFileAppend, Encoding.Default);
                // 开线程，队列写日志
                logQueue = new ConcurrentQueue<LogMessage>();
                mre = new ManualResetEvent(false);
                Thread t = new Thread(new ThreadStart(WriteLog));
                t.IsBackground = false;
                t.Start();

                //this.EnqueueLog("日志初始化完成", LogLevel.Info);

                // 清理日志
                var logFiles = dirInfo.GetFiles($"*.log").ToList();
                logFiles.Sort((a, b) =>
                {
                    return b.LastWriteTime.CompareTo(a.LastWriteTime);
                });
                // 保留最近一周日志
                for (int i = logFiles.Count - 1; i >= 7; --i)
                {
                    var logFile = logFiles[i];
                    logFile.Delete();
                    logFiles.RemoveAt(i);
                    this.EnqueueLog($"清理过期日志:{logFile.Name}", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                this.EnqueueLog(ex.ToString(), LogLevel.Fatal);
            }
        }
        private void Clear()
        {
            streamWriter?.Flush();
            streamWriter?.Dispose();
            streamWriter = null;
        }
        /// <summary>
        /// 从队列中写日志至磁盘
        /// </summary>
        private void WriteLog()
        {
            while (true)
            {
                // 等待信号通知
                mre.WaitOne();

                DoWriteLog();

                // 重新设置信号
                mre.Reset();
                Thread.Sleep(1);
            }
        }

        private void DoWriteLog()
        {
            LogMessage msg;
            if (streamWriter != null && logQueue.Count > 0)
            {
                // 判断是否有内容需要如磁盘 从列队中获取内容，并删除列队中的内容
                while (logQueue.Count > 0 && logQueue.TryDequeue(out msg))
                {
                    streamWriter.Write(msg.Message);
                    streamWriter.Write("\r\n\n");
                }
                streamWriter.Flush();
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志文本</param>
        /// <param name="level">等级</param>
        /// <param name="ex">Exception</param>
        public void EnqueueLog(string message, LogLevel level, Exception ex = null)
        {
            if ((level == LogLevel.Debug && IsDebugEnabled)
             || (level == LogLevel.Error && IsErrorEnabled)
             || (level == LogLevel.Fatal && IsFatalEnabled)
             || (level == LogLevel.Info && IsInfoEnabled)
             || (level == LogLevel.Warning && IsWarningEnabled))
            {
                // 判断日志等级，然后写日志
                message = $"{LOG_PREFIX}{level}: {message}";
#if !NodeExport
                switch (level)
                {
                    case LogLevel.Debug:
                        UnityEngine.Debug.Log(message);
                        break;
                    case LogLevel.Info:
                        UnityEngine.Debug.Log(message);
                        break;
                    case LogLevel.Error:
                        UnityEngine.Debug.LogError(message);
                        break;
                    case LogLevel.Warning:
                        UnityEngine.Debug.LogWarning(message);
                        break;
                    case LogLevel.Fatal:
                        UnityEngine.Debug.LogError(message);
                        break;
                }
#else
                System.Console.WriteLine(message);
#endif
                if (IsLog2File)
                {
                    var stackTrace = string.Empty;
                    if (level == LogLevel.Error || level == LogLevel.Fatal)
                    {
                        // 错误及异常才记录堆栈信息
                        stackTrace = $"\n{new System.Diagnostics.StackTrace(true)}";
                    }
                    logQueue?.Enqueue(new LogMessage
                    {
                        Message = Utils.Text.Format("【{0}】{1}{2}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            message,
                            stackTrace),
                        Level = level,
                        Exception = ex
                    });

                    // 通知线程往磁盘中写日志
                    mre?.Set();
                }
            }
        }
        public static void Debug(string format, params object[] args)
        {
            Inst.EnqueueLog(Utils.Text.Format(format, args), LogLevel.Debug);
        }
        public static void Info(string format, params object[] args)
        {
            Inst.EnqueueLog(Utils.Text.Format(format, args), LogLevel.Info);
        }
        public static void Warning(string format, params object[] args)
        {
            Inst.EnqueueLog(Utils.Text.Format(format, args), LogLevel.Warning);
        }
        public static void Error(string format, params object[] args)
        {
            Inst.EnqueueLog(Utils.Text.Format(format, args), LogLevel.Error);
        }
        public static void Fatal(string format, params object[] args)
        {
            Inst.EnqueueLog(Utils.Text.Format(format, args), LogLevel.Fatal);

            //在此添加列表监听
            if (!Inst.fatalTroubles.Contains(format))
            {
                //一个渲染层的Bug目前没有找到根源，先临时解决一下
                foreach(var ignore in Log.inst.IgnoreShowEditorError)
                {
                    if (format.Contains(ignore))
                        return;
                }
                Inst.fatalTroubles.Add(format);
            }
            Inst.fatalLogCallBackEvent?.Invoke(format, inst);
        }
        public static void Exception(Exception e)
        {
            Inst.EnqueueLog(e.ToString(), LogLevel.Fatal, e);
        }

        public static void Exception(string extraMsg,Exception e)
        {
            if (string.IsNullOrEmpty(extraMsg))
            {
                Exception(e);
                return;
            }
            Inst.EnqueueLog($"{extraMsg},{e.ToString()}", LogLevel.Fatal, e);
        }

        public static void EnqueueLog(string message, LogLevel level)
        {
            Inst.EnqueueLog(message, level);
        }
    }
}
