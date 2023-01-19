using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderScript : MonoBehaviour
{
    // Start is called before the first frame update
    private new Renderer renderer;
    private Transform player;
    public Texture Texture, Texture2;
    public Texture m_MainTexture, m_Normal, m_Metal;
    public GameObject go;
    private bool done = false;
    string LR = "";

    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;




    void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetTexture("_BaseColorMap", Texture2);

        player = GameObject.FindGameObjectWithTag("PlayerBody").transform;


        /*var mat = new Material(shader);
        mat.mainTexture = Texture;
        renderer.material = mat;*/
        /*renderer.material.shader = Shader.Find("HDRP/Lit");
        renderer.material.EnableKeyword("_NORMALMAP");
        renderer.material.EnableKeyword("_METALLICGLOSSMAP");
        renderer.material.EnableKeyword("_BaseColorMap")
        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        renderer.material.SetTexture("_MainTex", m_MainTexture);
        //Set the Normal map using the Texture you assign in the Inspector
        renderer.material.SetTexture("_BumpMap", m_Normal);
        //Set the Metallic Texture as a Texture you assign in the Inspector
        renderer.material.SetTexture("_MetallicGlossMap", m_Metal);*/

    }

    // Update is called once per frame
    void Update()
    {
        

        //if (distance() < 8 && !done)
        //{
        //    done = !done;
        //    gameObject.transform.Rotate(0.0f, 0.0f, 45.0f, Space.Self);
        //    renderer.material.SetTexture("_BaseColorMap", Texture);
        //    audioSource.PlayOneShot(clip, volume);
        //}
        //else if(distance() > 8 && done){
        //    done = !done;
        //    gameObject.transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
            
        //}



        

    }

    private void OnTriggerEnter(Collider collision)
    {
        //renderer.material.mainTexture = Texture;
        //renderer.material.SetTexture("_MainTex", Texture);
        //renderer.material.SetTexture("_BaseColorMap", Texture);

        StartCoroutine("Scary", collision);



    }

    private void OnTriggerExit(Collider collision)
    {
        
        


    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    gameObject.transform.Rotate(0.0f, 0.0f, 45.0f, Space.Self);
    //    renderer.material.SetTexture("_BaseColorMap", Texture);
    //    audioSource.PlayOneShot(clip, volume);
    //}
    //private  void OnCollisionExit(Collision collision)
    //{
    //    gameObject.transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
    //}

    IEnumerator Scary(Collider collision)
    {
        if (collision.gameObject.name == "Player" && !done && (side() == "right"))
        {
            LR = "right";
            gameObject.transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 4, gameObject.transform.position.y, gameObject.transform.position.z);

            renderer.material.SetTexture("_BaseColorMap", Texture);
            audioSource.PlayOneShot(clip, volume);
            done = !done;
            //Debug.Log("Enter R");
        }
        else if (collision.gameObject.name == "Player" && !done && (side() == "left"))
        {
            LR = "left";
            gameObject.transform.Rotate(0.0f, 0.0f, 45.0f, Space.Self);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 4, gameObject.transform.position.y, gameObject.transform.position.z);
            renderer.material.SetTexture("_BaseColorMap", Texture);
            audioSource.PlayOneShot(clip, volume);
            done = !done;
            //Debug.Log("Enter L");
        }
        yield return new WaitForSeconds(.99f);
        if (collision.gameObject.name == "Player" && done && LR == "right")
        {
            gameObject.transform.Rotate(0.0f, 0.0f, 45.0f, Space.Self);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 4, gameObject.transform.position.y, gameObject.transform.position.z);

            done = !done;
            //Debug.Log("Leave R");
        }
        else if (collision.gameObject.name == "Player" && done && LR == "left")
        {
            gameObject.transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 4, gameObject.transform.position.y, gameObject.transform.position.z);

            done = !done;
            //Debug.Log("Leave L");
        }
        yield return new WaitForSeconds(.99f);
        StopCoroutine("Scary");
    }









    private float distance()
    {
        return Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2) + Mathf.Pow(player.position.z - transform.position.z, 2));
    }

    private string side()
    {
        int dist = (int)(player.position.x - transform.position.x);
        if (dist >= 0)
        {
            
            return "right";
        }
        else{

            
            return "left";
        }
    }
}
