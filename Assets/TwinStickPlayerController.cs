using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwinStickPlayerController : MonoBehaviour
{
    public float movementSpeed = .06f;
    public float brakeForce = .1f;
    public float dashCooldown = 3f;
    public TextMeshProUGUI DashText;
    public AudioSource DashAudioSource;
    public AudioClip Dash;
    public AudioClip DashReplenish;
    public AudioClip WallHit;
    public AudioClip EnemyHit;

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private TopMotion _topMotionComp;
    private Vector2 _lastVelocity;

    private bool _braking = false;
    private Vector2 _motion = new Vector2(0, 0);
    private Vector2? _shootDirection = null;
    private bool _canDash = true;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _topMotionComp = GetComponent<TopMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidBody.velocity += _motion * movementSpeed;
        if (_braking)
        {
            if (_rigidBody.velocity.magnitude > 1)
            {
                _rigidBody.velocity -= _rigidBody.velocity.normalized * brakeForce;
            }
            else
            {
                _rigidBody.velocity -= _rigidBody.velocity * brakeForce;
            }
        }
        if (_shootDirection.HasValue && _canDash)
        {
            DashAudioSource.PlayOneShot(Dash);
            _canDash = false;
            DashText.color = Color.red;
            _rigidBody.velocity = _shootDirection.Value.normalized * ((_rigidBody.velocity * _shootDirection.Value.normalized).magnitude + _topMotionComp.maxVelocity) / 2;
            StartCoroutine(ResetDash());
        }

        DashText.transform.position = new Vector3(_rigidBody.position.x, _rigidBody.position.y + .63f, 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _motion = (Vector2) context.action.ReadValueAsObject();
        Debug.Log("MOVE: " + _motion + "\n" + context);
    }
    public void Brake(InputAction.CallbackContext context)
    {
        _braking = context.performed;
        Debug.Log("BRAKE: " + context.performed);
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        _shootDirection = ((Vector2)context.action.ReadValueAsObject()).normalized;
        if (context.canceled || _shootDirection.Value.magnitude == 0)
            _shootDirection = null;
        else
        Debug.Log("SHOOT: " + _shootDirection);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            DashAudioSource.PlayOneShot(WallHit);
        }
        else if(collision.gameObject.CompareTag("Top"))
        {
            DashAudioSource.PlayOneShot(EnemyHit);
        }
    }

    private IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
        DashText.color = Color.green;
        DashAudioSource.PlayOneShot(DashReplenish);
    }
}
