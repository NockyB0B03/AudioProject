using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// Se usi XR Interaction Toolkit 3.0 o superiore, scommenta la riga sotto se ti dà errore su XRSimpleInteractable:
// using UnityEngine.XR.Interaction.Toolkit.Interactables; 
using FMODUnity;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class DoorTouchController : MonoBehaviour
{
    [Header("Impostazioni Movimento Porta")]
    [Tooltip("Distanza di scorrimento verso destra (in unità/metri di Unity)")]
    public float slideDistance = 2f;

    [Tooltip("Direzione dello scorrimento (Vector3.right = destra locale)")]
    public Vector3 slideDirection = Vector3.right;

    [Tooltip("Velocità di scorrimento")]
    public float speed = 3f;

    [Header("Suoni FMOD")]
    public EventReference openSound;
    public EventReference closeSound;

    [Header("Modalità di Attivazione VR")]
    [Tooltip("Se TRUE la porta si apre cliccando/afferrando. Se FALSE si apre appena la sfiori col controller/mano.")]
    public bool triggerOnSelect = true;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable simpleInteractable;

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

        // Riproduzione suono FMOD posizionato nello spazio 3D
        if (isOpen && !openSound.IsNull)
        {
            RuntimeManager.PlayOneShot(openSound, transform.position);
        }
        else if (!isOpen && !closeSound.IsNull)
        {
            RuntimeManager.PlayOneShot(closeSound, transform.position);
        }

        StartCoroutine(SlideDoorRoutine(targetPosition));
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