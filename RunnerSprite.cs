using System;
using System.Windows.Forms;

namespace ClockRunner
{
    public partial class RunnerSprite : UserControl, IObserver
    {
        public static float trackLength = 4f;
        private string runnerName;
        private double speed;
        private double distanceCovered;
        private bool hasFinished;
        private PictureBox sprite;
        private Label statusLabel;
        private int trackLengthPixel;
        private static readonly Random rand = new Random();
        private string finishTime;
        private int finishOrder;
        private int finishTimeInSeconds;

        public bool HasFinished => hasFinished;
        public int FinishTimeInSeconds => finishTimeInSeconds;

        public string RunnerName => runnerName; // Thuộc tính để lấy tên runner

        public RunnerSprite()
        {
            InitializeComponent();
        }

        public RunnerSprite(string name, int x, int y, int trackLength)
        {
            runnerName = name;
            distanceCovered = 0;
            hasFinished = false;
            this.trackLengthPixel = trackLength;
            speed = rand.Next(1000, 2000) / 100.0f;

            this.Location = new System.Drawing.Point(x, y);
            this.Size = new System.Drawing.Size(trackLength + 50, 60);

            statusLabel = new Label();
            statusLabel.Text = $"{runnerName}: 0/{trackLength} km (Speed: {speed} km/h)";
            statusLabel.Location = new System.Drawing.Point(0, 0);
            statusLabel.AutoSize = true;
            this.Controls.Add(statusLabel);

            sprite = new PictureBox();
            sprite.Size = new System.Drawing.Size(30, 30);
            sprite.Location = new System.Drawing.Point(0, 20);
            sprite.BackColor = System.Drawing.Color.Red;
            this.Controls.Add(sprite);
        }

        public void Update(int timeElapsed)
        {
            if (!hasFinished)
            {
                distanceCovered = speed * (timeElapsed / 3600.0);
                if (distanceCovered >= trackLength)
                {
                    distanceCovered = trackLength;
                    hasFinished = true;
                    finishTimeInSeconds = timeElapsed; // Lưu thời gian hoàn thành
                    int hours = timeElapsed / 3600;
                    int minutes = (timeElapsed % 3600) / 60;
                    int seconds = timeElapsed % 60;
                    finishTime = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
                }

                int position = (int)((distanceCovered / trackLength) * trackLengthPixel);
                sprite.Location = new System.Drawing.Point(position, 20);

                statusLabel.Text = $"{runnerName}: {distanceCovered:F2}/{trackLength} km (Speed: {speed} km/h)" +
                                   (hasFinished ? $" - Finished! ({finishTime})" + (finishOrder > 0 ? $" #{finishOrder}" : "") : "");
            }
        }

        public void SetFinishOrder(int order)
        {
            finishOrder = order;
            if (hasFinished)
            {
                statusLabel.Text = $"{runnerName}: {distanceCovered:F2}/{trackLength} km (Speed: {speed} km/h) - Finished! ({finishTime}) #{finishOrder}";
            }
        }
    }
}