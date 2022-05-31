using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleRenderer : MonoBehaviour{
        ParticleSystem particle;
    // Start is called before the first frame update
    void Start(){
        gameObject.TryGetComponent<ParticleSystem>(out particle);
        if(particle != null){
            particle = gameObject.GetComponent<ParticleSystem>();
            particle.Stop();
        }
    }

    private void OnTriggerEnter(Collider c){
           if(gameObject.TryGetComponent<ParticleSystem>(out particle)){
            if(c.tag == "RenderSphere"){
                particle.Play();
            }
        }
    }
    private void OnTriggerExit(Collider c){
        if(gameObject.TryGetComponent<ParticleSystem>(out particle)){
            if(c.tag == "RenderSphere"){
                particle.Stop();
            }
        }
        
    }
}
