using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun
{
    /*
    * adds recoil before shooting the gun 
    */
    public override void ShootWithRecoil()
    {
        base.ShootWithRecoil();
        Debug.Log("Pistol: add recoil -- Need to be Implemented");
    }
}
