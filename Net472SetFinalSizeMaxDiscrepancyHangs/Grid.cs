using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Size = System.Windows.Size;


namespace WpfApp1
{
	public class Grid
			: UIElement
	{
		private readonly VisualCollection visualChildren;
		private readonly List<Tile> tiles = new List<Tile>(4);


		public Grid()
		{
			visualChildren = new VisualCollection(this);
			Tile tile = new Tile("Tile A0", 0, 0);
			tiles.Add(tile);
			visualChildren.Add(tile.Visual);
			tile = new Tile("Tile B0", 1, 0);
			tiles.Add(tile);
			visualChildren.Add(tile.Visual);
			tile = new Tile("Tile A1", 0, 1);
			tiles.Add(tile);
			visualChildren.Add(tile.Visual);
			tile = new Tile("Tile B1", 1, 1);
			tiles.Add(tile);
			visualChildren.Add(tile.Visual);
		}


		public int TilePadding { get; } = 6;


		protected override int VisualChildrenCount
			=> visualChildren.Count;

		protected override Visual GetVisualChild(int index)
			=> visualChildren[index];

		protected override Size MeasureCore(Size availableSize)
		{
			Size availableCellSize = coerceBounds(availableSize);
			Size largestTile = new Size();
			foreach (Tile tile in tiles) {
				Size tileSize = tile.GetPreferredSize(availableCellSize);
				largestTile.Width = Math.Max(largestTile.Width, tileSize.Width);
				largestTile.Height = Math.Max(largestTile.Height, tileSize.Height);
			}
			largestTile.Width += largestTile.Width + (TilePadding * 3D);
			largestTile.Height += largestTile.Height + (TilePadding * 3D);
			return largestTile;
		}

		private Size coerceBounds(Size bounds)
			=> new Size(
					Math.Max(
							0D,
							double.IsPositiveInfinity(bounds.Width)
									? int.MaxValue
									: bounds.Width),
					Math.Max(
							0D,
							double.IsPositiveInfinity(bounds.Height)
									? int.MaxValue
									: bounds.Height));

		private Size getCellSize(Size bounds)
			=> new Size(
					Math.Max(
							0D,
							((double.IsPositiveInfinity(bounds.Width)
									? int.MaxValue
									: bounds.Width)
							- (TilePadding * 3D))
							/ 2D),
					Math.Max(
							0D,
							((double.IsPositiveInfinity(bounds.Height)
									? int.MaxValue
									: bounds.Height)
							- (TilePadding * 3D))
							/ 2D));

		protected override void ArrangeCore(Rect finalRect)
		{
			base.ArrangeCore(finalRect);
			Size finalCellSize = getCellSize(finalRect.Size);
			Rect GetCellArrangeRect(Tile tile)
			{
				if (tile.Column == 1) {
					if (tile.Row == 1) {
						return new Rect(
								finalCellSize.Width + (TilePadding * 2D),
								finalCellSize.Height + (TilePadding * 2D),
								finalCellSize.Width,
								finalCellSize.Height);
					}
					return new Rect(
							finalCellSize.Width + (TilePadding * 2D),
							TilePadding,
							finalCellSize.Width,
							finalCellSize.Height);
				}
				if (tile.Row == 1) {
					return new Rect(
							TilePadding,
							finalCellSize.Height + (TilePadding * 2D),
							finalCellSize.Width,
							finalCellSize.Height);
				}
				return new Rect(
						TilePadding,
						TilePadding,
						finalCellSize.Width,
						finalCellSize.Height);
			}
			foreach (Tile tile in tiles) {
				tile.Arrange(GetCellArrangeRect(tile));
			}
		}
	}
}
