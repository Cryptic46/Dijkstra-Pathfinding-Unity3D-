using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDebugger : MonoBehaviour
{
    ///<summary>
    ///<para>This node came from the element of the corresponding currentNodeCollection</para>
    ///</summary>
    [HideInInspector]
    public List<Vector3> cameFromCollection;
    ///<summary>
    ///<para>A list of nodes which maps to cameFromCollection</para>
    ///</summary>
    [HideInInspector]
    public List<Vector3> currentNodeCollection;
    private List<GameObject> debugContainer;

    //Debug Start
    public Transform debugTile;
    private HashSet<Vector3> debugTileCollection;
    //Debug End

    public void Initializer(Transform source)
    {
        cameFromCollection = new List<Vector3>();
        currentNodeCollection = new List<Vector3>();
        debugTileCollection = new HashSet<Vector3>();
        debugContainer = new List<GameObject>();

        currentNodeCollection.Add(source.position);
        cameFromCollection.Add(source.position);
    }

    public void Reinitializer(Transform source)
    {
        currentNodeCollection.Clear();
        cameFromCollection.Clear();

        currentNodeCollection.Add(source.position);
        cameFromCollection.Add(source.position);
    }

    public void ShowDebugTiles()
    {
        float xDimension;
        float yDimension;

        for (int i = 0; i < currentNodeCollection.Count; ++i)
        {
            Transform objectInstance;

            xDimension = cameFromCollection[i].x - currentNodeCollection[i].x;
            yDimension = cameFromCollection[i].y - currentNodeCollection[i].y;

            if (!debugTileCollection.Contains(cameFromCollection[i] - new Vector3(0.0f, 0.0f, 0.25f)))
            {
                if (xDimension == -1)
                {
                    objectInstance = Instantiate(debugTile, cameFromCollection[i] - new Vector3(0.0f, 0.0f, 0.6f), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    debugTileCollection.Add(objectInstance.position);
                    debugContainer.Add(objectInstance.gameObject);
                }
                else if (xDimension == 1)
                {
                    objectInstance = Instantiate(debugTile, cameFromCollection[i] - new Vector3(0.0f, 0.0f, 0.6f), Quaternion.Euler(0.0f, 0.0f, 180.0f));
                    debugTileCollection.Add(objectInstance.position);
                    debugContainer.Add(objectInstance.gameObject);
                }
                else if (yDimension == 1)
                {
                    objectInstance = Instantiate(debugTile, cameFromCollection[i] - new Vector3(0.0f, 0.0f, 0.6f), Quaternion.Euler(0.0f, 0.0f, 270.0f));
                    debugTileCollection.Add(objectInstance.position);
                    debugContainer.Add(objectInstance.gameObject);
                }
                else if (yDimension == -1)
                {
                    objectInstance = Instantiate(debugTile, cameFromCollection[i] - new Vector3(0.0f, 0.0f, 0.6f), Quaternion.Euler(0.0f, 0.0f, 90.0f));
                    debugTileCollection.Add(objectInstance.position);
                    debugContainer.Add(objectInstance.gameObject);
                }
            }
        }
    }

    public void RemoveDebugTiles()
    {
        debugTileCollection.Clear();

        for (int z = 0; z < debugContainer.Count; ++z)
        {
            Destroy(debugContainer[z]);
        }
    }
}