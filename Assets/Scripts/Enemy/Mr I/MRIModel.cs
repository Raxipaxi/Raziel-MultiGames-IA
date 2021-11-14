using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIModel : MonoBehaviour
{

    private LineOfSightAI _lineOfSight;
    private MRIView _view;
    public LineOfSightAI LineOfSightAI => _lineOfSight;

    public void BakeReferences()
    {
        _view = GetComponent<MRIView>();
    }

    public void SubscribeToEvents(MRIController controller)
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
