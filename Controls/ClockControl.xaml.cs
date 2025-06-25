using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeTrainer.Controls
{
    public partial class ClockControl : UserControl
    {
        private const double Size = 250;
        private const double Center = Size / 2;
        private const double Radius = 110;
        private const double NumberRadius = 90;
        private const double HourHandLength = 60;
        private const double MinuteHandLength = 90;

        private Line hourHand;
        private Line minuteHand;
        private Ellipse centerCircle;
        private Ellipse centerDot;

        private int _hour = 0;
        private int _minute = 0;
        public int Hour
        {
            get => _hour;
            set { _hour = (value + 12) % 12; Redraw(); }
        }
        public int Minute
        {
            get => _minute;
            set { _minute = (value + 60) % 60; Redraw(); }
        }

        public ClockControl()
        {
            InitializeComponent();
            Loaded += (s, e) => Redraw();
            PART_Canvas.MouseLeftButtonDown += OnMouseDown;
            PART_Canvas.MouseMove += OnMouseMove;
            PART_Canvas.MouseLeftButtonUp += OnMouseUp;
        }

        private void Redraw()
        {
            if (PART_Canvas == null) return;
            PART_Canvas.Children.Clear();
            // Draw clock face
            var face = new Ellipse
            {
                Width = Size - 10,
                Height = Size - 10,
                Stroke = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                StrokeThickness = 4,
                Fill = Brushes.White
            };
            Canvas.SetLeft(face, 5);
            Canvas.SetTop(face, 5);
            PART_Canvas.Children.Add(face);
            // Draw minute ticks
            for (int i = 0; i < 60; i++)
            {
                double angle = i * 6 * Math.PI / 180;
                double x1 = Center + (Radius - 8) * Math.Sin(angle);
                double y1 = Center - (Radius - 8) * Math.Cos(angle);
                double x2 = Center + Radius * Math.Sin(angle);
                double y2 = Center - Radius * Math.Cos(angle);
                var tick = new Line
                {
                    X1 = x1, Y1 = y1, X2 = x2, Y2 = y2,
                    Stroke = Brushes.Gray,
                    StrokeThickness = (i % 5 == 0) ? 3 : 1.5
                };
                PART_Canvas.Children.Add(tick);
            }
            // Draw numbers
            for (int i = 1; i <= 12; i++)
            {
                double angle = (i - 3) * 30 * Math.PI / 180;
                double x = Center + NumberRadius * Math.Cos(angle);
                double y = Center + NumberRadius * Math.Sin(angle);
                var tb = new TextBlock
                {
                    Text = i.ToString(),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20,
                    Foreground = Brushes.Black
                };
                tb.Measure(new Size(30, 30));
                Canvas.SetLeft(tb, x - tb.DesiredSize.Width / 2);
                Canvas.SetTop(tb, y - tb.DesiredSize.Height / 2);
                PART_Canvas.Children.Add(tb);
            }
            // Draw hour hand
            double hourAngle = ((Hour % 12) + Minute / 60.0) * 30 * Math.PI / 180;
            double hx = Center + HourHandLength * Math.Sin(hourAngle);
            double hy = Center - HourHandLength * Math.Cos(hourAngle);
            hourHand = new Line
            {
                X1 = Center, Y1 = Center, X2 = hx, Y2 = hy,
                Stroke = new SolidColorBrush(Color.FromRgb(58, 110, 165)),
                StrokeThickness = 10,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            PART_Canvas.Children.Add(hourHand);
            // Draw minute hand
            double minAngle = Minute * 6 * Math.PI / 180;
            double mx = Center + MinuteHandLength * Math.Sin(minAngle);
            double my = Center - MinuteHandLength * Math.Cos(minAngle);
            minuteHand = new Line
            {
                X1 = Center, Y1 = Center, X2 = mx, Y2 = my,
                Stroke = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                StrokeThickness = 6,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            PART_Canvas.Children.Add(minuteHand);
            // Draw center
            centerCircle = new Ellipse
            {
                Width = 22, Height = 22,
                Fill = Brushes.White,
                Stroke = new SolidColorBrush(Color.FromRgb(58, 110, 165)),
                StrokeThickness = 4
            };
            Canvas.SetLeft(centerCircle, Center - 11);
            Canvas.SetTop(centerCircle, Center - 11);
            PART_Canvas.Children.Add(centerCircle);
            centerDot = new Ellipse
            {
                Width = 12, Height = 12,
                Fill = new SolidColorBrush(Color.FromRgb(58, 110, 165)),
                Stroke = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                StrokeThickness = 2
            };
            Canvas.SetLeft(centerDot, Center - 6);
            Canvas.SetTop(centerDot, Center - 6);
            PART_Canvas.Children.Add(centerDot);
        }

        // --- Drag logic ---
        private bool isDragging = false;
        private bool dragMinute = false;
        private int lastMinute = 0;
        private int lastHour = 0;

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(PART_Canvas);
            double dx = pos.X - Center;
            double dy = Center - pos.Y;
            double angle = Math.Atan2(dx, dy) * 180 / Math.PI;
            angle = (angle + 360) % 360;
            double minDiff = Math.Abs(angle - Minute * 6);
            double hourAngle = ((Hour % 12) + Minute / 60.0) * 30;
            double hourDiff = Math.Abs(angle - hourAngle);
            dragMinute = minDiff < hourDiff;
            isDragging = true;
            lastMinute = Minute;
            lastHour = Hour;
            PART_Canvas.CaptureMouse();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            var pos = e.GetPosition(PART_Canvas);
            double dx = pos.X - Center;
            double dy = Center - pos.Y;
            double angle = Math.Atan2(dx, dy) * 180 / Math.PI;
            angle = (angle + 360) % 360;
            if (dragMinute)
            {
                int newMinute = (int)Math.Round(angle / 6) % 60;
                if (newMinute < 0) newMinute += 60;
                if (Math.Abs(newMinute - lastMinute) > 30)
                {
                    if (newMinute < lastMinute)
                        Hour = (Hour + 1) % 12;
                    else
                        Hour = (Hour + 11) % 12;
                }
                Minute = newMinute;
                lastMinute = newMinute;
            }
            else
            {
                double hour = angle / 30.0;
                if (hour < 0) hour += 12;
                int newHour = (int)Math.Round(hour) % 12;
                double minutePart = (hour - newHour) * 60;
                if (minutePart < 0) minutePart += 60;
                Hour = newHour;
                Minute = (int)Math.Round(minutePart) % 60;
                lastHour = newHour;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            PART_Canvas.ReleaseMouseCapture();
        }
    }
} 