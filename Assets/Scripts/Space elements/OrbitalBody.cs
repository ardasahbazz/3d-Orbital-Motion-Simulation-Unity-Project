using UnityEngine;
using solsyssim;

namespace solsyssim {

    // Script managing the behaviour of the Orbital axis and it's child orbital body.
    // Sets the initial body parameter.
    // Updates the scales of all parameters if necessary.
    // Updates the orbit rotation and the body's day rotation.

    public class OrbitalBody : MonoBehaviour
    {
        // reference to object in the hierarchy
        [SerializeField] public GameObject _stellarParent;
        [SerializeField] public GameObject _body;
        [SerializeField] private Material _line;
        



        // Variables related to the orbital axis.
        [SerializeField] public float _sideralYear;
        [SerializeField] public float _mass = 0f;
        [SerializeField] public float _semiMajorAxis = 10f;
        [SerializeField] public float _inclination = 0f;
        [SerializeField] [Range(0f, 1f)] public float _eccentricity = 0f;
        [SerializeField] public float _argAscending = 0f; // longitude
        [SerializeField] public float _argPerihelion = 0f; // argument
        [SerializeField] public float _periapsis = 0f;
        [SerializeField] public float _apoapsis = 0f; 
        [SerializeField] public float _startMeanAnomaly = 0f; // J2000
        [SerializeField] public float _meanAnomaly;


        // Variables related to the orbital body.
        [SerializeField] private float _dayLength = 1f;
        [SerializeField] private float _startDayRotation = 0f;
        [SerializeField] private float _size = 1f;
        [SerializeField] public float _rightAscension = 0f;
        [SerializeField] public float _declination = 0f;
        private const float _eclipticTilt = 23.44f;
        public float _angularVelocity;
        public float EccentricAnom;
        public float r;
        public float TA;
        private const float G = 6.674e-11F;
        public float v;

        // Semi Constants
        public float SGP;
        public float MAM;
        public float TAC;
        public float CosLOAN;
        public float SinLOAN;
        public float CosI;
        public float SinI;

        // Scales from the StellarSystem script.
        public float SizeScaled { private set; get; }
        public float OrbitScaled { private set; get; }

        // planet UI color and path variables
        [SerializeField] private Color _uiVisual;
        public Color UiVisual { get { return _uiVisual; } }
        private bool _showPath = false;
        
        //For debugginf reference
        DateCalc dateCalc;
        GameObject gameObject;
        public string name;

        // registering to some events and initialising stuff
        private void Start() {
            
            SpaceTime.Instance.ScaleUpdated += UpdateScale;
            // FindObjectOfType<InterfaceManager>().OrbitToggle += TogglePath;
            // FindObjectOfType<InterfaceManager>().FullStart += TogglePath;

            SetScales();
            SetJ2000();
            CalcSemiConstants();
            AdvanceOrbit();
            
            //Debugging
            
            //OrbitalDebug debug = new OrbitalDebug();
            //debug.name = name;
            //debug.eccentricAnomaly = EccentricAnomaly(_meanAnomaly);  
            //debug.trueAnomaly = TrueAnomaly(EccentricAnomaly(_meanAnomaly));
            
        }

        private void OnDestroy() {
            SpaceTime.Instance.ScaleUpdated -= UpdateScale;
        }

        public void CalcSemiConstants(){
            
            float argrad = _argAscending * Mathf.Deg2Rad;
            float incrad = _inclination * Mathf.Deg2Rad;
            SGP = _stellarParent.gameObject.GetComponent<OrbitalBody>()._mass * G;
            MAM = Mathf.Sqrt(SGP / (float)Mathf.Pow(_semiMajorAxis, 3));
            TAC = Mathf.Sqrt((1 + _eccentricity) / (1 - _eccentricity));

            CosLOAN = Mathf.Cos(argrad);
            SinLOAN = Mathf.Sin(argrad);

            CosI = Mathf.Cos(incrad);
            SinI = Mathf.Sin(incrad);

        }

        // advance the body on it's orbit and it's rotation on itself separately
        private void Update() {
            AdvanceOrbit();
            AdvanceDayRotation();
            radius();

            //For UI
            
            //Debug.Log("Name:"+ name);
            //Debug.Log("Eccentric Anomaly: " + EccentricAnomaly(_meanAnomaly));
            //Debug.Log("True Anomaly: " + TrueAnomaly(EccentricAnomaly(_meanAnomaly)));
        }

        // renders or not the orbit path
        // private void OnRenderObject() {
        //     if (_showPath)
        //         DrawPath();
        // }

  
        // Initialises the orbital body state as per the J2000 data.

        private void SetJ2000() {
            transform.Rotate(new Vector3(0, 0, (90 - _declination))); //Rotates object around its local axis in Euler angles
            transform.Rotate(new Vector3(0, -_rightAscension, 0));
            transform.Rotate(new Vector3(_eclipticTilt, 0, 0));
            transform.Rotate(new Vector3(0, _startDayRotation, 0), Space.Self);
            _angularVelocity = Mathf.Deg2Rad * (360 / _sideralYear); // How much degree is covered each day.
            _meanAnomaly = _startMeanAnomaly * Mathf.Deg2Rad; //Converts to radians
        }



        // Initialises the scales from the StellarSystem.

        private void SetScales() {
            OrbitScaled = _semiMajorAxis * SpaceTime.Instance.OrbitScale;
            SizeScaled = _size * SpaceTime.Instance.BodyScale;
            _body.transform.localScale = Vector3.one * SizeScaled;
        }


        // Toggles the orbit path. Called on events.

        // public void TogglePath() {
        //     _showPath = _showPath;
        // }

        // Updates the scale. Called on event from the StellarSystem script.

        // <param name="variable">Which scale.</param>
        public void UpdateScale(SpaceTime.Scale scale, float value) {
            switch (scale) {

                case SpaceTime.Scale.Body:
                    SizeScaled = _size * value;
                    _body.transform.localScale = Vector3.one * SizeScaled;
                    break;

                case SpaceTime.Scale.Orbit:
                    OrbitScaled = _semiMajorAxis * value;
                    break;
                
                case SpaceTime.Scale.Time:
                    break; 

                default:
                    Debug.LogWarning("Wrong variable name in Body.updateScales() .");
                    break;
            }
        }

      
        // Calculates how much the body has moved in it's orbit and rotate it's axis accordingly.
     
        
        
        //public float CalculateAngulaVelocity() {
        //    float m1 = GameObject.Find("Sun").GetComponent<OrbitalBody>()._mass;
        //    float m2 = _mass;

        //    float sum = _G * (m1+m2);
        //    float _angVel = (radius()*m1*(Mathf.Sqrt(sum*((2/radius())-(1/_semiMajorAxis)))))/(m1*radius()*radius());

        //    return _angVel;

        //}
        
        //Finds radius from True Anomaly
        public float radius() {
            // from wikepedia its says that radius cna be solved with TA, eccentrity, and semi major axis
            // r = a ((1-e^2)/(1+e*cos(TA)))
            float e = _eccentricity;
            float a = _semiMajorAxis;
            EccentricAnom = EccentricAnomaly(_meanAnomaly, _eccentricity);
            float v = TrueAnomaly(EccentricAnom);
            r = (a) *((1-(e*e))/(1+(e*Mathf.Cos(v))));
            return r;
        }

        private void AdvanceOrbit() {
            // Calculate the distance to the sun
            r = radius();
            
            float m1 = _stellarParent.gameObject.GetComponent<OrbitalBody>()._mass;
            float m2 = _mass;
            
            // Calculate angular velocity based on current radius
            _angularVelocity = (Mathf.Sqrt(G * (m1 + m2) / Mathf.Pow(r, 3))) * 0.0001f; //(2f * Mathf.PI / Mathf.Sqrt(Mathf.Pow(r, 3)/G*m1)) ; 
            //Debug.Log("angvel" + _angularVelocity);
            // update mean anomaly
            _meanAnomaly += _angularVelocity * SpaceTime.Instance.DeltatTime;
            if (_meanAnomaly > 2f * Mathf.PI)  // we keep mean anomaly within 2*Pi
                _meanAnomaly -= 2f * Mathf.PI;

            Vector3 orbitPos = GetPosition(_meanAnomaly, "advanceOrbit");
            Vector3 parentPos = _stellarParent.transform.position; // position of the parent to offset the calculated pos (used for moons)
            transform.position = new Vector3(orbitPos.x + parentPos.x, orbitPos.y + parentPos.y, orbitPos.z + parentPos.z);
        }

                
        // Rotates the body on itself as per it's day length.      
        private void AdvanceDayRotation() {
            float rot = SpaceTime.Instance.DeltatTime * (360 / _dayLength);
            _body.transform.Rotate(new Vector3(0, -rot, 0)); // counter clockwise is the standard rotation considered
        }

              
        // Draws the orbit by calculating each next point in a circle.
        // Then using the GL.Lines to draw a line between each point.
             
        
        // private void DrawPath() {
        //         const float PathDetail = 0.03f;
        //         GL.PushMatrix();  // Push the current transformation matrix onto the matrix stack.
        //         _line.color = UiVisual;
        //         _line.SetPass(0);  // Set the material pass to the first file in the ui folder to render the line.

        //         GL.Begin(GL.LINES);  // Begin rendering lines using OpenGL.
        //         Vector3 parentPos = _stellarParent.transform.position;  // Get the position of the parent object.
        //         // Loop through a full circle by the specified path detail and create a line for each step.
        //         for (float theta = 0.0f; theta < (2f * Mathf.PI); theta += PathDetail) {
        //             float anomalyA = theta;
        //             Vector3 orbitPointA = GetPosition(anomalyA);  
        //             GL.Vertex3(orbitPointA.x + parentPos.x, orbitPointA.y + parentPos.y, orbitPointA.z + parentPos.z);

        //             float anomalyB = theta + PathDetail;
        //             Vector3 orbitPointB = GetPosition(anomalyB);  
        //             GL.Vertex3(orbitPointB.x + parentPos.x, orbitPointB.y + parentPos.y, orbitPointB.z + parentPos.z);
        //         }
        //         GL.End();
        //         GL.PopMatrix(); 
        // }




        // Computes the Eccentric Anomaly. Angles to be passed in Radians.
        // The Newton method used to solve E implies getting a first guess and itterating until the value is precise enough.

        //  returns the Eccentric Anomaly.
        private static float EccentricAnomaly(float M, float _eccentricity, int dp = 5) {
            // Mathematical Model is as follow:
            // E(n+1) = E(n) - f(E) / f'(E)
            // f(E) = E - e * sin(E) - M
            // f'(E) = 1 - e * cos(E)
            // we are happy when f(E)/f'(E) is small enough.

            int maxIter = 20;  // we make sure we won't loop too much
            int i = 0;
            
            float precision = Mathf.Pow(10, -dp);
            float E, F;

            // If the eccentricity is high we guess the Mean anomaly for E, otherwise we guess PI.
            E = (_eccentricity < 0.8) ? M : Mathf.PI;
            F = E - _eccentricity * Mathf.Sin(M) - M;  //f(E)

            // We will interate until f(E) higher than our wanted precision (as devided then by f'(E)).
            while ((Mathf.Abs(F) > precision) && (i < maxIter)) {
                E = E - F / (1f - _eccentricity * Mathf.Cos(E));
                F = E - _eccentricity * Mathf.Sin(E) - M;
                i++;
            }

            return E;
        }



        // Computes the True Anomaly. Angles to be passed in Radians.
        private float TrueAnomaly(float E) {
            // from wikipedia we can find several way to solve TA from E.
            // I tried sin(TA) = (sqrt(1-e*e) * sin(E))/(1 -e*cos(E)) but it didn't work properly for some reason,
            // so I sued the following as one of the my sources(jgiesen.de/Kepler) tan(TA) = (sqrt(1-e*e) * sin(E)) / (cos(E) - e).

            float e = _eccentricity; 
            float numerator = Mathf.Sqrt(1f - e * e) * Mathf.Sin(E);
            float denominator = Mathf.Cos(E) - e;
            float TA = Mathf.Atan2(numerator, denominator);
            return TA;
        }





        
        //Compute a point's position in a given orbit. All angles are to be passed in Radians and returns the point coordinates
        public Vector3 GetPosition(float M, string callerName) {
            
            float a = OrbitScaled; // semiMajorAxis
            float N = _argAscending * Mathf.Deg2Rad; // not const as might vary with precession
            float w = _argPerihelion * Mathf.Deg2Rad;
            float i = _inclination * Mathf.Deg2Rad;
 
            float E = EccentricAnomaly(M, _eccentricity);
            float TA = TrueAnomaly(E);
            float focusRadius = a * (1 - Mathf.Pow(_eccentricity, 2f)) / (1 + _eccentricity * Mathf.Cos(TA)); //distance from focus to object
            // Debug.Log($"{callerName} - Eccentric Anomaly: {E}");
            // Debug.Log($"{callerName} - True Anomaly: {TA}");
            // Debug.Log($"{callerName} - Focus Radius: {focusRadius}");
            // parametric equation of an elipse using the orbital elements
            float X = focusRadius * (Mathf.Cos(N) * Mathf.Cos(TA + w) - Mathf.Sin(N) * Mathf.Sin(TA + w)) * Mathf.Cos(i);
            float Y = focusRadius * Mathf.Sin(TA + w) * Mathf.Sin(i);
            float Z = focusRadius * (Mathf.Sin(N) * Mathf.Cos(TA + w) + Mathf.Cos(N) * Mathf.Sin(TA + w)) * Mathf.Cos(i);

            Vector3 orbitPoint = new Vector3(X, Y, Z);

            return orbitPoint;
        }

    
    }
    
}