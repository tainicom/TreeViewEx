using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace tainicom.TreeViewEx.DragNDrop
{
    class InsertContent
    {
        public bool Before { get; set; }

        public Point Position { get; set; }

        public Brush InsertionMarkerBrush { get; set; }

        public TreeViewExItem Item { get; set; }
    }
}
