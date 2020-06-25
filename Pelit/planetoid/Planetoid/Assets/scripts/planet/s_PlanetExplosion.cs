using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_PlanetExplosion : MonoBehaviour
{
    private Animator xplotionAnimate;

    public List<ParticleSystem> steamPool;
    public GameObject explosion;
    public GameObject gravityField;
    public GameObject planet;
    public Transform core;

    void Start()
    {
        xplotionAnimate = GetComponent<Animator>();
    }

    void OnEnable()
    {
        s_PlanetCoreCollide.OnEventBegan += StartDestructionSequence;
    }

    void OnDisable()
    {
        s_PlanetCoreCollide.OnEventBegan -= StartDestructionSequence;
    }

    /// <summary>
    /// Starts the destruction sequence. This function is called first, 
    /// right when the player touches the destructive planet. It triggers the
    /// StartTrigger in the Animator in the Icosphere.
    /// </summary>
    public void StartDestructionSequence(PlanetType planetType)
    {
        if (planetType == PlanetType.Destructive)
        {
            //The explotion animation starts here
            xplotionAnimate.SetTrigger("StartTrigger");
        }
    }

    /// <summary>
    /// This script is called from the animation event in Icosphere Animator.
    /// </summary>
    void StartExplotion()
    {
        //coreScript.DestroyPlanet ();
        StopSteam();
        DestroyPlanet();
    }

    /// <summary>
    /// Destroys the planet.
    /// </summary>
    public void DestroyPlanet()
    {
        int children = core.childCount;

        for (int i = 0; i < children; i++)
        {
            Transform t = core.GetChild(i);
            Rigidbody rb = t.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            int randomX = Random.Range(-100, 100);
            int randomY = Random.Range(-100, 100);
            int randomZ = Random.Range(-100, 100);

            rb.AddForce(new Vector3(randomX, randomY, randomZ));
        }

        Instantiate(explosion, transform.position, new Quaternion());

        core.GetComponent<CircleCollider2D>().enabled = false;
        // Disable the gravity field. Never ever enable it after this!!
        gravityField.SetActive(false);
    }

    /// <summary>
    /// Starts the debris scaling coroutine.
    /// This function is called from the ExplotionAnimation as an animation event.
    /// </summary>
    void StartDebrisScaling()
    {
        //coreScript.StartCoroutine ("ScaleDebris");
        StartCoroutine("ScaleDebris");
    }

    /// <summary>
    /// Scales the debris and when it is scaled to zero, deletes gameObject.
    /// When debris is is destroyed, destroys the whole planet.
    /// </summary>
    /// <returns>The debris.</returns>
    IEnumerator ScaleDebris()
    {
        Debug.Log("Scaling");

        bool endLoop = true;

        while (endLoop)
        {
            for (int i = 0; i < core.childCount; i++)
            {
                Transform t = core.GetChild(i);

                Vector3 currentScale = t.localScale;

                Debug.Log(currentScale);

                t.localScale = currentScale - new Vector3 (0.1f, 0.1f, 0.1f)* Time.deltaTime;

                if (t.localScale.x <= 0)
                {
                    Destroy(t.gameObject);
                }
            }

            yield return new WaitForSeconds (0.01f);

            if (core.childCount <= 0)
            {
                endLoop = false;
                //Destroy the planet.
                Destroy(planet);
            }
        }
    }

    /// <summary>
    /// Adds the next steam particle from the steampool to the breaking planets surface.
    /// </summary>
    void AddSteam()
    {
        for (int i = 0; i < steamPool.Count; i++)
        {
            if ( steamPool[i].isPlaying == false)
            {
                steamPool[i].Play(true);
                break;
            }
        }
    }

    /// <summary>
    /// Stops all steam particles on the breaking planets surface.
    /// </summary>
    void StopSteam()
    {
        for (int i = 0; i < steamPool.Count; i++)
        {
            steamPool[i].Stop(true);
        }
    }
}
