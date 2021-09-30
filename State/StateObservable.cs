using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace comdata_activiterDetector
{
    public interface StateObservable
    {
        void onVisibilityChange(bool isVisible);
        void onTimeChange(int tactif, int tpause, int tinactif);
    }
}
