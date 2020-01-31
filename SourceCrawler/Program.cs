using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SourceCrawler
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            

            if (args.Length > 0 && args[0].Equals("-crawldefault"))
            {
                var progressWindow = new RecrawlProgressCommandLine();
                progressWindow.SetProgress(0, "");

                var progressHandler = new Progress<ProgressResult>(value =>
                {
                    var percent = (int)(((double)progressWindow.GetCurrentProgressValue() / (double)progressWindow.GetMaxProgressValue()) * 100);
                    var workingOn = String.Empty;

                    if (value.WorkingOn != null && value.WorkingOn.Contains(".sln"))
                    {
                        workingOn = Path.GetFileName(value.WorkingOn);
                    }
                    else
                    {
                        workingOn = value.WorkingOn;
                    }

                    progressWindow.SetProgress(value.ProgressValue, workingOn);

                    if (value.CloseForm.HasValue && value.CloseForm.Value)
                    {
                        progressWindow.KillSelf();
                    }
                });

                var progress = progressHandler as IProgress<ProgressResult>;
                var t = Task.Run(() =>
                {
                    var defaultRootId = RepositoryUtils.GetDefaultRootId();
                    if (defaultRootId != null)
                    {
                        progress.Report(new ProgressResult {ProgressValue = 0, WorkingOn = "Caching..."});

                        var repo = new RepositoryFile(defaultRootId, progress);

                        progress.Report(new ProgressResult {ProgressValue = 0, WorkingOn = $"Refreshing default repository: {repo.Root.RootPath}..."});
                        repo.CrawlSource(progress);

                        progress.Report(new ProgressResult {ProgressValue = 0, CloseForm = true});
                    }
                    else
                    {
                        //Console.WriteLine("No default source repository exists.");
                    }
                });

                progressWindow.ShowDialog();

            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
        }
    }
}
