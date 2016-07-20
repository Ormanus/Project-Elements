using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	protected int threetimes;
	public static GameObject ASGO;
	void Start () {
		
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
		Vector2 position0 = GameObject.Find ("Void").GetComponent<RectTransform> ().pivot;
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
            rb.velocity = Vector2.right * Inventory.nopeus / 10f;
            //transform.Translate(Vector2.right * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            rb.velocity = -Vector2.right * Inventory.nopeus / 10f;
            //transform.Translate(-Vector2.right * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            rb.velocity = Vector2.up * Inventory.nopeus / 10f;
            //transform.Translate(Vector2.up * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            rb.velocity = -Vector2.up * Inventory.nopeus / 10f;
            //transform.Translate(-Vector2.up * Time.deltaTime * Inventory.nopeus / 10, Space.World);
        }
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