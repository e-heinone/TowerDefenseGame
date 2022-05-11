using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputs : MonoBehaviour
{
    private GameManager _gameManager;

    public GameObject tower1Prefab;
    public GameObject tower2Prefab;
    private GameObject towerChoice;

    private bool storeOpen = false;

    private RaycastHit2D spotClicked;

    // references to ui parts needed
    public GameObject towerBuyScreen;
    public Button tower1Button;
    public Button tower2Button;

    // reference to hud
    HUDScript hud;

    private void Start()
    {
        // sets tower buy screen false at start by default
        towerBuyScreen.gameObject.SetActive(false);

        tower1Button.onClick.AddListener(Tower1Selection);
        tower2Button.onClick.AddListener(Tower2Selection);

        _gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
        hud = GameObject.Find("HUD 1").gameObject.GetComponent<HUDScript>();
    }

    private void Update()
    {
        // mouse controls
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // projects a raycast from the spot clicked onto the playfield
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            // opens menu for store where player can choose a tower to set onto the clicked tile
            OpenTowerStore(rayHit);
        }

        // touch controls
        else
        {
            foreach (Touch touch in Input.touches)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(touch.position));

                    OpenTowerStore(rayHit);
                }
            }
        }
    }

    void Tower1Selection()
    {
        // if player has enough gold for tower1, instantiate a tower onto the clicked grid and then close the tower store screen
        if (_gameManager.gold >= 50)
        {
            towerChoice = tower1Prefab;
            Debug.Log("you clicked tower1 button");

            // create tower on the grid piece you clicked
            Instantiate(towerChoice, spotClicked.transform.position, Quaternion.identity);
            _gameManager.gold -= 250;

            towerBuyScreen.SetActive(false);
            storeOpen = false;
        }

        // if player does not have enough money, 
        else
        {
            hud.PrintMessage("You don't have enough gold!");
            towerBuyScreen.SetActive(false);
            storeOpen = false;
        }
    }

    void Tower2Selection()
    {
        if (_gameManager.gold >= 500)
        {
            towerChoice = tower2Prefab;
            Debug.Log("you clicked tower2 button");

            // create tower on the grid piece you clicked
            Instantiate(towerChoice, spotClicked.transform.position, Quaternion.identity);
            _gameManager.gold -= 100;

            towerBuyScreen.SetActive(false);
            storeOpen = false;
        }

        else
        {
            towerBuyScreen.SetActive(false);
            storeOpen = false;
        }
    }

    void OpenTowerStore(RaycastHit2D rayHit)
    {
        Collider2D hitCollider = rayHit.collider;

        if (hitCollider != null)
        {
            Debug.Log(rayHit.collider.gameObject);

            if (rayHit.transform.tag == "Grid")
            {
                // if you click/tap on an empty spot that is not a road tile
                if (!rayHit.collider.gameObject.GetComponent<GridInfo>().Full && !rayHit.collider.gameObject.GetComponent<GridInfo>().IsRoad && !storeOpen)
                {
                    rayHit.collider.gameObject.GetComponent<GridInfo>().Full = true;
                    //Debug.Log("you clicked: " + rayHit.transform.tag);
                    Debug.Log("You clicked " + rayHit.collider.gameObject);
                    // opens up the menu in which player can choose which tower to add
                    spotClicked = rayHit;
                    towerBuyScreen.SetActive(true);
                    storeOpen = true;
                }
            }
        }
    }
}
