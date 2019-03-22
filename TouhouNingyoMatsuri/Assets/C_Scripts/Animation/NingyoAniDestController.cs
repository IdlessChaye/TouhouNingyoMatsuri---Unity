//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// 必要なコンポーネントの列記
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class NingyoAniDestController : NetworkBehaviour {
    public Transform targetTF;
    public float animSpeed = 1.5f;              // アニメーション再生速度設定
    public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                // このスイッチが入っていないとカーブは使われない
    public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

    // 以下キャラクターコントローラ用パラメタ
    // 前進速度
    public float forwardSpeed;
    // 後退速度
    public float backwardSpeed;
    // 旋回速度
    public float rotateSpeed;
    // ジャンプ威力
    public float jumpPower;
    // Jump when target'y is greater than this
    public float jumpLimit;
    public float stopDistance;
    public bool notWork;
    // キャラクターコントローラ（カプセルコライダ）の参照
    private CapsuleCollider col;
    private Rigidbody rb;
    // キャラクターコントローラ（カプセルコライダ）の移動量
    private Vector3 velocity;
    // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;         // base layerで使われる、

    // アニメーター各ステートへの参照
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");

    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate() {
        if(!hasAuthority)
            return;
        if(notWork)
            return;
        if(targetTF == null)
            return;
        if(anim == null) {
            anim = GetComponent<Animator>();
            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            //メインカメラを取得する
            orgColHight = col.height;
            orgVectColCenter = col.center;
        }
        float v, h, u;
        Vector3 deltaPosition = targetTF.position - gameObject.transform.position;
        u = Mathf.Clamp(deltaPosition.y, 0, 10f);
        float vecForward = Vector3.Dot(transform.forward, deltaPosition);
        float vecRight = Vector3.Dot(transform.right, deltaPosition) / deltaPosition.magnitude;
        if(vecForward > 0) {
            v = 1;
            if(Mathf.Abs(vecRight) > 0.3f) {
                if(vecRight > 0) {
                    h = 1;
                } else {
                    h = -1;
                }
            } else {
                h = 0;
            }
        } else {
            v = 0.1f;
            if(Mathf.Abs(vecRight) > 0.3f) { 
                if(vecRight > 0) {
                    h = 1;
                }else {
                    h = -1;
                }
            }else {
                h = 0;
            }
        }
        if(deltaPosition.magnitude < stopDistance) {
            v = 0;
            h = 0;
        }
        //float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
        //float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
        anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
        anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする



        // 以下、キャラクターの移動処理
        velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
                                                // キャラクターのローカル空間での方向に変換
        velocity = transform.TransformDirection(velocity);
        //以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
        if(v > 0.1) {
            velocity *= forwardSpeed;       // 移動速度を掛ける
        } else if(v < -0.1) {
            velocity *= backwardSpeed;  // 移動速度を掛ける
        }

        if(u>jumpLimit) {  // スペースキーを入力したら

            //アニメーションのステートがLocomotionの最中のみジャンプできる
            if(currentBaseState.fullPathHash != idleState) {
                //ステート遷移中でなかったらジャンプできる
                if(!anim.IsInTransition(0)) {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                }
            }
        }


        // 上下のキー入力でキャラクターを移動させる
        transform.localPosition += velocity * Time.fixedDeltaTime;

        // 左右のキー入力でキャラクタをY軸で旋回させる
        transform.Rotate(0, h * rotateSpeed, 0);


        // 以下、Animatorの各ステート中での処理
        // Locomotion中
        // 現在のベースレイヤーがlocoStateの時
        if(currentBaseState.fullPathHash == locoState) {
            //カーブでコライダ調整をしている時は、念のためにリセットする
            if(useCurves) {
                resetCollider();
            }
        }
        // JUMP中の処理
        // 現在のベースレイヤーがjumpStateの時
        else if(currentBaseState.fullPathHash == jumpState) {
            if(!anim.IsInTransition(0)) {

                // 以下、カーブ調整をする場合の処理
                if(useCurves) {
                    // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                    // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
                    // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
                    float jumpHeight = anim.GetFloat("JumpHeight");
                    float gravityControl = anim.GetFloat("GravityControl");
                    if(gravityControl > 0)
                        rb.useGravity = false;  //ジャンプ中の重力の影響を切る

                    // レイキャストをキャラクターのセンターから落とす
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                    RaycastHit hitInfo = new RaycastHit();
                    // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                    if(Physics.Raycast(ray, out hitInfo)) {
                        if(hitInfo.distance > useCurvesHeight) {
                            col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
                            float adjCenterY = orgVectColCenter.y + jumpHeight;
                            col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                        } else {
                            // 閾値よりも低い時には初期値に戻す（念のため）					
                            resetCollider();
                        }
                    }
                }
                // Jump bool値をリセットする（ループしないようにする）				
                anim.SetBool("Jump", false);
            }
        }
        // IDLE中の処理
        // 現在のベースレイヤーがidleStateの時
        else if(currentBaseState.fullPathHash == idleState) {
            //カーブでコライダ調整をしている時は、念のためにリセットする
            if(useCurves) {
                resetCollider();
            }
            // スペースキーを入力したらRest状態になる
            if(h==0 && v==0) {
                anim.SetBool("Rest", true);
            }
        }
        // REST中の処理
        // 現在のベースレイヤーがrestStateの時
        else if(currentBaseState.fullPathHash == restState) {
            if(!anim.IsInTransition(0)) {
                anim.SetBool("Rest", false);
            }
        }
    }

    void SetTargetTF(Transform tf) {
        targetTF = tf;
    }

    public void DontWorkForTime(float time) {
        if(!hasAuthority)
            return;
        //print(gameObject.GetComponent<NetworkIdentity>().netId+"IsResting!"); 
        notWork = true;
        if(anim == null)
            return;
        anim.applyRootMotion = false;
        anim.enabled = false;
        Invoke("SetNotWorkTrue", time);
    }
    void SetNotWorkTrue() {
        //print(gameObject.GetComponent<NetworkIdentity>().netId + "Work!!");
        anim.enabled = true;
        anim.applyRootMotion = true;
        notWork = false;
    }

    void resetCollider() {
        // コンポーネントのHeight、Centerの初期値を戻す
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }
}
