using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    private ParticleSystem _particles;
    private List<ParticleCollisionEvent> _collisionEvents;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("particles entered city!");
        int numCollisionEvents = _particles.GetCollisionEvents(other, _collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            City city = _collisionEvents[i].colliderComponent.gameObject.GetComponent<City>();
            Debug.Log($"city = {city}");
            if (city)
            {
                Destroy(city.gameObject);
            }
        }
    }
}
