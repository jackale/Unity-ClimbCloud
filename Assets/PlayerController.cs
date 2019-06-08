using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    float jumpForce = 780.0f;
    float walkFroce = 30.0f;
    float maxWalkSpeed = 2.0f;
    Animator animator;
    float threshold = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプ
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        if (Input.GetMouseButtonDown(0)
            && Mathf.Approximately(this.rigid2D.velocity.y, 0f))
#else
        if (Input.GetKeyDown(KeyCode.Space)
          && Mathf.Approximately(this.rigid2D.velocity.y, 0f))
#endif
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        // 左右移動
        int key = 0;
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        if (Input.acceleration.x > this.threshold) key = 1;
        if (Input.acceleration.x < -this.threshold) key = -1;
#else
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;
#endif

        // 無限に加速しないように上限値で留める
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        if (speedx < maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkFroce);
        }

        // 動く方向に応じて反転（追加）
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }

        // プレイヤの速度に応じてアニメーション速度を変える
        if (Mathf.Approximately(this.rigid2D.velocity.y, 0f))
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Clear");
        SceneManager.LoadScene("ClearScene");
    }
}
