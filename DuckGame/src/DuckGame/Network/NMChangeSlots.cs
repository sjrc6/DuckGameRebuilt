﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMChangeSlots
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Collections.Generic;

namespace DuckGame
{
    [FixedNetworkID(30231)]
    public class NMChangeSlots : NMDuckNetworkEvent
    {
        public List<byte> slots = new List<byte>();
        public bool originalConfiguration;

        public NMChangeSlots()
        {
        }

        public NMChangeSlots(List<byte> pSlots, bool pOriginalConfiguration)
        {
            slots = pSlots;
            originalConfiguration = pOriginalConfiguration;
        }

        protected override void OnSerialize()
        {
            _serializedData.Write(originalConfiguration);
            _serializedData.Write((byte)slots.Count);
            for (int index = 0; index < slots.Count; ++index)
                _serializedData.Write(slots[index]);
            base.OnSerialize();
        }

        public override void OnDeserialize(BitBuffer msg)
        {
            originalConfiguration = msg.ReadBool();
            slots = new List<byte>();
            byte num = msg.ReadByte();
            for (int index = 0; index < num; ++index)
                slots.Add(msg.ReadByte());
            base.OnDeserialize(msg);
        }

        public override void Activate()
        {
            if (!Network.isServer)
            {
                int index = 0;
                foreach (int slot in slots)
                {
                    if (index < DuckNetwork.profiles.Count)
                    {
                        DuckNetwork.profiles[index].slotType = (SlotType)slot;
                        if (originalConfiguration && index < DG.MaxPlayers)
                            DuckNetwork.profiles[index].originalSlotType = (SlotType)slot;
                    }
                    ++index;
                }
            }
            base.Activate();
        }
    }
}
