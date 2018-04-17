using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TSWizardDemo
{
    public class Step1 : TSWizards.BaseExteriorStep
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox noShowWelcome;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.IContainer components = null;
        private List<String> m_hosts = new List<string>();

        private IProgress<string> _progress;
        

        public Step1()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            SideBarImage = new Bitmap(typeof(Step1), "customSideBarImage.jpg");

            populateHosts();

            

            
        }

        private void populateHosts()
        {
            Random r = new Random();
            int ip = r.Next(10, 200);
            for (int i = 0; i < 3000; i++)
            {
                m_hosts.Add(@"\\127.0.0.1\\c$");
                m_hosts.Add(@"\\127.0.0.1\\c$\\NonExistingDir");
                m_hosts.Add(String.Format("\\\\{0}.{1}.{2}.{3}\\c$\\NonExistingDir", ip, ip, ip, ip));
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.noShowWelcome = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Description
            // 
            this.Description.Size = new System.Drawing.Size(312, 48);
            this.Description.Text = "Welcome to the Wait On Us ordering wizard!";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.Location = new System.Drawing.Point(24, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Click next to proceed with your order";
            // 
            // noShowWelcome
            // 
            this.noShowWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.noShowWelcome.Location = new System.Drawing.Point(8, 288);
            this.noShowWelcome.Name = "noShowWelcome";
            this.noShowWelcome.Size = new System.Drawing.Size(240, 24);
            this.noShowWelcome.TabIndex = 2;
            this.noShowWelcome.Text = "Don\'t show this welcome screen again";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "In this wizard I will take your order then fetch it in 30 seconds or less; or els" +
    "e its free!";
            // 
            // Step1
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.noShowWelcome);
            this.Controls.Add(this.label1);
            this.Name = "Step1";
            this.NextStep = "Step2";
            this.StepDescription = "Welcome to the Wait On Us ordering wizard!";
            this.StepTitle = "Welcome to the Wait On Us ordering wizard!";
            this.ShowStep += new TSWizards.ShowStepEventHandler(this.onShowStep);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.noShowWelcome, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.Description, 0);
            this.ResumeLayout(false);

        }
        #endregion

        public bool NoShowWelcomeAgain
        {
            get
            {
                return noShowWelcome.Checked;
            }
            set
            {
                noShowWelcome.Checked = value;
            }
        }

        private void onShowStep(object sender, TSWizards.ShowStepEventArgs e)
        {

            Step2 step2 = Wizard.GetStep("Step2") as Step2;
            step2.SetProgressBar(0);
            step2.SetMaximumProgressBar(m_hosts.Count);
            _progress = new Progress<string>(data => step2.IncrementProgressBar(0));

            onDoWorkAsync();
            //if (backgroundWorker1.IsBusy != true)
            //{
            //    backgroundWorker1.RunWorkerAsync();
            //}

            //int nofExistingDirectories = 0;
            //Step2 step2 = Wizard.GetStep("Step2") as Step2;

            //// TODO: Add parallel for loop calculation
            //for (int i = 0; i < m_hosts.Count; i++)
            //{
            //    if (CheckNetworkDrive(m_hosts[i]))
            //    {
            //        nofExistingDirectories++;

            //        float percent = (float)i / (float)m_hosts.Count * 100.0f;
            //        step2.SetProgressBar((int)percent);
            //        step2.SetResult(nofExistingDirectories);
            //    }
            //}


            //Parallel.For(0, m_hosts.Count,
            //    index =>
            //    {
            //        if (CheckNetworkDrive(m_hosts[index]))
            //        {
            //            nofExistingDirectories++;

            //            float percent = (float)index / (float)m_hosts.Count * 100.0f;
            //            step2.SetProgressBar((int)percent);
            //            step2.SetResult(nofExistingDirectories);
            //        }
            //    });
        }

        public async Task<bool> CheckNetworkDrive(string path, TimeSpan timespan)
        {
            var task = Task.Run(() => {
                var di = new DirectoryInfo(path);
                bool res = di.Exists;
                Console.WriteLine(String.Format("CheckNetworkDrive: {0}: {1}", path, di.Exists));
                return res;
            });
            Task t = await Task.WhenAny(task, Task.Delay(timespan));

            bool isExits = (t == task && task.Result);

            _progress.Report(String.Format("_progress: {0}: {1}", path, isExits));

            return isExits;
        }



        // Replace with call to private me
        //private void onDoWork(object sender, DoWorkEventArgs e)
        //{
        //    Task _ = onDoWorkAsync(e);
        //}
        private async Task onDoWorkAsync()
        {
            int nofExistingDirectories = 0;
            Step2 step2 = Wizard.GetStep("Step2") as Step2;

            var tasks = m_hosts.Select(h => CheckNetworkDrive(h, TimeSpan.FromSeconds(300)));
            bool[] results = await Task.WhenAll(tasks);

            /*
            Task entireTask = Task.WhenAll(tasks);
            int i = 0;
            int foundRemoteDirs = 0;
            bool res;
            while (res = await Task.WhenAny(entireTask, Task.Delay(6000)) != entireTask)
            {
                var progressReporter = new ProgressReporter();
                float percent = (float)i / (float)m_hosts.Count * 100.0f;
                step2.SetProgressBar((int)percent);
                i++;

                if (res)
                {
                    step2.SetResult(foundRemoteDirs);
                    foundRemoteDirs++;
                }
            }
            */

            //step2.SetProgressBar(100);
            //bool[] results = await Task.WhenAll(tasks);
            //Trace.WriteLine(results.Count(m => m));
            ////TODO: Add parallel for loop calculation
            //for (int i = 0; i < m_hosts.Count; i++)
            //{
            //    Trace.Write(string.Format("{0}: Checking {1} ....", i, m_hosts[i]));
            //    bool res = await CheckNetworkDrive(m_hosts[i], TimeSpan.FromSeconds(5));
            //    Trace.WriteLine(res.ToString());
            //    //if (res)
            //    //{
            //    //    nofExistingDirectories++;
            //    //    step2.SetResult(nofExistingDirectories);
            //    //}

            //    //float percent = (float)i / (float)m_hosts.Count * 100.0f;
            //    //step2.SetProgressBar((int)percent);
            //}

            //Parallel.For(0, m_hosts.Count,
            //    async index =>
            //    {
            //        bool res = await CheckNetworkDrive(m_hosts[index], TimeSpan.FromSeconds(1));
            //        if (res)
            //        {
            //            nofExistingDirectories++;
            //            step2.SetResult(nofExistingDirectories);
            //        }

            //        float percent = (float)index / (float)m_hosts.Count * 100.0f;
            //        step2.SetProgressBar((int)percent);
            //    });
        }

        //private void onProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    if (Wizard == null)
        //    {
        //        return;
        //    }

        //    Step2 step2 = (Step2)e.UserState;

        //    if (step2 != null)
        //    {
        //        step2.SetProgressBar(e.ProgressPercentage);
        //    }
        //}

    }
}
