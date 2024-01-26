using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Animator anim;

    //public Sprite newImage;
    //public Button button;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOutButton()
    {
        anim.SetTrigger("FadeOut");
    }

    /*public void ChangeGraphic()
    {
        button.image.sprite = newImage;
    } */
}
