using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureGenerator2D : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture3D asd;
    void Start()
    {
        Material x = this.GetComponent<MeshRenderer>().materials[0];
        //Debug.Log(x);
        asd = Utils.generate3dTexture(20, 20, 20);
        x.SetTexture("Texture", asd);
        x.mainTexture = asd;
    }

}
