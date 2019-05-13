using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//acts as a collector for all data types related to a charcter 
public class Charecter : MonoBehaviour
{
    public string name;
    public PhysicalCharectristcs pc;
    public Data Data; // at this point read it from json 
 

    public Charecter()
    {
        pc = new PhysicalCharectristcs();
        Debug.Log(pc.eyeColor);

    }
    // Start is called before the first frame update
    void Start()
    {
        //Data = FindObjectOfType<Data>();
        //pc = Data.returnPhysicalCharectristcs();

        //pc = new PhysicalCharectristcs();


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //public Charecter() {
    //    if (pc != null)
    //    {
    //        pc = Data.returnPhysicalCharectristcs();
    //    }
    //    name = Data.returnAname();
    //}
}
