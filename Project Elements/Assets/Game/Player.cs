using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerEffect
{
    public float timer;
    public float amount;
    public int type;
};

public class Player : MonoBehaviour {

    public static GameObject ASGO;
    public static GameObject SoundGO;
    public AudioClip [] clips;

    public Transform BulletSpawn; //the object you want to rotate
    public GameObject BulletPrefab;
    public Transform itemBarTransform;
    public GameObject playerdeathparticle;
    public GameObject playerhitParticle;
    public Sprite[] bulletSprites;
    public List<PlayerEffect> effects;
    public RectTransform firstSpriteGameobj = null; //assign it via inspector.
    public RectTransform secondSpriteGameobj = null;
    public RectTransform thirdSpriteGameobj = null;
    public GameObject glowElement;
    public GameObject pointer;
    public Sprite[] Elements;
    public GameObject shieldPrefab;

    Vector3 mousePos;
    Rigidbody2D rb;
    Element element;
    Element previousElement;
    int selectedItem;
    Vector3 objectPos;
    float angle;
    float elementTimer;
    const float maxTime = 1.0f;
    bool elementDirection = false;

    private Animator anim;
	private Animator animDonePlayer;//when players health is about 0
    private ItemBar itemBar;
    private Vector2 oneGameO; 
    private Vector2 twoGameobj;
    private Vector2 thirdgameobj;
    private GameObject Void;
    private GameObject shield;
	public Animation animationClip;
	bool playedClipOnce = false;

    protected int threetimes;

	void Start () {

        effects = new List<PlayerEffect>();

        Void = GameObject.Find("Void");
        gameObject.GetComponent<SpriteRenderer>().color = Inventory.varihahmolle;
        rb = GetComponent<Rigidbody2D>();
        
        element = 0;
        	selectedItem = 0;
        anim = GetComponent<Animator>();

		animationClip = GetComponent<Animation>();
        itemBar = itemBarTransform.gameObject.GetComponent<ItemBar>();

		elementWheelPositions();
		ASGO = GameObject.Find("AudioSourceGameObj");
		SoundGO = GameObject.Find("HonkAudioSource");
		if(ASGO)
		{
			AudioSource music = ASGO.GetComponent<AudioSource>();
			if (MusicManager.volumeLevel == 0F) { music.volume = 0.6F;
			} else {
				music.volume = MusicManager.volumeLevel;
			}
			music.Play ();
		}

		if (SoundManager.volumeLevel == 0F)
        {
            SoundManager.volumeLevel = 0.2F;
		}
        elementTimer = 0.0f;
	}
	
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;

        float multiplier = 0.05f;
        float addition = 15.0f;

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            rb.velocity += Vector2.right * (Inventory.nopeus + addition) * multiplier;
            //transform.Translate(Vector2.right * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            rb.velocity += -Vector2.right * (Inventory.nopeus + addition) * multiplier;
            //transform.Translate(-Vector2.right * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            rb.velocity += Vector2.up * (Inventory.nopeus + addition) * multiplier;
            //transform.Translate(Vector2.up * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            rb.velocity += -Vector2.up * (Inventory.nopeus + addition) * multiplier;
            //transform.Translate(-Vector2.up * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }

        Void.transform.Rotate(new Vector3(0.0f, 0, 0.1f));
    }

	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && PlayerHealth.Playermana > 0.5f)
        {
            //create bullet
            //Bulletspawn = GameObject.Find("BulletSpawn").transform;

            Vector2 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - BulletSpawn.position;

            float direction = Mathf.Atan2(delta.y, delta.x) + 3.14159265f * 2.0f;

            GameObject obj = (GameObject)Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            obj.GetComponent<SpriteRenderer>().sprite = bulletSprites[(int)(elementTimer != 0 ? previousElement : element)];
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.direction = direction;
            bullet.element = (elementTimer != 0 ? previousElement : element);
            switch(element)
            {
                case Element.Fire:
                    bullet.damage = Inventory.fireLevel;
                    break;
                case Element.Air:
                    bullet.damage = Inventory.airLevel;
                    break;
                case Element.Ice:
                    bullet.damage = Inventory.iceLevel;
                    break;
            }

            //play sound
			AudioSource honksound = SoundGO.GetComponent<AudioSource>();
			honksound.volume = SoundManager.volumeLevel;
			honksound.PlayOneShot (clips [1], SoundManager.volumeLevel);

            //decrease mana
            PlayerHealth.Playermana -= 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedItem++;
            if (selectedItem > Inventory.inventory.Count - 1)
                selectedItem = 0;
            itemBar.SendMessage("choose", selectedItem);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll < 0)
            {
                if (elementTimer <= 0.0f)
                {
                    previousElement = element;
                    element++;
                    elementTimer = maxTime;
                    elementDirection = true;
                    if ((int)element > 2)
                        element = Element.Fire;
					
					elementWheelPositions ();
                }
            }
            else if (scroll > 0)
            {
                if (elementTimer <= 0.0f)
                {
                    previousElement = element;
                    element--;
                    elementTimer = maxTime;
                    elementDirection = false;
                    if (element < 0)
                        element = Element.Air;
					elementWheelPositions ();
                }
            }
        }
        
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].timer -= Time.deltaTime;
            if (effects[i].timer < 0)
            {
                switch (effects[i].type)
                {
                    case 3: //speed boost
                        Inventory.nopeus -= 5;
                        break;
                }
                effects.RemoveAt(i);
                i--;
            }
            else
            {
                switch (effects[i].type)
                {
                    case 0: //hp regen
                        PlayerHealth.Playerhealth += Time.deltaTime * effects[i].amount; break;
                    case 1: //mana regen
                        PlayerHealth.Playermana += Time.deltaTime * effects[i].amount; break;
                    case 2: //resurrection
                        if (PlayerHealth.Playerhealth <= 0)
                        {
                            PlayerHealth.Playerhealth = Inventory.maxHealth / 2.0f;
                        }
                        break;
                    case 3: // speed boost
                        break;
                }
            }
        }

        if (PlayerHealth.Playerhealth <= 0)
        {
            //TODO: death animation + nest scene after a few seconds?


			//animationTest.Play ();
            Instantiate(playerdeathparticle, transform.position, transform.rotation);
			Debug.Log ("0 kerho tai ei");

			//anim.SetBool ("PlayerDone", false);
			if (playedClipOnce == false) {
				GetComponent<Animator> ().Play ("PlayerFallIdle");
				playedClipOnce = true;
			} else {
				StartCoroutine(odota());
				playedClipOnce = true;

			}
			//anim.runtimeAnimatorController = Resources.Load("Animations/PlayerWalk/PlayerFall") as RuntimeAnimatorController;
			anim.enabled = true;

			//animation["AnimationName"].wrapMode = WrapMode.Once;
			//animation.Play("AnimationName");
            //SceneManager.LoadScene("EndScreenScene");
        }

        if(elementTimer > 0)
        {
            elementTimer -= Time.deltaTime;
            if(elementTimer < 0)
            {
                elementTimer = 0.0f;
            }

            elementWheelPositions();
        }

        //shield
        if (Input.GetMouseButtonDown(1) && PlayerHealth.Playermana > 5)
        {
            shield = Instantiate(shieldPrefab);
        }
        if(Input.GetMouseButton(1) && shield)
        {
            if (PlayerHealth.Playermana > 2.0f * Time.deltaTime)
            {
                PlayerHealth.Playermana -= 3.0f * Time.deltaTime;
                Vector2 direction = (mousePos - transform.position);
                shield.transform.position = transform.position + (Vector3)direction.normalized * 0.5f;
                shield.transform.localEulerAngles = new Vector3(0, 0, 180 * Mathf.Atan2(direction.y, direction.x) / Mathf.PI - 90);
            }
            else
            {
                PlayerHealth.Playermana = 0.0f;
                DestroyObject(shield);
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            DestroyObject(shield);
        }


        anim.SetFloat("MoveX",Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));


    }

	//Following enumerator just change the value of Animation Parameter randomly
	IEnumerator odota()
	{
		yield return new WaitForSeconds(1.0f);
		//This line set the value of Animation Parameter 
		GetComponent<Animator> ().Stop ();

	}

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemy") {
            float damage = 0.25f;
            if (element != other.gameObject.GetComponent<EnemyHealt>().element)
                damage *= 2;
            PlayerHealth.Playerhealth -= damage * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemyBullet")
        {
            float damage = other.gameObject.GetComponent<EnemyBullet>().damage;
            if (element != other.gameObject.GetComponent<EnemyBullet>().element)
                damage *= 2;
            PlayerHealth.Playerhealth -= damage;
            Instantiate(playerhitParticle,transform.position,transform.rotation);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Finish" && Inventory.key)
        {
            GameSceneLevelLoading.levelNumber++;
            if((GameSceneLevelLoading.levelNumber) % 3 == 2)
            {
                SceneManager.LoadScene("ShopScene");
            }
            else
            {
                SceneManager.LoadScene("InterLevelScene");
            }
        }
    }

    void elementWheelPositions()
    {
        float fraction = elementTimer / maxTime;

        float distance = 24.0f;
        Vector2 position0 = Void.GetComponent<RectTransform>().pivot;
        float degrees60 = Mathf.PI / 3 * 2;
        float angle = Mathf.PI / 2 - (int)element * degrees60 + fraction * degrees60 * (elementDirection ? 1 : -1);

        float c;
        if(fraction > 0.5f) { c = (fraction - 0.5f) * 2.0f; }
        else { c = 1.0f - (fraction * 2.0f); }

        Color color = new Color(c, c, c);

        firstSpriteGameobj.GetComponent<Image>().color = color;
        secondSpriteGameobj.GetComponent<Image>().color = color;
        thirdSpriteGameobj.GetComponent<Image>().color = color;
        glowElement.GetComponent<Image>().color = color;

        firstSpriteGameobj.anchoredPosition = position0 + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        angle += degrees60;
        secondSpriteGameobj.anchoredPosition = position0 + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        angle += degrees60;
        thirdSpriteGameobj.anchoredPosition = position0 + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

        if(fraction > 0.5f)
        {
            glowElement.GetComponent<Image>().sprite = Elements[(int)previousElement];
            pointer.SendMessage("setColor", new Vector2((float)previousElement, color.r));
        }
        else
        {
            glowElement.GetComponent<Image>().sprite = Elements[(int)element];
            pointer.SendMessage("setColor", new Vector2((float)element, color.r));
        }
    }
}