using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FxAutoDestroy : MonoBehaviour
{
    public void DestroySelf() => Destroy(gameObject);
}
