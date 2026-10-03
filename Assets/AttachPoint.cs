using UnityEngine;

public class AttachPoint : MonoBehaviour {
    public IShipModule Module { get; private set; }

    public void Attach(IShipModule module) => Module = module;
}