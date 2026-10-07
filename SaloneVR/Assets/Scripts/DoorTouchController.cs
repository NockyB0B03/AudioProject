using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using FMODUnity;
using FMOD.Studio; // Importante per gestire le istanze audio

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class DoorTouchController : MonoBehaviour
{
    [Header("Impostazioni Movimento Porta")]
    public float slideDistance = 2f;
    public Vector3 slideDirection = Vector3.right;
    public float speed = 3f;

    [Header("Suoni FMOD")]
    public EventReference openSound;
    public EventReference closeSound;

    [Header("Modalità di Attivazione VR")]
    public bool triggerOnSelect = true;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable simpleInteractable;

    // Gestione istanze audio per poterle interrompere
    private EventInstance openSoundInstance;
    private EventInstance closeSoundInstance;

    private void Awake()
    {
        simpleInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        if (simpleInteractable != null)
        {
            if (triggerOnSelect)
                simpleInteractable.selectEntered.AddListener(OnInteractSelect);
            else
                simpleInteractable.hoverEntered.AddListener(OnInteractHover);
        }
    }

    private void OnDisable()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.selectEntered.RemoveListener(OnInteractSelect);
            simpleInteractable.hoverEntered.RemoveListener(OnInteractHover);
        }

        // Interrompe e rilascia la memoria audio se la porta viene disattivata
        StopAndReleaseEvent(ref openSoundInstance);
        StopAndReleaseEvent(ref closeSoundInstance);
    }

    private void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + (slideDirection.normalized * slideDistance);
    }

    private void OnInteractSelect(SelectEnterEventArgs args) => ToggleDoor();
    private void OnInteractHover(HoverEnterEventArgs args) => ToggleDoor();

    public void ToggleDoor()
    {
        if (isMoving) return;

        isOpen = !isOpen;
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;

        if (isOpen)
        {
            // Se si stava chiudendo, ferma l'audio di chiusura
            StopAndReleaseEvent(ref closeSoundInstance);

            // Fai partire il suono di apertura
            if (!openSound.IsNull)
            {
                openSoundInstance = RuntimeManager.CreateInstance(openSound);
                openSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
                openSoundInstance.start();
            }
        }
        else
        {
            // Ferma immediatamente l'audio di apertura
            StopAndReleaseEvent(ref openSoundInstance);

            // Fai partire il suono di chiusura
            if (!closeSound.IsNull)
            {
                closeSoundInstance = RuntimeManager.CreateInstance(closeSound);
                closeSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
                closeSoundInstance.start();
            }
        }

        StartCoroutine(SlideDoorRoutine(targetPosition));
    }

    // Metodo di supporto per fermare e rilasciare un'istanza FMOD
    private void StopAndReleaseEvent(ref EventInstance instance)
    {
        if (instance.isValid())
        {
            // Usa FMOD.Studio.STOP_MODE.IMMEDIATE per un taglio netto
            // oppure FMOD.Studio.STOP_MODE.ALLOWFADEOUT se preferisci una sfumatura rapida
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }

    private IEnumerator SlideDoorRoutine(Vector3 target)
    {
        isMoving = true;

        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
            yield return null;
        }

        transform.localPosition = target;
        isMoving = false;
    }
}