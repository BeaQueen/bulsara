using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class AutoSave : MonoBehaviour
{
    public bool deactivateAfterSave;

    [FormerlySerializedAs("collider")]
    public BoxCollider2D saveCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Save.instance.SaveGame();

        if (deactivateAfterSave)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 120, .3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(saveCollider.offset.x, saveCollider.offset.y, -.7f), saveCollider.size);
    }
}
