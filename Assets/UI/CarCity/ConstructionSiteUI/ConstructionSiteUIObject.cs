using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSiteUIObject : MonoBehaviour
{
    //Settings
    public LimitedValueStripeIndicatorObject ProgressIndicator = null;
    public WorkersAssignemntControlObject WorkersAssignemntControl = null;

    //Methods
    //-API
    public void init(CarCityObject inCarCity, ConstructionSiteObject inConstructionSite) {
        _carCity = XUtils.verify(inCarCity);
        _constructionSite = XUtils.verify(inConstructionSite);
    }

    //-Implementation
    private void Awake() {
        XUtils.check(ProgressIndicator);
        XUtils.check(WorkersAssignemntControl);

        WorkersAssignemntControl.onPressedAssignWorker += ()=>{
            CrewMember theCrewMemberToAssign = _carCity.getFirstFreeCrewMember();
            if (null == theCrewMemberToAssign) return;

            _constructionSite.assignWorker(
                _carCity.startFreeCrewMemberAssignment(theCrewMemberToAssign)
            );
        };

        WorkersAssignemntControl.onPressedWithdrawWorker += () => {
            CrewMember theCrewMemberToWithdraw = _constructionSite.getFirstWorker();
            if (null == theCrewMemberToWithdraw) return;

            _carCity.withdrawWorkerAsFree(
                _constructionSite.startWithdrawWorker(theCrewMemberToWithdraw)
            );
        };
    }

    void Update() {
        ProgressIndicator.set(
            0.0f, _constructionSite.getBuildPointsToConstruct(),
            _constructionSite.getCurrentBuildPoints()
        );

        WorkersAssignemntControl.set(
            _constructionSite.getMaxWorkersPossibleToAssign(),
            _constructionSite.getCurrenAssignedWorkersCount()
        );
    }

    //Fields
    private CarCityObject _carCity = null;
    private ConstructionSiteObject _constructionSite = null;
}
