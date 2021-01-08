using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField, Range(10, 300)]
    int resolution = 10;

    [SerializeField, Range(0.1f, 1f)]
    float prefabScale = 0.5f;

    [SerializeField]
    Transform prefab;

    readonly List<Transform> transforms = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        CreateObjects();

        UpdateObjects();
    }

    void UpdateObjects()
    {
        var scale = 1f / resolution;

        var time = Time.time;

        var scaleVector = Vector3.one * scale * prefabScale;

        for (int x = 0, i = 0; x < resolution; ++x)
        {
            for (int z = 0; z < resolution; ++z, ++i)
            {
                var transform = transforms[i];

                var xScaled = (x + 0.5f) * scale;
                var zScaled = (z + 0.5f) * scale;
                var y = (Mathf.Sin(Mathf.PI * (time + Mathf.Min(xScaled, zScaled))) + 1f) * 0.125f;

                transform.localPosition = new Vector3(xScaled, y, zScaled);
                transform.localScale = scaleVector;
            }
        }
    }

    void CreateObjects()
    {
        var transformsCount = transforms.Count;
        var resolutionSqr = resolution * resolution;

        var difference = transformsCount - resolutionSqr;

        if (difference > 0)
        {
            var firstIndexToDelete = transformsCount - 1 - difference;

            var toDelete = transforms.GetRange(firstIndexToDelete, difference);

            for (int i = 0, c = toDelete.Count; i < c; ++i)
            {
                Destroy(toDelete[i].gameObject);
            }

            transforms.RemoveRange(firstIndexToDelete, difference);
        }

        else if (difference < 0)
        {
            /*if (transforms.Capacity < resolutionSqr)
            {
                transforms.Capacity = resolutionSqr;
            }*/

            for (int i = 0, c = Mathf.Abs(difference); i < c; ++i)
            {
                transforms.Add(Instantiate(prefab, transform, false));
            }
        }
    }
}
