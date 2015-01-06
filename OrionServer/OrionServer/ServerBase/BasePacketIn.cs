using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBase
{
    public class BasePacketIn : MemoryStream
    {
        public BasePacketIn(int size)
            : base(size)
        { }

        public BasePacketIn(byte[] buf, int start, int size)
            : base(buf, start, size)
        { }

        /// <summary>
        /// Reads in 2 bytes and converts it from network to host byte order
        /// </summary>
        /// <returns>A 2 byte (short) value</returns>
        public virtual ushort ReadShort()
        {
            var v1 = (byte)ReadByte();
            var v2 = (byte)ReadByte();

            return Marshal.ConvertToUInt16(v1, v2);
        }

        /// <summary>
        /// Reads in 2 bytes
        /// </summary>
        /// <returns>A 2 byte (short) value in network byte order</returns>
        public virtual ushort ReadShortLowEndian()
        {
            var v1 = (byte)ReadByte();
            var v2 = (byte)ReadByte();

            return Marshal.ConvertToUInt16(v2, v1);
        }

        /// <summary>
        /// Reads in 4 bytes and converts it from network to host byte order
        /// </summary>
        /// <returns>A 4 byte value</returns>
        public virtual uint ReadInt()
        {
            var v1 = (byte)ReadByte();
            var v2 = (byte)ReadByte();
            var v3 = (byte)ReadByte();
            var v4 = (byte)ReadByte();

            return Marshal.ConvertToUInt32(v1, v2, v3, v4);
        }

        public virtual float ReadFloat()
        {
            uint val = ReadInt();
            unsafe 
            {
                float* f = (float*)&val;
                return *f;
            }
        }

        /// <summary>
        /// Skips 'num' bytes ahead in the stream
        /// </summary>
        /// <param name="num">Number of bytes to skip ahead</param>
        public void Skip(long num)
        {
            Seek(num, SeekOrigin.Current);
        }

        /// <summary>
        /// Reads a null-terminated string from the stream
        /// </summary>
        /// <param name="maxlen">Maximum number of bytes to read in</param>
        /// <returns>A string of maxlen or less</returns>
        public virtual string ReadString(int maxlen)
        {
            var buf = new byte[maxlen];
            Read(buf, 0, maxlen);

            return Marshal.ConvertToString(buf);
        }

        /// <summary>
        /// Reads in a pascal style string
        /// </summary>
        /// <returns>A string from the stream</returns>
        public virtual string ReadPascalString()
        {
            return ReadString(ReadByte());
        }

        /// <summary>
        /// Reads in a pascal style string, with header count formatted as a Low Endian Short.
        /// </summary>
        /// <returns>A string from the stream</returns>
        public virtual string ReadLowEndianShortPascalString()
        {
            return ReadString(ReadShortLowEndian());
        }

        public virtual uint ReadIntLowEndian()
        {
            var v1 = (byte)ReadByte();
            var v2 = (byte)ReadByte();
            var v3 = (byte)ReadByte();
            var v4 = (byte)ReadByte();

            return Marshal.ConvertToUInt32(v4, v3, v2, v1);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
