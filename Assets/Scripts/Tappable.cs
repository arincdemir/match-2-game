using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITappable
{
    // process the user tapping on a node event
    // returns true if the tap is valid
    // which means the tap resulted in nodes to be destroyed
    public bool Tap();
}
