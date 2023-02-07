// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExHentaiDownloader3.Controls
{
    public enum ZoomMode
    {
        Uniform,
        Custom
    }

    public sealed partial class ZoomControl : UserControl
    {
        private ContentControl _contentControl;
        private float _zoom = 1;
        private DateTime _lastSetCenterTime = DateTime.MinValue;

        private ZoomMode _zoomMode = ZoomMode.Uniform;

        public float Zoom 
        {
            get => _zoom;
            set
            {
                value = (float)Math.Max(0.2, value);
                value = (float)Math.Min(5, value);
                if (_zoom != value)
                {
                    _zoom = value;
                    ApplyZoom();
                }
            }
        }

        public UIElement MyContent
        {
            get { return (UIElement)GetValue(MyContentProperty); }
            set { SetValue(MyContentProperty, value); }
        }

        public ZoomMode ZoomMode 
        {
            get => _zoomMode;
            set
            {
                if (_zoomMode != value)
                {
                    _zoomMode = value;
                }
                if (_zoomMode == ZoomMode.Uniform)
                {
                    SafeSetCenterPoint(0, 0);
                    ContentControl_SizeChanged(_contentControl, null);
                }
            }
        }


        // Using a DependencyProperty as the backing store for MyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyContentProperty =
            DependencyProperty.Register("MyContent", typeof(UIElement), typeof(ZoomControl), new PropertyMetadata(null));



        public ZoomControl()
        {
            this.InitializeComponent();
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            SafeMoveDeltaPosition(e.HorizontalChange, e.VerticalChange);
        }

        private void Thumb_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            ZoomMode = ZoomMode.Custom;
            if (_contentControl == null)
                return;

            var point = e.GetCurrentPoint(_contentControl);
            var position = point.Position;
            SafeSetCenterPoint((float)position.X, (float)position.Y);

            var dealta = point.Properties.MouseWheelDelta;
            Zoom += dealta / 2000f;
        }

        private void ApplyZoom()
        {
            if (_contentControl == null)
                return;

            var scale = _contentControl.Scale;
            scale.X = scale.Y = Zoom;
            _contentControl.Scale = scale;
        }

        private void SafeSetCenterPoint(float x, float y)
        {
            if (DateTime.Now.Subtract(_lastSetCenterTime).TotalMilliseconds > 1000)
            {
                _contentControl.CenterPoint = new System.Numerics.Vector3() { X = x, Y = y, Z = _contentControl.CenterPoint.Z };
                _lastSetCenterTime = DateTime.Now;
            }
        }

        private void SafeMoveDeltaPosition(double deltaLeft, double deltaTop)
        {
            double left = Canvas.GetLeft(thumb);
            double top = Canvas.GetTop(thumb);
            if (!double.IsNaN(left) && !double.IsInfinity(left))
            {
                Canvas.SetLeft(thumb, left + deltaLeft);
            }
            if (!double.IsNaN(top) && !double.IsInfinity(top))
            {
                Canvas.SetTop(thumb, top + deltaTop);
            }
        }

        private void SafeMovePosition(double left, double top)
        {
            if (!double.IsNaN(left) && !double.IsInfinity(left))
            {
                Canvas.SetLeft(thumb, left);
            }
            if (!double.IsNaN(top) && !double.IsInfinity(top))
            {
                Canvas.SetTop(thumb, top);
            }
        }
            
        private void ContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            _contentControl = sender as ContentControl;
        }

        private void ContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ZoomMode == ZoomMode.Custom)
                return;

            _contentControl ??= sender as ContentControl;

            double canH = canvas.ActualHeight;
            double canW = canvas.ActualWidth;
            double ccH = _contentControl.ActualHeight;
            double ccW = _contentControl.ActualWidth;

            if (double.IsInfinity(canH) || double.IsNaN(canH)
                || double.IsInfinity(canW) || double.IsNaN(canW)
                || double.IsInfinity(ccH) || double.IsNaN(ccH)
                || double.IsInfinity(ccW) || double.IsNaN(ccW))
            {
                return;
            }

            double zoomX = canW / ccW;
            double zoomY = canH / ccH;

            Zoom = (float)Math.Min(zoomX, zoomY);

            double imgWidth = ccW * Zoom;
            double imgHeight = ccH * Zoom;

            if (zoomX > zoomY)
            {
                SafeMovePosition((canW - imgWidth) / 2, 0);
            }
            else
            {
                SafeMovePosition(0, (canH - imgHeight) / 2);
            }

            //SafeSetCenterPoint((float)(canW - imgWidth) / 2, (float)(canH - imgHeight) / 2);
        }
    }
}
