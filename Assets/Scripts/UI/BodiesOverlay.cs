using UnityEngine;

namespace solsyssim {


    /// This class instantiate a BodyLabel as chidl for each orbital body in the StellarSystem.

    public class BodiesOverlay : MonoBehaviour {
        [SerializeField] private GameObject _stellarSystem;
        [SerializeField] private GameObject _label;

        private void Start() {
            InstantiateLabels();
        }


        /// Instantiates a BodyLabel as child for each orbital body in the StellarSystem.

        private void InstantiateLabels() {
            int i = 0;
            foreach (Transform body in _stellarSystem.transform) {
                if (body.GetComponent<OrbitalBody>() != null) {
                    GameObject newLabel = Instantiate(_label);
                    newLabel.GetComponent<BodyLabel>().Owner = body.gameObject;
                    newLabel.transform.SetParent(transform);
                }

                i++;
            }
        }

    }
}