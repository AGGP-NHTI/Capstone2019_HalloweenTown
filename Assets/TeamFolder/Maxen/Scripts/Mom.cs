﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS CLASS WILL BE MADE RESPONSIBLE FOR MUCH LESS ONCE WE GET A REAL MOM THAT HAS AI
public class Mom : MonoBehaviour
{
    public bool huntChildren = false;

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
        if(huntChildren)
        {
            //Capture targetPlayer if they are within range
            float targetSqrDistance = (targetPlayer.transform.position - transform.position).sqrMagnitude;
            if (targetSqrDistance < collectPlayerRange * collectPlayerRange)
            {
                RemainingPlayersToFind.Remove(targetPlayer);
                Destroy(targetPlayer);
                targetPlayer = FindNewTarget(RemainingPlayersToFind);
                if(!targetPlayer)
                {
                    huntChildren = false;
                }
            }

            //Figure out if players are nearby
            Collider[] nearbyPlayers = Physics.OverlapSphere(transform.position, reconsiderTargetRange, PlayerLayerMask);
            List<Pawn> nearbyPawns = new List<Pawn>();
            foreach (Collider c in nearbyPlayers)
            {
                Pawn foundPawn = c.GetComponent<Pawn>();
                if (foundPawn)
                {
                    nearbyPawns.Add(foundPawn);
                }
            }
            Pawn newTargetCandidate = FindNewTarget(nearbyPawns);

            //Change targetPlayer if necessary
            if (newTargetCandidate && newTargetCandidate != targetPlayer)
            {
                targetPlayer = newTargetCandidate;
            }
        }
    }

    public Pawn FindNewTarget(List<Pawn> possiblePawns)
    {
        Pawn newTargetCandidate = null;
        int targetCandidateCandyCount = 0;
        foreach (Pawn p in possiblePawns)
        {
            //Figure out the richest nearby player, and select them as the new target. If there's a tie, the nearest player gets picked.
            if (!newTargetCandidate || p.MyCandy.candy > newTargetCandidate.MyCandy.candy)
            {
                newTargetCandidate = p;
                targetCandidateCandyCount = p.MyCandy.candy;
            }
            else if (newTargetCandidate && p.MyCandy.candy == newTargetCandidate.MyCandy.candy)
            {
                float foundPawnSqrDistance = (p.transform.position - transform.position).sqrMagnitude;
                float targetCandidateSqrDistance = (newTargetCandidate.transform.position - transform.position).sqrMagnitude;
                if (foundPawnSqrDistance < targetCandidateCandyCount)
                {
                    newTargetCandidate = p;
                }
            }
        }

        return newTargetCandidate;
    }
}
