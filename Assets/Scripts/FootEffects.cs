using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FootEffects : MonoBehaviour
{
    
    [Header("Foot Effect")] 
    public GameObject RightFootDecal;
    public GameObject LeftFootDecal;
    public Transform RightFoot;
    public Transform LeftFoot;
    public GameObject FootParticules;
    public float DecalDepth=-0.9f;

    public void LeftFootEvent()
    {
        RaycastHit hit;
        if (Physics.Raycast(LeftFoot.position + Vector3.up, LeftFoot.position- (LeftFoot.position + Vector3.up),
            out hit)) {
            Transform footSteep = Instantiate(LeftFootDecal).transform;
            footSteep.position = hit.point+(hit.normal.normalized * DecalDepth);
            footSteep.up = transform.forward;
        }
        Destroy(
        Instantiate(FootParticules, hit.point + (hit.normal.normalized * 0.05f), quaternion.identity)
        ,5f);
    }

    public void RightFootEvent()
    {
        RaycastHit hit;
        if (Physics.Raycast(RightFoot.position + Vector3.up, RightFoot.position- (RightFoot.position + Vector3.up),
            out hit)) {
            Transform footSteep = Instantiate(RightFootDecal).transform;
            footSteep.position = hit.point+(hit.normal.normalized* DecalDepth);
            footSteep.up = transform.forward;
        } 
        Destroy(
            Instantiate(FootParticules, hit.point + (hit.normal.normalized * 0.05f), quaternion.identity)
            ,5f);
    }
}
