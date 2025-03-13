using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClockRunner
{
    public partial class MainForm : Form
    {
        private MarathonClock clock;
        private List<RunnerSprite> runners;
        private Button startButton;
        private Label clockLabel;
        private Label winnerLabel;
        private const int TRACK_LENGTH = 1000;
        private string winner = null;
        private const int NUMBER_OF_RUNNERS = 10;
        private List<RunnerSprite> finishedRunners;

        public MainForm()
        {
            clock = new MarathonClock();
            runners = new List<RunnerSprite>();
            finishedRunners = new List<RunnerSprite>();
            this.Text = "Marathon Race Simulation with Sprites";
            this.Width = TRACK_LENGTH + 100;
            this.Height = 150 + NUMBER_OF_RUNNERS * 70;

            clockLabel = new Label();
            clockLabel.Text = "Time: 00:00:00";
            clockLabel.Location = new System.Drawing.Point(10, 10);
            clockLabel.AutoSize = true;
            this.Controls.Add(clockLabel);

            startButton = new Button();
            startButton.Text = "Start Race";
            startButton.Location = new System.Drawing.Point(10, 40);
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            winnerLabel = new Label();
            winnerLabel.Text = "Winner: None";
            winnerLabel.Location = new System.Drawing.Point(10, 70 + NUMBER_OF_RUNNERS * 70);
            winnerLabel.AutoSize = true;
            this.Controls.Add(winnerLabel);

            for (int i = 0; i < NUMBER_OF_RUNNERS; i++)
            {
                int yPosition = 80 + i * 70;
                var runner = new RunnerSprite($"Runner {i + 1}", 10, yPosition, TRACK_LENGTH);
                runners.Add(runner);
                this.Controls.Add(runner);
                clock.Attach(runner);
            }

            clock.Attach(new ClockDisplay(clockLabel, clock, this));
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            clock.Start();
            startButton.Enabled = false;
        }

        private class ClockDisplay : IObserver
        {
            private Label label;
            private MarathonClock clock;
            private MainForm form;

            public ClockDisplay(Label label, MarathonClock clock, MainForm form)
            {
                this.label = label;
                this.clock = clock;
                this.form = form;
            }

            public void Update(int timeElapsed)
            {
                label.Text = $"Time: {clock.GetFormattedTime()}";

                // Cập nhật danh sách runner đã hoàn thành
                foreach (var runner in form.runners)
                {
                    if (runner.HasFinished && !form.finishedRunners.Contains(runner))
                    {
                        form.finishedRunners.Add(runner);
                    }
                }

                // Sắp xếp theo thời gian hoàn thành và gán thứ tự
                var sortedRunners = form.finishedRunners.OrderBy(r => r.FinishTimeInSeconds).ToList();
                for (int i = 0; i < sortedRunners.Count; i++)
                {
                    sortedRunners[i].SetFinishOrder(i + 1);
                    if (i == 0 && form.winner == null)
                    {
                        form.winner = sortedRunners[i].Name;
                    }
                }

                // Kiểm tra tất cả runner đã hoàn thành chưa
                bool allFinished = form.runners.TrueForAll(r => r.HasFinished);
                if (allFinished)
                {
                    clock.Stop();
                    form.winnerLabel.Text = $"Winner: {form.finishedRunners[0].RunnerName}";
                }
            }
        }
    }
}