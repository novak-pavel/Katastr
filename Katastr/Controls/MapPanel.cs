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
        public MapPanel()
        {
            _map = new Bitmap(Width, Height);
            _points = new List<Point>();
            DoubleBuffered = true;
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
            //TODO: scale image
            g.DrawImage(_map, 0, 0);

            for (int i = 0; i < _points.Count; i++)
            {
                var point = _points[i];
                g.FillEllipse(Brushes.Red, point.X, point.Y, POINT_SIZE, POINT_SIZE);
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
