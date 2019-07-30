using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace WpfApp1
{
	public class DrawingGrid
			: UIElement
	{
		// BUG:
		// Comment-out this DependencyProperty; and uncomment the CLR property below,
		// and the App will run ...

		/// <summary>
		/// Dependency property to define the Tile Padding.
		/// </summary>
		public static readonly DependencyProperty TilePaddingProperty
				= DependencyProperty.Register(
						nameof(DrawingGrid.TilePadding),
						typeof(int),
						typeof(DrawingGrid),
						new FrameworkPropertyMetadata(
								24,
								FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>
		/// Dependency property to define the Tile Padding.
		/// </summary>
		public int TilePadding
		{
			get => (int)GetValue(DrawingGrid.TilePaddingProperty);
			set => SetValue(DrawingGrid.TilePaddingProperty, value);
		}

		///// <summary>
		///// Property to define the Tile Padding.
		///// </summary>
		//public int TilePadding { get; set; } = 6;


		private readonly List<DrawingTile> tiles = new List<DrawingTile>(4);


		public DrawingGrid()
		{
			tiles.Add(new DrawingTile(this, "Tile A0", 0, 0));
			tiles.Add(new DrawingTile(this, "Tile B0", 1, 0));
			tiles.Add(new DrawingTile(this, "Tile A1", 0, 1));
			tiles.Add(new DrawingTile(this, "Tile B1", 1, 1));
		}


		protected override Size MeasureCore(Size availableSize)
		{
			Size availableCellSize = coerceBounds(availableSize);
			Size largestTile = new Size();
			foreach (DrawingTile tile in tiles) {
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
			Rect GetCellArrangeRect(DrawingTile tile)
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
			foreach (DrawingTile tile in tiles) {
				tile.Arrange(GetCellArrangeRect(tile));
			}
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			foreach (DrawingTile tile in tiles) {
				drawingContext.PushClip(new RectangleGeometry(tile.FinalRect));
				drawingContext.PushTransform(new TranslateTransform(tile.FinalRect.X, tile.FinalRect.Y));
				drawingContext.DrawDrawing(tile.Drawing);
				drawingContext.Pop();
				drawingContext.Pop();
			}
		}
	}
}
