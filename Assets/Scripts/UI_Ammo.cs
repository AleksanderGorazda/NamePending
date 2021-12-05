using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Ammo : MonoBehaviour
{

    TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmmo(int magazineAmmo, int equipmentAmmo)
    {
        textMesh.SetText(magazineAmmo + " | " + equipmentAmmo);
    }
}
