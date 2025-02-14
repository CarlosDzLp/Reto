using System.Runtime.CompilerServices;

namespace Reto.Controls;

public partial class ToggledButtonView : ContentView
{
    public ToggledButtonView()
    {
        InitializeComponent();
        ValueText.SetBinding(Label.TextProperty, new Binding(nameof(Value), BindingMode.TwoWay, source: this));
        MinusButton.TextColor = Color.FromHex("#FFFFFF");
        //pancake.BorderColor = Color.FromHex("#FF055D");
        PlusButton.TextColor = Color.FromHex("#FFFFFF");
        //pancake.BorderThickness = (float)0;
    }

    public static readonly BindableProperty ValueProperty =
         BindableProperty.Create(nameof(Value), typeof(double), typeof(ToggledButtonView), 0.0,
             propertyChanged: (bindable, oldValue, newValue) =>

                 ((ToggledButtonView)bindable).Value = (double)newValue,
             defaultBindingMode: BindingMode.TwoWay
             );

    public double Value
    {
        get { return (double)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(nameof(Minimum), typeof(double), typeof(ToggledButtonView), 0.0,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((ToggledButtonView)bindable).Minimum = (double)newValue
        );

    public double Minimum
    {
        get { return (double)GetValue(MinimumProperty); }
        set { SetValue(MinimumProperty, value); }
    }

    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum), typeof(double), typeof(ToggledButtonView), 0.0,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((ToggledButtonView)bindable).Maximum = (double)newValue,
            defaultBindingMode: BindingMode.TwoWay
        );

    public double Maximum
    {
        get { return (double)GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }

    public static readonly BindableProperty StepProperty =
        BindableProperty.Create(nameof(Step), typeof(double), typeof(ToggledButtonView), 1.0,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((ToggledButtonView)bindable).Step = (double)newValue
        );


    public double Step
    {
        get { return (double)GetValue(StepProperty); }
        set { SetValue(StepProperty, value); }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == ValueProperty.PropertyName)
            ValueText.Text = Value.ToString();
    }

    private async void MinusTapped(object sender, EventArgs e)
    {
        await AnimateAsync(MinusButton);

        if ((Value - Step) > Minimum)
            Value -= Step;
        else
            Value = Minimum;
    }

    private async void PlusTapped(object sender, EventArgs e)
    {
        await AnimateAsync(PlusButton);

        if (Value < Maximum)
            Value += Step;
    }

    private async Task AnimateAsync(VisualElement element)
    {
        await element.ScaleTo(0.9, 50, Easing.Linear);
        await Task.Delay(100);
        await element.ScaleTo(1, 50, Easing.Linear);
    }
}