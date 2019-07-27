using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace KhanBlaze
{
    internal class GridAnimation : AnimationTimeline
    {
        public readonly static DependencyProperty FromProperty;
        public readonly static DependencyProperty ToProperty;

        static GridAnimation()
        {
            FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridAnimation));
            ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridAnimation));
        }

        public override Type TargetPropertyType => typeof(GridLength);

        protected override Freezable CreateInstanceCore()
        {
            return new GridAnimation();
        }

        public GridLength From
        {
            get => (GridLength)GetValue(GridAnimation.FromProperty);
            set => SetValue(GridAnimation.FromProperty, value);
        }

        public GridLength To
        {
            get => (GridLength)GetValue(GridAnimation.ToProperty);
            set => SetValue(GridAnimation.ToProperty, value);
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromValue = ((GridLength)GetValue(GridAnimation.FromProperty)).Value;
            double toValue = ((GridLength)GetValue(GridAnimation.ToProperty)).Value;
            if (fromValue > toValue)
            {
                return new GridLength((animationClock.CurrentProgress.Value) * (toValue - fromValue) + fromValue, this.To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
            else
            {
                return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromValue - toValue) + toValue, this.To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }
    }
}