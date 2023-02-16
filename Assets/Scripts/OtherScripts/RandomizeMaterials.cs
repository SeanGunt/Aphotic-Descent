using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMaterials : MonoBehaviour
{
    [SerializeField] private Material[] mat1, mat2, mat3;
    private MeshRenderer meshRenderer;
    private int randomGenerator;

    private void Awake()
    {
        randomGenerator = Random.Range(0,3);
        meshRenderer = GetComponent<MeshRenderer>();
        switch(randomGenerator)
        {
            case 0:
                meshRenderer.sharedMaterials = mat1;
            break;
            case 1:
                meshRenderer.sharedMaterials = mat2;
            break;
            case 2:
                meshRenderer.sharedMaterials = mat3;
            break;
        }
    }
}
