using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Timers
{
    public class TaskTimer
    {
       private Timer? _timer;
       private AutoResetEvent? _autoResetEvent;
       private Action? _action;
        


       public DateTime TimerStarted { get; set; }
       public int currentSeconds{get;set;}
       public bool IsTimerStarted { get; set; }
       public  int MaxSeconds{get;set;}

       //added...
       public int TimerTickPeriod { get; set; }=1000;


          
        
        public void PrepareTimer(Action action)
       {
         _action = action;
        currentSeconds=-1;
        _autoResetEvent = new AutoResetEvent(false);
        _timer = new Timer(Execute,_autoResetEvent,1000,TimerTickPeriod);
        TimerStarted = DateTime.Now;
        IsTimerStarted = true;
        }


        

    
    public void Execute(object? stateInfo)
    {
         _action();
         
        currentSeconds= MaxSeconds- (Convert.ToInt32((DateTime.Now - TimerStarted).TotalSeconds));
        if (currentSeconds < 0)
        {
             IsTimerStarted = false;
            _timer.Dispose();
        }   
    }
    }
}