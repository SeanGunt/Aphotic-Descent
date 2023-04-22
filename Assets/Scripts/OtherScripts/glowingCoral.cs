using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowingCoral : MonoBehaviour
{
    [SerializeField] private Texture textureToApply;
    private Material glowMaterial;
    private float fresnelPower;
    private GameObject player;
    MeshRenderer meshRenderer;
    private float distToPlayer;
    private bool playing;
    private State state;

    private enum State
    {
        glowingUp, glowingDown, transition, initial
    }
    
    void Awake()
    {
      
      player = GameObject.FindGameObjectWithTag("Player");
    
      meshRenderer = GetComponent<MeshRenderer>();

      glowMaterial = Instantiate(meshRenderer.sharedMaterial);
      meshRenderer.material = glowMaterial;
      glowMaterial.SetTexture("_texture", textureToApply);
      playing = false;
      state = State.initial;
      fresnelPower = 30f;
    }

    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        if(playing) return;
        else
        {
            DistanceChecker();
        }

        switch(state)
        {
            case State.initial:
                Initial();
            break;
            case State.glowingUp:
                GlowUp();
            break;
            case State.glowingDown:
                GlowDown();
            break;
            case State.transition:

            break;
        }
    }

    private void DistanceChecker()
    {
        if(distToPlayer <= 10)
        {
            state = State.glowingUp;
        }

        if(distToPlayer > 10 && distToPlayer <= 25)
        {
            state = State.glowingDown;
        }
    }

    private void Initial()
    {
        DistanceChecker();
        playing = false;
    }
    private void GlowUp()
    {
        StartCoroutine(glowUp(15f));
        StopCoroutine("glowDown");
        state = State.transition;
    }

    private void GlowDown()
    {
        StartCoroutine(glowDown(10f));
        StopCoroutine("glowUp");
        state = State.transition;
    }

    private void OnDestroy()
    {
        if (glowMaterial != null)
        {
            Debug.Log("Destroy Was Called");
            Destroy(glowMaterial);
        }
    }

    private IEnumerator glowUp(float t)
    {
        playing = true;
        while (fresnelPower >= 1)
        {
            fresnelPower -= Time.deltaTime * t;
            glowMaterial.SetFloat("_power", fresnelPower);

            yield return null;
        }
        playing = false;
    }

    private IEnumerator glowDown(float t)
    {
        playing = true;
        while (fresnelPower <= 30)
        {
            fresnelPower += Time.deltaTime * t;
            glowMaterial.SetFloat("_power", fresnelPower);

            yield return null;
        }
        playing = false;
    }
}
