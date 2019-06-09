using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCoreCcTalk.crc
{
    public interface ICRCProvider
    {
        byte CalculateCRC8(byte[] input);

        byte[] CalculateCRC16(byte[] input);

        byte[] CalculateCRC16CCITT(byte[] input);
    }
}
