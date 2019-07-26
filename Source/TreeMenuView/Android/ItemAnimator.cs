using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.Support.V7.Widget;
using TreeMenuView.Shared.Models;
using Object = Java.Lang.Object;

namespace TreeMenuView.Android
{
    public sealed class ItemAnimator : DefaultItemAnimator
    {
        private readonly IList<AnimatorSet> _pendingAnimators;
        private List<int> _addedIndexes;
        private List<int> _removedIndexes;

        public ItemAnimator()
        {
            _pendingAnimators = new List<AnimatorSet>();
        }
        
        public override ItemHolderInfo RecordPreLayoutInformation(RecyclerView.State state, RecyclerView.ViewHolder viewHolder, int changeFlags, IList<Object> payloads)
        {
            var holderInfo = base.RecordPreLayoutInformation(state, viewHolder, changeFlags, payloads);
            return new IndexedItemHolderInfo(holderInfo, viewHolder.LayoutPosition);
        }

        public override ItemHolderInfo RecordPostLayoutInformation(RecyclerView.State state, RecyclerView.ViewHolder viewHolder)
        {
            var holderInfo = base.RecordPostLayoutInformation(state, viewHolder);
            return new IndexedItemHolderInfo(holderInfo, viewHolder.LayoutPosition);
        }

        public override bool AnimateDisappearance(RecyclerView.ViewHolder viewHolder, ItemHolderInfo preLayoutInfo, ItemHolderInfo postLayoutInfo)
        {
            EnsureDiffResultIsSet();
            
            var view = viewHolder.ItemView;
            var translationY = -view.Height * _removedIndexes.FindIndex(0, x => x == ((IndexedItemHolderInfo) preLayoutInfo).Index);
            var set = new AnimatorSet();
            var scaleAnimator = ObjectAnimator.OfFloat(view, "scaleY", 0f);
            var translateAnimator = ObjectAnimator.OfFloat(view, "translationY", translationY);
            view.PivotY = 0;
            set.PlayTogether(scaleAnimator, translateAnimator);
            _pendingAnimators.Add(set);
            return true; 
        }

        private void EnsureDiffResultIsSet()
        {
            if(_removedIndexes == null || _addedIndexes == null) {
                throw new ArgumentException($"{nameof(ItemAnimator)}s {nameof(DiffResult)} property needs to be set before an adapter change is notified");
            }
        }

        public override bool AnimateAppearance(RecyclerView.ViewHolder viewHolder, ItemHolderInfo preLayoutInfo, ItemHolderInfo postLayoutInfo)
        {
            EnsureDiffResultIsSet();
            
            var view = viewHolder.ItemView;
            var set = new AnimatorSet();
            var scaleAnimator = ObjectAnimator.OfFloat(view, "scaleY", 1f);
            var translateAnimator = ObjectAnimator.OfFloat(view, "translationY", 0);
            view.TranslationY = -view.Height * _addedIndexes.FindIndex(0, x => x == ((IndexedItemHolderInfo) postLayoutInfo).Index);
            view.ScaleY = 0f;
            view.PivotY = 0;
            set.PlayTogether(scaleAnimator, translateAnimator);
            set.AddListener(new AnimatorListener(onEndAction:_ => DispatchAnimationFinished(viewHolder)));
            _pendingAnimators.Add(set);
            return true;
        }

        public override void RunPendingAnimations()
        {
            base.RunPendingAnimations();
            foreach(var animator in _pendingAnimators) {
                animator.SetDuration(250);
                animator.Start();
            }
            _pendingAnimators.Clear();
        }

        public TreeMenuDiffResult DiffResult {
            set {
                _removedIndexes = value.RemovedIndexes.ToList();
                _addedIndexes = value.AddedIndexes.ToList();
            }
        }
    }
}