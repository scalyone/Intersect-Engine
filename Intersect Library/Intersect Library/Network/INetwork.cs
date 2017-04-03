﻿using System;

namespace Intersect.Network
{
    public interface INetwork : IDisposable
    {
        Guid Guid { get; }

        bool IsRunning { get; }

        bool Start();

        bool Stop();

        void Connect();

        void Disconnect(string message = "");

        bool Send(IPacket packet);
        bool Send(Guid guid, IPacket packet);

        IConnection FindConnection(Guid guid);
    }
}