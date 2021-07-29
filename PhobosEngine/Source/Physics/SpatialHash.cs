using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using PhobosEngine.Math;

namespace PhobosEngine.Collisions
{
    public class SpatialHash
    {
        private SpatialHashStorage hashStorage = new SpatialHashStorage();
        private int cellSize;
        private float inverseCellSize;
        private LinecastManager linecastManager = new LinecastManager();

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

        public HashSet<Collider> BroadphaseAABB(ref RectangleF bounds)
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

        public int Linecast(Vector2 start, Vector2 end, RaycastHit[] outputHits)
        {
            Point currentCell = CellCoordinates(start.X, start.Y);
            Point endCell = CellCoordinates(end.X, end.Y);
            
            Vector2 currentLocation = start;

            Vector2 direction = (end - start);

            linecastManager.Init(ref start, ref end, outputHits);

            // What direction in the cell space are we walking in?
            int cellStepX = MathF.Sign(direction.X);
            int cellStepY = MathF.Sign(direction.Y);

            // Avoid accidently crossing a border we don't need
            if(currentCell.X == endCell.X) cellStepX = 0;
            if(currentCell.Y == endCell.Y) cellStepY = 0;

            float xStep = cellStepX < 0 ? 0f : cellStepX;
            float yStep = cellStepY < 0 ? 0f : cellStepY;
            float nextBoundaryX = (currentCell.X + xStep) * cellSize;
            float nextBoundaryY = (currentCell.Y + yStep) * cellSize;

            // Find time when ray crosses first X/vertical boundary and Y/horizontal boundary
            float tMaxX = direction.X != 0 ? (nextBoundaryX - start.X) / direction.X : float.MaxValue;
            float tMaxY = direction.Y != 0 ? (nextBoundaryY - start.Y) / direction.Y : float.MaxValue;

            // Distance to go before crossing any cell boundary
            float tDeltaX = direction.X != 0 ? cellSize / (direction.X * cellStepX) : float.MaxValue;
            float tDeltaY = direction.Y != 0 ? cellSize / (direction.Y * cellStepY) : float.MaxValue;

            List<Collider> cellColliders = CollidersAtCell(currentCell.X, currentCell.Y);

            if(cellColliders != null && linecastManager.CheckCell(cellColliders))
            {
                linecastManager.Clear();
                return linecastManager.numHits;
            }

            // Iterate through cells, calling the function of the hit parser as we go
            while(currentCell.X != endCell.X || currentCell.Y != endCell.Y)
            {
                // Move to the next cell
                if(tMaxX < tMaxY)
                {
                    currentCell.X = (int) PBMath.Approach(currentCell.X, endCell.X, MathF.Abs(cellStepX));
                    tMaxX += tDeltaX;
                } else {
                    currentCell.Y = (int) PBMath.Approach(currentCell.Y, endCell.Y, MathF.Abs(cellStepY));
                }

                // Check colliders at this cell
                cellColliders = CollidersAtCell(currentCell.X, currentCell.Y);
                if(cellColliders != null && linecastManager.CheckCell(cellColliders))
                {
                    linecastManager.Clear();
                    return linecastManager.numHits;
                }
            }
            
            linecastManager.Clear();
            return linecastManager.numHits;
        }

        public void Remove(Collider collider)
        {
            hashStorage.Remove(collider);
        }

        public void Clear()
        {
            hashStorage.Clear();
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

    class LinecastManager
    {
        private static Comparison<RaycastHit> hitDistanceComparison = (x, y) => x.distance.CompareTo(y.distance);

        private RaycastHit[] outHits;
        private HashSet<Collider> checkedColliders = new HashSet<Collider>();
        private List<RaycastHit> cellHits = new List<RaycastHit>();
        public int numHits = 0;
        private RaycastHit tempHit;
 
        private Vector2 start;
        private Vector2 end;


        public void Init(ref Vector2 start, ref Vector2 end, RaycastHit[] hits)
        {
            this.start = start;
            this.end = end;
            this.outHits = hits;
            this.numHits = 0;
        }

        public void Clear()
        {
            outHits = null;
            checkedColliders.Clear();
            cellHits.Clear();
        }

        public bool CheckCell(List<Collider> cell)
        {
            for(int i = 0; i < cell.Count; i++)
            {
                Collider collider = cell[i];

                if(checkedColliders.Contains(collider))
                {
                    continue;
                }

                checkedColliders.Add(collider);

                // Check if we hit the bounds:
                if(collider.Bounds.LineIntersects(start, end))
                {
                    if(collider.LineIntersects(start, end, out tempHit))
                    {
                        tempHit.collider = collider;
                        cellHits.Add(tempHit);
                    }
                }
            }

            if(cellHits.Count == 0)
            {
                return false;
            }

            cellHits.Sort(hitDistanceComparison);
            for(int i = 0; i < cellHits.Count; i++)
            {
                outHits[numHits] = cellHits[i];
                numHits++;
                if(numHits == outHits.Length)
                {
                    return true;
                }
            }

            return false;
        }

    }
}