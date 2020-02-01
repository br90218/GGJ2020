using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringLine : MonoBehaviour {
    public LineRenderer Line;

    protected Vector3[] Points = new Vector3[2];

    public void Update() {
        Line.SetPositions(Points);
    }
}
