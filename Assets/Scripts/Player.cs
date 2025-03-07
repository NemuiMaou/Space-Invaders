using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;
    public float speed = 5f;

    public Transform shottingOffset;
    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        float direction = Input.GetAxis("Horizontal");
        position += new Vector3(direction, 0f, 0f) * (speed * Time.deltaTime);
        
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        transform.position = position;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
            Debug.Log("Bang!");

            Destroy(shot, 3f);

        }
    }
}