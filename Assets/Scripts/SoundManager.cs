using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DeliveryManger.Instance.OnRecipeSuccess += DeliveryManger_OnRecipeSuccess;
        DeliveryManger.Instance.OnRecipeFailed += DeliveryManger_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trach, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManger_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void DeliveryManger_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)], position, volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}
