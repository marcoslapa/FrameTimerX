using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace FrameTimerX
{
    public class FrameTimer : Frame
    {
        internal readonly Label _innerLabel;
        internal Color _defaultColor;
        internal DateTime _innerTime;
        internal int _innerCount;

        private TimerStrategyAb timerStrategy;

        #region Bindable Events (Commands)
        // Defining the timer bindable events 
        public delegate void TimerHandler(object sender, FrameTimerEventArgs evt);
        //public event TimerHandler Started;

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

        internal virtual void WarningStarted(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            OnStartWarning?.Execute(e);
        }

        internal virtual void Stopped(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            OnStop?.Execute(e);
        }

        internal virtual void Resumed(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            OnResume?.Execute(e);
        }

        internal virtual void Started(FrameTimerEventArgs e)
        {
            // ----- Event handler for value changes.
            OnStart?.Execute(e);
        }
        #endregion

        #region Component Initializing
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
            this.ClockFontNegaviteTextColor = Color.Red;
            Application.Current.PageAppearing += Current_PageAppearing;
        }

        private void Current_PageAppearing(object sender, Page e)
        {
            //Defines the current strategy
            if (this.TimerType == TimerFormats.IntegerCounter) 
                this.timerStrategy = new IntCounterStrategy();
            else 
                this.timerStrategy = new HourMinSecStrategy();
            
            // Init Timer
            ResetClockOrCounter();
            if (IsAutoStarted) this.Start();
        }
        #endregion

        #region Bindable Properties
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
                                                 defaultValue: Color.Red,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: WarningColorPropertyChanged);

        private static void WarningColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (FrameTimer)bindable;
            control.WarningColor = (Color)newValue;
        }


        private int tickVelocity = 1000;
        /// <summary>
        /// Defines the timer velocity in miliseconds (Default is 1000 => 1 Second) 
        /// </summary>
        public int TickVelocity {
            get {
                return this.tickVelocity;
            }
            set {
                this.tickVelocity = value;
            }
        }
        public static readonly BindableProperty TickVelocityProperty = BindableProperty.Create(
                                                         propertyName: "TickVelocity",
                                                         returnType: typeof(int),
                                                         declaringType: typeof(FrameTimer),
                                                         defaultValue: 1000,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: TickVelocityPropertyChanged);

        private static void TickVelocityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (FrameTimer)bindable;
            control.TickVelocity = (int)newValue;
        }
        #endregion

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

        private Color _negativeTextColor = Color.Red;
        /// <summary>
        /// Defines the Clock's Font Textcolor property when a negative value is reached
        /// </summary>
        public Color ClockFontNegaviteTextColor {
            get {
                return this._negativeTextColor;
            }
            set {
                this._negativeTextColor = value;
            }
        }

        public bool EnableWarning { get; set; }


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

        private bool _allowNegativeValues = false;
        /// <summary>
        /// Allows the timer to show negative values
        /// </summary>
        public bool AllowNegativeValues { 
            get {
                return _allowNegativeValues;
            }
            set {
                _allowNegativeValues = value;
            } 
        }

        int _startingSec, _startingMinute, _startingHour, _startingIntegerCounter;
        /// <summary>
        /// Defines the initial integer value to start the counter
        /// </summary>
        public int StartingCounter {
            get {
                return _startingIntegerCounter;
            }
            set {
                _startingIntegerCounter = value;
            }
        }

        /// <summary>
        /// Defines the initial second to start the timer
        /// </summary>
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

        /// <summary>
        /// Defines the initial minute to start the timer
        /// </summary>        
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

        /// <summary>
        /// Defines the initial hour to start the timer
        /// </summary>
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
        /// Defines if the timer is a countdown, or not...
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

        internal bool timerStopped = false;

        public void Resume()
        {
            this.timerStrategy.Resume(this);            
        }

        internal bool _warningStarted = false;

        /// <summary>
        /// Stops the timer, and rises the Stopped event
        /// </summary>
        public void Stop()
        {
            this.timerStopped = true;
            if (this.OnStop != null)
                Stopped(new FrameTimerEventArgs { Counter = 0 });
        }

        internal bool alreadyStarted = false;
        internal bool negativeValueReached = false;

        public void Start()
        {
            this.timerStrategy.Start(this);
        }

        internal bool IsWarningTime(int timerValue)
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

        internal void ChangeWarningBackgourndColor()
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

        internal string GetTimerString()
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
            this.timerStrategy.ResetClockOrCounter(this);

            this._warningStarted = false;
            this._innerLabel.Text = GetTimerString();
        }

        internal TimeSpan GetVelocity()
        {
            return TimeSpan.FromMilliseconds(TickVelocity);
        }
    }
}
