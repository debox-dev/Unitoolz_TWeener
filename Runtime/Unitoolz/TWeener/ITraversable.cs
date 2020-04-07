using UnityEngine;


namespace DeBox.Unitoolz.TWeener
{
    public interface ITraversable
    {
        float Length { get; }
        Vector3 GetPoint(float t);
        Vector3 GetDirection(float t);
    }

}