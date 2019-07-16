using System;
using CoreGraphics;
using Foundation;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public sealed class TreeMenuCell : UICollectionViewCell
    {
        public const string CellIdentifier = nameof(TreeMenuCell);

        private UILabel _label;
        
        [Export("initWithFrame:")]
        public TreeMenuCell(CGRect frame) 
            : base (frame)
        {
            InitLabel();
            InitBottomDivider();
        }

        private void InitLabel()
        {
            _label = new UILabel();
            _label.TranslatesAutoresizingMaskIntoConstraints = false;
			_label.Font = UIFont.SystemFontOfSize(UIFont.PreferredSubheadline.PointSize, UIFontWeight.Light);
            ContentView.AddSubview(_label);

            _label.CenterXAnchor.ConstraintEqualTo(ContentView.CenterXAnchor).Active = true;
            _label.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;
        }

        private void InitBottomDivider()
        {
            var divider = new UIView();
            divider.TranslatesAutoresizingMaskIntoConstraints = false;
            divider.BackgroundColor = UIColor.White;
            ContentView.AddSubview(divider);

            divider.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
            divider.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;
            divider.HeightAnchor.ConstraintEqualTo(1).Active = true;
        }

        public string Title {
            set => _label.Text = value;
        }

        public ItemRelation Relation {
            set {
                switch(value) {
                    case ItemRelation.Root:
                        ApplyRootLayout();
                        break;
                    case ItemRelation.Parent:
                        ApplyParentLayout();
                        break;
                    case ItemRelation.Selected:
                        ApplySelectedLayout();
                        break;
                    case ItemRelation.Child:
                        ApplyChildLayout();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void ApplyRootLayout()
        {
            Animate(0.2f, () => {
                _label.TextColor = UIColor.White; 
                BackgroundColor = UIColor.DarkGray; 
            });
        }

        private void ApplyParentLayout()
        {
            Animate(0.2f, () => {
                _label.TextColor = UIColor.White; 
                BackgroundColor = UIColor.DarkGray; 
            });
        }

        private void ApplySelectedLayout()
        {
            Animate(0.2f, () => {
                _label.TextColor = UIColor.Black; 
                BackgroundColor = UIColor.Yellow;
            });
        }

        private void ApplyChildLayout()
        {
            Animate(0.2f, () => {
                _label.TextColor = UIColor.White; 
                BackgroundColor = UIColor.LightGray; 
            });
        }
    }
}