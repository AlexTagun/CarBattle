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
            CrewMember theCrewMemberToAssign =
                XUtils.verify(_carCity).getFirstFreeCrewMember();
            if (null == theCrewMemberToAssign) return;

            XUtils.verify(_constructionSite).assignWorker(
                _carCity.startFreeCrewMemberAssignment(theCrewMemberToAssign)
            );
            theCrewMemberToAssign.setConstruction(_constructionSite);
        };

        WorkersAssignemntControl.onPressedWithdrawWorker += () => {
            CrewMember theCrewMemberToWithdraw =
                XUtils.verify(_constructionSite).getFirstWorker();
            if (null == theCrewMemberToWithdraw) return;

            XUtils.verify(_carCity).withdrawWorkerAsFree(
                _constructionSite.startWithdrawWorker(theCrewMemberToWithdraw)
            );
            theCrewMemberToWithdraw.setConstruction(null);
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
