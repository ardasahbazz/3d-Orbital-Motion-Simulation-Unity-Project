using UnityEngine;
using UnityEngine.UI;
using solsyssim;
using TMPro;

public class InfosDisplay : MonoBehaviour
{
    // Displays information about the currently focused celestial body in the UI.
    // UI references
    public TextMeshProUGUI speedText;

    //public TextMeshProUGUI massText;

    public TextMeshProUGUI radiusText;
    public TextMeshProUGUI focusText;
    public TextMeshProUGUI aphelionText;
    public TextMeshProUGUI perihelionText;
    public TextMeshProUGUI meanAnomalyText;
    public TextMeshProUGUI inclinationText;
    public TextMeshProUGUI eccentricityText;
    public TextMeshProUGUI semiMajorAxisText;
    public TextMeshProUGUI angularVelocityText;

    private CamControl camControl;
    private void Start()
    {
        // Find the CamControl component in the scene
        camControl = FindObjectOfType<CamControl>();
        if (camControl != null)
        {
            
            // Subscribe to the NewFocus event
            camControl.NewFocus += OnNewBodyFocused;
        }
    }

    private void Update()
    {
        
        if (camControl != null && camControl._selectedBody != null)
        {
            OnNewBodyFocused(camControl._selectedBody);
            float r = camControl._selectedBody.parent.GetComponent<OrbitalBody>().radius();
        }
        Debug.Log("True Anomaly:" + camControl._selectedBody.parent.GetComponent<OrbitalBody>().v);
    }
    private void OnDestroy()
    {
        // Unsubscribe from the NewFocus event
        if (camControl != null)
        {
            camControl.NewFocus -= OnNewBodyFocused;
        }
    }

    private void OnNewBodyFocused(Transform focusedBody)
    {
        // Get the OrbitalBody component from the focused body
        Debug.Log("New body focused: " + focusedBody.name);
        OrbitalBody orbitalBody = camControl._selectedBody.parent.GetComponent<OrbitalBody>();
        if (orbitalBody != null)
        {
            
            // Update UI elements with the parameters from the OrbitalBody
            focusText.text = orbitalBody.name;
            //speedText.text = "Speed: " + orbitalBody.GetSpeed().ToString("F2") + " km/s";
            //massText.text = "Mass: " + orbitalBody.mass.ToString("F2") + " kg";
            radiusText.text = "Radius: " + orbitalBody.r.ToString("G5") + " km";
            aphelionText.text = "Arg Ascending: " + orbitalBody._argAscending + " °";
            perihelionText.text = "Perihelion: " + orbitalBody._argPerihelion.ToString("F2") + " km";
            meanAnomalyText.text = "Mean Anomaly:" + orbitalBody._meanAnomaly.ToString("G5");
            Debug.Log("Updating inclination text to: " + orbitalBody._inclination.ToString("F2"));
            inclinationText.text = "Inclination: " + orbitalBody._inclination + " °";
            semiMajorAxisText.text = "Semi Major Axis: " + orbitalBody._semiMajorAxis + "000 km";
            angularVelocityText.text = "Angular Velocity:" + ((orbitalBody._angularVelocity)/(24*60*60)).ToString("G5") + "rad/s";
        }
        else
        {
            Debug.LogError("OrbitalBody component not found on focused object.");
        }
            
        

    }
}