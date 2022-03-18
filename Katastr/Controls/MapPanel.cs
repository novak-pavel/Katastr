using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Katastr.Controls
{
    
    public class MapPanel : Panel
    {
        private Bitmap _map;
        private readonly List<Point> _points;
        public bool IsSelectMode { get; set; } = false;
        private const int POINT_SIZE = 4;
        private const int LINE_SIZE = 7;
        private readonly Brush _brush;
        private Pen _pen;
        private double _scale = 1;
        public MapPanel()
        {
            _map = new Bitmap(Width, Height);
            _points = new List<Point>();
            DoubleBuffered = true;
            _brush = new SolidBrush(Color.FromArgb(20, 255, 0, 0));
            _pen = new Pen(Color.Red, LINE_SIZE);
        }
        
        public void SetMap(Image map)
        {
            _map = (Bitmap)map;
            ClearMap();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //TODO: scale image
            g.DrawImage(_map, 0, 0);
            if (_points.Count > 2)
            {
                var polygon = new List<Point>();
                polygon.AddRange(_points);
                polygon.Add(_points[0]);
                g.DrawLines(Pens.Red, polygon.ToArray());
                g.FillPolygon(_brush, polygon.ToArray());
            }
            for (int i = 0; i < _points.Count; i++)
            {
                var point = _points[i];
                int pointCenter = POINT_SIZE / 2;
                g.FillEllipse(Brushes.Red, point.X - pointCenter, point.Y - pointCenter, POINT_SIZE, POINT_SIZE);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left && IsSelectMode)
            {
                _points.Add(e.Location);
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                IsSelectMode = false;
            }
        }

        public void ClearMap()
        {
            _points.Clear();
            IsSelectMode = true;

            Invalidate();
        }
    }
}
