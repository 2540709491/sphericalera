using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OpenBLive.Runtime.Data
{
    public struct Packet
    {
        private static readonly Packet s_NoBodyHeartBeatPacket = new()
        {
            Header = new PacketHeader()
            {
                HeaderLength = PacketHeader.KPacketHeaderLength,
                SequenceId = 1,
                ProtocolVersion = ProtocolVersion.HeartBeat,
                Operation = Operation.HeartBeat
            }
        };

        public PacketHeader Header;

        public int Length => Header.PacketLength;

        public byte[] PacketBody;

        public Packet(ReadOnlySpan<byte> bytes)
        {
            var headerBuffer = bytes[0..PacketHeader.KPacketHeaderLength];
            Header = new PacketHeader(headerBuffer);
            PacketBody = bytes[Header.HeaderLength..Header.PacketLength].ToArray();
        }

        public Packet(Operation operation, byte[] body = null)
        {
            Header = new PacketHeader
            {
                Operation = operation,
                ProtocolVersion = ProtocolVersion.UnCompressed,
                PacketLength = PacketHeader.KPacketHeaderLength + (body?.Length ?? 0)
            };
            PacketBody = body;
        }

        public byte[] ToBytes
        {
            get
            {
                if (PacketBody != null)
                    Header.PacketLength = Header.HeaderLength + PacketBody.Length;
                else
                    Header.PacketLength = Header.HeaderLength;
                var arr = new byte[Header.PacketLength];
                Array.Copy(((ReadOnlySpan<byte>) Header).ToArray(), arr, Header.HeaderLength);
                if (PacketBody != null)
                    Array.Copy(PacketBody, 0, arr, Header.HeaderLength, PacketBody.Length);
                return arr;
            }
        }

        /// <summary>
        /// 生成附带msg信息的心跳包
        /// </summary>
        /// <param name="msg">需要带的信息</param>
        /// <returns>心跳包</returns>
        public static Packet HeartBeat(string msg)
        {
            return HeartBeat(Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 生成附带msg信息的心跳包
        /// </summary>
        /// <param name="msg">需要带的信息</param>
        /// <returns>心跳包</returns>
        public static Packet HeartBeat(byte[] msg = null)
        {
            if (msg == null) return s_NoBodyHeartBeatPacket;
            return new Packet()
            {
                Header = new PacketHeader()
                {
                    PacketLength = PacketHeader.KPacketHeaderLength + msg.Length,
                    ProtocolVersion = ProtocolVersion.HeartBeat,
                    Operation = Operation.HeartBeat,
                    SequenceId = 1,
                    HeaderLength = PacketHeader.KPacketHeaderLength
                },
                PacketBody = msg
            };
        }

        /// <summary>
        /// 生成验证用数据包
        /// </summary>
        /// <param name="token">http请求获取的token</param>
        /// <param name="protocolVersion">协议版本</param>
        /// <returns>验证请求数据包</returns>
        public static Packet Authority(string token,
            ProtocolVersion protocolVersion = ProtocolVersion.Brotli)
        {
            var obj = Encoding.UTF8.GetBytes(token);
            // var obj = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
            //     new
            //     {
            //         protover = (int) protocolVersion,
            //         key = token,
            //     #if UNITY_EDITOR
            //         platform = "UNITY_EDITOR",
            //     #elif UNITY_ANDROID
            //         platform = "UNITY_ANDROID",
            //     #elif UNITY_IOS
            //         platform = "UNITY_IOS",
            //     #elif UNITY_STANDALONE_OSX
            //         platform = "UNITY_STANDALONE_OSX",
            //     #elif UNITY_STANDALONE_WIN
            //         platform = "UNITY_STANDALONE_WIN",
            //     #endif
            //         type = 2
            //     }));
            return new Packet
            {
                Header = new PacketHeader
                {
                    Operation = Operation.Authority,
                    ProtocolVersion = ProtocolVersion.HeartBeat,
                    SequenceId = 1,
                    HeaderLength = PacketHeader.KPacketHeaderLength,
                    PacketLength = PacketHeader.KPacketHeaderLength + obj.Length
                },
                PacketBody = obj
            };
        }
    }
}