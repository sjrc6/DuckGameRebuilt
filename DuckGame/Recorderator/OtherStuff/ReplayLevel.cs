﻿using System.Collections;
using System.Collections.Generic;

namespace DuckGame
{
    public class ReplayLevel : Level, IHaveAVirtualTransition
    {
        public bool fake;
        public void DeserializeLevel(BitBuffer b)
        {
            //this is dumb
            byte bited = b.ReadByte();

            if (!fake) reAdd.Add(CCorderr);
            if (bited != 255)
            {
                BackgroundUpdater bu = (BackgroundUpdater)Editor.CreateThing(Recorderator.bgIDX[bited]);
                bu.y = -8000;
                reAdd.Add(bu);
            }
            Main.SpecialCode = "pipetilesets";
            int x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                byte shorted = b.ReadByte();
                BitArray a = new BitArray(new byte[] { shorted });
                PipeTileset p = null;
                int d = 16;
                int f = 0;
                for (int y = 0; y < 5; y++)
                {
                    if (a[y]) f += d;
                    d /= 2;
                }
                d = 2;
                int wow = 0;
                for (int y = 5; y < 7; y++)
                {
                    if (a[y]) wow += d;
                    d /= 2;
                }
                if (wow == 0) p = new PipeRed(v.x, v.y);
                else if (wow == 1) p = new PipeBlue(v.x, v.y);
                else if (wow == 2) p = new PipeGreen(v.x, v.y);
                p._sprite.frame = f;
                p.background.value = a[7];
                p._initializedConnections = false;
                p.searchUp = true;
                p.searchDown = true;
                p.searchLeft = true;
                p.searchRight = true;
                reAdd.Add(p);
            }

            Main.SpecialCode = "teleporters";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                byte b1 = b.ReadByte();
                byte b2 = b.ReadByte();
                BitArray a = new BitArray(new byte[] { b1, b2 });
                Teleporter t = new Teleporter(v.x, v.y);
                t.noduck = a[0];
                t.horizontal = a[1];
                int f = 0;
                int d = 4;
                for (int y = 2; y < 5; y++)
                {
                    if (a[y]) f += d;
                    d /= 2;
                }
                t.direction = f; //idk how i messed this up lol
                f = 0;
                d = 16;
                for (int y = 5; y < 10; y++)
                {
                    if (a[y]) f += d;
                    d /= 2;
                }
                t.teleHeight = f;
                reAdd.Add(t);
            }


            Main.SpecialCode = "springs/saws/spikes";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                byte byteOne = b.ReadByte();
                byte dataByte = b.ReadByte();
                Main.SpecialCode2 = "btr: " + byteOne + "  btwo: " + dataByte;
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                Thing somethingthing = null;
                switch (byteOne)
                {
                    case 0:
                        {
                            switch (dataByte)
                            {
                                case 0:
                                    somethingthing = new Saws(v.x, v.y);
                                    break;
                                case 1:
                                    somethingthing = new SawsDown(v.x, v.y);
                                    break;
                                case 2:
                                    somethingthing = new SawsLeft(v.x, v.y);
                                    break;
                                case 3:
                                    somethingthing = new SawsRight(v.x, v.y);
                                    break;
                                case 255:
                                default:
                                    continue;
                            }
                            break;
                        }
                    case 1:
                        {
                            switch (dataByte)
                            {
                                case 0:
                                    somethingthing = new Spikes(v.x, v.y);
                                    break;
                                case 1:
                                    somethingthing = new SpikesDown(v.x, v.y);
                                    break;
                                case 2:
                                    somethingthing = new SpikesLeft(v.x, v.y);
                                    break;
                                case 3:
                                    somethingthing = new SpikesRight(v.x, v.y);
                                    break;
                                case 255:
                                default:
                                    continue;
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (dataByte)
                            {
                                case 0:
                                    somethingthing = new Spring(v.x, v.y);
                                    break;
                                case 1:
                                    somethingthing = new SpringDown(v.x, v.y);
                                    break;
                                case 2:
                                    somethingthing = new SpringDownLeft(v.x, v.y);
                                    break;
                                case 3:
                                    somethingthing = new SpringDownRight(v.x, v.y);
                                    break;
                                case 4:
                                    somethingthing = new SpringLeft(v.x, v.y);
                                    break;
                                case 5:
                                    somethingthing = new SpringRight(v.x, v.y);
                                    break;
                                case 6:
                                    somethingthing = new SpringUpLeft(v.x, v.y);
                                    break;
                                case 7:
                                    somethingthing = new SpringUpRight(v.x, v.y);
                                    break;
                                case 255:
                                default:
                                    continue;
                            }
                            break;
                        }
                    case 3:
                        {
                            somethingthing = new ArcadeLight(v.x, v.y);
                            break;
                        }
                    case 4:
                        {
                            somethingthing = new PyramidLightRoof(v.x, v.y);
                            break;
                        }
                    case 5:
                        {
                            somethingthing = new PyramidWallLight(v.x, v.y);
                            break;
                        }
                    case 6:
                        {
                            somethingthing = new Bulb(v.x, v.y);
                            break;
                        }
                    case 7:
                        {
                            somethingthing = new HangingCityLight(v.x, v.y);
                            break;
                        }
                    case 8:
                        {
                            somethingthing = new Lamp(v.x, v.y);
                            break;
                        }
                    case 9:
                        {
                            somethingthing = new OfficeLight(v.x, v.y);
                            break;
                        }
                    case 10:
                        {
                            somethingthing = new WallLightRight(v.x, v.y);
                            break;
                        }
                    case 11:
                        {
                            somethingthing = new Sun(v.x, v.y);
                            break;
                        }
                    case 12:
                        {
                            somethingthing = new ArcadeTableLight(v.x, v.y);
                            break;
                        }
                    case 13:
                        {
                            somethingthing = new OfficeLight(v.x, v.y);
                            break;
                        }
                    case 14:
                        {
                            somethingthing = new WallLightLeft(v.x, v.y);
                            break;
                        }
                    case 15:
                        {
                            somethingthing = new FishinSign(v.x, v.y);
                            break;
                        }
                    case 16:
                        {
                            somethingthing = new WaterCooler(v.x, v.y);
                            break;
                        }
                    case 17:
                        {
                            Altar alt = new Altar(v.x, v.y, 0);
                            alt.wide.value = dataByte;
                            somethingthing = alt;
                            break;
                        }
                    case 18:
                        {
                            somethingthing = new SnowGenerator(v.x, v.y);
                            somethingthing.visible = false;
                            break;
                        }
                    case 19:
                        {
                            somethingthing = new WaterFall(v.x, v.y);
                            break;
                        }
                    case 20:
                        {
                            somethingthing = new WaterFallTile(v.x, v.y);
                            break;
                        }
                    case 80:
                        {
                            somethingthing = new MallardBillboard(v.x, v.y);
                            break;
                        }
                    case 81:
                        {
                            somethingthing = new ClippingSign(v.x, v.y);
                            ((ClippingSign)somethingthing).style.value = dataByte;
                            break;
                        }
                    case 82:
                        {
                            somethingthing = new SnowDrift(v.x, v.y, 0) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 83:
                        {
                            somethingthing = new StreetLight(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 84:
                        {
                            somethingthing = new PyramidBLight(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 85:
                        {
                            somethingthing = new TroubleLight(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 86:
                        {
                            somethingthing = new RaceSign(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 87:
                        {
                            somethingthing = new ArrowSign(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 88:
                        {
                            somethingthing = new DangerSign(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 89:
                        {
                            somethingthing = new EasySign(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 90:
                        {
                            somethingthing = new HardLeft(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 91:
                        {
                            somethingthing = new UpSign(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 92:
                        {
                            somethingthing = new VeryHardSign(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 93:
                        {
                            somethingthing = new SnowPile(v.x, v.y, 0) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 94:
                        {
                            somethingthing = new WaterFallEdge(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    case 95:
                        {
                            somethingthing = new WaterFallEdgeTop(v.x, v.y) { flipHorizontal = dataByte > 0 };
                            break;
                        }
                    default:
                        DevConsole.Log("|RED|RECORDERATOR |WHITE|A non indexed prop was found :" + byteOne);
                        break;
                }
                Main.SpecialCode = "a weird crash";
                if (somethingthing == null) continue;
                reAdd.Add(somethingthing);
            }
            Main.SpecialCode2 = "";

            Main.SpecialCode = "rprops";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                byte z = b.ReadByte();
                BitArray br = new BitArray(new byte[] { z });

                int switched = 0;
                int divide = 8;
                for (int y = 0; y < 4; y++)
                {
                    if (br[y]) switched += divide;
                    divide /= 2;
                }
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                Thing t;
                switch (switched)
                {
                    case 0:
                        t = new CityWall(v.x, v.y);
                        break;
                    case 1:
                        t = new TreeTop(v.x, v.y);
                        break;
                    case 2:
                        t = new TreeTopDead(v.x, v.y);
                        break;
                    case 3:
                        t = new RockWall(v.x, v.y);
                        break;
                    case 4:
                        t = new PyramidWall(v.x, v.y);
                        break;
                    case 5:
                        t = new VerticalDoor(v.x, v.y);
                        break;
                    case 6:
                        t = new PyramidDoor(v.x, v.y);
                        break;
                    case 7:
                        t = new IceWedge(v.x, v.y, 0);
                        break;
                    case 8:
                        t = new CityRamp(v.x, v.y, 0);
                        break;
                    case 9:
                        t = new WaterFlow(v.x, v.y);
                        break;
                    default:
                        reAdd.Add(new NilVessel(v, "lev vessel: " + switched + " idx: " + i));
                        continue;
                }
                t.flipVertical = br[6];
                t.flipHorizontal = br[7];
                reAdd.Add(t);
            }

            Main.SpecialCode = "block s";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                ushort indx = b.ReadUShort();
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                byte bote = b.ReadByte();

                AutoBlock bb = (AutoBlock)Editor.CreateThing(Recorderator.autoBlockIDX[bote]);
                bb.blockIndex = indx;
                bb.position = v;

                reAdd.Add(bb);
            }

            Main.SpecialCode = "tile s";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                byte bote = b.ReadByte();

                AutoTile bb = (AutoTile)Editor.CreateThing(Recorderator.autoTileIDX[bote]);
                bb.position = v;

                reAdd.Add(bb);
            }

            Main.SpecialCode = "platform s";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                Vec2 v = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                byte bote = b.ReadByte();

                AutoPlatform pp = (AutoPlatform)Editor.CreateThing(Recorderator.autoPlatIDX[bote]);
                pp.position = v;

                reAdd.Add(pp);
            }

            Main.SpecialCode = "its in here somewhere";
            x = b.ReadUShort();
            for (int i = 0; i < x; i++)
            {
                Main.SpecialCode2 = "posRead";
                Vec2 position = CompressedVec2Binding.GetUncompressedVec2(b.ReadInt(), 10000);
                Main.SpecialCode2 = "FrameRead";
                ushort Frame = b.ReadUShort();
                Main.SpecialCode2 = "bgTileIndex Read";
                byte bgTileIndex = b.ReadByte();

                //replaced this with a thing because ForegroundTile doesn't inherit BackgroundTile resulting in a crash when casting those to BackgroundTiles
                Thing bgTiles = Editor.CreateThing(Recorderator.bgtileIDX[bgTileIndex]);
                bgTiles.position = position;

                Main.SpecialCode2 = "Bit array creation";
                BitArray b_arr = new BitArray(16);
                BitCrusher.UShortIntoArray(Frame, ref b_arr);

                int val = 0;
                int divide = 1024;
                for (int y = 0; y < 11; y++)
                {
                    Main.SpecialCode2 = "ll Loop " + y;
                    if (b_arr[y]) val += divide;
                    divide /= 2;
                }
                Main.SpecialCode2 = "Something went wrong here";
                bgTiles.frame = val;
                bgTiles.flipHorizontal = b_arr[15];
                reAdd.Add(bgTiles);
            }
            Main.SpecialCode2 = "";
            Main.SpecialCode = "out of lev buffer";
        }
        public List<Thing> reAdd = new List<Thing>();
        public Corderator CCorderr;

        public int frame;
        public void FakeUpdate()
        {
            if (frame == 0)
            {
                List<AutoBlock> autoBlocks = Extensions.GetListOfThings<AutoBlock>();
                for (int i = 0; i < autoBlocks.Count; i++) autoBlocks[i].PlaceBlock();
                List<AutoPlatform> autoPlatforms = Extensions.GetListOfThings<AutoPlatform>();
                for (int i = 0; i < autoPlatforms.Count; i++) autoPlatforms[i].PlaceBlock();
                List<AutoTile> autoTiles = Extensions.GetListOfThings<AutoTile>();
                for (int i = 0; i < autoTiles.Count; i++) autoTiles[i].PlaceBlock();
            }
            frame++;
            base.Update();
        }
        public override void Update()
        {
            Recorderator.Playing = true;
            if (CCorderr.cFrame == 0)
            {
                List<AutoBlock> autoBlocks = Extensions.GetListOfThings<AutoBlock>();
                for (int i = 0; i < autoBlocks.Count; i++) autoBlocks[i].PlaceBlock();
                List<AutoPlatform> autoPlatforms = Extensions.GetListOfThings<AutoPlatform>();
                for (int i = 0; i < autoPlatforms.Count; i++) autoPlatforms[i].PlaceBlock();
                List<AutoTile> autoTiles = Extensions.GetListOfThings<AutoTile>();
                for (int i = 0; i < autoTiles.Count; i++) autoTiles[i].PlaceBlock();
            }

            if (Keyboard.Pressed(Keys.Home))
            {
                Recorderator.Playing = false;
                if (prev != null) current = prev;
                else current = new SendToLevel(new RecorderationSelector());
                return;
            }
            
            base.Update();
        }

        public RecorderationSelector prev;
        public override void PostDrawLayer(Layer layer)
        {
            if (layer == Layer.Console)
            {
                DuckNetwork.Draw();
            }
            base.PostDrawLayer(layer);
        }
        public override void Initialize()
        {
            DuckNetwork.core.chatMessages.Clear();
            for (int i = 0; i < reAdd.Count; i++)
            {
                AddThing(reAdd[i]);
            }
            base.Initialize();
        }
    }
}
