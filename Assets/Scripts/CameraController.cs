using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.transform.position = new Vector3(StaticDate.Settings.Heigth / 2 + 3, 
            StaticDate.Settings.Weidth / 2, this.transform.position.z);
    }
}
