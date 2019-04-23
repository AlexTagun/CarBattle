using UnityEngine;
using UnityEngine.UI;

public class WorkersAssignemntControlObject : MonoBehaviour
{
    public Button WithdrawWorkerButton = null;
    public Button AssignWorkerButton = null;
    public LimitedValueStripeIndicatorObject AssignementCountIndicator = null;

    public void set(int inMaxWorkersToAssign, int inAssignedWorkers) {
        AssignementCountIndicator.set(0.0f, inMaxWorkersToAssign, inAssignedWorkers);
    }

    private void Awake() {
        WithdrawWorkerButton.onClick.AddListener(()=> { onPressedWithdrawWorker.Invoke(); });
        AssignWorkerButton.onClick.AddListener(()=> { onPressedAssignWorker.Invoke(); });
    }

    public delegate void OnPressedWithdrawWorkerDelegate();
    public delegate void OnPressedAssignWorkerDelegate();

    public OnPressedWithdrawWorkerDelegate onPressedWithdrawWorker;
    public OnPressedAssignWorkerDelegate onPressedAssignWorker;
}
