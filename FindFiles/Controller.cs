using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FindFiles
{
    public class Controller
    {
        private Model model;     
        private SearchStatus searchStatus;
        private Stopwatch stopWatch = new Stopwatch();
        public Controller(Model model)
        {
            this.model = model;
        }
        
        public void StartSearch(string path, string regex)
        {
            model.DeleteAllElements();
            if (searchStatus!=null)
                 searchStatus.IsStopRequested = true;
             searchStatus = new SearchStatus();
             searchStatus.Tick = (fileName,numberMatchFiles,numberAllFiles) =>
             {                               
                 model.AddNewElement(fileName);
                 model.NumberOfMatchingElements = numberMatchFiles;
                 model.NumberOfAllElements = numberAllFiles;
                 model.MeasuredTime = stopWatch.Elapsed;
             };

             Task myTask = Task.Factory.StartNew(SearchFiles(searchStatus,path,regex));
             stopWatch.Restart();
        }
        public void StopSearch()
        {
            searchStatus.IsStopRequested = true;
            model.DeleteAllElements();
            stopWatch.Stop();
        }

        public void UnpauseSearch()
        {
            searchStatus.IsSleepRequested = false;
            stopWatch.Start();
        }

        public void PauseSearch()
        {
            searchStatus.IsSleepRequested = true;
            stopWatch.Stop();
        }

        private static Action SearchFiles(SearchStatus status,string path,string regex)
        {
            return () =>
            {
                var enumerator = Dfs(path, regex, status).GetEnumerator();
                while (!status.IsStopRequested)
                {
                    if (status.IsSleepRequested)
                    {
                        Thread.Sleep(500);
                        continue;
                    }                 
                    if (!enumerator.MoveNext())
                        break;

                    string fileName = enumerator.Current;
                    
                    if (status.Tick != null)
                        status.Tick.Invoke(fileName, 
                                            status.numberOfMatchingFiles,
                                            status.numberOfAllFiles);
                    
                    //Thread.Sleep(500);
                }
            };
        }

        private static IEnumerable<string> Dfs(string path, string regex, SearchStatus status)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string s in files)
                {             
                    string name = Path.GetFileName(s);
                    status.numberOfAllFiles++;
                    if (Regex.IsMatch(name, regex, RegexOptions.IgnoreCase))
                    {
                        status.numberOfMatchingFiles++;
                        yield return s;
                    }
                }

                string[] dirs = Directory.GetDirectories(path);
                foreach (string s in dirs)
                {
                    foreach (var element in Dfs(s, regex, status))
                        yield return element;
                }
            }
        }
    }
    public class SearchStatus
    {
        public int numberOfMatchingFiles { get; set; }
        public int numberOfAllFiles { get; set; }
        public bool IsStopRequested { get; set; }
        public bool IsSleepRequested { get; set; }
        public Action<string,int,int> Tick;
    }
}
