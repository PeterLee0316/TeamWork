﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LWDicer.Control
{
    public class CPos_XY
    {
        public double dX;
        public double dY;

        public CPos_XY()
        {
            this.dX = 0.0;
            this.dY = 0.0;
        }

        public CPos_XY(double dX, double dY)
        {
            this.dX = dX;
            this.dY = dY;
        }

        public override string ToString()
        {
            return $"X:{dX}, Y:{dY}";
        }

        public void Init()
        {
            dX = dY = 0.0;
        }

        public void Init<T>(T x, T y)
        {
            try
            {
                dX = Convert.ToDouble(x);
                dY = Convert.ToDouble(y);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void TransToArray(out double[] array)
        {
            array = new double[] { dX, dY };
        }

        public void TransFromArray<T>(T[] array)
        {
            if (array.Length != 2) return;
            try
            {
                dX = Convert.ToDouble(array[0]);
                dY = Convert.ToDouble(array[1]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CPos_XY)) return false;

            CPos_XY s2 = (CPos_XY)obj;
            return Math.Equals(dX, s2.dX) && Math.Equals(dY, s2.dY);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CPos_XY s1, CPos_XY s2)
        {
            return Math.Equals(s1.dX, s2.dX) && Math.Equals(s1.dY, s2.dY);
        }

        public static bool operator !=(CPos_XY s1, CPos_XY s2)
        {
            return !(s1 == s2);
        }

        public static CPos_XY operator +(CPos_XY s1, CPos_XY s2)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX + s2.dX;
            s.dY = s1.dY + s2.dY;

            return s;
        }

        public static CPos_XY operator +(CPos_XY s1, double[] dAdd)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX + dAdd[0];
            s.dY = s1.dY + dAdd[1];

            return s;
        }

        public static CPos_XY operator -(CPos_XY s1, CPos_XY s2)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX - s2.dX;
            s.dY = s1.dY - s2.dY;

            return s;
        }

        public static CPos_XY operator -(CPos_XY s1, double[] dSub)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX - dSub[0];
            s.dY = s1.dY - dSub[1];

            return s;
        }

        public static CPos_XY operator *(CPos_XY s1, double[] dMul)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX * dMul[0];
            s.dY = s1.dY * dMul[1];

            return s;
        }

        public static CPos_XY operator /(CPos_XY s1, double[] dDiv)
        {
            CPos_XY s = new CPos_XY();

            if (dDiv[0] != 0) s.dX = s1.dX / dDiv[0];
            if (dDiv[1] != 0) s.dY = s1.dY / dDiv[1];
                              
            return s;
        }

        public double GetAt(int axis)
        {
            switch (axis)
            {
                case 0: // DEF_X
                    return dX;
                    break;
                case 1: // DEF_Y
                    return dY;
                    break;
                //case 2: // DEF_T
                //    return dT;
                //    break;
                //case 3: // DEF_Z
                //    return dZ;
                //    break;
            }
            return 0;
        }

    }

    public class CPos_XYT
    {
        public double dX;
        public double dY;
        public double dT;

        public CPos_XYT()
        {
            this.dX = 0.0;
            this.dY = 0.0;
            this.dT = 0.0;
        }

        public CPos_XYT(double dX, double dY, double dT)
        {
            this.dX = dX;
            this.dY = dY;
            this.dT = dT;
        }

        public override string ToString()
        {
            return $"X:{dX}, Y:{dY}, T:{dT}";
        }

        public void Init()
        {
            dX = dY = dT = 0.0;
        }

        public void Init<T>(T x, T y, T t)
        {
            try
            {
                dX = Convert.ToDouble(x);
                dY = Convert.ToDouble(y);
                dT = Convert.ToDouble(t);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void TransToArray(out double[] array)
        {
            array = new double[] { dX, dY, dT };
        }

        public void TransFromArray<T>(T[] array)
        {
            if (array.Length != 3) return;
            try
            {
                dX = Convert.ToDouble(array[0]);
                dY = Convert.ToDouble(array[1]);
                dT = Convert.ToDouble(array[2]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CPos_XYT)) return false;

            CPos_XYT s2 = (CPos_XYT)obj;

            return Math.Equals(dX, s2.dX) && Math.Equals(dY, s2.dY)
                && Math.Equals(dT, s2.dT);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CPos_XYT s1, CPos_XYT s2)
        {
            return Math.Equals(s1.dX, s2.dX) && Math.Equals(s1.dY, s2.dY)
                 && Math.Equals(s1.dT, s2.dT);
        }

        public static bool operator !=(CPos_XYT s1, CPos_XYT s2)
        {
            return !(s1 == s2);
        }

        public static CPos_XYT operator +(CPos_XYT s1, CPos_XYT s2)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX + s2.dX;
            s.dY = s1.dY + s2.dY;
            s.dT = s1.dT + s2.dT;

            return s;
        }

        public static CPos_XYT operator +(CPos_XYT s1, double[] dAdd)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX + dAdd[0];
            s.dY = s1.dY + dAdd[1];
            s.dT = s1.dT + dAdd[2];

            return s;
        }

        public static CPos_XYT operator -(CPos_XYT s1, CPos_XYT s2)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX - s2.dX;
            s.dY = s1.dY - s2.dY;
            s.dT = s1.dT - s2.dT;

            return s;
        }

        public static CPos_XYT operator -(CPos_XYT s1, double[] dSub)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX - dSub[0];
            s.dY = s1.dY - dSub[1];
            s.dT = s1.dT - dSub[2];

            return s;
        }

        public static CPos_XYT operator *(CPos_XYT s1, double[] dMul)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX * dMul[0];
            s.dY = s1.dY * dMul[1];
            s.dT = s1.dT * dMul[2];

            return s;
        }

        public static CPos_XYT operator /(CPos_XYT s1, double[] dDiv)
        {
            CPos_XYT s = new CPos_XYT();

            if (dDiv[0] != 0) s.dX = s1.dX / dDiv[0];
            if (dDiv[1] != 0) s.dY = s1.dY / dDiv[1];
            if (dDiv[2] != 0) s.dT = s1.dT / dDiv[2];
                               
            return s;
        }

        public double GetAt(int axis)
        {
            switch (axis)
            {
                case 0: // DEF_X
                    return dX;
                    break;
                case 1: // DEF_Y
                    return dY;
                    break;
                case 2: // DEF_T
                    return dT;
                    break;
                //case 3: // DEF_Z
                //    return dZ;
                //    break;
            }
            return 0;
        }

    }

    public class CPos_XYTZ
    {
        public double dX;
        public double dY;
        public double dT;
        public double dZ;

        public CPos_XYTZ()
        {
            this.dX = 0.0;
            this.dY = 0.0;
            this.dT = 0.0;
            this.dZ = 0.0;
        }

        public CPos_XYTZ(double dX, double dY, double dT, double dZ)
        {
            this.dX = dX;
            this.dY = dY;
            this.dT = dT;
            this.dZ = dZ;
        }

        public override string ToString()
        {
            return $"X:{dX}, Y:{dY}, T:{dT}, Z:{dZ}";
        }

        public void Init()
        {
            dX = dY = dT = dZ = 0.0;
        }

        public void Init<T>(T x, T y, T t, T z)
        {
            try
            {
                dX = Convert.ToDouble(x);
                dY = Convert.ToDouble(y);
                dT = Convert.ToDouble(t);
                dZ = Convert.ToDouble(z);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void TransToArray(out double[] array)
        {
            array = new double[] { dX, dY, dT, dZ };
        }

        public void TransFromArray<T>(T[] array)
        {
            if (array.Length != 4) return;
            try
            {
                dX = Convert.ToDouble(array[0]);
                dY = Convert.ToDouble(array[1]);
                dT = Convert.ToDouble(array[2]);
                dZ = Convert.ToDouble(array[3]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void SetPosition<T>(int iCoordID, T pos)
        {
            switch(iCoordID)
            {
                case 0:
                    dX = Convert.ToDouble(pos);
                    break;
                case 1:
                    dY = Convert.ToDouble(pos);
                    break;
                case 2:
                    dT = Convert.ToDouble(pos);
                    break;
                case 3:
                    dZ = Convert.ToDouble(pos);
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CPos_XYTZ)) return false;

            CPos_XYTZ s2 = (CPos_XYTZ)obj;
            return Math.Equals(dX, s2.dX) && Math.Equals(dY, s2.dY)
                && Math.Equals(dT, s2.dT) && Math.Equals(dZ, s2.dZ);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            return Math.Equals(s1.dX, s2.dX) && Math.Equals(s1.dY, s2.dY)
                 && Math.Equals(s1.dT, s2.dT) && Math.Equals(s1.dZ, s2.dZ);
        }

        public static bool operator !=(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            return !(s1 == s2);
        }

        public static CPos_XYTZ operator +(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX + s2.dX;
            s.dY = s1.dY + s2.dY;
            s.dT = s1.dT + s2.dT;
            s.dZ = s1.dZ + s2.dZ;

            return s;
        }

        public static CPos_XYTZ operator +(CPos_XYTZ s1, double[] dAdd)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX + dAdd[0];
            s.dY = s1.dY + dAdd[1];
            s.dT = s1.dT + dAdd[2];
            s.dZ = s1.dZ + dAdd[3];

            return s;
        }

        public static CPos_XYTZ operator -(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX - s2.dX;
            s.dY = s1.dY - s2.dY;
            s.dT = s1.dT - s2.dT;
            s.dZ = s1.dZ - s2.dZ;

            return s;
        }

        public static CPos_XYTZ operator -(CPos_XYTZ s1, double[] dSub)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX - dSub[0];
            s.dY = s1.dY - dSub[1];
            s.dT = s1.dT - dSub[2];
            s.dZ = s1.dZ - dSub[3];

            return s;
        }

        public static CPos_XYTZ operator *(CPos_XYTZ s1, double[] dMul)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX * dMul[0];
            s.dY = s1.dY * dMul[1];
            s.dT = s1.dT * dMul[2];
            s.dZ = s1.dZ * dMul[3];

            return s;
        }

        public static CPos_XYTZ operator /(CPos_XYTZ s1, double[] dDiv)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            if (dDiv[0] != 0) s.dX = s1.dX / dDiv[0];
            if (dDiv[1] != 0) s.dY = s1.dY / dDiv[1];
            if (dDiv[2] != 0) s.dT = s1.dT / dDiv[2];
            if (dDiv[3] != 0) s.dZ = s1.dZ / dDiv[3];

            return s;
        }

        public double GetAt(int axis)
        {
            switch(axis)
            {
                case 0: // DEF_X
                    return dX;
                    break;
                case 1: // DEF_Y
                    return dY;
                    break;
                case 2: // DEF_T
                    return dT;
                    break;
                case 3: // DEF_Z
                    return dZ;
                    break;
            }
            return 0;
        }
    }
}
