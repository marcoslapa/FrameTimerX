using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace FrameTimerX
{
    public class FrameTimer : Frame
    {
        readonly Label _innerLabel;
        Color _defaultColor;
        DateTime _innerTime;
        int _innerCount;

        // Defining the timer bindable events 
        public delegate void TimerHandler(object sender, FrameTimerEventArgs evt);
        public event TimerHandler Started; 

        public static readonly BindableProperty OnStartProperty =
            BindableProperty.Create(nameof(OnStart), typeof(ICommand), typeof(FrameTimer), default(ICommand));

        public ICommand OnStart {
            get { return (ICommand)GetValue(OnStartProperty); }
            set { SetValue(OnStartProperty, value); }
        }

        public static readonly BindableProperty OnStartWarningProperty =
            BindableProperty.Create(nameof(OnStartWarning), typeof(ICommand), typeof(FrameTimer), default(ICommand));

        public ICommand OnStartWarning {
            get { return (ICommand)GetValue(OnStartWarningProperty); }
            set { SetValue(OnStartWarningProperty, value); }
        }

        public static readonly BindableProperty OnStopProperty =
            BindableProperty.Create(nameof(OnStop), typeof(ICommand), typeof(FrameTimer), default(ICommand));

        public ICommand OnStop {
            get { return (ICommand)GetValue(OnStopProperty); }
            set { SetValue(OnStopProperty, value); }
        }


        public static readonly BindableProperty OnResumeProperty =
            BindableProperty.Create(nameof(OnResume), typeof(ICommand), typeof(FrameTimer), default(ICommand));

        public ICommand OnResume {
            get { return (ICommand)GetValue(OnResumeProperty); }
            set { SetValue(OnResumeProperty, value); }
        }
        //protected virtual void Started(FrameTimerEventArgs e)
        //{
        //    // ----- Event handler for value changes.
        //    OnStart?.Execute(e);
        //}

        protected virtual void WarningStarted(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            OnStartWarning?.Execute(e);
        }

        protected virtual void Stopped(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            // OnStop?.Invoke(this, e);
            OnStop?.Execute(e);
        }

        protected virtual void Resumed(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            OnResume?.Execute(e);
        }

        public FrameTimer()
        {
            _innerLabel = new Label {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap,
            };
            this.Content = _innerLabel;
            this.TimerType = TimerFormats.HourMinuteSecond;
            this.AllowNegativeValues = false;
            //this.TickVelocity = 1; // Velocity of 1 tick/sec
        }
        
        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (IsAutoStarted) this.Start();
        }

        /// <summary>
        /// Defines the Clock's FontSize
        /// </summary>
        public double ClockFontSize {
            get {
                return this._innerLabel.FontSize;
            }
            set {
                this._innerLabel.FontSize = value;
            }
        }

        /// <summary>
        /// Defines the Clock's FontAttributes
        /// </summary>
        public FontAttributes ClockFontAttributes {
            get {
                return this._innerLabel.FontAttributes;
            }
            set {
                this._innerLabel.FontAttributes = value;
            }
        }

        /// <summary>
        /// Defines the Clock's Font Textcolor property
        /// </summary>
        public Color ClockFontTextColor {
            get {
                return this._innerLabel.TextColor;
            }
            set {
                this._innerLabel.TextColor = value;
            }
        }
        public bool EnableWarning { get; set; }

        private Color warningColor = Color.Red;
        /// <summary>
        /// Defines the color of frame's background to alternate when the WarningTime is reached.
        /// </summary>
        public Color WarningColor {
            get {
                return warningColor;
            }

            set {
                warningColor = value;
            }
        }

        public static readonly BindableProperty WarningColorProperty = BindableProperty.Create(
                                                 propertyName: "WarningColor",
                                                 returnType: typeof(Color),
                                                 declaringType: typeof(FrameTimer),
                                                 defaultValue: Color.White,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: WarningColorPropertyChanged);

        private static void WarningColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (FrameTimer)bindable;
            control.WarningColor = (Color)newValue;
        }

        private int startWarningTime = 0;

        /// <summary>
        /// Defines the time value (in seconds) to begin to alternate the WarningColor of the frame.
        /// </summary>
        public int StartWarningTime {
            get {
                return startWarningTime;
            }

            set {
                startWarningTime = value;
            }
        }

        /// <summary>
        /// Defines if the warning should be stopped
        /// </summary>
        public bool IsEndWarningEnabled { get; set; }

        private int endWarningTime = 0;

        /// <summary>
        /// Defines the value (in seconds) to stop the alternating WarningColor.
        /// </summary>
        public int EndWarningTime {
            get {
                return endWarningTime;
            }

            set {
                endWarningTime = value;
            }
        }

        private int startWarningCount = 0;

        /// <summary>
        /// Defines the value of counter to begin to alternate the WarningColor.
        /// </summary>
        public int StartWarningCount {
            get {
                return startWarningCount;
            }

            set {
                startWarningCount = value;
            }
        }

        private int endWarningCount = 0;

        /// <summary>
        /// Defines the value of counter to stop the alternating WarningColor.
        /// </summary>
        public int EndWarningCount {
            get {
                return endWarningCount;
            }

            set {
                endWarningCount = value;
            }
        }

        private TimerFormats _timerType;
        /// <summary>
        /// Defines the presentation format for the timer
        ///     "hh:mm:ss" - Hours, Minutes and Seconds
        ///     "mm:ss" - Minutes and Seconds
        ///     "ss" - Seconds
        /// </summary>
        public TimerFormats TimerType {
            get {
                return _timerType;
            }
            set {
                this._timerType = value;
            }
        }

        private double tickVelocity;
        /// <summary>
        /// Defines the timer velocity in seconds 
        /// (Note: You can put any valid double, sou the value 0.5 will be half of a second) 
        /// </summary>
        public double TickVelocity {
            get {
                return this.tickVelocity;
            }
            set {
                this.tickVelocity = value;
            }
        }
        public static readonly BindableProperty TickVelocityProperty = BindableProperty.Create(
                                                         propertyName: "TickVelocity",
                                                         returnType: typeof(double),
                                                         declaringType: typeof(FrameTimer),
                                                         defaultValue: 1.0,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: TickVelocityPropertyChanged);

        private static void TickVelocityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (FrameTimer)bindable;
            control.TickVelocity = Convert.ToDouble(newValue);
        }


        public bool AllowNegativeValues { get; set; }

        int _startingSec, _startingMinute, _startingHour, _startingIntegerCounter;
        public int StartingCounter {
            get {
                return _startingIntegerCounter;
            }
            set {
                _startingIntegerCounter = value;
            }
        }

        public int StartingSecond {
            get {
                return _startingSec;
            }
            set {
                // Validate until 59 secs
                if (value >= 0 && value < 60) _startingSec = value;
                else _startingSec = 0;
            }
        }

        public int StartingMinute {
            get {
                return _startingMinute;
            }
            set {
                // Validate until 59 secs
                if (value >= 0 && value < 60) _startingMinute = value;
                else _startingMinute = 0;
            }
        }

        public int StartingHour {
            get {
                return _startingHour;
            }
            set {
                // Validate negative hours
                if (value >= 0) _startingHour = value;
                else _startingHour = 0;
            }
        }

        /// <summary>
        /// Defines if the timer is a countdown or not
        /// </summary>
        public Boolean IsCountDown {
            get; set;
        }

        /// <summary>
        /// Defines if the timer is started automatically
        /// </summary>
        public Boolean IsAutoStarted {
            get; set;
        }

        private bool timerStopped = false;

        public void Resume()
        {
            timerStopped = false;
            if(this.TimerType != TimerFormats.IntegerCounter)
            {
                if (this.OnResume != null)
                {
                    Resumed(new FrameTimerEventArgs
                    {
                        Hour = this._innerTime.Hour,
                        Minute = this._innerTime.Minute,
                        Second = this._innerTime.Second
                    });
                }
            }
            else
            {
                if (this.OnResume != null)
                {
                    Resumed(new FrameTimerEventArgs
                    {
                        Counter = this._innerCount
                    });
                }
            }
        }

        bool _warningStarted = false;

        public void Stop()
        {
            this.timerStopped = true;
            if (this.OnStop != null)
                Stopped(new FrameTimerEventArgs { Counter = 0 });
        }

        bool alreadyStarted = false;
        public void Start()
        {
            // Starts only once
            if (!alreadyStarted)
            {
                ResetClockOrCounter();
                _defaultColor = this.BackgroundColor;

                // Raise the Started event!
                FrameTimerEventArgs args;
                if (TimerType == TimerFormats.IntegerCounter)
                {
                    args = new FrameTimerEventArgs { Counter = StartingCounter };
                    OnStart?.Execute(args);
                    Started?.Invoke(this, args);
                }
                else
                {
                    args = new FrameTimerEventArgs { Hour = StartingHour, Minute = StartingMinute, Second = StartingSecond };
                    OnStart?.Execute(args);
                    Started?.Invoke(this, args);
                }

                Device.StartTimer(GetVelocity(), () => {

                    // Verify if it has to continue increasing or decreasing the values
                    if (!timerStopped)
                    {
                        if (TimerType == TimerFormats.IntegerCounter)
                        {
                            if (this.IsCountDown) this._innerCount--;
                            else this._innerCount++;
                        }
                        else
                        {
                            this._innerTime = this.IsCountDown ? this._innerTime.AddSeconds(-1) : this._innerTime.AddSeconds(1);
                        }
                    }

                    int timeInSeconds = (this._innerTime.Hour * 360 + this._innerTime.Minute * 60 + this._innerTime.Second);
                    //Debug.WriteLine("timeInSeconds:" + timeInSeconds + " / WarningTime:" + WarningTime);

                    // Verify Automatically stopping...
                    if (this.TimerType == TimerFormats.IntegerCounter)
                    {
                        // If IsCountDown and IsNegativeEnabled == false, stops automatically when reach 0.
                        if (this.IsCountDown && !AllowNegativeValues && this._innerCount == 0)
                        {
                            timerStopped = true;
                            if (this.OnStop != null)
                                Stopped(new FrameTimerEventArgs { Counter = 0 });
                        }
                    }
                    else
                    {
                        // If IsCountDown and IsNegativeEnabled == false, stops automatically when reach 00:00:00.
                        if (this.IsCountDown && !AllowNegativeValues
                            && this._innerTime.Hour == 0
                            && this._innerTime.Minute == 0
                            && this._innerTime.Second == 0)
                        {
                            timerStopped = true;
                            if (this.OnStop != null)
                                Stopped(new FrameTimerEventArgs { Hour = 0, Minute = 0, Second = 0 });
                            //OnStop(this, new FrameTimerEventArgs { Hour = 0, Minute = 0, Second = 0 });
                        }
                    }

                    if (!timerStopped)
                    {
                        // Verify if it is time to warning
                        if (TimerType != TimerFormats.IntegerCounter)
                        {
                            // Verify if is the moment for Warning...
                            if (IsWarningTime(timeInSeconds))
                            {
                                if (!_warningStarted)
                                {
                                    // First time Warning!
                                    _warningStarted = true;

                                    if (this.OnStartWarning != null)
                                        WarningStarted(new FrameTimerEventArgs
                                        {
                                            Hour = this._innerTime.Hour,
                                            Minute = this._innerTime.Minute,
                                            Second = this._innerTime.Second
                                        });
                                }

                                ChangeWarningBackgourndColor();
                            }
                        }
                        else
                        {
                            // Verify if is the moment for Warning...
                            if (IsWarningTime(this._innerCount))
                            {
                                // Fires the OnStartWarning event
                                if (!_warningStarted)
                                {
                                    // First time Warning!
                                    _warningStarted = true;
                                    if (this.OnStartWarning != null)
                                        WarningStarted(new FrameTimerEventArgs
                                        {
                                            Counter = this.startWarningCount
                                        });
                                }

                                ChangeWarningBackgourndColor();
                            }
                        }
                    }

                    alreadyStarted = true;

                    this._innerLabel.Text = GetTimerString();

                    return true;
                });
            }
        }

        private bool IsWarningTime(int timerValue)
        {
            bool isWarningTime = false;

            if (TimerType == TimerFormats.IntegerCounter)
            {
                isWarningTime = IsCountDown ? EnableWarning && timerValue <= this.StartWarningCount : EnableWarning && timerValue >= this.StartWarningCount;
            }
            else
            {
                isWarningTime = IsCountDown ? EnableWarning && timerValue <= this.StartWarningTime : EnableWarning && timerValue >= this.StartWarningTime;
            }
            
            return isWarningTime;
        }

        private void ChangeWarningBackgourndColor()
        {
            if (timerStopped)
                this.BackgroundColor = WarningColor;
            else
            {
                if (this.BackgroundColor == _defaultColor)
                    this.BackgroundColor = WarningColor;
                else
                    this.BackgroundColor = _defaultColor;
            }
        }

        private string GetTimerString()
        {
            switch (this._timerType)
            {
                case TimerFormats.HourMinuteSecond: return this._innerTime.ToString("HH:mm:ss");
                case TimerFormats.MinuteSecond: return this._innerTime.ToString("mm:ss");
                case TimerFormats.IntegerCounter: return this._innerCount.ToString("0");
                default: return this._innerCount.ToString("0");
            }            
        }

        protected void ResetClockOrCounter()
        {
            if(TimerType != TimerFormats.IntegerCounter)
            {
                this._innerTime = new DateTime(
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    StartingHour,
                    StartingMinute,
                    StartingSecond
                );
            }
            else
            {
                this._innerCount = StartingCounter;
            }

            this._warningStarted = false;
        }

        private TimeSpan GetVelocity()
        {
            return TimeSpan.FromSeconds(TickVelocity);
        }
    }
}
