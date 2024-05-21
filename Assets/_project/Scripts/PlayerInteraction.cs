using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    List<Collider> hits;
    [SerializeField] private float radius;
    [SerializeField] private float floatRadius;
    [SerializeField] private LayerMask mask;

    [SerializeField] private List<float> distances;

    public List<Interactable> interactables;

    private void Start()
    {
        hits = new List<Collider>();
    }

    private void FixedUpdate()
    { 
        hits = Physics.OverlapSphere(transform.position, radius, mask).ToList();

        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].GetComponent<Interactable>())
            {
                if (!interactables.Contains(hits[i].GetComponent<Interactable>()))
                {
                    interactables.Add(hits[i].GetComponent<Interactable>());
                    hits[i].GetComponent<Interactable>().inRadius = true;
                    hits[i].GetComponent<Interactable>().inRadiusEvent.Invoke();
                }
            }
        }

        distances = DistToObjects(interactables);
    }

    private List<float> DistToObjects(List<Interactable> list)
    {
        List<float> dists = new();
        for (int i = 0; i < list.Count; i++)
        {
            dists.Add(Vector3.Distance(transform.position, list[i].transform.position));
        }

        for (int i = 0; i < dists.Count; i++)
        {
            if (dists[i] > floatRadius)
            {
                list[i].inRadius = false;
                list[i].inRadiusEvent.Invoke();
                list.Remove(list[i]);
                dists.Remove(dists[i]);
            }
        }

        return dists;
    }
}
