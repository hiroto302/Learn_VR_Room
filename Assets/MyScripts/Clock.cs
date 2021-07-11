using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    DateTime _localTime;                   // 現地時間
    [SerializeField] int localSecond;      // 現地時間の秒
    int _beforeLocalSecond;                 // 回転する前の秒針が示す秒の時間
    int _secondHandTotalRotationAngle;      // 秒針が回転した角度の合計

    public Transform hourHand;   // 時針
    public Transform minuteHand; // 分針
    public Transform secondHand; // 秒針


    void Start()
    {
      _localTime = DateTime.Now;
      InitializeClock();
      _beforeLocalSecond = _localTime.Second;
      StartCoroutine(RotateClockHandRoutine());
    }

    // 時計の初期設定
    void InitializeClock()
    {
      SetHourHand(_localTime.Hour, _localTime.Minute);
      SetMinuteHand(_localTime.Minute);
      SetSecondHand(_localTime.Second);
    }

    // 時針を設定するメソッド
    void SetHourHand(int localHour, int localMinute)
    {
      if(localHour >= 12)
      {
        localHour -= 12;
      }

      // 回転させる角度
      float rotatedAngle = 30 * localHour +  0.5f * localMinute;
      // 回転
      hourHand.Rotate(rotatedAngle, 0, 0, Space.Self);
    }

    // 分針を設定するメソッド
    void SetMinuteHand(int localMinute)
    {
      float rotatedAngle = 6 * localMinute;
      minuteHand.Rotate(rotatedAngle, 0, 0, Space.Self);
    }

    // 秒針を設定するメソッド
    void SetSecondHand(int localSecond)
    {
      int rotatedAngle = 6 * localSecond;
      secondHand.Rotate(rotatedAngle, 0, 0, Space.Self);

      _secondHandTotalRotationAngle += rotatedAngle;
    }

    // 時計の針を現地時間に合わせて回転させていく処理
    // 秒針の時間経過を追いかけ他の時計のはりを動かしていく
    // １秒経過毎に６度回転させていく
    // 秒針が360度回転したら分針を６度回転させる。時針を0.5度回転させる。
    void RotateClockHand()
    {
      _localTime = DateTime.Now;
      // 秒を取得
      localSecond = _localTime.Second;
      // 秒が変化した時に行う処理
      if(localSecond != _beforeLocalSecond)
      {
        // 経過時間
        int elapsedTime = localSecond - _beforeLocalSecond;
        // beforeLocalTime の更新
        _beforeLocalSecond = localSecond;
        // 回転角度
        int rotatedAngle = elapsedTime * 6;
        // 回転
        secondHand.Rotate(rotatedAngle, 0, 0, Space.Self);
        // 秒針が回転している合計を計測
        _secondHandTotalRotationAngle += rotatedAngle;
        if(_secondHandTotalRotationAngle % 360 == 0)
        {
          minuteHand.Rotate(6.0f, 0, 0, Space.Self);
          hourHand.Rotate(0.5f, 0, 0, Space.Self);
        }
      }
    }

  IEnumerator RotateClockHandRoutine()
  {
    while(true)
    {
      RotateClockHand();
      yield return new WaitForSeconds(0.2f);
    }
  }
}
