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

            collectConstructionSiteWorkers(ref __arrayRegister);
            if (__arrayRegister.getSize() >=
                _constructionSite.getMaxWorkersPossibleToAssign()) return;

            theCrewMemberToAssign.setConstruction(_constructionSite);
        };

        WorkersAssignemntControl.onPressedWithdrawWorker += () => {
            collectConstructionSiteWorkers(ref __arrayRegister);
            if (0 == __arrayRegister.getSize()) return;

            __arrayRegister[0].setConstruction(null);
        };
    }

    void Update() {
        ProgressIndicator.set(
            0.0f, _constructionSite.getBuildPointsToConstruct(),
            _constructionSite.getCurrentBuildPoints()
        );

        collectConstructionSiteWorkers(ref __arrayRegister);
        WorkersAssignemntControl.set(
            _constructionSite.getMaxWorkersPossibleToAssign(),
            __arrayRegister.getSize()
        );
    }

    private void collectConstructionSiteWorkers(ref FastArray<CrewMember> outWorkers) {
        outWorkers.clear();
        _carCity.collectCrewMembers(ref outWorkers, (CrewMember inCrewMember) => {
            return _constructionSite == inCrewMember.getConstruction();
        });
    }

    //Fields
    private CarCityObject _carCity = null;
    private ConstructionSiteObject _constructionSite = null;

    private FastArray<CrewMember> __arrayRegister = new FastArray<CrewMember>();
}
