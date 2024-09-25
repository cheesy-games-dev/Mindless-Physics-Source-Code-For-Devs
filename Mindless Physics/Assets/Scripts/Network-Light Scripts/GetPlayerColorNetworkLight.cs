using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerColorNetworkLight : MonoBehaviour
{
    public PlayerColorScript playerColorScript;
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;


    void Start()
    {
        playerColorScript = FindAnyObjectByType<PlayerColorScript>();
        meshRenderer = GetComponent<MeshRenderer>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if(meshRenderer != null) {
            meshRenderer.material.color = playerColorScript.universalPlayerColor;
        }
        else if (skinnedMeshRenderer != null) {
            skinnedMeshRenderer.material.color = playerColorScript.universalPlayerColor;
        }
    }
}
