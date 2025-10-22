using System;
using UnityEngine;

namespace solsyssim {


    // Class to manage difference UI element and send signal.

    
    public class InterfaceManager : MonoBehaviour {
        public event Action FullStart;

        // These two will be used with potential UI inprovement.
        public event Action IconToggle;
        public event Action OrbitToggle;

        private bool _launched = false;

        
        // Called through the info display animator to notify other game component the game has started.
        
        public void ExitLauncSequence() {
            if (!_launched) {
                if(FullStart != null)
                    FullStart.Invoke();
                _launched = true;
            }
        }
 
    }
}