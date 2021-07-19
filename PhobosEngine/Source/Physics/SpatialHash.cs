using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using PhobosEngine.Math;

namespace PhobosEngine.Physics
{
    public class SpatialHash
    {
        private SpatialHashStorage hashStorage = new SpatialHashStorage();
        private int cellSize;
        private float inverseCellSize;

        public SpatialHash(int cellSize = 100)
        {
            this.cellSize = cellSize;
            inverseCellSize = 1f / cellSize;
        }

        public List<Collider> CollidersAtCell(int x, int y, bool createIfNull = false)
        {
            if(!hashStorage.TryGetValue(x, y, out List<Collider> cell))
            {
                if(createIfNull)
                {
                    cell = new List<Collider>();
                    hashStorage.Add(x, y, cell);
                }
            }

            return cell;
        }

        public void Register(Collider collider)
        {
            RectangleF bounds = collider.Bounds;

            Point topLeft = CellCoordinates(bounds.Left, bounds.Top);
            Point bottomRight = CellCoordinates(bounds.Right, bounds.Bottom);

            for(int cx = topLeft.X; cx <= bottomRight.X; cx++)
            {
                for(int cy = topLeft.Y; cy <= bottomRight.Y; cy++)
                {
                    CollidersAtCell(cx, cy, true).Add(collider);
                }
            }
        }

        public HashSet<Collider> BroadphaseAABB(RectangleF bounds)
        {
            HashSet<Collider> overlappingColliders = new HashSet<Collider>();
            Point topLeft = CellCoordinates(bounds.Left, bounds.Top);
            Point bottomRight = CellCoordinates(bounds.Right, bounds.Bottom);

            for(int cx = topLeft.X; cx <= bottomRight.X; cx++)
            {
                for(int cy = topLeft.Y; cy <= bottomRight.Y; cy++)
                {
                    foreach(Collider col in CollidersAtCell(cx, cy))
                    {
                        if(col.Bounds.Intersects(bounds))
                        {
                            overlappingColliders.Add(col);
                        }
                    }
                }
            }

            return overlappingColliders;
        }

        public void Remove(Collider collider)
        {
            hashStorage.Remove(collider);
        }

        private Point CellCoordinates(float x, float y)
        {
            return new Point((int)MathF.Floor(x * inverseCellSize), (int)MathF.Floor(y * inverseCellSize));
        }
    }

    class SpatialHashStorage
    {
        private Dictionary<long, List<Collider>> store = new Dictionary<long, List<Collider>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetKey(int x, int y)
        {
            return unchecked((long) x << 32 | (uint) y);
        }

        public void Add(int x, int y, List<Collider> colliderList)
        {
            store.Add(GetKey(x, y), colliderList);
        }

        public void Remove(Collider collider)
        {
            foreach (List<Collider> list in store.Values)
            {
                if(list.Contains(collider))
                {
                    list.Remove(collider);
                }
            }
        }

        public bool TryGetValue(int x, int y, out List<Collider> list)
        {
            return store.TryGetValue(GetKey(x, y), out list);
        }

        public void Clear()
        {
            store.Clear();
        }
    }
}