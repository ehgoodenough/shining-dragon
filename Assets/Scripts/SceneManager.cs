using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {
    // Declare prefabs (for assigning in inspector)
    private GameObject Hero;
    private GameObject Thug;

    // Declare the entities in our scene
    private Thug thug;
    private Hero hero;
    public HealthBar healthbar;
    public Text endMessage;
    public const float restartTime = 3000;
    
    public bool gameHasEnded = false;

    private string state;

    void Awake() {
        this.Hero = Resources.Load("MonkHero") as GameObject;
        this.Thug = Resources.Load("OniThug") as GameObject;

        this.createHero();
        this.createThug();

        if (GameObject.Find("HealthBar")) {
            this.healthbar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        }

        endMessage.text = "";
    }

    public Hero getHero() {
        return hero;
    }

    public Thug getThug() {
        return thug;
    }

    public void gameEnd(bool win)
    {
        if(this.gameHasEnded != true) {
            this.gameHasEnded = true;
            print("game end");
            if (win)
            {
                endMessage.text = "You Win!";
            }
            else
            {
                endMessage.text = "You Lose!";
            }
            StartCoroutine(ExecuteAfterTime(3));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        print("execute after time");
        Application.LoadLevel(0);
    }

    public Thug createThug() {
        this.thug = Object.Instantiate(Thug, new Vector2(12f, 1.75f), Quaternion.identity).GetComponent<Thug>();
        this.thug.gameObject.name = "Thug" + Random.Range(100, 999);
        return this.thug;
    }

    public Hero createHero() {
        this.hero = Instantiate(Hero, new Vector2(-7.5f, 1.75f), Quaternion.identity).GetComponent<Hero>();
        this.hero.gameObject.name = "Hero";
        return this.hero;
    }

    public void destroyThug() {
        Object.Destroy(this.thug.gameObject);
    }

    void Update()
    {
       
    }
}
