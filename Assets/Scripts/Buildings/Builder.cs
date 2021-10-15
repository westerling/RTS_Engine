﻿using Mirror;
using System;
using UnityEngine;

public class Builder : BaseUnitClickAction
{
    [SyncVar]
    private Building m_Target;

    public Building Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }


    public void AddListener()
    {
        Building.AuthorityOnConstructionStarted += AuthorityHandleBuildingsSpawned;
    }

    public void RemoveListener()
    {
        Building.AuthorityOnConstructionStarted -= AuthorityHandleBuildingsSpawned;
    }

    private void AuthorityHandleBuildingsSpawned(Building building)
    {
        CmdSetTarget(building);
    }

    public void FindNewTarget()
    {
        var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        var constructions = player.Constructions;

        if (constructions.Count == 0)
        {
            return;
        }

        var closestConstruction = constructions[0];
        var distance = Vector3.Distance(closestConstruction.gameObject.transform.position, transform.position);

        foreach (var construction in constructions)
        {
            var newDistance = Vector3.Distance(construction.gameObject.transform.position, transform.position);

            if (newDistance < distance)
            {
                closestConstruction = construction;
                distance = newDistance;
            }
        }

        Target = closestConstruction;
    }

    #region server

    [Command]
    public void CmdSetTarget(Building targetBuilding)
    {
        Target = targetBuilding;
    }

    [Command]
    public void CmdClearTarget()
    {
        ClearTarget();
    }

    [Server]
    public override void ClearTarget()
    {
        Target = null;
    }

    public override void UpdateStats()
    {
        return;
    }

    #endregion
}
