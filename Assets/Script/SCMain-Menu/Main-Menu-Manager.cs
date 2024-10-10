using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Cinematic")]
    public GameObject policecar;
    public GameObject VCIN1;
    public GameObject VCIN2;
    public GameObject VCAM1;
    public GameObject VCAM2;

    public float CIN1to2;
    public float CIN2toMain;

    [Header("UI Panel")]
    public GameObject UI_Menu;
    public GameObject SettingPanel;
    public GameObject SelectLevelPanel;
    public GameObject SelectPlanePanel;

    [Header("Animation")]
    public GameObject Plane1;
    public GameObject Plane2;
    public Vector3 MovePlane1;
    public Vector3 BackPlane1;
    public Vector3 MovePlane2;
    public Vector3 BackPlane2;
    public float backdur;
    public float movedur;

    [Header("Button")]
    public Button HG1;
    public Button HG2;
    public Button LevForest;
    public Button LevSea;

    [Header("Level Selected")]
    public string LevelSelected;

    [Header("Plane Selected")]
    public string PlaneSelected;
    private void Awake()
    {
        
    }
    private void Start()
    {
        openingCIN1();
    }

    private void Update()
    {

    }
    public void openingCIN1()
    {
        DisableMenu();
        policecar.SetActive(true);
        VCIN1.SetActive(true);
        Invoke("openingCIN2", CIN1to2);
    }
    public void openingCIN2()
    {
        VCIN1.SetActive(false);
        VCIN2.SetActive(true);
        Invoke("ActivateMenu", CIN2toMain);
    }

    public void ActivateMenu()
    {
        policecar.SetActive(false);

        VCIN2.SetActive(false);
        VCAM1.SetActive(true);
        UI_Menu.SetActive(true);

        HG1.onClick.AddListener(HG1_Select);
        HG2.onClick.AddListener(HG2_Select);
        LevForest.onClick.AddListener(LevForest_Select);
        LevSea.onClick.AddListener(LevSea_Select);
    }
    public void DisableMenu()
    {
        UI_Menu.SetActive(false);
    }

    public void HG1_Select()
    {
        Plane2.transform.DOMove(BackPlane2, backdur);
        VCAM2.SetActive(false);
        VCAM1.SetActive(true);
        Plane1.transform.DOMove(MovePlane1, movedur);
        PlaneSelected = "Plane1";
    }
    public void HG2_Select()
    {
        Plane1.transform.DOMove(BackPlane1, backdur);
        VCAM1.SetActive(false);
        VCAM2.SetActive(true);
        Plane2.transform.DOMove(MovePlane2, movedur);
        PlaneSelected = "Plane2";
    }
    public void LevForest_Select()
    {
        LevelSelected = "Game-Forest";
    }
    public void LevSea_Select()
    {
        LevelSelected = "Game-Sea";
    }
    public void AALevel()
    {
        SelectLevelPanel.SetActive(true);
        UI_Menu.SetActive(false);
    }
    public void AALevel_Back()
    {
        SelectLevelPanel.SetActive(false);
        UI_Menu.SetActive(true);
    }
    public void AAPlane()
    {
        SelectLevelPanel.SetActive(false);
        SelectPlanePanel.SetActive(true);
    }
    public void AAPlane_Back()
    {
        SelectLevelPanel.SetActive(true);
        SelectPlanePanel.SetActive(false);
    }
    public void AASetting()
    {
        SettingPanel.SetActive(true);
        UI_Menu.SetActive(false);
    }
    public void AASetting_Back()
    {
        SettingPanel.SetActive(false);
        UI_Menu.SetActive(true);
    }
    public void AAPlayGame()
    {
        if (LevelSelected == null)
        {
            Debug.Log("Select Level!!");
            return;
        }

        if (PlaneSelected == null)
        {
            Debug.Log("Select Plane!!");
            return;
        }

        if (LevelSelected != null && PlaneSelected != null)
        {
            SceneManager.LoadScene(LevelSelected);
            PlayerPrefs.SetString("MyPlane", PlaneSelected);
        }
    }
    public void AAExit()
    {
        Application.Quit();
    }
}
