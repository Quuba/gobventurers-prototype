using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float baseMovementSpeed = 4f;
        [SerializeField] private float dashCooldown = 0.8f;
        [SerializeField] private float dashLength = 2f;
        
        private float _dashTimer = 0f;
        private float _inputX = 0f, _inputV = 0f;

        private Rigidbody _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: find out why using normal GetAxis here gives floaty movement 
            _inputX = Input.GetAxisRaw("Horizontal");
            _inputV = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) && _dashTimer <=0)
            {
                Dash();
            }

            if (_dashTimer > 0) _dashTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            //this guy (https://youtu.be/ixM2W2tPn6c) says it's a good method and he's right
            var direction = new Vector3(_inputX, 0f, _inputV).normalized * baseMovementSpeed;
            _rb.MovePosition(transform.position + (baseMovementSpeed * Time.deltaTime * direction));
        }

        private void Dash()
        {
            Debug.Log("dash");
            var dashDir = new Vector3(_inputX, 0f, _inputV).normalized * dashLength;
            _rb.velocity = dashDir;
            _dashTimer = dashCooldown;
        }
    }
}