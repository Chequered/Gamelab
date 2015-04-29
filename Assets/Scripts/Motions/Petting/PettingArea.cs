﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PettingArea : MonoBehaviour {

    public float pettingMultiplier = 1f;

    private Vector3 _enterPosition = Vector3.zero;
    private Vector3 _exitPosition = Vector3.zero;
	private float _startingTime;
	private float _endTime;

    private void OnDrawGizmos()
    {
        if (_enterPosition != Vector3.zero && _exitPosition != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_enterPosition, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_exitPosition, 0.5f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_enterPosition, _exitPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Palm")
        {
            Debug.Log("Enter");
			_startingTime = Time.time;
            GameObject collisionPoint = new GameObject("Collision Point");
            collisionPoint.transform.position = other.transform.position;
            collisionPoint.transform.parent = transform;
            _enterPosition = collisionPoint.transform.position;
            Destroy(collisionPoint);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Palm")
        {
            Debug.Log("Exit");
			_endTime = Time.time;
            GameObject collisionPoint = new GameObject("Collision Point");
            collisionPoint.transform.position = other.transform.position;
            collisionPoint.transform.parent = transform;
            _exitPosition = collisionPoint.transform.position;
            Destroy(collisionPoint);

            CalculatePettingValue();
        }
    }

    private void CalculatePettingValue()
    {
        float distance = Vector3.Distance(_enterPosition, _exitPosition);
        float pettingValue = distance * pettingMultiplier;
		float speed = _startingTime - _endTime;
		Mood petMood = GameObject.FindGameObjectWithTag("Cat").GetComponent<Mood>();
		if(speed > 0.5)
		{
			petMood.happyFeeling += pettingValue;
	        Debug.Log(GameObject.FindGameObjectWithTag("Cat").GetComponent<Mood>().happyFeeling);
		} 
		else 
		{
			petMood.ChangeMood(Moods.Annoyed);
			petMood.angryFeeling += pettingValue;
		}
    }
}
