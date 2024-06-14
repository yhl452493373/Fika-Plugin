﻿// © 2024 Lacyway All Rights Reserved

using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using Fika.Core.Coop.Players;
using Fika.Core.Networking;
using JetBrains.Annotations;
using System.IO;
using static EFT.Player;

namespace Fika.Core.Coop.BotClasses
{
    public class CoopBotInventoryController(Player player, Profile profile, bool examined) : PlayerInventoryController(player, profile, examined)
    {
        private readonly CoopBot CoopBot = (CoopBot)player;

        public override void Execute(GClass2854 operation, [CanBeNull] Callback callback)
        {
            base.Execute(operation, callback);

            InventoryPacket packet = new()
            {
                HasItemControllerExecutePacket = true
            };

            using MemoryStream memoryStream = new();
            using BinaryWriter binaryWriter = new(memoryStream);
            binaryWriter.WritePolymorph(FromObjectAbstractClass.FromInventoryOperation(operation, false));
            byte[] opBytes = memoryStream.ToArray();
            packet.ItemControllerExecutePacket = new()
            {
                CallbackId = operation.Id,
                OperationBytes = opBytes
            };

            CoopBot.PacketSender.InventoryPackets.Enqueue(packet);
        }
    }
}
