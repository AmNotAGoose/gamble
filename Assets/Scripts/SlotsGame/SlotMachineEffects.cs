using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SlotMachineEffects : MonoBehaviour
{
    public AudioSource yipee;
    public GameObject winEffects;

    public IEnumerator PlayWin(SlotWinEvent winEvent)
    {
        print("sd");

        GameObject particle = Instantiate(winEffects);
        particle.transform.position = new Vector2 (particle.transform.position.x + Random.Range(0f, 3f), particle.transform.position.y + Random.Range(0f, 3f));
        particle.GetComponent<ParticleSystem>().Play();
        yipee.Play();

        foreach (SlotItem item in winEvent.winningItems)
        {
            item.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.3f);

        foreach (SlotItem item in winEvent.winningItems)
        {
            item.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
    }
}
