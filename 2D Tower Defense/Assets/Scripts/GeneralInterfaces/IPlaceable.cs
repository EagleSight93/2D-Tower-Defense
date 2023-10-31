using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    public void PickedUp();
    public void Place(Vector3 position);
}
