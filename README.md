# Xamarin.TreeMenuView

[![NuGet](https://img.shields.io/nuget/v/treemenuview.svg?maxAge=86400&style=flat)](https://www.nuget.org/packages/TreeMenuView/)

The `TreeMenuView` constrol is an animated tree like structured list which enables the user to navigate through different levels of menu items. For now it can only be used by native Xamarin.iOS and Xamarin.Android clients, but Xamarin Forms support is planned as well. Under the hood it is implemented by utilising the power of the `UICollectionView` on iOS and the `RecyclerView` on android.

The tree structure of the data is realised by letting your model class implement the `Id` and `ParentId` property of the `ITreeNodeData<TKey>` interface. Now you can pass your data as a flat list to the `TreeMenuView.Items` property and it will handle the rest.

The layout of the menu item cells is completely custom. You just need to create a subclass of `TreeMenuCell<T, TKey>`, which itself is a subclass of `UICollectionViewCell` on iOS and `FrameLayout` on android.

<br/>

|<img src="https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Assets/ios.gif" width="320">|<img src="https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Assets/android.gif" width="320">|
|----------|-------------|

<a name="installation"> Installation </a>
--------------
#### Nuget

> Install-Package TreeMenuView

#### Manually

If you prefer not to use either of the aforementioned dependency managers, you can integrate TreeMenuView into your project manually.

<a name="usage"> Usage </a>
--------------

1. Install package into your application project
2. Create a subclass of `ITreeNodeData<TKey>` as your data model (see [Category.cs](https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Source/TreeMenuSample.Shared/Category.cs))
3. Create some data

```c#
var items = new List<Category> {
    new Category(TreeNode.ParentIdNone, 0, "All categories"),
    new Category(parentId:0, id:1, title:"Development"),
    new Category(parentId:0, id:2, title:"Recipes"),
    new Category(parentId:0, id:3, title:"Sport"),
    new Category(parentId:0, id:4, title:"Music"),
    new Category(parentId:0, id:5, title:"Cars"),
    new Category(parentId:1, id:6, title:"Android Development"),
    new Category(parentId:1, id:7, title:"iOS Development"),
    new Category(parentId:3, id:8, title:"Football"),
    new Category(parentId:3, id:9, title:"Formula 1")
};
```

4. Create a subclass of `TreeMenuCell<T, TKey>` and implement the setters of its `Data` and `Relation` properties (see [iOS/CategoryCell.cs](https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Source/TreeMenuSample.iOS/CategoryCell.cs) and [Android/CategoryCell.cs](https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Source/TreeMenuSample.Android/CategoryCell.cs))
- the `Data` property is your previously created model class
- the `Relation` property is the relation of the current cell - values are: `Root, Parent, Selected, Child`
5. Create a TreeMenuView control and apply the items (see [ViewController.cs](https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Source/TreeMenuSample.iOS/ViewController.cs) and [MainActiviy.cs](https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/Source/TreeMenuSample.Android/MainActivity.cs)):

```c#
// iOS
var treeMenu = new TreeMenuView<Category, long>(CategoryCell.CellIdentifier, CategoryCell.Height);
treeMenu.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
View.AddSubview(treeMenu.View);

treeMenu.Items = items;


// android
var cellHeight = Resources.GetDimensionPixelSize(Resource.Dimension.height_category_cell);
var treeMenu = new TreeMenuView<Category, long>(this, (_,__) => new CategoryCell(this), cellHeight);
layout.AddView(treeMenu.View, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

treeMenu.Items = items;

```
- as you can see you need to provide a specific height for the menu items
- on iOS you need to register the previously created `CategoryCell` by calling the `RegisterClassForCell(...)` method
- on android you need to provide a function which creates the `CategoryCell`

<a name="license"> License </a>
--------------

```TreeMenuView``` is released under the MIT license. See the [LICENSE](https://github.com/TobiasBuchholz/Xamarin.TreeMenuView/blob/master/LICENSE) file for details.
