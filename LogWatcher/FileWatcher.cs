using System;
using System.IO;
using System.Text;
using System.Threading;

namespace LogWatcher
{
    public class FileWatcher
    {
        public Action<string> UpdateAction { get; set; }

        string path;
        Stream fileStream; 
        Thread runThread;
        bool abort = false;

        public FileWatcher(string path)
        {
            this.path = path;
        }

        public void Start()
        {
            fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            runThread = new Thread(new ThreadStart(this.Run));
            runThread.Start();
        }

        public void Stop()
        {
            abort = true;

            runThread.Join();

            abort = false;
        }

        void Run()
        {
            long lastLength = 0;

            do
            {
                FileInfo info = new FileInfo(path);

                if (info.Length > lastLength)
                {
                    byte[] buffer = new byte[info.Length - lastLength];

                    int bytesRead = fileStream.Read(buffer, 0, (int)(info.Length - lastLength));

                    string text = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (UpdateAction != null)
                    {
                        UpdateAction(text);
                    }

                    lastLength = info.Length;
                }

                Thread.Sleep(100);
            }
            while (!abort);
        }
    }
}
