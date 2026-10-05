using System.Collections;                      // Importa il namespace per le collezioni standard di .NET.
using System.Collections.Generic;              // Importa il namespace per le collezioni generiche (es. List, Dictionary).
using Unity.Mathematics;                       // Importa funzionalità matematiche avanzate di Unity (non utilizzate direttamente qui).
using UnityEngine;                             // Importa le classi base di Unity come MonoBehaviour, GameObject, Transform, ecc.

public class FollowPlayer : MonoBehaviour      
{
    private GameObject player;                 // Riferimento al GameObject del giocatore (qui la MainCamera).
    private float speed = 1.5f;                // Velocità di movimento (non usata nel codice attuale, ma potrebbe servire per il movimento futuro).
    private float rotationspeed = 3f;          // Velocità alla quale l'oggetto ruota verso il giocatore.

    private Animator animator;                 // Riferimento al componente Animator, usato per gestire le animazioni.

    void Start()                               
    {
        animator = GetComponent<Animator>();   // Ottiene il componente Animator associato a questo GameObject.
        player = GameObject.FindWithTag("MainCamera");  // Trova il GameObject che ha il tag "MainCamera" e lo assegna a player.
    }

    
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position; // Calcola la direzione verso il giocatore.
        direction.y = 0;                             // Ignora la componente verticale per evitare inclinazioni verso l'alto o il basso.
        float distance = Vector3.Magnitude(direction); // Calcola la distanza tra l'oggetto e il giocatore.

        if (distance > 1f)                          // Se la distanza è maggiore di 1 unità...
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);  // Crea una rotazione che guarda verso il giocatore.
            
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationspeed * Time.deltaTime);
            // Ruota gradualmente verso il giocatore in modo fluido.
                
            
            animator.enabled = true;               // Attiva l'animatore (ad es. per far partire un'animazione di camminata).
        } else 
        {
            animator.enabled = false;              // Disattiva l'animatore quando è abbastanza vicino (es. per fermare l’animazione).
            Destroy(gameObject);
        }
    }
}



