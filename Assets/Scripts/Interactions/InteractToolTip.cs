using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractToolTip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI pressKey;
    [SerializeField] private InteractionToolTipScriptableObject data;

    private Color _vertexColor;



    private void Awake()
    {
        pressKey.text = data.charToDisplay;

        this.enabled = false;
        _vertexColor = pressKey.color;
        _vertexColor.a = 0;
        pressKey.color = _vertexColor;
  

    }
    private void Update()
    {
        transform.Rotate(new Vector3(0, data.rotationSpeed * Time.deltaTime, 0));
    }


    private void OnTriggerEnter(Collider other)
    {
        this.enabled = true;
        transform.rotation = Quaternion.identity;
        _vertexColor.a = 1;
        pressKey.color = _vertexColor;
    }

    private void OnTriggerExit(Collider other)
    {
        this.enabled = false;
        _vertexColor.a = 0;
        pressKey.color = _vertexColor;
    }



}
