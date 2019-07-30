using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;


namespace WpfApp1
{
	public class DrawingTile
	{
		private readonly Visual visualParent;
		private readonly GeometryDrawing geometryDrawing = new GeometryDrawing();
		private double lineHeight;
		private bool isDirty = true;

		private string text;


		public DrawingTile(Visual visualParent, string text, int column, int row)
		{
			this.visualParent = visualParent ?? throw new ArgumentNullException(nameof(visualParent));
			this.text = text ?? throw new ArgumentNullException(nameof(text));
			Column = column;
			Row = row;
			RenderOptions.SetClearTypeHint(geometryDrawing, ClearTypeHint.Enabled);
			RenderOptions.SetEdgeMode(geometryDrawing, EdgeMode.Aliased);
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
							VisualTreeHelper.GetDpi(visualParent)
									.PixelsPerDip);
			lineHeight = formattedText.LineHeight;
			geometryDrawing.Brush = GetForeGround();
			geometryDrawing.Geometry = formattedText.BuildGeometry(new Point());
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

		public Drawing Drawing
			=> geometryDrawing;

		protected internal Rect FinalRect { get; private set; }


		public Size GetPreferredSize(Size availableSize)
		{
			if (isDirty) {
				createDrawing();
				isDirty = false;
			}
			return new Size(
					geometryDrawing.Bounds.Width + geometryDrawing.Bounds.Left,
					Math.Max(
							lineHeight,
							geometryDrawing.Bounds.Height + geometryDrawing.Bounds.Top));
		}

		public void Arrange(Rect finalRect)
			=> FinalRect = finalRect;
	}
}
