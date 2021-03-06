﻿namespace TrafficManager.Patch._HumanAI {
    using ColossalFramework;
    using Harmony;
    using JetBrains.Annotations;
    using State;

    [HarmonyPatch(typeof(HumanAI), "ArriveAtDestination")]
    public class ArriveAtDestinationPatch {
        /// <summary>
        /// Notifies the extended citizen manager about a citizen that arrived at their destination if the Parking AI is active.
        /// </summary>
        [HarmonyPrefix]
        [UsedImplicitly]
        public static void Prefix(HumanAI __instance,
                                  ushort instanceID,
                                  ref CitizenInstance citizenData,
                                  bool success) {
            if (!Options.parkingAI) {
                return;
            }

            if (success && citizenData.m_citizen != 0 &&
                (citizenData.m_flags & CitizenInstance.Flags.TargetIsNode) == CitizenInstance.Flags.None)
            {
                Constants.ManagerFactory.ExtCitizenManager.OnArriveAtDestination(
                    citizenData.m_citizen,
                    ref Singleton<CitizenManager>.instance.m_citizens.m_buffer[citizenData.m_citizen],
                    ref citizenData);
            }
        }
    }
}