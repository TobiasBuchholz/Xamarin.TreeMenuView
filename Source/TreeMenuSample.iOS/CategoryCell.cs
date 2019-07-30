using System;
using CoreGraphics;
using Foundation;
using TreeMenuView.iOS;
using TreeMenuView.Shared.Models;
using TreeMenuSample.Shared;
using UIKit;

namespace TreeMenuSample.iOS
{
    public sealed class CategoryCell : TreeMenuCell<Category>
    {
        public const string CellIdentifier = nameof(CategoryCell);
        public const float Height = 56;
        
        private UILabel _label;
        
        [Export("initWithFrame:")]
        public CategoryCell(CGRect frame) 
            : base(frame)
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

        public override Category Data {
            set => _label.Text = value.Title;
        }

        public override ItemRelation Relation {
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
            _label.TextColor = UIColor.White; 
            BackgroundColor = UIColor.DarkGray; 
        }

        private void ApplyParentLayout()
        {
            _label.TextColor = UIColor.White; 
            BackgroundColor = UIColor.DarkGray; 
        }

        private void ApplySelectedLayout()
        {
            _label.TextColor = UIColor.Black; 
            BackgroundColor = UIColor.Green;
        }

        private void ApplyChildLayout()
        {
            _label.TextColor = UIColor.White; 
            BackgroundColor = UIColor.LightGray; 
        }
    }
}