using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace comdata_activiterDetector
{
    public static class StateManager
    {
        public static bool isVisible = false;
        public static List<TypeItem> types = new List<TypeItem>();
        public static int tactif = 0;
        public static int tpause = 0;
        public static int tinactif = 0;

        private static List<StateObservable> observables = new List<StateObservable>();

        public static void addObservable(StateObservable observable)
        {
            observables.Add(observable);
        }

        
        public static void changeVisibility(bool visibility)
        {
            isVisible = visibility;
            notifyVisibility();
        }
        public static void changeTime(int tactif, int tpause, int tinactif)
        {
            StateManager.tactif = tactif;
            StateManager.tpause = tpause;
            StateManager.tinactif = tinactif;
            notifyTime();
        }

        private static void notifyVisibility()
        {
            foreach (StateObservable observable in observables)
            {
                if(observable != null)
                    observable.onVisibilityChange(isVisible);
            }
        }
        private static void notifyTime()
        {
            foreach (StateObservable observable in observables)
            {
                if (observable != null)
                    observable.onTimeChange(tactif, tpause, tinactif);
            }
        }

    }
}
