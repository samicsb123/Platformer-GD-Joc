using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject ObjTrig;
    [SerializeField] string currentSubtitle;
    [SerializeField] TMP_Text subtitle;
    [SerializeField] GameObject Canvas;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        ObjTrig.SetActive(true);
        Canvas.SetActive(true);
        
        
    }
    IEnumerator WaitToFinishDialog()
    {
            subtitle.text = currentSubtitle;

            yield return new WaitForSeconds(10);
            StartCoroutine(WaitToFinishDialog());
            
    }
        private void OnTriggerExit(Collider other){
        
        Canvas.SetActive(false);
        
        
    }
    
    
}
