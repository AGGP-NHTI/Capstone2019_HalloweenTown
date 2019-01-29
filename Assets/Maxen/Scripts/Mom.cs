using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS CLASS WILL BE MADE RESPONSIBLE FOR MUCH LESS ONCE WE GET A REAL MOM THAT HAS AI
public class Mom : MonoBehaviour
{
    public List<Pawn> RemainingPlayersToFind;
    public Pawn targetPlayer;
    public float reconsiderTargetRange = 7.0f;
    public float collectPlayerRange = 2.0f;
    public LayerMask PlayerLayerMask;

	public virtual void FindPlayers(List<PlayerController> listOfPlayers)
    {
        foreach(PlayerController pc in listOfPlayers)
        {
            //find out if the controlled pawn is a player in the world or a spectator
            if(pc.ControlledPawn)
            {
                RemainingPlayersToFind.Add(pc.ControlledPawn);
            }
        }
    }

    private void Update()
    {
        //Capture targetPlayer if they are within range
        float targetSqrDistance = (targetPlayer.transform.position - transform.position).sqrMagnitude;
        if(targetSqrDistance < collectPlayerRange * collectPlayerRange)
        {
            RemainingPlayersToFind.Remove(targetPlayer);
            Destroy(targetPlayer);

        }

        //Figure out if players are nearby
        Collider[] nearbyPlayers = Physics.OverlapSphere(transform.position, reconsiderTargetRange, PlayerLayerMask);
        Pawn newTargetCandidate = null;
        int targetCandidateCandyCount = 0;
        foreach(Collider c in nearbyPlayers)
        {
            Pawn foundPawn = c.GetComponent<Pawn>();
            if(foundPawn)
            {
                //Figure out the richest nearby player, and select them as the new target. If there's a tie, the nearest player gets picked.
                if (!newTargetCandidate || foundPawn.MyCandy.candy > newTargetCandidate.MyCandy.candy)
                {
                    newTargetCandidate = foundPawn;
                    targetCandidateCandyCount = foundPawn.MyCandy.candy;
                }
                else if(newTargetCandidate && foundPawn.MyCandy.candy == newTargetCandidate.MyCandy.candy)
                {
                    float foundPawnSqrDistance = (foundPawn.transform.position - transform.position).sqrMagnitude;
                    float targetCandidateSqrDistance = (newTargetCandidate.transform.position - transform.position).sqrMagnitude;
                    if(foundPawnSqrDistance < targetCandidateCandyCount)
                    {
                        newTargetCandidate = foundPawn;
                    }
                }
            }
        }

        //Change targetPlayer if necessary
        if(newTargetCandidate && newTargetCandidate != targetPlayer)
        {
            targetPlayer = newTargetCandidate;
        }
    }

    //public void FindNew
}
