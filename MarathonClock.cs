using ClockRunner;
using System.Collections.Generic;
using System.Windows.Forms;

public class MarathonClock : ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private Timer timer;
    private int secondsElapsed;

    public MarathonClock()
    {
        secondsElapsed = 0;
        timer = new Timer();
        timer.Interval = 1; // 1 mili giây
        timer.Tick += OnTimedEvent;
    }

    private void OnTimedEvent(object sender, System.EventArgs e)
    {
        secondsElapsed++;
        Notify();
    }

    public void Start()
    {
        timer.Start();
    }

    public void Stop()
    {
        timer.Stop();
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update(secondsElapsed);
        }
    }

    public string GetFormattedTime()
    {
        int hours = secondsElapsed / 3600;
        int minutes = (secondsElapsed % 3600) / 60;
        int seconds = secondsElapsed % 60;
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}