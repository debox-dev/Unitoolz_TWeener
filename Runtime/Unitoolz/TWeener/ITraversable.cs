using UnityEngine;
using System.Collections;

public interface ITraversable {
    float Length { get; }
    Vector3 GetPoint(float t);
    Vector3 GetDirection(float t);
}
