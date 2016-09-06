using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using ProjectCars.DataFormat;

namespace ProjectCars.Readers
{
    class UDPReader
    {
        private readonly IPEndPoint _broadcastAddress;
        private readonly UdpClient _udpClient;
        private readonly AsyncCallback _socketCallback;
        private readonly byte[] _receivedDataBuffer;

        private byte[] _sTelemetryBuffer = new byte[UDPDataFormat.telemetrySize];
        private UDPDataFormat.sTelemetryData _telemetryData;

        private byte[] _participantInfoStringsBuffer = new byte[UDPDataFormat.participantInfoSize];
        private UDPDataFormat.sParticipantInfoStrings _participantInfoStrings;

        private byte[] _participantInfoStringsAdditionalBuffer = new byte[UDPDataFormat.participantInfoAdditionalSize];
        private UDPDataFormat.sParticipantInfoStringsAdditional _participantInfoStringsAdditional;

        private bool _readData = true;

        public UDPReader()
        {
            _socketCallback = new AsyncCallback(ReceiveCallback);
            _broadcastAddress = new IPEndPoint(IPAddress.Any, 5606);
            _udpClient = new UdpClient();
            _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpClient.ExclusiveAddressUse = false;
            _udpClient.Client.Bind(_broadcastAddress);
            _receivedDataBuffer = new byte[_udpClient.Client.ReceiveBufferSize];
            _udpClient.Client.BeginReceive(_receivedDataBuffer, 0, _receivedDataBuffer.Length, SocketFlags.None, ReceiveCallback, _udpClient.Client);
        }

        #region Getters and Setters
        public UDPDataFormat.sParticipantInfoStringsAdditional ParticipantInfoStringsAdditional
        {
            get
            {
                return _participantInfoStringsAdditional;
            }
            set
            {
                _participantInfoStringsAdditional = value;
            }
        }

        public UDPDataFormat.sParticipantInfoStrings ParticipantInfoStrings
        {
            get
            {
                return _participantInfoStrings;
            }
            set
            {
                _participantInfoStrings = value;
            }
        }

        public UDPDataFormat.sTelemetryData TelemetryData
        {
            get
            {
                return _telemetryData;
            }
            set
            {
                _telemetryData = value;
            }
        }
        #endregion

        public void Stop()
        {
            _readData = false;
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                Socket socket = (Socket)result.AsyncState;
                int received = socket.EndReceive(result);
                if (received > 0)
                {
                    ReceiveData(_receivedDataBuffer);
                }
                if (_readData)
                {
                    socket.BeginReceive(_receivedDataBuffer, 0, _receivedDataBuffer.Length, SocketFlags.None, _socketCallback, socket);
                }

            }
            catch (Exception e)
            {
                if (e is SocketException)
                {
                    return;
                }
                throw;
            }
        }

        private void ReceiveData(byte[] data)
        {
            int packetInfo = data[2];
            int packetType = packetInfo & 3;
            int sequenceNumber = packetInfo >> 2;

          //  Console.WriteLine(packetType.ToString() + "  " + sequenceNumber.ToString());

            switch (packetType)
            {
                case 0:
                    ProcessPacket0(data);
                    break;

                case 1:
                    ProcessPacket1(data);
                    break;

                case 2:
                    ProcessPacket2(data);
                    break;
            }
        }

        private bool ProcessPacket0(byte[] data)
        {
            _sTelemetryBuffer = data;
            GCHandle gCHandle = GCHandle.Alloc(_sTelemetryBuffer, GCHandleType.Pinned);
            try
            {
                lock (this) // YUK
                {
                    _telemetryData = (UDPDataFormat.sTelemetryData)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(UDPDataFormat.sTelemetryData));
                }
            }
            finally
            {
                gCHandle.Free();
            }
            return true;
        }

        private bool ProcessPacket1(byte[] data)
        {
            _participantInfoStringsBuffer = data;
                GCHandle gCHandle = GCHandle.Alloc(_participantInfoStringsBuffer, GCHandleType.Pinned);
                try
                {
                    lock (this) // YUK
                    {
                        _participantInfoStrings = (UDPDataFormat.sParticipantInfoStrings)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(UDPDataFormat.sParticipantInfoStrings));
                    }
                }
                finally
                {
                    gCHandle.Free();
                }
            return true;
        }

        private bool ProcessPacket2(byte[] data)
        {
            _participantInfoStringsAdditionalBuffer = data;
            GCHandle gCHandle = GCHandle.Alloc(_participantInfoStringsAdditionalBuffer, GCHandleType.Pinned);
            try
            {
                lock (this) //YUK
                {
                    _participantInfoStringsAdditional = (UDPDataFormat.sParticipantInfoStringsAdditional)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(UDPDataFormat.sParticipantInfoStringsAdditional));
                }
            }
            finally
            {
                gCHandle.Free();
            }
            return true;
        }
    }
}
