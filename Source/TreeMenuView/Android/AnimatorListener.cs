using System;
using Android.Animation;
using Object = Java.Lang.Object;

namespace TreeMenuView.Android
{
    public sealed class AnimatorListener : Object, Animator.IAnimatorListener
    {
        private readonly Action<Animator> _onCancelAction;
        private readonly Action<Animator> _onEndAction;
        private readonly Action<Animator> _onRepeatAction;
        private readonly Action<Animator> _onStartAction;

        public AnimatorListener(
            Action<Animator> onCancelAction = null,
            Action<Animator> onEndAction = null,
            Action<Animator> onRepeatAction = null,
            Action<Animator> onStartAction = null)
        {
            _onCancelAction = onCancelAction;
            _onEndAction = onEndAction;
            _onRepeatAction = onRepeatAction;
            _onStartAction = onStartAction;
        }

        public void OnAnimationCancel(Animator animation)
        {
            _onCancelAction?.Invoke(animation);
        }

        public void OnAnimationEnd(Animator animation)
        {
            _onEndAction?.Invoke(animation);
        }

        public void OnAnimationRepeat(Animator animation)
        {
            _onRepeatAction?.Invoke(animation);
        }

        public void OnAnimationStart(Animator animation)
        {
            _onStartAction?.Invoke(animation);
        }
    }
}