using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;


namespace OHQData.Helpers
{


    /// <summary>
    /// The resizable grid is a resizable 2-dimensional array.
    /// </summary>
    public class ResizableGrid<T>
    {
        private Dictionary<int,List<T>> rows;
        
        private int xMin = 0;
        private int yMin = 0;
        private int xMax = 0;
        private int yMax = 0;

        public ResizableGrid()
        {
            rows = new Dictionary<int,List<T>>();
            addRow(0);
        }

        #region Public interface
        public void containCoordinate(int x, int y)
        {
            if (containsCoordinate(x, y))
            {
                return;
            }

            while (x < xMin)
            {
                expandLeft();
            }
            while (xMax < x)
            {
                expandRight();
            }
            while (y < yMin)
            {
                expandUp();
            }
            while (yMax < y)
            {
                expandDown();
            }
            

        }

        public bool containsCoordinate(int x, int y)
        {
            return x >= xMin &&
                   y >= yMin &&
                   x <= xMax &&
                   y <= yMax;
        }

        public void set(int x, int y, T value)
        {
            if (!containsCoordinate(x, y))
            {
                throw new IndexOutOfRangeException();
            }
            List<T> row = getRow(y);
            row[getInternalX(x)] = value;
        }

        public virtual T get(int x, int y)
        {
            if (!containsCoordinate(x, y))
            {
                throw new IndexOutOfRangeException();
            }
            List<T> row = getRow(y);
            return row[getInternalX(x)];
        }
        #endregion

        #region Private utility methods.
        private void expandUp()
        {
            yMin--;
            addRow(yMin);

        }

        private void expandDown()
        {
            yMax++;
            addRow(yMax);
            
        }

        private void expandLeft()
        {
            xMin--;
            foreach (List<T> list in rows.Values)
            {
                list.Insert(0, getNullObject());
            }
        }

        private void expandRight()
        {
            xMax++;

            foreach (List<T> list in rows.Values)
            {
                list.Add(getNullObject());
            }
        }

        private T getNullObject()
        {
            return default(T);
        }
        #endregion

        #region Row management
        private void addRow(int key)
        {
            List<T> newRow = new List<T>(Width);
            for (int i = 0; i < Width; i++)
            {
                newRow.Add(getNullObject());
            }
            rows.Add(key, newRow);
        }

        private List<T> getRow(int y)
        {
            //int internalY = convertY(y);
            List<T> row;
            rows.TryGetValue(y, out row);
            return row;
        }
        #endregion

        #region Coordinate conversion
        private int getInternalX(int x)
        {
            return x - xMin;
        }
        /*
        private int convertY(int y)
        {
            return y + yMin;
        }*/
 
        
        #endregion

        #region Properties
        public int Width
        {
            get { return xMax - xMin + 1; }
        }
        public int Height
        {
            get { return yMax - yMin + 1; }
        }


        public int MinX
        {
            get { return xMin; }
        }
        public int MinY
        {
            get { return yMin; }
        }
        public int MaxX
        {
            get { return xMax; }
        }
        public int MaxY
        {
            get { return yMax; }
        }
        #endregion


       
       
            
    }

    
}
