# FrameTimerX
A cross-platform Timer that runs inside a Xamarin.Forms.Frame

Features:
  * AutoStart
  * CountDown/CountUp
  * Types: hh:mm:ss | mm:ss | IntergerCounter
  * Visual Warning (Frame BackgroundColor Changing)
  * Negative Numbers (IntegerCounter)


### General use:

* Adding the component to your page:
```xml
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"             
             ...             
             xmlns:frt="clr-namespace:FrameTimerX;assembly=FrameTimerX"             
             ...             
             x:Class="YourProject.MainPage">
```         

* FrameTimerX (Type = hh:mm:ss; AutoStart)
```xml
    <frt:FrameTimer IsAutoStarted="True" />
```

* FrameTimerX (Type = IntegerCounter; CountDown; AutoStart; TickVelocity = HalfSecond)
```xml
    <frt:FrameTimer BackgroundColor="LightBlue" WarningColor="Blue"
          StartingCounter="50" TickVelocity="500" EnableWarning="True"           
          IsCountDown="True" IsAutoStarted="True" TimerType="IntegerCounter"           
          StartWarningCount="15" />
```          
          
* FrameTimerX (Type = hh:mm:ss; CountDown; Starting Manually)
```xml
        <frt:FrameTimer x:Name="frtManual" BackgroundColor="LightGreen" WarningColor="Green"
                        StartingMinute="1" StartingSecond="0" EnableWarning="True"  
                        IsCountDown="True" TimerType="MinuteSecond" StartWarningTime="30" 
                        HorizontalOptions="Center" VerticalOptions="Center" />
        <Button Text="Start" Clicked="Button_Clicked"/>
```

  * On the .cs file
  ```cs      
        private void Button_Clicked(object sender, EventArgs e){
            frtManual.Start();
        }
 ```
