using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform HorizontalPivot;
    public Transform VerticalPivot;
    [SerializeField] private float _moveSpeed = 0.3f;
    [SerializeField] private float _rotateSpeed = 0.3f;
    private float maxX;
    private float minX;
    private float maxZ;
    private float minZ;

    private float? mouseX;
    private float? mouseY;
    private float rotateTimer;

    private Transform activePlayer;
    // Start is called before the first frame update
    void Start()
    {
        var tails = FindObjectsOfType<Tail>();
        minX = tails.Min(x => x.transform.position.x);
        minZ = tails.Min(x => x.transform.position.z);
        maxX = tails.Max(x => x.transform.position.x);
        maxZ = tails.Max(x => x.transform.position.z);

        MatchSystem.instance.OnChangePlayer += MatchSystem_OnChangePlayer;
    }

    private void MatchSystem_OnChangePlayer(CreatureStats creature)
    {
        if (Vector3.Distance(transform.position,creature.transform.position) > 4)
        {
            activePlayer = creature.transform;
        } else
        {
            activePlayer = null;
        }
        

        /*if (creature.GetComponent<AIScript>().isActiveAndEnabled)
        {
            activePlayer = creature.transform;
        } else
        {
            activePlayer = null;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        /*if (Input.GetMouseButton(1))
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer >= Time.deltaTime * 10)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                RotateCamera();
            }
        }
        else
        {
            MoveCamera();
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            rotateTimer = 0;
        }*/
    }

    void RotateCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if (x != 0)
        {
            HorizontalPivot.Rotate(0, x * _rotateSpeed, 0);
        }
        
        if (y != 0)
        {
            float rotate = Mathf.Clamp(VerticalPivot.localEulerAngles.x - y * _rotateSpeed,10,80);
            VerticalPivot.localEulerAngles = new Vector3(rotate, 0, 0);
        }
        
    }

    void MoveCamera()
    {
        Vector3 moveVector = Vector3.zero;

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        if (Input.GetKey(KeyCode.LeftArrow) || mouseX < 10)
        {
            moveVector.x = -_moveSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || mouseX > (Screen.width - 10))
        {
            moveVector.x = _moveSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow) || mouseY > (Screen.height - 10))
        {
            moveVector.z = _moveSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow) || mouseY < 10)
        {
            moveVector.z = -_moveSpeed;
        }

        if (moveVector == Vector3.zero)
        {
            if (activePlayer != null)
            {
                transform.position = Vector3.Lerp(transform.position, activePlayer.position, 3f * Time.deltaTime);
            }
        }
        else
        {
            activePlayer = null;
            transform.Translate(moveVector * Time.deltaTime, HorizontalPivot);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minZ, maxZ));
    }
}
