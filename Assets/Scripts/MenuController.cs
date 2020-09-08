using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Animator anim;

    private bool open;

    [SerializeField]
    private GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressMenu()
    {
        if(!anim.GetBool("Open"))
        {
            anim.SetBool("Open", true);
            button.SetActive(true);
        } else
        {
            anim.SetBool("Open", false);
            button.SetActive(false);
        }
    }

    [SerializeField]
    public GameObject rotate;
    [SerializeField]
    public GameObject scale;
    [SerializeField]
    public GameObject moving;


    public void ObjectMenu_On()
    {
        rotate.SetActive(true);
        scale.SetActive(true);
        moving.SetActive(true);
    }

    public void ObjectMenu_Off()
    {
        rotate.SetActive(false);
        scale.SetActive(false);
        moving.SetActive(false);
    }
}
