using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Sprite[] Elements;

    Vector3 mousePos;
    Rigidbody2D rb;
    Element element;
    Element previousElement;
    int selectedItem;
    Vector3 objectPos;
    float angle;
    float elementTimer;
    const float maxTime = 2.0f;
    bool elementDirection = false;

    private Animator anim;
    private ItemBar itemBar;
    private Vector2 oneGameO; 
    private Vector2 twoGameobj;
    private Vector2 thirdgameobj;
    private GameObject Void;

    protected int threetimes;

	void Start () {

        effects = new List<PlayerEffect>();

        Void = GameObject.Find("Void");
        gameObject.GetComponent<SpriteRenderer>().color = Inventory.varihahmolle;
        rb = GetComponent<Rigidbody2D>();
        
        element = 0;
        	selectedItem = 0;
        anim = GetComponent<Animator>();

        itemBar = itemBarTransform.gameObject.GetComponent<ItemBar>();

		elementWheelPositions ();
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

		if (SoundManager.volumeLevel == 0F) { SoundManager.volumeLevel = 0.2F;
		}
	}

        elementTimer = 0.0f;
	}
	
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            rb.velocity += Vector2.right * Inventory.nopeus / 10f;
            //transform.Translate(Vector2.right * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            rb.velocity += -Vector2.right * Inventory.nopeus / 10f;
            //transform.Translate(-Vector2.right * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            rb.velocity += Vector2.up * Inventory.nopeus / 10f;
            //transform.Translate(Vector2.up * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            rb.velocity += -Vector2.up * Inventory.nopeus / 10f;
            //transform.Translate(-Vector2.up * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }

        Void.transform.Rotate(new Vector3(0.0f, 0, 0.1f));
    }

	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && PlayerHealth.Playermana > 0.1f)
        {
            GameObject obj = (GameObject)Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            obj.GetComponent<SpriteRenderer>().sprite = bulletSprites[(int)(elementTimer != 0 ? previousElement : element)];
            obj.GetComponent<Bullet>().element = (elementTimer != 0 ? previousElement : element);
			AudioSource honksound = SoundGO.GetComponent<AudioSource>();
			honksound.volume = SoundManager.volumeLevel;

			//AudioSource.PlayClipAtPoint (clips [0], transform.position);

			//			Debug.Log (SoundManager.volumeLevel + "pöö");


			honksound.PlayOneShot (clips [1], SoundManager.volumeLevel);
            PlayerHealth.Playermana -= 0.5f;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll < 0)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    selectedItem++;
                    if (selectedItem > Inventory.inventory.Count - 1)
                        selectedItem = 0;
                    itemBar.SendMessage("choose", selectedItem);
                }
                else if (elementTimer <= 0.0f)
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
                if (Input.GetKey(KeyCode.E))
                {
                    selectedItem--;
                    if (selectedItem < 0)
                        selectedItem = Inventory.inventory.Count - 1;
                    itemBar.SendMessage("choose", selectedItem);
                }
                else if (elementTimer <= 0.0f)
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

            Instantiate(playerdeathparticle, transform.position, transform.rotation);
            
            SceneManager.LoadScene("EndScreenScene");
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


        anim.SetFloat("MoveX",Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));


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
            float damage = 0.25f;
            if (element != other.gameObject.GetComponent<EnemyBullet>().element)
                damage *= 2;
            PlayerHealth.Playerhealth -= damage;
            Instantiate(playerhitParticle,transform.position,transform.rotation);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("ShopScene");
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
        }
        else
        {
            glowElement.GetComponent<Image>().sprite = Elements[(int)element];
        }
    }
}