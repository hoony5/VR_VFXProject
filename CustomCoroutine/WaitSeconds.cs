 using System;
 using UnityEngine;

 public class WaitSeconds : CustomYieldInstruction
 {
     // 1초의 틱 (고정값)
     readonly float TicksPerSecond = TimeSpan.TicksPerSecond;

     private float seconds;

     // x초 후의 틱
     long m_end;

     public WaitSeconds(float seconds)
     {
         // 생성 시점 틱(가변값) + x 초 후 틱 (고정값)
         this.seconds = seconds <= 0 ? 0.0001f : seconds;
         UpdateTimer();
     }

     public WaitSeconds WaitForSeconds(float seconds)
     {
         this.seconds = seconds <= 0 ? 0.0001f : seconds;
         UpdateTimer();
         return this;
     }
     // 대기조건

     public override bool keepWaiting => DateTime.Now.Ticks < m_end;

     private void UpdateTimer()
     {
         m_end = DateTime.Now.Ticks + (long)(TicksPerSecond * seconds);
     }
 }