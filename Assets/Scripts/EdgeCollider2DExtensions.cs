using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptsUteis
{
    public static class EdgeCollider2DExtensions
    {
        public static float MaiorX(this EdgeCollider2D edgeCollider2D)
        {
            return edgeCollider2D.bounds.center.x + edgeCollider2D.bounds.extents.x;
        }

        public static float MenorX(this EdgeCollider2D edgeCollider2D)
        {
            return edgeCollider2D.bounds.center.x - edgeCollider2D.bounds.extents.x;
        }

        public static float MaiorY(this EdgeCollider2D edgeCollider2D)
        {
            return edgeCollider2D.bounds.center.y + edgeCollider2D.bounds.extents.y;
        }

        public static float MenorY(this EdgeCollider2D edgeCollider2D)
        {
            return edgeCollider2D.bounds.center.y - edgeCollider2D.bounds.extents.y;
        }
    }
}
