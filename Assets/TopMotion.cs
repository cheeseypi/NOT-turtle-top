using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TopMotion : MonoBehaviour
{
    public float centerPull = 1.7f;
    public float energyLossCoefficient = .88f;
    public float maxVelocity = 20f;
    public bool MeridianPull = true;
    public bool HorizonPull = true;
    public TextMeshProUGUI HealthText;

    public float StartingHealth = 100f;
    public float WallDamageBase = 10f;
    public float TopDamageBase = 20f;
    public float Health { get => _health; }
    public float LastVelocity { get => _lastVelocity.magnitude; }

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private Vector2 _lastVelocity;
    private float _health = 0f;
    private bool died = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _health = StartingHealth;
        if (HealthText != null)
        {
            HealthText.text = "" + roundToDegree(_health, 1);
        }
        if (tag == "Top")
        {
            _rigidBody.velocity = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(HealthText == null)
        {
            HealthText = GameObject.FindGameObjectWithTag("EnemyHealth").GetComponent<TextMeshProUGUI>();
            if (HealthText != null)
            {
                HealthText.text = "" + roundToDegree(_health, 1);
            }
        }
        var centerVectorX = MeridianPull ? -_rigidBody.position.x : 0;
        var centerVectorY = HorizonPull ? -_rigidBody.position.y : 0;
        var toCenter = new Vector2(centerVectorX, centerVectorY);
        _rigidBody.velocity += toCenter / 45 * centerPull;

        if(_rigidBody.velocity.magnitude > maxVelocity)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * maxVelocity;
        }

        _lastVelocity = _rigidBody.velocity;

        HealthText.transform.position = new Vector3(_rigidBody.position.x, _rigidBody.position.y + 1f, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            var dir = Vector2.Reflect(_lastVelocity, normal).normalized;

            //_rigidBody.velocity = Vector2.Reflect(_rigidBody.velocity, normal);
            _rigidBody.velocity = dir * (_lastVelocity.magnitude * energyLossCoefficient);
            _health -= (LastVelocity / maxVelocity) * WallDamageBase;
        }
        else if(collision.gameObject.CompareTag("Top") || collision.gameObject.CompareTag("Player"))
        {
            var otherTopMotion = collision.gameObject.GetComponent<TopMotion>();
            _health -= (otherTopMotion.LastVelocity / otherTopMotion.maxVelocity) * TopDamageBase;
        }
        if (HealthText != null)
        {
            HealthText.text = "" + roundToDegree(_health, 1);
        }
        if (_health <= 0 && !died)
        {
            died = true;
            if (tag == "Top")
            {
                try
                {
                    GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreTrack>().BroadcastMessage("IncreaseScore");
                }
                catch {}
            }
            else if (tag == "Player")
            {
                Destroy(HealthText);
                SceneManager.LoadScene(2);
            }
            Destroy(gameObject);
        }
    }
    private float roundToDegree(float f, int places)
    {
        int num = Mathf.FloorToInt(f * (10 * places));
        return ((float)num) / (10 * places);
    }
}
