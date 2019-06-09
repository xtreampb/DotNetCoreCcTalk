using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DotNetCoreCcTalk.crc
{
    public sealed class Crc8Provider : ICRCProvider
    {
        public byte CalculateCRC8(byte[] input)
        {
            return (byte)input.Aggregate(0, (prev, next) => prev += next);

            //byte crc = 0;
            //foreach(var bit in input)
            //{
            //    crc += bit;
            //}

            //return crc;
        }

        public byte[] CalculateCRC16(byte[] input)
        {
            throw new NotImplementedException();
        }

        public byte[] CalculateCRC16CCITT(byte[] input)
        {
            throw new NotImplementedException();
        }
    }
}
