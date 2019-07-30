using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;


namespace WpfApp1
{
	public class Tile
	{
		private readonly DrawingVisual drawingVisual = new DrawingVisual();
		private double lineHeight;
		private bool isDirty = true;

		private string text;


		public Tile(string text, int column, int row)
		{
			this.text = text ?? throw new ArgumentNullException(nameof(text));
			Column = column;
			Row = row;
			RenderOptions.SetClearTypeHint(drawingVisual, ClearTypeHint.Enabled);
			RenderOptions.SetEdgeMode(drawingVisual, EdgeMode.Aliased);
		}


		private void createDrawing()
		{
			FormattedText formattedText
					= new FormattedText(
							text,
							CultureInfo.CurrentUICulture,
							FlowDirection.LeftToRight,
							CreateTypeFace(),
							GetFontSize(),
							GetForeGround(),
							null,
							VisualTreeHelper.GetDpi(drawingVisual)
									.PixelsPerDip);
			lineHeight = formattedText.LineHeight;
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawText(formattedText, new Point());
			drawingContext.Close();
		}


		public Typeface CreateTypeFace()
			=> new Typeface(
					GetFontFamily(),
					GetFontStyle(),
					GetFontWeight(),
					GetFontStretch());

		public virtual FontFamily GetFontFamily()
			=> new FontFamily("Segoe UI");

		public virtual FontStyle GetFontStyle()
			=> FontStyles.Normal;

		public virtual FontWeight GetFontWeight()
			=> FontWeights.Normal;

		public virtual FontStretch GetFontStretch()
			=> FontStretch.FromOpenTypeStretch(5);

		public virtual double GetFontSize()
			=> 48;

		public virtual Brush GetForeGround()
			=> Brushes.Black;


		public int Column { get; set; }

		public int Row { get; set; }

		public Visual Visual
			=> drawingVisual;


		public Size GetPreferredSize(Size availableSize)
		{
			if (isDirty) {
				createDrawing();
				isDirty = false;
			}
			return new Size(
					drawingVisual.ContentBounds.Width + drawingVisual.ContentBounds.Left,
					Math.Max(
							lineHeight,
							drawingVisual.ContentBounds.Height + drawingVisual.ContentBounds.Top));
		}

		public void Arrange(Rect finalRect)
		{
			drawingVisual.Clip = new RectangleGeometry(new Rect(new Point(), finalRect.Size));
			drawingVisual.Offset = new Vector(finalRect.X, finalRect.Y);
		}
	}
}
