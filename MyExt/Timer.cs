using System;

namespace TamrielTradeApp
{
    public class Timer
    {
        public readonly bool IsReactivateTimer;
        public readonly float MaxTimeValue;

        public Timer(float maxTimeValue = 0, bool isReactivateTimer = true)
        {
            IsReactivateTimer = isReactivateTimer;
            MaxTimeValue = maxTimeValue;
        }

        public bool IsTimerActive { get; private set; }
        public float CurrentTime { get; private set; }

        public event Action OnEndTime;


        #region Public Functions

        public void TimerSetActive(bool isActive, bool isReset = false)
        {
            IsTimerActive = isActive;
            if (isReset)
            {
                CurrentTime = MaxTimeValue;
            }
        }

        public string GetTimeString(ViewType viewType)
        {
            return viewType switch
            {
                ViewType.JustInt => ((int)CurrentTime).ToString(),
                ViewType.JustFloat => CurrentTime.ToString(),
                ViewType.Time => GetReadableTime((int)CurrentTime),
                _ => "",
            };
        }

        #endregion

        //public static string GetReadableTime(int seconds) => $"{seconds / 3600:00}:{seconds / 60 % 60:00}:{seconds % 60:00}";
        public static string GetReadableTime(int seconds)
        {
            if (seconds >= 3600)
            {
                return $"{seconds / 3600:00}:{seconds / 60 % 60:00}:{seconds % 60:00}";
            }
            else
            {
                return $"{seconds / 60 % 60:00}:{seconds % 60:00}";
            }
        }

        #region Update Functions

        public void UpdateFunc(float deltaTime)
        {
            if (!IsTimerActive)
            {
                return;
            }

            CurrentTime -= deltaTime;
            CurrentTime = Math.Clamp(CurrentTime, 0f, MaxTimeValue);

            if (CurrentTime <= 0f)
            {
                OnEndTime?.Invoke();
                TimerSetActive(IsReactivateTimer, IsReactivateTimer);
            }
        }

        #endregion

        public enum ViewType
        {
            JustInt,
            JustFloat,
            Time //TODO Add Type 00:00
        }
    }
}