using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace FergoSparkline {
	[ToolboxItem(true)]
	public partial class Sparkline : UserControl {
		private float _minY = 0, _maxY = 100;
		private bool _autoMax = true, _autoMin = true; 
		private Pen _lineStyle;
		private bool _filled = true;
		private Color _fillColor = Color.FromArgb(197, 255, 110);
		private IList<float> _values = new List<float> { 3, 15, 10, 12, 6, 8, 9, 3 };

		//Exposed properties
		[Description("Values from the series"), Category("Data")]
		public IList<float> Values { get { return _values; } set { this._values = value; this.Refresh(); } }
		[Description("Maximum Y value. Only applies if AutoMax is true"), Category("Data")]
		public float YMax { get { return _maxY; } set { this._maxY = value; this.Refresh(); } }
		[Description("Minimum Y value. Only applies if AutoMin is true"), Category("Data")]
		public float YMin { get { return _minY; } set { this._minY = value; this.Refresh(); } }
		[Description("Maximum Y value. Only applies if AutoMax is true"), Category("Data")]
		public bool AutoMax { get { return _autoMax; } set { this._autoMax = value; this.Refresh(); } }
		[Description("Minimum Y value. Only applies if AutoMin is true"), Category("Data")]
		public bool AutoMin { get { return _autoMin; } set { this._autoMin = value; this.Refresh(); } }
		[Description("Line style in the form of a Pen object. Updates LineColor and LineWidth"), Category("Appearance")]
		public Pen LineStyle { get { return _lineStyle; } set { _lineStyle = value; this.Refresh(); } }
		[Description("Fills the area below the series"), Category("Appearance")]
		public bool Filled { get { return _filled; } set { _filled = value; this.Refresh(); } }
		[Description("Gets or sets the fill color"), Category("Appearance")]
		public Color FillColor { get { return _fillColor; } set { _fillColor = value; this.Refresh(); } }
		[Description("Gets or sets the line color"), Category("Appearance")]
		public Color LineColor { get { return _lineStyle.Color; } set { _lineStyle.Color = value; this.Refresh(); } }
		[Description("Gets or sets the line width"), Category("Appearance")]
		public float LineWidth { get { return _lineStyle.Width; } set { _lineStyle.Width = value; this.Refresh(); } }

		public Sparkline()  {
			InitializeComponent();

			this._lineStyle = new Pen(Color.FromArgb(122, 204, 0), 2);
			this.Refresh();
		}

		//Convert from series amplitude to control height
		private int Remap(float Value) {
			float minY = AutoMin ? Values.Min() : YMin;
			float maxY = AutoMax ? Values.Max() : YMax;

			if (Value > maxY)
				Value = maxY;

			if (Value < minY)
				Value = minY;

			return (int)((Value - maxY) * (this.Height - 0) / (minY - maxY) + 0);
		}

		//It doesn't automatically repaint on resize, so make sure we repaint it
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			this.Refresh();
		}
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			if (Values != null && Values.Count > 1) {
				List<Point> points = new List<Point>();
				float _xIncrement = (float)this.Width / (Values.Count() == 1 ? 1 : Values.Count() - 1);
				float currentX = 0;

				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

				//Create our fill polygon
				points.Add(new Point((int)currentX, this.Height));

				Point lastPoint = new Point((int)currentX, Remap(Values[0]));
				points.Add(lastPoint);

				for (int i = 1; i < Values.Count; i++) { 
					Point currentPoint = new Point((int)(currentX + _xIncrement), Remap(Values[i]));
					points.Add(currentPoint);

					lastPoint = currentPoint;
					currentX += _xIncrement;
				}

				points.Add(new Point((int)currentX, this.Height ));

				//Draw our polygon
				if (Filled) {
					e.Graphics.FillPolygon(new SolidBrush(FillColor), points.ToArray());
				}

				//Draw our series (excluding the first and last points, which are the polygon lower boundaries)
				for (int i = 2; i < points.Count - 1; i++) {
					e.Graphics.DrawLine(_lineStyle, points[i], points[i - 1]);
				}
				
			}
		}
	}
}
