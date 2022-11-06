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
        int numCollisionEvents = _particles.GetCollisionEvents(other, _collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Debug.Log("particle entered");
            // If particle entered a city, then destroy it
            City city = _collisionEvents[i].colliderComponent.gameObject.GetComponent<City>();
            if (city)
            {
                city.DestroyCity();
            }

            // if particle entered bomb, then destroy it
            Bomb bomb = _collisionEvents[i].colliderComponent.gameObject.GetComponent<Bomb>();
            if (bomb)
            {
                bomb.DestroyBomb(true);
            }
        }
    }
}
