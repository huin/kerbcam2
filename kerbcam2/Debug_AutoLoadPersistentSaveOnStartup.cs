﻿using KSPPluginFramework;
using UnityEngine;

// Original author unknown - taken and modified from:
// http://forum.kerbalspaceprogram.com/entries/1253-An-Adventure-in-Plugin-Coding-3-A-Fast-Dev-Environment-I-hope
#if DEBUG
namespace kerbcam2 {
    // This will kick us into the save called default and set the first vessel active
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class Debug_AutoLoadPersistentSaveOnStartup : MonoBehaviour {
        public void Start() {
            MonoBehaviourExtended.LogFormatted_DebugOnly("Start");
            HighLogic.SaveFolder = "default";
            Game game = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, true, false);

            if (game != null && game.flightState != null && game.compatible) {
                MonoBehaviourExtended.LogFormatted_DebugOnly("game found and loaded");
                for (int i = 0; i < game.flightState.protoVessels.Count; i++) {
                    var vesselType = game.flightState.protoVessels[i].vesselType;
                    MonoBehaviourExtended.LogFormatted_DebugOnly("vessel found {0}", vesselType);
                    //This logic finds the first non-asteroid vessel
                    if (vesselType != VesselType.SpaceObject &&
                        vesselType != VesselType.Unknown) {
                        MonoBehaviourExtended.LogFormatted_DebugOnly("suitable vessel found");
                        FlightDriver.StartAndFocusVessel(game, i);
                        MonoBehaviourExtended.LogFormatted_DebugOnly("started with vessel");
                        break;
                    }
                }
            }

            //CheatOptions.InfiniteFuel = true;
        }
    }
}
#endif