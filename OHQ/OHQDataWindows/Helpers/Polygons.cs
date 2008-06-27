using System;
using System.Collections.Generic;
using System.Text;

namespace OHQData
{
    public interface Polygon
    {
        bool contains(double x, double y);
        double LeftBound();
        double RightBound();
        double TopBound();
        double BottomBound();
    }

    public abstract class RadialPolygon : Polygon
    {
        protected static int NUM_RADIUS = 32;
        protected static double ANGLE_PER_RADIUS = (2.0 * Math.PI) / NUM_RADIUS;

        protected List<Coordinate> points;
        protected List<Coordinate> normals;

        protected Random random;

        protected double centerX;
        protected double centerY;

        private double left;
        public double LeftBound()
        {
            return left;
        }

        private double right;
        public double RightBound()
        {
            return right;
        }

        private double top;
        public double TopBound()
        {
            return top;
        }

        private double bottom;
        public double BottomBound()
        {
            return bottom;
        }

        public RadialPolygon(double centerX, double centerY)
        {
            this.centerX = centerX;
            this.centerY = centerY;

            random = new Random();
        }

        protected void calcBoundaries()
        {
            left = points[0].x;
            right = points[0].x;
            top = points[0].y;
            bottom = points[0].y;
            foreach (Coordinate c in points)
            {
                left = Math.Min(c.x, left);
                right = Math.Max(c.x, right);

                top = Math.Min(c.y, top);
                bottom = Math.Max(c.y, bottom);
            }
        }

        /**
         * Decides if a point is inside the polygon
         * 
         * @param x
         *            X-coordinate of the point.
         * @param y
         *            Y-coordinate of the point.
         * @return True if the point is inside the polygon, otherwise false.
         */
        public bool contains(double x, double y)
        {
            // The vector from the polygon center to the actual polygon.
            double vectorX = x - centerX;
            double vectorY = y - centerY;

            // The angle to the center of the polygon to the point.
            double angle = Math.Atan2(vectorY, vectorX);

            int radius1 = (int)Math.Floor(angle / ANGLE_PER_RADIUS);
            int radius2 = radius1 + 1;
            if (radius1 < 0)
            {
                radius1 += NUM_RADIUS;
            }
            if (radius2 < 0)
            {
                radius2 += NUM_RADIUS;
            }

            double pointX = (double)x - points[radius1].x;
            double pointY = (double)y - points[radius1].y;
            double normalX = normals[radius1].x;
            double normalY = normals[radius1].y;
            double dot = pointX * normalX + pointY * normalY;
            if (dot < 0)
            {
                return true;
            }
            return false;

        }

        protected void calcNormals()
        {
            normals = new List<Coordinate>();
            for (int i = 0; i < NUM_RADIUS - 1; i++)
            {
                normals.Add(normalTo(i, i + 1));
            }
            normals.Add(normalTo(NUM_RADIUS - 1, 0));
        }

        private Coordinate normalTo(int radius1, int radius2)
        {
            double x1 = points[radius1].x;
            double y1 = points[radius1].y;
            double x2 = points[radius2].x;
            double y2 = points[radius2].y;

            double xNormal = y2 - y1;
            double yNormal = -(x2 - x1);

            double normLength = 1.0 / Math.Sqrt(xNormal * xNormal + yNormal
                    * yNormal);
            xNormal *= normLength;
            yNormal *= normLength;

            return new Coordinate(xNormal, yNormal);
        }

        protected class Coordinate
        {
            public double x;
            public double y;

            public Coordinate(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }

    public class RadialCirclePolygon : RadialPolygon
    {

        public RadialCirclePolygon(double centerX, double centerY,
                                   double lowerRadius, double higherRadius)
            : base(centerX, centerY)
        {

            double[] radius = new double[NUM_RADIUS];

            for (int i = 0; i < NUM_RADIUS; i++)
            {
                radius[i] = lowerRadius + (higherRadius - lowerRadius)
                        * random.NextDouble();
            }

            points = new List<Coordinate>();
            for (int i = 0; i < NUM_RADIUS; i++)
            {
                double angle = i * ANGLE_PER_RADIUS;

                double pointX = centerX + radius[i] * Math.Cos(angle);
                double pointY = centerY + radius[i] * Math.Sin(angle);
                points.Add(new Coordinate(pointX, pointY));
            }

            calcNormals();
            calcBoundaries();

        }


    }
}
