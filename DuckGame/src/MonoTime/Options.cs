﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DuckGame
{
    public static class Options
    {
        private static UIMenu _optionsMenu;
        private static UIMenu _controllerWarning;
        private static OptionsData _data = new OptionsData();
        private static OptionsDataLocal _localData = new OptionsDataLocal();
        private static bool _removedOptionsMenu = false;
        private static bool _openedOptionsMenu = false;
        public static UIMenu openOnClose = null;
        private static UIMenu _graphicsMenu;
        private static UIMenu _audioMenu;
        private static UIMenu _accessibilityMenu;
        private static UIMenu _ttsMenu;
        private static UIMenu _blockMenu;
        private static UIMenu _controlsMenu;
        public static UIMenu _lastCreatedAudioMenu;
        public static UIMenu _lastCreatedGraphicsMenu;
        public static UIMenu _lastCreatedAccessibilityMenu;
        public static UIMenu _lastCreatedTTSMenu;
        public static UIMenu _lastCreatedBlockMenu;
        public static UIMenu _lastCreatedControlsMenu;
        public static UIMenu _lastCreatedDGRMenu;
        public static UIMenu _lastCreatedDGRQOLMenu;
        public static UIMenu _lastCreatedDGRHudMenu;
        public static UIMenu _lastCreatedOptimizationsMenu;
        public static UIMenu _lastCreatedDGRGraphicsMenu;
        public static UIMenu _lastCreatedDGREditorMenu;
        public static UIMenu _lastCreatedDGRDumbShitMenu;
        public static UIMenu _lastCreatedDGRDeveloperMenu;
        public static UIMenu _lastCreatedDGRRecorderatorMenu;
        public static int flagForSave = 0;
        private static bool _doingResolutionRestart = false;
        private static List<string> chatFonts = new List<string>()
        {
          "Duck Font",
          "Arial",
          "Calibri",
          "Courier New",
          "Comic Sans MS",
          "Custom"
        };
        public static UIMenu tempTTSMenu;
        public static UIMenu tempBlockMenu;
        private static bool loadCalled = false;
        public static int legacyPreferredColor = -1;
        //private static string _pendingTTS;
        private static bool _resolutionChanged = false;

        public static string GetMuteSettings(Profile pProfile)
        {
            string str;
            return Data.muteSettings.TryGetValue(pProfile.steamID, out str) ? str : "";
        }

        public static void SetMuteSetting(Profile pProfile, string pSetting, bool pValue)
        {
            string str;
            if (!Data.muteSettings.TryGetValue(pProfile.steamID, out str))
                str = Data.muteSettings[pProfile.steamID] = "";
            if (pValue && !str.Contains(pSetting))
                str += pSetting;
            else if (!pValue)
                str = str.Replace(pSetting, "");
            Data.muteSettings[pProfile.steamID] = str;
        }

        public static UIMenu optionsMenu => _optionsMenu;

        public static UIMenu controllerWarning => _controllerWarning;

        public static OptionsData Data
        {
            get => _data;
            set => _data = value;
        }

        public static OptionsDataLocal LocalData
        {
            get => _localData;
            set => _localData = value;
        }

        public static bool menuOpen => _optionsMenu.open;

        public static UIMenu graphicsMenu => _graphicsMenu;

        public static UIMenu audioMenu => _audioMenu;

        public static UIMenu DGRMenu => _DGRMenu;

        public static UIMenu accessibilityMenu => _accessibilityMenu;

        public static UIMenu ttsMenu => _ttsMenu;

        public static UIMenu blockMenu => _blockMenu;

        public static UIMenu controlsMenu => _controlsMenu;

        public static void AddMenus(UIComponent to)
        {
            to.Add(optionsMenu, false);
            to.Add(graphicsMenu, false);
            to.Add(audioMenu, false);
            to.Add(DGRMenu, false);
            //AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
            //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
            //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
            //Best. -NiK0
            to.Add(_lastCreatedOptimizationsMenu, false);
            to.Add(_lastCreatedDGRGraphicsMenu, false);
            to.Add(_lastCreatedDGRQOLMenu, false);
            to.Add(_lastCreatedDGRHudMenu, false);
            to.Add(_lastCreatedDGREditorMenu, false);
            #if AutoUpdater
            #else
            to.Add(_lastCreatedDGRRecorderatorMenu, false);
            #endif
            to.Add(_lastCreatedDGRDumbShitMenu, false);
            if (Program.IS_DEV_BUILD)
                to.Add(_lastCreatedDGRDeveloperMenu, false);


            if (accessibilityMenu != null)
                to.Add(accessibilityMenu, false);
            if (ttsMenu != null)
                to.Add(ttsMenu, false);
            if (blockMenu != null)
                to.Add(blockMenu, false);
            if (controlsMenu != null)
            {
                to.Add(controlsMenu, false);
                to.Add((controlsMenu as UIControlConfig)._confirmMenu, false);
                to.Add((controlsMenu as UIControlConfig)._warningMenu, false);
            }
            to.Add(controllerWarning, false);
        }

        public static void QuitShowingControllerWarning()
        {
            Data.showControllerWarning = false;
            Save();
        }

        public static UIMenu CreateOptionsMenu()
        {
            UIMenu optionsMenu = new UIMenu("@WRENCH@OPTIONS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@CANCEL@BACK @SELECT@SELECT");
            optionsMenu.Add(new UIMenuItemSlider("SFX Volume", field: new FieldBinding(Data, "sfxVolume"), step: 0.06666667f), true);
            optionsMenu.Add(new UIMenuItemSlider("Music Volume", field: new FieldBinding(Data, "musicVolume"), step: 0.06666667f), true);
            optionsMenu.Add(new UIMenuItemSlider("Rumble Intensity", field: new FieldBinding(Data, "rumbleIntensity"), step: 0.06666667f), true);
            optionsMenu.Add(new UIText(" ", Color.White), true);
            optionsMenu.Add(new UIMenuItemToggle("SHENANIGANS", field: new FieldBinding(Data, "shennanigans")), true);
            _lastCreatedControlsMenu = CreateControlsMenu(optionsMenu);
            _lastCreatedGraphicsMenu = CreateGraphicsMenu(optionsMenu);
            _lastCreatedAccessibilityMenu = CreateAccessibilityMenu(optionsMenu);
            _lastCreatedTTSMenu = tempTTSMenu;
            _lastCreatedBlockMenu = tempBlockMenu;
            _lastCreatedAudioMenu = CreateAudioMenu(optionsMenu);
            _lastCreatedDGRMenu = CreateDGRMenu(optionsMenu);
            _lastCreatedDGRQOLMenu = TEMPDGRQOL;
            _lastCreatedDGRHudMenu = TEMPDGRHUD;
            _lastCreatedDGRGraphicsMenu = TEMPDGRGRAPHICS;
            _lastCreatedOptimizationsMenu = TEMPDGROPTIM;
            _lastCreatedDGREditorMenu = TEMPDGREDITOR;
            _lastCreatedDGRDumbShitMenu = TEMPDGRMISC;
            _lastCreatedDGRDeveloperMenu = TEMPDGRDEV;
            _lastCreatedDGRRecorderatorMenu = TEMPDGRRECORDERATOR;
            //DGR OPTIONS GUI HELL BEGINS HERE -NiK0

            optionsMenu.Add(new UIText(" ", Color.White), true);

            optionsMenu.Add(new UIMenuItem("REBUILT|PINK|♠", new UIMenuActionOpenMenu(optionsMenu, _lastCreatedDGRMenu), backButton: true), true);
            optionsMenu.Add(new UIMenuItem("EDIT CONTROLS", new UIMenuActionOpenMenu(optionsMenu, _lastCreatedControlsMenu), backButton: true), true);
            optionsMenu.Add(new UIMenuItem("GRAPHICS", new UIMenuActionOpenMenu(optionsMenu, _lastCreatedGraphicsMenu), backButton: true), true);
            optionsMenu.Add(new UIMenuItem("AUDIO", new UIMenuActionOpenMenu(optionsMenu, _lastCreatedAudioMenu), backButton: true), true);
            optionsMenu.Add(new UIText(" ", Color.White), true);
            optionsMenu.Add(new UIMenuItem("USABILITY", new UIMenuActionOpenMenu(optionsMenu, _lastCreatedAccessibilityMenu), backButton: true), true);
            optionsMenu.SetBackFunction(new UIMenuActionCloseMenuCallFunction(optionsMenu, new UIMenuActionCloseMenuCallFunction.Function(OptionsMenuClosed)));
            optionsMenu.Close();

            return optionsMenu;
        }

        public static void Initialize()
        {
            _optionsMenu = CreateOptionsMenu();
            _controllerWarning = CreateControllerWarning();
            _controlsMenu = _lastCreatedControlsMenu;
            _graphicsMenu = _lastCreatedGraphicsMenu;
            _accessibilityMenu = _lastCreatedAccessibilityMenu;
            _audioMenu = _lastCreatedAudioMenu;
            _ttsMenu = _lastCreatedTTSMenu;
            _blockMenu = _lastCreatedBlockMenu;
            _DGRMenu = _lastCreatedDGRMenu;
            _DGRHudMenu = _lastCreatedDGRHudMenu;
            _DGRQOLMenu = _lastCreatedDGRQOLMenu;
            _DGROptimMenu = _lastCreatedOptimizationsMenu;
            _DGRGraphicsMenu = _lastCreatedDGRGraphicsMenu;
            _DGRRecorderatorMenu = _lastCreatedDGRRecorderatorMenu;
            _DGREditorMenu = _lastCreatedDGREditorMenu;
            _DGRDumbShitMenu = _lastCreatedDGRDumbShitMenu;
            _DGRDeveloperMenu = _lastCreatedDGRDeveloperMenu;
        }

        public static UIMenu CreateControllerWarning()
        {
            UIMenu menu = new UIMenu("Is that a PS4 Controller?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 220f, conString: "@CANCEL@BACK @SELECT@SELECT");
            UIMenu uiMenu1 = menu;
            UIText component1 = new UIText("", Color.White, heightAdd: -3f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu1.Add(component1, true);
            UIMenu uiMenu2 = menu;
            UIText component2 = new UIText("It seems you may have a |DGBLUE|PS4 Controller", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu2.Add(component2, true);
            UIMenu uiMenu3 = menu;
            UIText component3 = new UIText("plugged in! If so, and if you are running", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu3.Add(component3, true);
            UIMenu uiMenu4 = menu;
            UIText component4 = new UIText("|DGBLUE|DS4Windows|PREV| or a |DGBLUE|3rd party PS4 Controller Driver|PREV|,", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu4.Add(component4, true);
            UIMenu uiMenu5 = menu;
            UIText component5 = new UIText("you may need to |DGRED|disable|PREV| it.", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu5.Add(component5, true);
            UIMenu uiMenu6 = menu;
            UIText component6 = new UIText("", Color.White, heightAdd: -3f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu6.Add(component6, true);
            UIMenu uiMenu7 = menu;
            UIText component7 = new UIText("If everything works okay, you can ignore", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu7.Add(component7, true);
            UIMenu uiMenu8 = menu;
            UIText component8 = new UIText("this message. If you're controlling 2 ducks", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu8.Add(component8, true);
            UIMenu uiMenu9 = menu;
            UIText component9 = new UIText("at once, then this message is for you!", Color.White, heightAdd: -4f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu9.Add(component9, true);
            UIMenu uiMenu10 = menu;
            UIText component10 = new UIText("", Color.White, heightAdd: -3f)
            {
                scale = new Vec2(0.5f)
            };
            uiMenu10.Add(component10, true);
            menu.Add(new UIMenuItem("|DGORANGE|OK THEN", new UIMenuActionCloseMenu(menu), c: Color.White));
            menu.Add(new UIMenuItem("|DGRED|DON'T SHOW THIS AGAIN", new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(QuitShowingControllerWarning)), c: Color.White));
            menu.SetBackFunction(new UIMenuActionCloseMenu(menu));
            menu.Close();
            return menu;
        }

        public static float GetWindowScaleMultiplier()
        {
            if (Data.windowScale == 0)
                return 1f;
            return Data.windowScale == 1 ? 1.5f : 2f;
        }

        public static void ScreenModeChanged(string pMode)
        {
            if (pMode == "Fullscreen")
            {
                // From dan Auto Resize Window in cases of fullscreening, as if not it uses incorrect res
                int windowindex = SDL2.SDL.SDL_GetWindowDisplayIndex(MonoMain.instance.Window.Handle);
                if (windowindex >= 0 && windowindex < GraphicsAdapter.Adapters.Count)
                {
                    GraphicsAdapter currentdisplay = GraphicsAdapter.Adapters[windowindex];
                    LocalData.windowedFullscreenResolution.x = currentdisplay.CurrentDisplayMode.Width;
                    LocalData.windowedFullscreenResolution.y = currentdisplay.CurrentDisplayMode.Height;
                    LocalData.fullscreenResolution.x = currentdisplay.CurrentDisplayMode.Width;
                    LocalData.fullscreenResolution.y = currentdisplay.CurrentDisplayMode.Height;
                }
                Resolution.Set(Data.windowedFullscreen ? LocalData.windowedFullscreenResolution : LocalData.fullscreenResolution);
            }
            else
                Resolution.Set(LocalData.windowedResolution);
        }

        public static void WindowedFullscreenChanged()
        {
            if (Resolution.current.mode == ScreenMode.Windowed)
                return;
            ScreenModeChanged("Fullscreen");
        }

        public static void FullscreenChanged() => ScreenModeChanged(Data.fullscreen ? "Fullscreen" : "Windowed");

        private static void ExclusiveAudioModeChanged() => SFX._audio.LoseDevice();

        private static void AudioEngineChanged()
        {
            MonoMain.audioModeOverride = (AudioMode)Data.audioMode;
            Windows_Audio.ResetDevice();
        }

        private static void MuteOnBackground()
        {

        }

        private static void ApplyResolution()
        {
            if (!_doingResolutionRestart)
                Resolution.Set(LocalData.currentResolution);
            if (LocalData.currentResolution.mode == ScreenMode.Fullscreen)
                LocalData.fullscreenResolution = LocalData.currentResolution;
            else if (LocalData.currentResolution.mode == ScreenMode.Borderless)
                LocalData.windowedFullscreenResolution = LocalData.currentResolution;
            else
                LocalData.windowedResolution = LocalData.currentResolution;
        }
        public static UIMenu _DGRMenu;
        public static UIMenu _DGRGraphicsMenu;
        public static UIMenu _DGROptimMenu;
        public static UIMenu _DGRQOLMenu;
        public static UIMenu _DGRHudMenu;
        public static UIMenu _DGREditorMenu;
        public static UIMenu _DGRDumbShitMenu;
        public static UIMenu _DGRDeveloperMenu;
        public static UIMenu _DGRRecorderatorMenu;

        public static UIMenu CreateDGREditorMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|EDITOR|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Online Physics", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.EditorOnlinePhysics)))
            {
                dgrDescription = "WARNING This may be highly unstable but it'll make it so online physics apply while testing levels in the editor (Ragdoll rng, etc)"
            });

            menu.Add(new UIMenuItemToggle("Test Timer", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.EditorTimer)))
            {
                dgrDescription = "Displays a timer of how much time the current level has been running for while testing it in the editor"
            });

            menu.Add(new UIMenuItemToggle("Instructions", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.EditorInstructions)))
            {
                dgrDescription = "Displays real-time instructions on how to operate the editor. You might not need them anymore if you're already familiar with everything"
            });

            menu.Add(new UIMenuItemToggle("Level Name", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.EditorLevelName)))
            {
                dgrDescription = "Displays current level name at top left of the screen"
            });
            //EditorTimer

            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }
        
        public static UIMenu CreateDGRDumbShitMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|MISCELLANEOUS|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Green Text Support", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.GreenTextSupport)))
            {
                dgrDescription = "Chat messages beginning with a \">\" will be green (only for your game)"
            });
            menu.Add(new UIMenuItemToggle("Discord RPC", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.RPC)))
            {
                dgrDescription = "Toggles discord rich presence displaying the current level, if you're in the editor, etc\n(May take a few seconds to connect)"
            });
            menu.Add(new UIMenuItemToggle("Open URLs in Browser", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.OpenURLsInBrowser)))
            {
                dgrDescription = "URLs will open in your web browser instead of the Steam Overlay."
            });
            menu.Add(new UIMenuItemToggle("Custom Hat Teams", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.CustomHatTeams)))
            {
                dgrDescription = "Allows for teams with custom hats that have the same name (HOST ONLY)"
            });
            menu.Add(new UIMenuItem("Reload Hats", new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(ReloadHats)))
            {
                dgrDescription = "Reloads all hats (OFFLINE ONLY, MIGHT REMOVE MODDED HATS, F6 QUICK RELOAD, F5 RELOADS CURRENTLY WORN ONE)"
            });

            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }
        
        // go wild with option naming here
        public static UIMenu CreateDGRDeveloperMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|DEVELOPER|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 260f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });
            
            menu.Add(new UIMenuItemToggle("No Level Restrictions", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.IgnoreLevRestrictions)))
            {
                dgrDescription = "When enabled, you'll be able to turn on any custom level on an online match"
            });

            menu.Add(new UIMenuItemToggle("Faster Load", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.FasterLoad)))
            {
                dgrDescription = "If this is enabled hats, effects, devconsole, challenges, textures wont load on startup resulting in instability so use at your own risk"
            });

            menu.Add(new UIMenuItemToggle("Sync Ching", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SyncChing)))
            {
                dgrDescription = "Want everyone to know that you just clipped them? Turn this on and other people will be able to hear the ching when you clip with Recorderator!"
            });

            menu.Add(new UIMenuItemToggle("Alt SeqCrate Texture", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SequenceCrateRetexture)))
            {
                dgrDescription = "Retextures the Sequence Crate so it doesn't look indentical to the regular crate"
            });

            menu.Add(new UIMenuItemToggle("QR Code Join Links", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.QRCodeJoinLinks)))
            {
                dgrDescription = "Copies a QR code representing the join link"
            });

            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }

        public static UIMenu CreateDGRGraphicsMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|GRAPHICS|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Uncap FPS", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.UncappedFPS)))
            {
                dgrDescription = "Game will use interpolation to render at higher than 60fps (REQUIRES RESTART)"
            });
            menu.Add(new UIMenuItemToggle("Use V-Sync", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.UseVSync)))
            {
                dgrDescription = "Verticaly synced drawing, overrides FPS target (REQUIRES RESTART)"
            });

            menu.Add(new UIMenuItemNumber("FPS Target", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.TargetFrameRate), 0, 1000, 60), step: 60)
            {
                dgrDescription = "Tries to target a specific FPS value, set to 0 for unlimited FPS"
            });
            menu.Add(new UIMenuItemToggle("Frame Stacking", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.UseFrameStacking)))
            {
                dgrDescription = "Cursed"
            });

            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemSlider("Weather Chance", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.RandomWeather), 0, 10, 1), step: 1f)
            {
                dgrDescription = "Chance for random weather to occur in levels from 0% to 100%"
            });
            menu.Add(new UIMenuItemSlider("Weather Particle Level", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.WeatherMultiplier), 0, 16, 1), step: 1f)
            {
                dgrDescription = "Particle multiplier for weather events"
            });
            menu.Add(new UIMenuItemSlider("Weather Thunder Chance", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.WeatherLighting), 0, 16, 1), step: 1f)
            {
                dgrDescription = "Chance for thunder to occur in levels from x0 to x16"
            });

            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemSlider("Heat Wave Strength", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.HeatWaveMultiplier), 0, 1, 0.1f), step: 0.1f)
            {
                dgrDescription = "The strength that the heat wave shader has on the enviroment around it"
            });

            menu.Add(new UIMenuItemToggle("Ambient Particles", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.AmbientParticles)))
            {
                dgrDescription = "Extra cosmetic particles added by DGR, embers from lamps, leafs from trees, etc"
            });

            menu.Add(new UIMenuItemToggle("Explosion Decals", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.ExplosionDecals)))
            {
                dgrDescription = "Toggles comestic dust decals when explosions happen"
            });


            menu.Add(new UIMenuItemNumber("Particle Level", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.ParticleMultiplier), 0, 7, 1), valStrings: new List<string>()
            {
                "None     ",
                "Minimum     ",
                "Low     ",
                "Default     ",
                "Many     ",
                "EXTREME     ",
                "WUMBO     ",
                "|RED|UNCOUNTABLE"
            })
            {
                dgrDescription = "Global particle multiplier from x0 to x16"
            });
            menu.Add(new UIMenuItemNumber("Rebuilt Effect", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.RebuiltEffect), 0, 2, 1), valStrings: new List<string>()
            {
                "HEART",
                "NAME",
                "NONE :(",
            })
            {
                dgrDescription = "The effect displayed for other rebuilt users"
            });


            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }

        public static UIMenu CreateDGRQOLMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|QOL|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Camera unfollow", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.CameraUnfollow)))
            {
                dgrDescription = "When the camera is big enough it stops following distant players"
            });
            menu.Add(new UIMenuItemNumber("Start in", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.StartIn), 0, 3), valStrings: new List<string>
            {
                "TITLE",
                "LOBBY",
                "EDITOR",
                "ARCADE"
            })
            {
                dgrDescription = "When starting up the game you'll spawn into the selected level"
            });
            menu.Add(new UIMenuItemToggle("Sticky Hats", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.StickyHats)))
            {
                dgrDescription = "Vanity hats no longer fall off when ragdolling"
            });

            menu.Add(new UIMenuItemToggle("Lobby data", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.LobbyData)))
            {
                dgrDescription = "Shows the percentage of maps and the list of people in the lobby if host uses Rebuilt"
            });
            //SkipXP
            menu.Add(new UIMenuItemToggle("Skip Excess Rounds", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SkipExcessRounds)))
            {
                dgrDescription = "If a player has already definitely won extra rounds that wont change the outcome of the match will be skipped (HOST ONLY)"
            });

            menu.Add(new UIMenuItemToggle("Auto Input Switch", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SwitchInput)))
            {
                dgrDescription = "On the titlescreen all input profiles will work as the main duck adjusting them dynamically"
            });
            menu.Add(new UIMenuItemToggle("Skip XP", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SkipXP)))
            {
                dgrDescription = "Completely skips the XP level up and Vincent dialogue. You wont get the XP from it if this is ON"
            });
            menu.Add(new UIMenuItemNumber("Invite Link", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.DGRJoinLink), 0, 3), valStrings: new List<string>
            {
                "STEAM",
                "DGR",
                "BOTH (D)",
                "BOTH (G)"
            })
            {
                dgrDescription = "STEAM    - Regular steam link\nDGR      - Custom DGR link (https protocol)\nBOTH (D) - Both in one link, clickable on Discord\nBOTH (G) - Both copied but as seperate links",
                manualFormatting = true
            });


            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }

        public static void ReloadHats()
        {
            if (Network.isActive) return;
            try
            {
                if (Teams.core != null && Teams.core.extraTeams != null)
                {
                    for (int i = 0; i < Teams.core.extraTeams.Count; i++)
                    {
                        Team t = Teams.core.extraTeams[i];
                        if (t.customHatPath != null)
                        {
                            for (int x = 0; x < Team.hatSearchPaths.Count; x++)
                            {
                                if (t.customHatPath.Contains(Team.hatSearchPaths[x]))
                                {
                                    Teams.core.extraTeams.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }

                    List<string> files = new List<string>();
                    for (int i = 0; i < Team.hatSearchPaths.Count; i++)
                    {
                        files.AddRange(DuckFile.ReGetFiles(Team.hatSearchPaths[i], "*.png"));
                        files.AddRange(DuckFile.ReGetFiles(Team.hatSearchPaths[i], "*.hat"));
                    }

                    Dictionary<string, Team> tths = new Dictionary<string, Team>();
                    for (int i = 0; i < files.Count; i++)
                    {
                        Team team = Team.Deserialize(files[i]);
                        if (team != null)
                        {
                            tths.Add(files[i], team);
                            Teams.core.extraTeams.Add(team);
                        }
                    }

                    IEnumerable<TeamHat> ths = Level.current.things[typeof(TeamHat)].Cast<TeamHat>();
                    foreach (TeamHat th in ths)
                    {
                        //might be a bit unoptimal to do this but im going with it anyways -NiK0
                        if (files.Contains(th.team.customHatPath))
                        {
                            th.team = tths[th.team.customHatPath];
                        }
                    }

                    DGRSettings.InitializeFavoritedHats();
                }
                /*
                List<string> rel = new List<string>();
                for (int i = 0; i < Teams.all.Count; i++)
                {
                    Team t = Teams.all[i];
                    if (t.defaultTeam)
                    {
                        if (DGRSettings.favoriteHats.Contains("D" + t.name))
                        {
                            t.favorited = true;
                            rel.Add("D" + t.name);
                        }
                    }
                    else
                    {
                        if (DGRSettings.favoriteHats.Contains("C" + t.name))
                        {
                            t.favorited = true;
                            rel.Add("C" + t.name);
                        }
                    }
                }
                //If any hats have been renamed or deleted they get deleted from the list
                DGRSettings.favoriteHats = rel;
                */
                SFX.Play("consoleSelect");
            }
            catch
            {
                SFX.Play("consoleError");
            }
        }

        public static UIMenu CreateDGRHudMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|HUD|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("GUI Name Display", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.QOLScoreThingButWithoutScore)))
            {
                dgrDescription = "Displays every participating duck in the game's name and color above"
            });

            menu.Add(new UIMenuItemToggle("Name Tags", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.NameTags)))
            {
                dgrDescription = "Before the round starts or when you're dead/spectating, display the name of every duck above their heads"
            });

            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Lobby Name", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.LobbyNameOnPause)))
            {
                dgrDescription = "Displays lobby name on pause screen (not supporting LAN lobbies)"
            });

            menu.Add(new UIMenuItemToggle("Menu Mouse", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.MenuMouse)))
            {
                dgrDescription = "Toggles the menu mouse"
            });
            menu.Add(new UIMenuItemToggle("Dubber Speed", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.dubberspeed)))
            {
                dgrDescription = "For true vim users, adds keybinds from 1-9 for faster menu browsing\nHold SHIFT to unignore player names"
            });

            menu.Add(new UIMenuItemToggle("No Force Start Menu", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.HideFS)))
            {
                dgrDescription = "Disables the force start button from the pause menu while hosting."
            });
            menu.Add(new UIMenuItemToggle("Reduced Movement", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.ReducedMovement)))
            {
                dgrDescription = "If on, menu animations will be skipped."
            });

            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }

        public static UIMenu CreateDGROptimMenu(UIMenu pPrev)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|OPTIMIZATIONS|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");

            menu.Add(new UIDGRDescribe(Colors.DGPink) { scale = new Vec2(0.5f) }, true);
            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Graphics Culling", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.GraphicsCulling)))
            {
                dgrDescription = "If on, anything outside the camera wont render"
            });
            menu.Add(new UIMenuItemToggle("Use sprite atlas", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SpriteAtlas)))
            {
                dgrDescription = "Lowers render times using an atlas so buffer doesn't constantly switch sprites\n(Requires restart)"
            });
            menu.Add(new UIMenuItemToggle("Single Load Line", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SingleLoadLine)))
            {
                dgrDescription = "Whether or not to render a single line of load progress at startup, in low end systems this might help"
            });


            menu.Add(new UIText(" ", Colors.DGPink) { scale = new Vec2(0.5f) });

            menu.Add(new UIMenuItemToggle("Pre-load levels", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.PreloadLevels)))
            {
                dgrDescription = "Loads custom levels on startup instead of when the level folder is opened\n(Will increase load times)"
            });

            menu.Add(new UIMenuItemToggle("Sort levels", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.SortLevels)))
            {
                dgrDescription = "Whether or not to sort levels on the level selector (If off it will decrease level load time)"
            });

            menu.Add(new UIMenuItemToggle("Load music", field: new FieldBinding(typeof(DGRSettings), nameof(DGRSettings.LoadMusic)))
            {
                dgrDescription = "If this is disabled music wont load resulting in faster load times\n(Requires restart)"
            });

            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pPrev), backButton: true));
            return menu;
        }
        public static UIMenu TEMPDGROPTIM;
        public static UIMenu TEMPDGRGRAPHICS;
        public static UIMenu TEMPDGRQOL;
        public static UIMenu TEMPDGRHUD;
        public static UIMenu TEMPDGREDITOR;
        public static UIMenu TEMPDGRRECORDERATOR;
        public static UIMenu TEMPDGRMISC;
        public static UIMenu TEMPDGRDEV;
        public static UIMenu CreateDGRMenu(UIMenu pOptionsMenu)
        {
            UIMenu menu = new UIMenu("|PINK|♥|WHITE|REBUILT|PINK|♥", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@CANCEL@BACK @SELECT@SELECT");

            TEMPDGROPTIM = CreateDGROptimMenu(menu);
            menu.Add(new UIMenuItem("OPTIMIZATIONS", new UIMenuActionOpenMenu(menu, TEMPDGROPTIM), backButton: true));

            TEMPDGRGRAPHICS = CreateDGRGraphicsMenu(menu);
            menu.Add(new UIMenuItem("GRAPHICS", new UIMenuActionOpenMenu(menu, TEMPDGRGRAPHICS), backButton: true));

            TEMPDGRQOL = CreateDGRQOLMenu(menu);
            menu.Add(new UIMenuItem("QOL", new UIMenuActionOpenMenu(menu, TEMPDGRQOL), backButton: true));

            TEMPDGRHUD = CreateDGRHudMenu(menu);
            menu.Add(new UIMenuItem("HUD", new UIMenuActionOpenMenu(menu, TEMPDGRHUD), backButton: true));

            TEMPDGREDITOR = CreateDGREditorMenu(menu);
            menu.Add(new UIMenuItem("EDITOR", new UIMenuActionOpenMenu(menu, TEMPDGREDITOR), backButton: true));

            TEMPDGRRECORDERATOR = Recorderator.CreateRecorderatorMenu(menu);
            menu.Add(new UIMenuItem("RECORDERATOR", new UIMenuActionOpenMenu(menu, TEMPDGRRECORDERATOR), backButton: true));

            TEMPDGRMISC = CreateDGRDumbShitMenu(menu);
            menu.Add(new UIMenuItem("MISCELLANEOUS", new UIMenuActionOpenMenu(menu, TEMPDGRMISC), backButton: true));

            TEMPDGRDEV = CreateDGRDeveloperMenu(menu);
            if (Program.IS_DEV_BUILD)
            {
                menu.Add(new UIText(" ", Color.White));
                menu.Add(new UIMenuItem("|PINK|DEVELOPER", new UIMenuActionOpenMenu(menu, TEMPDGRDEV), backButton: true));
            }

            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pOptionsMenu), backButton: true));
            return menu;
        }

        public static UIMenu CreateControlsMenu(UIMenu pOptionsMenu)
        {
            UIMenu menu = new UIControlConfig(pOptionsMenu, "@WRENCH@DEVICE DEFAULTS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 194f, conString: "@WASD@@SELECT@ADJUST @CANCEL@BACK");
            return menu;
        }

        public static UIMenu CreateGraphicsMenu(UIMenu pOptionsMenu)
        {
            UIMenu menu = new UIMenu("@WRENCH@GRAPHICS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@CANCEL@BACK @SELECT@SELECT");
            menu.Add(new UIMenuItemToggle("Fullscreen", new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(FullscreenChanged)), new FieldBinding(Data, "fullscreen")));
            menu.Add(new UIMenuItemResolution("Resolution", new FieldBinding(LocalData, "currentResolution", max: 0f))
            {
                selectAction = new Action(ApplyResolution)
            });
            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItemToggle("Windowed Fullscreen", new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(WindowedFullscreenChanged)), new FieldBinding(Data, "windowedFullscreen")));
            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItemToggle("Fire Glow", field: new FieldBinding(Data, "fireGlow")));
            menu.Add(new UIMenuItemToggle("Lighting", field: new FieldBinding(Data, "lighting")));
            menu.Add(new UIMenuItemToggle("Backfill Fix", field: new FieldBinding(Data, "fillBackground")));
            menu.Add(new UIMenuItemToggle("Explosion Flashes", field: new FieldBinding(Data, "flashing")));
            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItemNumber("Console Width", field: new FieldBinding(Data, "consoleWidth", 25f, 100f), step: 10));
            menu.Add(new UIMenuItemNumber("Console Height", field: new FieldBinding(Data, "consoleHeight", 10f, 100f), step: 10));
            menu.Add(new UIMenuItemNumber("Console Scale", field: new FieldBinding(Data, "consoleScale", max: 4f), valStrings: new List<string>()
              {
                "Tiny",
                "Regular",
                "Large",
                "Gigantic",
                "WUMBO"
              }));
            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pOptionsMenu), backButton: true));
            return menu;
        }

        public static UIMenu CreateAudioMenu(UIMenu pOptionsMenu)
        {
            UIMenu menu = new UIMenu("@WRENCH@Audio@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@CANCEL@BACK @SELECT@SELECT");
            menu.Add(new UIText("Exclusive Mode can reduce", Colors.DGBlue));
            menu.Add(new UIText("audio latency, but will", Colors.DGBlue));
            menu.Add(new UIText("stop all other programs from", Colors.DGBlue));
            menu.Add(new UIText("making sound while Duck Game", Colors.DGBlue));
            menu.Add(new UIText("is running!", Colors.DGBlue));
            menu.Add(new UIText(" ", Colors.DGBlue));
            menu.Add(new UIMenuItemToggle("Exclusive Mode", new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(ExclusiveAudioModeChanged)), new FieldBinding(Data, "audioExclusiveMode")));
            menu.Add(new UIMenuItemNumber("Audio Engine", new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(AudioEngineChanged)), new FieldBinding(Data, "audioMode", 1f, 3f), valStrings: new List<string>()
          {
            "None",
            "WaveOut",
            "Wasapi",
            "DirectSound"
          }));
            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItemToggle("Mute If In The Background", new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(MuteOnBackground)), new FieldBinding(Data, "muteOnBackground")));
            menu.Add(new UIText(" ", Color.White));
            menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(menu, pOptionsMenu), backButton: true));
            return menu;
        }

        public static int selectedFont
        {
            get
            {
                string chatFont = Data.chatFont;
                int num = chatFonts.IndexOf(chatFont);
                if (chatFont == "")
                    num = 0;
                return num == -1 ? chatFonts.Count - 1 : num;
            }
            set
            {
                if (value < 0)
                    value = 0;
                if (value >= chatFonts.Count - 1)
                    value = chatFonts.Count - 2;
                Data.chatFont = value != 0 ? chatFonts[value] : "";
                DuckNetwork.UpdateFont();
            }
        }

        public static int fontSize
        {
            get => Data.chatFontSize;
            set
            {
                if (value < 12)
                    value = 12;
                if (value >= 80)
                    value = 80;
                Data.chatFontSize = value;
                DuckNetwork.UpdateFont();
            }
        }

        private static void CloseMoreMenu() => DuckNetwork.UpdateFont();

        public static int languageFilter
        {
            get => !Data.languageFilter ? 0 : 1;
            set => Data.languageFilter = value == 1;
        }

        public static int mojiFilter
        {
            get => Data.mojiFilter;
            set => Data.mojiFilter = value;
        }

        public static int hatFilter
        {
            get => Data.hatFilter;
            set => Data.hatFilter = value;
        }

        public static UIMenu CreateBlockMenu(UIMenu pAccessibilityMenu)
        {
            try
            {
                return new UIBlockManagement(pAccessibilityMenu);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UIMenu CreateTTSMenu(UIMenu pAccessibilityMenu)
        {
            try
            {
                UIMenu menu = new UIMenu("TTS SETTINGS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@SELECT@SELECT");
                menu.Add(new UIMenuItemToggle("Text To Speech", field: new FieldBinding(Data, "textToSpeech")));
                List<string> sayVoices = SFX.GetSayVoices();
                List<string> valStrings = new List<string>();
                foreach (string str1 in sayVoices)
                {
                    string str2 = str1.Replace("Microsoft ", "").Replace(" Desktop", "");
                    if (str2.Length > 10)
                        str2 = str2.Substring(0, 8) + "..";
                    valStrings.Add(str2);
                }
                menu.Add(new UIMenuItemNumber("TTS Voice", field: new FieldBinding(Data, "textToSpeechVoice", max: sayVoices.Count), valStrings: valStrings));
                menu.Add(new UIMenuItemSlider("TTS Volume", field: new FieldBinding(Data, "textToSpeechVolume"), step: 0.06666667f));
                menu.Add(new UIMenuItemSlider("TTS Speed", field: new FieldBinding(Data, "textToSpeechRate"), step: 0.04739336f));
                menu.Add(new UIMenuItemToggle("TTS Read Names", field: new FieldBinding(Data, "textToSpeechReadNames")));
                menu.Add(new UIText(" ", Color.White));
                menu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenuCallFunction(menu, pAccessibilityMenu, new UIMenuActionOpenMenuCallFunction.Function(CloseMoreMenu))));
                menu.SetBackFunction(new UIMenuActionOpenMenuCallFunction(menu, pAccessibilityMenu, new UIMenuActionOpenMenuCallFunction.Function(CloseMoreMenu)));
                return menu;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UIMenu CreateAccessibilityMenu(UIMenu pOptionsMenu)
        {
            try
            {
                UIMenu accessibilityMenu = new UIMenu("USABILITY", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@SELECT@SELECT");
                accessibilityMenu.Add(new UIMenuItemToggle("IME Support", field: new FieldBinding(Data, "imeSupport")), true);
                accessibilityMenu.Add(new UIText(" ", Color.White), true);
                accessibilityMenu.Add(new UIText("Chat Settings", Color.White), true);
                accessibilityMenu.Add(new UIMenuItemNumber("Custom MOJIs", field: new FieldBinding(typeof(Options), "mojiFilter", 0f, 2f, 0.1f), valStrings: new List<string>()
                {
                  "|DGGREENN|@languageFilterOn@DISABLED",
                  "|DGYELLO|@languageFilterOn@FRIENDS ",
                  "|DGREDDD| @languageFilterOff@ENABLED"
                }), true);
                accessibilityMenu.Add(new UIMenuItemNumber("Custom Hats", field: new FieldBinding(typeof(Options), "hatFilter", 0f, 2f, 0.1f), valStrings: new List<string>()
                {
                  "|DGGREEN|   ENABLED",
                  "|DGYELLO|   FRIENDS",
                  "|DGRED|  DISABLED  "
                }), true);
                tempBlockMenu = CreateBlockMenu(accessibilityMenu);
                accessibilityMenu.Add(new UIMenuItem("Manage Block List", new UIMenuActionOpenMenu(accessibilityMenu, tempBlockMenu)), true);
                accessibilityMenu.Add(new UIText(" ", Color.White), true);
                accessibilityMenu.Add(new UIMenuItemNumber("Chat Font", field: new FieldBinding(typeof(Options), "selectedFont", 0f, 6f, 0.1f), valStrings: chatFonts), true);
                accessibilityMenu.Add(new UIMenuItemNumber("Chat Font Size", field: new FieldBinding(typeof(Options), "fontSize", 12f, 30f, 0.1f)), true);
                accessibilityMenu.Add(new UIMenuItemNumber("Chat Head Size", field: new FieldBinding(Data, "chatHeadScale"), valStrings: new List<string>()
        {
          "Regular",
          "Large"
        }), true);
                accessibilityMenu.Add(new UIMenuItemNumber("Chat Opacity", field: new FieldBinding(Data, "chatOpacity", 20f, 100f), step: 10), true);
                if (SFX.hasTTS)
                {
                    tempTTSMenu = CreateTTSMenu(accessibilityMenu);
                    accessibilityMenu.Add(new UIText(" ", Color.White), true);
                    accessibilityMenu.Add(new UIMenuItem("Text To Speech", new UIMenuActionOpenMenu(accessibilityMenu, tempTTSMenu), backButton: true), true);
                }
                else
                {
                    accessibilityMenu.Add(new UIText(" ", Color.White), true);
                    accessibilityMenu.Add(new UIText("|DGRED|Text To Speech Not Installed...", Color.White), true);
                }
                accessibilityMenu.Add(new UIText(" ", Color.White), true);
                accessibilityMenu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenuCallFunction(accessibilityMenu, pOptionsMenu, new UIMenuActionOpenMenuCallFunction.Function(CloseMoreMenu))), true);
                accessibilityMenu.SetBackFunction(new UIMenuActionOpenMenuCallFunction(accessibilityMenu, pOptionsMenu, new UIMenuActionOpenMenuCallFunction.Function(CloseMoreMenu)));
                return accessibilityMenu;
            }
            catch (Exception ex)
            {
                DevConsole.LogComplexMessage("Error creating accessibility menu: " + ex.StackTrace.ToString(), Colors.DGRed);
                return null;
            }
        }

        public static void MergeDefaultPreferDefault() => MergeDefault(true);

        public static void MergeDefaultPreferAccount() => MergeDefault(false);

        public static void CancelResolutionChange() => LocalData.currentResolution = Resolution.lastApplied;

        public static void RestartAndApplyResolution()
        {
            _doingResolutionRestart = true;
            ApplyResolution();
            Save();
            SaveLocalData();
            ModLoader.RestartGame();
        }

        public static void MergeDefault(bool pPreferDefault, bool pShowDialog = true)
        {
            if (Profiles.experienceProfile != null)
            {
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Options.MergeDefault()");
                Profile experienceProfile = Profiles.experienceProfile;
                Profile p = Profiles.alllist[0];
                Profiles.Save(experienceProfile, "__backup_");
                Profiles.Save(p, "__backup_");
                experienceProfile.numSandwiches += p.numSandwiches;
                experienceProfile.milkFill += p.milkFill;
                experienceProfile.littleManLevel += p.littleManLevel - 1;
                experienceProfile.numLittleMen += p.numLittleMen;
                experienceProfile.littleManBucks += p.littleManBucks;
                experienceProfile.timesMetVincent += p.timesMetVincent;
                experienceProfile.timesMetVincentSale += p.timesMetVincentSale;
                experienceProfile.timesMetVincentSell += p.timesMetVincentSell;
                experienceProfile.timesMetVincentImport += p.timesMetVincentImport;
                experienceProfile.xp += p.xp;
                foreach (KeyValuePair<int, int> furniture in p._furnitures)
                {
                    if (!experienceProfile._furnitures.ContainsKey(furniture.Key))
                        experienceProfile._furnitures[furniture.Key] = furniture.Value;
                    else
                        experienceProfile._furnitures[furniture.Key] += furniture.Value;
                }
                experienceProfile.stats = (ProfileStats)(experienceProfile.stats + p.stats);
                foreach (string unlock in p.unlocks)
                {
                    if (!experienceProfile.unlocks.Contains(unlock))
                        experienceProfile.unlocks.Add(unlock);
                }
                foreach (KeyValuePair<string, ChallengeData> challenge in Challenges.challenges)
                {
                    ChallengeSaveData saveData1 = p.GetSaveData(challenge.Key);
                    ChallengeSaveData saveData2 = experienceProfile.GetSaveData(challenge.Key);
                    if (saveData2.trophy == TrophyType.Baseline && saveData1.trophy != TrophyType.Baseline || saveData1.trophy != 0 & pPreferDefault)
                    {
                        saveData2.Deserialize(saveData1.Serialize());
                        saveData2.profileID = experienceProfile.id;
                        experienceProfile.challengeData[saveData2.challenge] = saveData2;
                    }
                }
                experienceProfile.ticketCount = Challenges.GetTicketCount(experienceProfile);
                Profiles.Save(experienceProfile);
            }
            if (pShowDialog)
            {
                UIMenu menu = new UIMenu("@WRENCH@MERGE COMPLETE@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 260f, conString: "@CANCEL@BACK @SELECT@SELECT");
                menu.Add(new UIText("Successfully merged profiles!", Colors.DGBlue));
                menu.Add(new UIMenuItem("FINALLY!!", new UIMenuActionCloseMenu(menu)));
                menu.SetBackFunction(new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(OptionsMenuClosed)));
                menu.Close();
                Level.Add(menu);
                MonoMain.pauseMenu = menu;
                menu.Open();
            }
            Data.defaultAccountMerged = true;
            Save();
        }

        public static UIMenu CreateProfileMergeMenu()
        {
            UIMenu menu = new UIMenu("@WRENCH@MERGE PROFILES@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 270f, conString: "@CANCEL@BACK @SELECT@SELECT");
            menu.Add(new UIText("Looks like you have a 'DEFAULT'", Colors.DGBlue));
            menu.Add(new UIText("profile. These are now obsolete.", Colors.DGBlue));
            menu.Add(new UIText("", Colors.DGBlue));
            menu.Add(new UIText("Would you like to merge all", Colors.DGBlue));
            menu.Add(new UIText("data from the 'DEFAULT' profile", Colors.DGBlue));
            menu.Add(new UIText("into this one?", Colors.DGBlue));
            menu.Add(new UIText("", Colors.DGBlue));
            menu.Add(new UIMenuItem("NO!", new UIMenuActionCloseMenu(menu)));
            menu.Add(new UIMenuItem("YES! (PREFER DEFAULT)", new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(MergeDefaultPreferDefault))));
            menu.Add(new UIMenuItem("YES! (PREFER THIS ACCOUNT)", new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(MergeDefaultPreferAccount))));
            menu.SetBackFunction(new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(OptionsMenuClosed)));
            menu.Close();
            return menu;
        }

        public static UIMenu CreateResolutionApplyMenu()
        {
            UIMenu menu = new UIMenu("@WRENCH@NEW ASPECT RATIO@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 270f, conString: "@CANCEL@BACK @SELECT@SELECT");
            menu.Add(new UIText("To apply a resolution", Colors.DGBlue));
            menu.Add(new UIText("with a different aspect ratio,", Colors.DGBlue));
            menu.Add(new UIText("The game must be restarted.", Colors.DGBlue));
            menu.Add(new UIText("", Colors.DGBlue));
            menu.Add(new UIText("Would you like to restart", Colors.DGBlue));
            menu.Add(new UIText("and apply changes?", Colors.DGBlue));
            menu.Add(new UIText("", Colors.DGBlue));
            menu.Add(new UIMenuItem("NO!", new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(CancelResolutionChange))));
            menu.Add(new UIMenuItem("YES! (Restart)", new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(RestartAndApplyResolution))));
            menu.SetBackFunction(new UIMenuActionCloseMenuCallFunction(menu, new UIMenuActionCloseMenuCallFunction.Function(OptionsMenuClosed)));
            menu.Close();
            return menu;
        }

        public static void OpenOptionsMenu()
        {
            _removedOptionsMenu = false;
            _openedOptionsMenu = true;
            Level.Add(_optionsMenu);
            _optionsMenu.Open();
        }

        public static void OptionsMenuClosed()
        {
            Save();
            SaveLocalData();
            if (openOnClose == null)
                return;
            openOnClose.Open();
        }

        public static string optionsFileName => DuckFile.optionsDirectory + "/options.dat";

        private static string optionsFileLocalName => DuckFile.optionsDirectory + "/localsettings.dat";

        public static void Save()
        {
            if (!loadCalled)
            {
                if (!MonoMain.logFileOperations)
                    return;
                DevConsole.Log(DCSection.General, "Options.Save() skipped (loadCalled = false)");
            }
            else
            {
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Options.Save()");
                DuckXML doc = new DuckXML();
                DXMLNode node = new DXMLNode("Data");
                node.Add(_data.Serialize());
                doc.Add(node);
                DuckFile.SaveDuckXML(doc, optionsFileName);
            }
        }

        public static void SaveLocalData()
        {
            if (!loadCalled)
            {
                if (!MonoMain.logFileOperations)
                    return;
                DevConsole.Log(DCSection.General, "Options.SaveLocalData() skipped (loadCalled = false)");
            }
            else
            {
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Options.SaveLocalData()");
                DuckXML doc = new DuckXML();
                DXMLNode node = new DXMLNode("Data");
                node.Add(_localData.Serialize());
                doc.Add(node);
                DuckFile.SaveDuckXML(doc, optionsFileLocalName);
            }
        }

        public static void Load()
        {
            if (MonoMain.logFileOperations)
                DevConsole.Log(DCSection.General, "Options.Load()");
            loadCalled = true;
            DuckXML duckXml1 = DuckFile.LoadDuckXML(optionsFileName);
            if (duckXml1 != null)
            {
                Profile profile = new Profile("");
                IEnumerable<DXMLNode> source = duckXml1.Elements("Data") ?? duckXml1.Elements("OptionsData");
                if (source != null)
                {
                    foreach (DXMLNode element in source.Elements())
                    {
                        if (element.Name == nameof(Options))
                        {
                            _data.Deserialize(element);
                            break;
                        }
                    }
                }
            }
            DuckXML duckXml2 = DuckFile.LoadDuckXML(optionsFileLocalName);
            if (duckXml2 == null)
                return;
            Profile profile1 = new Profile("");
            IEnumerable<DXMLNode> source1 = duckXml2.Elements("Data");
            if (source1 == null)
                return;
            foreach (DXMLNode element in source1.Elements())
            {
                if (element.Name == nameof(Options))
                {
                    _localData.Deserialize(element);
                    break;
                }
            }
        }

        public static void PostLoad()
        {
            if (Data.musicVolume > 1)
                Data.musicVolume /= 100f;
            if (Data.sfxVolume > 1)
                Data.sfxVolume /= 100f;
            if (Data.windowScale < 0)
                Data.windowScale = !MonoMain.fourK ? 0 : 1;
            Data.consoleWidth = Math.Min(100, Math.Max(Data.consoleWidth, 25));
            Data.consoleHeight = Math.Min(100, Math.Max(Data.consoleHeight, 10));
            Data.consoleScale = Math.Min(5, Math.Max(Data.consoleScale, 1));
            if (Data.currentSaveVersion != -1)
            {
                if (Data.currentSaveVersion < 2)
                    Data.consoleScale = 1;
                if (Data.currentSaveVersion < 3)
                    Data.windowedFullscreen = true;
            }
            if (Data.currentSaveVersion < 4 || Data.currentSaveVersion == -1)
                DGSave.showOnePointFiveMessages = true;
            if (Data.currentSaveVersion < 5)
            {
                if (Data.keyboard1PlayerIndex > 0)
                {
                    legacyPreferredColor = Data.keyboard1PlayerIndex;
                    Data.keyboard1PlayerIndex = 0;
                }
                Data.windowedFullscreen = true;
            }
            if (Data.audioMode == 0 || Data.audioMode >= 4)
                Data.audioMode = 2;
            Data.UpdateCurrentVersion();
            if (LocalData.previousAdapterResolution == null || Resolution.adapterResolution != LocalData.previousAdapterResolution)
            {
                Resolution.RestoreDefaults();
                LocalData.previousAdapterResolution = Resolution.adapterResolution;
            }
            if (LocalData.windowedResolution.mode != ScreenMode.Windowed)
                LocalData.windowedResolution = Resolution.FindNearest(ScreenMode.Windowed, LocalData.windowedResolution.x, LocalData.windowedResolution.y);
            if (LocalData.fullscreenResolution.mode != ScreenMode.Fullscreen)
                LocalData.fullscreenResolution = Resolution.FindNearest(ScreenMode.Fullscreen, LocalData.fullscreenResolution.x, LocalData.fullscreenResolution.y);
            if (LocalData.windowedFullscreenResolution.mode != ScreenMode.Borderless)
                LocalData.windowedFullscreenResolution = Resolution.FindNearest(ScreenMode.Borderless, LocalData.windowedFullscreenResolution.x, LocalData.windowedFullscreenResolution.y);
            LocalData.currentResolution = !Data.fullscreen ? LocalData.windowedResolution : (Data.windowedFullscreen ? LocalData.windowedFullscreenResolution : LocalData.fullscreenResolution);
            if (MonoMain.oldAngles)
                Data.oldAngleCode = true;
            if (!MonoMain.defaultControls)
                return;
            Data.keyboard1PlayerIndex = 0;
            Data.keyboard2PlayerIndex = 1;
        }

        public static void Update()
        {
            if (!Data.muteOnBackground || MonoMain.instance.IsFocused)
            {
                Music.masterVolume = Math.Min(1f, Math.Max(0f, Data.musicVolume));
                SFX.volume = Math.Min(1f, Math.Max(0f, Data.sfxVolume));
            }
            else if (Data.muteOnBackground && !MonoMain.instance.IsFocused)
            {
                Music.masterVolume = 0f;
                SFX.volume = 0f;
            }
            if (_openedOptionsMenu && !_removedOptionsMenu && _optionsMenu != null && !_optionsMenu.open && !_optionsMenu.animating)
            {
                _openedOptionsMenu = false;
                _removedOptionsMenu = true;
                Level.Remove(_optionsMenu);
            }
            if (_resolutionChanged)
            {
                _resolutionChanged = false;
                ResolutionChanged();
            }
            if (flagForSave <= 0)
                return;
            --flagForSave;
            if (flagForSave != 0)
                return;
            Save();
        }

        public static void ResolutionChanged()
        {
            if (!MonoMain.started)
            {
                _resolutionChanged = true;
            }
            else
            {
                FontGDIContext._fontDatas.Clear();
                DuckNetwork.UpdateFont();
                DevConsole.RefreshConsoleFont();
            }
        }
    }
}
