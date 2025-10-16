using Monsters;
using UnityEngine;

public interface IAiming
{
    Vector3 GetAimDirection(Transform shootPoint, Monster target, float projectileSpeed, Vector3 gravity);
}