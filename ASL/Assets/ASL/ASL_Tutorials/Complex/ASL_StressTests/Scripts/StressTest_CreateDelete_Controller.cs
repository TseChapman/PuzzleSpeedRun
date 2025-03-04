﻿using UnityEngine;

namespace StressTesting
{
    /// <summary>Randomly creates, moves, and deletes objects, testing to see if participants will stay in sync</summary>
    public class StressTest_CreateDelete_Controller : MonoBehaviour
    {
        private float moveTimer = 0;
        private float creationTimer = 0;
        private float deletionTimer = 0;
        private float randomMoveTime = 0;
        private float randomCreationTime = 0;
        private float randomDeletionTime = 0;

        private void Update()
        {          
            if (creationTimer > randomCreationTime) //Create an object in a random location
            {
                SpawnObject();
            }
            if (moveTimer > randomMoveTime) //Move a random object
            {
                MoveObject();
            }
            if (deletionTimer > randomDeletionTime) //Delete a random object
            {
                DeleteObject();
            }

            creationTimer += Time.deltaTime * 1000; //Timer in milliseconds
            moveTimer += Time.deltaTime * 1000; //Timer in milliseconds
            deletionTimer += Time.deltaTime * 1000; //Timer in milliseconds            
        }


        /// <summary>
        /// Get a random vector for spawning and moving objects
        /// </summary>
        /// <returns>A random vector between -5 and 5 for X and Z, and 0 and 3 for Y</returns>
        private Vector3 GetRandomVector()
        {
            return new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(0.0f, 3.0f), Random.Range(-5.0f, 5.0f));
        }

        /// <summary>
        /// Spawn an object
        /// </summary>
        private void SpawnObject()
        {
            //Instantiate an object at a random position with no parent and a canceled claim recovery function attached to it
            ASL.ASLHelper.InstantiateASLObject(PrimitiveType.Cube, GetRandomVector(),
                Quaternion.identity, "", "", null, CanceledClaimRecovery);

            randomCreationTime = Random.Range(1500, 2500); //Chose a new random time to create the next object
            creationTimer = 0; //Reset the creation timer
        }

        /// <summary>
        /// Move a random ASL object
        /// </summary>
        private void MoveObject()
        {
            //Since we don't keep track of the amount of ASL objects in the scene, we need to find all of them so we know which ones we can move
            var aslObjects = FindObjectsOfType<ASL.ASLObject>(); //Warning: Getting objects this way is slow
            if (aslObjects.Length > 0) //If there is an ASL object to move
            {
                int objectNumber = Random.Range(0, aslObjects.Length); //Randomly grab an ASL object
                aslObjects[objectNumber].SendAndSetClaim(() =>
                {
                    aslObjects[objectNumber].SendAndSetLocalPosition(GetRandomVector()); //Once claimed, move this object
                });
            }
            randomMoveTime = Random.Range(1000, 2000); //Chose a new random time to move another random ASL object
            moveTimer = 0; //Reset move timer
        }

        /// <summary>
        /// Delete a random ASL object
        /// </summary>
        private void DeleteObject()
        {
            //Since we don't keep track of the amount of ASL objects in the scene, we need to find all of them so we know which ones we can move
            var aslObjects = FindObjectsOfType<ASL.ASLObject>(); //Warning: Getting objects this way is slow
            if (aslObjects.Length > 0) //If there is an ASL object to move
            {
                int objectNumber = Random.Range(0, aslObjects.Length); //Randomly grab an ASL object
                aslObjects[objectNumber].SendAndSetClaim(() =>
                {
                    aslObjects[objectNumber].DeleteObject(); //Once claimed, delete this object
                });
            }
            randomDeletionTime = Random.Range(1500, 2400); //Chose a new random time to delete another random ASL object
            deletionTimer = 0; //Reset deletion timer
        }

        /// <summary>
        /// A claim recovery function for objects that get their claims canceled
        /// </summary>
        /// <param name="_id">The id of the object who's claim got rejected</param>
        /// <param name="_cancelledCallbacks">How many claim callbacks got canceled</param>
        public static void CanceledClaimRecovery(string _id, int _cancelledCallbacks)
        {
            Debug.LogWarning("We are going to cancel " + _cancelledCallbacks +
                " callbacks generated by a claim for object: " + _id + " rather than try to recover.");
        }
    }
}