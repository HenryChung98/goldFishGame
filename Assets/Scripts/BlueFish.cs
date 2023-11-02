using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueFish : MonoBehaviour
{
    [SerializeField] 
    private Slider blueFishHPBar;
    [SerializeField] 
    private GameObject attackOne;
    [SerializeField] 
    private GameObject attackTwo;

    //-----------for move sin-----------//
    private float verticalSpeed = 1f;
    private float minY = -3.0f;
    private float maxY = 3.0f;

    private float bossAttackMin = -2f;
    private float bossAttackMax = 2f;



    //-----------common objects-----------//
    public float hp = 1f;
    [SerializeField] 
    private float moveSpeed = 5f;
    [SerializeField] 
    private GameObject crystal;
    private Color originalColor;
    private Color getRed = new Color(255, 0, 0, 0.3f);
    private Color frozen = new Color(0, 255, 255, 0.3f);
    private SpriteRenderer spriteRenderer;
    private bool isMovementEnabled;
    void Start()
    {
        //-----------common objects-----------//
        isMovementEnabled = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;


        StartCoroutine(attack1());
        StartCoroutine(attack2());
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovementEnabled){
        if (transform.position.x > 8){
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        float verticalMovement = Mathf.Sin(Time.time) * verticalSpeed;
        transform.position += Vector3.up * verticalMovement * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
        }
    }


    //-----------Attack methods-----------//

        private IEnumerator attack1(){
            while (true){
                float randomAttack = Random.Range(bossAttackMin, bossAttackMax);
                Vector3 attackPos = new Vector3(transform.position.x, transform.position.y + randomAttack, transform.position.z);
                Instantiate(attackOne, attackPos, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                
                }
            }

        private IEnumerator attack2(){
            while (true){
                
                for (int i = 0; i <= 10; i++){
                float randomAttack = Random.Range(0, 12);
                Vector3 attackPos = new Vector3(randomAttack, 7, transform.position.z);
                Instantiate(attackTwo, attackPos, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(5f);
                }
            }


    //-----------common objects-----------//
    private void displayBar(){
        spriteRenderer.color = getRed;
        RectTransform hpBarSize = blueFishHPBar.GetComponent<RectTransform>();
        float forBar = hp * (30f/7f);
        hpBarSize.offsetMax = new Vector2(-forBar, hpBarSize.offsetMax.y);
        
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Bullet")){
            hp += GameManager.instance.damage;
            displayBar();
            // critical hit
            int randomNum = Random.Range(1, 101);
            if (randomNum <= GameManager.instance.criticalPercent){
                hp += GameManager.instance.damage * 2;
                Debug.Log("Critical!");
                displayBar();
            }
            StartCoroutine(becomeOriginalColor());
            Destroy(other.gameObject);
            }

        if (other.gameObject.CompareTag("Spark")){
            hp += GameManager.instance.damage / 2;
            displayBar();
            // critical hit
            int randomNum = Random.Range(1, 101);
            if (randomNum <= GameManager.instance.criticalPercent){
                hp += GameManager.instance.damage;
                Debug.Log("Critical!");
                displayBar();
            }
            StartCoroutine(becomeOriginalColor());
        }
        if (other.gameObject.CompareTag("Meteor")){
            hp += GameManager.instance.damage / 3;
            displayBar();
            
            // critical hit
            int randomNum = Random.Range(1, 101);
            if (randomNum <= GameManager.instance.criticalPercent){
                hp += (GameManager.instance.damage / 3) * 2;
                Debug.Log("Critical!");
                displayBar();
            }
            StartCoroutine(becomeOriginalColor());
        }
        if (other.gameObject.CompareTag("ShadowBullet")){
            hp += GameManager.instance.damage / 5;
            displayBar();
            
            // critical hit
            int randomNum = Random.Range(1, 101);
            if (randomNum <= GameManager.instance.criticalPercent){
                hp += (GameManager.instance.damage / 5) * 2;
                Debug.Log("Critical!");
                displayBar();
            }
            StartCoroutine(becomeOriginalColor());
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Ice")){
                spriteRenderer.color = frozen;
                isMovementEnabled = false;
                StartCoroutine(unfrozen());
            }

            if (hp >= 350){ // here is boss max hp
                Destroy(gameObject);
                moveSpeed = 1f;
                EnemySpawner enemySpawner = gameObject.GetComponent<EnemySpawner>();

                Vector3 newPos1 = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                Instantiate(crystal, newPos1, Quaternion.identity);
                Vector3 newPos2 = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
                Instantiate(crystal, newPos2, Quaternion.identity);
                Vector3 newPos3 = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(crystal, newPos3, Quaternion.identity);
                Vector3 newPos4 = new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z);
                Instantiate(crystal, newPos4, Quaternion.identity);
                Vector3 newPos5 = new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z);
                Instantiate(crystal, newPos5, Quaternion.identity);
                
                // get score
                for (int i = 0; i <= 10; i++){
                    GameManager.instance.getScore();
                }

                // life steal
                if (GameManager.instance.isLifeSteal == true){
                    GameManager.instance.hp -= 5;
                    GameManager.instance.GetDamage();
                }
                }
        }
        private IEnumerator becomeOriginalColor(){
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.color = originalColor;
            }

        
        private IEnumerator unfrozen(){
        yield return new WaitForSeconds(2f);
        spriteRenderer.color = originalColor;
        isMovementEnabled = true;
    }
}
