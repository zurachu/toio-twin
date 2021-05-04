using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using toio;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Button connectButton;
    [SerializeField] private Text connectButtonText;
    [SerializeField] private Text timeSpanText;

    private int RemainingRequiredCubeCount => colors.Length - ToioCubeManagerService.Instance.CubeManager.cubes.Count;

    private static readonly Color[] colors = new Color[]
    {
        Color.magenta,
        Color.cyan,
    };

    private int phase;
    private DateTime[] caughtTimes = new DateTime[2];

    // Start is called before the first frame update
    void Start()
    {
        UpdateConnectButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (RemainingRequiredCubeCount > 0)
        {
            return;
        }

        var cubeManager = ToioCubeManagerService.Instance.CubeManager;

        switch (phase)
        {
            case 0:
                if (cubeManager.cubes.TrueForAll(_cube => _cube.isGrounded))
                {
                    phase++;
                }

                break;
            case 1:
                if (cubeManager.synced)
                {
                    var mv0 = cubeManager.navigators[0].Navi2Target(250, 100, maxSpd: 50).Exec();
                    var mv1 = cubeManager.navigators[1].Navi2Target(250, 400, maxSpd: 50).Exec();
                    if (mv0.reached && mv1.reached)
                    {
                        StartGame(cubeManager.cubes);
                        phase++;
                    }
                }

                break;
        }
    }

    private async void StartGame(List<Cube> cubes)
    {
        await UniTask.WhenAll(
            ToioMotorUtility.TargetMove(cubes[0], 250, 100, 0),
            ToioMotorUtility.TargetMove(cubes[1], 250, 400, 180));
        phase++;
        cubes[0].Move(115, 101, 0);
        cubes[1].Move(115, 101, 0);
    }

    public async void OnClickConnect()
    {
        connectButton.interactable = false;

        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        var cube = await cubeManager.SingleConnect();
        if (cube != null)
        {
            var index = cubeManager.cubes.Count - 1;
            cube.idMissedCallback.AddListener("toio-twin", (_cube) => { OnPositionIdMissed(_cube, index); });
            ToioLedUtility.TurnLedOn(cube, colors[index], 0);
        }

        UpdateConnectButton();
        connectButton.interactable = true;
    }

    private void OnPositionIdMissed(Cube cube, int index)
    {
        caughtTimes[index] = DateTime.Now;

        cube.Move(0, 0, 0, Cube.ORDER_TYPE.Strong);

        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        if (/*phase == 3 &&*/ cubeManager.cubes.TrueForAll(_cube => !_cube.isGrounded))
        {
            var timeSpan = caughtTimes[0] - caughtTimes[1];
            UIUtility.TrySetText(timeSpanText, $"{timeSpan}");

            if (Mathf.Abs((float)timeSpan.TotalSeconds) < 0.1f)
            {
                cubeManager.cubes.ForEach(_cube => _cube.PlayPresetSound(ToioSoundUtility.PresetSoundId.Get1, order: Cube.ORDER_TYPE.Weak));
            }
        }

        phase = 0;
    }

    private void UpdateConnectButton()
    {
        UIUtility.TrySetActive(connectButton, RemainingRequiredCubeCount > 0);
        UIUtility.TrySetText(connectButtonText, $"キューブに接続する（残り{RemainingRequiredCubeCount}台）");
    }
}
