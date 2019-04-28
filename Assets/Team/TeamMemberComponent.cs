using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMemberComponent : MonoBehaviour
{
    public TeamObject getTeam() {
        return _team;
    }

    public void setTeam(TeamObject inTeam) {
        _team = inTeam;
    }

    [SerializeField] TeamObject _team = null;
}
