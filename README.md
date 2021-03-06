# FrameTimerX v1.0.2
**Author**: Marcos Lapa dos Santos

A cross-platform Timer that runs inside a Xamarin.Forms.Frame

**Features**:
  * AutoStart Timer
  * CountDown/CountUp
  * TimerTypes: "hh:mm:ss" | "mm:ss" | IntergerCounter
  * Visual Warning (Frame BackgroundColor Changing)
  * Negative Numbers (Auto Stop on **zero** by default)
---

### Configuration:

1. Add the component to your xaml page:
```xml
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"             
             ...             
             xmlns:frt="clr-namespace:FrameTimerX;assembly=FrameTimerX"             
             ...             
             x:Class="YourProject.MainPage">
```         

2. Creating a simple FrameTimerX (Default Type = "hh:mm:ss"; AutoStart = true)
```xml
    <frt:FrameTimer IsAutoStarted="True" />
```
---

## Examples

 1. Configuring the FrameTimerX for a Integer CountDown, TickVelocity = 500 miliseconds
```xml
    <frt:FrameTimer BackgroundColor="LightBlue" 
          StartingCounter="50" TickVelocity="500"            
          IsCountDown="True" IsAutoStarted="True"
          TimerType="IntegerCounter" />
```          
          
 2. Starting manually a FrameTimerX in a button click
 - **On Xaml file**:
```xml
        <frt:FrameTimer x:Name="frtManual" BackgroundColor="LightGreen" 
                        StartingMinute="1" StartingSecond="0" IsCountDown="True" TimerType="MinuteSecond" 
                        HorizontalOptions="Center" VerticalOptions="Center" />
        <Button Text="Start" Clicked="Button_Clicked"/>
```

 - **On .cs file**:
   ```cs      
   private void Button_Clicked(object sender, EventArgs e){
     frtManual.Start();
   }
   ```

 3. Creating a FrameTimerX with alternate background color on Warning
```xml
        <frt:FrameTimer AllowNegativeValues="True" BackgroundColor="LightCoral" WarningColor="Orange"                       
                        StartingCounter="30" TickVelocity="500" EnableWarning="True" ClockFontNegaviteTextColor="DarkBlue"
                        IsCountDown="True" IsAutoStarted="True" TimerType="IntegerCounter" 
                        StartWarningCount="10" HorizontalOptions="Center" VerticalOptions="Center" />
```

## Handling Events

FrameTimerX has the **Started**, **Stopped**, **Resumed** and **WarningStarted** events to handle. We can use the **CodeBehind** or **MVVM** (Command) aproach. 

### CodeBehind sample

 Handling the Stopped event
 
 **XAML File**
```xml
        <frt:FrameTimer StartingCounter="0" TickVelocity="500" Stopped="FrtTimer_Stopped"
                        IsAutoStarted="True" TimerType="IntegerCounter" />
```

**CS File**
```cs
        private void FrtTimer_Stopped(object sender, FrameTimerEventArgs args)
        {
            Debug.WriteLine("=======================================================");
            Debug.WriteLine("========== Simple Stopped Event raised!!!!! ===========");
            Debug.WriteLine($"  ==> Counter:{args.Counter}");
            Debug.WriteLine($"  ==> Hour:{args.Hour}");
            Debug.WriteLine($"  ==> Minute:{args.Minute}");
            Debug.WriteLine($"  ==> Second:{args.Second}");
            Debug.WriteLine("=======================================================");
        }
```

### MVVM (Bindable Command) sample

 Handling the WarningStarted event
 
 **XAML File** (Remember to link your ViewModel to the Page Context before...)

```xml
        <frt:FrameTimer StartingCounter="90" TickVelocity="1000" TimerType="IntegerCounter"
                        EnableWarning="True" StartWarningTime="30"
                        WarningColor="Orange" IsCountDown="True" 
                        OnStartWarning="{Binding StartWarningCommmand} />
```

**CS File => ViewModel**
```cs
        // Constructor
        public MyViewModel(){
            StartWarningCommmand = new Command(StartWarning);
        }
        
        public Command StartWarningCommmand { get; }

        public void StartWarning()
        {
            Debug.WriteLine("########### Warning Started! ###########");
        }
```

---
### Added Features
- ResetTimerOnAppearing property: Now you can choose if your timer resets on PageAppearing or not.
- Pause method and event (now the Stop method resets the timer)

---
### Known Issues

 - We can't put any componente inside the **FrameTimer**, because it'll became a simple Frame. 
 For example, if we do something like this:
 ```xml
         <frt:FrameTimer IsAutoStarted="True" BorderColor="Black">
            <StackLayout>
                <Label Text="Test"></Label>
            </StackLayout>
        </frt:FrameTimer>
```
We'll get a simple **Frame** with a **Label "Test"** inside a **StackLayout**...
