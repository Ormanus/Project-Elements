using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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


	void Start () {
		
        gameObject.GetComponent<SpriteRenderer>().color = Inventory.varihahmolle;
        rb = GetComponent<Rigidbody2D>();
        
        element = 0;
        selectedItem = 0;
        anim = GetComponent<Animator>();

        itemBar = itemBarTransform.gameObject.GetComponent<ItemBar>();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rb.velocity = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            transform.Translate(Vector2.right * Time.deltaTime * Inventory.nopeus * 5.0f, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Translate(-Vector2.right * Time.deltaTime * Inventory.nopeus * 5.0f, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            transform.Translate(Vector2.up * Time.deltaTime * Inventory.nopeus * 5.0f, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(-Vector2.up * Time.deltaTime * Inventory.nopeus * 5.0f, Space.World);
        }

        if (Input.GetMouseButtonDown(0) && PlayerHealth.Playermana > 0.1f)
        {
            GameObject obj = (GameObject)Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            obj.GetComponent<SpriteRenderer>().sprite = bulletSprites[(int)element];
            obj.GetComponent<Bullet>().element = element;
            GameObject ASGO = GameObject.Find("AudioSourceGameObj");
            if(ASGO)
            {
                AudioSource music = ASGO.GetComponent<AudioSource>();
            }
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
                    //TODO: update element wheel
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
                    //TODO: update element wheel
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
    }
}