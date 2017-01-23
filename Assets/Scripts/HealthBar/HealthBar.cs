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
    public int LastHealth;

    public List<GlowEffect> LiveGlowEffects;


    private SpriteRenderer _Sprite;
    private float BestScore;

    public float ShakeTimeRemaining;
    public float ShakePower;

    private Vector3 InitialPLocalosition;
    private Vector3? CurrentPositionTarget;
    private float CurrentPositionTargetSpeed;
    private bool PositionAtInitial;


    private float InitialLocalRotationZ;
    private float? CurrentRotationTargetZ;
    private float CurrentRotationTargetSpeed;
    private bool RotationAtInitial;

    private GameObject HPContainer;

    public float ShakePowerMultiplierScore;
    public float ShakeTimeMultiplierScore;

    public float ShakePowerPositionMultiplier;
    public float ShakePowerRotationMultiplier;
    // Use this for initialization
    void Start () {
        HPContainer = GameObject.Find("HealthBarOverlay");
        LiveGlowEffects = new List<GlowEffect>();

        ShakeTimeRemaining = 0;
        ShakePower = 0;
        InitialPLocalosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        InitialLocalRotationZ = transform.localRotation.z;
        CurrentPositionTarget = null;
        CurrentRotationTargetZ = null;

        PositionAtInitial = true;
        RotationAtInitial = true;

        StartingX = transform.position.x;
        HealthBarHolder = transform.parent.gameObject;

        _Sprite = GetComponent<SpriteRenderer>();

        WidthPerPipe = _Sprite.bounds.size.x / transform.localScale.x;
        OriginalNumberOfPipes = (int) transform.localScale.x;

        PreparePipes();

        LastHealth = OriginalNumberOfPipes;
        _Sprite.enabled = false;
    }

    void SetLocalPos(Vector3 localPos)
    {
        transform.localPosition = localPos;
        HPContainer.transform.localPosition = localPos;
    }

    void SetLocalRot(Quaternion rot)
    {
        transform.localRotation = rot;
        HPContainer.transform.localRotation = rot;
    }

    // Update is called once per frame
    void Update () {
        if(DebugHealthBar)
        {
            ImplUpdateWidth(DebugForcedHealth, DebugForcedHealthMax);
        }

        ShakePower *= 0.9f;

        if(CurrentPositionTarget.HasValue)
        {
            var sqrMaxMovement = CurrentPositionTargetSpeed * CurrentPositionTargetSpeed;
            var movementNeeded = CurrentPositionTarget.Value - transform.localPosition;

            if (movementNeeded.sqrMagnitude <= sqrMaxMovement)
            {
                SetLocalPos(CurrentPositionTarget.Value);
                CurrentPositionTarget = null;
            }else
            {
                var scale = CurrentPositionTargetSpeed / movementNeeded.magnitude;
                SetLocalPos(new Vector3(transform.localPosition.x + movementNeeded.x * scale,
                    transform.localPosition.y + movementNeeded.y * scale, transform.localPosition.z + movementNeeded.z * scale));
            }
        }

        if(CurrentRotationTargetZ.HasValue)
        {
            var maxMovement = CurrentRotationTargetSpeed;
            var movementNeeded = CurrentRotationTargetZ.Value - transform.localRotation.z;
            
            if(movementNeeded <= maxMovement)
            {
                SetLocalRot(new Quaternion(transform.localRotation.x, transform.localRotation.y, CurrentRotationTargetZ.Value, transform.localRotation.w));
                CurrentRotationTargetZ = null;
            }else
            {
                var deltaMove = maxMovement;
                if (movementNeeded < 0)
                    deltaMove = -maxMovement;

                SetLocalRot(new Quaternion(
                    transform.localRotation.x,
                    transform.localRotation.y,
                    transform.localRotation.z + deltaMove,
                    transform.localRotation.w
                    ));
            }
        }

        if(ShakeTimeRemaining > 0)
        {
            ShakeTimeRemaining -= Time.deltaTime;
            
            if(ShakePower <= 0)
            {
                Debug.LogWarning($"HealthBar ShakeTimeRemaining > 0 (is {ShakeTimeRemaining}) but ShakePower is non-positive (is {ShakePower}); this doesn't accomplish anything!");
            }else
            {
                if(!CurrentPositionTarget.HasValue)
                {
                    var targetX = InitialPLocalosition.x + ShakePower * Random.Range(-ShakePowerPositionMultiplier, ShakePowerPositionMultiplier);
                    var targetY = InitialPLocalosition.y + ShakePower * Random.Range(-ShakePowerPositionMultiplier, ShakePowerPositionMultiplier);

                    CurrentPositionTargetSpeed = ShakePower * Random.Range(1f, 2f);
                    CurrentPositionTarget = new Vector3(
                        targetX, targetY, InitialPLocalosition.z
                        );
                    PositionAtInitial = false;
                }

                if(!CurrentRotationTargetZ.HasValue)
                {
                    var targetZ = InitialLocalRotationZ + ShakePower * Random.Range(-ShakePowerRotationMultiplier, ShakePowerRotationMultiplier);

                    CurrentRotationTargetSpeed = ShakePower * Random.Range(1f, 2f);
                    CurrentRotationTargetZ = targetZ;
                    RotationAtInitial = false;
                }
            }
        }else
        {
            if(!CurrentPositionTarget.HasValue && !PositionAtInitial)
            {
                CurrentPositionTarget = new Vector3(InitialPLocalosition.x, InitialPLocalosition.y, InitialPLocalosition.z);
                CurrentPositionTargetSpeed = 1;
                PositionAtInitial = true;
            }

            if(!CurrentRotationTargetZ.HasValue && !RotationAtInitial)
            {
                CurrentRotationTargetZ = InitialLocalRotationZ;
                CurrentRotationTargetSpeed = 1;
                RotationAtInitial = true;
            }
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

        int lostHealth = LastHealth - health;
        if (lostHealth < 0)
            lostHealth = 0;

        float score = lostHealth;
        if (LiveGlowEffects.Count > 0)
            score += LiveGlowEffects[LiveGlowEffects.Count - 1].ComboMultiplier * 1.1f;

        for (int i = health; i < maxHealth; i++)
        {
            var pipe = Pipes[i];
            var sprite = pipe.GetComponent<SpriteRenderer>();

            var justDied = sprite.color.a != 0.0f;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.0f);

            if(justDied)
            {
                var glowEffectObj = Instantiate(HPBarGlowEffectPreFab);
                glowEffectObj.transform.SetParent(transform, false);
                glowEffectObj.transform.position = new Vector3(pipe.transform.position.x, pipe.transform.position.y, pipe.transform.position.z);
                var glowEffect = glowEffectObj.GetComponent<GlowEffect>();
                
                glowEffect.ComboMultiplier = score;
                LiveGlowEffects.Add(glowEffect);
            }
        }

        ShakePower += score * ShakePowerMultiplierScore;
        ShakeTimeRemaining += score * ShakeTimeMultiplierScore;

        LastHealth = health;
    }

    private void SpawnGlowEffect(Vector3 position, int width)
    {
        var glowEffectGameObj = Instantiate(HPBarGlowEffectPreFab, position, Quaternion.identity);
        glowEffectGameObj.name = $"Glow Effect {position.x}";
        glowEffectGameObj.transform.SetParent(HealthBarHolder.transform , false);

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
