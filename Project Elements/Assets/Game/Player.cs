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
	Vector3 mousePos;
	public Transform BulletSpawn; //the object you want to rotate
    public GameObject BulletPrefab;
    public Transform itemBarTransform;

	Vector3 objectPos;
	float angle;

    private Animator anim;
    private ItemBar itemBar;

    public GameObject playerdeathparticle;
    public GameObject playerhitParticle;

    public Sprite[] bulletSprites;

    public List<PlayerEffect> effects;

    Rigidbody2D rb;
    Element element;
    int selectedItem;

	public RectTransform firstSpriteGameobj = null; //asign it via inspector.
	public RectTransform secondSpriteGameobj = null;
	public RectTransform thirdSpriteGameobj = null;
	public GameObject glowElement;
	public Sprite[] Elements;
	private Vector2 oneGameO; 
	private Vector2 twoGameobj;
	private Vector2 thirdgameobj;
    private GameObject Void;
	protected int threetimes;
	public static GameObject ASGO;

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
		if(ASGO)
		{
			AudioSource music = ASGO.GetComponent<AudioSource>();
			if (MusicManager.volumeLevel == 0F) { music.volume = 0.6F;
			} else {
				music.volume = MusicManager.volumeLevel;
			}
			music.Play ();
		}
	}

	void elementWheelPositions()
	{
		float distance = 24.0f;
		Vector2 position0 = Void.GetComponent<RectTransform>().pivot;
		float angle = Mathf.PI / 2 - (int)element * (Mathf.PI / 3 * 2);
		firstSpriteGameobj.anchoredPosition = position0 + new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * distance;
		angle += Mathf.PI / 3 * 2;
		secondSpriteGameobj.anchoredPosition = position0 + new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * distance;
		angle += Mathf.PI / 3 * 2;
		thirdSpriteGameobj.anchoredPosition = position0 + new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * distance;

		glowElement.GetComponent<Image>().sprite = Elements[(int)element];
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
            obj.GetComponent<SpriteRenderer>().sprite = bulletSprites[(int)element];
            obj.GetComponent<Bullet>().element = element;
            
            PlayerHealth.Playermana -= 0.1f;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {



            if (scroll < 0)
            {
                if(Input.GetKey(KeyCode.E))
                {
                    selectedItem++;
                    if (selectedItem > Inventory.inventory.Count - 1)
                        selectedItem = 0;
                    itemBar.SendMessage("choose", selectedItem);
                }
                else
                {
                    element++;
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
                else
                {
                    element--;
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
}