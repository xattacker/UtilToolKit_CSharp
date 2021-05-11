using System;
using System.IO;
using System.Text;

using Xattacker.Utility.Except;

namespace Xattacker.Utility.Binary
{
    /// <summary>
    ///  the class which handles binary data
    /// </summary>
    public class BinaryBuffer : IBinaryReadable, IBinaryWritable<BinaryBuffer>
    {
        #region data member

        private Stream stream;

        #endregion


        #region constructor and destructor

        public BinaryBuffer()
        {
            this.stream = new MemoryStream();
        }

        public BinaryBuffer(Stream stream)
        {
            this.stream = stream;    
        }

        public BinaryBuffer(byte[] data)
            : this()
        {
            this.WriteBinary(data, 0, data.Length);
            this.SeekToHead();
        }

        public BinaryBuffer(BinaryBuffer buffer)
            : this()
        {
            this.WriteBuffer(buffer);
            this.SeekToHead();
        }

        ~BinaryBuffer()
        {
            try
            {
                if (this.stream != null)
                {
                    this.stream.Close();
                    this.stream.Dispose();
                    this.stream = null;
                }
            }
            catch
            { 
            }
        } 

        #endregion


        #region operator overload

        public static BinaryBuffer operator +(BinaryBuffer x, BinaryBuffer y)
        {
            BinaryBuffer buffer = new BinaryBuffer();

            if (x != null)
            {
                buffer.WriteBuffer(x);
            }

            if (y != null)
            {
                buffer.WriteBuffer(y);
            }

            buffer.SeekToHead();

            return buffer;
        }

        #endregion


        #region public function

        public long Length
        {
            get => this.stream.Length;
        }

        public bool IsEmpty
        {
            get => this.stream.Length == 0;
        }

        public byte[] GetData()
        {
            if (this.stream is MemoryStream)
            {
                return ((MemoryStream)this.stream).ToArray();
            }

            throw new CustomException(ErrorId.UNSUPPORTED, GetType());
        }

        public void Clear()
        {
            if (this.stream != null)
            {
                this.stream.Close();
                this.stream.Dispose();
            }

            this.stream = new MemoryStream();
        }

        public BinaryBuffer WriteBuffer(BinaryBuffer buffer)
        {
            byte[] data = buffer.GetData();

            if (data != null && data.Length > 0)
            {
                this.WriteBinary(data, 0, data.Length);
            }
            
            return this;
        }

        #endregion


        #region index related function

        public void SeekToHead()
        {
            this.stream.Seek(0, SeekOrigin.Begin);
        }

        public void SeekTo(ulong index)
        {
            this.stream.Position = (long)index;
        }

        public void SeekToEnd()
        {
            this.stream.Seek(0, SeekOrigin.End);
        }

        public ulong GetCurrentIndex()
        {
            return (ulong)this.stream.Position;
        }

        #endregion


        #region implement from IBinaryReadable

        public bool Available
        {
            get => this.stream.CanRead;
        }

        public byte[] ReadBinary(int length)
        { 
            byte[] data = new byte[length];
            this.stream.Read(data, 0, length);

            return data;
        }

        public short ReadShort()
        {
            byte[] data = new byte[sizeof(short)];
            this.stream.Read(data, 0, data.Length);

            return BitConverter.ToInt16(data, 0);
        }

        public int ReadInteger()
        {
            byte[] data = new byte[sizeof(int)];
            this.stream.Read(data, 0, data.Length);

            return BitConverter.ToInt32(data, 0);
        }

        public long ReadLong()
        {
            byte[] data = new byte[sizeof(long)];
            this.stream.Read(data, 0, data.Length);

            return BitConverter.ToInt64(data, 0);
        }

        public double ReadDouble()
        {
            byte[] data = new byte[sizeof(double)];
            this.stream.Read(data, 0, data.Length);

            return BitConverter.ToDouble(data, 0);
        }

        public string ReadString()
        {
            int length = this.ReadInteger();

            if (length == 0)
            {
                return string.Empty;
            }

            byte[] data = this.ReadBinary(length);

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        #endregion


        #region implement from IBinaryWritable

        public BinaryBuffer WriteBinary(byte[] data, int offset, int length)
        {
            this.stream.Write(data, offset, length);

            return this;
        }

        public BinaryBuffer WriteShort(short value)
        {
            byte[] data = BitConverter.GetBytes(value);
            this.stream.Write(data, 0, data.Length);

            return this;
        }

        public BinaryBuffer WriteInteger(int value)
        {
            byte[] data = BitConverter.GetBytes(value);
            this.stream.Write(data, 0, data.Length);

            return this;
        }

        public BinaryBuffer WriteLong(long value)
        {
            byte[] data = BitConverter.GetBytes(value);
            this.stream.Write(data, 0, data.Length);

            return this;
        }

        public BinaryBuffer WriteDouble(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            this.stream.Write(data, 0, data.Length);

            return this;
        }

        public BinaryBuffer WriteString(string value)
        {
            if (value.Length == 0)
            {
                this.WriteInteger(0);
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                int length = data.Length;

                this.WriteInteger(length);
                this.WriteBinary(data, 0, length);
            }
            
            return this;
        }

        #endregion
    }
}
