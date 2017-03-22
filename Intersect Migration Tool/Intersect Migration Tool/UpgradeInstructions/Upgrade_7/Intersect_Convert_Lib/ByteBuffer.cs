using System;
using System.Collections.Generic;
using System.Text;

namespace Intersect_Migration_Tool.UpgradeInstructions.Upgrade_7.Intersect_Convert_Lib
{
    public class ByteBuffer : IDisposable
    {
        readonly List<byte> _buff;
        public int Readpos;
        private bool _wasUpdated;
        private byte[] _readBytes;
        public ByteBuffer()
        {
            _buff = new List<byte>();
            Readpos = 0;
        }
        public byte[] ToArray()
        {
            return _buff.ToArray();
        }
        public int Count()
        {
            return _buff.Count;
        }
        public int Pos()
        {
            return Readpos;
        }
        public int Length()
        {
            return Count() - Readpos;
        }
        public void Clear()
        {
            _buff.Clear();
            Readpos = 0;
        }
        public void WriteBytes(byte[] input)
        {
            _buff.AddRange(input);
            _wasUpdated = true;
        }
        public void WriteByte(byte input)
        {
            _buff.Add(input);
            _wasUpdated = true;
        }
        public void WriteBoolean(bool input)
        {
            WriteByte(Convert.ToByte(input));
        }
        public void WriteShort(short input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteInteger(int input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteLong(long input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteDouble(double input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteString(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                WriteInteger(0);
            }
            else
            {
                var data = Encoding.Unicode.GetBytes(input);
                WriteInteger(data.Length);
                WriteBytes(data);
            }
            _wasUpdated = true;
        }
        public string ReadString()
        {
            return ReadString(true);
        }
        public string ReadString(bool peek)
        {
            var len = ReadInteger(true);
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            if (len == 0) return "";
            var ret = Encoding.Unicode.GetString(_readBytes, Readpos, len);
            if (peek & _buff.Count > Readpos)
            {
                if (ret.Length > 0)
                {
                    Readpos += len;
                }
            }
            return ret;
        }

        public bool ReadBoolean(bool peek = true)
        {
            return Convert.ToBoolean(ReadByte(peek));
        }

        public byte ReadByte(bool peek = true)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            var ret = _buff[Readpos];
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 1;
            }
            return ret;
        }

        public byte[] ReadBytes(int length, bool peek = true)
        {
            var ret = _buff.GetRange(Readpos, length).ToArray();
            if (peek)
                Readpos += length;
            return ret;
        }

        public short ReadShort(bool peek = true)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToInt16(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 2;
            }
            return ret;
        }

        public int ReadInteger(bool peek = true)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToInt32(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 4;
            }
            return ret;
        }

        public long ReadLong(bool peek = true)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToInt64(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 8;
            }
            return ret;
        }

        public double ReadDouble()
        {
            return ReadDouble(true);
        }
        public double ReadDouble(bool peek)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToDouble(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 8;
            }
            return ret;
        }

        // To detect redundant calls
        private bool _disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _buff.Clear();
                }
                Readpos = 0;
            }
            _disposedValue = true;
        }

        #region " IDisposable Support "
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}