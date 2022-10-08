using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{

    public static bool horizontal = true;
    public Toggle horizontalToggle;
    public Toggle verticalToggle;
    public GameObject OreintationGroup;

    public static bool flat = true;
    public Toggle flatToggle;
    public Toggle threeDToggle;

    void Start(){

        if(flat){
            flatToggle.isOn = true;
            threeDToggle.isOn = false;
            OreintationGroup.SetActive(true);
        } else{
            flatToggle.isOn = false;
            threeDToggle.isOn = true;
            OreintationGroup.SetActive(false);
        }

        if(horizontal){
            horizontalToggle.isOn = true;
            verticalToggle.isOn = false;
        } else{
            horizontalToggle.isOn = false;
            verticalToggle.isOn = true;
        }
    }
    
    public void Horizontal(bool t)
    {
        if(t){
            horizontal = true; 
        }

    }

    public void Vertical(bool t)
    {
        if(t){
            horizontal = false;
        }
    }

    public void Flat(bool t)
    {
        if(t){
            flat = true;
            OreintationGroup.SetActive(true);
        }
    }

    public void ThreeD(bool t)
    {
        if(t){
            flat = false;
            OreintationGroup.SetActive(false);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
