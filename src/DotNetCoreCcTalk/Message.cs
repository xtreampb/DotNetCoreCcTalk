using DotNetCoreCcTalk.crc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DotNetCoreCcTalk
{
    public enum ChecksumType
    {
        Crc8bit,
        Crc16bit,
        CrC16bitCCITT
    }

    public class CcTalkException : Exception
    {
        public CcTalkException() { }

        public CcTalkException(string message) : base(message) { }

        public CcTalkException(string message, Exception innerException) : base(message, innerException) { }

    }

    public sealed class Message
    {
        private readonly ICRCProvider _crcProvider;

        public readonly ChecksumType CrcType;

        public byte DestinationAddress { get; }

        // TODO: is currently incorrect and needs time to reflect protocol
        public int DataByteCount =>  Data.Length;

        public byte SourceAddress { get; }

        public byte Header { get; }

        public byte[] Data { get; }

        // TODO: is currently incorrect and doesn't include data
        private byte[] Checksum { get
            {
                switch(CrcType)
                {
                    default:
                    case (ChecksumType.Crc8bit):
                        return new[] { _crcProvider.CalculateCRC8(new[] { Header }) };
                    case (ChecksumType.Crc16bit):
                        return _crcProvider.CalculateCRC16(new[] { Header });
                    case (ChecksumType.CrC16bitCCITT):
                        return _crcProvider.CalculateCRC16CCITT(new[] { Header });
                }
            } }

        // TODO: Create ICRCProvider implementation as a default implementation.
        public Message(byte destinationAddr, byte sourceAddr, byte header, byte[] data, ICRCProvider crcProvider, ChecksumType crcType = ChecksumType.Crc8bit)
        {
            DestinationAddress = destinationAddr;
            SourceAddress = sourceAddr;
            Header = header;
            Data = data;
            _crcProvider = crcProvider;
            CrcType = crcType;
        }

        public byte[] SerializeMessage()
        {
            List<byte> serializedMessage = new List<byte>();

            serializedMessage.Add(DestinationAddress);
            serializedMessage.Add((byte)DataByteCount);
            serializedMessage.Add(SourceAddress);
            serializedMessage.Add(Header);
            serializedMessage.AddRange(Data);
            serializedMessage.AddRange(Checksum);

            var mssgArray = serializedMessage.ToArray();

            if (mssgArray.Count() > 256)
                throw new CcTalkException("Message length exceeded 256 bytes.  Automatic message pagination not currently supported.  Try splitting up you command into multiple messages");

            return serializedMessage.ToArray();
        }
    }
}
