using UnityEngine;

public class DEBUG : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Physics.simulationMode = " + Physics.simulationMode);
        Debug.Log("Time.timeScale = " + Time.timeScale);
    }

    void Update(){
        Debug.Log("Physics.simulationMode = " + Physics.simulationMode);
        Debug.Log("Time.timeScale = " + Time.timeScale);
    }
}
