using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private GameObject HealthBarHolder;
    public GameObject HPBarGlowEffectPreFab;

    public bool DebugHealthBar;
    public int DebugForcedHealth;
    public int DebugForcedHealthMax;

    private float StartingX;

    
    private int OriginalNumberOfPipes;
    private List<GameObject> Pipes;
    public float WidthPerPipe;

    public List<GlowEffect> LiveGlowEffects;


    private SpriteRenderer _Sprite;
    private float BestScore;

	// Use this for initialization
	void Start () {
        StartingX = transform.position.x;
        HealthBarHolder = transform.parent.gameObject;

        _Sprite = GetComponent<SpriteRenderer>();

        WidthPerPipe = _Sprite.bounds.size.x / transform.localScale.x;
        OriginalNumberOfPipes = (int) transform.localScale.x;

        PreparePipes();

        _Sprite.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if(DebugHealthBar)
        {
            ImplUpdateWidth(DebugForcedHealth, DebugForcedHealthMax);
        }
	}

    private void PreparePipes()
    {
        Pipes = new List<GameObject>();
        var leftEdge = StartingX - (OriginalNumberOfPipes * WidthPerPipe) / 2.0f;

        var spriteBase = _Sprite;
        var childPrefab = new GameObject();
        var childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childSprite.transform.position = transform.position;
        childSprite.sprite = spriteBase.sprite;
        childSprite.transform.localScale = new Vector3(1, 1);
        childSprite.sortingOrder = spriteBase.sortingOrder;
        childSprite.sortingLayerID = spriteBase.sortingLayerID;
        childSprite.color = spriteBase.color;
        childSprite.name = "Health Pipe";

        for (var i = 0; i < OriginalNumberOfPipes; i++)
        {
            var x = leftEdge + (WidthPerPipe / 2.0f) + i * WidthPerPipe;

            var pipe = Instantiate(childPrefab);
            pipe.transform.position = new Vector3(x, transform.position.y, transform.position.z);
            pipe.transform.parent = transform;
            Pipes.Add(pipe);
        }

        Destroy(childPrefab);
    }

    private void ImplUpdateWidth(int health, int maxHealth)
    {
        if (health > maxHealth)
            throw new System.Exception("More health than max health!");
        if (health < 0)
            throw new System.Exception("Less than 0 health!");
        if (maxHealth != OriginalNumberOfPipes)
            throw new System.Exception($"This healthbar only works for exactly {OriginalNumberOfPipes} pipes - the same as the original x scale on the healthbar - but you specifed {maxHealth} max health");
        for(int i = 0; i < health; i++)
        {
            var pipe = Pipes[i];
            var sprite = pipe.GetComponent<SpriteRenderer>();

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1.0f);
        }

        for(int i = health; i < maxHealth; i++)
        {
            var pipe = Pipes[i];
            var sprite = pipe.GetComponent<SpriteRenderer>();

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.0f);
        }
    }

    private void SpawnGlowEffect(Vector3 position, int width)
    {
        var glowEffectGameObj = Instantiate(HPBarGlowEffectPreFab, position, Quaternion.identity);
        glowEffectGameObj.name = $"Glow Effect {position.x}";
        glowEffectGameObj.transform.SetParent(HealthBarHolder.transform , true);

        var rectTrans = glowEffectGameObj.GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(width, rectTrans.sizeDelta.y);

        var glowEffect = glowEffectGameObj.GetComponent<GlowEffect>();

        var score = (float)width;
        if (LiveGlowEffects.Count > 0)
            score += LiveGlowEffects[LiveGlowEffects.Count - 1].ComboMultiplier;
        glowEffect.ComboMultiplier = score;

        if(score > BestScore)
        {
            BestScore = score;
        }

        LiveGlowEffects.Add(glowEffect);
    }

    public void UpdateWidth (int health, int maxHealth)
    {
        if(!DebugHealthBar)
            ImplUpdateWidth(health, maxHealth);
    }
}
