using System;
using Cysharp.Threading.Tasks;
using toio;

public static class ToioMotorUtility
{
    public static UniTask<Cube.TargetMoveRespondType> TargetMove(Cube cube,
        int targetX, int targetY, int targetAngle, int configID = 0, int timeOut = 0,
        Cube.TargetMoveType targetMoveType = Cube.TargetMoveType.RotatingMove, int maxSpd = 80,
        Cube.TargetSpeedType targetSpeedType = Cube.TargetSpeedType.UniformSpeed,
        Cube.TargetRotationType targetRotationType = Cube.TargetRotationType.AbsoluteLeastAngle,
        Cube.ORDER_TYPE order = Cube.ORDER_TYPE.Strong)
    {
        var source = new UniTaskCompletionSource<Cube.TargetMoveRespondType>();
        var callbackKey = Guid.NewGuid().ToString();
        cube.targetMoveCallback.RemoveListener(callbackKey);
        cube.targetMoveCallback.AddListener(callbackKey, (_cube, _configId, _respondType) =>
        {
            if (_configId == configID)
            {
                DelayedRemoveTargetMoveCallback(cube, callbackKey);
                source.TrySetResult(_respondType);
            }
        });

        cube.TargetMove(targetX, targetY, targetAngle, configID, timeOut,
            targetMoveType, maxSpd, targetSpeedType, targetRotationType, order);

        return source.Task;
    }

    private static async void DelayedRemoveTargetMoveCallback(Cube cube, string callbackKey)
    {
        await UniTask.NextFrame();
        cube.targetMoveCallback.RemoveListener(callbackKey);
    }
}
