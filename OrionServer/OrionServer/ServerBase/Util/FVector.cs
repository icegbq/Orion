using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBase.Util
{
    public class FVector
    {

        public enum eIndex { X = 0, Y, Z, MAX };
        private enum eArithmatic { ADD, SUB, MULT, DIV };

        private bool _dirty = false;
        private float[] _data = new float[(int)eIndex.MAX];

        public FVector(float x, float y, float z)
        {
            _data[0] = x;
            _data[1] = y;
            _data[2] = z;
        }

        public FVector()
        {
            SetZeroVector();
        }

        public float X
        {
            get { return _data[0]; }
            set 
            {
                if (_data[0] == value)
                    return;
                _dirty = true;
                _data[0] = value; 
            }
        }

        public float Y
        {
            get { return _data[1]; }
            set
            {
                if (_data[1] == value)
                    return;
                _dirty = true;
                _data[1] = value;
            }
        }

        public float Z
        {
            get { return _data[2]; }
            set
            {
                if (_data[2] == value)
                    return;
                _dirty = true;
                _data[2] = value;
            }
        }

        public bool GetIsDirty()
        {
            if (_dirty)
            {
                _dirty = false;
                return true;
            }
            return false;
        }

        public void SetZeroVector()
        {
            for (int i = 0; i < (int)eIndex.MAX; i++)
            {
                _data[i] = 0;
            }
        }

        public float Length()
        {
            double lengthSqrd = (double)(this * this);
            return (float)Math.Sqrt(lengthSqrd);
        }

        public FVector Normalize()
        {
            return (new FVector(X, Y, Z) / Length());
        }

        public static FVector operator +(FVector c1, FVector c2)
        { 
            return new FVector(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static FVector operator *(float x, FVector c2)
        {
            return new FVector(x * c2.X, x * c2.Y, x * c2.Z); 
        }

        public static float operator *(FVector c1, FVector c2)
        {
            return (c1.X * c2.X) + (c1.Y * c2.Y) + (c1.Z * c2.Z);
        }

        public static FVector operator %(FVector c1, FVector c2)
        {
            return new FVector(c1.X * c2.X, c1.Y * c2.Y, c1.Z * c2.Z); 
        }
         
        public static FVector operator /(float x, FVector c2)
        {
            return new FVector(x / c2.X, x / c2.Y, x / c2.Z);
        }

        public static FVector operator /(FVector c1, float x)
        {
            return (1 / x) * c1;
        }
    }
}
