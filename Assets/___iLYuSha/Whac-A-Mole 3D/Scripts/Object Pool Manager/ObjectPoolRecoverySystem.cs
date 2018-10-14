using UnityEngine;

public class ObjectPoolRecoverySystem : MonoBehaviour
{
    // 方案1
    //public event RecoverEventHandler RecoverCallee;
    // 方案2
    public ObjPoolData OPD;

    protected virtual void RecoverGameObject(GameObject obj)
    {
        // 方案1
        //RecoverCallee(this, new RecoverEventArgs(obj));
        // 方案2
        OPD.Recovery(obj);
    }
}